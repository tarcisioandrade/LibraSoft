using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Models
{
    public class User : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telephone { get; private set; } = string.Empty;
        public Address? Address { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public EUserRole Role { get; private set; } = EUserRole.Common;
        public EUserStatus Status { get; private set; } = EUserStatus.Active;
        public IEnumerable<Rent> Rents { get; private set; } = new List<Rent>();

        protected User() { }    
        public User(string name,
                    string email,
                    string telephone,
                    Address? address,
                    string password,
                    EUserRole role = EUserRole.Common,
                    EUserStatus status = EUserStatus.Active)
        {
            Name = name;
            Email = email;
            Telephone = telephone;
            Address = address;
            Password = password;
            Role = role;
            Status = status;

            this.Validate();
        }

        public void Inactived()
        {
            this.Status = EUserStatus.Inactive;
        }

        public void Suspend()
        {
            this.Status = EUserStatus.Suspense;
        }

        public void Ban()
        {
            this.Status = EUserStatus.Banned;
        }

        public void Active()
        {
            this.Status = EUserStatus.Active;
        }

        public void Validate()
        {
            var validator = new UserValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }
    }
}
