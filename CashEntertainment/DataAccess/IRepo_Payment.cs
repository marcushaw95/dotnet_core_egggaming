using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Payment
    {
        public int RequestDepositPaymentGateway(long MemberSrno,string DepositUrl, string Merchant, string Currency, string Customer, string Reference, string Key, decimal Amount, string Note, string Datetime, string FrontUrl, string BackUrl, string Language, string Bank, string ClientIp);

        
        public int BackResponseDepositPaymentGateway(string Merchant, string Reference, string Currency, string Amount, string Language, string Customer, string Datetimme, string Note, string Key, string Status, string ID);
    }
}
