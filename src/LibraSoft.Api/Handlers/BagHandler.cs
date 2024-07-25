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

        public async Task<List<Bag>?> GetAllAsync(Guid userId)
        {
            var bags = await _context.Bags.Where(b => b.UserId == userId).AsNoTracking().ToListAsync();

            return bags;
        }
    }
}
