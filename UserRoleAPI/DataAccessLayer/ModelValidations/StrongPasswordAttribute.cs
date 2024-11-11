using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserRoleAPI.DataAccessLayer.ModelValidations
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult("Password is required.");

            var password = value.ToString();

            // Define password complexity requirements
            bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]");
            bool hasMinLength = password.Length >= 8;

            if (hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar && hasMinLength)
                return ValidationResult.Success;

            return new ValidationResult("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }
    }
}
