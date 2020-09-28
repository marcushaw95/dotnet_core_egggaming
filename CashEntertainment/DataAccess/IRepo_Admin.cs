using CashEntertainment.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Admin;
using static CashEntertainment.Models.Models_User;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Admin
    {
        Models_Monthly_Member_HightChart ChartData();
        int? RetrieveTotalMember();
        decimal? RetrieveTotalTopUp();
        decimal? RetrieveTotalWithdrawal();
 

    }
}
