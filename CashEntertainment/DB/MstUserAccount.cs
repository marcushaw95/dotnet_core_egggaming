using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstUserAccount
    {
        public long MemberSrno { get; set; }
        public string LoginId { get; set; }
        public string AccountType { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public bool VerifiedStatus { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int GameRegister { get; set; }
    }
}
