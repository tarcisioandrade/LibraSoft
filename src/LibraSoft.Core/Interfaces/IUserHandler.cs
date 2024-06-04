using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.User;

namespace LibraSoft.Core.Interfaces
{
    public interface IUserHandler
    {
        Task CreateAsync(CreateUserRequest request, bool isAdmin = false);
        Task DeleteAsync(DeleteUserRequest request);
        Task<User?> GetByEmailAsync(GetByEmailRequest request);
        Task<User?> GetByIdAsync(GetByIdRequest request);
    }
}
