using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstSubGame
    {
        public long Srno { get; set; }
        public string VendorCode { get; set; }
        public string GameCode { get; set; }
        public string GameName { get; set; }
        public string GameImageUrl { get; set; }
        public string GameImageUrl2 { get; set; }
        public string Status { get; set; }
        public byte Maintenance { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string GameType { get; set; }
    }
}
