using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class UserHasPunishmentError : ErrorBase
    {
        public UserHasPunishmentError(string punishment)
        {
            Errors.Add($"The user's status is ${punishment} and they can't make rent.");
        }
    }
}
