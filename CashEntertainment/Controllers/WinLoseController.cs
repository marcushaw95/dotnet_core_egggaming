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
    public class WinLoseController : ControllerBase
    {
        public readonly IRepo_Winlose _winlose_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public WinLoseController(IRepo_Winlose winlose_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor)
        {
            _winlose_service = winlose_service;
            _settings = settings;
            _jwt_services = jwt_services;
            _httpContextAccessor = httpContextAccessor;

        }

        [Route("RetrieveUserWinLoseListing")]
        [HttpGet]
        public IActionResult RetrieveUserWinLoseListing(long? MemberSrno)
        {

            // Using ModelState to help validate the input from the end user 
            if (MemberSrno != null || MemberSrno == 0)
            {
                var result = _winlose_service.RetrieveWinloseList().Where(x => x.MemberSrno == MemberSrno);
                if (result != null)
                {
                    return Ok(new Resp(HttpStatusCode.OK, 0, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
                }
            }

            return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
        }


 
    }
}

