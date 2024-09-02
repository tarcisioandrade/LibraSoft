using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class UserTelephoneAlreadyExists : ErrorBase
    {
        public UserTelephoneAlreadyExists()
        {
            Errors.Add("Telephone already registered.");
        }
    }
}
