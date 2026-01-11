using Server.Core.Entities.Progress;

namespace Server.Network.Session
{
    public interface IGameSessionManager
    {
        Task<GameSession> CreateSessionAsync(Player player);
        Task<GameSession?> GetSessionAsync(string sessionToken);
        Task<bool> ValidateSessionAsync(string sessionToken);
        Task RemoveSessionAsync(string sessionToken);
        Task UpdateSessionActivityAsync(string sessionToken);
        Task<IEnumerable<GameSession>> GetAllActiveSessions();
    }
}