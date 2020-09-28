using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashEntertainment.DataAccess;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static CashEntertainment.Models.Models_Admin;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{

    [Authorize(Policy = "MemberPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {

        public readonly IRepo_Withdraw _withdraw_service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        public WithdrawController(IRepo_Withdraw withdraw_service, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor)
        {
            _withdraw_service = withdraw_service;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
        }

        [Route("RequestWithdrawalOnlineBanking")]
        [HttpPost]
        public IActionResult RequestWithdrawalOnlineBanking(Models_Withdrawal_Request_Online_Banking model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _withdraw_service.MemberRequestWithdrawalOnlineBanking(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.WihtdrawalAmount, model.BankSrno, model.Currency);
            if (result == Models_General.SUCC_CREATE_REQUEST_WITHDRAWAL)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }


        [Route("RequestWithdrawalCrypto")]
        [HttpPost]
        public IActionResult RequestWithdrawalCrypto(Models_Withdrawal_Request_Crypto model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _withdraw_service.MemberRequestWithdrawalCrypto(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.WihtdrawalAmount, model.ToAddress, model.Rate, model.Currency);
            if (result == Models_General.SUCC_CREATE_REQUEST_WITHDRAWAL)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }







        [Route("WithdrawGameCredit")]
        [HttpPost]
        public async Task<IActionResult> WithdrawGameCredit(Models_Withdrawal_Credit_Request model)
        {

            if (model.WithdrawalAmount > 0)

            {
                var result = await _withdraw_service.WithdrawGameCredit(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.WithdrawalAmount);
                if (result == Models_General.SUCC_WITHDRAWAL_GAME_CREDIT)
                {

                    return Ok(new Resp(HttpStatusCode.OK, result));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, result));
                }
            }

            return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_AMOUNT_CANNOT_BE_ZERO_NULL));
        }

    }
}