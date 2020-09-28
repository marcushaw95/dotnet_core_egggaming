using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class TblErrStoredProcedures
    {
        public long ErrIndex { get; set; }
        public string ErrFile { get; set; }
        public string ErrMessage { get; set; }
        public int? ErrSeverity { get; set; }
        public int? ErrState { get; set; }
        public long? ErrDateTime { get; set; }
    }
}
