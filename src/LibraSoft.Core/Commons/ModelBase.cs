using FluentValidation.Results;
using LibraSoft.Core.Exceptions;

namespace LibraSoft.Core.Commons
{
    public abstract class ModelBase
    {
        protected static void ThrowErrorInValidate(ValidationResult validate)
        {
            if (!validate.IsValid)
            {
                var messages = validate.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ModelValidateError(messages);
            }
        }

    }
}
