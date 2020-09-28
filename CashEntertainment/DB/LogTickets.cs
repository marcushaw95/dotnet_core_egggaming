using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogTickets
    {
        public long Srno { get; set; }
        public string TicketSessionId { get; set; }
        public string TicketId { get; set; }
        public long? MemberSrno { get; set; }
        public string GameType { get; set; }
        public string RoundId { get; set; }
        public decimal? Stake { get; set; }
        public decimal? StakeMoney { get; set; }
        public string Result { get; set; }
        public string Currency { get; set; }
        public DateTime? StatementDate { get; set; }
        public decimal? PlayerWinLoss { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? Vendor { get; set; }
        public int? Product { get; set; }
    }
}
