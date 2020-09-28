
using CashEntertainment.DB;
using CashEntertainment.Helper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CashEntertainment.Models.Models_User;
using System;

namespace CashEntertainment.DataAccess
{
    public class Repo_Hierarchy : IRepo_Hierarchy
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;

        public Repo_Hierarchy(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }

        public List<Models_User_Downline_Listing> GetDownlineUsersExcludeOwner(long UplineId, int level = 0, int maxLevel = 0)
        {
   
                var trees = new List<Models_User_Downline_Listing>();
                var users = _db.MstUser.Where(x => x.UplineId == UplineId).ToList();
                foreach (var user in users)
                {
                    trees.Add(new Models_User_Downline_Listing
                    {
                        UserId = user.MemberSrno,
                        UplineId = user.UplineId,
                        Level = level,

                    });

                    if (maxLevel <= 0 || maxLevel >= level + 1)
                        trees.AddRange(GetDownlineUsersExcludeOwner(user.MemberSrno, level + 1, maxLevel));
                }
                return trees.OrderBy(x => x.Level).ToList();

        }



        public List<Models_User_Account_Downline_Listing> GetDownlineUsers(long UserId)
        {
            try
            {
                var trees = new List<Models_User_Downline_Listing>();
                var user = _db.MstUser.Where(x => x.MemberSrno == UserId).FirstOrDefault();
                trees.Add(new Models_User_Downline_Listing
                {
                    UserId = user.MemberSrno,
                    UplineId = user.UplineId,
                    Level = 0,
                });

                trees.AddRange(GetDownlineUsersExcludeOwner(user.MemberSrno, 1));

                var result = (from t1 in trees
                              join t2 in _db.MstUserAccount on t1.UserId equals t2.MemberSrno
                              join t3 in _db.MstUserWallet on t1.UserId equals t3.MemberSrno
                              select new Models_User_Account_Downline_Listing
                              {
                                  UplineId = t1.UplineId,
                                  Level = t1.Level,
                                  UserId = t1.UserId,
                                  LoginId = t2.LoginId,
                                  CashCredit = t3.CashCredit,
                                  TurnoverAmount = t3.TurnoverAmount
                              }).OrderBy(x => x.Level).ToList();
                return result;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve User Downline Listing",
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
