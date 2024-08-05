namespace LibraSoft.Core.Exceptions
{
    public class RentBookLimitExceededError : ControllerError
    {
        public RentBookLimitExceededError(int? LIMIT_TO_BOOKS_RENT = 3)
        {
            Errors.Add($"Limit max ({LIMIT_TO_BOOKS_RENT}) of rent exceeded.");
        }
    }
}
