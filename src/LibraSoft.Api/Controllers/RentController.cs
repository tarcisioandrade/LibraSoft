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
    [Authorize("admin")]
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
            List<Book> books = [];

            foreach (var bookReq in req.Books)
            {
                var book = await _bookhandler.GetByIDAsync(bookReq.Id);

                if (book is null)
                {
                    throw new BookNotFoundError(bookReq.Id.ToString());
                }

                books.Add(book);
            }

            await _renthandler.CreateAsync(req, books);

            return Created();
        }
    }
}
