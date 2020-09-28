using System;
using System.Threading.Tasks;
using CashEntertainment.DB;
using System.Collections.Generic;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Settings
    {
        public List<MstSettings> RetrieveSettingsListing();
        public int UpdateSettings(long SettingSrno, string AdminID, string SettingValue);

    }
}
