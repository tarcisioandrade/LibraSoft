using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Users;

namespace LibraSoft.Core.Handlers
{
    public interface IUserHandler
    {
        Task CreateAsync(CreateUserRequest request);
        Task DeleteAsync(DeleteUserRequest userId);
        Task<User?> GetByEmailAsync(GetByEmailRequest email);
    }
}
