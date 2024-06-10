namespace LibraSoft.Core.Exceptions
{
    public class UserNotFoundError : HandlerError
    {

        public UserNotFoundError()
        {
            Errors.Add("User not found.");
        }
    }
}
