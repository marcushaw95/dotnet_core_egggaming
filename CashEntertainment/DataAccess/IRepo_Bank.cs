using CashEntertainment.DB;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Topup;
using static CashEntertainment.Models.Models_Bank;
using static CashEntertainment.Models.Models_Withdrawal;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Bank
    {
 
        int MemberAddNewBank(long MemberSrno, long BankSrno, string BankAccountHolderName, string BankCardNo);

        int MemberUpdateBank(long MemberSrno, long UserBankSrno, long BankSrno, string BankAccountHolderName, string BankCardNo);

        int AdminAddNewBank(long BankSrno, string BankAccountHolderName, string BankCardNo, string Country);

        int AdminUpdateBank(long AdminBankSrno, long BankSrno, string BankAccountHolderName, string BankCardNo, string Country);
        List<MstBank> RetrieveBankList();
        List<MstPaylah88Bank> RetrievePaylah88BankList();
        List<Models_Member_Bank_List> RetrieveUserBankList();
        List<Models_Admin_Bank_List> RetrieveAdminBankList();



    }
}
