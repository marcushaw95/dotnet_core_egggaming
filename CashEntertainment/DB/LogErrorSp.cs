using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogErrorSp
    {
        public long Id { get; set; }
        public string File { get; set; }
        public string Message { get; set; }
        public int? Severity { get; set; }
        public int? State { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
