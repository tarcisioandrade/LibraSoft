﻿using FluentValidation;

namespace LibraSoft.Core.Models.Validations
{
    public class BookValidate : AbstractValidator<Book>
    {
        public BookValidate()
        {
            RuleFor(book => book.Title).NotEmpty().WithMessage("Book title is required.");
            RuleFor(book => book.AuthorId).NotEmpty().WithMessage("Author Id is required.");
            RuleFor(book => book.Publisher).NotEmpty().WithMessage("Publisher is required.");
            RuleFor(book => book.Isbn).NotEmpty().WithMessage("Isbn is required.");
            RuleFor(book => book.Categories).NotEmpty().WithMessage("Category is required.");
            RuleFor(book => book.CopiesAvailable).NotEmpty()
            .WithMessage("Copies Avaliable is required.")
            .LessThan(0).WithMessage("The value of available copies cannot be negative.");
            RuleFor(book => book.Status).IsInEnum().WithMessage("Incorrect book status.");
        }
    }
}
