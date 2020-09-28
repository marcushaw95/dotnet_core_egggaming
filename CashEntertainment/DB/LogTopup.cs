using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogTopup
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string WalletFrom { get; set; }
        public string WalletTo { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal CurrentTotalAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int TransactionType { get; set; }
    }
}
