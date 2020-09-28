using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstAdminBank
    {
        public long Srno { get; set; }
        public long BankSrno { get; set; }
        public string BankAccountHolder { get; set; }
        public string BankCardNo { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
