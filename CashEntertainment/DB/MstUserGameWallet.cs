using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstUserGameWallet
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string GameId { get; set; }
        public decimal GameCredit { get; set; }
    }
}
