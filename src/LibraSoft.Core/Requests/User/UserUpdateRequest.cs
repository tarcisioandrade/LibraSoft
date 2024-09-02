using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Requests.User
{
    public class UserUpdateRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public string? Telephone { get; set; }
    }
}
