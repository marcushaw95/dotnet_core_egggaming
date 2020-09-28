using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstGame
    {
        public long Srno { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorImageUrl { get; set; }
        public string GameCategory { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public byte? Maintenance { get; set; }
        public byte? IsSubgame { get; set; }
    }
}
