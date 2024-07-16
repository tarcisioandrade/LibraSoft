using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Models
{
    public class Like : ModelBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid ReviewId { get; private set; }
        public Guid UserId { get; private set; }
        public Review Review { get; private set; } = null!;
        public User User { get; private set; } = null!;

        protected Like() { }

        public Like(Guid reviewId, Guid userId)
        {
            ReviewId = reviewId;
            UserId = userId;    
        }
    }
}
