using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;
using System.Threading;

namespace TimeSeriesWPF.Tests.PageObjects
{
    public class RegisterPage
    {
        private readonly UIA3Automation _automation;
        private readonly AutomationElement _window;

        public RegisterPage(UIA3Automation automation, AutomationElement window)
        {
            _automation = automation ?? throw new ArgumentNullException(nameof(automation));
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public RegisterPage EnterUsername(string username)
        {
            Thread.Sleep(300);

            var usernameBox = _window.FindFirstDescendant(
                cf => cf.ByAutomationId("txtUsername"));

            if (usernameBox == null)
                usernameBox = _window.FindFirstDescendant(
                    cf => cf.ByAutomationId("txtRegisterUsername"));

            if (usernameBox == null)
                usernameBox = _window.FindFirstDescendant(
                    cf => cf.ByControlType(ControlType.Edit));

            if (usernameBox != null)
            {
                var textBox = usernameBox.AsTextBox();
                if (textBox != null)
                {
                    textBox.Text = username;
                    Thread.Sleep(200);
                    System.Diagnostics.Debug.WriteLine($"[OK] Введено имя: {username}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[ERROR] Поле имени не найдено!");
            }
            return this;
        }

        public RegisterPage EnterPassword(string password)
        {
            Thread.Sleep(300);

            var passwordBox = _window.FindFirstDescendant(
                cf => cf.ByAutomationId("txtPassword"));

            if (passwordBox == null)
                passwordBox = _window.FindFirstDescendant(
                    cf => cf.ByAutomationId("txtRegisterPassword"));

            if (passwordBox == null)
            {
                var allEdits = _window.FindAllDescendants(
                    cf => cf.ByControlType(ControlType.Edit));
                if (allEdits.Length > 1)
                    passwordBox = allEdits[1];
            }

            if (passwordBox != null)
            {
                var textBox = passwordBox.AsTextBox();
                if (textBox != null)
                {
                    textBox.Text = password;
                    Thread.Sleep(200);
                    System.Diagnostics.Debug.WriteLine("[OK] Введён пароль");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[ERROR] Поле пароля не найдено!");
            }
            return this;
        }

        public RegisterPage SelectRole(string role)
        {
            Thread.Sleep(300);

            var comboBox = _window.FindFirstDescendant(
                cf => cf.ByAutomationId("cmbRole"));

            if (comboBox == null)
                comboBox = _window.FindFirstDescendant(
                    cf => cf.ByControlType(ControlType.ComboBox));

            if (comboBox != null)
            {
                var cb = comboBox.AsComboBox();
                if (cb != null)
                {
                    cb.Select(role);
                    Thread.Sleep(200);
                    System.Diagnostics.Debug.WriteLine($"[OK] Выбрана роль: {role}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[ERROR] ComboBox не найден!");
            }
            return this;
        }

        public void ClickRegisterButton()
        {
            var registerBtn = _window.FindFirstDescendant(
                cf => cf.ByText("Зарегистрироваться"));

            if (registerBtn == null)
                registerBtn = _window.FindFirstDescendant(
                    cf => cf.ByText("Register"));

            registerBtn?.AsButton()?.Invoke();
            Thread.Sleep(500);
        }

        public void ClickMessageBoxOk()
        {
            Thread.Sleep(500);
            var desktop = _automation.GetDesktop();

            var okButton = desktop.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Button)
                        .And(cf.ByName("OK")));

            if (okButton == null)
                okButton = desktop.FindFirstDescendant(
                    cf => cf.ByControlType(ControlType.Button)
                            .And(cf.ByName("ОК")));

            okButton?.AsButton()?.Invoke();
            Thread.Sleep(500);
        }

        public string GetSuccessMessage(int timeoutMs = 5000)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < timeoutMs)
            {
                var desktop = _automation.GetDesktop();

                var msgBox = desktop.FindFirstDescendant(
                    cf => cf.ByControlType(ControlType.Window)
                            .And(cf.ByName("Успех")));

                if (msgBox == null)
                    msgBox = desktop.FindFirstDescendant(
                        cf => cf.ByControlType(ControlType.Window)
                                .And(cf.ByName("Success")));

                if (msgBox != null)
                {
                    var textElement = msgBox.FindFirstDescendant(
                        cf => cf.ByControlType(ControlType.Text));

                    if (textElement != null)
                    {
                        var text = textElement.Name;
                        if (!string.IsNullOrWhiteSpace(text))
                            return text.Trim();
                    }
                }

                var successText = desktop.FindFirstDescendant(
                    cf => cf.ByControlType(ControlType.Text)
                            .And(cf.ByName("Регистрация успешна")));

                if (successText != null)
                    return successText.Name?.Trim() ?? "";

                Thread.Sleep(200);
            }
            return string.Empty;
        }

        public bool IsVisible()
        {
            var header = _window.FindFirstDescendant(
                cf => cf.ByText("Регистрация"));
            return header != null;
        }
    }
}