using Microsoft.Extensions.Options;

namespace LibraSoft.Api.Services.EmailService
{
    public class EmailSettingsValidation : IValidateOptions<EmailSettings>
    {
        public ValidateOptionsResult Validate(string? name, EmailSettings options)
        {
            if (options.Port == 0)
            {
                return ValidateOptionsResult.Fail("Email Port is required field.");

            }
            if (int.IsNegative(options.Port))
            {
                return ValidateOptionsResult.Fail("Email Port can't be a negative");
            }
            if (string.IsNullOrEmpty(options.Host))
            {
                return ValidateOptionsResult.Fail("Email Host is required field");
            }
            if (string.IsNullOrEmpty(options.EmailFrom))
            {
                return ValidateOptionsResult.Fail("EmailFrom is required field");
            }
            if (options.Credentials == null)
            {
                return ValidateOptionsResult.Fail("Credentials is required field");

            }
            if (string.IsNullOrEmpty(options.Credentials.UserName))
            {
                return ValidateOptionsResult.Fail("Email Credential UserName is required field");
            }
            if (string.IsNullOrEmpty(options.Credentials.Password))
            {
                return ValidateOptionsResult.Fail("Email Credential Password is required field");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
