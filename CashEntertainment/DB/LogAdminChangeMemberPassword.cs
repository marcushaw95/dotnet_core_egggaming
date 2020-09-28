using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogAdminChangeMemberPassword
    {
        public long Srno { get; set; }
        public string ActionBy { get; set; }
        public long MemberSrno { get; set; }
        public string PreviousPassword { get; set; }
        public string CurrentPassword { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
