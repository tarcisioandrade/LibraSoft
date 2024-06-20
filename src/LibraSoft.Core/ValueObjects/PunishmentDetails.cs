using LibraSoft.Core.Enums;

namespace LibraSoft.Core.ValueObjects
{
    public class PunishmentDetails
    {
        public DateTime PunishEndDate { get; set; }
        public DateTime PunishInitDate { get; set; } = DateTime.UtcNow;
        public EStatus Status { get; set; } = EStatus.Active;

        public void Inactive()
        {
            this.Status = EStatus.Inactive;
        }
    }
}
