using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Responses.Rent
{
    public class RentResponse
    {
        public Guid Id { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public ERentStatus Status { get; set; }
        public required IEnumerable<BookInRent> Books { get; set; }
    }

    public class BookInRent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public ECoverType CoverType { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public required AuthorInBookRent Author { get; set; }
    }

    public class AuthorInBookRent
    {
        public string Name { get; set; } = string.Empty;
    }
}