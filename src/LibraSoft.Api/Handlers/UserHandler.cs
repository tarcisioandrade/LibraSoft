using LibraSoft.Api.Database;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.User;
using LibraSoft.Core.Responses.User;
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

        public async Task ChangePasswordAsync(ChangePasswordRequest request, User user)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ChangePassword(passwordHash);
            await _context.SaveChangesAsync();
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

        public async Task<User?> GetByEmailOrTelephoneAsync(string email, string telephone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email || u.Telephone == telephone);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.Include(user => user.Rents.Where(r => r.Status != ERentStatus.Rent_Canceled && r.Status != ERentStatus.Rent_Finished)).ThenInclude(u => u.Books).FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public async Task InactiveAsync(Guid id)
        {
            var user = await this.GetByIdAsync(id);

            user!.Inactive();

            await _context.SaveChangesAsync();
        }

        public async Task<UserResponse> UpdateAsync(UserUpdateRequest request, User user)
        {
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

                        if (currentValue is null || currentValue.Equals(newValue) is false)
                        {

                            if (property.Name == "Telephone")
                            {
                                var exists = await _context.Users.Where(u => u.Telephone == newValue.ToString()).AnyAsync();
                                if (exists)
                                {
                                    throw new UserTelephoneAlreadyExists();
                                }
                            }
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
            var response = new UserResponse
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                Status = user.Status,
                Telephone = user.Telephone,
            };

            return response;
        }
    }
}
