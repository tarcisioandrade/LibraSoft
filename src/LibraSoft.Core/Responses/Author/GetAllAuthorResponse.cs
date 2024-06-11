using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Responses.Author
{
    public class GetAllAuthorResponse 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Biography {  get; set; } = string.Empty;
        public DateTime? DateBirth { get; set; }
        public EStatus Status { get; set; }
    }
}
