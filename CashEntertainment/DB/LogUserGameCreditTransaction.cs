using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogUserGameCreditTransaction
    {
        public long Srno { get; set; }
        public string GameApi { get; set; }
        public string TrasactionId { get; set; }
        public long MemberSrno { get; set; }
        public string Player { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal BeforeAmount { get; set; }
        public decimal AfterAmount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
