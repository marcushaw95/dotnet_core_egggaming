using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogRecreateUser
    {
        public long MemberSrno { get; set; }
        public string LoginId { get; set; }
        public string PreviousGameId { get; set; }
        public string PreviousGamePassword { get; set; }
        public string CurrentGameId { get; set; }
        public string CurrentGamePassword { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
