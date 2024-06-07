namespace LibraSoft.Core.Exceptions
{
    public class RentBookLimitExceeded : ControllerError
    {
        public RentBookLimitExceeded()
        {
            Errors.Add("Limit max (5) of rent exceeded.");
        }
    }
}
