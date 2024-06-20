using LibraSoft.Core.Requests.Email;

namespace LibraSoft.Core.Interfaces
{
    public interface IEmailSenderService
    {
        public void Send(string emailTo, EmailMessageRequest message);
    }
}
