using _998Intergration;
using CashEntertainment.DB;
using CashEntertainment.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_Winlose;


namespace CashEntertainment.DataAccess
{
    public class Repo_Winlose : IRepo_Winlose
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;

        public Repo_Winlose(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }


        public List<Models_Winlose_List> RetrieveWinloseList()
        {
            try
            {

                var data = (from t1 in _db.MstWinlose
                            join t2 in _db.MstUserAccount on t1.MemberSrno equals t2.MemberSrno
                            select new Models_Winlose_List
                            {
                                MemberSrno = t1.MemberSrno,
                                WinloseAmount = t1.WinloseAmount,
                                StakeAmount = t1.StakeAmount,
                                Vendor = t1.Vendor,
                                Currency = t1.Currency,
                                CreatedDateTime = t1.CreatedDateTime,
                                Product = t1.Product,
                                GameType = t1.GameType,
                                LoginId = t2.LoginId,
                             }).OrderByDescending(x => x.CreatedDateTime).ToList();
                            return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Winlose List",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
             }
        }

        public List<Models_Winlose_List> RetrieveWinloseListByFilter(string loginid , int vendor, string startdate, string enddate)
        { 
            try
            {
                var end = Convert.ToDateTime(enddate).AddDays(1);

                var query1 = _db.MstWinlose.Where(x => x.CreatedDateTime >= Convert.ToDateTime(startdate) && x.CreatedDateTime < end).ToList();
                var query2 = _db.MstUserAccount.ToList();
                if (vendor > 0)
                {
                    query1 = query1.Where(x => x.Vendor == vendor).ToList();
                }


                if (!string.IsNullOrEmpty(loginid))
                {
                    query2 = query2.Where(x => x.LoginId == loginid).ToList();
                }
               
                    var data = (from t1 in query1
                                join t2 in query2 on t1.MemberSrno equals t2.MemberSrno
                            select new Models_Winlose_List
                            {
                                MemberSrno = t1.MemberSrno,
                                WinloseAmount = t1.WinloseAmount,
                                StakeAmount = t1.StakeAmount,
                                Vendor = t1.Vendor,
                                Currency = t1.Currency,
                                CreatedDateTime = t1.CreatedDateTime,
                                Product = t1.Product,
                                GameType = t1.GameType,
                                LoginId = t2.LoginId,
                            }).OrderByDescending(x => x.CreatedDateTime).ToList();
                return data;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Winlose List",
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