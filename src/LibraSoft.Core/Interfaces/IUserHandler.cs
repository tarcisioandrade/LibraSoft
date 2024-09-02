using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.User;
using LibraSoft.Core.Responses.User;

namespace LibraSoft.Core.Interfaces
{
    public interface IUserHandler
    {
        Task CreateAsync(CreateUserRequest request, bool isAdmin = false);
        Task DeleteAsync(DeleteUserRequest request);
        Task<User?> GetByEmailAsync(GetByEmailRequest request);
        Task<User?> GetByIdAsync(Guid id);
        Task<UserResponse> UpdateAsync(UserUpdateRequest request, User user);
        Task InactiveAsync(Guid id);
    }
}
    