﻿using System.Security.Claims;
using LibraSoft.Api.Constants;
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
        private readonly ICacheService _cache;

        public RentController(IRentHandler rentHandler, IBookHandler bookHandler, IUserHandler userhandler, ICacheService cache)
        {
            _renthandler = rentHandler;
            _bookhandler = bookHandler;
            _userhandler = userhandler;
            _cache = cache;
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

                books.Add(book);
            }

            await _renthandler.CreateAsync(userId, req, books);
            return Created();
        }

        [HttpPost("{id}/{actionType}")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Return(Guid id, ERentAction actionType)
        {
            var rent = await _renthandler.GetByIdAsync(id);
            if (rent is null) return BadRequest(new RentNotFoundError(id));

            switch (actionType)
            {
                case ERentAction.Delete:
                    if (rent.Status == ERentStatus.Rent_Canceled) return BadRequest(new RentAlreadyCanceled(id));
                    await _renthandler.CancelAsync(rent);
                    break;
                case ERentAction.Return:
                    await _renthandler.ReturnAsync(rent);
                    await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
                    break;
                case ERentAction.Confirm:
                    await _renthandler.ConfirmAsync(rent);
                    await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
                    break;

                default:
                    return BadRequest("Invalid action type.");
            }

            return NoContent();
        }

        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<RentResponse>?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOfUser(EQueryRentStatus status = EQueryRentStatus.All,
                                                      int pageNumber = Configuration.DefaultPageNumber,
                                                      int pageSize = Configuration.DefaultPageSize)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var request = new GetAllUserRentRequest
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
                }),
                User = new UserInRent { Id = rent.User.Id, Email = rent.User.Email, Name = rent.User.Name },
            });
            return Ok(response);
        }

        [HttpGet]
        [Authorize("admin")]
        [ProducesResponseType(typeof(Response<RentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(ERentStatus? status,
                                                string? search,
                                                int pageNumber = Configuration.DefaultPageNumber,
                                                int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllRentRequest { Status = status, SearchEmail = search, PageNumber = pageNumber, PageSize = pageSize };  
            var rent = await _renthandler.GetAllAsync(request);

            return Ok(rent);
        }
    }
}
