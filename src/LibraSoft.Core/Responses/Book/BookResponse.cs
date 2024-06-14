using LibraSoft.Core.Enums;
using LibraSoft.Core.Responses.Author;
using LibraSoft.Core.Responses.Category;

namespace LibraSoft.Core.Responses.Book
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string? Image {  get; set; }
        public string Isbn { get; set; } = string.Empty;
        public DateTime PublicationAt { get; set; }
        public int CopiesAvaliable { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; } = [];
        public required AuthorResponse Author { get; set; }
        public EStatus Status { get; set; }
        
    }
}
