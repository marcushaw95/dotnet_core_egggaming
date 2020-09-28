using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashEntertainment.DataAccess;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static CashEntertainment.Models.Models_Admin;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{
    [Authorize(Policy = "TwelvePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TwelveController : ControllerBase
    {
        public readonly IRepo_Twelve _twelve_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        public TwelveController(IRepo_Twelve twelve_service, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor, IOptions<Models_AppSettings> settings, JWT jwt_services)
        {
            _twelve_service = twelve_service;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            _settings = settings;
            _jwt_services = jwt_services;
        }
        [Route("RegisterAccount")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccount(Models_Twelve_Register_Request model)
        {
            // Using ModelState to help validate the input from the end user 

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = await _twelve_service.UserRegisterNewAccount(model.LoginId,  model.Password, model.CountryCode, model.Upline);
            if (result.Item1 == Models_General.SUCC_CREATE_ACCOUNT )
            {
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginId, "MEMBER", model.CountryCode, ip, 1, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else if(result.Item1 == Models_General.SUCC_CREATE_ACCOUNT_WITHOUT_GAME_ACCOUNT)
            {
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginId, "MEMBER", model.CountryCode, ip, 2, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }

        }


        [Route("ChangeUpline")]
        [HttpPost]
        public IActionResult ChangeUpline(Models_Twelve_Change_Upline_Request model)
        {
            // Using ModelState to help validate the input from the end user 


            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _twelve_service.UserChangeUpline(model.LoginId, model.Upline);
            if (result == Models_General.SUCC_CHANGE_UPLINE)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }

        }




        [Route("TopupCredit")]
        [HttpPost]
        public IActionResult TopupCredit(Models_Twelve_Topup model)
        {
            // Using ModelState to help validate the input from the end user 

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _twelve_service.TopupCredit(model.LoginId, model.TopupAmount, model.IsReset);
            if (result == Models_General.SUCC_TOPUP)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }

   
        [Route("GetTwelveTurnoverRemainingAmount")]
        [HttpPost]
        public IActionResult GetTwelveTurnoverRemainingAmount(Models_Twelve_Turnover_Request model)
        {
            // Using ModelState to help validate the input from the end user 

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _twelve_service.GetTwelveTurnoverRemainingAmount(model.LoginIds);
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