using CashEntertainment.DB;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace CashEntertainment.DataAccess
{
    public class Repo_Announcement : IRepo_Announcement
    {
        private readonly UAT_CasinoContext _db;
        public readonly UploadImagesHelper _image_services;
        private readonly IActionContextAccessor _accessor;

        public Repo_Announcement(UAT_CasinoContext db, UploadImagesHelper image_services, IActionContextAccessor accessor)
        {
            _db = db;
            _image_services = image_services;
            _accessor = accessor;
        }

        public int CreateAnnoucement(string AdminID, string TitleEN, string TitleCN, string TitleMS, string AnnoucementContentEN, string AnnouncementContentCN, string AnnouncementContentMS, bool IsPublish, bool IsImagePublish, string WebPath, string BaseURL, IFormFile AnnoucementImg = null)
        {
            try
            {
                var imageurl = "";
                if (AnnoucementImg != null)
                {
                    var image_url = _image_services.SaveImage(AnnoucementImg, WebPath, "ANNOUNCEMENT", BaseURL);

                    if (!string.IsNullOrEmpty(image_url))
                    {
                        imageurl = image_url;
                    }
                }

                var new_announcement = new MstAnnouncement
                {
                    TitleEn = TitleEN,
                    TitleCn = TitleCN,
                    TitleMs = TitleMS,
                    AnnouncementContentEn = AnnoucementContentEN,
                    AnnouncementContentCn = AnnouncementContentCN,
                    AnnouncementContentMs = AnnouncementContentMS,
                    IsPublish = IsPublish,
                    IsImagePublish = IsImagePublish,
                    AnnouncementImagePath = imageurl,
                    LastUpdateBy = AdminID,
                    LastModifiedDate = DateTime.Now,
                    CreatedDateTime = DateTime.Now,
                };
                _db.MstAnnouncement.Add(new_announcement);
                _db.SaveChanges();

                return Models_General.SUCC_ADMIN_CREATE_ANNOUNCEMENT;

            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Create Announcement",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int UpdateAnnoucement(long AnnoucementSrno, string AdminID, string TitleEN, string TitleCN, string TitleMS, string AnnoucementContentEN, string AnnouncementContentCN, string AnnouncementContentMS, bool IsPublish, bool IsImagePublish, string WebPath, string BaseURL, IFormFile AnnoucementImg= null)
        {
            try
            {
                var Announcement_Details = _db.MstAnnouncement.Where(x => x.Srno == AnnoucementSrno).FirstOrDefault();

                if (Announcement_Details != null)
                {
                    if (AnnoucementImg != null)
                    {
                        var image_url = _image_services.SaveImage(AnnoucementImg, WebPath, "ANNOUNCEMENT", BaseURL);

                        if (!string.IsNullOrEmpty(image_url))
                        {
                            Announcement_Details.AnnouncementImagePath = image_url;
                        }
                    }

                    Announcement_Details.TitleEn = TitleEN;
                    Announcement_Details.TitleCn = TitleCN;
                    Announcement_Details.TitleMs = TitleMS;
                    Announcement_Details.AnnouncementContentEn = AnnoucementContentEN;
                    Announcement_Details.AnnouncementContentCn = AnnouncementContentCN;
                    Announcement_Details.AnnouncementContentMs = AnnouncementContentMS;
                    Announcement_Details.IsPublish = IsPublish;
                    Announcement_Details.IsImagePublish = IsImagePublish;
                    Announcement_Details.LastUpdateBy = AdminID;
                    Announcement_Details.LastModifiedDate = DateTime.Now;
                    _db.SaveChanges();

                    return Models_General.SUCC_ADMIN_UPDATE_ANNOUNCEMENT;
                }

                return Models_General.ERR_ANNOUNCEMENT_NOT_FOUND;
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Update Announcement",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();

                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public int DeleteAnnoucement(long AnnoucementSrno)
        {
            try
            {
                var Announcement_Details = _db.MstAnnouncement.Where(x => x.Srno == AnnoucementSrno).FirstOrDefault();
                if (Announcement_Details != null)
                {
                    _db.MstAnnouncement.Remove(Announcement_Details);
                    _db.SaveChanges();
                    return Models_General.SUCC_ADMIN_DELETE_ANNOUNCEMENT;
                }
                return Models_General.ERR_ANNOUNCEMENT_NOT_FOUND;
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Delete Announcement",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();

                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }

        public List<MstAnnouncement> RetrieveAnnouncementListing()
        {
            try
            {
                return _db.MstAnnouncement.ToList();
            }
            catch(Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Announcement Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
            
        }

        public List<MstBanner> RetrieveBannerListing()
        {
            try
            {
                return _db.MstBanner.ToList();
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Retrieve Banner Listing",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }

        public int CreateNewBanner(string AdminID, IFormFile BannerImage, string RedirectURL, bool IsActive, string WebPath, string BaseURL)
        {
            try
            {
                var image_url = _image_services.SaveImage(BannerImage, WebPath, "BANNER", BaseURL);
                if (!string.IsNullOrEmpty(image_url))
                {

                    var new_banner = new MstBanner
                    {
                        ImagePath = image_url,
                        RedirectUrl = RedirectURL,
                        IsActive = IsActive,
                        CreatedBy = AdminID,
                        CreatedDateTime = DateTime.Now,
                    };
                    _db.MstBanner.Add(new_banner);
                    _db.SaveChanges();
                    return Models_General.SUCC_ADMIN_CREATE_BANNER;
                }
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
            catch (Exception ex)
            {

                var new_error = new LogErrorSystem
                {
                    Title = "Create Banner",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }

        }

        public int UpdateBanner(long BannerSrno,  string RedirectURL, bool IsActive, string WebPath, string BaseURL, IFormFile BannerImage = null)
        {
            try
            {
                var Banner_Details = _db.MstBanner.Where(x => x.Srno == BannerSrno).FirstOrDefault();
                if (Banner_Details != null)
                {
                    if (BannerImage != null)
                    {
                        var image_url = _image_services.SaveImage(BannerImage, WebPath, "BANNER", BaseURL);

                        if (!string.IsNullOrEmpty(image_url))
                        {
                            Banner_Details.ImagePath = image_url;
                        }
                    }

                    Banner_Details.RedirectUrl = RedirectURL;
                    Banner_Details.IsActive = IsActive;
                    _db.SaveChanges();
                    return Models_General.SUCC_ADMIN_UPDATE_BANNER;
                }


                return Models_General.ERR_BANNER_NOT_FOUND;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Update Banner",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR;
            }
        }
    }
}
