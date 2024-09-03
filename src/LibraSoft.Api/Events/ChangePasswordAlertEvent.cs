using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Email;

namespace LibraSoft.Api.Events
{
    public class ChangePasswordAlertEvent
    {
        private readonly IEmailSenderService _emailSender;

        public ChangePasswordAlertEvent(IEmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        public void Execute(User user)
        {
            EmailMessageRequest emailContent = new()
            {
                Subject = "Sua senha foi alterada.",
                Body = $"Olá {user.Name}, vim te informar que a sua senha acaba de ser alterada. Se não foi você, entre em contato com o suporte."
            };
            _emailSender.Send(user.Email, emailContent);
        }
    }
}
