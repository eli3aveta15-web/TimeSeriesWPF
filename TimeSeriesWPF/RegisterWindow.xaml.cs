// <copyright file="RegisterWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TimeSeriesWPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Окно регистрации пользователя.
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterWindow"/> class.
        /// </summary>
        public RegisterWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки регистрации.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события <see cref="RoutedEventArgs"/>.</param>
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = this.txtUsername.Text.Trim();
            string password = this.txtPassword.Password;
            string role = ((ComboBoxItem)this.cmbRole.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                this.ShowError("Заполните все поля");
                return;
            }

            try
            {
                var user = new User { Username = username, Password = password, Role = role };
                UserRepository.SaveUser(user);

                MessageBox.Show(
                    "Регистрация успешна!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки возврата.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события <see cref="RoutedEventArgs"/>.</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        private void ShowError(string message)
        {
            this.txtError.Text = message;
            this.txtError.Visibility = Visibility.Visible;
        }
    }
}