using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Launcher
{
    public class AuthManager
    {
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:4000/") };

        public async Task<string> LoginAsync(string email, string password)
        {
            var loginData = new { email, password };
            var response = await httpClient.PostAsJsonAsync("api/auth/login", loginData);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            if (string.IsNullOrWhiteSpace(result?.token))
                throw new Exception("토큰 없음");
            return result.token;
        }

        public class LoginResult { public string token { get; set; } public string refreshToken { get; set; } }
    }
}
