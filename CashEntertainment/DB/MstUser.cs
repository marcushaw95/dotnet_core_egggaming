using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstUser
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string DoB { get; set; }
        public string DirectSponsor { get; set; }
        public string UserLevel { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Gender { get; set; }
        public string RefCode { get; set; }
        public string Upline { get; set; }
        public long? UplineId { get; set; }
    }
}
