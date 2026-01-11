using System.Net;
using System.Text.Json;

namespace Server.Middleware
{
    /// <summary>
    /// 모든 예외를 잡아 일관된 JSON 에러 응답과 로그를 남기는 글로벌 예외 처리 미들웨어
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GlobalException] {Message}", ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var error = new { message = "서버 내부 오류가 발생했습니다.", detail = ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(error));
            }
        }
    }
}
