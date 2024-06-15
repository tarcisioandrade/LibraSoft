using LibraSoft.Api.Database;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class RentHandler : IRentHandler
    {
        private readonly AppDbContext _context;

        public RentHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Guid userId, CreateRentRequest request, List<Book> books)
        {
            // Quantidade de dias para devolver o livro;
            var rentalDays = 7;

            DateTime returnDate = CalculateReturnDate(request.RentDate, rentalDays);

            var handlerRequest = new CreateRentRequestHandler
            {
                RentDate = request.RentDate,
                ReturnDate = returnDate,
                Books = books
            };

            var rent = new Rent(rentDate: handlerRequest.RentDate, returnDate: handlerRequest.ReturnDate, userId: userId, books: books);

            await _context.Rents.AddAsync(rent);
            await _context.SaveChangesAsync();
        }
        private static DateTime CalculateReturnDate(DateTime rentDate, int rentalDays)
        {
            int daysAdded = 0;
            DateTime returnDate = rentDate;

            while (daysAdded < rentalDays)
            {
                returnDate = returnDate.AddDays(1);

                if (returnDate.DayOfWeek != DayOfWeek.Saturday && returnDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysAdded++;
                }
            }

            return returnDate;
        }

        public async Task<List<Rent>?> GetRentsByUserIdAsync(Guid id)
        {
            var rents = await _context.Rents.AsNoTracking().Where(r => r.UserId == id).Include(r => r.Books).ToListAsync();

            return rents;
        }

        public async Task<List<Rent>?> GetRentsByUserEmailAsync(string email)
        {
            var rents = await _context.Rents.AsNoTracking().Where(r => r.User.Email == email).Include(r => r.Books).ToListAsync();

            return rents;
        }
    }
}
