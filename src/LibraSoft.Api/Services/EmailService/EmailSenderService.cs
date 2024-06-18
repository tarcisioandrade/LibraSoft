using System.Net;
using System.Net.Mail;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace LibraSoft.Api.Services.EmailService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailSettings _settings;

        public EmailSenderService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void Send(string emailTo)
        {
            MailAddress to = new(emailTo);
            MailAddress from = new(_settings.EmailFrom);

            MailMessage email = new(from, to);
            email.Subject = "Testing out email sending";
            email.Body = "Hello all the way from the land of C#";

            var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Credentials.UserName, _settings.Credentials.Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            try
            {
                client.Send(email);
            }
            catch (SmtpException ex)
            {
                throw new EmailSenderError(ex);
            }
        }
    }
}
