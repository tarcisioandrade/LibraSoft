using LibraSoft.Api.Database;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Events
{
    public class CheckPickingRentEvent : EventBase
    {
        private readonly AppDbContext _context;

        public CheckPickingRentEvent(AppDbContext context)
        {
            _context = context;
        }

        public override async Task Execute()
        {
            var HOURS_LIMIT_TO_EXPIRES_RENT_AWAITING_TO_PICKUP = 48;
            var hasModify = false;
            var rentsExpiredToPickup = await _context.Rents.Where(r => DateTime.UtcNow.Date >= r.RentDate.AddHours(HOURS_LIMIT_TO_EXPIRES_RENT_AWAITING_TO_PICKUP).Date
                                                                       && r.Status == ERentStatus.Requested_Awaiting_Pickup).ToListAsync();

            foreach (var rent in rentsExpiredToPickup)
            {
                rent.SetCanceled();
                hasModify = true;
            }

            if (hasModify)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
