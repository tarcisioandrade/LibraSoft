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
        public User User { get; private set; } = null!;
        public IEnumerable<Book> Books { get; private set; } = new List<Book>();
    }
}
