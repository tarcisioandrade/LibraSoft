using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Users;

namespace LibraSoft.Core.Handlers
{
    public interface IUserHandler
    {
        Task<User?> CreateAsync(CreateUserRequest request);
    }
}
