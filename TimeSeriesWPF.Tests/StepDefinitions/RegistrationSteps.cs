//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using FluentAssertions;
//using FlaUI.Core.AutomationElements;
//using FlaUI.Core.Definitions;
//using FlaUI.UIA3;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TechTalk.SpecFlow;
//using TimeSeriesWPF.Tests.PageObjects;

//namespace TimeSeriesWPF.Tests.StepDefinitions
//{
//    [Binding]
//    public class RegistrationSteps
//    {
//        private Process _process;
//        private UIA3Automation _automation;
//        private RegisterPage _registerPage;
//        private AutomationElement _mainWindow;

//        [BeforeScenario]
//        public void Setup()
//        {
//            CloseExistingInstances();

//            string appPath = @"D:\УНИК\8 семестр\пестин\lr7\TimeSeriesWPF\bin\Debug\TimeSeriesWPF.exe";
//            appPath = Path.GetFullPath(appPath);

//            if (!File.Exists(appPath))
//            {
//                throw new FileNotFoundException($"Приложение не найдено: {appPath}");
//            }

//            var startInfo = new ProcessStartInfo
//            {
//                FileName = appPath,
//                WorkingDirectory = Path.GetDirectoryName(appPath),
//                UseShellExecute = true
//            };

//            _process = Process.Start(startInfo);
//            _automation = new UIA3Automation();

//            Thread.Sleep(4000);

//            _mainWindow = FindMainWindow();

//            if (_mainWindow == null)
//            {
//                DebugPrintDesktopElements();
//                Assert.Fail("Главное окно приложения не найдено");
//            }

//            Debug.WriteLine($"[OK] Главное окно найдено: '{_mainWindow.Name}'");
//        }

//        private void CloseExistingInstances()
//        {
//            var existingProcesses = Process.GetProcessesByName("TimeSeriesWPF");
//            foreach (var proc in existingProcesses)
//            {
//                try
//                {
//                    if (!proc.HasExited)
//                    {
//                        proc.Kill();
//                        proc.WaitForExit(1000);
//                    }
//                    proc.Dispose();
//                }
//                catch { }
//            }
//            Thread.Sleep(1000);
//        }


//        private AutomationElement FindMainWindow()
//        {
//            var desktop = _automation.GetDesktop();
//            var windows = desktop.FindAllDescendants(cf => cf.ByControlType(ControlType.Window));

//            foreach (var window in windows)
//            {
    
//                var windowName = window.Name ?? "";
//                if (string.IsNullOrWhiteSpace(windowName))
//                    continue;

//                if (windowName.Contains("Microsoft Visual Studio") ||
//                    windowName.Contains("Program Manager") ||
//                    windowName.Contains("Панель задач") ||
//                    windowName.Contains("Start") ||
//                    windowName.Contains("Search"))
//                    continue;

         
//                if (windowName == "Вход в систему")
//                    return window;

//                if (windowName.Contains("TimeSeriesWPF") && !windowName.Contains("Visual Studio"))
//                    return window;
//            }

//            return null;
//        }

//        [AfterScenario]
//        public void TearDown()
//        {
//            try
//            {
//                if (_process != null && !_process.HasExited)
//                {
//                    _process.Kill();
//                    _process.WaitForExit(1000);
//                    _process.Dispose();
//                }
//            }
//            catch { }

//            try { _automation?.Dispose(); } catch { }
//        }

//        private void DebugPrintDesktopElements()
//        {
//            Debug.WriteLine("=== 🖥️ ЭЛЕМЕНТЫ НА ДЕСКТОПЕ ===");
//            var desktop = _automation.GetDesktop();
//            var windows = desktop.FindAllDescendants(cf => cf.ByControlType(ControlType.Window));

//            foreach (var window in windows)
//            {
//                var name = window.Name ?? "<пусто>";
//                Debug.WriteLine($"🪟 Окно: '{name}'");

//                var buttons = window.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));
//                foreach (var btn in buttons.Take(5))
//                {
//                    var btnName = btn.Name ?? "<пусто>";
//                    Debug.WriteLine($"   🔘 Кнопка: '{btnName}'");
//                }
//            }
//            Debug.WriteLine("=== 🔚 КОНЕЦ ДИАГНОСТИКИ ===");
//        }

