using LibraSoft.Core;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;
using LibraSoft.Core.Responses.Book;
using LibraSoft.Core.Responses.Author;
using LibraSoft.Core.Responses.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("admin")]
    public class BookController : ControllerBase
    {
        private readonly IBookHandler _bookHandler;
        private readonly ICategoryHandler _categoryHandler;
        private readonly IAuthorHandler _authorHandler;
        public BookController(IBookHandler bookHandler, ICategoryHandler categoryHandler, IAuthorHandler authorHandler)
        {
            _bookHandler = bookHandler;
            _categoryHandler = categoryHandler;
            _authorHandler = authorHandler;
        }

        [HttpPost]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var categories = new List<Category>();

            foreach (var categoryRequest in request.Categories)
            {
                var category = await _categoryHandler.GetById(categoryRequest.Id);

                if (category is null)
                {
                    return BadRequest(new CategoryNotFoundError(categoryRequest.Title));
                }
                categories.Add(category);
            }

            var author = await _authorHandler.GetByIdAsync(request.AuthorId, asNoTracking: true);

            if (author is null)
            {
                return BadRequest(new AuthorNotFoundError(request.AuthorId.ToString()));
            }

            var existsBookWithSameIsbn = await _bookHandler.GetByIsbnAsync(request.Isbn, asNoTracking: true);

            if (existsBookWithSameIsbn is not null)
            {
                return BadRequest(new BookIsbnAlreadyExistsError(request.Isbn));
            }

            await _bookHandler.CreateAsync(request, categories, author);

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<BookResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(string? search,
                                                string? category,
                                                bool includeInactive = false,
                                                int pageNumber = Configuration.DefaultPageNumber,
                                                int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllBookRequest
            {
                IncludeInactive = includeInactive,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                Category = category,
            };

            var books = await _bookHandler.GetAllAsync(request);

            return Ok(books);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            var bookInData = await _bookHandler.GetByIdAsync(id, asNoTracking: true);

            if (bookInData is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            var book = new BookResponse
            {
                Id = bookInData.Id,
                Title = bookInData.Title,
                Image = bookInData.Image,
                Isbn = bookInData.Isbn,
                CopiesAvaliable = bookInData.CopiesAvailable,
                Publisher = bookInData.Publisher,
                PublicationAt = bookInData.PublicationAt,
                Author = new AuthorResponse
                {
                    Id = bookInData.Author.Id,
                    Name = bookInData.Author.Name,
                    Status = bookInData.Author.Status,
                    Biography = bookInData.Author.Biography,
                    DateBirth = bookInData.Author.DateBirth
                },
                Categories = bookInData.Categories.Select(c => new CategoryResponse { Id = c.Id, Title = c.Title }),
                Status = bookInData.Status
            };

            var response = new Response<BookResponse>(book);

            return Ok(response);
        }

        [HttpDelete("{id}/delete")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _bookHandler.GetByIdAsync(id);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            if (book.HasRent())
            {
                return BadRequest(new BookHasRentAssociatedError(book.Title));
            }

            await _bookHandler.DeleteAsync(book);

            return NoContent();
        }

        [HttpDelete("{id}/inactive")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Inactive(Guid id)
        {
            var book = await _bookHandler.GetByIdAsync(id);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            await _bookHandler.InactiveAsync(book);

            return NoContent();
        }
    }
}
