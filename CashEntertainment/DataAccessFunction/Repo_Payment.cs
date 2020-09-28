using _998Intergration;
using CashEntertainment.DB;
using CashEntertainment.Models;
using CashEntertainment.Helper;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CashEntertainment.DataAccess
{
    public class Repo_Payment : IRepo_Payment
    {

        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;

        public Repo_Payment(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }


        public int RequestDepositPaymentGateway(long MemberSrno,string DepositUrl, string Merchant, string Currency, string Customer, string Reference, string Key, decimal Amount, string Note, string Datetime, string FrontUrl, string BackUrl, string Language, string Bank, string ClientIp)
        {

            try
            {
                var PaymentGatewayRequest = new LogRequestPaymentGateway
                {
                    MemberSrno = MemberSrno,
                    DepositUrl= DepositUrl,
                   Merchant = Merchant,
                   Currency =  Currency,
                   Customer = Customer,
                   Reference = Reference,
                   Key = Key,
                   Amount = Amount,
                   Note = Note,
                   Datetime = Datetime,
                   FrontUrl = FrontUrl,
                   BackUrl = BackUrl,
                   Language = Language,
                   Bank = Bank,
                   ClientIp = ClientIp,
                   CreatedDateTime = DateTime.Now,
                };
                _db.LogRequestPaymentGateway.Add(PaymentGatewayRequest);
                _db.SaveChanges();
                return Models_General.SUCC_CREATE_PAYMENT_REQUEST;

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Request Deposit Payment Gateway",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
        public int BackResponseDepositPaymentGateway(string Merchant, string Reference, string Currency, string Amount, string Language, string Customer, string Datetime, string Note, string Key, string Status, string ID)
        {

            try
            {
                int result;
                var UserAccount = _db.MstUserAccount.Where(x => x.LoginId == Customer).FirstOrDefault();
                var UserWallet = _db.MstUserWallet.Where(x => x.MemberSrno == UserAccount.MemberSrno).FirstOrDefault();
                var CurrentSettings = _db.MstSettings.Where(x => x.SettingName == "NormalTurnover").FirstOrDefault();

                var PaymentGatewayResponse = new LogResponsePaymentGateway
                {
                    Merchant = Merchant,
                    Reference = Reference,
                    Currency = Currency,
                    Amount = Convert.ToDecimal(Amount),
                    Language = Language,
                    Customer = Customer,
                    Datetime = Datetime,
                    Note = Note,
                    Key = Key,
                    Status = Status,
                    Id = ID,
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogResponsePaymentGateway.Add(PaymentGatewayResponse);



                if (  Status  == "000"){
                    var user_topup_request_tracking = new LogUserTrackingWallet
                    {
                        MemberSrno = UserWallet.MemberSrno,
                        WalletFrom = "PAYLAH88",
                        WalletTo = "CASH WALLET",
                        TransactionType = 12,
                        PreviousAmount = UserWallet.CashCredit,
                        TransactionAmount = Convert.ToDecimal(Amount),
                        CurrentTotalAmount = UserWallet.CashCredit + Convert.ToDecimal(Amount),
                        IsDeduct = false,
                        Description = string.Format("User:{0} HAS TOPUP AMOUNT:{1} WITH {2} USING PAYLAH88 PAYMENT GATEWAY AT:{3}", Customer, Convert.ToDecimal(Amount),ID, DateTime.Now),
                        CreatedDateTime = DateTime.Now,
                    };


                    var log_topup = new LogTopup
                    {
                        MemberSrno = UserWallet.MemberSrno,
                        WalletFrom = "PAYLAH88",
                        WalletTo = "CASH WALLET",
                        TransactionType = 12,
                        PreviousAmount = UserWallet.CashCredit,
                        TransactionAmount = Convert.ToDecimal(Amount),
                        CurrentTotalAmount = UserWallet.CashCredit + Convert.ToDecimal(Amount),
                        Description = string.Format("User:{0} HAS TOPUP AMOUNT:{1} USING PAYLAH88 PAYMENT GATEWAY AT:{2}", Customer, Convert.ToDecimal(Amount), DateTime.Now),
                        CreatedDateTime = DateTime.Now,

                    };


                    var TopupRequest = new MstTopUp
                    {
                        MemberSrno = UserWallet.MemberSrno,
                        WalletType = "CASH WALLET",
                        TopupAmount = Convert.ToDecimal(Amount),
                        Currency = Currency,
                        Status = 1,
                        RequestDate = DateTime.Now,
                        TransactionReferenceNumber = ID,
                        TransactionType = 2,
                    };

                    _db.LogUserTrackingWallet.Add(user_topup_request_tracking);
                    _db.LogTopup.Add(log_topup);
                    _db.MstTopUp.Add(TopupRequest);

                    decimal finalturnover = Convert.ToDecimal(Amount) * int.Parse(CurrentSettings.SettingValue);
                    UserWallet.CashCredit += Convert.ToDecimal(Amount);
                    UserWallet.TurnoverAmount += finalturnover;


                    result = Models_General.SUCC_PAYMENT;

                }
                else
                {
                    result = Models_General.ERR_PAYMENT;
                }
             
                    _db.SaveChanges();
                    return result;
                

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Back Response Deposit Payment Gateway",
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















