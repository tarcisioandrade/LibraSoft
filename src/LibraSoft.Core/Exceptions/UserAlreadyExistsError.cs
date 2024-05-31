using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class UserAlreadyExistsError : ErrorBase
    {
        public UserAlreadyExistsError(string message)
        {
            Errors.Add(message);
        }

        public UserAlreadyExistsError(List<string> messages)
        {
            Errors = messages;
        }
    }
}
