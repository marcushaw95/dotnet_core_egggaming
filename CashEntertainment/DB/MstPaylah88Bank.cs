using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstPaylah88Bank
    {
        public long Srno { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
    }
}
