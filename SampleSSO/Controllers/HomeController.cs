using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
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

            
            //var hasClaim = User.HasClaim(c => c.Type == "role");


            var url = HttpContext.Features.Get<IHttpRequestFeature>().RawTarget;

            foreach(var header in HttpContext.Response.Headers)
            {

            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> GetValues(string accessToken)
        {
            // call api
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
            client.SetBearerToken(accessToken);

            //Consume API with the auth token
            var response = await client.GetAsync("http://localhost:51005/api/values");

            var content = await response.Content.ReadAsStringAsync();

            ViewBag.Values = content;
            
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> PostValue(string accessToken2, string myValue)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken2);
            client.SetBearerToken(accessToken2);

            var model = new ValueModel
            {
                Id = 1,
                Value = myValue
            };

            var json = JObject.FromObject(model).ToString();

            //Consume API with the auth token
            var data = new StringContent(json);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("http://localhost:51005/api/values", data);

            var content = await response.Content.ReadAsStringAsync();

            ViewBag.Message = content;

            return View("Index");
        }

        public async void GetToken(int index)
        {
            //var client = new HttpClient();


            

            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            ////parameters.Add("client_id", "0oa1hn4cac4BZMxnx0h8");
            ////parameters.Add("redirect_uri", "http://localhost:51338/Home/");
            //parameters.Add("response_type", "code token");
            //parameters.Add("response_method", "fragment");
            //parameters.Add("scope", "openid allow_post");
            //parameters.Add("state", Guid.NewGuid().ToString());
            //parameters.Add("nonce", Guid.NewGuid().ToString());
            //parameters.Add("prompt", "none");
            ////parameters.Add("code", "code");

            //var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            //{
            //    Address = "https://usaid-eval.okta.com/oauth2/default/v1/authorize",
            //    ClientId = "0oa1hn4cac4BZMxnx0h8",
            //    Parameters = parameters,
            //    RedirectUri = "http://localhost:51338/Home/",
            //    Code = "dfasdf"
            //});

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //}




            var client = new HttpClient();

            var state = Guid.NewGuid().ToString();
            var nonce = Guid.NewGuid().ToString();
            var scope = "openid%20allow_post%20allow_get";

            foreach(var claim in HttpContext.User.Claims)
            {
                if (claim.Value == "kim.byong@miraclesystems.net")
                    scope = "openid%20allow_get";
            }

            //var url = string.Format("https://usaid-eval.okta.com/oauth2/default/v1/authorize?client_id=0oa1hn4cac4BZMxnx0h8&redirect_uri=http://localhost:51338/Home/&response_type=code%20token&response_mode=fragment&scope={0}&state={1}&nonce={2}&prompt=none", scope, state, nonce);
            var url = string.Format("https://usaid-eval.okta.com/oauth2/default/v1/authorize?client_id=0oa1hn4cac4BZMxnx0h8&redirect_uri=https://sso-poc-sample-clientapp.usaid-devapps-east.p.azurewebsites.net/Home/&response_type=code%20token&response_mode=fragment&scope={0}&state={1}&nonce={2}&prompt=none", scope, state, nonce);
            //var url = string.Format("https://usaid-eval.okta.com/oauth2/default/v1/authorize?client_id=0oa1hn4cac4BZMxnx0h8&redirect_uri=http://localhost:51338/Home/&response_type=code%20token&response_mode=form_post&scope={0}&state={1}&nonce={2}&prompt=none", scope, state, nonce);

            var client2 = new HttpClient();
            var response = await client2.GetAsync(url);
            
            //HttpContext.Response.Redirect(url);

            //string msg;
            //var response = WebRequest("GET", url, "", out msg);


        }

        public async Task Logout()
        {
            //await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            await HttpContext.SignOutAsync();
        }


        private string WebRequest(string method, string url, string postData, out string msg)
        {
            HttpWebRequest webRequest = null;
            string responseData = "";
                        
            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.AllowAutoRedirect = false;


            webRequest.Method = method.ToString();
            Stream dataStream = null;
            if (!string.IsNullOrEmpty(postData))
            {
                webRequest.ContentType = "application/json";
                Encoding encoding = Encoding.UTF8;
                byte[] docAsBytes = encoding.GetBytes(postData);

                webRequest.ContentLength = docAsBytes.Length;

                dataStream = webRequest.GetRequestStream();
                dataStream.Write(docAsBytes, 0, docAsBytes.Length);
            }

            responseData = WebResponseGet(webRequest, out msg);
            if (dataStream != null)
                dataStream.Close();

            webRequest = null;

            return responseData;
        }

        private string WebResponseGet(HttpWebRequest webRequest, out string msg)
        {
            msg = "";
            StreamReader responseReader = null;
            string responseData = "";


            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            try
            {
                
                //if (response.StatusCode != HttpStatusCode.OK)
                //    throw new Exception(response.StatusDescription);
                responseReader = new StreamReader(response.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                msg = ex.ToString();
            }
            finally
            {
                try
                {
                    webRequest.GetResponse().GetResponseStream().Close();
                }
                catch { }
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader = null;
                }
            }

            return responseData;
        }
    }
}
