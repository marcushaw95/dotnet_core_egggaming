using CashEntertainment.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CashEntertainment.Helper
{
    public class JWT
    {
       public string GenerateJWTToken(long MemberSrno, string LoginID, string Role, string Country,  string Ip, int GameRegister, string SignKey, string Issuer, string Audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SignKey);
            var claimsDictionary = new Dictionary<string, object>();
            var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, MemberSrno.ToString()),
                    new Claim("LoginId",LoginID),
                    new Claim(ClaimTypes.Role, Role),
                    new Claim("Country", Country),
                    new Claim("Ip", Ip),
                    new Claim("GameRegister", GameRegister.ToString()),

                };
            foreach (var claim in claims)
            {
                claimsDictionary.Add(claim.Type, claim.Value);

            }

            //Update At 20200331 Added Issuer and Audience If not It Will Not Work
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddHours(3),
                Subject = new ClaimsIdentity(claims),
                Claims = claimsDictionary,
                Issuer = Issuer,
                Audience = Audience,
                NotBefore = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
