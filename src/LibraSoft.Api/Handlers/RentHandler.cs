using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using LibraSoft.Core.Responses.Rent;
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
            var amountDaysToReturnBook = 30;

            DateTime returnDate = CalculateReturnDate(request.RentDate, amountDaysToReturnBook);

            var handlerRequest = new CreateRentRequestHandler
            {
                RentDate = request.RentDate,
                ExpectedReturnDate = returnDate,
                Books = books
            };

            var rent = new Rent(rentDate: handlerRequest.RentDate, expectedReturnDate: handlerRequest.ExpectedReturnDate, userId: userId, books: books);

            await _context.Rents.AddAsync(rent);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<IEnumerable<RentResponse>?>> GetAllByUserIdAsync(GetAllUserRentRequest request, Guid id)
        {
            IQueryable<Rent> query = _context.Rents.AsNoTracking().Where(r => r.UserId == id).Include(r => r.User).Include(r => r.Books).ThenInclude(b => b.Author);

           if (request.Status == EQueryRentStatus.Pending)
            {
                query = query.Where(r => r.Status != ERentStatus.Rent_Finished && r.Status != ERentStatus.Rent_Canceled);
            }

            if (request.Status == EQueryRentStatus.Complete)
            {
                query = query.Where(r => r.Status == ERentStatus.Rent_Finished || r.Status == ERentStatus.Rent_Canceled);
            }

            var result = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).OrderByDescending(r => r.RentDate).ToListAsync();
            var count = await query.CountAsync();

            var rents = result?.Select(r => new RentResponse
            {
                Id = r.Id,
                RentDate = r.RentDate,
                ExpectedReturnDate = r.ExpectedReturnDate,
                ReturnedDate = r.ReturnedDate,
                Status = r.Status,
                Books = r.Books.Select(b => new BookInRent
                {
                    Id = b.Id,
                    Title = b.Title,
                    Image = b.Image,
                    CoverType = b.CoverType,
                    Author = new AuthorInBookRent { Name = b.Author.Name },
                    AverageRating = b.AverageRating,
                    Publisher = b.Publisher
                }),
                User = new UserInRent { Id = r.User.Id, Email = r.User.Email, Name = r.User.Name },
            });

            return new PagedResponse<IEnumerable<RentResponse>?>(rents, count, request.PageNumber, request.PageSize);
        }

        public async Task<List<Rent>?> GetAllByUserEmailAsync(string email)
        {
            var rents = await _context.Rents.AsNoTracking().Where(r => r.User.Email == email).Include(r => r.Books).ToListAsync();

            return rents;
        }

        public async Task<Rent?> GetByIdAsync(Guid id)
        {
            var rent = await _context.Rents.Where(r => r.Id == id).Include(r => r.User).Include(r => r.Books).ThenInclude(b => b.Author).FirstOrDefaultAsync();
            return rent;
        }

        public async Task ReturnAsync(Rent rent)
        {
            rent.SetFinished();
            rent.SetReturnedDate(DateTime.UtcNow);

            foreach (var book in rent.Books)
            {
                book.IncreaseNumberOfCopies();
            }

            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(Rent rent)
        {
            rent.SetCanceled();
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmAsync(Rent rent)
        {
            rent.SetInProgress();
            foreach (var book in rent.Books)
            {
                book.DecreaseNumberOfCopies();
            }
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<IEnumerable<RentResponse>?>> GetAllAsync(GetAllRentRequest request)
        {
            IQueryable<Rent> query = _context.Rents.AsNoTracking().Include(r => r.User).Include(r => r.Books).ThenInclude(b => b.Author);

            if (request.SearchEmail is not null)
            {
                query = query.Where(r => EF.Functions.ILike(r.User.Email, $"%{request.SearchEmail}%"));
            }

            if (request.Status is not null)
            {
                query = query.Where(r => r.Status == request.Status);
            }

            var result = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).OrderByDescending(r => r.RentDate).ToListAsync();
            var count = await query.CountAsync();

            var rents = result?.Select(r => new RentResponse
            {
                Id = r.Id,
                RentDate = r.RentDate,
                ExpectedReturnDate = r.ExpectedReturnDate,
                ReturnedDate = r.ReturnedDate,
                Status = r.Status,
                Books = r.Books.Select(b => new BookInRent
                {
                    Id = b.Id,
                    Title = b.Title,
                    Image = b.Image,
                    CoverType = b.CoverType,
                    Author = new AuthorInBookRent { Name = b.Author.Name },
                    AverageRating = b.AverageRating,
                    Publisher = b.Publisher
                }),
                User = new UserInRent { Id = r.User.Id, Email = r.User.Email, Name = r.User.Name },
            });

            return new PagedResponse<IEnumerable<RentResponse>?>(rents, count, request.PageNumber, request.PageSize);
        }

        private static DateTime CalculateReturnDate(DateTime rentDate, int amountDaysToReturnBook)
        {
            int daysAdded = 0;
            DateTime returnDate = rentDate;

            while (daysAdded < amountDaysToReturnBook)
            {
                returnDate = returnDate.AddDays(1);

                if (returnDate.DayOfWeek != DayOfWeek.Saturday && returnDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysAdded++;
                }
            }

            return returnDate;
        }
    }
}
