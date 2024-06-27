using LibraSoft.Api.Database;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
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
                                punishmentDetails: [],
                                email: request.Email,
                                password: passwordHash,
                                telephone: request.Telephone,
                                role: isAdmin ? EUserRole.Admin : EUserRole.Common);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user = await GetByIdAsync(request.Id);
            _context.Users.Remove(user!);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(GetByEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public async Task InactiveAsync(Guid id)
        {
            var user = await this.GetByIdAsync(id);

            user!.Inactive();

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UserUpdateRequest request, Guid id)
        {
            var user = await this.GetByIdAsync(id);

            bool updated = false;

            var requestesProperties = typeof(UserUpdateRequest).GetProperties();

            foreach (var property in requestesProperties)
            {
               var newValue = property.GetValue(request);

                if (newValue != null)
                {
                    var userProperty = typeof(User).GetProperty(property.Name);
                    if (userProperty != null)
                    {
                        var currentValue = userProperty.GetValue(user);

                        if (currentValue.Equals(newValue) is false)
                        {
                            userProperty.SetValue(user, newValue);
                            updated = true;
                        }
                    }
                }
            }

            user.Validate();

            if (updated)
            {
                await _context.SaveChangesAsync();
            }

            return updated;
        }
    }
}
