using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Server.Middleware
{
    /// <summary>
    /// 주요 보안 HTTP 헤더를 자동으로 추가하는 미들웨어 (XSS, 클릭재킹 등 예방)
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["Referrer-Policy"] = "no-referrer";
                context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
