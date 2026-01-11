using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Server.Network.Session;
using GameProto;
using Server.Core.Entities.Progress;

namespace Server.Network
{
    public class UdpServer
    {
        private readonly int _port;
        private readonly IGameSessionManager _sessionManager;
        private readonly ILogger<UdpServer> _logger;
        private UdpClient? _udpClient;
        private bool _isRunning;
        private readonly CancellationTokenSource _cts;
        private readonly Dictionary<string, IPEndPoint> _sessionEndpoints;

        public UdpServer(
            int port,
            IGameSessionManager sessionManager,
            ILogger<UdpServer> logger)
        {
            _port = port;
            _sessionManager = sessionManager;
            _logger = logger;
            _cts = new CancellationTokenSource();
            _sessionEndpoints = new Dictionary<string, IPEndPoint>();
        }

        public async Task StartAsync()
        {
            try
            {
                _udpClient = new UdpClient(_port);
                _isRunning = true;
                _logger.LogInformation($"UDP Server started on port {_port}");

                while (_isRunning && !_cts.Token.IsCancellationRequested)
                {
                    var result = await _udpClient.ReceiveAsync(_cts.Token);
                    _ = ProcessPacketAsync(result);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("UDP Server shutting down...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UDP server");
            }
        }

        public async Task StopAsync()
        {
            _isRunning = false;
            _cts.Cancel();
            _udpClient?.Close();
            await Task.CompletedTask;
        }

        private async Task ProcessPacketAsync(UdpReceiveResult result)
        {
            try
            {
                var packet = GamePacket.Parser.ParseFrom(result.Buffer);
                
                // 세션 검증
                var session = await _sessionManager.GetSessionAsync(packet.SessionToken);
                if (session == null)
                {
                    _logger.LogWarning($"Invalid session token from {result.RemoteEndPoint}");
                    return;
                }

                // UDP 엔드포인트 업데이트
                UpdateSessionEndpoint(packet.SessionToken, result.RemoteEndPoint);
                session.UdpEndPoint = result.RemoteEndPoint;

                switch (packet.Type)
                {
                    case PacketType.PlayerMove:
                        await HandlePlayerMoveAsync(session, packet);
                        break;
                    case PacketType.PlayerAction:
                        await HandlePlayerActionAsync(session, packet);
                        break;
                    default:
                        _logger.LogWarning($"Unexpected packet type for UDP: {packet.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing UDP packet from {result.RemoteEndPoint}");
            }
        }

        private async Task HandlePlayerMoveAsync(GameSession session, GamePacket packet)
        {
            try
            {
                var moveRequest = MoveRequest.Parser.ParseFrom(packet.Payload);
                
                // 이동 검증 로직 (필요한 경우)
                var response = new MoveResponse
                {
                    PlayerId = session.Player.Id.ToString(),
                    Position = moveRequest.Position,
                    Speed = moveRequest.Speed,
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                // 주변 플레이어들에게 브로드캐스트
                await BroadcastToNearbyPlayersAsync(session, PacketType.PlayerMove, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling player move");
            }
        }

        private async Task HandlePlayerActionAsync(GameSession session, GamePacket packet)
        {
            try
            {
                // TODO: 플레이어 액션 처리
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling player action");
            }
        }

        private void UpdateSessionEndpoint(string sessionToken, IPEndPoint endpoint)
        {
            _sessionEndpoints[sessionToken] = endpoint;
        }

        private async Task SendToClientAsync(IPEndPoint endpoint, PacketType type, IMessage message)
        {
            try
            {
                var packet = new GamePacket
                {
                    Type = type,
                    Payload = message.ToByteString(),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                var data = packet.ToByteArray();
                await _udpClient!.SendAsync(data, data.Length, endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending UDP packet to {endpoint}");
            }
        }

        private async Task BroadcastToNearbyPlayersAsync(GameSession sourceSession, PacketType type, IMessage message)
        {
            try
            {
                var activeSessions = await _sessionManager.GetAllActiveSessions();
                var nearbyPlayers = activeSessions.Where(s => 
                    s.SessionToken != sourceSession.SessionToken && 
                    IsPlayerNearby(sourceSession.Player, s.Player));

                var packet = new GamePacket
                {
                    Type = type,
                    Payload = message.ToByteString(),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                var data = packet.ToByteArray();
                foreach (var session in nearbyPlayers)
                {
                    if (_sessionEndpoints.TryGetValue(session.SessionToken, out var endpoint))
                    {
                        await _udpClient!.SendAsync(data, data.Length, endpoint);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting to nearby players");
            }
        }

        private bool IsPlayerNearby(Player source, Player target)
        {
            // TODO: 실제 거리 계산 로직 구현
            return true; // 임시로 모든 플레이어를 근처로 간주
        }
    }
}
