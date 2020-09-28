using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;
using CashEntertainment.DB;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace CashEntertainment.Helper
{
    public class Intergration
    {
         //Return Value Guide
        //----------------------
        //0 = Missing Parameters
        //1 = Success
        //2 = Server Error

        //998 Stagging Account
        //private const string PartnerName = "mxm001";
        //private const string PatnerKey = "B83E5368-123A-448F-BF61-89F0F072F68B";
        //private const string ACCOUNT_BASE_URL = "http://api2.data333.com/api/";
        //private const string WALLET_BASE_URL = "http://w2.data333.com/rest/transfer/";
        //private const string OPEN_GAME_BASE_URL = "http://opengameapi.data333.com/api/";
        //private const string GAME_LIST_BASE_URL = "http://gamelistapi.data333.com/api/";
        //private const string WIN_LOSS_BASE_URL = "http://pwlapi.data333.com/api/";

        private const string PartnerName = "P11";
        private const string PatnerKey = "069E1EB9-EED9-409F-8858-CD2A8089961B";
        private const string ACCOUNT_BASE_URL = "http://pauthapi.linkv2.com/api/";
        private const string WALLET_BASE_URL = "http://pwalletapi.linkv2.com/rest/transfer/";
        private const string OPEN_GAME_BASE_URL = "http://opengameapi.linkv2.com/api/";
        private const string GAME_LIST_BASE_URL = "http://gamelistapi.data333.com/api/";
        private const string WIN_LOSS_BASE_URL = "http://pwlapi.linkv2.com/api/";

        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;


        public Intergration(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }
       


        //998 API List Start//
        public async Task<Login_Response> CreateNewPlayer(string strPlayerUserName, string strPlayerFullName, string strPlayerEmail, string strPlayerPhone, string strPlayerDOB, string strPlayerPassword, string Country, int Gender)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "partner/register";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(strPlayerUserName) || string.IsNullOrEmpty(strPlayerPassword))
                {
                    return null;
                }

                string currency_code = "";

                //Based on the country code set the currency code
                currency_code = Country switch
                {
                    "CN" => "CNY",
                    "MY" => "MYR",
                    _ => "MYR",
                };


                var requestbody = new Models_Register_Player_Intergration()
                {
                    Partner = PartnerName,
                    Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    UserName = strPlayerUserName,
                    Password = strPlayerPassword,
                    Fullname = strPlayerFullName,
                    Email = strPlayerEmail,
                    Mobile = strPlayerPhone,
                    DoB = strPlayerDOB,
                    Gender = Gender,
                    Currency = currency_code,

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);


                string FULL_API_PATH = ACCOUNT_BASE_URL + API_PATH;
                //Invoke API
                return JsonConvert.DeserializeObject<Login_Response>(await HttpService.POST(FULL_API_PATH, Content));

            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Create Player Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context =_accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }

        }
        public async Task<Deposit_Response> TransferGameCredit(string strUserName, decimal dcTransferAmount, string strTransactionID)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "deposit";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var requestbody = new Models_Player_TransferCredit_Intergration()
                {
                    Partner = PartnerName,
                    Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    TransactionId = strTransactionID,
                    Player = strUserName,
                    Amount = dcTransferAmount

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);

                string FULL_API_PATH = WALLET_BASE_URL + API_PATH;
                //Invoke API
                return JsonConvert.DeserializeObject<Deposit_Response>(await HttpService.POST(FULL_API_PATH, Content));
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Deposit Game Credit Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }
           
        }
        public  async Task<Withdraw_Response> WithdrawGameCredit(string strUserName, decimal dcTWithdrawAmount, string strTransactionID)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "withdrawal";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var requestbody = new Models_Player_WithdrawCredit_Intergration()
                {
                    Partner = PartnerName,
                    Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    TransactionId = strTransactionID,
                    Player = strUserName,
                    Amount = dcTWithdrawAmount

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);

                string FULL_API_PATH = WALLET_BASE_URL + API_PATH;
                //Invoke API
                return JsonConvert.DeserializeObject<Withdraw_Response>(await HttpService.POST(FULL_API_PATH, Content));

            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Withdraw Game Credit Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }

           
        }
        public  async Task<Balance_Response> GetBalance(string strUserName)
        {

            try
            {
                //PREPARE URL
                string API_PATH = "partner/balance";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(strUserName))
                {

                    return null;
                }

                var requestbody = new Models_Retrieve_Balance_Player_Intergration()
                {
                    Partner = PartnerName,
                    // PartnerName Will Be : PartnerName + strUserName For Creating Sign
                    Sign = HttpService.CreateSignature(PartnerName + strUserName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    UserName = strUserName,

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);
                string FULL_API_PATH = ACCOUNT_BASE_URL + API_PATH;
                //Invoke API

                var data = await HttpService.POST(FULL_API_PATH, Content);
                return JsonConvert.DeserializeObject<Balance_Response>(data);

            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Get Game Credit Balance Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }
        }
        public  async Task<CheckTransaction_Response> CheckTransactionStatus(string strTransactionID)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "check";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var requestbody = new Models_Player_CheckTransaction_Intergration
                {
                    Partner = PartnerName,
                    Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    TransactionId = strTransactionID,

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);

                string FULL_API_PATH = WALLET_BASE_URL + API_PATH;
                //Invoke API
                return JsonConvert.DeserializeObject<CheckTransaction_Response>(await HttpService.POST(FULL_API_PATH, Content));
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Check Transaction Status Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }
           
        }
        public  async Task<Login_Response> RetrievePlayerToken(string strUserName)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "partner/login";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(strUserName))
                {

                    return null;
                }

                var requestbody = new Models_Login_Player_Intergration()
                {
                    Partner = PartnerName,

                    // For Login API the PartnerName will be : partnername + loginid for creating sign
                    Sign = HttpService.CreateSignature(PartnerName + strUserName + "game1234", TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    UserName = strUserName,

                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);
                string FULL_API_PATH = ACCOUNT_BASE_URL + API_PATH;
                //Invoke API

                var data = await HttpService.POST(FULL_API_PATH, Content);
                return JsonConvert.DeserializeObject<Login_Response>(data);

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Token Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }

        }
        public  async Task<Open_Game_Response> OpenGame(string token, string gameVendor, string language, string browser, string gameCode)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "play/login";
                string finalGameCode = "";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(gameVendor) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(browser))
                {

                    return null;
                }

                if (gameCode != null)
                {
                    finalGameCode = gameCode;
                }

                var requestbody = new Models_Player_PlayGame_Intergration()
                {
                    Vendor = gameVendor,
                    Browser = browser,
                    Lang = language,
                    GameCode = finalGameCode,
                    GameHall = "",
                    MerchantCode = "",
                    Ticket = "",
                    SeatId = "",
                    Tag = "",
                    GameProvider = "",
                    Device = "unknown"
                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);
                string FULL_API_PATH = OPEN_GAME_BASE_URL + API_PATH;
                //Invoke API

                var data = await HttpService.GAME_POST(FULL_API_PATH, Content, token);
                return JsonConvert.DeserializeObject<Open_Game_Response>(data);

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Open Game Intergration",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                return null;
            }
        }
        public  async Task<Game_List_Response> GameList(string token, string gameVendor)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "gamelist/find";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(gameVendor))
                {

                    return null;
                }


                string FULL_API_PATH = GAME_LIST_BASE_URL + API_PATH;
                //Invoke API

                var data = await HttpService.GET_GAME(FULL_API_PATH, token, gameVendor);
                return JsonConvert.DeserializeObject<Game_List_Response>(data);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public  async Task<GetTicket_Fetch_Response> GetTicketsByFetch(string strStartTime, string strEndTime)
        {
            try
            {
                //PREPARE URL
                string API_PATH = "tickets/fetch";
                int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (string.IsNullOrEmpty(strStartTime) || string.IsNullOrEmpty(strEndTime))
                {
                    return null;
                }

                var requestbody = new Models_Player_GetTickets_Fetch_Intergration()
                {
                    Partner = PartnerName,
                    Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
                    TimeStamp = TimeStamp,
                    StartTime = strStartTime,
                    EndTime = strEndTime
                };

                //Prepare Body in json format
                string Content = JsonConvert.SerializeObject(requestbody);

                string FULL_API_PATH = WIN_LOSS_BASE_URL + API_PATH;
                //Invoke API

                var data = await HttpService.POST(FULL_API_PATH, Content);
                return JsonConvert.DeserializeObject<GetTicket_Fetch_Response>(data);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public  async Task<GetTicket_Response> GetTicket(string strVendorCode, string strUsername, string strStartTime, string strEndTime)
        //{
        //    try
        //    {
        //        //PREPARE URL
        //        string API_PATH = "tickets/find";
        //        int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //        if (string.IsNullOrEmpty(strVendorCode) || string.IsNullOrEmpty(strUsername) || string.IsNullOrEmpty(strStartTime) || string.IsNullOrEmpty(strEndTime))
        //        {

        //            return null;
        //        }

        //        var requestbody = new Models_Player_GetTickets_Intergration()
        //        {
        //            Partner = PartnerName,
        //            Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
        //            TimeStamp = TimeStamp,
        //            Vendor = strVendorCode,
        //            PlayerName = strUsername,
        //            StartTime = strStartTime,
        //            EndTime = strEndTime
        //        };

        //        //Prepare Body in json format
        //        string Content = JsonConvert.SerializeObject(requestbody);

        //        string FULL_API_PATH = WIN_LOSS_BASE_URL + API_PATH;
        //        //Invoke API



        //        var data = await HttpService.POST(FULL_API_PATH, Content);
        //        return JsonConvert.DeserializeObject<GetTicket_Response>(data);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //public  async Task<GetWinLoss_Response> GetWinLoss(int intProductcode, string strUsername, string strStatementDate)
        //{

        //    try
        //    {
        //        //PREPARE URL
        //        string API_PATH = "winloss/find";
        //        int TimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //        if (intProductcode == 0 || string.IsNullOrEmpty(strUsername) || string.IsNullOrEmpty(strStatementDate))
        //        {

        //            return null;
        //        }

        //        var requestbody = new Models_Player_GetWinLoss_Intergration()
        //        {
        //            Partner = PartnerName,
        //            Sign = HttpService.CreateSignature(PartnerName, TimeStamp, PatnerKey),
        //            TimeStamp = TimeStamp,
        //            Product = intProductcode,
        //            PlayerName = strUsername,
        //            StatementDate = strStatementDate,

        //        };

        //        //Prepare Body in json format
        //        string Content = JsonConvert.SerializeObject(requestbody);

        //        string FULL_API_PATH = WIN_LOSS_BASE_URL + API_PATH;
        //        //Invoke API



        //        var data = await HttpService.POST(FULL_API_PATH, Content);
        //        return JsonConvert.DeserializeObject<GetWinLoss_Response>(data);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //998 API List End//
    }
}
