using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_Games;

namespace CashEntertainment.DataAccess
{
    public  interface IRepo_Game
    {
        Task<Tuple<int, string>> LoginGame(string VendorCode, long MemberSrno, string browserType, string language, string gameCode);

        List<GameDescription> RetrieveGameListing();

        List<SubGameDescription> RetrieveSubGameListing();

        int ChangeGameMaintenance(long Gamesrno, byte Maintenancestatus);

        Task<int> GetTicketsByFetch();

        Task<int> UpdateGameWalletAmounts();

        Task<int> RecollectGameWalletAmounts();

        Task<Tuple<int, int>> CheckUserGameRegister();

    }
}
