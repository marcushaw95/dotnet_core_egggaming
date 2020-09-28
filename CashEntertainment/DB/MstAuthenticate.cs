using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstAuthenticate
    {
        public long Srno { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
