using LibraSoft.Api.Database;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Like;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class LikeHandler : ILikeHandler
    {
        private readonly AppDbContext _context;
        public LikeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Review review, Guid userId)
        {
            var like = new Like(review.Id, userId);
            review.AddLike(like);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Review review, Like like)
        {
            review.RemoveLike(like.Id);
            _context.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<Like?> GetWithUserIdAndReviewIdAsync(Guid userId, Guid reviewId)
        {
            var like = await _context.Likes.Where(l => l.UserId == userId).FirstOrDefaultAsync(l => l.ReviewId == reviewId);

            return like;
        }
    }
}