//        [Given(@"открыта форма регистрации")]
//        public void GivenRegistrationFormIsOpen()
//        {
//            Debug.WriteLine("=== ОТКРЫТИЕ ФОРМЫ РЕГИСТРАЦИИ ===");

//            if (_mainWindow == null)
//            {
//                Assert.Fail("Главное окно не инициализировано");
//            }

//            var registerButton = FindButtonByText(_mainWindow, "Зарегистрироваться");

//            if (registerButton == null)
//            {
//                Debug.WriteLine("[ERROR] Кнопка 'Зарегистрироваться' не найдена");
//                DebugPrintDesktopElements();
//                Assert.Fail("Кнопка 'Зарегистрироваться' не найдена в главном окне");
//            }

//            Debug.WriteLine("[INFO] Нажимаем кнопку 'Зарегистрироваться'...");
//            registerButton.Invoke();
//            Thread.Sleep(2000);

//            var regWindow = FindRegistrationWindow();
//            if (regWindow == null)
//            {
//                Debug.WriteLine("[ERROR] Окно регистрации не найдено");
//                DebugPrintDesktopElements();
//                Assert.Fail("Окно регистрации не открылось");
//            }

//            _registerPage = new RegisterPage(_automation, regWindow);
//            Debug.WriteLine($"[OK] Окно регистрации найдено: '{regWindow.Name}'");
//        }

//        private Button FindButtonByText(AutomationElement parent, string text)
//        {
//            var buttons = parent.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));
//            foreach (var btn in buttons)
//            {
//                var name = btn.Name ?? "";
//                if (name.Contains(text))
//                    return btn.AsButton();
//            }
//            return null;
//        }

//        private AutomationElement FindRegistrationWindow()
//        {
//            var desktop = _automation.GetDesktop();
//            var windows = desktop.FindAllDescendants(cf => cf.ByControlType(ControlType.Window));

//            foreach (var window in windows)
//            {
//                var name = window.Name ?? "";
        
//                if (name == "Регистрация")
//                    return window;
//            }
//            return null;
//        }

//        [When(@"пользователь вводит имя ""(.*)""")]
//        public void WhenUserEntersUsername(string username)
//        {
//            EnsurePageNotNull();
//            _registerPage.EnterUsername(username);
//            Thread.Sleep(500);
//        }

//        [When(@"вводит пароль ""(.*)""")]
//        public void WhenUserEntersPassword(string password)
//        {
//            EnsurePageNotNull();
//            _registerPage.EnterPassword(password);
//            Thread.Sleep(500);
//        }

//        [When(@"выбирает роль ""(.*)""")]
//        public void WhenUserSelectsRole(string role)
//        {
//            EnsurePageNotNull();
//            _registerPage.SelectRole(role);
//            Thread.Sleep(500);
//        }

//        [When(@"нажимает кнопку ""Зарегистрироваться""")]
//        public void WhenClicksRegisterButton()
//        {
//            EnsurePageNotNull();
//            _registerPage.ClickRegisterButton();
//            Thread.Sleep(2000);
//        }

//        [Then(@"система должна показать сообщение ""(.*)""")]
//        public void ThenSuccessMessageDisplayed(string expectedMessage)
//        {
//            EnsurePageNotNull();
//            var actualMessage = _registerPage.GetSuccessMessage();
//            Debug.WriteLine($"[DEBUG] Ожидаемое: '{expectedMessage}' | Полученное: '{actualMessage}'");

//            actualMessage.Should().Contain("Регистрация успешна");
//            _registerPage.ClickMessageBoxOk();
//            Thread.Sleep(1500);
//        }

//        [Then(@"пользователь должен быть перенаправлен на страницу входа")]
//        public void ThenRedirectedToLoginPage()
//        {
//            Thread.Sleep(1000);
//            var desktop = _automation.GetDesktop();

//            var loginWindow = desktop.FindFirstDescendant(
//                cf => cf.ByControlType(ControlType.Window).And(cf.ByName("Вход в систему")));

//            var usernameField = desktop.FindFirstDescendant(cf => cf.ByAutomationId("txtUsername"));

//            (loginWindow != null || usernameField != null).Should().BeTrue("Пользователь должен быть на странице входа");
//        }

//        private void EnsurePageNotNull()
//        {
//            if (_registerPage == null)
//                Assert.Fail("_registerPage не инициализирован. Проверьте шаг Given.");
//        }
//    }
//}