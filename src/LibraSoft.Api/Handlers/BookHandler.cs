using LibraSoft.Api.Data;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class BookHandler : IBookHandler
    {
        private readonly AppDbContext _context;
        public BookHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateBookRequest request)
        {
            var categories = new List<Category>();

            foreach(var categoryRequest in request.Categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryRequest.Id);

                if (category is null)
                {
                    throw new CategoryNotFoundError(categoryRequest.Title);
                }
                categories.Add(category);
            }

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == request.AuthorId);

            if (author is null)
            {
                throw new AuthorNotFoundError(request.AuthorId.ToString());
            }

            var book = new Book(title: request.Title,
                                publisher: request.Publisher,
                                isbn: request.Isbn,
                                publicationAt: request.PublicationAt,
                                categories: categories,
                                copiesAvailable: request.CopiesAvailable,
                                authorId: request.AuthorId);

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetByIDAsync(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);

            return book;
        }
    }
}
