using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;
using LibraSoft.Core.Responses.Author;
using LibraSoft.Core.Responses.Book;
using LibraSoft.Core.Responses.Category;
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
                                authorId: author.Id,
                                pageCount: request.PageCount,
                                sinopse: request.Sinopse,
                                coverType: request.CoverType,
                                dimensions: request.Dimensions,
                                language: request.Language,
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
            IQueryable<Book> query = _context.Books.Include(book => book.Categories)
                .Include(book => book.Author)
                .Include(book => book.Reviews)
                .OrderByDescending(b => b.CreatedAt);

            List<Book>? books = [];

            if (request.Status == EBookStatusFilter.Active)
            {
                query = query.Where(book => book.Status == EStatus.Active);
                
            }

            if (request.Status == EBookStatusFilter.Inactive)
            {
                query = query.Where(book => book.Status == EStatus.Inactive);

            }

            if (request.Search is not null)
            {
                var searchInBookTitle = query.Where(book => EF.Functions.ILike(book.Title, $"%{request.Search}%"));
                
                if (searchInBookTitle.IsNullOrEmpty() is false)
                {
                    query = searchInBookTitle;
                } 
                else
                {
                    var searchInBookAuthorName = query.Where(book => EF.Functions.ILike(book.Author.Name, $"%{request.Search}%"));
                    query = searchInBookAuthorName;
                }
            }

            if (request.Categories is not null)
            {
                var filterBookByCategory = query.Where(book => request.Categories.All(category => book.Categories.Any(bc => EF.Functions.ILike(bc.Title, "%" + category + "%"))));
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
                AverageRating = book.AverageRating,
                Language = book.Language,
                PageCount = book.PageCount,
                CoverType = book.CoverType,
                Dimensions = book.Dimensions,
                Sinopse = book.Sinopse,
                CreatedAt = book.CreatedAt,
                ReviewsCount = book.Reviews.Count(),
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
            IQueryable<Book> query = _context.Books.Include(b => b.Author).Include(b => b.Categories).Include(b => b.Rents).Include(r => r.Reviews);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var book = await query.FirstOrDefaultAsync(book => book.Id == id);

            return book;
        }

        public async Task<Book?> GetByIsbnAsync(string isbn, bool asNoTracking = false)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Reviews);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var book = await query.FirstOrDefaultAsync(book => book.Isbn == isbn);

            return book;
        }

        public async Task<List<Book>?> GetWithCategoriesAsync(Book book)
        {
            var categoriesId = book.Categories.Select(c => c.Id);
            var relatedBooks = await _context.Books.Include(b => b.Categories).Include(b => b.Author).Where(b => b.Id != book.Id && b.Status == EStatus.Active && b.Categories.Any(c => categoriesId.Contains(c.Id))).AsNoTracking().Take(5).ToListAsync();

            return relatedBooks;
        }

        public async Task InactiveAsync(Book book)
        {
            book.Inactive();
            await _context.SaveChangesAsync();
        }

        public async Task ReactivatedAsync(Book book)
        {
            book.Active();
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateBookHandlerRequest request, Book book)
        {
            var hasChanges = false;

            if (book.Title != request.Title)
            {
                book.UpdateTitle(request.Title);
                hasChanges = true;
            }

            if (book.Image != request.Image)
            {
                book.UpdateImage(request.Image);
                hasChanges = true;
            }

            if (book.Publisher != request.Publisher)
            {
                book.UpdatePublisher(request.Publisher);
                hasChanges = true;
            }

            if (book.PageCount != request.PageCount)
            {
                book.UpdatePageCount(request.PageCount);
                hasChanges = true;
            }

            if (book.Sinopse != request.Sinopse)
            {
                book.UpdateSinopse(request.Sinopse);
                hasChanges = true;
            }

            if (book.Language != request.Language)
            {
                book.UpdateLanguage(request.Language);
                hasChanges = true;
            }

            if (book.PublicationAt != request.PublicationAt)
            {
                book.UpdatePublicationAt(request.PublicationAt);
                hasChanges = true;
            }

            if (book.AuthorId != request.Author.Id)
            {

                book.UpdateAuthorId(request.Author.Id);
                hasChanges = true;
            }

            if (book.CopiesAvailable != request.CopiesAvailable)
            {
                book.UpdateCopiesAvailable(request.CopiesAvailable);
                hasChanges = true;
            }

            if (!book.Dimensions.Equals(request.Dimensions))
            {
                book.UpdateDimensions(request.Dimensions);
                hasChanges = true;
            }

            if (book.CoverType != request.CoverType)
            {
                book.UpdateCoverType(request.CoverType);
                hasChanges = true;
            }

            var categoryIds = request.Categories.Select(c => c.Id).ToList();
            var categories = await _context.Categories
                                           .Where(c => categoryIds.Contains(c.Id))
                                           .ToListAsync();

            if (!book.Categories.Select(c => c.Id).SequenceEqual(categoryIds))
            {
                book.UpdateCategories(categories);
                hasChanges = true;
            }


            if (hasChanges)
            {
                book.Validate();
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }

            return hasChanges;
        }

        public async Task UpdateRatingAsync(Book book)
        {

            var average = book.Reviews.Any() ? book.Reviews.Average(b => b.Rating) : 0;
            book.SetAverage(average);
            await _context.SaveChangesAsync();
        }

    }
}
