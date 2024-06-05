using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;

namespace LibraSoft.Core.Interfaces
{
    public interface IRentHandler
    {
        public Task CreateAsync(CreateRentRequest request, List<Book> books);
    }

}
