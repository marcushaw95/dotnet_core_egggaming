using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstWinlose
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public decimal WinloseAmount { get; set; }
        public decimal StakeAmount { get; set; }
        public int Vendor { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? Product { get; set; }
        public string GameType { get; set; }
    }
}
