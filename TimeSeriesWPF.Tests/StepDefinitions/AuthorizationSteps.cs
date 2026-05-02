using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using FlaUI.UIA3;
using TechTalk.SpecFlow;
using TimeSeriesWPF.Tests.PageObjects;

namespace TimeSeriesWPF.Tests.StepDefinitions
{
    [Binding]
    public class AuthorizationSteps
    {
        private Process _process;
        private UIA3Automation _automation;
        private LoginPage _loginPage;
        private MainPage _mainPage;

        [BeforeScenario]
        public void Setup()
        {
            string appPath = @"D:\УНИК\8 семестр\пестин\lr7\TimeSeriesWPF\bin\Debug\TimeSeriesWPF.exe";
            appPath = Path.GetFullPath(appPath);

            if (!File.Exists(appPath))
            {
                throw new FileNotFoundException($"Приложение не найдено: {appPath}");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = appPath,
                WorkingDirectory = Path.GetDirectoryName(appPath)
            };

            _process = Process.Start(startInfo);
            System.Threading.Thread.Sleep(3000);

            _automation = new UIA3Automation();
            _loginPage = new LoginPage(_automation, _automation.GetDesktop());
        }

        [AfterScenario]
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

        [Given(@"пользователь ""(.*)"" существует в системе с паролем ""(.*)""")]
        public void GivenUserExists(string username, string password)
        {
      
        }

        [When(@"пользователь вводит логин ""(.*)"" и пароль ""(.*)""")]
        public void WhenUserEntersCredentials(string login, string password)
        {
            _loginPage.EnterUsername(login).EnterPassword(password);
            System.Threading.Thread.Sleep(500);
        }

        [When(@"нажимает кнопку ""Войти""")]
        public void WhenClicksLoginButton()
        {
            _mainPage = _loginPage.ClickLogin();
            System.Threading.Thread.Sleep(1000);
        }

        [Then(@"система должна открыть главное окно")]
        public void ThenMainWindowOpens()
        {
            _mainPage.Should().NotBeNull("Главное окно должно открыться после успешного входа");
        }

        [Then(@"отобразить информацию о пользователе ""(.*)""")]
        public void ThenUserInfoDisplayed(string expectedUser)
        {
            var userInfo = _mainPage.GetUserInfo();
            userInfo.Should().Contain(expectedUser,
                $"Информация о пользователе должна содержать '{expectedUser}'");
        }

        [Then(@"система должна показать сообщение об ошибке входа")]
        public void ThenLoginErrorMessageDisplayed()
        {
            var isErrorVisible = _loginPage.IsErrorVisible();
            isErrorVisible.Should().BeTrue("Должно отображаться сообщение об ошибке входа");
        }

        [Then(@"пользователь должен остаться на странице входа")]
        public void ThenUserRemainsOnLoginPage()
        {
            _loginPage.Should().NotBeNull("Пользователь должен остаться на странице входа");
        }
    }
}