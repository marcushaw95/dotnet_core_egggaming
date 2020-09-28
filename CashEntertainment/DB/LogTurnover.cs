using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogTurnover
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UpdatedAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int TransactionType { get; set; }
    }
}
