using System.ComponentModel.DataAnnotations.Schema;
using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; private set; } = string.Empty;
        public string Publisher { get; private set; } = string.Empty;
        public string Isbn { get; private set; } = string.Empty;
        public DateTime PublicationAt { get; private set; }
        [NotMapped]
        public List<Category> Categories { get; private set; } = new List<Category>();
        [NotMapped]
        public List<Rent> Rents { get; private set; } = new List<Rent>();
        public int CopiesAvailable { get; private set; }
        public EStatus Status { get; private set; }
        public Guid AuthorId { get; private set; }
    }
}
