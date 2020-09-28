using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Tracking_Wallet_Log
    {

        public class Models_Tracking_Wallet_Log_List
        {
            public long MemberSrno { get; set; }
            public string WalletFrom { get; set; }
            public string WalletTo { get; set; }
            public int TransactionType { get; set; }
            public decimal PreviousAmount { get; set; }
            public decimal TransactionAmount { get; set; }
            public decimal CurrentTotalAmount { get; set; }
            public bool IsDeduct { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public long? Receiver { get; set; }
            public long? Sender { get; set; }
            public string LoginId { get; set; }
        }
    }
}
