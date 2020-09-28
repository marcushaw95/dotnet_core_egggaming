using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstExchangeRate
    {
        public long Srno { get; set; }
        public string Base { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string LastUpdateBy { get; set; }
    }
}
