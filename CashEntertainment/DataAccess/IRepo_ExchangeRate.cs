using System;
using System.Threading.Tasks;
using CashEntertainment.DB;
using System.Collections.Generic;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_ExchangeRate
    {
        public List<MstExchangeRate> RetrieveExchangeRateListing();

        public int CreateExchangeRate(string AdminID, string Base, string Currency, decimal Rate);

        public int UpdateExchangeRate(long ExchangeRateSrno, string AdminID, string Base, string Currency, decimal Rate);

    }
}
