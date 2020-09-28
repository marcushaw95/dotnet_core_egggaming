using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstWithdraw
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public DateTime? RejectDate { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankCardNo { get; set; }
        public string BankAccountHolder { get; set; }
        public decimal WithdrawAmount { get; set; }
        public int? Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string RejectBy { get; set; }
        public string RejectRemark { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string ToAddress { get; set; }
        public int? TransactionType { get; set; }
        public decimal? Rate { get; set; }
        public string Currency { get; set; }
    }
}
