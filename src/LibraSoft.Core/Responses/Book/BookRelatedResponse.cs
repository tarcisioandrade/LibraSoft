using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Responses.Book
{
    public class BookRelatedResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public ECoverType CoverType { get; set; }
    }
}
