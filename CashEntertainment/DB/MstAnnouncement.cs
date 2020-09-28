using System;
using System.Collections.Generic;

namespace CashEntertainment.DB
{
    public partial class MstAnnouncement
    {
        public long Srno { get; set; }
        public string TitleEn { get; set; }
        public string TitleCn { get; set; }
        public string AnnouncementContentEn { get; set; }
        public string AnnouncementContentCn { get; set; }
        public bool? IsPublish { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string AnnouncementImagePath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string TitleMs { get; set; }
        public string AnnouncementContentMs { get; set; }
        public bool IsImagePublish { get; set; }
    }
}
