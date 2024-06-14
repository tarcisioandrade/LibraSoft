namespace LibraSoft.Core.Requests.Book
{
    public class GetAllBookRequest
    {
        public bool IncludeInactive { get; set; }
        public string? Search {  get; set; }
        public string? Category { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
