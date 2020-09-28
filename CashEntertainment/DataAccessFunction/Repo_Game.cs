
using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_Games;
using CashEntertainment.Helper;

namespace CashEntertainment.DataAccess
{
    public class Repo_Game : IRepo_Game
    {
        private readonly UAT_CasinoContext _db;
        private readonly Common _common_services;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;

        public Repo_Game(UAT_CasinoContext db, Common common_services, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _common_services = common_services;
            _accessor = accessor;
            _intergration = intergration;
        }


        public async Task<Tuple<int,string>> LoginGame(string VendorCode, long MemberSrno, string browserType, string language, string gameCode)
        {
            try {

                var user_game_account = _db.MstUserGameAccount.Where(x => x.MemberSrno == MemberSrno).FirstOrDefault();


            var access_token = await _intergration.RetrievePlayerToken(user_game_account.GameId);

            if (access_token.Error >=0 )
            {

                var game_url = await _intergration.OpenGame(access_token.Token, VendorCode, language, browserType, gameCode);

                if (game_url.Success)
                {
                    return new Tuple<int, string>(Models_General.SUCC_OPEN_GAME, game_url.Result.Data);
                }

               return new Tuple<int, string>(Models_General.ERR_CANNOT_LOGIN_GAME, "");
            }


                return new Tuple<int, string>(Models_General.ERR_GAME_TOKEN_NOT_FOUND, "");

            }
            catch (Exception ex)
            {
               
                var new_error = new LogErrorSystem
                {
                    Title = "Login Game",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new  Tuple<int, string>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, "");
            }
        }

