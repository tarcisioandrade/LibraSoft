using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.User;

namespace LibraSoft.Core.Handlers
{
    public interface IUserHandler
    {
        Task CreateAsync(CreateUserRequest request);
        Task DeleteAsync(DeleteUserRequest request);
        Task<User?> GetByEmailAsync(GetByEmailRequest request);
        Task<User?> GetByIdAsync(GetByIdRequest request);
    }
}
