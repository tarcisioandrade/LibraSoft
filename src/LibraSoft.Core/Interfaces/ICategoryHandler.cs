using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Category;
using LibraSoft.Core.Responses.Category;

namespace LibraSoft.Core.Interfaces
{
    public interface ICategoryHandler
    {
        public Task<Category> CreateAsync(CreateCategoryRequest request);
        public Task<Category?> GetByTitle(string title);
        public Task<Response<List<CategoryResponse>>?> GetAll();
        public Task<Category?> GetById(Guid id, bool asNoTracking = false);
    }
}
