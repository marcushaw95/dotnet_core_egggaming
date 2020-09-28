using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using CashEntertainment.Helper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_Withdrawal;

namespace CashEntertainment.DataAccess
{
    public class Repo_Withdraw : IRepo_Withdraw
    {
        private readonly UAT_CasinoContext _db;
        public readonly UploadImagesHelper _image_services;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;

        public Repo_Withdraw(UAT_CasinoContext db, UploadImagesHelper image_services, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _image_services = image_services;
            _accessor = accessor;
            _intergration = intergration;
        }

        public int MemberRequestWithdrawalOnlineBanking(long _MemberSrno, decimal WithdrawalAmount, long MemberBankSrno, string Currency)
        {

            try
            {
                var UserBankDetails = _db.MstUserBank.Where(x => x.Srno == MemberBankSrno).FirstOrDefault();

                if (UserBankDetails != null)
                {

                    var BankDetails = _db.MstBank.Where(x => x.Srno == UserBankDetails.BankSrno).FirstOrDefault();

                    if (BankDetails != null)
                    {

                        var WithdrawalRequest = new MstWithdraw
                        {
                            MemberSrno = _MemberSrno,
                            BankName = BankDetails.BankName,
                            BankCardNo = UserBankDetails.BankCardNo,
                            BankAccountHolder = UserBankDetails.BankAccountHolder,
                            BankCode = BankDetails.BankCode,
                            Status = 0,
                            RequestDate = DateTime.Now,
                            WithdrawAmount = WithdrawalAmount,
                            TransactionType = 0,
                            Currency = Currency,
                        };

                        var CurrentUserWalletInfo = _db.MstUserWallet.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                        if (WithdrawalAmount > CurrentUserWalletInfo.CashCredit)
                        {
                            return Models_General.ERR_WALLET_CREDIT_INSUFFICIENT;
                        }

                        if (CurrentUserWalletInfo.TurnoverAmount > 0)
                        {
                            return Models_General.ERR_WALLET_TURNOVER_CLEAR;
                        }

                        CurrentUserWalletInfo.CashCredit = CurrentUserWalletInfo.CashCredit - WithdrawalAmount;
                        CurrentUserWalletInfo.PendingWithdrawAmount += WithdrawalAmount;

                        _db.MstWithdraw.Add(WithdrawalRequest);
                        _db.SaveChanges();
                        return Models_General.SUCC_CREATE_REQUEST_WITHDRAWAL;
                    }
                    else
                    {
                        return Models_General.ERR_BANK_INFO_NOT_MATCH;
                    }

                }
                return Models_General.ERR_BANK_INFO_NOT_MATCH;

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Member Request Withdrawal Online Banking",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public int MemberRequestWithdrawalCrypto(long _MemberSrno, decimal WithdrawalAmount, string ToAddress, decimal Rate, string Currency)
        {
            try
            {
                        var WithdrawalRequest = new MstWithdraw
                        {
                            MemberSrno = _MemberSrno,
                            Status = 0,
                            RequestDate = DateTime.Now,
                            WithdrawAmount = WithdrawalAmount,
                            TransactionType = 1,
                            ToAddress = ToAddress,
                            Rate = Rate,
                            Currency = Currency,
                        };

                        var CurrentUserWalletInfo = _db.MstUserWallet.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                        if (WithdrawalAmount > CurrentUserWalletInfo.CashCredit)
                        {
                            return Models_General.ERR_WALLET_CREDIT_INSUFFICIENT;
                        }

                        if (CurrentUserWalletInfo.TurnoverAmount > 0)
                        {
                            return Models_General.ERR_WALLET_TURNOVER_CLEAR;
                        }

                        CurrentUserWalletInfo.CashCredit = CurrentUserWalletInfo.CashCredit - WithdrawalAmount;
                        CurrentUserWalletInfo.PendingWithdrawAmount += WithdrawalAmount;

                        _db.MstWithdraw.Add(WithdrawalRequest);
                        _db.SaveChanges();
                        return Models_General.SUCC_CREATE_REQUEST_WITHDRAWAL;
                 
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Member Request Withdrawal Crypto",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int AdminApproveWithdrawal(string AdminID, long withdrawalsrno, bool approvestatus, string remarks)
        {
            try
            {
                //Retrieve The Following Withdrawal Details And Check Is it Valid
                var WithdrawalDetails = _db.MstWithdraw.Where(x => x.Srno == withdrawalsrno && x.Status == 0).FirstOrDefault();
                //Declare a default result;
                int result;
                if (WithdrawalDetails != null)
                {
                    // Check The Approval Staus For The Withdrawal
                    if (approvestatus)
                    {

                        if(WithdrawalDetails.TransactionType == 0)
                        {
                            WithdrawalDetails.ApproveBy = AdminID;
                            WithdrawalDetails.ApproveDate = DateTime.Now;
                            WithdrawalDetails.ApproveRemark = remarks;
                            WithdrawalDetails.Status = 1;

                            //UPDATE MEMBER WALLET PENDING WITHDRAWAL AMOUNT TO ZERO
                            var UserWalletDetails = _db.MstUserWallet.Where(x => x.MemberSrno == WithdrawalDetails.MemberSrno).FirstOrDefault();
                       

                            //LOG_USER_TRAKCING_WALLET - LOG USER TRANSACTION ACTIVITY ONLY WHEN TRANSACTION COMPLETED : LIKE  WITHDRAWAL APPROVED, TOPUP APPROVED, TRANSFER GAME CREDIT SUCCESS
                            var log_user_tracking_wallet = new LogUserTrackingWallet
                            {
                                MemberSrno = WithdrawalDetails.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = WithdrawalDetails.BankName + " " + WithdrawalDetails.BankCardNo + " " + WithdrawalDetails.BankAccountHolder,
                                TransactionType = 1,
                                PreviousAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount,
                                TransactionAmount = WithdrawalDetails.WithdrawAmount,
                                CurrentTotalAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount - WithdrawalDetails.WithdrawAmount,
                                IsDeduct = true,
                                Description = string.Format("ADMIN:{0} APPROVED THIS WITHDRAWAL SERIAL NUMBER:{1} AT:{2}", AdminID, WithdrawalDetails.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };

                            var log_topup = new LogWithdraw
                            {
                                MemberSrno = WithdrawalDetails.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = WithdrawalDetails.BankName + " " + WithdrawalDetails.BankCardNo + " " + WithdrawalDetails.BankAccountHolder,
                                TransactionType = 1,
                                PreviousAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount,
                                TransactionAmount = WithdrawalDetails.WithdrawAmount,
                                CurrentTotalAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount - WithdrawalDetails.WithdrawAmount,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, WithdrawalDetails.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };


                            UserWalletDetails.PendingWithdrawAmount -= WithdrawalDetails.WithdrawAmount;

                            _db.LogUserTrackingWallet.Add(log_user_tracking_wallet);

                            result = Models_General.SUCC_ADMIN_APPROVE_WITHDRAWAL;
                        }
                        else if (WithdrawalDetails.TransactionType == 1){
                            WithdrawalDetails.ApproveBy = AdminID;
                            WithdrawalDetails.ApproveDate = DateTime.Now;
                            WithdrawalDetails.ApproveRemark = remarks;
                            WithdrawalDetails.Status = 1;

                            //UPDATE MEMBER WALLET PENDING WITHDRAWAL AMOUNT TO ZERO
                            var UserWalletDetails = _db.MstUserWallet.Where(x => x.MemberSrno == WithdrawalDetails.MemberSrno).FirstOrDefault();
                            UserWalletDetails.PendingWithdrawAmount -= WithdrawalDetails.WithdrawAmount;

                            //LOG_USER_TRAKCING_WALLET - LOG USER TRANSACTION ACTIVITY ONLY WHEN TRANSACTION COMPLETED : LIKE  WITHDRAWAL APPROVED, TOPUP APPROVED, TRANSFER GAME CREDIT SUCCESS
                            var log_user_tracking_wallet = new LogUserTrackingWallet
                            {
                                MemberSrno = WithdrawalDetails.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = "WALLET ADDRESS "+ WithdrawalDetails.ToAddress,
                                TransactionType = 3,
                                PreviousAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount,
                                TransactionAmount = WithdrawalDetails.WithdrawAmount,
                                CurrentTotalAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount - WithdrawalDetails.WithdrawAmount,
                                IsDeduct = true,
                                Description = string.Format("ADMIN:{0} APPROVED THIS WITHDRAWAL SERIAL NUMBER:{1} AT:{2}", AdminID, WithdrawalDetails.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };


                            var log_withdraw = new LogWithdraw
                            {
                                MemberSrno = WithdrawalDetails.MemberSrno,
                                WalletFrom = "CASH WALLET",
                                WalletTo = "WALLET ADDRESS " + WithdrawalDetails.ToAddress,
                                TransactionType = 3,
                                PreviousAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount,
                                TransactionAmount = WithdrawalDetails.WithdrawAmount,
                                CurrentTotalAmount = UserWalletDetails.CashCredit + UserWalletDetails.PendingWithdrawAmount - WithdrawalDetails.WithdrawAmount,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, WithdrawalDetails.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };



                            UserWalletDetails.PendingWithdrawAmount -= WithdrawalDetails.WithdrawAmount;
                            _db.LogUserTrackingWallet.Add(log_user_tracking_wallet);
                            _db.LogWithdraw.Add(log_withdraw);
                            result = Models_General.SUCC_ADMIN_APPROVE_WITHDRAWAL;
                        }else
                        {
                            result = Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                        }
                        //Update The Withdrawal Details and Set Status to APPROVED
                       
                    }
                    else
                    {
                        //Update The Withdrawal Details and Set Status to REJECTED
                        WithdrawalDetails.RejectBy = AdminID;
                        WithdrawalDetails.RejectDate = DateTime.Now;
                        WithdrawalDetails.RejectRemark = remarks;
                        WithdrawalDetails.Status = 2;

                        //UPDATE MEMBER WALLET PENDING WITHDRAWAL AMOUNT TO ZERO AND TRANSFER BACK TO CASH CREDIT
                        var UserWalletDetails = _db.MstUserWallet.Where(x => x.MemberSrno == WithdrawalDetails.MemberSrno).FirstOrDefault();
                        UserWalletDetails.CashCredit = UserWalletDetails.CashCredit + WithdrawalDetails.WithdrawAmount;
                        UserWalletDetails.PendingWithdrawAmount -= WithdrawalDetails.WithdrawAmount;

                        result = Models_General.SUCC_ADMIN_REJECT_WITHDRAWAL;
                    }
                    _db.SaveChanges();
                    return result;
                }
                return Models_General.ERR_WITHDRAWAL_NOT_FOUND;
            }

            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Admin Approve Withdrawal",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public List<MstWithdraw> RetrieveWithdrawalListing(long? MemberSrno)
        {
            try
            {
                var data = _db.MstWithdraw.OrderByDescending(x => x.RequestDate).ToList();

                if (MemberSrno != null)
                {
                    data = data.Where(x => x.MemberSrno == MemberSrno).ToList();
                }

                return data;
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Withdrawal Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public List<Models_Withdrawal_List> AdminRetrieveWithdrawalListing()
        {
            try
            {
                var data = (from t1 in _db.MstWithdraw
                            join t2 in _db.MstUserAccount on t1.MemberSrno equals t2.MemberSrno
                            select new Models_Withdrawal_List
                            {
                                Srno = t1.Srno,
                                LoginId = t2.LoginId,
                                MemberSrno = t1.MemberSrno,
                                BankCode = t1.BankCode,
                                BankAccountHolder = t1.BankAccountHolder,
                                BankName = t1.BankName,
                                BankCardNo = t1.BankCardNo,
                                WithdrawAmount = t1.WithdrawAmount,
                                Status = t1.Status,
                                RequestDate = t1.RequestDate,
                                ApproveBy = t1.ApproveBy,
                                ApproveRemark = t1.ApproveRemark,
                                ApproveDate = t1.ApproveDate,
                                RejectBy = t1.RejectBy,
                                RejectRemark = t1.RejectRemark,
                                RejectDate = t1.RejectDate,
                                ToAddress = t1.ToAddress,
                                TransactionType = t1.TransactionType,
                                Rate = t1.Rate,
                                Currency = t1.Currency,
                            }).OrderByDescending(x => x.RequestDate).ToList();
                return data;
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Admin Retrieve Withdrawal Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }

          
        }

        public async Task<int> WithdrawGameCredit(long _MemberSrno, decimal WithdrawAmount)
        {
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var ISUserExist = _db.MstUserAccount.Where(x => x.MemberSrno == _MemberSrno && x.Status == "ACTIVE").Any();

                    if (ISUserExist)
                    {
                        var UserWalletData = _db.MstUserWallet.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();
                        var UserDetails = _db.MstUserAccount.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();
                        var UserGameAccountDetails = _db.MstUserGameAccount.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();

                        var TransactionID = new Guid().ToString();
                        //CALL 998 API TO TRANSFER CASH CREDIT TO GAME CREDIT
                        var result = await _intergration.WithdrawGameCredit(UserGameAccountDetails.GameId, WithdrawAmount, TransactionID);

                        if (result.Error == 0)
                        {
                            // add new log from cash credit transaction
                            var log_user_tracking_wallet = new LogUserTrackingWallet
                            {
                                MemberSrno = _MemberSrno,
                                WalletFrom = "GAME WALLET",
                                WalletTo = "CASH WALLET",
                                TransactionType = 5,
                                PreviousAmount = UserWalletData.CashCredit,
                                TransactionAmount = WithdrawAmount,
                                CurrentTotalAmount = UserWalletData.CashCredit + WithdrawAmount,
                                IsDeduct = false,
                                Description = string.Format("MEMBER:{0} WITHDRAWAL GAME CREDIT INTO CASH WALLET WITH AMOUNT:{1} AT:{2}", UserDetails.LoginId, WithdrawAmount, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };

                            //add new log for game credit transaction
                            var log_user_game_credit_transaction = new LogUserGameCreditTransaction
                            {
                                GameApi = "998 API",
                                TrasactionId = TransactionID,
                                MemberSrno = _MemberSrno,
                                Player = UserGameAccountDetails.GameId,
                                TransferAmount = result.Amount,
                                BeforeAmount = result.Before,
                                AfterAmount = result.After,
                                TransactionType = "WITHDRAWAL",
                                Status = "SUCCESS",
                                TransferDate = DateTime.Now
                            };

                            //DEDUCT TRANSFER AMOUNT FROM USER CASH WALLET
                            var UserWalletDetails = _db.MstUserWallet.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();
                            UserWalletDetails.CashCredit += WithdrawAmount;


                            //Update all the changes into database
                            _db.LogUserTrackingWallet.Add(log_user_tracking_wallet);
                            _db.LogUserGameCreditTransaction.Add(log_user_game_credit_transaction);
                            await _db.SaveChangesAsync();
                            dbContextTransaction.Commit();

                            return Models_General.SUCC_WITHDRAWAL_GAME_CREDIT;
                        }

                        return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                    }

                    return Models_General.ERR_USER_NOT_FOUND;


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    var new_error = new LogErrorSystem
                    {
                        Title = "Withdraw Game Credit",
                        Details = ex.Message + "/"+ ex.StackTrace,
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
}
