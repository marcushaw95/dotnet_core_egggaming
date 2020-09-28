using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Bank
    {
        public class Models_Admin_Bank_List
        {
            public long Srno { get; set; }

            public long BankSrno { get; set; }
            public string BankCode { get; set; }
            public string BankName { get; set; }
            public string BankAccountHolder { get; set; }
            public string BankCardNo { get; set; }
            public string Country { get; set; }
            public string Status { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }


        public class Models_Member_Bank_List
        {
            public long Srno { get; set; }
            public long BankSrno { get; set; }
            public long MemberSrno { get; set; }
            public string BankCode { get; set; }
            public string BankName { get; set; }
            public string BankAccountHolder { get; set; }
            public string BankCardNo { get; set; }
            public string Status { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }

    }
}
