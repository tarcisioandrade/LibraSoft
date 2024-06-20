namespace LibraSoft.Core.Exceptions
{
    public class RentBookLimitExceededError : ControllerError
    {
        public RentBookLimitExceededError()
        {
            Errors.Add("Limit max (5) of rent exceeded.");
        }
    }
}
