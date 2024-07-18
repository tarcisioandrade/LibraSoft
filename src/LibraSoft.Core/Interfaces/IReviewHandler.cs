using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Review;
using LibraSoft.Core.Responses.Review;

namespace LibraSoft.Core.Interfaces
{
    public interface IReviewHandler
    {
        public Task CreateAsync(CreateReviewRequest request, Guid userId);
        public Task<Review?> GetByUserAndBookIdAsync(Guid userId, Guid bookId);
        public Task<Review?> GetByIdAsync(Guid id);
        public Task DeleteAsync(Review review);
        public Task<PagedResponse<IEnumerable<ReviewResponse>?>> GetAllAsync(GetAllReviewRequest request);
    }
}
