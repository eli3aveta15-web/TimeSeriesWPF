using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlaUI.UIA3;
using System.Diagnostics;
using System.IO;
using TimeSeriesWPF.Tests.PageObjects;

namespace TimeSeriesWPF.Tests.Tests
{
    [TestClass]
    public class LoginTests
    {
        private Process _process;
        private UIA3Automation _automation;
        private string _appPath;

        // Добавь это свойство!
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            // Простой способ - используем жесткий путь
            // Замени на свой актуальный путь!
            _appPath = @"D:\УНИК\8 семестр\пестин\lr7\TimeSeriesWPF\bin\Debug\TimeSeriesWPF.exe";
            _appPath = Path.GetFullPath(_appPath);

            // Если файл не найден, попробуй альтернативный путь
            if (!File.Exists(_appPath))
            {
                _appPath = @"D:\УНИК\8 семестр\пестин\TimeSeriesWPF\bin\Debug\net48\TimeSeriesWPF.exe";
            }

            // Проверяем существование файла
            if (!File.Exists(_appPath))
            {
                throw new FileNotFoundException($"Приложение не найдено по пути: {_appPath}");
            }

            // Запускаем приложение
            var startInfo = new ProcessStartInfo
            {
                FileName = _appPath,
                WorkingDirectory = Path.GetDirectoryName(_appPath)
            };

            _process = Process.Start(startInfo);
            System.Threading.Thread.Sleep(3000);

            _automation = new UIA3Automation();
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill();
                    _process.Dispose();
                }
            }
            catch { }

            try
            {
                _automation?.Dispose();
            }
            catch { }
        }

        [TestMethod]
        public void Login_ValidCredentials_Success()
        {
            var loginPage = new LoginPage(_automation, _automation.GetDesktop());

            var mainPage = loginPage
                .EnterUsername("admin")
                .EnterPassword("admin123")
                .ClickLogin();

            Assert.IsTrue(mainPage.IsUserLoggedIn("admin"));
        }

        [TestMethod]
        public void Login_InvalidPassword_ShowError()
        {
            var loginPage = new LoginPage(_automation, _automation.GetDesktop());

            loginPage
                .EnterUsername("admin")
                .EnterPassword("wrongpassword")
                .ClickLogin();

            Assert.IsTrue(loginPage.IsErrorVisible());
        }

     //   [TestMethod]
     //   public void Register_NewUser_CanLogin()
     //   {
     //       var loginPage = new LoginPage(_automation, _automation.GetDesktop());
     //       string testUsername = "testuser" + System.DateTime.Now.Second;
     //       string testPassword = "test1234";

     //       var registerPage = loginPage.ClickRegister();
     //       registerPage
     //.EnterUsername(testUsername)
     //.EnterPassword(testPassword)
     //.SelectRole("Analyst");
     //       registerPage.ClickRegisterButton();

     //       var mainPage = loginPage
     //           .EnterUsername(testUsername)
     //           .EnterPassword(testPassword)
     //           .ClickLogin();

     //       Assert.IsTrue(mainPage.IsUserLoggedIn(testUsername));
     //   }
    }
}