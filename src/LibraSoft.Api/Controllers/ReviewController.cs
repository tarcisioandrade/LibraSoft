using System.Security.Claims;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Review;
using LibraSoft.Core.Responses.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewHandler _reviewHandler;
        private readonly IBookHandler _bookHandler;

        public ReviewController(IReviewHandler reviewHandler, IBookHandler bookHandler)
        {
            _reviewHandler = reviewHandler;
            _bookHandler = bookHandler;
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

            return NoContent();
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetAll(Guid bookId)
        {
            var reviews = await _reviewHandler.GetAllAsync(bookId);

            if (reviews is null)
            {
                return Ok(new Response<List<string>>([]));
            }
            var response = 
                new Response<IEnumerable<ReviewResponse>>(
                    reviews.Select(r => 
                    new ReviewResponse { Id = r.Id, Comment = r.Comment, Title = r.Title, Rating = r.Rating, Author = r.User.Name, CreatedAt = r.CreatedAt, LikesCount = r.LikesCount }));

            return Ok(response);
        }

        [HttpGet("{bookId}/user")]
        [Authorize]
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
