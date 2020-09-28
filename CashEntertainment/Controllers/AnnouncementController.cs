using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashEntertainment.DataAccess;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{
    [Authorize(Policy = "MemberPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        public readonly IRepo_Announcement _announcement_service;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        //This is for accessing server map path (server path to store image)
        private readonly IWebHostEnvironment webHostEnvironment;


        public AnnouncementController(IRepo_Announcement announcement_service, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor, IWebHostEnvironment hostEnvironment)
        {
            _announcement_service = announcement_service;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            webHostEnvironment = hostEnvironment;
        }



        [AllowAnonymous]
        [Route("AnnoucementListing")]
        [HttpGet]
        public IActionResult AnnoucementListing()
        {
            var result = _announcement_service.RetrieveAnnouncementListing().Where(x => x.IsPublish == true).OrderByDescending(x => x.Srno).ToList();
            if (result != null)
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }


        [Route("CreateBanner")]
        [HttpPost]
        public IActionResult AdminCreateBanner([FromBody] Models_Create_Banner_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var base_url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value.ToString()}{_httpContextAccessor.HttpContext.Request.PathBase.Value.ToString()}";

            var result = _announcement_service.CreateNewBanner(_httpContextAccessor.HttpContext.User.Identity.Name, model.BannerImage, model.RedirectURL, model.IsActive, webHostEnvironment.WebRootPath, base_url);
            if (result == Models_General.SUCC_ADMIN_CREATE_BANNER)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("UpdateBanner")]
        [HttpPost]
        public IActionResult AdminUpdateBanner([FromBody] Models_Update_Banner_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var base_url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value.ToString()}{_httpContextAccessor.HttpContext.Request.PathBase.Value.ToString()}";

            var result = _announcement_service.UpdateBanner(model.BannerSrno, model.RedirectURL, model.IsActive, webHostEnvironment.WebRootPath, base_url, model.BannerImage);

            if (result == Models_General.SUCC_ADMIN_UPDATE_ANNOUNCEMENT)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("BannerListing")]
        [HttpPost]
        public IActionResult BannerListing()
        {
            var result = _announcement_service.RetrieveBannerListing();
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }

    }
}