using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Requests.Author
{
    public class UpdateAuthorRequest
    {
        public string? Name { get; set; }
        public string? Biography {  get; set; }
        public DateTime? Birthdate { get; set; }
        public EStatus Status { get; set; }
    }
}
