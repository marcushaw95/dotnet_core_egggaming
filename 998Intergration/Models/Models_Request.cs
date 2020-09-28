using System;
using System.Collections.Generic;
using System.Text;

namespace _998Intergration.Models
{
    class Models_Request
    {
        public class Models_Register_Player
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public int Gender { get; set; } = -1;
            public string DoB { get; set; }
            public string Currency { get; set; }
            public string BankName { get; set; } = "CH";
            public string BankAccountNo { get; set; } = "CH";
        }
      
        public class Models_Login_Player
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UserName { get; set; }

        }

        public class Models_Retrieve_Balance_Player
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UserName { get; set; }
        }
       
        public class Models_Player_PlayGame
        {
            public string Vendor { get; set; }
            public string Device { get; set; } = "";
            public string Browser { get; set; }
            public string GameCode { get; set; } = "";
            public string GameHall { get; set; } = "";
            public string Lang { get; set; }
            public string MerchantCode { get; set; } = "";
            public string Ticket { get; set; } = "";
            public string SeatId { get; set; } = "";
            public string Tag { get; set; } = "";
            public string GameProvider { get; set; } = "";
        }


        public class Models_Player_TransferCredit
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
        }

        public class Models_Player_WithdrawCredit
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
        }

        public class Models_Player_CheckTransaction
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }

        }

        public class Models_Player_GetTickets_Fetch
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }


        public class Models_Player_GetTickets
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string Vendor { get; set; }
            public string PlayerName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }

        public class Models_Player_GetWinLoss
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public int Product { get; set; }
            public string PlayerName { get; set; }
            public string StatementDate { get; set; }

        }
    }
}
