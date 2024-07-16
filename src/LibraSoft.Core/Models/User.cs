using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;
using LibraSoft.Core.ValueObjects;
using LibraSoft.Domain.ValueObjects;

namespace LibraSoft.Core.Models
{
    public class User : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telephone { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public EUserRole Role { get; private set; } = EUserRole.Common;
        public Address? Address { get; private set; }
        public List<PunishmentDetails> PunishmentsDetails { get; private set; } = [];
        public EUserStatus Status { get; private set; } = EUserStatus.Active;
        public IEnumerable<Rent> Rents { get; private set; } = new List<Rent>();
        public IEnumerable<Review> Reviews { get; private set; } = new List<Review>();
        
        protected User() { }    
        public User(string name,
                    string email,
                    string telephone,
                    Address? address,
                    List<PunishmentDetails> punishmentDetails,
                    string password,
                    EUserRole role = EUserRole.Common,
                    EUserStatus status = EUserStatus.Active)
        {
            Name = name;
            Email = email;
            Telephone = telephone;
            Address = address;
            PunishmentsDetails = punishmentDetails;
            Password = password;
            Role = role;
            Status = status;

            this.Validate();
        }

        public void Inactive()
        {
            this.Status = EUserStatus.Inactive;
        }

        public void Suspend(PunishmentDetails punishmentDetails)   
        {
            PunishmentsDetails.Add(punishmentDetails);
            this.Status = EUserStatus.Suspense;
        }

        public void Ban(PunishmentDetails punishmentDetails)
        {
            PunishmentsDetails.Add(punishmentDetails);
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
