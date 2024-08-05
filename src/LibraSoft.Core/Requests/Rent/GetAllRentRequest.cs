using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Requests.Rent
{
    public class GetAllRentRequest
    {
        public EQueryRentStatus? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
       
    }
}
