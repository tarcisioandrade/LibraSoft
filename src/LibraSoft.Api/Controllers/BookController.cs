using LibraSoft.Api.Constants;
using LibraSoft.Core;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Requests.Book;
using LibraSoft.Core.Responses.Author;
using LibraSoft.Core.Responses.Book;
using LibraSoft.Core.Responses.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraSoft.Core.Requests.Category;
using LibraSoft.Core.Requests.Author;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BookController : ControllerBase
    {
        private readonly IBookHandler _bookHandler;
        private readonly ICategoryHandler _categoryHandler;
        private readonly IAuthorHandler _authorHandler;
        private readonly ICacheService _cache;
        public BookController(IBookHandler bookHandler, ICategoryHandler categoryHandler, IAuthorHandler authorHandler, ICacheService cache)
        {
            _bookHandler = bookHandler;
            _categoryHandler = categoryHandler;
            _authorHandler = authorHandler;
            _cache = cache;
        }

        [HttpPost]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var categories = new List<Category>();

            foreach (var categoryRequest in request.Categories)
            {
                var category = await _categoryHandler.GetByTitle(categoryRequest.Title);

                if (category is null)
                {
                    var newCategory = new CreateCategoryRequest { Title = categoryRequest.Title };
                    category = await _categoryHandler.CreateAsync(newCategory);
                }

                categories.Add(category);
            }

            var author = await _authorHandler.GetByNameAsync(request.Author.Name, asNoTracking: true);

            if (author is null)
            {
                var newAuthor = new CreateAuthorRequest { Name = request.Author.Name };
                author = await _authorHandler.CreateAsync(newAuthor);
            }

            var existsBookWithSameIsbn = await _bookHandler.GetByIsbnAsync(request.Isbn, asNoTracking: true);

            if (existsBookWithSameIsbn is not null)
            {
                return BadRequest(new BookIsbnAlreadyExistsError(request.Isbn));
            }

            await _bookHandler.CreateAsync(request, categories, author);

            await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Category);

            return Created();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<BookResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(string? search,
                                                string? category,
                                                EBookStatusFilter status = EBookStatusFilter.All,
                                                int pageNumber = Configuration.DefaultPageNumber,
                                                int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllBookRequest
            {
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                Categories = category?.Split(",").Select(c => c.Trim()).ToList(),
            };

            string cacheKey = $"get-all-book-{Uri.EscapeDataString(search ?? string.Empty)}-{Uri.EscapeDataString(stringToEscape: category
                ?? string.Empty)}-{status}-{pageNumber}-{pageSize}";

            var books = await _cache.GetOrCreateAsync(cacheKey, async () =>
            {
                var booksFromDb = await _bookHandler.GetAllAsync(request);

                return booksFromDb;
            }, tag: CacheTagConstants.Book);

            return Ok(books);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
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
                AverageRating = bookInData.AverageRating,
                ReviewsCount = bookInData.Reviews.Count(),
                Publisher = bookInData.Publisher,
                PublicationAt = bookInData.PublicationAt,
                Language = bookInData.Language,
                PageCount = bookInData.PageCount,
                CoverType = bookInData.CoverType,
                Dimensions = bookInData.Dimensions,
                Sinopse = bookInData.Sinopse,
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

            await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Category);

            return NoContent();
        }

        [HttpDelete("{id}/inactive")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Inactive(Guid id)
        {
            var book = await _bookHandler.GetByIdAsync(id);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            await _bookHandler.InactiveAsync(book);

            await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Category);

            return NoContent();
        }

        [HttpPost("{id}/reactivate")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Reactivate(Guid id)
        {
            var book = await _bookHandler.GetByIdAsync(id);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            await _bookHandler.ReactivatedAsync(book);

            await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Category);

            return NoContent();
        }

        [HttpGet("{id}/related")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookRelatedResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelated(Guid id)
        {
            var book = await _bookHandler.GetByIdAsync(id, asNoTracking: true);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(id));
            }

            var bookInDb = await _bookHandler.GetWithCategoriesAsync(book);

            var response = bookInDb?.Select(b => new BookRelatedResponse
            {
                Id = b.Id,
                Title = b.Title,
                Image = b.Image,
                AverageRating = b.AverageRating,
                AuthorName = b.Author.Name,
                CoverType = b.CoverType
            });

            return Ok(new Response<IEnumerable<BookRelatedResponse>?>(response));  
        }

        [HttpPut]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            var book = await _bookHandler.GetByIdAsync(request.Id);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(request.Id));
            }

            var categories = new List<Category>();

            foreach (var categoryRequest in request.Categories)
            {
                var category = await _categoryHandler.GetByTitle(categoryRequest.Title);

                if (category is null)
                {
                    var newCategory = new CreateCategoryRequest { Title = categoryRequest.Title };
                    category = await _categoryHandler.CreateAsync(newCategory);
                }

                categories.Add(category);
            }

            var author = await _authorHandler.GetByNameAsync(request.Author.Name, asNoTracking: true);

            if (author is null)
            {
                var newAuthor = new CreateAuthorRequest { Name = request.Author.Name };
                author = await _authorHandler.CreateAsync(newAuthor);
            }

            var handlerRequest = new UpdateBookHandlerRequest
            {
                Id = request.Id,
                Author = author,
                Categories = categories,
                CopiesAvailable = request.CopiesAvailable,
                CoverType = request.CoverType,
                Dimensions = request.Dimensions,
                Image = request.Image,
                Isbn = request.Isbn,
                Language = request.Language,
                PageCount = request.PageCount,
                PublicationAt = request.PublicationAt,
                Publisher = request.Publisher,
                Sinopse = request.Sinopse,
                Title = request.Title,
            };
            
            var hasChanged = await _bookHandler.UpdateAsync(handlerRequest, book);

            if (hasChanged == true)
            {
                await _cache.InvalidateCacheAsync(CacheTagConstants.Book);
                await _cache.InvalidateCacheAsync(CacheTagConstants.Category);
            }

            return NoContent();
        }
    }
}
