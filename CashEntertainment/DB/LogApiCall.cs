using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogApiCall
    {
        public long Srno { get; set; }
        public string Action { get; set; }
        public string SendUrl { get; set; }
        public string SendParam { get; set; }
        public string RecieveParam { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
