using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Author : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string? Biography { get; private set; }
        public DateTime? DateBirth { get; private set; }
        public EStatus Status { get; private set; } = EStatus.Active;
        public IEnumerable<Book> Books { get; private set; } = new List<Book>();

        protected Author() { }

        public Author(string name, string? biography, DateTime? dateBirth, EStatus status = EStatus.Active)
        {
            Name = name;
            Biography = biography;
            DateBirth = dateBirth;
            Status = status;

            this.Validate();
        }

        public void Validate()
        {
            var validator = new AuthorValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }
    }
}
