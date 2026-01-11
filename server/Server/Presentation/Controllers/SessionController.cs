using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Entities;
using Server.Infrastructure.Providers;
using System.Security.Claims;

namespace Server.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly RedisManager _redis;
        public SessionController(RedisManager redis)
        {
            _redis = redis;
        }

        /// <summary>
        /// 로그인 시 호출: 온라인 상태 등록
        /// </summary>
        [HttpPost("online")]
        [Authorize]
        public async Task<IActionResult> SetOnline()
        {
            var accountIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountIdStr)) return Unauthorized();

            if (!Guid.TryParse(accountIdStr, out var accountId))
                return Unauthorized();

            await _redis.SetOnlineAsync(accountId);
            return Ok(new { success = true });
        }

        /// <summary>
        /// 로그아웃 시 호출: 온라인 상태 제거
        /// </summary>
        [HttpPost("offline")]
        [Authorize]
        public async Task<IActionResult> SetOffline()
        {
            var accountIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountIdStr)) return Unauthorized();

            if (!Guid.TryParse(accountIdStr, out var accountId))
                return Unauthorized();

            await _redis.RemoveOnlineAsync(accountId);
            return Ok(new { success = true });
        }

        /// <summary>
        /// Heartbeat: TTL 갱신
        /// </summary>
        [HttpPost("heartbeat")]
        [Authorize]
        public async Task<IActionResult> Heartbeat()
        {
            var accountIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountIdStr)) return Unauthorized();

            if (!Guid.TryParse(accountIdStr, out var accountId))
                return Unauthorized();

            await _redis.SetOnlineAsync(accountId);
            return Ok(new { success = true });
        }
    }
}
