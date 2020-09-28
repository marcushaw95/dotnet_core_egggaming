using System;
using System.Collections.Generic;
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
    public class GameController : ControllerBase
    {
        public readonly IRepo_Game _game_service;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        //This is for accessing server map path (server path to store image)
        private readonly IWebHostEnvironment webHostEnvironment;


        public GameController(IRepo_Game game_service, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor, IWebHostEnvironment hostEnvironment)
        {
            _game_service = game_service;

            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
            webHostEnvironment = hostEnvironment;
        }

        [Route("PlayGame")]
        [HttpPost]
        public async Task<IActionResult> PlayGame([FromForm] Models_Play_Game_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            //Retrieve Current Domain Path
            var user_language = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "Country").FirstOrDefault().Value == "MY"? "en-us": "zh-cn";
            var result = await _game_service.LoginGame(model.VendorCode, Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.browserType, user_language, model.GameCode);
            if (result.Item1 == Models_General.SUCC_OPEN_GAME)
            {

                return Ok(new Resp(HttpStatusCode.OK, result.Item1, result.Item2));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }
        }
        [AllowAnonymous]
        [Route("GameListing")]
        [HttpGet]
        public IActionResult GameListing()
        {

            var result =  _game_service.RetrieveGameListing();
            if (result !=null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }



        [AllowAnonymous]
        [Route("SubGameListing")]
        [HttpGet]
        public IActionResult SubGameListing()
        {

            var result = _game_service.RetrieveSubGameListing();
            if (result != null)
            {

                return Ok(new Resp(HttpStatusCode.OK, 0, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, Models_General.ERR_SERVER_BUSY_INTERNAL_ERROR));
            }
        }



        [AllowAnonymous]
        [Route("GetTicketsByFetch")]
        [HttpGet]
        public async Task<IActionResult> TicketsByFetch()
        {

            var result = await _game_service.GetTicketsByFetch();
            if (result == Models_General.SUCC_GET_TICKETS)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [AllowAnonymous]
        [Route("UpdateGameWalletAmounts")]
        [HttpGet]
        public async Task<IActionResult> UpdateGameWalletAmounts()
        {
            var result = await _game_service.UpdateGameWalletAmounts();
            if (result == Models_General.SUCC_GET_ALL_GAMER_WALLET_BALANCE)
            {

                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }


        //[AllowAnonymous]
        //[Route("RecollectGameWalletAmounts")]
        //[HttpGet]
        //public async Task<IActionResult> RecollectGameWalletAmounts()
        //{
        //    var result = await _game_service.RecollectGameWalletAmounts();
        //    if (result == Models_General.SUCC_RECOLLECT_ALL_GAMER_WALLET_BALANCE)
        //    {
        //        return Ok(new Resp(HttpStatusCode.OK, result));
        //    }
        //    else
        //    {
        //        return Ok(new Resp(HttpStatusCode.BadRequest, result));
        //    }
        //}


        [AllowAnonymous]
        [Route("CheckUserGameRegister")]
        [HttpGet]
        public async Task<IActionResult> CheckUserGameRegister()
        {
            var result = await _game_service.CheckUserGameRegister();
            if (result.Item1 == Models_General.SUCC_CHECK_USER_GAME_REGISTER)
            {
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, result.Item2));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }
        }

    }
}