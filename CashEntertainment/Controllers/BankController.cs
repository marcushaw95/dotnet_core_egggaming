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
    public class BankController : ControllerBase
    {
        public readonly IRepo_Bank _bank_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;
  

        public BankController(IRepo_Bank bank_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor)
        {
            _bank_service = bank_service;
            _settings = settings;
            _jwt_services = jwt_services;
            _httpContextAccessor = httpContextAccessor;
       
    
        }

        [Route("MemberAddNewBank")]
        [HttpPost]
        public IActionResult MemberAddNewBank(Modesl_Member_Add_New_Bank_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _bank_service.MemberAddNewBank(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.BankSrno, model.BankAccountHolderName, model.BankCardNo);
            if (result == Models_General.SUCC_MEMBER_ADD_BANK)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("MemberUpdateBank")]
        [HttpPost]
        public IActionResult MemberUpdateBank(Modesl_Member_Update_Bank_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _bank_service.MemberUpdateBank(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.UserBankSrno,  model.BankSrno, model.BankAccountHolderName, model.BankCardNo);
            if (result == Models_General.SUCC_MEMBER_UPDATE_BANK)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


    
        [Route("RetrieveAdminBankList")]
        [HttpGet]
        public IActionResult RetrieveAdminBankList(string CountryCode)
        {

            var result = _bank_service.RetrieveAdminBankList();
            // Using ModelState to help validate the input from the end user 
            if (!string.IsNullOrEmpty(CountryCode))
            {
                result = _bank_service.RetrieveAdminBankList().Where(x => x.Country == CountryCode).ToList();
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


        [Route("RetrieveMemberBankList")]
        [HttpGet]
        public IActionResult RetrieveMemberBankList(long? MemberSrno)
        {
            // Using ModelState to help validate the input from the end user 
            if (MemberSrno != null || MemberSrno == 0)
            {
                var result = _bank_service.RetrieveUserBankList().Where(x=>x.MemberSrno == MemberSrno);
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

        [AllowAnonymous]
        [Route("RetrieveBankList")]
        [HttpGet]
        public IActionResult RetrieveBankList(string CountryCode)
        {

            var result = _bank_service.RetrieveBankList();
            // Using ModelState to help validate the input from the end user 
            if (!string.IsNullOrEmpty(CountryCode))
            {
                 result = _bank_service.RetrieveBankList().Where(x => x.Country == CountryCode).ToList();
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

        [AllowAnonymous]
        [Route("RetrievePaylah88BankList")]
        [HttpGet]
        public IActionResult RetrievePaylah88BankList(string CountryCode)
        {

            var result = _bank_service.RetrievePaylah88BankList();
            // Using ModelState to help validate the input from the end user 
            if (!string.IsNullOrEmpty(CountryCode))
            {
                result = _bank_service.RetrievePaylah88BankList().Where(x => x.Country == CountryCode).ToList();
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

