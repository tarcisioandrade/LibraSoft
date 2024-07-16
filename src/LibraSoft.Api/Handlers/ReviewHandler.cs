using LibraSoft.Api.Database;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Review;
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

        public async Task<List<Review>?> GetAllAsync(Guid bookId)
        {
            var reviews = await _context.Reviews.Include(r => r.User).Include(r => r.Likes).Where(r => r.BookId == bookId).Take(5).OrderByDescending(r => r.CreatedAt).ToListAsync();

            return reviews;
        }
    }
}
