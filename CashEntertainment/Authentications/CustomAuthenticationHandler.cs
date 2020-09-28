using CashEntertainment.DataAccess;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CashEntertainment.Authentications
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public readonly IRepo_User _user_service;
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRepo_User user_service
            )
            : base(options, logger, encoder, clock)
        {
            _user_service = user_service;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
           
            try
            {

                if (!Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail("Unauthorized");
                string authorizationHeader = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return AuthenticateResult.NoResult();
                }

                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                var user = await Task.Run(() => _user_service.Authenticate(username, password));

                if (user == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
                 

                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.User),
                    new Claim(ClaimTypes.Role, "TWELVE"),
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);

            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

    }
}
