using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_User;
using static CashEntertainment.Models.Models_Winlose;
using CashEntertainment.DB;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Winlose
    {
        List<Models_Winlose_List> RetrieveWinloseList();
        List<Models_Winlose_List> RetrieveWinloseListByFilter(string loginid, int vendor, string startdate, string enddate);
    }
}
