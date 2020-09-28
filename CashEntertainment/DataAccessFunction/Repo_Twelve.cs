
using CashEntertainment.DB;
using CashEntertainment.Models;
using CashEntertainment.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_Twelve;


namespace CashEntertainment.DataAccess
{
    public class Repo_Twelve : IRepo_Twelve
    {

        private readonly UAT_CasinoContext _db;
        private readonly Common _common_services;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;
        public Repo_Twelve(UAT_CasinoContext db, Common common_services, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _common_services = common_services;
            _accessor = accessor;
            _intergration = intergration;
        }

        public async Task<Tuple<int, long>> UserRegisterNewAccount(string LoginID,  string Password, string CountryCode, string Upline)
        {

            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
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

                    if(Upline != null && Upline != "" && !IsUsernameExist(Upline))
                    {
                        return new Tuple<int, long>(Models_General.ERR_UPLINE_REFERCODE_NOT_EXIST, 0);
                    }

                    if (IsUsernameExist(LoginID))
                    {
                        return new Tuple<int, long>(Models_General.ERR_USERNAME_EXIST, 0);
                    }

                    var UserDetails = new MstUser { };


                    //This will be the main table that all references will be linked to this MemberSrno so we need it to be the first table to store in our database and get the MemberSrno
                    var UserAccount = new MstUserAccount
                    {
                        LoginId = LoginID,
                        Password = Password,
                        Status = "ACTIVE",
                        AccountType = "MEMBER",
                        GameRegister = 0
                    };
                    //Add this into database first so that we can retrive the MemberSrno
                    _db.MstUserAccount.Add(UserAccount);
                    _db.SaveChanges();



                    if (Upline != null && Upline != "")
                    {
                        var UplineAccount = _db.MstUserAccount.Where(x => x.LoginId.Equals(Upline)).FirstOrDefault();

                        //Register a new account into our system
                        UserDetails = new MstUser
                        {
                            MemberSrno = UserAccount.MemberSrno,
                            Name = LoginID,
                            Email = LoginID + "@example.com",
                            Phone = "0123456789",
                            Country = CountryCode,
                            DoB = DateTime.Now.ToString("yyyy-MM-dd"),
                            Gender = 0,
                            RegisterDate = DateTime.Now,
                            RefCode = RandomCode,
                            Upline = Upline,
                            UplineId = UplineAccount.MemberSrno
                        };

                    }
                    else
                    {
                        UserDetails = new MstUser
                        {
                            MemberSrno = UserAccount.MemberSrno,
                            Name = LoginID,
                            Email = LoginID + "@example.com",
                            Phone = "0123456789",
                            Country = CountryCode,
                            DoB = DateTime.Now.ToString("yyyy-MM-dd"),
                            Gender = 0,
                            RegisterDate = DateTime.Now,
                            RefCode = RandomCode,
                        };
                    }

                    var UserCashWallet = new MstUserWallet
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        CashCredit = 0
                    };

                    //Set defualt password for game account
                    var _GamePassword = "game1234";

