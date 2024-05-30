using LibraSoft.Core.Enums;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Requests.Users
{
    public class CreateUserRequest
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telephone { get; private set; } = string.Empty;
        public Address? Address { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public EUserRole Role { get; private set; } = EUserRole.Common;
        public EUserStatus Status { get; private set; } = EUserStatus.Active;
    }
}
