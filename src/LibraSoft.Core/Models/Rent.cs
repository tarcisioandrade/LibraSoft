using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Models.Validations;

namespace LibraSoft.Core.Models
{
    public class Rent : ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RentDate { get; private set; }
        public DateTime ExpectedReturnDate { get; private set; }
        public DateTime? ReturnedDate { get; private set; } = null;
        public ERentStatus Status { get; private set; }
        public bool EmailAlertSended { get; private set; } = false;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public IEnumerable<Book> Books { get; private set; } = new List<Book>();

        protected Rent() { }

        public Rent(DateTime rentDate, DateTime expectedReturnDate, Guid userId, IEnumerable<Book> books, ERentStatus status = ERentStatus.Requested_Awaiting_Pickup, DateTime? returnedDate = null)
        {
            RentDate = rentDate;
            ExpectedReturnDate = expectedReturnDate;
            ReturnedDate = returnedDate;
            UserId = userId;
            Books = books;
            Status = status;

            this.Validate();
        }
        public void Validate()
        {
            var validator = new RentValidate();

            var validate = validator.Validate(this);

            ThrowErrorInValidate(validate);
        }

        public int BooksRented()
        {
            return Books.Count();
        }

        public void SetInProgress()
        {
            this.Status = ERentStatus.Rent_In_Progress;
        }

        public void SetFinished()
        {
            this.Status = ERentStatus.Rent_Finished;
        }

        public void SetCanceled()
        {
            this.Status = ERentStatus.Rent_Canceled;
        }

        public void EmailAlerted()
        {
            this.EmailAlertSended = true;
        }
    }
}
