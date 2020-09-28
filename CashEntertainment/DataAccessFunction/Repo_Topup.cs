using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using CashEntertainment.Helper;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Topup;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CashEntertainment.DataAccess
{
    public class Repo_Topup : IRepo_Topup
    {
        private readonly UAT_CasinoContext _db;
        public readonly UploadImagesHelper _image_services;
        private readonly IActionContextAccessor _accessor;
        private readonly Intergration _intergration;
        public Repo_Topup(UAT_CasinoContext db, UploadImagesHelper image_services, IActionContextAccessor accessor, Intergration intergration)
        {
            _db = db;
            _image_services = image_services;
            _accessor = accessor;
            _intergration = intergration;
        }


        public int MemberRequestTopupWithOnlineBanking(long MemberSrno, long AdminBankSrno, decimal TopupAmount, IFormFile TopupImageProof, string TransactionReferenceNumber, string WebPath, string BaseURL, string Currency)
        {
            try
            {
                var CurrentAdminBank = _db.MstAdminBank.Where(x => x.Srno == AdminBankSrno).FirstOrDefault();
                if (CurrentAdminBank != null)
                {
                    var CurrentMstBank = _db.MstBank.Where(x => x.Srno == CurrentAdminBank.BankSrno).FirstOrDefault();
                    if (CurrentMstBank != null)
                    {
                        var image_url = _image_services.SaveImage(TopupImageProof, WebPath, "TOPUP", BaseURL);
                        if (!string.IsNullOrEmpty(image_url))
                        {
                            var TopupRequest = new MstTopUp
                            {
                                MemberSrno = MemberSrno,
                                WalletType = "CASH WALLET",
                                TopupAmount = TopupAmount,
                                TopupImageProof = image_url,
                                TransactionReferenceNumber = TransactionReferenceNumber,
                                Currency = Currency,
                                Status = 0,
                                RequestDate = DateTime.Now,
                                BankCode = CurrentMstBank.BankCode,
                                BankName = CurrentMstBank.BankName,
                                BankAccountHolder = CurrentAdminBank.BankAccountHolder,
                                BankAccountNumber = CurrentAdminBank.BankCardNo,
                                TransactionType = 0
                            };
                            _db.MstTopUp.Add(TopupRequest);
                            _db.SaveChanges();
                            return Models_General.SUCC_CREATE_REQUEST_TOPUP;
                        }
                        else
                        {
                            return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                        }
                    }
                    else
                    {
                        return Models_General.ERR_BANK_INFO_NOT_MATCH;
                    }
                }
                else
                {
                    return Models_General.ERR_ADMIN_BANK_NOT_FOUND;
                }

            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Member Request Topup With Online Banking",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }


        }
        public int MemberRequestTopupWithCrypto(long MemberSrno,  decimal TopupAmount, decimal Rate, string Currency, string TransactionHash)
        {
            try
            {
                            var TopupRequest = new MstTopUp
                            {
                                MemberSrno = MemberSrno,
                                TopupAmount = TopupAmount,
                                Rate = Rate,
                                WalletType = "CASH WALLET",
                                Currency = Currency,
                                TransactionHash = TransactionHash,
                                Status = 0,
                                RequestDate = DateTime.Now,
                                TransactionType = 1
                            };
                            _db.MstTopUp.Add(TopupRequest);
                            _db.SaveChanges();
                            return Models_General.SUCC_CREATE_REQUEST_TOPUP;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Member Request Topup With Crypto",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }


        }
        public int AdminTopupApproval(string AdminID, long TopupSrno, bool ApproveStatus, string Remarks)
        {
            try
            {
                int result;
                var CurrentUserTopup = _db.MstTopUp.Where(x => x.Srno == TopupSrno && x.Status == 0).FirstOrDefault();
                if (CurrentUserTopup != null)
                {

                    var CurrentUserWallet = _db.MstUserWallet.Where(x => x.MemberSrno == CurrentUserTopup.MemberSrno).FirstOrDefault();
                    if (ApproveStatus)
                    {
                        var CurrentSettings = _db.MstSettings.Where(x => x.SettingName == "NormalTurnover").FirstOrDefault();

                        if (CurrentUserTopup.TransactionType == 0)
                        {
                            var user_topup_request_tracking = new LogUserTrackingWallet
                            {
                                MemberSrno = CurrentUserTopup.MemberSrno,
                                WalletFrom = CurrentUserTopup.BankName + " " + CurrentUserTopup.BankAccountNumber + " " + CurrentUserTopup.BankAccountHolder + " with reference number " + CurrentUserTopup.TransactionReferenceNumber,
                                WalletTo = "CASH WALLET",
                                TransactionType = 0,
                                PreviousAmount = CurrentUserWallet.CashCredit,
                                TransactionAmount = (decimal)CurrentUserTopup.TopupAmount,
                                CurrentTotalAmount = CurrentUserWallet.CashCredit + (decimal)CurrentUserTopup.TopupAmount,
                                IsDeduct = false,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, CurrentUserTopup.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                            };


                            var log_topup = new LogTopup
                            {
                                MemberSrno = CurrentUserTopup.MemberSrno,
                                WalletFrom = CurrentUserTopup.BankName + " " + CurrentUserTopup.BankAccountNumber + " " + CurrentUserTopup.BankAccountHolder + " with reference number " + CurrentUserTopup.TransactionReferenceNumber,
                                WalletTo = "CASH WALLET",
                                TransactionType = 0,
                                PreviousAmount = CurrentUserWallet.CashCredit,
                                TransactionAmount = (decimal)CurrentUserTopup.TopupAmount,
                                CurrentTotalAmount = CurrentUserWallet.CashCredit + (decimal)CurrentUserTopup.TopupAmount,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, CurrentUserTopup.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,

                            };

                            _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                            _db.LogTopup.Add(log_topup);


                            result = Models_General.SUCC_ADMIN_APPROVE_TOPUP;
                        }
                        else if (CurrentUserTopup.TransactionType == 1)
                        {
                            var user_topup_request_tracking = new LogUserTrackingWallet
                            {
                                MemberSrno = CurrentUserTopup.MemberSrno,
                                WalletFrom = "CRYPTO CURRENCY WITH TRANSACTION HASH"+ CurrentUserTopup.TransactionHash,
                                WalletTo = "CASH WALLET",
                                TransactionType = 2,
                                PreviousAmount = CurrentUserWallet.CashCredit,
                                TransactionAmount = (decimal)CurrentUserTopup.TopupAmount,
                                CurrentTotalAmount = CurrentUserWallet.CashCredit + (decimal)CurrentUserTopup.TopupAmount,
                                IsDeduct = false,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, CurrentUserTopup.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                            };


                            var log_topup = new LogTopup
                            {
                                MemberSrno = CurrentUserTopup.MemberSrno,
                                WalletFrom = "CRYPTO CURRENCY WITH TRANSACTION HASH" + CurrentUserTopup.TransactionHash,
                                WalletTo = "CASH WALLET",
                                TransactionType = 2,
                                PreviousAmount = CurrentUserWallet.CashCredit,
                                TransactionAmount = (decimal)CurrentUserTopup.TopupAmount,
                                CurrentTotalAmount = CurrentUserWallet.CashCredit + (decimal)CurrentUserTopup.TopupAmount,
                                Description = string.Format("ADMIN:{0} APPROVED THIS TOPUP SERIAL NUMBER:{1} AT:{2}", AdminID, CurrentUserTopup.Srno, DateTime.Now),
                                CreatedDateTime = DateTime.Now,
                            };

                            _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                            _db.LogTopup.Add(log_topup);


                            result = Models_General.SUCC_ADMIN_APPROVE_TOPUP;

                        }
                        else
                        {
                            result = Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                        }


                        decimal finalturnover = CurrentUserTopup.TopupAmount * int.Parse(CurrentSettings.SettingValue);
                        CurrentUserTopup.Status = 1;
                        CurrentUserTopup.ApproveBy = AdminID;
                        CurrentUserTopup.ApproveDate = DateTime.Now;
                        CurrentUserTopup.ApproveRemark = Remarks;
                        CurrentUserWallet.CashCredit += CurrentUserTopup.TopupAmount;
                        CurrentUserWallet.TurnoverAmount += finalturnover;
                    }
                    else
                    {
                        CurrentUserTopup.Status = 2;
                        CurrentUserTopup.ApproveBy = AdminID;
                        CurrentUserTopup.ApproveDate = DateTime.Now;
                        CurrentUserTopup.ApproveRemark = Remarks;

                        result = Models_General.SUCC_ADMIN_REJECT_TOPUP;
                    }

                    _db.SaveChanges();
                    return result;
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
                    Title = "Admin Approve Topup",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public List<MstTopUp> RetrieveTopupListing(long? _MemberSrno)
        {

            try
            {
                var data = _db.MstTopUp.ToList();

                if (_MemberSrno != null)
                {
                    data = _db.MstTopUp.Where(x => x.MemberSrno == _MemberSrno).ToList();
                }
                return data;
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Topup Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();

                return null;
            }

           
        }
        public List<Models_Topup_List> AdminRetrieveTopupListing()
        {

            try
            {
                var data = (from t1 in _db.MstTopUp
                            join t2 in _db.MstUserAccount on t1.MemberSrno equals t2.MemberSrno
                            select new Models_Topup_List
                            {
                                Srno = t1.Srno,
                                LoginId = t2.LoginId,
                                MemberSrno = t1.MemberSrno,
                                WalletType = t1.WalletType,
                                TopupAmount = t1.TopupAmount,
                                Status = t1.Status,
                                Remarks = t1.Remarks,
                                RequestDate = t1.RequestDate,
                                ApproveBy = t1.ApproveBy,
                                ApproveRemark = t1.ApproveRemark,
                                ApproveDate = t1.ApproveDate,
                                RejectBy = t1.RejectBy,
                                RejectRemark = t1.RejectRemark,
                                RejectDate = t1.RejectDate,
                                TopupImageProof = t1.TopupImageProof,
                                TransactionReferenceNumber = t1.TransactionReferenceNumber,
                                BankCode = t1.BankCode,
                                BankAccountNumber = t1.BankAccountNumber,
                                BankName = t1.BankName,
                                BankAccountHolder = t1.BankAccountHolder,
                                TransactionType = t1.TransactionType,
                                TransactionHash = t1.TransactionHash,
                                Rate = t1.Rate,
                                Currency = t1.Currency,
                            }).OrderByDescending(x => x.RequestDate).ToList();
                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Admin Retrieve Topup Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();

                return null;
            }


         
        }
        public async Task<int> TopUpGameCredit(long _MemberSrno, decimal TransferAmount)
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

                        if (UserWalletData.CashCredit >= TransferAmount)
                        {
                            var TransactionID = new Guid().ToString();
                            //CALL 998 API TO TRANSFER CASH CREDIT TO GAME CREDIT
                            var result = await _intergration.TransferGameCredit(UserGameAccountDetails.GameId, TransferAmount, TransactionID);

                            if (result.Error == 0)
                            {
                                // add new log from cash credit transaction
                                var log_user_tracking_wallet = new LogUserTrackingWallet
                                {
                                    MemberSrno = _MemberSrno,
                                    WalletFrom = "CASH WALLET",
                                    WalletTo = "GAME WALLET",
                                    TransactionType = 4,
                                    PreviousAmount = UserWalletData.CashCredit,
                                    TransactionAmount = TransferAmount,
                                    CurrentTotalAmount = UserWalletData.CashCredit - TransferAmount,
                                    IsDeduct = true,
                                    Description = string.Format("MEMBER:{0} TRANSFER CASH INTO GAME WALLET WITH AMOUNT:{1} AT:{2}", UserDetails.LoginId, TransferAmount, DateTime.Now),
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
                                    TransactionType = "DEPOSIT",
                                    Status = "SUCCESS",
                                    TransferDate = DateTime.Now
                                };

                                //DEDUCT TRANSFER AMOUNT FROM USER CASH WALLET
                                var UserWalletDetails = _db.MstUserWallet.Where(x => x.MemberSrno == _MemberSrno).FirstOrDefault();
                                UserWalletDetails.CashCredit -= TransferAmount;


                                //Update all the changes into database
                                _db.LogUserTrackingWallet.Add(log_user_tracking_wallet);
                                _db.LogUserGameCreditTransaction.Add(log_user_game_credit_transaction);
                                await _db.SaveChangesAsync();
                                dbContextTransaction.Commit();

                                return Models_General.SUCC_TOPUP_GAME_CREDIT;
                            }

                            return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
                        }

                        return Models_General.ERR_INSUFFICIENT_CASH_BALANCE;
                    }

                    return Models_General.ERR_USER_NOT_FOUND;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    var new_error = new LogErrorSystem
                    {
                        Title = "Topup Game Credit",
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


