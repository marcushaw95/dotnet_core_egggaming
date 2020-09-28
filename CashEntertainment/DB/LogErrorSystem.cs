using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogErrorSystem
    {
        public long Srno { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Context { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
