using System.ComponentModel.DataAnnotations.Schema;
using LibraSoft.Core.Enums;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telephone { get; private set; } = string.Empty;
        public Address? Address { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public EUserRole Role { get; private set; } = EUserRole.Common;
        public EUserStatus Status { get; private set; } = EUserStatus.Active;
        [NotMapped]
        public List<Rent> Rents { get; private set; } = new List<Rent>();

        protected User() { }    
        public User(string name, string email, string telephone, Address? address, string password, EUserRole role = EUserRole.Common, EUserStatus status = EUserStatus.Active)
        {
            Name = name;
            Email = email;
            Telephone = telephone;
            Address = address;
            Password = password;
            Role = role;
            Status = status;
        }
    }
}
