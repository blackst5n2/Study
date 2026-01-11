using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Services;

namespace Server.Presentation.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/admin/stats")]
    public class AdminStatsController : ControllerBase
    {
        private readonly SessionService _sessionService;
        public AdminStatsController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var totalUsers = await _sessionService.CountOnlineUsersAsync();
            return Ok(new { totalUsers });
        }
    }
}
