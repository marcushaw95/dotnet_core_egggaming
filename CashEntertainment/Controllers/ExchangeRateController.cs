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
    public class ExchangeRateController : ControllerBase
    {
        public readonly IRepo_ExchangeRate _exchange_rate_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ExchangeRateController(IRepo_ExchangeRate exchange_rate_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor)
        {
            _exchange_rate_service = exchange_rate_service;
            _settings = settings;
            _jwt_services = jwt_services;
            _httpContextAccessor = httpContextAccessor;

        }

        [AllowAnonymous]
        [Route("RetrieveExchangeRateListing")]
        [HttpGet]
        public IActionResult RetrieveExchangeRateList(string Currency)
        {

            var result = _exchange_rate_service.RetrieveExchangeRateListing();
            // Using ModelState to help validate the input from the end user 
            if (!string.IsNullOrEmpty(Currency))
            {
                result = _exchange_rate_service.RetrieveExchangeRateListing().Where(x => x.Currency == Currency).ToList();
                if (result != null)
                {

                    return Ok(new Resp(HttpStatusCode.OK, 0, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
                }
            }
            else
            {
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
}

