namespace LibraSoft.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
