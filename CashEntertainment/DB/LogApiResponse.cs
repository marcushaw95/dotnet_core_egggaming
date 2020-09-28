using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogApiResponse
    {
        public long Srno { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
