using LibraSoft.Api.Database;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Bag;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class BagHandler : IBagHandler
    {
        private readonly AppDbContext _context;
        public BagHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateBagRequest request, Guid userId)
        {
            var bag = new Bag(request.BookId, userId);
            await _context.Bags.AddAsync(bag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bag bag)
        {
            _context.Remove(bag);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Bag>> GetAllAsync(Guid userId)
        {
            var bags = await _context.Bags.Where(b => b.UserId == userId).Include(b => b.Book).ThenInclude(b => b.Author).AsNoTracking().ToListAsync();
            return bags;
        }

        public async Task<Bag?> GetAsync(Guid bagId)
        {
            var bag = await _context.Bags.FirstOrDefaultAsync(b => b.Id == bagId);
            return bag;
        }
    }
}
