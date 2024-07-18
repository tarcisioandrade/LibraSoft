using System.Security.Claims;
using LibraSoft.Api.Constants;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Review;
using LibraSoft.Core.Responses.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewHandler _reviewHandler;
        private readonly IBookHandler _bookHandler;
        private readonly ICacheService _cache;

        public ReviewController(IReviewHandler reviewHandler, IBookHandler bookHandler, ICacheService cache)
        {
            _reviewHandler = reviewHandler;
            _bookHandler = bookHandler;
            _cache = cache;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var exists = await _reviewHandler.GetByUserAndBookIdAsync(userId, request.BookId);

            if (exists is not null) {
                return BadRequest(new ReviewAlreadyCreated());
            }

            var book = await _bookHandler.GetByIdAsync(request.BookId);

            if (book is null)
            {
                return BadRequest(new BookNotFoundError(request.BookId));
            }

            await _reviewHandler.CreateAsync(request, userId);

            await _bookHandler.UpdateBookRatingAsync(book);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Review);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var review = await _reviewHandler.GetByIdAsync(id);

            if (review is null)
            {
                return BadRequest(new ReviewNotFound());
            }

            await _reviewHandler.DeleteAsync(review);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Review);

            return NoContent();
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<ReviewResponse>?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(Guid bookId, [FromQuery] int pageSize = 5, [FromQuery] int pageNumber = 1)
        {
            var request = new GetAllReviewRequest { BookId = bookId, PageNumber = pageNumber, PageSize = pageSize };
            string cacheKey = $"get-all-review-{bookId}-{pageNumber}-{pageSize}";

            var reviews = await _cache.GetOrCreateAsync(cacheKey, async () =>
            {
                var reviewsInDb = await _reviewHandler.GetAllAsync(request);
                return reviewsInDb;
            }, CacheTagConstants.Review);

            return Ok(reviews);
        }

        [HttpGet("{bookId}/user")]
        [Authorize]
        [ProducesResponseType(typeof(Response<ReviewResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var review = await _reviewHandler.GetByUserAndBookIdAsync(userId, bookId);
            if (review is null)
            {
                return NoContent();
            }
            var response = new Response<ReviewResponse>(new ReviewResponse { Id = review.Id, Comment = review.Comment, Title = review.Title, Rating = review.Rating, Author = review.User.Name, CreatedAt = review.CreatedAt });
            return Ok(response);
        }
    }
}
