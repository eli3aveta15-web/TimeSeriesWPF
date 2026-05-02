using System;
using System.Linq;

namespace TimeSeriesWPF
{
    public class AuthenticationService
    {
        public bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            return password.Length >= 8 && password.Any(char.IsDigit);
        }
    }
}