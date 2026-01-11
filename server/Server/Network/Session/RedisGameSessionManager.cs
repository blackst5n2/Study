using Server.Core.Entities.Progress;
using StackExchange.Redis;
using System.Text.Json;

namespace Server.Network.Session
{
    public class RedisGameSessionManager : IGameSessionManager
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private const string SESSION_KEY_PREFIX = "game:session:";
        private const int SESSION_EXPIRY_MINUTES = 30;

        public RedisGameSessionManager(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public IConnectionMultiplexer Connection => _redis;

        public async Task<GameSession> CreateSessionAsync(Player player)
        {
            var sessionToken = GenerateSessionToken();
            var session = new GameSession(sessionToken, player);
            
            await _db.StringSetAsync(
                GetSessionKey(sessionToken),
                JsonSerializer.Serialize(session),
                TimeSpan.FromMinutes(SESSION_EXPIRY_MINUTES)
            );
            
            return session;
        }

        public async Task<GameSession?> GetSessionAsync(string sessionToken)
        {
            var sessionData = await _db.StringGetAsync(GetSessionKey(sessionToken));
            if (!sessionData.HasValue) return null;
            
            return JsonSerializer.Deserialize<GameSession>(sessionData!);
        }

        public async Task<bool> ValidateSessionAsync(string sessionToken)
        {
            var session = await GetSessionAsync(sessionToken);
            if (session == null) return false;
            
            await UpdateSessionActivityAsync(sessionToken);
            return true;
        }

        public async Task RemoveSessionAsync(string sessionToken)
        {
            await _db.KeyDeleteAsync(GetSessionKey(sessionToken));
        }

        public async Task UpdateSessionActivityAsync(string sessionToken)
        {
            var session = await GetSessionAsync(sessionToken);
            if (session == null) return;
            
            session.UpdateActivity();
            await _db.StringSetAsync(
                GetSessionKey(sessionToken),
                JsonSerializer.Serialize(session),
                TimeSpan.FromMinutes(SESSION_EXPIRY_MINUTES)
            );
        }

        public async Task<IEnumerable<GameSession>> GetAllActiveSessions()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var sessions = new List<GameSession>();
            
            var keys = server.Keys(pattern: $"{SESSION_KEY_PREFIX}*");
            foreach (var key in keys)
            {
                var sessionData = await _db.StringGetAsync(key);
                if (sessionData.HasValue)
                {
                    var session = JsonSerializer.Deserialize<GameSession>(sessionData!);
                    if (session != null) sessions.Add(session);
                }
            }
            
            return sessions;
        }

        private string GenerateSessionToken() => 
            Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");

        private string GetSessionKey(string sessionToken) => 
            $"{SESSION_KEY_PREFIX}{sessionToken}";
    }
}