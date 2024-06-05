using LibraSoft.Api.Data;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Author;
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

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(author => author.Id == id);

            return author;
        }

        public async Task<Author?> GetByNameAsync(string name)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(author => author.Name == name);

            return author;
        }
    }
}
