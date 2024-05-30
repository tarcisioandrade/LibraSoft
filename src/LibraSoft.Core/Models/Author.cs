using System.ComponentModel.DataAnnotations.Schema;
using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Models
{
    public class Author
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string? Biography { get; private set; }
        public DateTime? DateBirth { get; private set; }
        public EStatus Status { get; private set; } = EStatus.Active;
        [NotMapped]
        public List<Book> Books { get; private set; } = new List<Book>();
    }
}
