using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogRequestPaymentGateway
    {
        public long Srno { get; set; }
        public long MemberSrno { get; set; }
        public string DepositUrl { get; set; }
        public string Merchant { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Reference { get; set; }
        public string Key { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Datetime { get; set; }
        public string FrontUrl { get; set; }
        public string BackUrl { get; set; }
        public string Language { get; set; }
        public string Bank { get; set; }
        public string ClientIp { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
