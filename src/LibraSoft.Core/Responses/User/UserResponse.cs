using LibraSoft.Core.Enums;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Responses.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public Address? Address { get; set; } = null;
        public EUserRole Role { get; set; }
        public EUserStatus Status { get; set; }
    }
}
