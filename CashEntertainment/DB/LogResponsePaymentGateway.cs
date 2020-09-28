using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class LogResponsePaymentGateway
    {
        public long Srno { get; set; }
        public string Merchant { get; set; }
        public string Reference { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Language { get; set; }
        public string Customer { get; set; }
        public string Datetime { get; set; }
        public string Note { get; set; }
        public string Key { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Id { get; set; }
    }
}
