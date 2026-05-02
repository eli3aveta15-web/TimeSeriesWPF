using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;

namespace TimeSeriesWPF.Tests.PageObjects
{
    public class LoginPage
    {
        private readonly UIA3Automation _automation;
        private readonly AutomationElement _window;

        public LoginPage(UIA3Automation automation, AutomationElement window)
        {
            _automation = automation;
            _window = window;
        }

        private AutomationElement FindElementByAutomationId(string automationId)
        {
            try
            {
                return _window?.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
            }
            catch
            {
                return null;
            }
        }

        private AutomationElement FindElementByText(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return null;
                return _window?.FindFirstDescendant(cf => cf.ByText(text));
            }
            catch
            {
                return null;
            }
        }

        private AutomationElement UsernameField => FindElementByAutomationId("txtUsername");
        private AutomationElement PasswordField => FindElementByAutomationId("txtPassword");
        private AutomationElement LoginButton => FindElementByText("Войти");
        private AutomationElement RegisterButton => FindElementByText("Зарегистрироваться");
        private AutomationElement ErrorText => FindElementByAutomationId("txtError");

        public LoginPage EnterUsername(string username)
        {
            var field = UsernameField;
            if (field != null)
            {
                try { field.AsTextBox().Text = username; }
                catch { }
            }
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            var field = PasswordField;
            if (field != null)
            {
                try { field.AsTextBox().Text = password; }
                catch { }
            }
            return this;
        }

        // ✅ ИСПРАВЛЕНО: Находим и возвращаем ОКНО РЕГИСТРАЦИИ, а не десктоп
        public RegisterPage ClickRegister()
        {
            System.Threading.Thread.Sleep(500);

            // Ищем кнопку "Регистрация" на странице входа
            var registerButton = _window.FindFirstDescendant(cf => cf.ByText("Регистрация"));

            if (registerButton != null)
            {
                var button = registerButton.AsButton();
                if (button != null)
                {
                    System.Diagnostics.Debug.WriteLine("[INFO] Нажимаем кнопку 'Регистрация'...");
                    button.Invoke();

                    // 🔥 ВАЖНО: Ждём появления НОВОГО окна регистрации
                    System.Threading.Thread.Sleep(2000);

                    // Ищем открывшееся окно регистрации на уровне Desktop
                    var desktop = _automation.GetDesktop();
                    var registerWindow = desktop.FindFirstDescendant(
                        cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window)
                                .And(cf.ByName("Регистрация")));

                    if (registerWindow != null)
                    {
                        System.Diagnostics.Debug.WriteLine("[OK] Окно регистрации найдено, создаём RegisterPage");
                        return new RegisterPage(_automation, registerWindow);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[ERROR] Окно регистрации НЕ найдено!");
                        // Выводим все окна для отладки
                        var allWindows = desktop.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
                        foreach (var win in allWindows)
                        {
                            System.Diagnostics.Debug.WriteLine($"  Окно: '{win.Name}' | AutomationId: '{win.AutomationId}'");
                        }
                    }
                }
            }

            return null;
        }
        public bool IsVisible()
        {
            // Последовательный поиск характерных элементов страницы входа.
            // Вместо || выполняем независимые поиски: как только находится хотя бы один элемент, возвращаем true.

            // 1. Проверяем наличие кнопки "Войти"
            var loginButton = _window.FindFirstDescendant(cf => cf.ByText("Войти"));
            if (loginButton != null) return true;

            // 2. Проверяем поле ввода логина (замените AutomationId на реальный, если отличается)
            var usernameField = _window.FindFirstDescendant(cf => cf.ByAutomationId("txtUsername"));
            if (usernameField != null) return true;

            // 3. Проверяем заголовок формы/окна
            var header = _window.FindFirstDescendant(cf => cf.ByText("Вход"));
            if (header != null) return true;

            header = _window.FindFirstDescendant(cf => cf.ByText("Авторизация"));
            if (header != null) return true;

            // Если ни один из характерных элементов не найден
            return false;
        }

        public MainPage ClickLogin()
        {
            var button = LoginButton;
            if (button != null)
            {
                try { button.Click(); }
                catch { }
            }

            System.Threading.Thread.Sleep(1500);

            // Закрываем MessageBox если появился
            try
            {
                var okButton = _automation.GetDesktop()
                    .FindFirstDescendant(cf => cf.ByText("OK"));
                okButton?.Click();
            }
            catch { }

            return new MainPage(_automation, _automation.GetDesktop());
        }

        public string GetErrorMessage()
        {
            var error = ErrorText;
            try { return error?.Name ?? ""; }
            catch { return ""; }
        }

        public bool IsErrorVisible()
        {
            var error = ErrorText;
            try { return error != null && !error.IsOffscreen; }
            catch { return false; }
        }
    }
}