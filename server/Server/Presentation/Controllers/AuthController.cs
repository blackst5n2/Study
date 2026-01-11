using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Server.Application.Services;
using Server.Infrastructure.Providers;
using Server.Application.DTOs;

namespace Server.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _jwtSecret;
        private readonly RedisManager _redis;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthController(RedisManager redis, RefreshTokenService refreshTokenService)
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(_jwtSecret))
                throw new InvalidOperationException("JWT_SECRET 환경변수가 설정되어 있지 않습니다.");
            _redis = redis;
            _refreshTokenService = refreshTokenService;
        }

        public class AdminLoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("admin-login")]
        public IActionResult AdminLogin([FromBody] AdminLoginRequest req)
        {
            var adminUser = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";
            var adminPw = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "adminpw";
            if (req.Username == adminUser && req.Password == adminPw)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, req.Username),
                    new Claim(ClaimTypes.Role, "admin")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(4),
                    signingCredentials: creds);
                // 관리자 로그인은 온라인 유저 처리 제외
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }


        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "토큰 없음" });
            var token = authHeader.Substring("Bearer ".Length);
            // 1. 블랙리스트 체크
            if (await _redis.IsBlacklistedAsync(token))
                return Unauthorized(new { message = "블랙리스트 토큰" });
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "AUTH_SERVER",
                    ValidateAudience = true,
                    ValidAudience = "GAME_SERVER",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)) { KeyId = "main-key" }
                };
                var principal = tokenHandler.ValidateToken(token, validationParams, out var validatedToken);
                // 2. 권한/역할 체크
                var role = principal.FindFirst("role")?.Value ?? "user";
                if (role != "user" && role != "admin")
                    return Forbid("권한 없음");
                return Ok(new
                {
                    message = "토큰 인증 성공",
                    user = new
                    {
                        email = principal.FindFirst("email")?.Value,
                        nickname = principal.FindFirst("nickname")?.Value,
                        role,
                        userId = principal.FindFirst("userId")?.Value
                    }
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new { message = "토큰 만료" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "토큰 인증 실패", error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest req)
        {
            var accountId = await _refreshTokenService.GetAccountIdByRefreshTokenAsync(req.RefreshToken);
            if (accountId == null)
                return Unauthorized(new { message = "유효하지 않은 리프레시 토큰" });
            if (await _redis.IsBlacklistedAsync(req.RefreshToken))
                return Unauthorized(new { message = "블랙리스트 토큰" });

            // TODO: accountId로 DB에서 계정 정보 조회
            var accountNickname = "nickname_placeholder";
            var authProvider = "provider_placeholder";
            var authEmail = "email_placeholder";
            // account 정보 조회 로직을 실제로 구현하세요.

            // 새 Access/Refresh Token 발급
            var claims = new List<Claim>
            {
                new Claim("accountId", accountId.Value.ToString()),
                new Claim("nickname", accountNickname ?? string.Empty),
                new Claim("provider", authProvider ?? string.Empty),
                new Claim("email", authEmail ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, accountId.Value.ToString())
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = "AUTH_SERVER",
                Audience = "GAME_SERVER",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            var newRefreshToken = await _refreshTokenService.GenerateAndStoreRefreshTokenAsync(accountId.Value);
            await _redis.SetOnlineAsync(accountId.Value); // 로그인 시 온라인 등록
            await _redis.BlacklistTokenAsync(req.RefreshToken, TimeSpan.FromDays(14));
            return Ok(new TokenResponse { AccessToken = accessToken, RefreshToken = newRefreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        {
            // userId 추출 (JWT에서)
            string userId = null;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(req.AccessToken);
                userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "userId")?.Value;
            }
            catch { }
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var guid))
            {
                await _redis.RemoveOnlineAsync(guid);
            }
            await _redis.BlacklistTokenAsync(req.AccessToken, TimeSpan.FromMinutes(30));
            await _redis.BlacklistTokenAsync(req.RefreshToken, TimeSpan.FromDays(14));
            await _refreshTokenService.InvalidateRefreshTokenAsync(req.RefreshToken);
            return Ok(new { message = "로그아웃 완료" });
        }

    } // End of AuthController

} // End of namespace