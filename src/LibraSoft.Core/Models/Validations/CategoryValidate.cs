using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    internal class CategoryValidate : AbstractValidator<Category>
    {
        public CategoryValidate() {
            RuleFor(category => category.Title).NotEmpty().WithMessage("Category title is required.");
        }
    }
}
