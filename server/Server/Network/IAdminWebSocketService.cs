using System.Threading.Tasks;

namespace Server.Network
{
    public interface IAdminWebSocketService
    {
        Task BroadcastAsync(string message);
        void Register(System.Net.WebSockets.WebSocket socket); // 추가
        // 필요시: Task SendToAsync(string adminId, string message);
    }
}
