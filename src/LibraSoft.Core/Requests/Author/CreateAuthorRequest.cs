namespace LibraSoft.Core.Requests.Author
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime? DateBirth { get; set; }
    }
}
