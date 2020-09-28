using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using CashEntertainment.Helper;
using System.Linq;
using static CashEntertainment.Models.Models_Bank;


namespace CashEntertainment.DataAccess
{
    public class Repo_Bank : IRepo_Bank
    {
        private readonly UAT_CasinoContext _db;
        public readonly UploadImagesHelper _image_services;
        private readonly IActionContextAccessor _accessor;

        public Repo_Bank(UAT_CasinoContext db, UploadImagesHelper image_services, IActionContextAccessor accessor)
        {
            _db = db;
            _image_services = image_services;
            _accessor = accessor;
        }

        public int MemberAddNewBank(long MemberSrno, long BankSrno, string BankAccountHolderName, string BankCardNo)
        {
            try
            {


                var BankFound = _db.MstBank.Where(x => x.Srno == BankSrno).FirstOrDefault();

                if(BankFound != null)
                {
                    var UserBankDetails = new MstUserBank
                    {
                        MemberSrno = MemberSrno,
                        BankSrno = BankFound.Srno,
                        BankAccountHolder = BankAccountHolderName,
                        BankCardNo = BankCardNo,
                         Status = "ACTIVE",
                        CreatedDateTime = DateTime.Now,
                    };
                    _db.MstUserBank.Add(UserBankDetails);
                    _db.SaveChanges();

                    return Models_General.SUCC_MEMBER_ADD_BANK;
                }
                else
                {
              return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                }
               
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Member Add Bank",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public int MemberUpdateBank(long MemberSrno, long UserBankSrno, long BankSrno, string BankAccountHolderName, string BankCardNo)
        {
            try
            {
                var UserBankDetails = _db.MstUserBank.Where(x => x.MemberSrno == MemberSrno && x.Srno == UserBankSrno).FirstOrDefault();
                if (UserBankDetails != null)
                {
                    UserBankDetails.BankSrno = BankSrno;
                    UserBankDetails.BankAccountHolder = BankAccountHolderName;
                    UserBankDetails.BankCardNo = BankCardNo;

                    _db.SaveChanges();
                    return Models_General.SUCC_MEMBER_UPDATE_BANK;
                }
                return Models_General.ERR_USER_BANK_NOT_FOUND;

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Member Update Bank",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public int AdminAddNewBank(long BankSrno, string BankAccountHolderName, string BankCardNo, string Country)
        {
            try
            {


                var BankFound = _db.MstBank.Where(x => x.Srno == BankSrno).FirstOrDefault();

                if (BankFound != null)
                {
                    var AdminBankDetails = new MstAdminBank
                    {
                        BankSrno = BankFound.Srno,
                        BankAccountHolder = BankAccountHolderName,
                        BankCardNo = BankCardNo,
                        Status = "ACTIVE",
                        Country = Country,
                        CreatedDateTime = DateTime.Now
                    };
                    _db.MstAdminBank.Add(AdminBankDetails);
                    _db.SaveChanges();

                    return Models_General.SUCC_ADMIN_ADD_BANK;
                }
                else
                {
                    return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                }

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Admin Add Bank",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public int AdminUpdateBank(long AdminBankSrno, long BankSrno, string BankAccountHolderName, string BankCardNo, string Country)
        {
            try
            {
                var AdminBankDetails = _db.MstAdminBank.Where(x => x.Srno == AdminBankSrno).FirstOrDefault();
                if (AdminBankDetails != null)
                {
                    AdminBankDetails.BankSrno = BankSrno;
                    AdminBankDetails.BankAccountHolder = BankAccountHolderName;
                    AdminBankDetails.BankCardNo = BankCardNo;
                    AdminBankDetails.Country = Country;
                    _db.SaveChanges();
                    return Models_General.SUCC_ADMIN_UPDATE_BANK;
                }
                return Models_General.ERR_ADMIN_BANK_NOT_FOUND;

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Admin Update Bank",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public List<MstBank> RetrieveBankList()
        {
            try
            {
                return _db.MstBank.ToList();
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Bank Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public List<MstPaylah88Bank> RetrievePaylah88BankList()
        {
            try
            {
                return _db.MstPaylah88Bank.ToList();
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Paylah88 Bank Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public List<Models_Member_Bank_List> RetrieveUserBankList()
        {
            try
            {

              var data =  (from t1 in _db.MstUserBank
                          join t2 in _db.MstBank on t1.BankSrno equals t2.Srno
                select new Models_Member_Bank_List
                {
                    Srno = t1.Srno,
                    MemberSrno = t1.MemberSrno,
                    BankSrno = t1.BankSrno,
                    BankCode = t2.BankCode,
                    BankName = t2.BankName,
                    BankAccountHolder = t1.BankAccountHolder,
                    BankCardNo = t1.BankCardNo,
                    Status = t1.Status,
                    CreatedDateTime = t1.CreatedDateTime,
                }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve User Bank Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public List<Models_Admin_Bank_List> RetrieveAdminBankList()
        {
            try
            {
                var data = (from t1 in _db.MstAdminBank
                            join t2 in _db.MstBank on t1.BankSrno equals t2.Srno
                            select new Models_Admin_Bank_List
                            {
                                Srno = t1.Srno,
                                BankSrno = t1.BankSrno,
                                BankCode = t2.BankCode,
                                BankName = t2.BankName,
                                BankAccountHolder = t1.BankAccountHolder,
                                BankCardNo = t1.BankCardNo,
                                Country = t1.Country,
                                Status = t1.Status,
                                CreatedDateTime = t1.CreatedDateTime,
                            }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Admin Bank Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
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
