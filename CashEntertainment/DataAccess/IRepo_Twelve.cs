using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Twelve;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Twelve
    {

        Task<Tuple<int, long>> UserRegisterNewAccount(string LoginID, string Password, string CountryCode, string Upline);

        public int UserChangeUpline(string LoginID, string Upline);
        public int TopupCredit(string loginid, decimal TopupAmount, bool IsReset);

        List<Models_Twelve_Turnover_List> GetTwelveTurnoverRemainingAmount(string[] loginids);
    }
}
