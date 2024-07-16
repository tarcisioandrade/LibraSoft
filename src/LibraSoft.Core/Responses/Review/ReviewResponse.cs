namespace LibraSoft.Core.Responses.Review
{
    public class ReviewResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Author { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
