using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Category;

namespace LibraSoft.Core.Interfaces
{
    public interface ICategoryHandler
    {
        public Task CreateAsync(CreateCategoryRequest request);
        public Task<Category?> GetByTitle(string title);
        public Task<List<Category>?> GetAll();
        public Task<Category?> GetById(Guid id, bool asNoTracking = false);
    }
}
