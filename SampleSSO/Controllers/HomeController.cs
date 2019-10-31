using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SampleSSO.Models;

namespace SampleSSO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            //// call api
            var client2 = new HttpClient();
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
            //client2.SetBearerToken(tokenResponse.Result.AccessToken);

            //Consume API with the auth token
            var response = client2.GetAsync("http://localhost:51005/api/values");

            while (!response.IsCompleted)
            {

            }

            if (response.IsFaulted)
            {
                Console.WriteLine(response.Result);
            }
            else
            {
                var content = response.Result.Content.ReadAsStringAsync();
                while (!content.IsCompleted)
                {

                }

                Console.WriteLine(JArray.Parse(content.Result));
            }


           


            

            
           
            var hasClaim = User.HasClaim(c => c.Type == "role");
            return View();

        }        

        public async Task Logout()
        {
            //await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            await HttpContext.SignOutAsync();
        }
    }
}
