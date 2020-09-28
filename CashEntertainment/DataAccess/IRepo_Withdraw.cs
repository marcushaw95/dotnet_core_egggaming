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
    public interface IRepo_Withdraw
    {
        int MemberRequestWithdrawalOnlineBanking(long _MemberSrno, decimal WithdrawalAmount, long BankSrno, string Currenc);
        int MemberRequestWithdrawalCrypto(long _MemberSrno, decimal WithdrawalAmount, string ToAddress, decimal Rate, string Currency);
        int AdminApproveWithdrawal(string AdminID, long withdrawalsrno, bool approvestatus, string remarks);
        List<MstWithdraw> RetrieveWithdrawalListing(long? MemberSrno);
        List<Models_Withdrawal_List> AdminRetrieveWithdrawalListing();
        Task<int> WithdrawGameCredit(long _MemberSrno, decimal WithdrawAmount);

    }
}
