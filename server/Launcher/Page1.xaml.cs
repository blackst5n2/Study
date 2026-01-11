using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Window
    {
        private GameLauncher gameLauncher = new GameLauncher();
        private SecurityManager securityManager = new SecurityManager();

        private string? loginToken;
        
        public Page1(string token)
        {
            InitializeComponent();
            loginToken = token;
        }
        
        private async void btnLaunchGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginToken))
                {
                    MessageBox.Show("로그인 토큰이 없습니다. 다시 로그인 해주세요.");
                    // MainWindow로 이동
                    new MainWindow().Show();
                    this.Close();
                    return;
                }
                await gameLauncher.LaunchGameAsync(loginToken);
                MessageBox.Show("게임이 실행되었습니다!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("게임 실행 실패: " + ex.Message);
            }
        }
    }
}
