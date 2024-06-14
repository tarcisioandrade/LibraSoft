using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Author;
using LibraSoft.Core.Responses.Author;

namespace LibraSoft.Core.Interfaces
{
    public interface IAuthorHandler
    {
        public Task CreateAsync(CreateAuthorRequest request);
        public Task<Author?> GetByIdAsync(Guid id, bool asNoTracking = false);
        public Task<Author?> GetByNameAsync(string name);
        public Task DeleteAsync(Author author);
        public Task InactiveAsync(Author author);
        public Task<PagedResponse<IEnumerable<AuthorResponse>?>> GetAll(GetAllAuthorRequest request);
        public Task<bool> Update(UpdateAuthorRequest request, Author author);
    }
}
