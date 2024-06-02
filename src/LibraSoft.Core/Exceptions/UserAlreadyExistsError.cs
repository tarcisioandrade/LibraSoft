using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class UserAlreadyExistsError : ErrorBase
    {
        public UserAlreadyExistsError()
        {
            Errors.Add("User already exists.");
        }
    }
}
