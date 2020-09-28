using CashEntertainment.DB;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Admin;
using static CashEntertainment.Models.Models_User;

namespace CashEntertainment.DataAccess
{
    public class Repo_Admin : IRepo_Admin
    {
        private readonly UAT_CasinoContext _db;
        public readonly UploadImagesHelper _image_services;
        public Repo_Admin(UAT_CasinoContext db, UploadImagesHelper image_services)
        {
            _db = db;
            _image_services = image_services;
        }

        public Models_Monthly_Member_HightChart ChartData()
        {
            var raw = (from t1 in _db.MstUser.AsNoTracking().AsEnumerable()
                       select new
                       {
                           CountryType = t1.Country,
                           Month = t1.RegisterDate.ToString("MMMM"),
                       });

            var filter_1 = raw.GroupBy(x => new { x.CountryType, x.Month }).Select(g => new
            {
                name = g.Key.CountryType,
                data = g.Count()


            });
            var final_filter = filter_1.GroupBy(i => i.name).Select(g => new Models_HightChart_Series
            {

                name = g.Key,
                data = g.Select(i => i.data).ToArray()


            }).ToList();


            var result = new Models_Monthly_Member_HightChart
            {
                xAxis = raw.Select(x => x.Month).Distinct().ToArray(),
                series = final_filter

            };

            return result;
        }
        public int? RetrieveTotalMember()
        {
            return _db.MstUserAccount.Where(x => x.Status == "ACTIVE").Count();
        }
        public decimal? RetrieveTotalTopUp()
        {
            return _db.MstTopUp.Where(x => x.Status == 1).Count();
        }
        public decimal? RetrieveTotalWithdrawal()
        {
            return _db.MstWithdraw.Where(x => x.Status == 1).Count();
        }
    }
}
