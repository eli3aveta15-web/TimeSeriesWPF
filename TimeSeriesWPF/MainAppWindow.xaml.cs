using System.Windows;

namespace TimeSeriesWPF
{
    public partial class MainAppWindow : Window
    {
        private readonly User _currentUser;

        public MainAppWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            txtUserInfo.Text = $"Пользователь: {_currentUser.Username} | Роль: {_currentUser.Role}";
        }

        private void BtnInterpolate_Click(object sender, RoutedEventArgs e)
        {
            double x1 = 0, y1 = 10;
            double x2 = 10, y2 = 100;
            double x = 5;

            double result = y1 + (y2 - y1) * (x - x1) / (x2 - x1);

            MessageBox.Show($"Интерполяция:\nТочки: ({x1}, {y1}) и ({x2}, {y2})\nX = {x}\nРезультат: {result}",
                "Интерполяция", MessageBoxButton.OK, MessageBoxImage.Information);

            txtResult.Text = $"Последняя интерполяция: {result}";
        }

        private void BtnExtrapolate_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Analyst")
            {
                MessageBox.Show("Недостаточно прав для выполнения прогноза!",
                    "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double[] data = { 10, 20, 30, 40, 50 };
            double avgDiff = 10;
            double forecast = data[data.Length - 1] + avgDiff;

            MessageBox.Show($"Экстраполяция:\nДанные: {string.Join(", ", data)}\nПрогноз: {forecast}",
                "Прогноз", MessageBoxButton.OK, MessageBoxImage.Information);

            txtResult.Text = $"Последний прогноз: {forecast}";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}