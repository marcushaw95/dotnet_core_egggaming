using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstBanner
    {
        public long Srno { get; set; }
        public string ImagePath { get; set; }
        public string RedirectUrl { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
