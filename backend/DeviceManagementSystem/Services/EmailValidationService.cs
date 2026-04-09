using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeviceManagementSystem.Services
{
    public class EmailValidationService
    {
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
