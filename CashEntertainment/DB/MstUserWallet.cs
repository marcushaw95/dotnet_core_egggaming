using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstUserWallet
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public decimal CashCredit { get; set; }
        public decimal Commission { get; set; }
        public decimal WinLossCommission { get; set; }
        public decimal PendingWithdrawAmount { get; set; }
        public decimal PendingWinLossWithdrawAmount { get; set; }
        public decimal TurnoverAmount { get; set; }
        public decimal TwelveTurnoverAmount { get; set; }
    }
}
