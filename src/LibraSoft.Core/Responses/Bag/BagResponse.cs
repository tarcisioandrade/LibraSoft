using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Responses.Bag
{
    public class BagResponse
    {
        public Guid Id { get; set; }
        public BookInBag Book { get; set; } = new BookInBag();
        public DateTime CreatedAt { get; set; }
    }

    public class BookInBag
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int CopiesAvaliable { get; set; }
        public double AverageRating { get; set; }
        public ECoverType CoverType { get; set; }
        public EStatus Status { get; set; }
    }
}
