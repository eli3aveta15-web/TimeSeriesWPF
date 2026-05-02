using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSeriesWPF;

namespace TimeSeriesWPF.Tests.Tests
{
    [TestClass]
    public class PasswordPolicyTests
    {
        [TestMethod]
        public void IsPasswordStrong_PasswordShorterThan8_ReturnsFalse()
        {
         
            var service = new AuthenticationService();
            string weakPassword = "short";

           
            bool result = service.IsPasswordStrong(weakPassword);

           
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPasswordStrong_NoDigit_ReturnsFalse()
        {
            var service = new AuthenticationService();
            string weakPassword = "abcdefgh"; 

           
            bool result = service.IsPasswordStrong(weakPassword);

       
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPasswordStrong_StrongPassword_ReturnsTrue()
        {
    
            var service = new AuthenticationService();
            string strongPassword = "StrongPass1"; 

         
            bool result = service.IsPasswordStrong(strongPassword);

        
            Assert.IsTrue(result);
        }
    }


}