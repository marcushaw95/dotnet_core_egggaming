using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Games
    {

        public class GameListing
        {
            public string gametype { get; set; }

            public List<GameDescription> data { get; set; }
        }

        public class GameDescription
        {
            public long Srno { get; set; }
            public string GameCategory { get; set; }
            public string VendorCode { get; set; }
            public string VendorName { get; set; }
            public string ImageUrl { get; set; }
            public byte? Maintenance { get; set; }
            public string Status { get; set; }

            public byte? IsSubgame { get; set; }
        }


        public class SubGameDescription
        {
            public string GameType { get; set; }
            public string VendorCode { get; set; }
            public string GameName { get; set; }
            public string GameCode { get; set; }
            public string ImageUrl { get; set; }
            public string Status { get; set; }


        }
    }
}
