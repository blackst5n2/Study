using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server.Middleware
{
    /// <summary>
    /// 모든 HTTP 요청/응답을 로그로 남기는 미들웨어
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation($"[Request] {context.Request.Method} {context.Request.Path}");
            await _next(context);
            sw.Stop();
            _logger.LogInformation($"[Response] {context.Response.StatusCode} ({sw.ElapsedMilliseconds}ms)");
        }
    }
}
