using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Book : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime PublicationAt { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Rent> Rents { get; set; } = new List<Rent>();
        public int CopiesAvailable { get; set; }
        public EStatus Status { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        protected Book() { }
        public Book(string title,
                    string publisher,
                    string isbn,
                    DateTime publicationAt,
                    IEnumerable<Category> categories,
                    int copiesAvailable,
                    Guid authorId,
                    EStatus status = EStatus.Active)
        {
            Title = title;
            Publisher = publisher;
            Isbn = isbn;
            PublicationAt = publicationAt;
            Categories = categories;
            Status = status;
            AuthorId = authorId;
            CopiesAvailable = copiesAvailable;

            this.Validate();
        }

        protected override void Validate()
        {
            var validator = new BookValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }

        public bool Equal(Guid id)
        {
            return Id.Equals(id);
        }
    }
}
