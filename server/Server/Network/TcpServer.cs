using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Server.Network.Session;
using GameProto;
using Server.Core.Entities.Progress;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

namespace Server.Network
{
    public class TcpServer
    {
        private readonly int _port;
        private readonly IGameSessionManager _sessionManager;
        private readonly ILogger<TcpServer> _logger;
        private TcpListener? _listener;
        private bool _isRunning;
        private readonly CancellationTokenSource _cts;

        public TcpServer(
            int port,
            IGameSessionManager sessionManager,
            ILogger<TcpServer> logger)
        {
            _port = port;
            _sessionManager = sessionManager;
            _logger = logger;
            _cts = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _isRunning = true;
                _logger.LogInformation($"TCP Server started on port {_port}");

                while (_isRunning && !_cts.Token.IsCancellationRequested)
                {
                    var client = await _listener.AcceptTcpClientAsync(_cts.Token);
                    _ = HandleClientAsync(client);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("TCP Server shutting down...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TCP server");
            }
        }

        public async Task StopAsync()
        {
            _isRunning = false;
            _cts.Cancel();
            _listener?.Stop();
            await Task.CompletedTask;
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            GameSession? session = null;
            try
            {
                using var stream = client.GetStream();
                
                // 핸드셰이크 처리
                var handshakeResult = await HandleHandshakeAsync(stream);
                if (!handshakeResult.Success)
                {
                    await SendErrorAsync(stream, "Handshake failed: " + handshakeResult.ErrorMessage);
                    return;
                }
                
                session = handshakeResult.Session;
                session.TcpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                _logger.LogInformation($"Client authenticated: {session.Player.Id}");

                // 메시지 루프
                while (_isRunning && client.Connected && !_cts.Token.IsCancellationRequested)
                {
                    var packet = await ReceivePacketAsync(stream);
                    if (packet == null) break;

                    await ProcessPacketAsync(session, packet, stream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling client connection");
            }
            finally
            {
                if (session != null)
                {
                    await _sessionManager.UpdateSessionActivityAsync(session.SessionToken);
                }
                client.Close();
                _logger.LogInformation("Client disconnected");
            }
        }

        private async Task<(bool Success, string ErrorMessage, GameSession? Session)> HandleHandshakeAsync(NetworkStream stream)
        {
            try
            {
                var packet = await ReceivePacketAsync(stream);
                if (packet?.Type != PacketType.Handshake)
                {
                    return (false, "Expected handshake packet", null);
                }

                var handshakeRequest = HandshakeRequest.Parser.ParseFrom(packet.Payload);

                if (string.IsNullOrEmpty(handshakeRequest.JwtToken)) {
                    return (false, "Jwt is missing", null);
                }

                ClaimsPrincipal principal;
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var parameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "WIW_AUTH_SERVER",
                        ValidAudience = "WIW_GAME_SERVER",
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("WIW_JWT_SECRET") ?? ""))
                    };
                    principal = handler.ValidateToken(handshakeRequest.JwtToken, parameters, out var securityToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"[AUTH FAIL] JWT validation failed: {ex.Message}");
                    return (false, "JWT validation failed: " + ex.Message, null);
                }

                var nickname = principal.FindFirst("nickname")?.Value;
                var accountId = principal.FindFirst("accountId")?.Value;
                _logger.LogInformation($"[AUTH SUCCESS] accountId={accountId}, nickname={nickname}");
                var player = new Player()
                {
                    Id = !string.IsNullOrEmpty(accountId) ? Guid.Parse(accountId) : Guid.NewGuid(),
                    Nickname = nickname ?? "Uknown"
                };
                var session = await _sessionManager.CreateSessionAsync(player);

                var response = new HandshakeResponse
                {
                    Success = true,
                    SessionToken = session.SessionToken,
                    AccountData = new AccountData
                    {
                        Id = accountId,
                        Nickname = nickname ?? "Unknown"
                    }
                };

                await SendPacketAsync(stream, PacketType.HandshakeAck, response);
                return (true, string.Empty, session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Handshake failed");
                return (false, ex.Message, null);
            }
        }

        private async Task ProcessPacketAsync(GameSession session, GamePacket packet, NetworkStream stream)
        {
            try
            {
                switch (packet.Type)
                {
                    case PacketType.Heartbeat:
                        await HandleHeartbeatAsync(session, stream);
                        break;
                    case PacketType.PlayerMove:
                        await HandlePlayerMoveAsync(session, packet, stream);
                        break;
                    case PacketType.ChatMessage:
                        await HandleChatMessageAsync(session, packet, stream);
                        break;
                    default:
                        _logger.LogWarning($"Unknown packet type: {packet.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing packet type {packet.Type}");
                await SendErrorAsync(stream, "Error processing packet");
            }
        }

        private async Task HandleHeartbeatAsync(GameSession session, NetworkStream stream)
        {
            await _sessionManager.UpdateSessionActivityAsync(session.SessionToken);
            await SendPacketAsync(stream, PacketType.Heartbeat, new GamePacket());
        }

        private async Task HandlePlayerMoveAsync(GameSession session, GamePacket packet, NetworkStream stream)
        {
            var moveRequest = MoveRequest.Parser.ParseFrom(packet.Payload);
            // TODO: 이동 검증 및 처리
            var response = new MoveResponse
            {
                PlayerId = session.Player.Id.ToString(),
                Position = moveRequest.Position,
                Speed = moveRequest.Speed,
                ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            await SendPacketAsync(stream, PacketType.PlayerMove, response);
        }

        private async Task HandleChatMessageAsync(GameSession session, GamePacket packet, NetworkStream stream)
        {
            var chatMessage = ChatMessage.Parser.ParseFrom(packet.Payload);
            // TODO: 채팅 메시지 처리 및 브로드캐스트
        }

        private async Task SendErrorAsync(NetworkStream stream, string message)
        {
            var errorMessage = new ErrorMessage { Message = message };
            await SendPacketAsync(stream, PacketType.Error, errorMessage);
        }

        private async Task<GamePacket?> ReceivePacketAsync(NetworkStream stream)
        {
            try
            {
                var lengthBuffer = new byte[4];
                if (await ReadExactAsync(stream, lengthBuffer, 0, 4) != 4)
                {
                    return null;
                }

                var length = BitConverter.ToInt32(lengthBuffer, 0);
                var buffer = new byte[length];
                if (await ReadExactAsync(stream, buffer, 0, length) != length)
                {
                    return null;
                }

                return GamePacket.Parser.ParseFrom(buffer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving packet");
                return null;
            }
        }

        private async Task SendPacketAsync(NetworkStream stream, PacketType type, IMessage message)
        {
            try
            {
                var packet = new GamePacket
                {
                    Type = type,
                    Payload = message.ToByteString(),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                var packetData = packet.ToByteArray();
                var lengthBytes = BitConverter.GetBytes(packetData.Length);
                await stream.WriteAsync(lengthBytes);
                await stream.WriteAsync(packetData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending packet");
                throw;
            }
        }

        private async Task<int> ReadExactAsync(NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                int bytesRead = await stream.ReadAsync(buffer.AsMemory(offset + totalBytesRead, count - totalBytesRead));
                if (bytesRead == 0) return totalBytesRead;
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
