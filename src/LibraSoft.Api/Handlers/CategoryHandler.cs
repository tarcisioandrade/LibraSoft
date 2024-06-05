using LibraSoft.Api.Data;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Category;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class CategoryHandler : ICategoryHandler
    {
        readonly private AppDbContext _context;

        public CategoryHandler(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task CreateAsync(CreateCategoryRequest request)
        {
            var category = new Category(request.Title);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>?> GetAll()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();

            return categories;
        }

        public async Task<Category?> GetByTitle(string title)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Title == title);

            return category;
        }

    }
}
