using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Author;
using LibraSoft.Core.Responses.Author;

namespace LibraSoft.Core.Interfaces
{
    public interface IAuthorHandler
    {
        public Task CreateAsync(CreateAuthorRequest request);
        public Task<Author?> GetByIdAsync(Guid id);
        public Task<Author?> GetByNameAsync(string name);
        public Task DeleteAsync(Author author);
        public Task InactiveAsync(Author author);
        public Task<PagedResponse<IEnumerable<GetAllAuthorResponse>?>> GetAll(GetAllAuthorRequest request);
    }
}
