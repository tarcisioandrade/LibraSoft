using LibraSoft.Api.Data;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Requests.User;
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

        public async Task CreateAsync(CreateUserRequest request, bool isAdmin = false)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(name: request.Name,
                                address: request.Address,
                                email: request.Email,
                                password: passwordHash,
                                telephone: request.Telephone,
                                role: isAdmin ? EUserRole.Admin : EUserRole.Common);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user = await GetByIdAsync(new GetByIdRequest { Id = request.Id});
            _context.Users.Remove(user!);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(GetByEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

            return user;
        }

        public async Task<User?> GetByIdAsync(GetByIdRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == request.Id);
            return user;
        }
    }
}
