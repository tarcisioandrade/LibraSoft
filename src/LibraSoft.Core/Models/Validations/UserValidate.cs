using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    internal class UserValidate : AbstractValidator<User>
    {
        private string TelephoneRegex { get; init; } = 
            @"(\b\(\d{2}\)\s?[9]?\s?\d{4}(\-|\s)?\d\d{4})|(\b\d{2}\s?[9]?\s?\d{4}(\-|\s)?\d{4})|(\b([9]|[9]\s)?\d{4}(\-|\s)?\d{4})|(\b\d{4}(\-|\s)?\d{4})";
        private string EmailRegex { get; init; } = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private string ZipCodeRegex { get; init; } = @"^\d{2}\d{3}\d{3}$";

        public UserValidate()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches("[a-zA-Z]").WithMessage("Password must contain at least one letter.");
            RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .Matches(EmailRegex).WithMessage("Email must be a valid email address.");
            RuleFor(user => user.Telephone)
           .NotEmpty().WithMessage("Phone number cannot be empty.")
           .Matches(TelephoneRegex).WithMessage("Phone number must be a valid mobile number.");
            RuleFor(user => user.Status).IsInEnum().WithMessage("User status must be a valid.");
            RuleFor(user => user.Role).IsInEnum().WithMessage("User role must be a valid.");
            When(user => user.Address != null, () =>
            {
                RuleFor(user => user.Address!.Street).NotEmpty().WithMessage("Street is required.");
                RuleFor(user => user.Address!.City).NotEmpty().WithMessage("City is required.");
                RuleFor(user => user.Address!.State).NotEmpty().WithMessage("State is required.");
                RuleFor(user => user.Address!.ZipCode).NotEmpty().WithMessage("ZipCode is required.").Matches(ZipCodeRegex).WithMessage("ZipCode must be a valid.");
                RuleFor(user => user.Address!.Country).NotEmpty().WithMessage("Country is required.");
            });
        }
    }
}
