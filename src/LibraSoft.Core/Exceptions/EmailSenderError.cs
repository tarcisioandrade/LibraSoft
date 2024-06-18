using System.Net.Mail;
using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class EmailSenderError : ErrorBase
    {
        public EmailSenderError()
        {
            Errors.Add("The email could not be sent.");
        }

        public EmailSenderError(SmtpException ex)
        {
            Errors.Add(ex.ToString());
        }
    }
}
