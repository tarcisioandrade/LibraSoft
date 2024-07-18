namespace LibraSoft.Core.Requests.Review
{
    public class GetAllReviewRequest
    {
        public Guid BookId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
