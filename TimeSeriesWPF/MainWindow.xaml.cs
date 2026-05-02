using System.Windows;
using System.Windows.Controls;

namespace TimeSeriesWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите имя пользователя и пароль");
                return;
            }

            var user = UserRepository.ValidateLogin(username, password);
            if (user != null)
            {
                var mainAppWindow = new MainAppWindow(user);
                mainAppWindow.Show();
                this.Close();
            }
            else
            {
                ShowError("Неверное имя пользователя или пароль");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}