using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using System;
using Server.Data.Providers;

namespace Server.Tests
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _gameServerClient;
        private readonly HttpClient _authServerClient;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _gameServerClient = factory.CreateClient();
            // 인증 서버 주소는 실제 환경에 맞게 수정 (포트/호스트 확인)
            _authServerClient = new HttpClient { BaseAddress = new System.Uri("http://localhost:4000") };
        }

        [Fact(DisplayName = "인증 서버-게임 서버 연동 토큰 검증 E2E")]
        public async Task AuthServer_GameServer_Token_E2E()
        {
            // 1. 인증 서버에 로그인 요청
            var loginReq = new {
                email = "testuser@example.com",
                password = "Test1234!"
            };
            var loginContent = new StringContent(JsonConvert.SerializeObject(loginReq), Encoding.UTF8, "application/json");
            var loginRes = await _authServerClient.PostAsync("/api/auth/login", loginContent);
            var loginBody = await loginRes.Content.ReadAsStringAsync();
            dynamic loginData = JsonConvert.DeserializeObject(loginBody);

            Assert.True(loginRes.IsSuccessStatusCode, loginBody);
            Assert.NotNull(loginData.token);

            // 2. 게임 서버에 토큰을 Authorization 헤더로 전달하여 검증
            _gameServerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)loginData.token);
            var verifyRes = await _gameServerClient.PostAsync("/api/auth/verify-token", null);
            var verifyBody = await verifyRes.Content.ReadAsStringAsync();

            Assert.True(verifyRes.IsSuccessStatusCode, verifyBody);
            Assert.Contains("토큰 인증 성공", verifyBody);
        }

        [Fact(DisplayName = "토큰 검증 실패 (만료/위조/블랙리스트)")]
        public async Task VerifyToken_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange: 만료/위조/블랙리스트 토큰
            var token = "invalid.jwt.token";
            _gameServerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _gameServerClient.PostAsync("/api/auth/verify-token", null);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("토큰", content); // 에러 메시지 내 토큰 관련 문구 포함
        }
    }
}
