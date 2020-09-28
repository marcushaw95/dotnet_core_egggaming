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
    public class UserController : ControllerBase
    {

        public readonly IRepo_User _user_service;
        public readonly IOptions<Models_AppSettings> _settings;
        public readonly JWT _jwt_services;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _accessor;
        public UserController(IRepo_User user_service, IOptions<Models_AppSettings> settings, JWT jwt_services, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor)
        {
            _user_service = user_service;
            _settings = settings;
            _jwt_services = jwt_services;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(Models_Login_Request model)
        {

            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }



            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = _user_service.AuthrorizeAccount(model.LoginID, model.Password, model.LoginCountry, ip);
            if (result.Item1 == Models_General.SUCC_AUTHORIZE_GRANTED)
            {
                var secrect = _settings.Value.JWTSecret;
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginID, "MEMBER" ,model.LoginCountry, ip, result.Item3, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else
            {

                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }

        }

        [AllowAnonymous]
        [Route("RegisterAccount")]
        [HttpPost] 
        public async Task<IActionResult> RegisterAccount(Models_Register_Request model)
        {
            // Using ModelState to help validate the input from the end user 

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            
            var result = await _user_service.UserRegisterNewAccount(model.LoginId, model.FullName, model.Password, model.Email, model.Phonenumber, model.CountryCode, model.DOB, model.Gender, model.Upline);
            if (result.Item1 == Models_General.SUCC_CREATE_ACCOUNT)
            {
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginId, "MEMBER" , model.CountryCode, ip,1, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else if (result.Item1 == Models_General.SUCC_CREATE_ACCOUNT_WITHOUT_GAME_ACCOUNT)
            {
                var token = _jwt_services.GenerateJWTToken(result.Item2, model.LoginId, "MEMBER", model.CountryCode, ip, 2, _settings.Value.JWTSecret, _settings.Value.Issuer, _settings.Value.Audience);
                return Ok(new Resp(HttpStatusCode.OK, result.Item1, token));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
            }

        }
       
        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(Models_ChangePassword_Request model)
        {
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }

            
            var result = _user_service.UserUpdatePassword(model.NewPassword, model.CurrentPassword, Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name));
            if (result == Models_General.SUCC_UPDATE_PASSWORD)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }

        [Route("UpdateUserProfile")]
        [HttpPost]
        public IActionResult UpdateUserProfile(Models_UpdateProfile_Request model)
        { 
            // Using ModelState to help validate the input from the end user 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(new Resp(HttpStatusCode.BadRequest, 0, allErrors.Select(x => x.ErrorMessage)));
            }
            var result = _user_service.UserUpdateAccountDetails(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name), model.FullName, model.Email, model.Phonenumber, model.DOB , model.Gender);
            if (result== Models_General.SUCC_UPDATE_PROFILE)
            {
      
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }
        }

        [Route("RetrieveUserProfile")]
        [HttpGet]
        public IActionResult RetrieveUserProfile()
        {
             
                var result = _user_service.GetUserProfile(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name));
                if (result.Item2 != null)
                {

                    return Ok(new Resp(HttpStatusCode.OK, 0, result.Item2));
                }
                else
                {
                    return Ok(new Resp(HttpStatusCode.BadRequest, result.Item1));
                }
            
        }


        //[Route("Dummy")]
        //[HttpGet]
        //public IActionResult CheckUser()
        //{
        //    return Ok(_httpContextAccessor.HttpContext.User.Identity.Name);
        //}
        //[AllowAnonymous]
        //[Route("gamelogin")]
        //[HttpGet]
        //public async Task<IActionResult> gamelogin()
        //{
        //    var result =await  Intergration.RetrievePlayerToken("marcus123");
        //    return Ok(result.Error);
        //}

    }
}

