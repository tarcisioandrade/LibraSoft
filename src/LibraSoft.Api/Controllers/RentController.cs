using System.Security.Claims;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentController : ControllerBase
    {
        private readonly IRentHandler _renthandler;
        private readonly IBookHandler _bookhandler;

        public RentController(IRentHandler rentHandler, IBookHandler bookHandler)
        {
            _renthandler = rentHandler;
            _bookhandler = bookHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateRentRequest req)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (req.Books.Count > 5)
            {
                return BadRequest(new RentBookLimitExceeded());
            }

            var userRents = await _renthandler.GetRentsByUserIdAsync(userId);
            
            var booksInRents = userRents?.Aggregate(0, (acc, ur) => acc + ur.BooksRented());

            if (userRents?.Count == 5 || (booksInRents + req.Books.Count) > 5)
            {
                return BadRequest(new RentBookLimitExceeded());
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

                var book = await _bookhandler.GetByIDAsync(bookReq.Id);

                if (book is null)
                {
                    throw new BookNotFoundError(bookReq.Id.ToString());
                }

                if (book.HasCopiesAvaliable() is false)
                {
                    throw new NoBookCopiesAvaliable(book.Title);
                }

                book.DecreaseNumberOfCopies();

                books.Add(book);
            }

            await _renthandler.CreateAsync(userId, req, books);

            return Created();
        }
    }
}
