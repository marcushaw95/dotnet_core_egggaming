using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstApiDomain
    {
        public long Srno { get; set; }
        public string Domain { get; set; }
        public string IsActivate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
