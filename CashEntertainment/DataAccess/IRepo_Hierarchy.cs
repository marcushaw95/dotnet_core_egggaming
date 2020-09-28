using System;
using System.Threading.Tasks;
using CashEntertainment.DB;
using System.Collections.Generic;
using static CashEntertainment.Models.Models_User;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Hierarchy
    {
        public List<Models_User_Account_Downline_Listing> GetDownlineUsers(long UserId);

    }
}

