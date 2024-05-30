using System.ComponentModel.DataAnnotations.Schema;
using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Models
{
    public class Rent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RentDate { get; private set; }
        public DateTime ReturnDate { get; private set; }
        public ERentStatus Status { get; private set; }
        public Guid UserId { get; private set; }
        [NotMapped]
        public List<Book> Books { get; private set; } = new List<Book>();

    }
}
