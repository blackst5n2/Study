using System.Net.WebSockets;
using System.Text;
using Server.Utils;
using Server.Application.Services;

namespace Server.Network
{
    public static class AdminWebSocketHandler
    {
       public static async Task Handle(HttpContext context, WebSocket webSocket, IAdminWebSocketService wsService, AccountService accountService, SessionService sessionService, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            try
            {
                // 1. 첫 메시지로 JWT 인증 처리
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Missing JWT", CancellationToken.None);
                    return;
                }
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var authMsg = System.Text.Json.JsonSerializer.Deserialize<AuthCommand>(message);
                if (authMsg?.Type != "auth" || string.IsNullOrEmpty(authMsg.Token))
                {
                    Logger.Error("[WebSocket] 인증 메시지 누락");
                    await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Missing JWT", CancellationToken.None);
                    return;
                }
                // 2. JWT 검증 및 권한 체크
                var principal = JwtUtil.ValidateToken(authMsg.Token);
                if (principal == null || !IsAdmin(principal))
                {
                    Logger.Error("[WebSocket] JWT 검증 실패 또는 관리자 권한 없음");
                    await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid or Not Admin", CancellationToken.None);
                    return;
                }
                Logger.Info("[WebSocket] 인증 성공, 명령 루프 진입");
                wsService.Register(webSocket);

                // 3. 실제 통계 정보 즉시 전송
                await SendStatsAsync(wsService, accountService, sessionService);

                // 4. 명령 루프
                await CommandLoopAsync(webSocket, wsService, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.Error($"[WebSocket] 처리 예외: {ex.Message}");
            }
            finally
            {
                if (webSocket != null)
                {
                    try
                    {
                        if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down", CancellationToken.None);
                        }
                    }
                    catch {}
                    webSocket.Dispose();
                }
            }
        }

        private static async Task SendStatsAsync(IAdminWebSocketService wsService, AccountService accountService, SessionService sessionService)
        {
            var totalUsers = await accountService.CountAllAsync();
            var newUsersToday = await accountService.CountNewUsersTodayAsync();
            var onlineUsers = await sessionService.CountOnlineUsersAsync();
            var serverStatus = "정상";
            var statsMsg = System.Text.Json.JsonSerializer.Serialize(new {
                type = "stats_update",
                stats = new {
                    totalUsers,
                    newUsersToday,
                    onlineUsers,
                    serverStatus
                }
            });
            await wsService.BroadcastAsync(statsMsg);
        }

        private static async Task CommandLoopAsync(WebSocket webSocket, IAdminWebSocketService wsService, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var msgText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var cmd = System.Text.Json.JsonSerializer.Deserialize<AdminCommand>(msgText);
                        if (cmd == null || string.IsNullOrWhiteSpace(cmd.Type))
                        {
                            Logger.Error("[WebSocket] 잘못된 명령: type 누락 또는 파싱 실패");
                            continue;
                        }
                        switch (cmd.Type)
                        {
                            case "kick":
                                if (string.IsNullOrWhiteSpace(cmd.TargetUserId))
                                {
                                    Logger.Error("[WebSocket] kick 명령: targetUserId 누락");
                                    break;
                                }
                                Logger.Info($"[WebSocket] kick 명령: targetUserId={cmd.TargetUserId}");
                                // TODO: 유저 강제 퇴장 처리
                                break;
                            case "broadcast_notice":
                                if (string.IsNullOrWhiteSpace(cmd.Message))
                                {
                                    Logger.Error("[WebSocket] broadcast_notice 명령: message 누락");
                                    break;
                                }
                                Logger.Info($"[WebSocket] broadcast_notice 명령: {cmd.Message}");
                                // TODO: 전체 공지 처리
                                break;
                            default:
                                Logger.Error($"[WebSocket] 알 수 없는 명령: {cmd.Type}");
                                break;
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Logger.Info("[WebSocket] 클라이언트가 연결 종료 요청");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"[WebSocket] 명령 루프 예외: {ex.Message}");
                    break;
                }
            }
            Logger.Info("[WebSocket] 명령 루프 종료");
        }

        private static bool IsAdmin(System.Security.Claims.ClaimsPrincipal principal)
        {
            var roleClaims = principal.Claims
                .Where(c => c.Type == "role" || c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value.ToLower())
                .ToList();
            Logger.Info($"[WebSocket] role 클레임: {string.Join(",", roleClaims)}");
            return roleClaims.Contains("admin");
        }
    }
}
   