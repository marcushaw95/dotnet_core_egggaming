using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CashEntertainment.DataAccess;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{
    [Authorize(Policy = "MemberPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        public readonly IRepo_Settings _settings_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SettingsController(IRepo_Settings settings_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor)
        {
            _settings_service = settings_service;
            _settings = settings;
            _jwt_services = jwt_services;
            _httpContextAccessor = httpContextAccessor;

        }

        [AllowAnonymous]
        [Route("RetrieveSettingsListing")]
        [HttpGet]
        public IActionResult RetrieveSettingsListing()
        {

            var result = _settings_service.RetrieveSettingsListing();
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

