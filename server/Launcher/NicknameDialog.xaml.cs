using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Launcher
{
    public partial class NicknameDialog : Window
    {
        public string Nickname { get; private set; }
        private readonly string tempToken;
        public string ResultToken { get; private set; }
        public string ResultNickname { get; private set; }

        public NicknameDialog(string tempToken)
        {
            InitializeComponent();
            this.tempToken = tempToken;
        }

        private async void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            string nickname = txtNickname.Text.Trim();
            if (string.IsNullOrWhiteSpace(nickname))
            {
                txtStatus.Text = "닉네임을 입력하세요.";
                return;
            }
            txtStatus.Text = "닉네임 확인 중...";
            bool success = await TrySetNicknameAsync(nickname);
            if (success)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async Task<bool> TrySetNicknameAsync(string nickname)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tempToken);
                var content = new StringContent(JsonConvert.SerializeObject(new { nickname }), Encoding.UTF8, "application/json");
                try
                {
                    var resp = await client.PostAsync("http://localhost:4000/api/auth/set-nickname", content);
                    var respStr = await resp.Content.ReadAsStringAsync();
                    if (resp.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(respStr);
                        ResultToken = result.token;
                        ResultNickname = result.nickname;
                        return true;
                    }
                    else
                    {
                        dynamic result = JsonConvert.DeserializeObject(respStr);
                        txtStatus.Text = result.message != null ? (string)result.message : "닉네임 등록 실패";
                        return false;
                    }
                }
                catch
                {
                    txtStatus.Text = "서버 연결 오류";
                    return false;
                }
            }
        }
    }
}