        public List<GameDescription> RetrieveGameListing()
        {
            try
            {
                var data = (from t1 in _db.MstGame.Where(x => x.Status == "ACTIVE")
                            select new GameDescription
                            {
                                Srno = t1.Srno,
                                GameCategory = t1.GameCategory,
                                VendorCode = t1.VendorCode,
                                VendorName = t1.VendorName,
                                ImageUrl = t1.VendorImageUrl,
                                Status = t1.Status,
                                Maintenance = t1.Maintenance,
                                IsSubgame = t1.IsSubgame,
                            }).ToList();

                return data;
    
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Game Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public List<SubGameDescription> RetrieveSubGameListing()
        {
            try
            {
               var data = (from t1 in _db.MstSubGame.Where(x => x.Status == "ACTIVE")
                 select new SubGameDescription
                 {
                     GameType = t1.GameType,
                     VendorCode = t1.VendorCode,
                     GameName = t1.GameName,
                     GameCode = t1.GameCode,
                     ImageUrl = t1.GameImageUrl,
                     Status = t1.Status,
                 }).ToList();

                return data;
            
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Sub Game Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public int ChangeGameMaintenance(long Gamesrno, byte Maintenancestatus)
        {
            try
            {
                //Retrieve The Following Maintenance Details And Check Is it Valid
                var GameDetails = _db.MstGame.Where(x => x.Srno == Gamesrno).FirstOrDefault();
                //Declare a default result;
                int result;
                if (GameDetails != null)
                {
                    // Check The Approval Status For The Maintenance
                    if (Maintenancestatus != GameDetails.Maintenance)
                    {
                        //Update The Withdrawal Details and Set Status to APPROVED
                        GameDetails.Maintenance = Maintenancestatus;

                        result = Models_General.SUCC_ADMIN_CHANGE_MAINTENANCE;
                    }
                    else
                    {
 
                        result = Models_General.ERR_REJECT_CHANGE_MAINTENANCE;
                    }
                    _db.SaveChanges();
                    return result;
                }
                return Models_General.ERR_GAME_NOT_FOUND;
            }

            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Change Game Maintenance",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public async Task<int> GetTicketsByFetch()
        {
            using var dbContextTransaction = _db.Database.BeginTransaction();
            try
            {
                int result;
                var dt = DateTime.UtcNow;
                var strStartTime = dt.AddSeconds(-30).ToString("yyyy-MM-dd HH:mm:ss");
                var strEndTime = dt.ToString("yyyy-MM-dd HH:mm:ss");



                var data = await _intergration.GetTicketsByFetch(strStartTime, strEndTime);

                if (data.Success == true)
                {
                    if (data.Result.Tickets.Length > 0)
                    {
                        foreach (var ticket in data.Result.Tickets)
                        {

                            var userfound = _db.MstUserGameAccount.Where(x => x.GameId == ticket.MemberName).FirstOrDefault();
                            var userwallet = _db.MstUserWallet.Where(x => x.MemberSrno == userfound.MemberSrno).FirstOrDefault();

                            var log_tickets = new LogTickets
                            {
                                MemberSrno = userfound.MemberSrno,
                                TicketSessionId = ticket.TicketSessionID,
                                TicketId = ticket.TicketID,
                                GameType = ticket.GameType,
                                RoundId = ticket.RoundID,
                                Stake = ticket.Stake,
                                StakeMoney = ticket.StakeMoney,
                                Result = ticket.Result,
                                Currency = ticket.Currency,
                                StatementDate = Convert.ToDateTime(ticket.StatementDate),
                                PlayerWinLoss = ticket.PlayerWinLoss,
                                Vendor = ticket.Vendor,
                                Product = ticket.Product,
                                CreatedDateTime = DateTime.Now,
                            };

                            var winlose = new MstWinlose
                            {
                                MemberSrno = userfound.MemberSrno,
                                WinloseAmount = ticket.PlayerWinLoss,
                                StakeAmount = ticket.StakeMoney,
                                Vendor = ticket.Vendor,
                                Currency = ticket.Currency,
                                GameType = ticket.GameType,
                                Product = ticket.Product,
                                CreatedDateTime = DateTime.Now,

                            };

                            if (userwallet.TurnoverAmount > 0)
                            {
                                if (Math.Abs(ticket.PlayerWinLoss) > userwallet.TurnoverAmount)
                                {
                                    userwallet.TurnoverAmount = 0;
                                }
                                else
                                {
                                    userwallet.TurnoverAmount -= Math.Abs(ticket.PlayerWinLoss);
                                }
                            }


                            if (userwallet.TwelveTurnoverAmount > 0)
                            {
                                if (Math.Abs(ticket.PlayerWinLoss) > userwallet.TwelveTurnoverAmount)
                                {
                                    userwallet.TwelveTurnoverAmount = 0;
                                }
                                else
                                {
                                    userwallet.TwelveTurnoverAmount -= Math.Abs(ticket.PlayerWinLoss);
                                }
                            }


                            _db.MstWinlose.Add(winlose);
                            _db.LogTickets.Add(log_tickets);
                            await _db.SaveChangesAsync();
                        }
                    }

               
                    dbContextTransaction.Commit();
                    result = Models_General.SUCC_GET_TICKETS;
                    return result;
                }


                dbContextTransaction.Rollback();
                return Models_General.ERR_GET_TICKETS;
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();

                var new_error = new LogErrorSystem
                {
                    Title = "Get Tickets By Fetch",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public async Task<int> UpdateGameWalletAmounts()
        {
            using var dbContextTransaction = _db.Database.BeginTransaction();
            try
            {

                var UserGameAccounts = _db.MstUserGameAccount.ToList();

                foreach (var useraccount in UserGameAccounts)
                {

                    var balance = await _intergration.GetBalance(useraccount.GameId);
                 
                    if (balance.Error == 0)
                    {
                        var founduserwallet = _db.MstUserGameWallet.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();

                        if (founduserwallet != null)
                        {
                            founduserwallet.GameCredit = balance.Balance;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                await _db.SaveChangesAsync();
                dbContextTransaction.Commit();
                return Models_General.SUCC_GET_ALL_GAMER_WALLET_BALANCE;
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();

                var new_error = new LogErrorSystem
                {
                    Title = "Update Game Wallets Account",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public async Task<int> RecollectGameWalletAmounts()
        {
            try
            {
                var UserGameAccounts = _db.MstUserGameAccount.ToList();

                foreach (var useraccount in UserGameAccounts)
                {

                    var balance = await _intergration.GetBalance(useraccount.GameId);

             

                    if (balance.Error == 0)
                    {
                        var founduserwallet = _db.MstUserWallet.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();

                        if (founduserwallet != null)
                        {

                            var TransactionID = new Guid().ToString();
                            var result = await _intergration.WithdrawGameCredit(useraccount.GameId, balance.Balance, TransactionID);
                           
                            var UserDetails = _db.MstUserAccount.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();
                            if (result.Error == 0)
                            {
                                // add new log from cash credit transaction
                                var log_user_tracking_wallet = new LogUserTrackingWallet
                                {
                                    MemberSrno = useraccount.MemberSrno,
                                    WalletFrom = "GAME WALLET",
                                    WalletTo = "CASH WALLET",
                                    TransactionType = 5,
                                    PreviousAmount = founduserwallet.CashCredit,
                                    TransactionAmount = balance.Balance,
                                    CurrentTotalAmount = founduserwallet.CashCredit + balance.Balance,
                                    IsDeduct = false,
                                    Description = string.Format("MEMBER:{0} WITHDRAWAL GAME CREDIT INTO CASH WALLET WITH AMOUNT:{1} AT:{2}", UserDetails.LoginId, balance.Balance, DateTime.Now),
                                    CreatedDateTime = DateTime.Now,

                                };

                                //add new log for game credit transaction
                                var log_user_game_credit_transaction = new LogUserGameCreditTransaction
                                {
                                    GameApi = "998 API",
                                    TrasactionId = TransactionID,
                                    MemberSrno = useraccount.MemberSrno,
                                    Player = useraccount.GameId,
                                    TransferAmount = result.Amount,
                                    BeforeAmount = result.Before,
                                    AfterAmount = result.After,
                                    TransactionType = "WITHDRAWAL",
                                    Status = "SUCCESS",
                                    TransferDate = DateTime.Now
                                };

                                founduserwallet.CashCredit += balance.Balance;
                                _db.LogUserTrackingWallet.Add(log_user_tracking_wallet);
                                _db.LogUserGameCreditTransaction.Add(log_user_game_credit_transaction);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }



                await _db.SaveChangesAsync();
                return Models_General.SUCC_RECOLLECT_ALL_GAMER_WALLET_BALANCE;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Recollect Game Wallets Amount",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

         public async Task<Tuple<int, int>> CheckUserGameRegister()
        {
            try
            {
                var UserAccounts = _db.MstUserAccount.ToList();
                int count = 0;

                foreach (var useraccount in UserAccounts)
                {

                    if(useraccount.GameRegister == 2)
                    {
                        var UserDetails = _db.MstUser.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();
                        var GameAccountDetails = _db.MstUserGameAccount.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();
                        var GameWalletDetails = _db.MstUserGameWallet.Where(x => x.MemberSrno == useraccount.MemberSrno).FirstOrDefault();
                        bool temp_result = false;
                        do
                        {

                            int temp = 1;
                            var RandomCode = "";
                            do
                            {

                                var tempcode = _common_services.GenerateRandomNumber(10);
                                if (IsRefcodeExist(tempcode))
                                {
                                    temp = 1;
                                }
                                else
                                {
                                    RandomCode = tempcode;
                                    temp = 0;
                                }
                            }
                            while (temp == 1);

                            var gamelogin = "egg" + RandomCode;

                            var result = await _intergration.CreateNewPlayer(gamelogin, UserDetails.Name, UserDetails.Email, UserDetails.Phone, UserDetails.DoB, GameAccountDetails.GamePassword, UserDetails.Country, UserDetails.Gender);



                            if (result.Error == 0)
                            {
                                //API Successfully Created an Account from the thrid party side then only allow to create an account into our database
                                var new_recreate_user = new LogRecreateUser
                                {
                                    MemberSrno = GameAccountDetails.MemberSrno,
                                    LoginId = useraccount.LoginId,
                                    PreviousGameId = GameAccountDetails.GameId,
                                    PreviousGamePassword = GameAccountDetails.GamePassword,
                                    CurrentGameId = gamelogin,
                                    CurrentGamePassword = GameAccountDetails.GamePassword,
                                    Status = true,
                                    StatusCode = result.Error,
                                    Message = result.Message,
                                    CreatedDateTime = DateTime.Now,
                                };
                                _db.LogRecreateUser.Add(new_recreate_user);

                                temp_result = true;
                                useraccount.GameRegister = 1;
                                GameAccountDetails.GameId = gamelogin;
                                GameWalletDetails.GameId = gamelogin;
                                UserDetails.RefCode = RandomCode;

                                count++;
                            }
                            else
                            {
                                var new_recreate_user_error = new LogRecreateUser
                                {
                                    MemberSrno = GameAccountDetails.MemberSrno,
                                    LoginId = useraccount.LoginId,
                                    PreviousGameId = GameAccountDetails.GameId,
                                    PreviousGamePassword = GameAccountDetails.GamePassword,
                                    CurrentGameId = gamelogin,
                                    CurrentGamePassword = GameAccountDetails.GamePassword,
                                    Status = false,
                                    StatusCode = result.Error,
                                    Message = result.Message,
                                    CreatedDateTime = DateTime.Now,
                                };
                                _db.LogRecreateUser.Add(new_recreate_user_error);
                            }

                        } while (temp_result == false);
                    }
                }
                await _db.SaveChangesAsync();
                return new Tuple<int, int>(Models_General.SUCC_CHECK_USER_GAME_REGISTER, count);
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Check User Game Account",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, int>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, 0);
            }
        }

        private bool IsRefcodeExist(string refcode)
        {
            return _db.MstUser.Where(x => x.RefCode.Equals(refcode.ToLower())).Any();
        }

    }
}

