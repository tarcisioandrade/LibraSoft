using LibraSoft.Core.Models;

namespace LibraSoft.Core.Interfaces
{
    public interface ILikeHandler
    {
        public Task CreateAsync(Review review, Guid userId);
        public Task<Like?> GetWithUserIdAndReviewIdAsync(Guid userId, Guid reviewId);
        public Task DeleteAsync(Review review, Like like); 
    }
}
