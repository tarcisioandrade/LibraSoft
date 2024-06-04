using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class UserOrPasswordIncorrectError : ErrorBase
    {
        public UserOrPasswordIncorrectError()
        {
            Errors.Add("Email or password incorrect");
        }
    }
}
