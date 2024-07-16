using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Review : ModelBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; private set; } = string.Empty;
        public string Comment { get; private set; } = string.Empty;
        public int Rating { get; private set; }
        public Guid BookId {  get; private set; }
        public Guid UserId { get; private set; }
        public EStatus Status { get; private set; } = EStatus.Active;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Book Book { get; private set; } = null!;
        public User User { get; private set; } = null!;

        private readonly List<Like> _likes = new();
        public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();
        public int LikesCount => _likes.Count;

        protected Review(){}

        public Review(string title, string comment, int rating, Guid bookId, Guid userId, EStatus status = EStatus.Active)
        {
            Title = title;
            Comment = comment;
            Rating = rating;
            BookId = bookId;
            UserId = userId;
            Status = status;

            this.Validate();
        }

        public void AddLike(Like like)
        {
            if (_likes.All(l => l.UserId != like.UserId))
            {
                _likes.Add(like);
            }
        }
        public void RemoveLike(Guid userId)
        {
            var like = _likes.FirstOrDefault(l => l.UserId == userId);
            if (like != null)
            {
                _likes.Remove(like);
            }
        }
        public void Validate()
        {
            var validator = new ReviewValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }
    }
}
