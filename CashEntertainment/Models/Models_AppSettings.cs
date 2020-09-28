using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
    public class Models_AppSettings
    {
        public string JWTSecret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
