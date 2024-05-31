using System.ComponentModel.DataAnnotations;
using LibraSoft.Core.Enums;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Requests.Users
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public Address? Address { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
