
using CashEntertainment.DB;
using CashEntertainment.Models;
using CashEntertainment.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_Tracking_Wallet_Log;

namespace CashEntertainment.DataAccess
{
    public class Repo_Wallet : IRepo_Wallet
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;

        public Repo_Wallet(UAT_CasinoContext db, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _accessor = accessor;
            _intergration = intergration;
        }

     
        public Tuple<int, MstUserWallet> GetUserWallet(long MemberSrno)
        {
            try
            {
                var result = _db.MstUserWallet.Where(x => x.MemberSrno.Equals(MemberSrno)).FirstOrDefault();

                if (result != null)
                {
                    return new Tuple<int, MstUserWallet>(0, result);
                }
                else
                {

                    return new Tuple<int, MstUserWallet>(Models_General.ERR_USER_NOT_FOUND, null);
                }
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve User Wallet",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return new Tuple<int, MstUserWallet>(Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR, null);
            }
        }

        public async Task<decimal> GetBalanceGameCredit(long _MemberSrno)
        {
            using var dbContextTransaction = _db.Database.BeginTransaction();
            try
            {
                var ISUserExist = _db.MstUserAccount.Where(x => x.MemberSrno == _MemberSrno && x.Status == "ACTIVE").Any();
                decimal data = -1;
                if (ISUserExist)
                {

                    var UserGameAccountDetails = _db.MstUserGameAccount.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                    var result = await _intergration.GetBalance(UserGameAccountDetails.GameId);

                    if (result.Error == 0)
                    {
                        data = result.Balance;
                        await _db.SaveChangesAsync();
                        dbContextTransaction.Commit();

                        return data;
                    }

                    return data;
                }

                return Models_General.ERR_BALANCE_NOT_FOUND;


            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();


                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Game Balance Credit",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int TransferWalletCredit(long Srno, string RefCode, decimal TransferAmount)
        {
            try
            {
                var OwnerExist = _db.MstUser.Where(x => x.MemberSrno == Srno).FirstOrDefault();
                var ReceiverExist = _db.MstUser.Where(x => x.RefCode == RefCode).FirstOrDefault();

                if (OwnerExist != null)
                {
                    var OwnerWallet = _db.MstUserWallet.Where(x => x.MemberSrno == OwnerExist.MemberSrno).FirstOrDefault();
                    if (OwnerWallet.CashCredit < TransferAmount)
                    {
                        return Models_General.ERR_INSUFFICIENT_CASH_BALANCE;
                    }
                    else
                    {
                        if (ReceiverExist != null)
                        {
                            if(OwnerExist.Country != ReceiverExist.Country)
                            {
                                return Models_General.ERR_CURRENCY_NOT_SAME;
                            }


                            var ReceiverWallet = _db.MstUserWallet.Where(x => x.MemberSrno == ReceiverExist.MemberSrno).FirstOrDefault();


                            var owner_topup_request_tracking = new LogUserTrackingWallet
                            {
                                MemberSrno = OwnerExist.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = "OTHER USER CASH WALLET",
                                TransactionType = 10,
                                PreviousAmount = OwnerWallet.CashCredit,
                                TransactionAmount = TransferAmount,
                                CurrentTotalAmount = OwnerWallet.CashCredit - TransferAmount,
                                IsDeduct = true,
                                Description = string.Format("USER:{0} TRANSFER AMOUNT:{1} TO USER:{2} AT:{3}", OwnerExist.MemberSrno, TransferAmount, ReceiverExist.MemberSrno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                                Sender = OwnerExist.MemberSrno,
                               Receiver = ReceiverExist.MemberSrno
                            };


                            var receiver_topup_request_tracking = new LogUserTrackingWallet
                            {
                                MemberSrno = ReceiverExist.MemberSrno,
                                WalletFrom = "OTHER USER CASH WALLET",
                                WalletTo = "CASH WALLET",
                                TransactionType = 11,
                                PreviousAmount = ReceiverWallet.CashCredit,
                                TransactionAmount = TransferAmount,
                                CurrentTotalAmount = ReceiverWallet.CashCredit + TransferAmount,
                                IsDeduct = false,
                                Description = string.Format("USER:{0} TRANSFER AMOUNT:{1} TO USER:{2} AT:{3}", OwnerExist.MemberSrno, TransferAmount, ReceiverExist.MemberSrno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                                Sender = OwnerExist.MemberSrno,
                                Receiver = ReceiverExist.MemberSrno
                            };




                            var receiver_log_topup = new LogTopup
                            {
                                MemberSrno = ReceiverExist.MemberSrno,
                                WalletFrom = "OTHER USER CASH WALLET",
                                WalletTo = "CASH WALLET",
                                TransactionType = 11,
                                PreviousAmount = ReceiverWallet.CashCredit,
                                TransactionAmount = TransferAmount,
                                CurrentTotalAmount = ReceiverWallet.CashCredit + TransferAmount,
                                Description = string.Format("USER:{0} TRANSFER AMOUNT:{1} TO USER:{2} AT:{3}", OwnerExist.MemberSrno, TransferAmount, ReceiverExist.MemberSrno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };


                            var owner_log_withdraw = new LogWithdraw
                            {
                                MemberSrno = OwnerExist.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = "OTHER USER CASH WALLET",
                                TransactionType = 10,
                                PreviousAmount = OwnerWallet.CashCredit,
                                TransactionAmount = TransferAmount,
                                CurrentTotalAmount = OwnerWallet.CashCredit - TransferAmount,
                                Description = string.Format("USER:{0} TRANSFER AMOUNT:{1} TO USER:{2} AT:{3}", OwnerExist.MemberSrno, TransferAmount, ReceiverExist.MemberSrno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                            };



                            _db.LogUserTrackingWallet.Add(owner_topup_request_tracking);
                            _db.LogUserTrackingWallet.Add(receiver_topup_request_tracking);
                            _db.LogTopup.Add(receiver_log_topup);
                            _db.LogWithdraw.Add(owner_log_withdraw);


                            OwnerWallet.CashCredit -= TransferAmount;
                            ReceiverWallet.CashCredit += TransferAmount;
                            ReceiverWallet.TurnoverAmount += TransferAmount;

                            _db.SaveChanges();
                            return Models_General.SUCC_TRANSFER_CREDIT;
                        }
                        else
                        {
                            return Models_General.ERR_TRANSFER_USER_NOT_FOUND;
                        }

                    }                  
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
                    Title = "Transfer Wallet Credit",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }

        }

        public int UpdateWalletTurnover(long Srno, long MemberSrno, decimal TurnoverAmount)
        {
       
            try
            {
                var UserWallet = _db.MstUserWallet.Where(x => x.MemberSrno == MemberSrno).FirstOrDefault();

                if (UserWallet != null)
                {
                    //Create new log to log which admin at what datetime change member password
                    var log_turnover = new LogTurnover
                    {
                        MemberSrno = MemberSrno,
                        PreviousAmount = UserWallet.TurnoverAmount,
                        TotalAmount = TurnoverAmount,
                        UpdatedAmount = TurnoverAmount,
                        Description = string.Format("ADMIN:{0} UPDATE TURNOVER AMOUNT OF USER WALLET:{1} TO AMOUNT:{2} AT:{3}", Srno, MemberSrno, TurnoverAmount, DateTime.Now),
                        CreatedDateTime = DateTime.Now,
                        TransactionType = 14
                    };

                    //update the current user password to latest
                    UserWallet.TurnoverAmount = TurnoverAmount;

                    _db.LogTurnover.Add(log_turnover);
                    _db.SaveChanges();

                    return Models_General.SUCC_CHANGE_MEMBER_TURNOVER;
                }
                return Models_General.ERR_USER_WALLET_NOT_FOUND;
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Update Wallet Turnover",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }


        public List<LogUserTrackingWallet> RetrieveTrackingWalletList()
        {
            try
            {
                return _db.LogUserTrackingWallet.ToList();
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Tracking Wallet List",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }



        public List<Models_Tracking_Wallet_Log_List> RetrieveTrackingWalletLogListByFilter(string loginid, int transactiontype, string startdate, string enddate)
        {
            try
            {
                var end = Convert.ToDateTime(enddate).AddDays(1);

                var query1 = _db.LogUserTrackingWallet.Where(x => x.CreatedDateTime >= Convert.ToDateTime(startdate) && x.CreatedDateTime < end).ToList();
                var query2 = _db.MstUserAccount.ToList();
                if (transactiontype > -1)
                {
                    query1 = query1.Where(x => x.TransactionType == transactiontype).ToList();
                }


                if (!string.IsNullOrEmpty(loginid))
                {
                    query2 = query2.Where(x => x.LoginId == loginid).ToList();
                }

                var data = (from t1 in query1
                            join t2 in query2 on t1.MemberSrno equals t2.MemberSrno
                            select new Models_Tracking_Wallet_Log_List
                            {
                                MemberSrno = t1.MemberSrno,
                                WalletFrom = t1.WalletFrom,
                                WalletTo = t1.WalletTo,
                                TransactionType = t1.TransactionType,
                                PreviousAmount = t1.PreviousAmount,
                                TransactionAmount = t1.TransactionAmount,
                                CurrentTotalAmount = t1.CurrentTotalAmount,
                                IsDeduct = t1.IsDeduct,
                                Description = t1.Description,
                                CreatedDateTime = t1.CreatedDateTime,
                                Receiver = t1.Receiver,
                                Sender = t1.Sender,
                                LoginId = t2.LoginId,
                            }).OrderByDescending(x => x.CreatedDateTime).ToList();
                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Tracking Wallet Log List",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }



        public int EditMainWalletCredit(string AdminID, long MemberSrno, int ManipulateType , decimal ManipulateAmount, decimal TurnoverAmount)
        {
            try
            {
                int result;
                var CurrentUserWallet = _db.MstUserWallet.Where(x => x.MemberSrno == MemberSrno).FirstOrDefault();

                if (CurrentUserWallet != null)
                {
                    if (ManipulateType == 0)
                    {
                        var user_topup_request_tracking = new LogUserTrackingWallet
                        {
                            MemberSrno = MemberSrno,
                            WalletFrom = "EGG SYSTEM",
                            WalletTo = "CASH WALLET",
                            TransactionType = 6,
                            PreviousAmount = CurrentUserWallet.CashCredit,
                            TransactionAmount = ManipulateAmount,
                            CurrentTotalAmount = CurrentUserWallet.CashCredit + ManipulateAmount,
                            IsDeduct = false,
                            Description = string.Format("ADMIN:{0} MANUALLY TOPUP TO {1} WITH TURNOVER AMOUNT {2} AT:{3}", AdminID, MemberSrno, TurnoverAmount, DateTime.Now),
                            CreatedDateTime = DateTime.Now,
                        };


                        var log_topup = new LogTopup
                        {
                            MemberSrno = MemberSrno,
                            WalletFrom = "EGG SYSTEM",
                            WalletTo = "CASH WALLET",
                            TransactionType = 6,
                            PreviousAmount = CurrentUserWallet.CashCredit,
                            TransactionAmount = ManipulateAmount,
                            CurrentTotalAmount = CurrentUserWallet.CashCredit + ManipulateAmount,
                            Description = string.Format("ADMIN:{0} MANUALLY TOPUP TO {1} WITH TURNOVER AMOUNT {2} AT:{3}", AdminID, MemberSrno, TurnoverAmount, DateTime.Now),
                            CreatedDateTime = DateTime.Now,

                        };

                        _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                        _db.LogTopup.Add(log_topup);
                        CurrentUserWallet.CashCredit += ManipulateAmount;
                        CurrentUserWallet.TurnoverAmount += TurnoverAmount;
                        _db.SaveChanges();
                        result = Models_General.SUCC_EDIT_CASH_WALLET;
                    }
                    else if(ManipulateType == 1)
                    {
                        var user_topup_request_tracking = new LogUserTrackingWallet
                        {
                            MemberSrno = MemberSrno,
                            WalletFrom = "CASH WALLET",
                            WalletTo = "EGG SYSTEM",
                            TransactionType = 7,
                            PreviousAmount = CurrentUserWallet.CashCredit,
                            TransactionAmount = ManipulateAmount,
                            CurrentTotalAmount = CurrentUserWallet.CashCredit - ManipulateAmount,
                            IsDeduct = true,
                            Description = string.Format("ADMIN:{0} MANUALLY WITHDRAWAL TO {1} WITH TURNOVER AMOUNT {2} AT:{3}", AdminID, MemberSrno, TurnoverAmount, DateTime.Now),
                            CreatedDateTime = DateTime.Now,
                        };


                        var log_withdraw = new LogWithdraw
                        {
                            MemberSrno = MemberSrno,
                            WalletFrom = "CASH WALLET",
                            WalletTo = "EGG SYSTEM",
                            TransactionType = 7,
                            PreviousAmount = CurrentUserWallet.CashCredit,
                            TransactionAmount = ManipulateAmount,
                            CurrentTotalAmount = CurrentUserWallet.CashCredit - ManipulateAmount,
                            Description = string.Format("ADMIN:{0} MANUALLY WITHDRAWAL TO {1} WITH TURNOVER AMOUNT {2} AT:{3}", AdminID, MemberSrno, TurnoverAmount, DateTime.Now),
                            CreatedDateTime = DateTime.Now,

                        };

                        _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                        _db.LogWithdraw.Add(log_withdraw);

         

                        if (CurrentUserWallet.CashCredit > 0)
                        {
                            if (ManipulateAmount > CurrentUserWallet.CashCredit)
                            {
                                CurrentUserWallet.CashCredit = 0;
                            }
                            else
                            {
                                CurrentUserWallet.CashCredit -= ManipulateAmount;
                            }
                        }


                        if (CurrentUserWallet.TurnoverAmount > 0)
                        {
                            if (TurnoverAmount > CurrentUserWallet.TurnoverAmount)
                            {
                                CurrentUserWallet.TurnoverAmount = 0;
                            }
                            else
                            {
                                CurrentUserWallet.TurnoverAmount -= TurnoverAmount;
                            }
                        }
                        _db.SaveChanges();
                        result = Models_General.SUCC_EDIT_CASH_WALLET;
                    }
                    else
                    {
                        result = Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                    }
                }
                else
                {
                    result = Models_General.ERR_USER_WALLET_NOT_FOUND;
                }

                return result;

            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Manipulate Main Wallet Credit",
                    Details = ex.Message + "/" + ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }


        }

    }
}














