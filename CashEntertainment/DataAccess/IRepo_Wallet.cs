using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_User;
using CashEntertainment.DB;
using static CashEntertainment.Models.Models_Tracking_Wallet_Log;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Wallet
    {
        Tuple<int, MstUserWallet> GetUserWallet(long MemberSrno);

        Task<decimal> GetBalanceGameCredit(long _MemberSrno);

        int TransferWalletCredit(long Srno, string RefCode, decimal TransferAmount);
        int UpdateWalletTurnover(long Srno, long MemberSrno, decimal TurnoverAmount);

        List<LogUserTrackingWallet> RetrieveTrackingWalletList();

        List<Models_Tracking_Wallet_Log_List> RetrieveTrackingWalletLogListByFilter(string loginid, int transactiontype, string startdate, string enddate);

        int EditMainWalletCredit(string AdminID, long MemberSrno, int ManipulateType, decimal ManipulateAmount, decimal TurnoverAmount);
    }
}
