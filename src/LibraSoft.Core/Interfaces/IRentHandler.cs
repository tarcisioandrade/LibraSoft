using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;

namespace LibraSoft.Core.Interfaces
{
    public interface IRentHandler
    {
        public Task CreateAsync(Guid userId, CreateRentRequest request, List<Book> books);
        public Task<List<Rent>?> GetRentsByUserIdAsync(Guid id);
        public Task<List<Rent>?> GetRentsByUserEmailAsync(string email);
    }

}
