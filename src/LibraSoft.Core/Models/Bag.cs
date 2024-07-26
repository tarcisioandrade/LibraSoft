using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Models
{
    public class Bag : ModelBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid BookId { get; private set; }
        public Book Book { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        protected Bag() { }

        public Bag(Guid bookId, Guid userId)
        {
            BookId = bookId;
            UserId = userId;
        }
    }
}
