using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    public class RentValidate : AbstractValidator<Rent>
    {

        public RentValidate()
        {
            RuleFor(rent => rent.UserId).NotEmpty().WithMessage("Rent user id is required.");
            RuleFor(rent => rent.RentDate).NotEmpty().WithMessage("Rent date required.");
            RuleFor(rent => rent.ReturnDate).NotEmpty().WithMessage("Rent return date is required.")
                .GreaterThan(rent => rent.RentDate).WithMessage("The return date cannot be less than the rental date");
            RuleFor(rent => rent.Status).IsInEnum().WithMessage("Rent status must be a valid.");
        }
    }
}
