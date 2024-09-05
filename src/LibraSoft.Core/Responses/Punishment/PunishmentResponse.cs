using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Responses.Punishment
{
    public class PunishmentResponse
    {
        public DateTime PunishInitDate { get; set; }
        public DateTime PunishEndDate { get; set; }
        public EStatus Status { get; set; }
    }
}
