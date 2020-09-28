using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstSettings
    {
        public long Srno { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ValueType { get; set; }
        public bool IsEdit { get; set; }
    }
}
