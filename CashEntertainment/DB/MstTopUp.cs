using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstTopUp
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string WalletType { get; set; }
        public decimal TopupAmount { get; set; }
        public int? Status { get; set; }
        public string Remarks { get; set; }
        public DateTime RequestDate { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveRemark { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string RejectBy { get; set; }
        public string RejectRemark { get; set; }
        public DateTime? RejectDate { get; set; }
        public string TopupImageProof { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string BankCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccountHolder { get; set; }
        public int? TransactionType { get; set; }
        public string TransactionHash { get; set; }
        public decimal? Rate { get; set; }
        public string Currency { get; set; }
    }
}
