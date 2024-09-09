using LibraSoft.Api.Database;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Requests.Category;
using LibraSoft.Core.Responses.Category;
using Microsoft.EntityFrameworkCore;
using LibraSoft.Core.Commons;

namespace LibraSoft.Api.Handlers
{
    public class CategoryHandler : ICategoryHandler
    {
        readonly private AppDbContext _context;

        public CategoryHandler(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Category> CreateAsync(CreateCategoryRequest request)
        {
            var category = new Category(request.Title);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Response<List<CategoryResponse>>?> GetAll()
        {
            var categories = await _context.Categories.Include(c => c.Books).Where(c => c.Books.Any(b => b.Status == EStatus.Active)).AsNoTracking().ToListAsync();

            var response = new Response<List<CategoryResponse>>(categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Title = category.Title
            }).OrderBy(c => c.Title).ToList());

            return response;
        }

        public Task<Category?> GetById(Guid id, bool asNoTracking = false)
        {
            IQueryable<Category> query = _context.Categories;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var category = query.FirstOrDefaultAsync(categ => categ.Id == id);

            return category;
        }

        public async Task<Category?> GetByTitle(string title)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Title == title);

            return category;
        }

    }
}
