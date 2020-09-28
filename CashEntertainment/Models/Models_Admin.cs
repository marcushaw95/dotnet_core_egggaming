using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_Admin
    {

        public class Models_Admin_Dashboard
        {
            public int? TotalMember { get; set; }
            public decimal? TotalTopup { get; set; }
            public decimal? TotalWithdrawal { get; set; }

            public Models_Monthly_Member_HightChart ChartData { get; set; }

        }

        public class Models_Monthly_Member_HightChart
        {

            public string[] xAxis { get; set; }

            public List<Models_HightChart_Series> series { get; set; }
        }

        public class Models_HightChart_Series
        {
            public string name { get; set; }
            public int[] data { get; set; }
        }
    }
}
