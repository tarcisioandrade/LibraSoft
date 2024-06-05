using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Author;

namespace LibraSoft.Core.Interfaces
{
    public interface IAuthorHandler
    {
        public Task CreateAsync(CreateAuthorRequest request);
        public Task<Author?> GetByIdAsync(Guid id);
        public Task<Author?> GetByNameAsync(string name);

    }
}
