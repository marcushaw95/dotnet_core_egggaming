using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstCurrency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Names { get; set; }
        public string Country { get; set; }
        public bool? Active { get; set; }
    }
}
