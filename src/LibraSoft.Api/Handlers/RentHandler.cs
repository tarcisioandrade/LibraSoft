using LibraSoft.Api.Data;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Rent;
using LibraSoft.Core.Models;

namespace LibraSoft.Api.Handlers
{
    public class RentHandler : IRentHandler
    {
        private readonly AppDbContext _context;

        public RentHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateRentRequest request, List<Book> books)
        {
            var rent = new Rent(rentDate: request.RentDate, returnDate: request.RentDate,userId: request.UserId, books: books);
            await _context.Rents.AddAsync(rent);
            await _context.SaveChangesAsync();
        }
    }
}
