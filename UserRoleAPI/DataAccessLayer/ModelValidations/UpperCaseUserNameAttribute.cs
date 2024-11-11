using System.ComponentModel.DataAnnotations;

namespace UserRoleAPI.DataAccessLayer.ModelValidations
{
    public class UpperCaseUserNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult("UserName is required.");

            var userName = value.ToString();
            if (char.IsUpper(userName[0]))
                return ValidationResult.Success;

            return new ValidationResult("The first letter of UserName must be uppercase.");
        }
    }
}