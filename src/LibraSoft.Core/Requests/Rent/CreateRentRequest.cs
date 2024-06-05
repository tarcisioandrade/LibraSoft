using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Requests.Rent
{
    public class CreateRentRequest
    {
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Guid UserId { get; set; }
        public List<RentBookObject> Books { get; set; }
    }
    public class RentBookObject
    {
        public Guid Id { get; set; }
    }
}
