using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Server.Utils
{
    public static class JwtUtil
    {
        public static ClaimsPrincipal? ValidateToken(string token)
        {
            string? secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("[JWT] 시크릿 환경변수(JWT_SECRET)가 없습니다.");
                return null;
            }
            var key = System.Text.Encoding.UTF8.GetBytes(secret);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                }, out _);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JWT] 검증 예외: {ex.Message}");
                return null;
            }
        }
    }
}
