namespace LibraSoft.Core.Requests.Review
{
    public class CreateReviewRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public Guid BookId { get; set; }
    }
}
