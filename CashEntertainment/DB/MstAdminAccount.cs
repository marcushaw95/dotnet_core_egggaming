using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstAdminAccount
    {
        public long Srno { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Auth { get; set; }
        public string Status { get; set; }
        public string AdminType { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
