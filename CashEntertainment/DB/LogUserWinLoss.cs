using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogUserWinLoss
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string GameId { get; set; }
        public string LogDate { get; set; }
        public string GameCode { get; set; }
        public decimal TurnOver { get; set; }
        public decimal WinLoss { get; set; }
        public string Status { get; set; }
    }
}
