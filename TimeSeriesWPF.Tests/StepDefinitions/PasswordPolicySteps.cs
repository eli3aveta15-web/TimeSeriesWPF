using FluentAssertions;
using TechTalk.SpecFlow;
using TimeSeriesWPF;

namespace TimeSeriesWPF.Tests.StepDefinitions  
{
    [Binding]  
    public class PasswordPolicySteps
    {
        private readonly AuthenticationService _authService;
        private bool _result;
        private string _currentPassword;

        public PasswordPolicySteps()
        {
            _authService = new AuthenticationService();
        }

        [BeforeScenario]
        public void Setup()
        {
            _result = false;
            _currentPassword = null;
        }

        [AfterScenario]
        public void TearDown()
        {
           
        }

  
        [Given(@"система проверки пароля активна")]
        public void GivenPasswordSystemIsActive()
        {
          
        }

        [When(@"пользователь вводит пароль ""(.*)""")]
        public void WhenUserEntersPassword(string password)
        {
            _currentPassword = password;

            if (password == "null")
            {
                _currentPassword = null;
            }

            _result = _authService.IsPasswordStrong(_currentPassword);
        }

        [Then(@"система должна вернуть результат ""(.*)""")]
        public void ThenSystemReturnsResult(string expectedResult)
        {
            bool expected = expectedResult.ToLower() == "true";
            _result.Should().Be(expected,
                $"Пароль '{_currentPassword ?? "null"}' должен вернуть {expectedResult}");
        }
    }
}