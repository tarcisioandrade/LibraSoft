
namespace LibraSoft.Core.Requests.Book
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Image {  get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime PublicationAt { get; set; }
        public Guid AuthorId { get; set; }
        public int CopiesAvailable { get; set; }
        public List<CategoryBookObject> Categories { get; set; } = [];
    }

    public class CategoryBookObject
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
