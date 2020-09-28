
using CashEntertainment.DB;
using CashEntertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CashEntertainment.DataAccess
{
    public class Repo_Settings : IRepo_Settings
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;

        public Repo_Settings(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }

        public List<MstSettings> RetrieveSettingsListing()
        {
            try
            {
                return _db.MstSettings.ToList();
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Settings Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }

         
        }

        public int UpdateSettings(long SettingSrno, string AdminID, string SettingValue)
        {
            try
            {
                var Settings_Details = _db.MstSettings.Where(x => x.Srno == SettingSrno).FirstOrDefault();

                if (Settings_Details != null)
                {
                    Settings_Details.SettingValue = SettingValue;
                    Settings_Details.LastUpdateBy = AdminID;
                    Settings_Details.ModifiedDateTime = DateTime.Now;
                    _db.SaveChanges();

                    return Models_General.SUCC_ADMIN_UPDATE_SETTINGS;
                }

                return Models_General.ERR_SETTINGS_NOT_FOUND;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Update Settings",
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
