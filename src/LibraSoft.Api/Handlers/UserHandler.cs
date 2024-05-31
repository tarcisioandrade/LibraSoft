using LibraSoft.Api.Data;
using LibraSoft.Core.Handlers;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Users;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Handlers
{
    public class UserHandler : IUserHandler
    {

        readonly private AppDbContext _context;

        public UserHandler(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task CreateAsync(CreateUserRequest request)
        {
            var user = new User(name: request.Name,
                                address: request.Address,
                                email: request.Email,
                                password: request.Password,
                                telephone: request.Telephone);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == request.Id);
            _context.Users.Remove(user!);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(GetByEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

            return user;
        }
    }
}
