using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    public class ReviewValidate : AbstractValidator<Review>
    {
        public ReviewValidate()
        {
            RuleFor(review => review.Comment).NotEmpty()
                .WithMessage("Review comment is required.")
                .MaximumLength(1100)
                .WithMessage("Rating comment don't must be greate than 1100 characters.");
            RuleFor(review => review.Title).NotEmpty().WithMessage("Review title is required.").MaximumLength(90)
                .WithMessage("Rating title don't must be greate than 90 characters.");
            RuleFor(review => review.Rating).LessThanOrEqualTo(5)
                .WithMessage("Rating must be less than or equal to 5")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Rating must be greater than or equal to 1");
        }
    }
}
