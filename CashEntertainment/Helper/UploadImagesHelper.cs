using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using CashEntertainment.DB;

namespace CashEntertainment.Helper
{
    public class UploadImagesHelper
    {
        private readonly UAT_CasinoContext _db;
        private readonly IActionContextAccessor _accessor;


        public UploadImagesHelper(UAT_CasinoContext db, IActionContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;
        }


        public string SaveImage(IFormFile Images, string WebPath,string ImageType, string BaseURL)
        {
            try
            {
                if (Images != null)
                {
                    string UploadPath = null;
                    string URL = null;

                    switch (ImageType)
                    {
                        case "ANNOUNCEMENT":
                            UploadPath = Path.Combine(WebPath, "images/announcement");
                            URL = BaseURL+"/announcement";
                            break;
                        case "BANNER":
                            UploadPath = Path.Combine(WebPath, "images/banner");
                            URL = BaseURL+"/banner";
                            break;
                        case "TOPUP":
                            UploadPath = Path.Combine(WebPath, "images/topup");
                            URL = BaseURL+ "/topup";
                            break;
                    }

                    string ImageName = Guid.NewGuid().ToString()+Path.GetExtension(Images.FileName);
                    string FullPath = Path.Combine(UploadPath, ImageName);
                    using (var fileStream = new FileStream(FullPath, FileMode.Create))
                    {
                        Images.CopyTo(fileStream);
                    }

                    return URL + "/" + ImageName;
                }
                return null;
            }
            catch (Exception ex)
            {
                var new_error = new LogErrorSystem
                {
                    Title = "Save Image",
                    Details = ex.Message + "/"+ ex.StackTrace,
                    Context = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                _db.LogErrorSystem.Add(new_error);
                _db.SaveChanges();
                return null;
            }
        }
    }
}
