using System.Net;
using Server.Core.Entities.Progress;

namespace Server.Network.Session
{
    public class GameSession
    {
        public string SessionToken { get; }
        public Player Player { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastActivityAt { get; private set; }
        public IPEndPoint? TcpEndPoint { get; set; }
        public IPEndPoint? UdpEndPoint { get; set; }
        
        public GameSession(string sessionToken, Player player)
        {
            SessionToken = sessionToken;
            Player = player;
            CreatedAt = DateTime.UtcNow;
            LastActivityAt = DateTime.UtcNow;
        }

        public void UpdateActivity()
        {
            LastActivityAt = DateTime.UtcNow;
        }
    }
}