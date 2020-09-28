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
    public interface IRepo_Topup
    {
       
        int MemberRequestTopupWithOnlineBanking(long MemberSrno, long BankSrno, decimal TopupAmount, IFormFile TopupImageProof, string TransactionReferenceNumber, string WebPath, string BaseURL, string Currency);
        int MemberRequestTopupWithCrypto(long MemberSrno, decimal TopupAmount, decimal Rate, string Currency, string TransactionHash);
        int AdminTopupApproval(string AdminID, long Srno, bool ApproveStatus, string Remarks);
        List<MstTopUp> RetrieveTopupListing(long? _MemberSrno);
        List<Models_Topup_List> AdminRetrieveTopupListing();
        Task<int> TopUpGameCredit(long _MemberSrno, decimal TransferAmount);
       
    }
}

