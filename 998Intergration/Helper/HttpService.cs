using _998Intergration.Models;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static _998Intergration.Models.Models_Reponse;

namespace _998Intergration.Helper
{
    public class HttpService
    {
        /*
         * 
         * Create Signature key from body text and agent's private key
         *
         */
        public static string CreateSignature(string PartnerName, int TimeStamp, string SignatureKey)
        {
            var word = PartnerName.ToLower() + TimeStamp + SignatureKey.ToLower();
            using (var crypt = new SHA256Managed())
            {
                var hash = new StringBuilder();
                var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(word), 0, Encoding.UTF8.GetByteCount(word));

                foreach (var _Byte in crypto)
                {
                    hash.Append(_Byte.ToString("x2"));
                }

                return hash.ToString();
            }
        }


        //HTTP POST CALLER
     

        public static async Task<string> POST(string apiPath, string PostContent)
        {
            

            var data = new StringContent(PostContent, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(apiPath, data);

            return response.Content.ReadAsStringAsync().Result;
            
        }

        public static async Task<string> GET_GAME(string apiPath, string token, string vendorName)
        {
            //Adding Parameters
            UriBuilder builder = new UriBuilder("http://localhost:6598/api/get");
            builder.Query = "Vendor='"+ vendorName + "'";

            HttpClient client = new HttpClient();
            //Adding bearer token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await client.GetAsync(builder.Uri);

            return result.Content.ReadAsStringAsync().Result;
        }

        public static async Task<string> GAME_POST(string apiPath, string postcontent, string token)
        {

            var data = new StringContent(postcontent, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            //Adding bearer token
            client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Referer", "http://119.81.200.187/sport");

            var response = await client.PostAsync(apiPath, data);

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
