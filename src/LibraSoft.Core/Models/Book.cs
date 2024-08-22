using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;
using LibraSoft.Core.ValueObjects;

namespace LibraSoft.Core.Models
{
    public class Book : ModelBase
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public string Publisher { get; private set; } = string.Empty;
        public string Isbn { get; private set; } = string.Empty;
        public DateTime PublicationAt { get; private set; }
        public int CopiesAvailable { get; private set; }
        public double AverageRating { get; private set; }
        public int PageCount { get; private set; }
        public string Sinopse { get; private set; } = string.Empty; 
        public string Language { get; private set; } = string.Empty;
        public Dimensions Dimensions { get; private set; } = new();
        public ECoverType CoverType { get; private set; }
        public EStatus Status { get; private set; }
        public IEnumerable<Category> Categories { get; private set; } = new List<Category>();
        public IEnumerable<Rent> Rents { get; private set; } = new List<Rent>();
        public IEnumerable<Review> Reviews { get; private set; } = new List<Review>();
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
                    int pageCount,
                    string sinopse,
                    string language,
                    Dimensions dimensions,
                    string image,
                    ECoverType coverType,
                    EStatus status = EStatus.Active)
        {
            Title = title;
            Publisher = publisher;
            Isbn = isbn;
            PublicationAt = publicationAt;
            Categories = categories;
            Status = status;
            AuthorId = authorId;
            PageCount = pageCount;
            Sinopse = sinopse;
            Language = language;
            Dimensions = dimensions;
            CoverType = coverType;
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

        public void Active()
        {
            Status = EStatus.Active;
        }

        public void SetAverage(double average)
        {
            AverageRating = average;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateImage(string image)
        {
            Image = image;
        }

        public void UpdatePublisher(string publisher)
        {
            Publisher = publisher;
        }

        public void UpdateIsbn(string isbn)
        {
            Isbn = isbn;
        }

        public void UpdatePageCount(int pageCount)
        {
            PageCount = pageCount;
        }

        public void UpdateSinopse(string sinopse)
        {
            Sinopse = sinopse;
        }

        public void UpdateLanguage(string language)
        {
            Language = language;
        }

        public void UpdatePublicationAt(DateTime publicationAt)
        {
            PublicationAt = publicationAt;
        }

        public void UpdateAuthorId(Guid authorId)
        {
            AuthorId = authorId;
        }

        public void UpdateCopiesAvailable(int copiesAvailable)
        {
            CopiesAvailable = copiesAvailable;
        }

        public void UpdateDimensions(Dimensions dimensions)
        {
            Dimensions = dimensions;
        }

        public void UpdateCoverType(ECoverType coverType)
        {
            CoverType = coverType;
        }

        public void UpdateCategories(IEnumerable<Category> categories)
        {
            Categories = categories;
        }

        public void Validate()
        {
            var validator = new BookValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }

    }
}
