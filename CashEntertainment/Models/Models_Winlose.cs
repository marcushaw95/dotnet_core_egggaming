using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Winlose
    {
        public class Models_Winlose_List
        {
            public long MemberSrno { get; set; }
            public decimal WinloseAmount { get; set; }
            public decimal StakeAmount { get; set; }
            public int Vendor { get; set; }
            public string Currency { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public int? Product { get; set; }
            public string GameType { get; set; }
            public string LoginId { get; set; }
        }
    }
}
