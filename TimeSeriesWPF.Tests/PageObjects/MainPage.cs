using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;

namespace TimeSeriesWPF.Tests.PageObjects
{
    public class MainPage
    {
        private readonly UIA3Automation _automation;
        private readonly AutomationElement _window;

        public MainPage(UIA3Automation automation, AutomationElement window)
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

        private AutomationElement UserInfoText => FindElementByAutomationId("txtUserInfo");
        private AutomationElement InterpolateButton => FindElementByText("Выполнить интерполяцию");
        private AutomationElement ExtrapolateButton => FindElementByText("Выполнить экстраполяцию");
        private AutomationElement LogoutButton => FindElementByText("Выйти");
        private AutomationElement ResultText => FindElementByAutomationId("txtResult");

        public string GetUserInfo()
        {
            var info = UserInfoText;
            try
            {
                return info?.Name ?? "";
            }
            catch
            {
                return "";
            }
        }

        public void ClickInterpolate()
        {
            var button = InterpolateButton;
            if (button != null)
            {
                try
                {
                    button.Click();
                }
                catch { }
            }
            System.Threading.Thread.Sleep(500);

            try
            {
                var okButton = _automation.GetDesktop()
                    .FindFirstDescendant(cf => cf.ByText("OK"));
                okButton?.Click();
            }
            catch { }
        }

        public void ClickExtrapolate()
        {
            var button = ExtrapolateButton;
            if (button != null)
            {
                try
                {
                    button.Click();
                }
                catch { }
            }
            System.Threading.Thread.Sleep(500);

            try
            {
                var okButton = _automation.GetDesktop()
                    .FindFirstDescendant(cf => cf.ByText("OK"));
                okButton?.Click();
            }
            catch { }
        }

        public LoginPage ClickLogout()
        {
            var button = LogoutButton;
            if (button != null)
            {
                try
                {
                    button.Click();
                }
                catch { }
            }
            System.Threading.Thread.Sleep(1500);
            return new LoginPage(_automation, _automation.GetDesktop());
        }

        public string GetResultText()
        {
            var result = ResultText;
            try
            {
                return result?.Name ?? "";
            }
            catch
            {
                return "";
            }
        }

        public bool IsUserLoggedIn(string username)
        {
            var userInfo = GetUserInfo();
            return userInfo.Contains(username);
        }
    }
}