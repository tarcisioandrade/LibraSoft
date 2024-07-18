using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Review;
using LibraSoft.Core.Responses.Review;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class ReviewHandler : IReviewHandler
    {
        private AppDbContext _context;
        public ReviewHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateReviewRequest request, Guid userId)
        {
            var review = new Review(title: request.Title, comment: request.Comment, rating: request.Rating, bookId: request.BookId, userId);
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<Review?> GetByUserAndBookIdAsync(Guid userId, Guid bookId)
        {
            var review = await _context.Reviews.Include(r => r.User).Include(r => r.Likes).FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

            return review;
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            var review = await _context.Reviews.Include(r => r.User).Include(r => r.Likes).FirstOrDefaultAsync(r => r.Id == id);

            return review;
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<IEnumerable<ReviewResponse>?>> GetAllAsync(GetAllReviewRequest request)
        {
            var query = _context.Reviews.Include(r => r.User)
                                                .Include(r => r.Likes)
                                                .Where(r => r.BookId == request.BookId)
                                                .AsNoTracking();
            var data = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                                .Take(request.PageSize)
                                                .OrderByDescending(r => r.CreatedAt)
                                                .ToListAsync();

            var reviews =
                    data.Select(r =>
                    new ReviewResponse { Id = r.Id, Comment = r.Comment, Title = r.Title, Rating = r.Rating, Author = r.User.Name, CreatedAt = r.CreatedAt, LikesCount = r.LikesCount });

            var count = await query.CountAsync();

            return new PagedResponse<IEnumerable<ReviewResponse>?>(reviews, count, request.PageNumber, request.PageSize);
        }
    }
}