                    var UserGameAccount = new MstUserGameAccount
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        GamePassword = _GamePassword,
                        GameId = gamelogin, //Form Now I set the Game ID same with our Login ID
                        CreatedDateTime = DateTime.Now,
                    };
                    var UserGameWallet = new MstUserGameWallet
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        GameId = gamelogin,//Form Now I set the Game ID same with our Login ID
                        GameCredit = 0,

                    };

                    //Call 998 API to create a agame account 
                    var result = await _intergration.CreateNewPlayer(gamelogin, LoginID, LoginID + "@example.com", "0123456789", DateTime.Now.ToString("yyyy-MM-dd"), _GamePassword, CountryCode, 0);
                    if (result.Error == 0)
                    {
                        //API Successfully Created an Account from the thrid party side then only allow to create an account into our database
                        _db.MstUser.Add(UserDetails);
                        _db.MstUserWallet.Add(UserCashWallet);
                        _db.MstUserGameAccount.Add(UserGameAccount);
                        _db.MstUserGameWallet.Add(UserGameWallet);
                        UserAccount.GameRegister = 1;
                        await _db.SaveChangesAsync();
                        dbContextTransaction.Commit();

                        return new Tuple<int, long>(Models_General.SUCC_CREATE_ACCOUNT, UserAccount.MemberSrno);
                    }
                    else
                    {
                        _db.MstUser.Add(UserDetails);
                        _db.MstUserWallet.Add(UserCashWallet);
                        _db.MstUserGameAccount.Add(UserGameAccount);
                        _db.MstUserGameWallet.Add(UserGameWallet);
                        UserAccount.GameRegister = 2;
                        await _db.SaveChangesAsync();
                        dbContextTransaction.Commit();

                        return new Tuple<int, long>(Models_General.SUCC_CREATE_ACCOUNT_WITHOUT_GAME_ACCOUNT, UserAccount.MemberSrno);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    var new_error = new LogErrorSystem
                    {
                        Title = "Twelve User Register New Account",
                        Details = ex.Message + "/"+ ex.StackTrace,
                        Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                        CreatedDateTime = DateTime.Now,
                    };
                    _db.LogErrorSystem.Add(new_error);
                    _db.SaveChanges();
                    return new Tuple<int, long>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, 0);
                }



            }
        }

        public int UserChangeUpline(string LoginID, string Upline)
        {
          
            try
            {
                int result;
                if (Upline != null && Upline != "" && !IsUsernameExist(Upline))
                {
                    result = Models_General.ERR_UPLINE_REFERCODE_NOT_EXIST;
                    return result;
                }

                if (!IsUsernameExist(LoginID))
                {
                    result = Models_General.ERR_USER_NOT_FOUND;
                    return result;
                }
                if( Upline != null && Upline != "")
                {
                    var UplineAccount = _db.MstUserAccount.Where(x => x.LoginId.Equals(Upline)).FirstOrDefault();
                    var LoginIdAccount = _db.MstUserAccount.Where(x => x.LoginId.Equals(LoginID)).FirstOrDefault();

                    var UplineUser = _db.MstUser.Where(x => x.MemberSrno.Equals(UplineAccount.MemberSrno)).FirstOrDefault();
                    var LoginIdUser = _db.MstUser.Where(x => x.MemberSrno.Equals(LoginIdAccount.MemberSrno)).FirstOrDefault();

                    LoginIdUser.Upline = UplineAccount.LoginId;
                    LoginIdUser.UplineId = UplineAccount.MemberSrno;
                    result = Models_General.SUCC_CHANGE_UPLINE;
                }
                else
                {
                    var LoginIdAccount = _db.MstUserAccount.Where(x => x.LoginId.Equals(LoginID)).FirstOrDefault();
                    var LoginIdUser = _db.MstUser.Where(x => x.MemberSrno.Equals(LoginIdAccount.MemberSrno)).FirstOrDefault();

                    LoginIdUser.Upline = null;
                    LoginIdUser.UplineId = null;
                    result = Models_General.SUCC_CHANGE_UPLINE;
                }
               
                
                _db.SaveChanges();

                return result;

            }catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Twelve User Change Upline",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int TopupCredit(string loginid, decimal TopupAmount, bool IsReset)
        {

            try
            {
                int result;
                var UserAccount = _db.MstUserAccount.Where(x => x.LoginId == loginid).FirstOrDefault();
                if (UserAccount != null)
                {
                    var CurrentUserWallet = _db.MstUserWallet.Where(x => x.MemberSrno == UserAccount.MemberSrno).FirstOrDefault();
                    var CurrentSettings = _db.MstSettings.Where(x => x.SettingName == "TwelveTurnover").FirstOrDefault();


                    var user_topup_request_tracking = new LogUserTrackingWallet
                        {
                            MemberSrno = UserAccount.MemberSrno,
                            WalletFrom = "TWELVE SYSTEM",
                            WalletTo = "CASH WALLET",
                            TransactionType = 8,
                            PreviousAmount = CurrentUserWallet.CashCredit,
                            TransactionAmount = TopupAmount,
                            CurrentTotalAmount = CurrentUserWallet.CashCredit + TopupAmount,
                            IsDeduct = false,
                            Description = string.Format("TWELVE SYSTEM TOPUP CREDIT TO THIS USER AT {0}", DateTime.Now),
                        CreatedDateTime = DateTime.Now,
                    };

                    var log_topup = new LogTopup
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        WalletFrom = "TWELVE SYSTEM",
                        WalletTo = "CASH WALLET",
                        TransactionType = 8,
                        PreviousAmount = CurrentUserWallet.CashCredit,
                        TransactionAmount = TopupAmount,
                        CurrentTotalAmount = CurrentUserWallet.CashCredit + TopupAmount,
                        Description = string.Format("TWELVE SYSTEM TOPUP CREDIT TO THIS USER AT {0}", DateTime.Now),
                        CreatedDateTime = DateTime.Now,

                    };
                    _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                    _db.LogTopup.Add(log_topup);
                    result = Models_General.SUCC_TOPUP;

                    decimal finalturnover = TopupAmount * int.Parse(CurrentSettings.SettingValue);
                    CurrentUserWallet.CashCredit += TopupAmount;
                    CurrentUserWallet.TurnoverAmount += finalturnover;

                    if(IsReset == true)
                    {
                        CurrentUserWallet.TwelveTurnoverAmount = TopupAmount;
                    }
                    _db.SaveChanges();
                    return result;
                }
                else
                {
                    return Models_General.ERR_USER_NOT_FOUND;
                }

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Twelve Topup Credit",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }


        public List<Models_Twelve_Turnover_List> GetTwelveTurnoverRemainingAmount(string[] loginids)
        {
            try
            {
                var result = new List<Models_Twelve_Turnover_List>();
                foreach (var loginid in loginids)
                {
                    var currentuser = _db.MstUserAccount.Where(x => x.LoginId == loginid).FirstOrDefault();
                    if(currentuser != null)
                    {
                        var currentwallet = _db.MstUserWallet.Where(x => x.MemberSrno == currentuser.MemberSrno).FirstOrDefault();

                        result.Add(new Models_Twelve_Turnover_List
                        {
                            LoginId = loginid,
                            TwelveTurnoverAmount = currentwallet.TwelveTurnoverAmount
                        });
                    }
                    else
                    {
                        continue;
                    }
                    
                }

                return result;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Twelve Get Turnover Remaining Amount",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }
        private bool IsUsernameExist(string _LoginId)
        {
            return _db.MstUserAccount.Where(x => x.LoginId.Equals(_LoginId)).Any();

        }
        private bool IsRefcodeExist(string refcode)
        {
            return _db.MstUser.Where(x => x.RefCode.Equals(refcode.ToLower())).Any();
        }
    }
}
