using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_User
    {
        public class Models_User_Profile
        {
            public string LoginID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Dob { get; set; }
            public int Gender { get; set; }
            public string RefCode { get; set; }
            public string Upline { get; set; }
        }

        public class Models_User_Listing
        {
            public long MemberSrno { get; set; }
            public string LoginID { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Status { get; set; }
            public string Country { get; set; }
            public DateTime RegisterDate { get; set; }
            public string Dob { get; set; }
            public string Gender { get; set; }
            public string Refcode { get; set; }
            public string DirectSponsor { get; set; }
            public string Upline { get; set; }
            public decimal CashCredit { get; set; }
            public decimal TurnoverAmount { get; set; }
            public int GameRegister { get; set; }
        }


        public class Models_User_Downline_Listing
        {
            public long? UplineId { get; set; }
            public int Level { get; set; }

            public long UserId { get; set; }

        }

        public class Models_User_Account_Downline_Listing
        {
            public long? UplineId { get; set; }
            public int Level { get; set; }
            public long UserId { get; set; }
            public string LoginId { get; set; }
            public decimal CashCredit { get; set; }
            public decimal TurnoverAmount { get; set; }
        }

        public class Models_User_Login_Log_List
        {
            public string LoginId { get; set; }
            public long MemberSrno { get; set; }
            public string Password { get; set; }
            public string Ip { get; set; }
            public string Status { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }
    }
}
