using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    public class AuthorValidate : AbstractValidator<Author>
    {
        public AuthorValidate() 
        {
            RuleFor(author => author.Name).NotEmpty().WithMessage("Author name is required.");
            RuleFor(author => author.Status).IsInEnum().WithMessage("Incorrect book status.");
        }
    }
}
