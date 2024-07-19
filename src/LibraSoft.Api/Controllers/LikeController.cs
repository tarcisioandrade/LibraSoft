using System.Security.Claims;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Like;
using LibraSoft.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Responses.Like;
using LibraSoft.Api.Constants;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeHandler _likeHandler;
        private readonly IReviewHandler _reviewHandler;
        private readonly ICacheService _cache;

        public LikeController(ILikeHandler likeHandler, IReviewHandler reviewHandler, ICacheService cache)
        {
            _likeHandler = likeHandler;
            _reviewHandler = reviewHandler;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLikeRequest request)
        {
            var review = await _reviewHandler.GetByIdAsync(request.ReviewId);

            if (review is null)
            {
                return BadRequest(new ReviewNotFound());
            }
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _likeHandler.CreateAsync(review, userId);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Review);

            return Ok(new { review.LikesCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var like = await _likeHandler.GetWithUserIdAndReviewIdAsync(userId, id);

            if (like is null)
            {
                return NoContent();
            }

            return Ok(new Response<Like>(like));
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(Guid reviewId)
        {
            var review = await _reviewHandler.GetByIdAsync(reviewId);

            if (review is null)
            {
                return BadRequest(new ReviewNotFound());
            }

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var like = await _likeHandler.GetWithUserIdAndReviewIdAsync(userId, reviewId);

            if (like is null)
            {
                return BadRequest();
            }

            await _likeHandler.DeleteAsync(review, like);
            await _cache.InvalidateCacheAsync(CacheTagConstants.Review);

            return Ok(new Response<LikeCountResponse>(new LikeCountResponse { LikesCount = review.LikesCount }));
        }

    }
}
