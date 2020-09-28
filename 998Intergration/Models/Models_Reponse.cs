using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace _998Intergration.Models
{
    public class Models_Reponse
    {
        public class Login_Response
        {
            public string Token { get; set; }
            public int Error { get; set; }
            public string Message { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UTC { get; set; }
        }

        public class Balance_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public decimal Balance { get; set; }
        }

        public class Open_Game_Response
        {
            public Open_Game_Response_Result Result { get; set; }
            public string TargetUrl { get; set; }
            public bool Success { get; set; }
            public Open_Game_Response_Error_Result Error { get; set; } = null;
            public string Message { get; set; }
        }


        public class Open_Game_Response_Result
        {
            public string Data { get; set; }

            public Open_Game_Response_Settings_Result Settings { get; set; }
        }

        public class Open_Game_Response_Settings_Result
        {
            public double MaxPerBet { get; set; }
        }

        public class Open_Game_Response_Error_Result
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
        public class Game_List_Response
        {
            public int ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public Game_Data Data { get; set; }
            public string TargetUrl { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        public class Game_Data
        {
            public string GameID { get; set; }
            public string GameName { get; set; }
            public string GameTypeID { get; set; }
            public string TypeDescription { get; set; }
            public string Technology { get; set; }
            public string TechnologyID { get; set; }
            public string Platform { get; set; }
            public bool DemoGameAvailable { get; set; }
            public int GameIdNumeric { get; set; }
        }

        public class Deposit_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
            public decimal Before { get; set; }
            public decimal After { get; set; }
        }

        public class Withdraw_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
            public decimal Before { get; set; }
            public decimal After { get; set; }
        }

        public class CheckTransaction_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public int Status { get; set; }

        }


        public class GetTicket_Fetch_Response
        {
            public GetTicket_Fetch_Error Error { get; set; }
            public string Message { get; set; }
            public string TargetUrl { get; set; }
            public bool Success { get; set; }
            public GetTicket_Fetch_Tickests Result { get; set; }
        }

        public class GetTicket_Fetch_Tickests
        {
            public Tickets_Result[] Tickets { get; set; }
        }

        public class GetTicket_Fetch_Error
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }



        public class Tickets_Result
        {
            public int Vendor { get; set; }
            public int Product { get; set; }
            public string TicketSessionID { get; set; }
            public string TicketID { get; set; }
            public string MemberName { get; set; }

            public string GameType { get; set; }
            public string RoundID { get; set; }

            public decimal Stake { get; set; }
            public decimal StakeMoney { get; set; }

            public string Result { get; set; }
            public string Currency { get; set; }
            public string VendorTicketDateUTC { get; set; }
            public string VendorTicketDate { get; set; }

            public string StatementDate { get; set; }
            public decimal PlayerWinLoss { get; set; }

            public decimal PlayerCommissionAmount { get; set; }
            public string ProcessedTime { get; set; }
            public string CreatedTime { get; set; }
            public string Ip { get; set; }

            public decimal ValidAmount { get; set; }

            public int Type { get; set; }

        }

        public class GetTicket_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string TargetUrl { get; set; }
            public bool Success { get; set; }
            public string Result { get; set; }
        }

       

        public class GetWinLoss_Response
        {
            public int Error { get; set; }
            public string Message { get; set; }
            public string TargetUrl { get; set; }
            public bool Success { get; set; }
            public string Result { get; set; }
        }
    }
}
