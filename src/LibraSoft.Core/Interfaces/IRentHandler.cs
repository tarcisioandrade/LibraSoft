using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using LibraSoft.Core.Responses.Rent;

namespace LibraSoft.Core.Interfaces
{
    public interface IRentHandler
    {
        public Task CreateAsync(Guid userId, CreateRentRequest request, List<Book> books);
        public Task<Rent?> GetByIdAsync(Guid id);
        public Task ReturnAsync(Rent rent);
        public Task CancelAsync(Rent rent);
        public Task ConfirmAsync(Rent rent);
        public Task<PagedResponse<IEnumerable<RentResponse>?>> GetAllByUserIdAsync(GetAllUserRentRequest request, Guid id);
        public Task<List<Rent>?> GetAllByUserEmailAsync(string email);
        public Task<PagedResponse<IEnumerable<RentResponse>?>> GetAllAsync(GetAllRentRequest request);
    }
}
