using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstCountry
    {
        public long Srno { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryPhone { get; set; }
        public bool? Active { get; set; }
        public string Cnnames { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
