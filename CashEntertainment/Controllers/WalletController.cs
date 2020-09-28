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
    public class WalletController : ControllerBase
    {


        public readonly IRepo_Wallet _wallet_service;
        private readonly IHttpContextAccessor _httpContextAccessor;
    
        public WalletController(IRepo_Wallet wallet_service, IHttpContextAccessor httpContextAccessor)
        {
            _wallet_service = wallet_service;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("RetrieveUserWallet")]
        [HttpGet]
        public IActionResult RetrieveUserWallet()
        {

            var result = _wallet_service.GetUserWallet(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name));
            if (result.Item2 != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result.Item2));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }

        }


        [Route("GetBalanceGameCredit")]
        [HttpGet]
        public async Task<IActionResult> GetBalanceGameCredit()
        {

            decimal result = await _wallet_service.GetBalanceGameCredit(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name));
            if (result == -1)
            {

                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
        }



        [Route("TransferCredit")]
        [HttpPost]
        public IActionResult TransferCredit(Models_Player_Transfer_Credit_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            var result = _wallet_service.TransferWalletCredit(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.RefCode, model.TransferAmount);
            if (result == Models_General.SUCC_TRANSFER_CREDIT)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }



        [Route("RetrieveUserTrackingWalletListing")]
        [HttpGet]
        public IActionResult RetrieveUserTrackingWalletListing(long? MemberSrno)
        {

            // Using ModelState to help validate the input from the end user 
            if (MemberSrno != null || MemberSrno == 0)
            {
                var result = _wallet_service.RetrieveTrackingWalletList().Where(x => x.MemberSrno == MemberSrno);
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