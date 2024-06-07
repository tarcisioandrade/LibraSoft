namespace LibraSoft.Core.Requests.Rent
{
    public class CreateRentRequestHandler
    {
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<Models.Book> Books { get; set; } = [];
    }
}
