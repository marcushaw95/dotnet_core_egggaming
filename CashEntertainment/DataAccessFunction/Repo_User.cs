
using CashEntertainment.DB;
using CashEntertainment.Models;
using CashEntertainment.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_User;
using static CashEntertainment.Helper.Intergration;

namespace CashEntertainment.DataAccess
{
    public class Repo_User : IRepo_User
    {
        private readonly UAT_CasinoContext _db;
        private readonly Common _common_services;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;

        public Repo_User(UAT_CasinoContext db, Common common_services, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _common_services = common_services;
            _accessor = accessor;
            _intergration = intergration;
        }

        public async Task<Tuple<int, long>> UserRegisterNewAccount(string LoginID, string FullName, string Password, string Email, string Phonenumber, string CountryCode, string DOB, int Gender,string Upline)
        {

            using var dbContextTransaction = _db.Database.BeginTransaction();
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

                if (Upline != null && !IsRefcodeExist(Upline))
                {
                    return new Tuple<int, long>(Models_General.ERR_UPLINE_REFERCODE_NOT_EXIST, 0);
                }
                else if (IsUsernameExist(LoginID))
                {
                    return new Tuple<int, long>(Models_General.ERR_USERNAME_EXIST, 0);
                }
                else if (IsPhoneExist(Phonenumber))
                {
                    return new Tuple<int, long>(Models_General.ERR_PHONENUMBER_EXIST, 0);
                }
                else if (IsEmailExist(Email))
                {
                    return new Tuple<int, long>(Models_General.ERR_EMAIL_EXIST, 0);
                }

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

                //Register a new account into our system

                var UserDetails = new MstUser { };


                if (Upline != null && Upline != "")
                {
                    var UplineDetails = _db.MstUser.Where(x => x.RefCode.Equals(Upline)).FirstOrDefault();
                    var UplineAccount = _db.MstUserAccount.Where(x => x.MemberSrno.Equals(UplineDetails.MemberSrno)).FirstOrDefault();

                    UserDetails = new MstUser
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        Name = FullName,
                        Email = Email,
                        Phone = Phonenumber,
                        Country = CountryCode,
                        DoB = DOB,
                        Gender = Gender,
                        RegisterDate = DateTime.Now,
                        RefCode = RandomCode,
                        Upline = UplineAccount.LoginId,
                        UplineId = UplineAccount.MemberSrno,

                    };

                }
                else
                {
                    UserDetails = new MstUser
                    {
                        MemberSrno = UserAccount.MemberSrno,
                        Name = FullName,
                        Email = Email,
                        Phone = Phonenumber,
                        Country = CountryCode,
                        DoB = DOB,
                        Gender = Gender,
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
                    GameId = gamelogin,
                    CreatedDateTime = DateTime.Now,
                };
                var UserGameWallet = new MstUserGameWallet
                {
                    MemberSrno = UserAccount.MemberSrno,
                    GameId = gamelogin,
                    GameCredit = 0,

                };

                //Call 998 API to create a agame account 
                var result = await _intergration.CreateNewPlayer(gamelogin, FullName, Email, Phonenumber, DOB, _GamePassword, CountryCode, Gender);
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
                    Title = "User Register New Account",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, long>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, 0);
            }
        }

        public Tuple<int, long, int> AuthrorizeAccount(string LoginID, string _Password, string LoginCountry, string UserIpAddress)
        {
            //Declare a default result  

            try
            {
                int result = Models_General.ERR_INCORRECT_CREDENTIALS;


                //Check Account exist in the following country
                var AccounExist = (from
                                   t1 in _db.MstUserAccount
                                   join
                                   t2 in _db.MstUser on t1.MemberSrno equals t2.MemberSrno
                                   where t1.LoginId == LoginID && t2.Country == LoginCountry
                                   select new
                                   {
                                       t1,
                                       t2
                                   }).FirstOrDefault();


                if (AccounExist != null)
                {
                    //Check Account Status if is inactive show error
                    if (AccounExist.t1.Status.Trim().ToUpper() == "INACTIVE")
                    {

                        return new Tuple<int, long, int>(Models_General.ERR_ACCOUNT_LOCKED, 0, AccounExist.t1.GameRegister);
                    }


                    //Declare A New Log For User Login
                    var LogLogin = new LogLoginIp
                    {
                        MemberSrno = AccounExist.t1.MemberSrno,
                        CreatedDateTime = DateTime.Now,
                        Password = _Password,
                        Ip = UserIpAddress,
                    };

                    //Check the Password Correct for the corressponding account.
                    var IsCorrectPassword = _db.MstUserAccount.Where(x => x.Password.Equals(_Password) && x.LoginId.Equals(LoginID)).Any();

                    if (IsCorrectPassword)
                    {
                        //Correct Update the Log With Status Success
                        LogLogin.Status = "SUCCESS";
                        result = Models_General.SUCC_AUTHORIZE_GRANTED;
                    }
                    else
                    {
                        //Correct Update the Log With Status Failed
                        LogLogin.Status = "FAILED";

                    }

                    //Save the new log into database
                    _db.LogLoginIp.Add(LogLogin);
                    _db.SaveChanges();
                    return new Tuple<int, long, int>(result, AccounExist.t1.MemberSrno, AccounExist.t1.GameRegister);
                }


                return new Tuple<int, long, int>(result, 0, 0);
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Authorize Member Account",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, long, int>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, 0, 0);
            }


        

        }

        public int UserUpdateAccountDetails(long _MemberSrno, string FullName, string _Email, string _Phonenumber, string _DOB, int Gender)
        {


            try
            {
                var UserDetails = _db.MstUser.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                var checkEmail = _db.MstUser.Where(x => x.Email == _Email).FirstOrDefault();
                var checkPhone = _db.MstUser.Where(x => x.Phone == _Phonenumber).FirstOrDefault();

                if (UserDetails == null)
                {
                    return Models_General.ERR_USER_NOT_FOUND;
                }
                else if (checkEmail != null && checkEmail.MemberSrno != _MemberSrno)
                {
                    return Models_General.ERR_EMAIL_EXIST;
                }
                else if (checkPhone != null && checkPhone.MemberSrno != _MemberSrno)
                {
                    return Models_General.ERR_PHONENUMBER_EXIST;
                }

                UserDetails.Name = FullName;
                UserDetails.Email = _Email;
                UserDetails.Phone = _Phonenumber;
                UserDetails.DoB = _DOB;
                UserDetails.Gender = Gender;
                _db.SaveChanges();

                return Models_General.SUCC_UPDATE_PROFILE;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "User Update Account Details",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }

        }

        public int UserUpdatePassword(string NewPassword, string OldPassword, long MemberSrno)
        {
            try
            {
                var UserAccount = _db.MstUserAccount.Where(x => x.MemberSrno == MemberSrno && x.Password == OldPassword).FirstOrDefault();
                if (UserAccount != null)
                {
                    UserAccount.Password = NewPassword;
                    _db.SaveChanges();

                    return Models_General.SUCC_UPDATE_PASSWORD;
                }


                return Models_General.ERR_INCORRECT_PASSWORD;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "User Update Password",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
  
        private bool IsEmailExist(string _Email)
        {
            return _db.MstUser.Where(x => x.Email.Equals(_Email)).Any();

        }
        private bool IsPhoneExist(string _Phone)
        {
            return _db.MstUser.Where(x => x.Phone.Equals(_Phone)).Any();

        }
        private bool IsUsernameExist(string _LoginId)
        {
            return _db.MstUserAccount.Where(x => x.LoginId.Equals(_LoginId)).Any();

        }
        private bool IsRefcodeExist(string refcode)
        {
            return _db.MstUser.Where(x => x.RefCode.Equals(refcode.ToLower())).Any();
        }

        public Tuple<int, Models_User_Profile> GetUserProfile(long MemberSrno)
        {
            try
            {
                var result = (from t1 in _db.MstUserAccount
                              join t2 in _db.MstUser on t1.MemberSrno equals t2.MemberSrno
                              where t1.MemberSrno == MemberSrno
                              select new Models_User_Profile
                              {
                                  LoginID = t1.LoginId,
                                  Name = t2.Name,
                                  Phone = t2.Phone,
                                  Email = t2.Email,
                                  Dob = t2.DoB,
                                  Gender = t2.Gender,
                                  RefCode = t2.RefCode,
                                  Upline = t2.Upline
                              }).FirstOrDefault();

                if (result != null)
                {
                    return new Tuple<int, Models_User_Profile>(0, result);
                }
                else
                {

                    return new Tuple<int, Models_User_Profile>(Models_General.ERR_USER_NOT_FOUND, null);
                }
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve User Profile",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, Models_User_Profile>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, null);
            }
        }

        public Tuple<int, long> AuthorizeAdminAccount(string LoginID, string _Password)
        {
            //Declare a default result  
            int result = Models_General.ERR_INCORRECT_CREDENTIALS;
            long AdminSrno = 0;

            try
            {


                //Check the Password Correct for the corressponding account.
                var AdminDetails = _db.MstAdminAccount.Where(x => x.Password.Equals(_Password) && x.LoginId.Equals(LoginID) && x.Status == "ACTIVE").FirstOrDefault();

                if (AdminDetails != null)
                {
                    AdminSrno = AdminDetails.Srno;
                    result = Models_General.SUCC_AUTHORIZE_GRANTED;
                }


                return new Tuple<int, long>(result, AdminSrno);
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Authorize Admin Account",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, long>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, AdminSrno);
            }
        }

        public List<Models_User_Listing> RetrieveAllMemberListing()
        {

            try
            {
                return (from t1 in _db.MstUserAccount
                        join t2 in _db.MstUser
                        on t1.MemberSrno equals t2.MemberSrno
                        join t3 in _db.MstUserWallet
                        on t1.MemberSrno equals t3.MemberSrno
                        select new Models_User_Listing
                        {
                            MemberSrno = t1.MemberSrno,
                            LoginID = t1.LoginId,
                            Name = t2.Name,
                            Phone = t2.Phone,
                            Email = t2.Email,
                            Password = t1.Password,
                            Status = t1.Status,
                            GameRegister = t1.GameRegister,
                            Country = t2.Country,
                            Dob = t2.DoB,
                            RegisterDate = t2.RegisterDate,
                            Gender = t2.Gender == 1 ? "MALE" : "FEMALE",
                            DirectSponsor = t2.DirectSponsor,
                            Refcode = t2.RefCode,
                            Upline = t2.Upline,
                            CashCredit = t3.CashCredit,
                            TurnoverAmount = t3.TurnoverAmount
                        }).ToList() ?? null;
            }
            catch(Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve All Member Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }

            
        }
        public int InactiveMember(long _MemberSrno, bool status)
        {
            try
            {
                var UserExist = _db.MstUserAccount.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();
                if (UserExist != null)
                {
                    UserExist.Status = status == true ? "ACTIVE" : "INACTIVE";
                    _db.SaveChanges();

                    return status == true ? Models_General.SUCC_ACTIVE_MEMBER : Models_General.SUCC_INACTIVE_MEMBER;
                }
                return Models_General.ERR_USER_NOT_FOUND;
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve All Member Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }

        }

        public int ChangeMemberPassword(string AdminLoginID, long _MemberSrno, string NewPassword)
        {
            try
            {
                var UserDetails = _db.MstUserAccount.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                if (UserDetails != null)
                {
                    //Create new log to log which admin at what datetime change member password
                    var log_password_changes = new LogAdminChangeMemberPassword
                    {
                        ActionBy = AdminLoginID,
                        MemberSrno = _MemberSrno,
                        PreviousPassword = UserDetails.Password,
                        CurrentPassword = NewPassword,
                        CreatedDateTime = DateTime.Now,
                    };

                    //update the current user password to latest
                    UserDetails.Password = NewPassword;

                    _db.LogAdminChangeMemberPassword.Add(log_password_changes);
                    _db.SaveChanges();

                    return Models_General.SUCC_CHANGE_MEMBER_PASSWORD;
                }
                return Models_General.ERR_USER_NOT_FOUND;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Change Member Password",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public MstAuthenticate Authenticate(string username, string password)
        {

            var user = _db.MstAuthenticate.Where(x => x.User == username && x.Password == password).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;
            // authentication successful so return user details without password
            return user;
        }



        public List<Models_User_Login_Log_List> RetrieveUserLoginLogListByFilter(string loginid,  string startdate, string enddate)
        {
            try
            {
                var end = Convert.ToDateTime(enddate).AddDays(1);

                var query1 = _db.LogLoginIp.Where(x => x.CreatedDateTime >= Convert.ToDateTime(startdate) && x.CreatedDateTime < end).ToList();
                var query2 = _db.MstUserAccount.ToList();
              

                if (!string.IsNullOrEmpty(loginid))
                {
                    query2 = query2.Where(x => x.LoginId == loginid).ToList();
                }

                var data = (from t1 in query1
                            join t2 in query2 on t1.MemberSrno equals t2.MemberSrno
                            select new Models_User_Login_Log_List
                            {
                                MemberSrno = t1.MemberSrno,
                                Password = t1.Password,
                                Ip = t1.Ip,
                                Status = t1.Status,
                                CreatedDateTime = t1.CreatedDateTime,
                                LoginId = t2.LoginId,
                            }).OrderByDescending(x => x.CreatedDateTime).ToList();
                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve User Login Log List",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

    }
}














