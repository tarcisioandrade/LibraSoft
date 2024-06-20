using System.Security.Claims;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using LibraSoft.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RentController : ControllerBase
    {
        private readonly IRentHandler _renthandler;
        private readonly IBookHandler _bookhandler;
        private readonly IUserHandler _userhandler;

        public RentController(IRentHandler rentHandler, IBookHandler bookHandler, IUserHandler userhandler)
        {
            _renthandler = rentHandler;
            _bookhandler = bookHandler;
            _userhandler = userhandler;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public async Task<IActionResult> Create(CreateRentRequest req)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userhandler.GetByIdAsync(userId);

            if (user!.PunishmentsDetails.Count > 0)
            {
                foreach (var punishment in user.PunishmentsDetails)
                {
                    if (punishment.Status == EStatus.Active && punishment.PunishEndDate < DateTime.UtcNow)
                    {
                        punishment.Inactive();
                        user.Active();
                    }
                }
            }

            if (user!.Status != EUserStatus.Active)
            {
                return BadRequest(new UserHasPunishmentError(user.Status.ToString()));
            }

            if (req.Books.Count > 5)
            {
                return BadRequest(new RentBookLimitExceededError());
            }

            var userRents = await _renthandler.GetRentsByUserIdAsync(userId);
            
            var booksInRents = userRents?.Aggregate(0, (acc, ur) => acc + ur.BooksRented());

            if (userRents?.Count == 5 || (booksInRents + req.Books.Count) > 5)
            {
                return BadRequest(new RentBookLimitExceededError());
            }

            List<Book> books = [];

            foreach (var bookReq in req.Books)
            {
                if (userRents != null && userRents.Count > 0)
                {
                    var booksRented = userRents.SelectMany(ur => ur.Books).ToList();
                    var bookAlreadyRent = booksRented.FirstOrDefault(b => b.Equal(bookReq.Id));

                    if (bookAlreadyRent is not null)
                    {
                        return BadRequest(new BookAlreadyRented(bookAlreadyRent.Id));
                    }
                }

                var book = await _bookhandler.GetByIdAsync(bookReq.Id);

                if (book is null)
                {
                    return BadRequest(new BookNotFoundError(bookReq.Id));
                }

                if (book.HasCopiesAvaliable() is false)
                {
                    return BadRequest(new NoBookCopiesAvaliableError(book.Title));
                }

                if (book.Status == EStatus.Inactive)
                {
                    return BadRequest(new BookInactiveError());
                }

                book.DecreaseNumberOfCopies();

                books.Add(book);
            }

            await _renthandler.CreateAsync(userId, req, books);

            return Created();
        }

        [HttpPost("{id}/return")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Return(Guid id)
        {
            var rent = await _renthandler.GetByIdAsync(id);

            if (rent is null)
            {
                return BadRequest(new RentNotFoundError(id));
            }

            await _renthandler.ReturnRent(rent);

            return NoContent();
        }
    }
}
