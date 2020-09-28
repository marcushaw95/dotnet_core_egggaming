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



    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        public readonly IRepo_Payment _payment_service;
        public readonly IOptions<Models_AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentGatewayController(IRepo_Payment payment_service, IHttpContextAccessor httpContextAccessor, IOptions<Models_AppSettings> settings)
        {
            _payment_service = payment_service;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
        }

        [Route("RequestDepositPaymentGateway")]
        [HttpPost]
        public IActionResult RequestDepositPaymentGateway(Models_PaymentRequest_Request model)
        {
            var result = _payment_service.RequestDepositPaymentGateway(Convert.ToInt64(_httpContextAccessor.HttpContext.User.Identity.Name),model.DepositUrl, model.Merchant, model.Currency, model.Customer, model.Reference, model.Key, model.Amount, model.Note, model.Datetime, model.FrontUrl, model.BackUrl, model.Language, model.Bank, model.ClientIp);
             if (result == Models_General.SUCC_CREATE_PAYMENT_REQUEST)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }


        [Route("FrontResponseDepositPaymentGateway")]
        [HttpPost]
        public ContentResult FrontResponseDepositPaymentGateway([FromForm]  Models_PaymentResponse_Request model)
        {
            string result = "error";
            string country_code = "";


            if (model.Status == "000")
            {
                result = "success";
            }

            country_code = model.Currency switch
            {
                "MYR" => "my",
                "CNY" => "cn",
                _ => "my",
            };

            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<div></div> <script>window.location.href='https://empiregaminggame.com/" + country_code+"/paymentresult?result="+result+"';</script>"
            };
        }


        [Route("BackResponseDepositPaymentGateway")]
        [HttpPost]
        public IActionResult BackResponseDepositPaymentGateway([FromForm]  Models_PaymentResponse_Request model)
        {
            var result = _payment_service.BackResponseDepositPaymentGateway(model.Merchant, model.Reference, model.Currency, model.Amount, model.Language, model.Customer, model.Datetime, model.Note, model.Key, model.Status, model.ID);

            if (result == Models_General.SUCC_PAYMENT)
            {
                return Ok(new Resp(HttpStatusCode.OK, result));
            }
            else
            {
                return Ok(new Resp(HttpStatusCode.BadRequest, result));
            }

        }


    }
}