using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogLoginIp
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
