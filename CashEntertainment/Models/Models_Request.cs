using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Request
    {
        public class Models_Register_Request
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Must be alpanumeric with at least minimum 6 characters without any symbolic")]
            public string LoginId { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "Password Length must be at least 6 characters")]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Phonenumber { get; set; }
            [Required]
            public string CountryCode { get; set; }

            [Required]
            [RegularExpression(@"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))", ErrorMessage = "Invalid DoB format please use yyyy-MM-dd format for the DoB")]
            public string DOB { get; set; }

            [Required]
            public int Gender { get; set; }
            public string Upline { get; set; }
        }


        public class Models_Twelve_Register_Request
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Must be alpanumeric with at least minimum 6 characters without any symbolic")]
            public string LoginId { get; set; }


            [Required]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "Password Length must be at least 6 characters")]
            public string Password { get; set; }


            [Required]
            public string CountryCode { get; set; }

            public string Upline { get; set; }
        }

        public class Models_ChangePassword_Request
        {
            [Required]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "Password Length must be at least 6 characters")]
            public string NewPassword { get; set; }



            [Required]
            public string CurrentPassword { get; set; }
        }

        public class Models_UpdateProfile_Request
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Phonenumber { get; set; }

            [Required]
            [RegularExpression(@"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))", ErrorMessage = "Invalid DoB format please use yyyy-MM-dd format for the DoB")]
            public string DOB { get; set; }


            [Required]
            public int Gender { get; set; }
        }
        public class Models_Login_Request
        {
            [Required]
            public string LoginID { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string LoginCountry { get; set; }
        }
        public class Models_Topup_Request_Online_Banking
        {

            [Required]
            public long BankSrno { get; set; }
            [Required]
            public decimal TopupAmount { get; set; }
            [Required]
            public IFormFile TopupImageProof { get; set; }

            [Required]
            public string TransactionReferenceNumber { get; set; }

            [Required]
            public string Currency { get; set; }
        }

        public class Models_Topup_Request_Crypto
        {

            [Required]
            public decimal TopupAmount { get; set; }
            [Required]
            public string TransactionHash { get; set; }

            [Required]
            public string Currency { get; set; }

            [Required]
            public decimal Rate { get; set; }
        }

        public class Models_Topup_Approve_Request
        {
            [Required]
            public long Srno { get; set; }
            [Required]
            public bool ApproveStatus { get; set; }
            public string Remarks { get; set; }

        }


        public class Models_Manually_Edit_Cash_Wallet_Request
        {
            [Required]
            public long MemberSrno { get; set; }
            [Required]
            public int ManipulateType { get; set; }

            [Required]
            public decimal ManipulateAmount { get; set; }

            [Required]
            public decimal TurnoverAmount { get; set; }
        }


        public class Models_Twelve_Topup
        {
            [Required]
            public string LoginId { get; set; }

            [Required]
            public decimal TopupAmount { get; set; }

            [Required]
            public bool IsReset { get; set; } = false;

        }

        public class Models_Member_Topup_Listing_Request
        {
            [Required]
            public string LoginID { get; set; }
        }
        public class Models_Withdrawal_Request_Online_Banking
        {
            [Required]
            public decimal WihtdrawalAmount { get; set; }
            [Required]
            public long BankSrno { get; set; }
            [Required]
            public string Currency { get; set; }
        }

        public class Models_Withdrawal_Request_Crypto
        {
            [Required]
            public decimal WihtdrawalAmount { get; set; }
            [Required]
            public string ToAddress { get; set; }
            [Required]
            public decimal Rate { get; set; }
            [Required]
            public string Currency { get; set; }
        }


        public class Models_Admin_Wihtdrawal_Approval_Request
        {
            [Required]
            public long WithdrawalSrno { get; set; }
            [Required]
            public bool ApprovalStatus { get; set; }

            public string Remarks { get; set; }
        }
        public class Models_Member_Withdrawal_Listing_Request
        {
            [Required]
            public string LoginID { get; set; }
        }
        public class Models_Create_Annoucement_Request
        {
            [Required]
            public string TitleEN { get; set; }
            [Required]
            public string TitleCN { get; set; }
            [Required]
            public string TitleMS { get; set; }
            [Required]
            public string AnnouncementContentEN { get; set; }
            [Required]
            public string AnnouncementContentCN { get; set; }
            [Required]
            public string AnnouncementContentMS { get; set; }
            [Required]
            public bool IsPublish { get; set; }
            [Required]
            public bool IsImagePublish { get; set; }
            public IFormFile AnnouncementImg { get; set; }
        }
        public class Models_Update_Annoucement_Request
        {
            [Required]
            public long AnnoucementSrno { get; set; }
            [Required]
            public string TitleEN { get; set; }
            [Required]
            public string TitleCN { get; set; }

            [Required]
            public string TitleMS { get; set; }
            [Required]
            public string AnnouncementContentEN { get; set; }
            [Required]
            public string AnnouncementContentCN { get; set; }

            [Required]
            public string AnnouncementContentMS { get; set; }
            [Required]
            public bool IsPublish { get; set; }

            [Required]
            public bool IsImagePublish { get; set; }
            public IFormFile AnnouncementImg { get; set; }
        }


        public class Models_Delete_Annoucement_Request
        {
            [Required]
            public long AnnoucementSrno { get; set; }
        }



        public class Models_Create_Banner_Request
        {
            [Required]
            public IFormFile BannerImage { get; set; }
            public string RedirectURL { get; set; }
            [Required]
            public bool IsActive { get; set; }
        }
        public class Models_Update_Banner_Request
        {
            [Required]
            public long BannerSrno { get; set; }
            [Required]
            public IFormFile BannerImage { get; set; }
            public string RedirectURL { get; set; }
            [Required]
            public bool IsActive { get; set; }
        }





        public class Modesl_Member_Add_New_Bank_Request
        {
            [Required]
            public long BankSrno { get; set; }
            [Required]
            public string BankAccountHolderName { get; set; }
            [Required]
            public string BankCardNo { get; set; }
        }

        public class Modesl_Member_Update_Bank_Request
        {

            [Required]
            public long UserBankSrno { get; set; }
            [Required]
            public long BankSrno { get; set; }
            [Required]
            public string BankAccountHolderName { get; set; }
            [Required]
            public string BankCardNo { get; set; }
        }

        public class Modesl_Admin_Add_New_Bank_Request
        {
            [Required]

            public long BankSrno { get; set; }
            [Required]
            public string BankAccountHolderName { get; set; }
            [Required]
            public string BankCardNo { get; set; }
            [Required]
            public string Country { get; set; }
        }

        public class Modesl_Admin_Update_Bank_Request
        {

            [Required]
            public long AdminBankSrno { get; set; }
            [Required]
            public long BankSrno { get; set; }
            [Required]
            public string BankAccountHolderName { get; set; }
            [Required]
            public string BankCardNo { get; set; }
            [Required]
            public string Country { get; set; }
        }


        public class Models_Admin_Login_Request
        {
            [Required]
            public string LoginID { get; set; }
            [Required]
            public string Password { get; set; }

        }

        public class Models_Admin_Inactive_Member_Request
        {
            [Required]
            public long MemberSrno { get; set; }
            [Required]
            public bool Status { get; set; }
        }

        public class Models_Admin_Change_Member_Password_Request
        {
            [Required]
            public long MemberSrno { get; set; }
            [Required]
            public string NewPassword { get; set; }
        }


        public class Models_Play_Game_Request
        {
            [Required]
            public string VendorCode { get; set; }
            [Required]
            public string browserType { get; set; }

            public string GameCode { get; set; }
        }


        public class Models_Transfer_Credit_Request
        {
            [Required]
            public decimal TransferAmount { get; set; }

        }

        public class Models_Player_Transfer_Credit_Request
        {
            [Required]
            public string RefCode { get; set; }

            [Required]
            public decimal TransferAmount { get; set; }

        }


        public class Models_Withdrawal_Credit_Request
        {
            [Required]
            public decimal WithdrawalAmount { get; set; }
        }

        public class Models_Change_Game_Maintenance_Request
        {
            [Required]
            public long Gamesrno { get; set; }

            [Required]
            public byte Maintenancestatus { get; set; }
        }


        public class Models_PaymentResponse_Request
        {
            [Required]
            public string Merchant { get; set; }
            [Required]
            public string Reference { get; set; }
            [Required]
            public string Currency { get; set; }
            [Required]
            public string Amount { get; set; }
            [Required]
            public string Language { get; set; }
            [Required]
            public string Customer { get; set; }
            [Required]
            public string Datetime { get; set; }
            [Required]
            public string Note { get; set; }
            [Required]
            public string Key { get; set; }
            [Required]
            public string Status { get; set; }

            [Required]
            public string ID { get; set; }
        }


        public class Models_PaymentRequest_Request
        {
            [Required]
            public string DepositUrl { get; set; }

            [Required]
            public string Merchant { get; set; }
            [Required]
            public string Customer { get; set; }

            [Required]
            public string Currency { get; set; }

            [Required]
            public string Reference { get; set; }

            [Required]
            public string Key { get; set; }


            [Required]
            public decimal Amount { get; set; }


            [Required]
            public string Note { get; set; }


            [Required]
            public string Datetime { get; set; }

            [Required]
            public string FrontUrl { get; set; }

            [Required]
            public string BackUrl { get; set; }

            [Required]
            public string Language { get; set; }

            [Required]
            public string Bank { get; set; }

            [Required]
            public string ClientIp { get; set; }

        }



        public class Models_Create_Exchange_Rate_Request
        {
            [Required]
            public string Base { get; set; }

            [Required]
            public string Currency { get; set; }

            [Required]
            public decimal Rate { get; set; }

        }

        public class Models_Update_Exchange_Rate_Request
        {
            [Required]
            public long Srno { get; set; }
            [Required]
            public string Base { get; set; }

            [Required]
            public string Currency { get; set; }

            [Required]
            public decimal Rate { get; set; }
        }



        public class Models_Update_Settings_Request
        {
            [Required]
            public long Srno { get; set; }
            [Required]
            public string SettingValue { get; set; }


        }

        public class Models_Twelve_Change_Upline_Request
        {
            [Required]
            public string LoginId { get; set; }

            public string Upline { get; set; }
        }


        public class Models_Update_Turnover_Request
        {
            [Required]
            public long MemberSrno { get; set; }
            [Required]
            public decimal TurnoverAmount { get; set; }
        }

        public class Models_Twelve_Turnover_Request
        {
            [Required]
            [MaxLength(1000, ErrorMessage = "Maximum 1000 string in Array")]
            public string[] LoginIds { get; set; }
        }

        public class Models_Player_Winlose_Filter_Request
        {
            public string LoginId { get; set; } = null;
            public int Vendor { get; set; } = 0;

            [Required]
            public string Startdate { get; set; }

            [Required]
            public string Enddate { get; set; }

        }

        public class Models_Trackin_Wallet_Log_Filter_Request
        {
            public string LoginId { get; set; } = null;
            public int TransactionType { get; set; } = -1;

            [Required]
            public string Startdate { get; set; }

            [Required]
            public string Enddate { get; set; }

        }


        public class Models_User_Login_Log_Filter_Request
        {

        public string LoginId { get; set; } = null;

        [Required]
        public string Startdate { get; set; }

        [Required]
        public string Enddate { get; set; }
    }


        public class Models_Register_Player_Intergration
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

        public class Models_Login_Player_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UserName { get; set; }

        }

        public class Models_Retrieve_Balance_Player_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string UserName { get; set; }
        }

        public class Models_Player_PlayGame_Intergration
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


        public class Models_Player_TransferCredit_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
        }

        public class Models_Player_WithdrawCredit_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }
            public string Player { get; set; }
            public decimal Amount { get; set; }
        }

        public class Models_Player_CheckTransaction_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string TransactionId { get; set; }

        }

        public class Models_Player_GetTickets_Fetch_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }


        public class Models_Player_GetTickets_Intergration
        {
            public string Partner { get; set; }
            public string Sign { get; set; }
            public long TimeStamp { get; set; }
            public string Vendor { get; set; }
            public string PlayerName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }

        public class Models_Player_GetWinLoss_Intergration
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
