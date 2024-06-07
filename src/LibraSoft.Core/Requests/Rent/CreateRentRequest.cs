namespace LibraSoft.Core.Requests.Rent
{
    public class CreateRentRequest
    {
        public DateTime RentDate { get; set; }
        public List<RentBookObject> Books { get; set; } = [];
    }
    public class RentBookObject
    {
        public Guid Id { get; set; }
    }
}
