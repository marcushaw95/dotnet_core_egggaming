
using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace CashEntertainment.DataAccess
{
    public class Repo_ExchangeRate : IRepo_ExchangeRate
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;
        public Repo_ExchangeRate(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }

        public List<MstExchangeRate> RetrieveExchangeRateListing()
        {

            try
            {
                return _db.MstExchangeRate.ToList();
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Exchange Rate Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
            
        }


        public int CreateExchangeRate(string AdminID, string Base, string Currency, decimal Rate)
        {
            try
            {

                if (IsExchangeRateExist(Base,Currency))
                {
                    return Models_General.ERR_EXCHANGE_RATE_CURRENCY_EXIST;
                }

                var new_exchangerate = new MstExchangeRate
                {
                    Base = Base,
                    Currency = Currency,
                    Rate = Rate,
                    LastUpdateBy = AdminID,
                    CreatedDateTime = DateTime.Now,
                };
                _db.MstExchangeRate.Add(new_exchangerate);
                _db.SaveChanges();

                return Models_General.SUCC_ADMIN_CREATE_EXCHANGE_RATE;

            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Create Exchange Rate",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int UpdateExchangeRate(long ExchangeRateSrno, string AdminID, string Base, string Currency, decimal Rate)
        {
            try
            {
                var ExchangeRate_Details = _db.MstExchangeRate.Where(x => x.Srno == ExchangeRateSrno).FirstOrDefault();

                var checkExchangeRate = _db.MstExchangeRate.Where(x =>x.Base == Base && x.Currency == Currency ).FirstOrDefault();

                if (ExchangeRate_Details != null)
                {
                    if (checkExchangeRate != null && checkExchangeRate.Srno != ExchangeRateSrno)
                    {
                        return Models_General.ERR_EXCHANGE_RATE_CURRENCY_EXIST;
                    }
                    ExchangeRate_Details.Base = Base;
                    ExchangeRate_Details.Currency = Currency;
                    ExchangeRate_Details.Rate = Rate;
                    ExchangeRate_Details.LastUpdateBy = AdminID;
                    ExchangeRate_Details.ModifiedDateTime = DateTime.Now;
                    _db.SaveChanges();

                    return Models_General.SUCC_ADMIN_UPDATE_EXCHANGE_RATE;
                }

                return Models_General.ERR_EXCHANGE_RATE_NOT_FOUND;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Update Exchange Rate",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        private bool IsExchangeRateExist(string Base, string Currency)
        {
            return _db.MstExchangeRate.Where(x => x.Base == Base && x.Currency ==Currency).Any();
        }

    }
}