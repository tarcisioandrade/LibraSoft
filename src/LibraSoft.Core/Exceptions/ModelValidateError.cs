using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class ModelValidateError : ErrorBase
    {
        public ModelValidateError(string message)
        {
            Errors.Add(message);
        }

        public ModelValidateError(List<string> messages)
        {
            Errors = messages;
        }
    }
}
