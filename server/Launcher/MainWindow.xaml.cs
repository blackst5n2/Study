using System;
using System.Threading.Tasks;
using System.Windows;
using Launcher;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthManager authManager = new AuthManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string? loginToken;
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtLoginStatus.Text = "";
            btnLogin.IsEnabled = false;
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    txtLoginStatus.Text = "이메일과 비밀번호를 입력하세요.";
                    
                    return;
                }
                txtLoginStatus.Text = "서버에 로그인 요청 중...";
                string token = await authManager.LoginAsync(txtEmail.Text, txtPassword.Password);
                txtLoginStatus.Text = "로그인 성공!";
                loginToken = token; // token 저장

                // 1. 파이프 이름 생성
                string pipeName = Guid.NewGuid().ToString();

                // 2. RPG 실행 (파이프 이름 인자로 전달)
                var rpgPath = "../Rpg/Rpg.exe"; // 실제 경로로 수정 필요
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = rpgPath,
                    Arguments = $"--pipe={pipeName}",
                    UseShellExecute = false
                };
                System.Diagnostics.Process rpgProc = null;
                try
                {
                    rpgProc = System.Diagnostics.Process.Start(psi);
                }
                catch (Exception ex)
                {
                    txtLoginStatus.Text = $"게임 실행 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                    return;
                }

                // 3. 파이프로 토큰 전송
                try
                {
                    var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = token });
                    await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(pipeName, tokensJson);
                }
                catch (Exception ex)
                {
                    txtLoginStatus.Text = $"토큰 전달 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                    if (rpgProc != null && !rpgProc.HasExited) rpgProc.Kill();
                    return;
                }

                // (기존) Page1로 라우팅
                LoginPanel.Visibility = Visibility.Collapsed;
                GamePanel.Visibility = Visibility.Visible;
                txtGameStatus.Text = "로그인에 성공했습니다! 게임이 자동으로 실행됩니다.";
                // 게임 자동 실행
                string loginPipeName = Guid.NewGuid().ToString();
                var loginRpgPath = "../Rpg/Rpg.exe";
                var loginPsi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = loginRpgPath,
                    Arguments = $"--pipe={loginPipeName}",
                    UseShellExecute = false
                };
                System.Diagnostics.Process loginRpgProc = null;
                try { loginRpgProc = System.Diagnostics.Process.Start(loginPsi); }
                catch (Exception ex) { txtGameStatus.Text = $"게임 실행 실패: {ex.Message}"; return; }
                try {
                    var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = token });
                    await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(loginPipeName, tokensJson);
                } catch (Exception ex) {
                    txtGameStatus.Text = $"토큰 전달 실패: {ex.Message}";
                    if (loginRpgProc != null && !loginRpgProc.HasExited) loginRpgProc.Kill();
                    return;
                }
                // new Page1(token).Show();
                // this.Close();
            }
            catch (Exception ex)
            {
                txtLoginStatus.Text = "로그인 실패: " + ex.Message;
            }
            finally
            {
                btnLogin.IsEnabled = true;
                btnGoogleLogin.IsEnabled = true;
            }
        }

        private async void btnGoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            btnGoogleLogin.IsEnabled = false;

            int port = 4001;
            string callbackUrl = $"http://localhost:{port}/callback/";
            txtLoginStatus.Text = "구글 로그인 대기 중...";

            // 1. 리스너 시작
            var listener = new System.Net.HttpListener();
            listener.Prefixes.Add(callbackUrl);
            listener.Start();

            // 2. 브라우저로 구글 OAuth 시작
            string googleAuthUrl = $"http://localhost:4000/api/auth/google?callback={System.Net.WebUtility.UrlEncode(callbackUrl)}";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = googleAuthUrl,
                UseShellExecute = true
            });

            // 3. 콜백에서 토큰 수신 대기
            string token = null;
            try
            {
                var context = await listener.GetContextAsync();
                var req = context.Request;
                token = req.QueryString["token"];
                string responseString = "<html><body>구글 로그인 완료!<script>window.close();</script></body></html>";
                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
                listener.Stop();
            }
            catch (Exception ex)
            {
                listener.Stop();
                txtLoginStatus.Text = "구글 로그인 콜백 오류: " + ex.Message + "\n재로그인 후 다시 시도하세요.";
                btnLogin.IsEnabled = true;
                btnGoogleLogin.IsEnabled = true;
                txtEmail.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                // 토큰이 임시(needNickname)인지 확인
                bool needNickname = false;
                string nickname = null;
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    try
                    {
                        var resp = await client.GetAsync("http://localhost:4000/api/auth/me");
                        var respStr = await resp.Content.ReadAsStringAsync();
                        if (!resp.IsSuccessStatusCode && respStr.Contains("needNickname"))
                        {
                            needNickname = true;
                        }
                        else if (resp.IsSuccessStatusCode)
                        {
                            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(respStr);
                            nickname = result.nickname;
                        }
                    }
                    catch { }
                }
                if (needNickname || string.IsNullOrEmpty(nickname))
                {
                    // 닉네임 입력 다이얼로그
                    var dlg = new NicknameDialog(token);
                    var dlgResult = dlg.ShowDialog();
                    if (dlgResult == true && !string.IsNullOrWhiteSpace(dlg.ResultToken))
                    {
                        txtLoginStatus.Text = "닉네임 등록 완료!";
                        loginToken = dlg.ResultToken;
                        try
                        {
                            // 1. 파이프 이름 생성
                            string pipeName = Guid.NewGuid().ToString();

                            // 2. RPG 실행 (파이프 이름 인자로 전달)
                            var rpgPath = "../Rpg/Rpg.exe"; // 실제 경로로 수정 필요
                            var psi = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = rpgPath,
                                Arguments = $"--pipe={pipeName}",
                                UseShellExecute = false
                            };
                            System.Diagnostics.Process rpgProc = null;
                            try
                            {
                                rpgProc = System.Diagnostics.Process.Start(psi);
                            }
                            catch (Exception ex)
                            {
                                txtLoginStatus.Text = $"게임 실행 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                                btnLogin.IsEnabled = true;
                                btnGoogleLogin.IsEnabled = true;
                                txtEmail.Focus();
                                return;
                            }

                            // 3. 파이프로 토큰 전송
                            try
                            {
                                var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = loginToken });
                                await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(pipeName, tokensJson);
                            }
                            catch (Exception ex)
                            {
                                txtLoginStatus.Text = $"토큰 전달 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                                if (rpgProc != null && !rpgProc.HasExited) rpgProc.Kill();
                                btnLogin.IsEnabled = true;
                                btnGoogleLogin.IsEnabled = true;
                                txtEmail.Focus();
                                return;
                            }
                            // (기존) Page1로 라우팅
                            btnLogout.Visibility = Visibility.Visible;
                            new Page1(loginToken).Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            txtLoginStatus.Text = "게임 실행/토큰 전달 실패: " + ex.Message + "\n재로그인 후 다시 시도하세요.";
                            btnLogin.IsEnabled = true;
                            btnGoogleLogin.IsEnabled = true;
                            txtEmail.Focus();
                        }
                    }
                    else
                    {
                        txtLoginStatus.Text = "닉네임 등록이 취소되었습니다.\n재로그인 후 다시 시도하세요.";
                        btnLogin.IsEnabled = true;
                        btnGoogleLogin.IsEnabled = true;
                        txtEmail.Focus();
                    }
                }
                else
                {
                    txtLoginStatus.Text = "구글 로그인 성공!";
                    loginToken = token;
                    string pipeName = Guid.NewGuid().ToString();
                    // 2. RPG 실행 (파이프 이름 인자로 전달)
                    var rpgPath = "../Rpg/Rpg.exe"; // 실제 경로로 수정 필요
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = rpgPath,
                        Arguments = $"--pipe={pipeName}",
                        UseShellExecute = false
                    };
                    System.Diagnostics.Process rpgProc = null;
                    try
                    {
                        rpgProc = System.Diagnostics.Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        txtLoginStatus.Text = $"게임 실행 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                        btnLogin.IsEnabled = true;
                        btnGoogleLogin.IsEnabled = true;
                        txtEmail.Focus();
                        return;
                    }
                    // 3. 파이프로 토큰 전송
                    try
                    {
                        var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = token });
                        await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(pipeName, tokensJson);
                    }
                    catch (Exception ex)
                    {
                        txtLoginStatus.Text = $"토큰 전달 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                        if (rpgProc != null && !rpgProc.HasExited) rpgProc.Kill();
                        btnLogin.IsEnabled = true;
                        btnGoogleLogin.IsEnabled = true;
                        txtEmail.Focus();
                        return;
                    }
                    // (기존) Page1로 라우팅
                    LoginPanel.Visibility = Visibility.Collapsed;
                    GamePanel.Visibility = Visibility.Visible;
                    txtGameStatus.Text = "로그인에 성공했습니다! 게임이 자동으로 실행됩니다.";
                    // 게임 자동 실행
                    string googlePipeName = Guid.NewGuid().ToString();
                    var googleRpgPath = "../Rpg/Rpg.exe";
                    var googlePsi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = googleRpgPath,
                        Arguments = $"--pipe={googlePipeName}",
                        UseShellExecute = false
                    };
                    System.Diagnostics.Process googleRpgProc = null;
                    try
                    {
                        googleRpgProc = System.Diagnostics.Process.Start(googlePsi);
                    }
                    catch (Exception ex)
                    {
                        txtLoginStatus.Text = $"게임 실행 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                        btnLogin.IsEnabled = true;
                        btnGoogleLogin.IsEnabled = true;
                        txtEmail.Focus();
                        return;
                    }

                    // 3. 파이프로 토큰 전송
                    try
                    {
                        var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = loginToken });
                        await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(googlePipeName, tokensJson);
                    }
                    catch (Exception ex)
                    {
                        txtLoginStatus.Text = $"토큰 전달 실패: {ex.Message}\n재로그인 후 다시 시도하세요.";
                        if (googleRpgProc != null && !googleRpgProc.HasExited) googleRpgProc.Kill();
                        btnLogin.IsEnabled = true;
                        btnGoogleLogin.IsEnabled = true;
                        txtEmail.Focus();
                        return;
                    }
                }
            }
            else
            {
                txtLoginStatus.Text = "구글 로그인 실패: 토큰 없음\n재로그인 후 다시 시도하세요.";
                btnLogin.IsEnabled = true;
                btnGoogleLogin.IsEnabled = true;
                txtEmail.Focus();
            }
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            string signupUrl = "http://localhost:3000";
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = signupUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                txtLoginStatus.Text = "회원가입 페이지 열기 실패: " + ex.Message;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Clear();
            txtPassword.Clear();
            txtLoginStatus.Text = "로그아웃 되었습니다. 다시 로그인 해주세요.";
            btnLogin.IsEnabled = true;
            btnGoogleLogin.IsEnabled = true;
            LoginPanel.Visibility = Visibility.Visible;
            GamePanel.Visibility = Visibility.Collapsed;
            txtEmail.Clear();
            txtPassword.Clear();
            txtLoginStatus.Text = "로그아웃 되었습니다. 다시 로그인 해주세요.";
            txtEmail.Focus();
        }
        private async void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            txtGameStatus.Text = "게임을 실행합니다...";
            string manualPipeName = Guid.NewGuid().ToString();
            var manualRpgPath = "../Rpg/Rpg.exe";
            var manualPsi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = manualRpgPath,
                Arguments = $"--pipe={manualPipeName}",
                UseShellExecute = false
            };
            System.Diagnostics.Process manualRpgProc = null;
            try { manualRpgProc = System.Diagnostics.Process.Start(manualPsi); }
            catch (Exception ex) { txtGameStatus.Text = $"게임 실행 실패: {ex.Message}"; return; }
            try {
                var tokensJson = System.Text.Json.JsonSerializer.Serialize(new { jwt = loginToken });
                await Launcher.LauncherPipeUtil.SendTokenToPipeAsync(manualPipeName, tokensJson);
                txtGameStatus.Text = "게임이 실행되었습니다.";
            } catch (Exception ex) {
                txtGameStatus.Text = $"토큰 전달 실패: {ex.Message}";
                if (manualRpgProc != null && !manualRpgProc.HasExited) manualRpgProc.Kill();
                return;
            }
        }
    }
}