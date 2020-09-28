using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _998Intergration;
using CashEntertainment.DataAccess;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static CashEntertainment.Models.Models_Admin;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly IRepo_Admin _admin_service;
        public readonly IRepo_User _user_service;
        public readonly IRepo_Bank _bank_service;
        public readonly IRepo_Topup _topup_service;
        public readonly IRepo_Wallet _wallet_service;
        public readonly IRepo_Withdraw _withdraw_service;
        public readonly IRepo_Game _game_service;
        public readonly IRepo_ExchangeRate _exchange_rate_service;
        public readonly IRepo_Settings _settings_service;
        public readonly IRepo_Hierarchy _hierarchy_service;
        public readonly IRepo_Winlose _winlose_service;
        public readonly IRepo_Announcement _announcement_service;
        public readonly JWT _jwt_services;
        public readonly IOptions<Models_AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminController(IRepo_Admin admin_service, IRepo_User user_service, IRepo_Bank bank_service, IRepo_Topup topup_service, IRepo_Wallet wallet_service, IRepo_Withdraw withdraw_service, IRepo_Game game_services, IRepo_ExchangeRate exchange_rate_service, IRepo_Settings settings_service, IRepo_Hierarchy hierarchy_service, IRepo_Winlose winlose_service, IRepo_Announcement announcement_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor, IWebHostEnvironment hostEnvironment)
        {
            _admin_service = admin_service;
            _user_service = user_service;
            _bank_service = bank_service;
            _topup_service = topup_service;
            _wallet_service = wallet_service;
            _withdraw_service = withdraw_service;
            _game_service = game_services;
            _exchange_rate_service = exchange_rate_service;
            _settings_service = settings_service;
            _hierarchy_service = hierarchy_service;
            _jwt_services = jwt_services;
            _winlose_service = winlose_service;
            _announcement_service = announcement_service;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            webHostEnvironment = hostEnvironment;
        }


        //login
        [AllowAnonymous]
        [Route("AdminLogin")]
        [HttpPost]
        public IActionResult AdminLogin(Models_Admin_Login_Request model)
        {

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _user_service.AuthorizeAdminAccount(model.LoginID, model.Password);
            if (result.Item1 == Models_General.SUCC_AUTHORIZE_GRANTED)
            {
                var secrect = _settings.Value.JWTSecret;
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginID, "ADMIN", "ALL", ip, 0, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else
            {

                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }
        }

        //Dashboard Section
        [Route("RetrieveAdminDashboard")]
        [HttpGet]
        public IActionResult RetrieveAdminDashboard()
        {
            var result = new Models_Admin_Dashboard
            {
                TotalMember = _admin_service.RetrieveTotalMember(),
                TotalTopup = _admin_service.RetrieveTotalTopUp(),
                TotalWithdrawal = _admin_service.RetrieveTotalWithdrawal(),
                ChartData = _admin_service.ChartData()
            };
        
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }

        //Member Section
        [Route("RetrieveMemberListing")]
        [HttpGet]
        public IActionResult RetrieveMemberListing()
        {
            var result = _user_service.RetrieveAllMemberListing();
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }

        [Route("InactiveMember")]
        [HttpPost]
        public IActionResult InactiveMember(Models_Admin_Inactive_Member_Request model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _user_service.InactiveMember(model.MemberSrno, model.Status);
            if (result == Models_General.SUCC_INACTIVE_MEMBER|| result == Models_General.SUCC_ACTIVE_MEMBER)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        [Route("ChangeMemberPassword")]
        [HttpPost]
        public IActionResult ChangeMemberPassword(Models_Admin_Change_Member_Password_Request model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var AdminLoginID = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "LoginId").FirstOrDefault().Value;
            var result = _user_service.ChangeMemberPassword(AdminLoginID,model.MemberSrno, model.NewPassword);
            if (result == Models_General.SUCC_CHANGE_MEMBER_PASSWORD)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        //ExchangeRate Section

        [Route("RetrieveExchangeRateListing")]
        [HttpGet]
        public IActionResult RetrieveExchangeRateListing()
        {

            var result = _exchange_rate_service.RetrieveExchangeRateListing();
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }




        [Route("CreateExchangeRate")]
        [HttpPost]
        public IActionResult CreateExchangeRate( Models_Create_Exchange_Rate_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var result = _exchange_rate_service.CreateExchangeRate(_httpContextAccessor.HttpContext.User.Identity.Name, model.Base, model.Currency, model.Rate);
            if (result == Models_General.SUCC_ADMIN_CREATE_EXCHANGE_RATE)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }



        [Route("UpdateExchangeRate")]
        [HttpPost]
        public IActionResult UpdateExchangeRate(Models_Update_Exchange_Rate_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var result = _exchange_rate_service.UpdateExchangeRate( model.Srno, _httpContextAccessor.HttpContext.User.Identity.Name, model.Base, model.Currency, model.Rate);
            if (result == Models_General.SUCC_ADMIN_UPDATE_EXCHANGE_RATE)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        //Topup Section

        [Route("TopupApproval")]
        [HttpPost]
        public IActionResult AdminTopupApproval(Models_Topup_Approve_Request model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _topup_service.AdminTopupApproval(_httpContextAccessor.HttpContext.User.Identity.Name, model.Srno, model.ApproveStatus, model.Remarks);

            if (result == Models_General.SUCC_ADMIN_APPROVE_TOPUP || result == Models_General.SUCC_ADMIN_REJECT_TOPUP)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }



        }

        [Route("AdminTopupListing")]
        [HttpGet]
        public IActionResult AdminTopupListing()
        {
            var result = _topup_service.AdminRetrieveTopupListing();
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }

        }


        [Route("EditMainWalletCredit")]
        [HttpPost]
        public IActionResult EditMainWalletCredit(Models_Manually_Edit_Cash_Wallet_Request model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _wallet_service.EditMainWalletCredit(_httpContextAccessor.HttpContext.User.Identity.Name, model.MemberSrno, model.ManipulateType, model.ManipulateAmount, model.TurnoverAmount);

            if (result == Models_General.SUCC_EDIT_CASH_WALLET)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        //Withdraw Section
        [Route("WithdrawalApproval")]
        [HttpPost]
        public IActionResult WithdrawalApproval(Models_Admin_Wihtdrawal_Approval_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _withdraw_service.AdminApproveWithdrawal(_httpContextAccessor.HttpContext.User.Identity.Name, model.WithdrawalSrno, model.ApprovalStatus, model.Remarks);
            if (result == Models_General.SUCC_ADMIN_APPROVE_WITHDRAWAL || result == Models_General.SUCC_ADMIN_REJECT_WITHDRAWAL)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }


        [Route("AdminWithdrawalListing")]
        [HttpGet]
        public IActionResult AdminWithdrawalListing()
        {

            var result = _withdraw_service.AdminRetrieveWithdrawalListing();
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }



        //Bank Section
        [AllowAnonymous]
        [Route("RetrieveAdminBankList")]
        [HttpGet]
        public IActionResult RetrieveAdminBankList(string CountryCode)
        {

            var result = _bank_service.RetrieveAdminBankList();
            // Using ModelState to help validate the input from the end user 
            if (!string.IsNullOrEmpty(CountryCode))
            {
                result = _bank_service.RetrieveAdminBankList().Where(x => x.Country == CountryCode).ToList();
                if (result != null)
                {

                    return Ok(new Resp(HttpStatusCode.OK, 0, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
                }
            }
            else
            {
                if (result != null)
                {

                    return Ok(new Resp(HttpStatusCode.OK, 0, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
                }
            }
        }

        [Route("AdminAddNewBank")]
        [HttpPost]
        public IActionResult AdminAddNewBank(Modesl_Admin_Add_New_Bank_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _bank_service.AdminAddNewBank(model.BankSrno, model.BankAccountHolderName, model.BankCardNo, model.Country);
            if (result == Models_General.SUCC_ADMIN_ADD_BANK)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("AdminUpdateBank")]
        [HttpPost]
        public IActionResult AdminUpdateBank(Modesl_Admin_Update_Bank_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _bank_service.AdminUpdateBank(model.AdminBankSrno, model.BankSrno, model.BankAccountHolderName, model.BankCardNo, model.Country);
            if (result == Models_General.SUCC_ADMIN_UPDATE_BANK)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        //Game Section

        [Route("ChangeGameMaintenance")]
        [HttpPost]
        public IActionResult ChangeGameMaintenance(Models_Change_Game_Maintenance_Request model)
        {

            var result = _game_service.ChangeGameMaintenance(model.Gamesrno, model.Maintenancestatus);
            if (result == Models_General.SUCC_ADMIN_CHANGE_MAINTENANCE)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }


        //General Setting 


        [Route("RetrieveSettingsListing")]
        [HttpGet]
        public IActionResult RetrieveSettingsListing()
        {

            var result = _settings_service.RetrieveSettingsListing();
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }


        [Route("UpdateSettings")]
        [HttpPost]
        public IActionResult UpdateSettings(Models_Update_Settings_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var result = _settings_service.UpdateSettings(model.Srno, _httpContextAccessor.HttpContext.User.Identity.Name, model.SettingValue);
            if (result == Models_General.SUCC_ADMIN_UPDATE_SETTINGS)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        //hierarchy

        [Route("GetDownlineUsersListing")]
        [HttpGet]
        public IActionResult GetDownlineUsersListing(long UserId)
        {

            var result = _hierarchy_service.GetDownlineUsers(UserId);
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }

        }



        //winlose

        [Route("RetrieveAdminWinLoseListing")]
        [HttpPost]
        public IActionResult RetrieveAdminWinLoseListing(Models_Player_Winlose_Filter_Request model)
        {

            // Using ModelState to help validate the input from the end user 

            var result = _winlose_service.RetrieveWinloseListByFilter(model.LoginId, model.Vendor, model.Startdate, model.Enddate);
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }

        //wallet

        [Route("UpdateWalletTurnover")]
        [HttpPost]
        public IActionResult UpdateWalletTurnover(Models_Update_Turnover_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var result = _wallet_service.UpdateWalletTurnover(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.MemberSrno, model.TurnoverAmount);
            if (result == Models_General.SUCC_CHANGE_MEMBER_TURNOVER)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }



        [Route("RetrieveMemberTrackingWalletListing")]
        [HttpGet]
        public IActionResult RetrieveMemberTrackingWalletListing()
        {

            // Using ModelState to help validate the input from the end user 
                var result = _wallet_service.RetrieveTrackingWalletList();
                if (result != null)
                {

                    return Ok(new Resp(HttpStatusCode.OK, 0, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
                }
          
        }


        //Announcement

        [Route("CreateAnnouncement")]
        [HttpPost]
        public IActionResult AdminCreateAnnouncement([FromForm] Models_Create_Annoucement_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var base_url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value.ToString()}{_httpContextAccessor.HttpContext.Request.PathBase.Value.ToString()}";

            var result = _announcement_service.CreateAnnoucement(_httpContextAccessor.HttpContext.User.Identity.Name, model.TitleEN, model.TitleCN, model.TitleMS, model.AnnouncementContentEN, model.AnnouncementContentCN, model.AnnouncementContentMS, model.IsPublish, model.IsImagePublish, webHostEnvironment.WebRootPath, base_url, model.AnnouncementImg);
            if (result == Models_General.SUCC_ADMIN_CREATE_ANNOUNCEMENT)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("UpdateAnnouncement")]
        [HttpPost]
        public IActionResult AdminUpdateAnnouncement([FromForm] Models_Update_Annoucement_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            //Retrieve Current Domain Path
            var base_url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value.ToString()}{_httpContextAccessor.HttpContext.Request.PathBase.Value.ToString()}";

            var result = _announcement_service.UpdateAnnoucement(model.AnnoucementSrno, _httpContextAccessor.HttpContext.User.Identity.Name, model.TitleEN, model.TitleCN, model.TitleMS, model.AnnouncementContentEN, model.AnnouncementContentCN, model.AnnouncementContentMS, model.IsPublish, model.IsImagePublish, webHostEnvironment.WebRootPath, base_url, model.AnnouncementImg);
            if (result == Models_General.SUCC_ADMIN_UPDATE_ANNOUNCEMENT)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        [Route("DeleteAnnouncement")]
        [HttpPost]
        public IActionResult AdminDeleteAnnouncement([FromForm] Models_Delete_Annoucement_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _announcement_service.DeleteAnnoucement(model.AnnoucementSrno);
            if (result == Models_General.SUCC_ADMIN_DELETE_ANNOUNCEMENT)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        [Route("AdminAnnoucementListing")]
        [HttpGet]
        public IActionResult AdminAnnoucementListing()
        {
            var result = _announcement_service.RetrieveAnnouncementListing();
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }


        //log

        [Route("RetrieveAdminTrackingWalletLogListing")]
        [HttpPost]
        public IActionResult RetrieveAdminTrackingWalletLogListing(Models_Trackin_Wallet_Log_Filter_Request model)
        {

            // Using ModelState to help validate the input from the end user
            var result = _wallet_service.RetrieveTrackingWalletLogListByFilter(model.LoginId, model.TransactionType, model.Startdate, model.Enddate);

            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }



        [Route("RetrieveAdminUserLoginLogListing")]
        [HttpPost]
        public IActionResult RetrieveAdminUserLoginLogListing(Models_User_Login_Log_Filter_Request model)
        {
            // Using ModelState to help validate the input from the end user
            var result = _user_service.RetrieveUserLoginLogListByFilter(model.LoginId,  model.Startdate, model.Enddate);

            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }



    }
}