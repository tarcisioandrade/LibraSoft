using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;

namespace LibraSoft.Core.Interfaces
{
    public interface IBookHandler
    {
        public Task CreateAsync(CreateBookRequest request);
        public Task<Book?> GetByIDAsync(Guid id);
    }
}
