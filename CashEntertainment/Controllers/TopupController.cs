using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _998Intergration;
using CashEntertainment.DataAccess;
using CashEntertainment.Helper;
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using static CashEntertainment.Models.Models_Response;
using static CashEntertainment.Models.Models_Request;

namespace CashEntertainment.Controllers
{

    [Authorize(Policy = "MemberPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TopupController : ControllerBase
    {

        public readonly IRepo_Topup _topup_service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TopupController(IRepo_Topup topup_service, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor, IWebHostEnvironment hostEnvironment)
        {
            _topup_service = topup_service;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            _webHostEnvironment = hostEnvironment;
        }

        [Route("RequestTopupWithOnlineBanking")]
        [HttpPost]
        public IActionResult UserRequestTopupWithOnlineBanking([FromForm] Models_Topup_Request_Online_Banking model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));


            }
            var base_url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value.ToString()}{_httpContextAccessor.HttpContext.Request.PathBase.Value.ToString()}";

            var result = _topup_service.MemberRequestTopupWithOnlineBanking(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.BankSrno, model.TopupAmount, model.TopupImageProof, model.TransactionReferenceNumber, _webHostEnvironment.WebRootPath, base_url, model.Currency);

            if (result == Models_General.SUCC_CREATE_REQUEST_TOPUP)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("RequestTopupWithCrypto")]
        [HttpPost]
        public IActionResult UserRequestTopupWithCrypto([FromForm] Models_Topup_Request_Crypto model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _topup_service.MemberRequestTopupWithCrypto(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.TopupAmount, model.Rate, model.Currency, model.TransactionHash);

            if (result == Models_General.SUCC_CREATE_REQUEST_TOPUP)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }



        }

        [Route("TopUpGameCredit")]
        [HttpPost]
        public async Task<IActionResult> TopUpGameCredit(Models_Transfer_Credit_Request model)
        {

            if (model.TransferAmount > 0)

            {
                var result = await _topup_service.TopUpGameCredit(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.TransferAmount);
                if (result == Models_General.SUCC_TOPUP_GAME_CREDIT)
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