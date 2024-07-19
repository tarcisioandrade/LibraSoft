using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    public class BookValidate : AbstractValidator<Book>
    {
        public BookValidate()
        {
            RuleFor(book => book.Title).NotEmpty().WithMessage("Book title is required.");
            RuleFor(book => book.Image).NotEmpty().WithMessage("Book image is required.");
            RuleFor(book => book.AuthorId).NotEmpty().WithMessage("Author Id is required.");
            RuleFor(book => book.Publisher).NotEmpty().WithMessage("Publisher is required.");
            RuleFor(book => book.Isbn).NotEmpty().WithMessage("Isbn is required.");
            RuleFor(book => book.Categories).NotEmpty().WithMessage("Category is required.");
            RuleFor(book => book.Status).IsInEnum().WithMessage("Incorrect book status.");
            RuleFor(book => book.CoverType).IsInEnum().WithMessage("Incorrect cover status.");
            RuleFor(book => book.Sinopse).NotEmpty().WithMessage("Book sinopse is required.");
            RuleFor(book => book.PageCount).NotEmpty().WithMessage("Book page count is required.");
            RuleFor(book => book.Language).NotEmpty().WithMessage("Book language is required.");
            RuleFor(book => book.Dimensions).NotEmpty().WithMessage("Book dimensions is required.");
            RuleFor(book => book.CopiesAvailable).NotNull()
            .WithMessage("Copies Avaliable is required.")
            .Custom((copiesAvailable, context) =>
            {
                if (copiesAvailable < 0)
                {
                    context.AddFailure("The value of available copies cannot be negative.");
                }
            });

        }
    }
}
