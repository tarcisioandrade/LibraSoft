using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Book : ModelBase
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; } = string.Empty;
        public string? Image { get; private set; }
        public string Publisher { get; private set; } = string.Empty;
        public string Isbn { get; private set; } = string.Empty;
        public DateTime PublicationAt { get; private set; }
        public IEnumerable<Category> Categories { get; private set; } = new List<Category>();
        public IEnumerable<Rent> Rents { get; private set; } = new List<Rent>();
        public IEnumerable<Review> Reviews { get; private set; } = new List<Review>();
        public int CopiesAvailable { get; private set; }
        public double AverageRating { get; private set; }
        public EStatus Status { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; } = null!;

        protected Book() { }
        public Book(string title,
                    string publisher,
                    string isbn,
                    DateTime publicationAt,
                    IEnumerable<Category> categories,
                    int copiesAvailable,
                    Guid authorId,
                    EStatus status = EStatus.Active,
                    string? image = null)
        {
            Title = title;
            Publisher = publisher;
            Isbn = isbn;
            PublicationAt = publicationAt;
            Categories = categories;
            Status = status;
            AuthorId = authorId;
            CopiesAvailable = copiesAvailable;
            Image = image;

            this.Validate();
        }
        public bool Equal(Guid id)
        {
            return Id.Equals(id);
        }

        public void DecreaseNumberOfCopies()
        {
            CopiesAvailable = CopiesAvailable - 1;
        }
        
        public bool HasCopiesAvaliable()
        {
            return CopiesAvailable > 0;
        }

        public bool HasRent()
        {
            return Rents.Any();
        }

        public void Inactive()
        {
            Status = EStatus.Inactive;
        }

        public void SetAverage(double average)
        {
            AverageRating = average;
        }

        public void Validate()
        {
            var validator = new BookValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }

    }
}
