using CashEntertainment.DB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_Announcement
    {
        int CreateAnnoucement(string AdminID, string TitleEN, string TitleCN, string TitleMS, string AnnoucementContentEN, string AnnouncementContentCN, string AnnouncementContentMS, bool IsPublish, bool IsImagePublish, string WebPath, string BaseURL, IFormFile AnnoucementImg = null);
        int UpdateAnnoucement(long AnnoucementSrno, string AdminID, string TitleEN, string TitleCN, string TitleMS, string AnnoucementContentEN, string AnnouncementContentCN, string AnnouncementContentMS, bool IsPublish, bool IsImagePublish, string WebPath, string BaseURL,IFormFile AnnoucementImg =null);
        int DeleteAnnoucement(long AnnoucementSrno);
        int CreateNewBanner(string AdminID, IFormFile BannerImage, string RedirectURL, bool IsActive, string WebPath, string BaseURL);
        int UpdateBanner(long BannerSrno, string RedirectURL, bool IsActive, string WebPath, string BaseURL, IFormFile BannerImage = null);
        List<MstAnnouncement> RetrieveAnnouncementListing();
        List<MstBanner> RetrieveBannerListing();
    }
}
