using System.Security.Claims;
using LibraSoft.Core;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Rent;
using LibraSoft.Core.Responses.Rent;
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
            var LIMIT_TO_BOOKS_RENT = 3;

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

            if (req.Books.Count > LIMIT_TO_BOOKS_RENT)
            {
                return BadRequest(new RentBookLimitExceededError());
            }

            var userRents = user!.Rents;
            
            var booksInRents = userRents?.Aggregate(0, (acc, ur) => acc + ur.BooksRented());

            if (userRents?.Count() == LIMIT_TO_BOOKS_RENT || (booksInRents + req.Books.Count) > LIMIT_TO_BOOKS_RENT)
            {
                return BadRequest(new RentBookLimitExceededError());
            }

            List<Book> books = [];

            foreach (var bookReq in req.Books)
            {
                if (userRents != null && userRents?.Count() > 0)
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
            if (rent is null) return BadRequest(new RentNotFoundError(id));
            await _renthandler.ReturnAsync(rent);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<RentResponse>?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(EQueryRentStatus status = EQueryRentStatus.All,
                                                int pageNumber = Configuration.DefaultPageNumber,
                                                int pageSize = Configuration.DefaultPageSize)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var request = new GetAllRentRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status
            };
            var rents = await _renthandler.GetAllByUserIdAsync(request, userId);
            return Ok(rents);
        }

        [HttpGet(("{id}"))]
        [Authorize]
        [ProducesResponseType(typeof(Response<RentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            var rent = await _renthandler.GetByIdAsync(id);
            if (rent is null) return BadRequest(new RentNotFoundError(id));
            var response = new Response<RentResponse>(new RentResponse
            {
                Id = rent.Id,
                RentDate = rent.RentDate,
                ExpectedReturnDate = rent.ExpectedReturnDate,
                ReturnedDate = rent.ReturnedDate,
                Status = rent.Status,
                Books = rent.Books.Select(b => new BookInRent
                {
                    Id = b.Id,
                    Title = b.Title,
                    Image = b.Image,
                    CoverType = b.CoverType,
                    Author = new AuthorInBookRent { Name = b.Author.Name },
                    AverageRating = b.AverageRating,
                    Publisher = b.Publisher
                })
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var rent = await _renthandler.GetByIdAsync(id);
            if (rent is null) return BadRequest(new RentNotFoundError(id));
            if (rent.Status == ERentStatus.Rent_Canceled) return BadRequest(new RentAlreadyCanceled(id));
            await _renthandler.CancelAsync(rent);
            return NoContent();
        }

        [HttpPost("{id}/confirm")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var rent = await _renthandler.GetByIdAsync(id);
            if (rent is null) return BadRequest(new RentNotFoundError(id));
            await _renthandler.ConfirmAsync(rent);
            return NoContent();
        }
    }
}
