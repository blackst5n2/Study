using Microsoft.Extensions.Caching.Memory;

namespace Server.Middleware
{
    /// <summary>
    /// 간단한 IP 기반 속도 제한 미들웨어 (초당 최대 N회 요청 허용)
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private const int LIMIT = 10; // 초당 허용 요청 수
        private const int WINDOW_SECONDS = 1;

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = $"rl:{ip}";
            var now = DateTimeOffset.UtcNow;
            var entry = _cache.GetOrCreate(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(WINDOW_SECONDS);
                return (Count: 0, WindowStart: now);
            });
            if (entry.Count >= LIMIT)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync($"Rate limit exceeded. Try again later.");
                return;
            }
            _cache.Set(key, (entry.Count + 1, entry.WindowStart), TimeSpan.FromSeconds(WINDOW_SECONDS));
            await _next(context);
        }
    }
}
