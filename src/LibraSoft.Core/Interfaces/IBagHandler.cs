using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Bag;

namespace LibraSoft.Core.Interfaces
{
    public interface IBagHandler
    {
        public Task CreateAsync(CreateBagRequest request, Guid userId);
        public Task<List<Bag>> GetAllAsync(Guid userId);
        public Task<Bag?> GetAsync(Guid bagId);
        public Task DeleteAsync(Bag bag);
    }
}
