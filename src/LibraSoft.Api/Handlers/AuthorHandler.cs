using LibraSoft.Api.Data;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Author;
using LibraSoft.Core.Responses.Author;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class AuthorHandler : IAuthorHandler

    {
        private readonly AppDbContext _context;

        public AuthorHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateAuthorRequest request)
        {
            var author = new Author(name: request.Name, biography: request.Biography, dateBirth: request.DateBirth);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<IEnumerable<AuthorResponse>?>> GetAll(GetAllAuthorRequest request)
        {
            var query = _context.Authors.Where(author => author.Status == EStatus.Active);

            List<Author>? authors = [];

            if (request.IncludeInactive is true)
            {
                query = _context.Authors;
            }

            if(request.Search is not null)
            {
                query = query.Where(author => EF.Functions.ILike(author.Name, $"%{request.Search}%"));
            }

            authors = await query.AsNoTracking().Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var count = await query.CountAsync();

            var data = authors.Select(author => new AuthorResponse
            {
                Id = author.Id,
                Biography = author.Biography,
                Name = author.Name,
                Status = author.Status,
                DateBirth = author.DateBirth
            });

            return new PagedResponse<IEnumerable<AuthorResponse>?>(data, count, request.PageNumber, request.PageSize);
        }

        public async Task<Author?> GetByIdAsync(Guid id, bool asNoTracking = false)
        {
            IQueryable<Author> query = _context.Authors;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var author = await query.Include(author => author.Books).FirstOrDefaultAsync(author => author.Id == id);

            return author;
        }

        public async Task<Author?> GetByNameAsync(string name)
        {
            var author = await _context.Authors.Include(author => author.Books).FirstOrDefaultAsync(author => author.Name == name);

            return author;
        }

        public async Task InactiveAsync(Author author)
        {
            author.Inactive();
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(UpdateAuthorRequest request, Author author)
        {
            bool updated = false;

            var requestesProperties = typeof(UpdateAuthorRequest).GetProperties();

            foreach (var property in requestesProperties)
            {
                var newValue = property.GetValue(request);

                if (newValue != null)
                {
                    var userProperty = typeof(Author).GetProperty(property.Name);
                    if (userProperty != null)
                    {
                        var currentValue = userProperty.GetValue(author);

                        if (currentValue.Equals(newValue) is false)
                        {
                            userProperty.SetValue(author, newValue);
                            updated = true;
                        }
                    }
                }
            }

            author.Validate();

            if (updated)
            {
                await _context.SaveChangesAsync();
            }

            return updated;
        }
    }
}
