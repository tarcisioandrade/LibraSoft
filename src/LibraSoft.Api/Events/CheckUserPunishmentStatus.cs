using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Email;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Events
{
    public class CheckUserPunishmentStatus : EventBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailSenderService _emailSender;

        public CheckUserPunishmentStatus(AppDbContext context, IEmailSenderService emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public override async Task Execute()
        {
            var users = await _context.Users.Where(user => user.Status == EUserStatus.Suspense).ToListAsync();
            var isModify = false;

            foreach (var user in users)
            {
                var punishments = user.PunishmentsDetails.ToList().Where(punishment => punishment.Status == EStatus.Active);

                if (punishments.Count() < 0) return;

                foreach (var punishment in punishments)
                {
                    var today = DateTime.Now.Date;
                    var punishmentHasExpired = punishment.PunishEndDate.Date <= today;

                    if (punishmentHasExpired)
                    {
                        punishment.Inactive();
                        user.Active();

                        EmailMessageRequest emailContent = new()
                        {
                            Subject = "ALERTA: Suspensão Expirada",
                            Body = $"Prezado Sr.{user.Name}, sua suspensão foi expirada, você já pode voltar alugar livros, evite outra suspensão retornando o(s) livro(s) na data prevista ."
                        };
                        _emailSender.Send(user.Email, emailContent);
                        isModify = true;
                    }
                }
            }

            if (isModify)
            {
                    await _context.SaveChangesAsync();
            }
        }
    }
}
