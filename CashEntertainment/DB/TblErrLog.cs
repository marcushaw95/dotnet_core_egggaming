using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class TblErrLog
    {
        public long ErrIndex { get; set; }
        public string ErrForm { get; set; }
        public string ErrMessage { get; set; }
        public string ErrStackTrace { get; set; }
        public DateTime? ErrDateTime { get; set; }
        public string ErrLoginId { get; set; }
        public string ErrIp { get; set; }
    }
}
