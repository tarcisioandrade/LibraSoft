using LibraSoft.Api.Data;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;
using LibraSoft.Core.Responses.Book;
using LibraSoft.Core.Responses.Category;
using LibraSoft.Core.Responses.Author;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraSoft.Api.Handlers
{
    public class BookHandler : IBookHandler
    {
        private readonly AppDbContext _context;
        public BookHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateBookRequest request, List<Category> categories, Author author)
        {
            var book = new Book(title: request.Title,
                                publisher: request.Publisher,
                                isbn: request.Isbn,
                                publicationAt: request.PublicationAt,
                                categories: categories,
                                copiesAvailable: request.CopiesAvailable,
                                authorId: request.AuthorId,
                                image: request.Image);

            await _context.Books.AddAsync(book);    
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<IEnumerable<BookResponse>?>> GetAllAsync(GetAllBookRequest request)
        {
            IQueryable<Book> query = _context.Books.Include(book => book.Categories).Include(book => book.Author);

            List<Book>? books = [];

            if (request.IncludeInactive is false)
            {
                query = query.Where(book => book.Status == EStatus.Active);
                
            }

            if (request.Search is not null)
            {
                var searchInBookTitle = query.Where(book => EF.Functions.ILike(book.Title, $"%{request.Search}%"));
                var searchInBookAuthorName = query.Include(book => book.Author).Where(book => EF.Functions.ILike(book.Author.Name, $"%{request.Search}%"));
                
                if (searchInBookTitle.IsNullOrEmpty() is false)
                {
                    query = searchInBookTitle;
                } 
                else
                {
                    query = searchInBookAuthorName;
                }
            }

            if (request.Category is not null)
            {
                var filterBookByCategory = query.Where(book => book.Categories.Any(c => EF.Functions.ILike(c.Title, $"%{request.Category}%")));
                
                query = filterBookByCategory;
            }

            books = await query.AsNoTracking().Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var count = await query.CountAsync();

            var data = books.Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Image = book.Image,
                Isbn = book.Isbn,
                CopiesAvaliable = book.CopiesAvailable,
                Publisher = book.Publisher,
                PublicationAt = book.PublicationAt,
                Author = new AuthorResponse
                {
                    Id = book.Author.Id,
                    Name = book.Author.Name,
                    Status = book.Author.Status,
                    Biography = book.Author.Biography,
                    DateBirth = book.Author.DateBirth
                },
                Categories = book.Categories.Select(c => new CategoryResponse { Id = c.Id, Title = c.Title }),
                Status = book.Status
            });

            return new PagedResponse<IEnumerable<BookResponse>?>(data, count, request.PageNumber, request.PageSize);
        }

        public async Task<Book?> GetByIdAsync(Guid id, bool asNoTracking = false)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Author).Include(b => b.Categories).Include(b => b.Rents);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var book = await query.FirstOrDefaultAsync(book => book.Id == id);

            return book;
        }

        public async Task<Book?> GetByIsbnAsync(string isbn, bool asNoTracking = false)
        {
            IQueryable<Book> query = _context.Books;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var book = await query.FirstOrDefaultAsync(book => book.Isbn == isbn);

            return book;
        }

        public async Task InactiveAsync(Book book)
        {
            book.Inactive();
            await _context.SaveChangesAsync();
        }
    }
}
