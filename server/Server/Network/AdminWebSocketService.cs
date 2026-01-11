using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Server.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Network
{
    public class AdminWebSocketService : IAdminWebSocketService
    {
        private readonly ConcurrentBag<WebSocket> _adminSockets = new ConcurrentBag<WebSocket>();
        private readonly PlayerService _playerService;
        private readonly SessionService _sessionService;
        private readonly System.Timers.Timer _statsTimer;

        public AdminWebSocketService(PlayerService playerService, SessionService sessionService)
        {
            _playerService = playerService;
            _sessionService = sessionService;
            _statsTimer = new System.Timers.Timer(5000); // 5초마다
            _statsTimer.Elapsed += async (s, e) => await BroadcastStatsAsync();
            _statsTimer.AutoReset = true;
            _statsTimer.Start();
        }

        private async Task BroadcastStatsAsync()
        {
            //var totalUsers = await _playerService.CountAllAsync();
            //var newUsersToday = await _playerService.CountNewUsersTodayAsync();
            var onlineUsers = await _sessionService.CountOnlineUsersAsync();
            var serverStatus = "정상";
            var statsMsg = System.Text.Json.JsonSerializer.Serialize(new {
                type = "stats_update",
                stats = new {
                    //totalUsers,
                    //newUsersToday,
                    onlineUsers,
                    serverStatus
                }
            });
            await BroadcastAsync(statsMsg);
        }

        public void Register(WebSocket socket)
        {
            _adminSockets.Add(socket);
        }

        public async Task BroadcastAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(bytes);
            foreach (var socket in _adminSockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
