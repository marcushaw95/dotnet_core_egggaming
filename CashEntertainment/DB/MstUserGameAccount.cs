using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstUserGameAccount
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string GamePassword { get; set; }
        public string GameId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
