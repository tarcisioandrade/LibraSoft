using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Email;
using LibraSoft.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Events
{
    public class CheckReturnRentEvent : EventBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailSenderService _emailSender;
        public CheckReturnRentEvent(AppDbContext context, IEmailSenderService emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public override async Task Execute()
        {
            var rents = await _context.Rents.Where(rent => rent.Status != ERentStatus.Rent_Finished).ToListAsync();

            if (rents.Count == 0) return;

            foreach (var rent in rents)
            {
                var user = await _context.Users.Where(user => user.Status == EUserStatus.Active).FirstOrDefaultAsync(user => user.Id == rent.UserId);
                var isToReturnBookInNextDay = IsReturnBookOnNextBusinessDay(rent.ExpectedReturnDate);

                if (user == null) return;

                if (isToReturnBookInNextDay)
                {
                    if (rent.EmailAlertSended is false)
                    {
                        EmailMessageRequest emailContent = new()
                        {
                            Subject = "ALERTA: Devolução de livro",
                            Body = $"Prezado Sr.{user.Name}, evite sua suspensão, não esqueça de devolver o(s) livro(s) até o dia {rent.ExpectedReturnDate.ToLocalTime()}."
                        };

                        _emailSender.Send(user.Email, emailContent);

                        rent.EmailAlerted();
                    }
                }

                if (IsReturnBookDatePassed(rent.ExpectedReturnDate))
                {
                    rent.SetExpired();

                    if (user.PunishmentsDetails.Count == 2)
                    {
                        EmailMessageRequest bannedEmailContent = new()
                        {
                            Subject = "ALERTA: Sua conta foi banida.",
                            Body = $"Prezado Sr.{user.Name}, devido a esse ser seu terceiro atraso na entrega de livro(s) a sua conta está sendo banida, você nunca mais poderá efetuar empréstimos de livros na LibraSoft. Também estamos enviando nossa equipe atrás de você para recuperar-mos o livro que está em sua posse!"
                        };

                        _emailSender.Send(user.Email, bannedEmailContent);

                        var bannedPunishmentDetails = new PunishmentDetails { PunishInitDate = DateTime.UtcNow, PunishEndDate = DateTime.UtcNow.AddYears(100)};
                        user.Ban(bannedPunishmentDetails);
                    }
                    else
                    {
                        EmailMessageRequest suspenseEmailContent = new()
                        {
                            Subject = "ALERTA: Sua conta foi suspensa.",
                            Body = $"Prezado Sr.{user.Name}, devido a seu atraso na entrega do(s) livro(s) a sua conta está sendo suspensa, você não poderá mais efetuar empréstimos de livros por 1 mês. Tambem estamos enviando nossa equipe atrás de você para recuperar-mos o livro que está em sua posse!"

                        };

                        var suspensePunishmentDetails = new PunishmentDetails { PunishInitDate = DateTime.UtcNow, PunishEndDate = DateTime.UtcNow.AddMonths(1) };

                        _emailSender.Send(user.Email, suspenseEmailContent);

                        user.Suspend(suspensePunishmentDetails);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        private static bool IsReturnBookOnNextBusinessDay(DateTime returnDate)
        {
            DateTime nextBusinessDay = GetNextBusinessDay(DateTime.UtcNow);
            return returnDate.Date == nextBusinessDay.Date;
        }
        private static DateTime GetNextBusinessDay(DateTime date)
        {
            date = date.AddDays(1);
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }
            return date;
        }
        private static bool IsReturnBookDatePassed(DateTime returnDate)
        {
            DateTime now = DateTime.UtcNow;
            return returnDate.Date < now.Date;
        }
    }
}
