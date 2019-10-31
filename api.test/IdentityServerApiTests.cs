using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace api.test
{
    [TestClass]
    public class IdentityServerApiTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task Get_User_Claims()
        {
            var client = new HttpClient();
            //Discovery of API
            var disco = await client.GetDiscoveryDocumentAsync("https://identitydev-test.usaid.gov");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }



            //Obtain API auth token by passing in client Secret
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "identityapi-client",
                ClientSecret = "s8YNtwUyl7%%*qco$!V",
                Scope = "identityapi",
                GrantType = "hybrid"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            

            // call api
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            //Consume API with the auth token
            var response = await client2.GetAsync("https://identitydev-test.usaid.gov/api/users/bykim@usaid.gov");
            //var response = await client2.GetAsync("https://localhost:44345/users/bykim@usaid.gov");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

        }


        [TestMethod]
        public async System.Threading.Tasks.Task Add_And_Remove_User_Claims()
        {
            var client = new HttpClient();
            
            var disco = await client.GetDiscoveryDocumentAsync("https://identitydev-test.usaid.gov");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "identityapi-client",
                ClientSecret = "s8YNtwUyl7%%*qco$!V",
                Scope = "identityapi"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }


            // call api
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            string msg;

            var claims = new List<Claim>();
            claims.Add(new Claim("role", "test.administrator"));
            claims.Add(new Claim("role", "test.employee"));

            var user = new User
            {
                Claims = claims,
                Email = "bykim22@usaid.gov",
                ProviderType = ProviderType.ADFS
            };

            var userJson = JObject.FromObject(user).ToString();

            var content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"); 

            //var response = await client2.PostAsync("https://identitydev-test.usaid.gov/api/users/addclaims", content);
            var response = await client2.PostAsync("https://localhost:44345/users/addclaims", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(JArray.Parse(content2));
            }

            //response = await client2.GetAsync("https://identitydev-test.usaid.gov/api/users/bykim@usaid.gov");
            response = await client2.GetAsync("https://localhost:44345/users/bykim@usaid.gov");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(JArray.Parse(content2));
            }

            //var response = await client2.PostAsync("https://identitydev-test.usaid.gov/api/users/addclaims", content);
            response = await client2.PostAsync("https://localhost:44345/users/removeclaims", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(JArray.Parse(content2));
            }

        }

        [TestMethod]
        public async System.Threading.Tasks.Task Get_Users_Online()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://identitydev-test.usaid.gov");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "identityapi-client",
                ClientSecret = "s8YNtwUyl7%%*qco$!V",
                Scope = "identityapi"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }


            // call api
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            string msg;
            
            var response = await client2.GetAsync("https://localhost:44345/users/online");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content2));
            }

        }

        [TestMethod]
        public async System.Threading.Tasks.Task Enable_Disable_User()
        {
            var client = new HttpClient();
            //Discovery of API
            var disco = await client.GetDiscoveryDocumentAsync("https://identitydev-test.usaid.gov");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
                       
            //Obtain API auth token by passing in client Secret
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "identityapi-client",
                ClientSecret = "s8YNtwUyl7%%*qco$!V",
                Scope = "identityapi",
                GrantType = "client_credentials"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }


            // call api
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            var user = new User
            {
                Email = "bykim@usaid.gov",
                ProviderType = ProviderType.ADFS
            };

            var userJson = JObject.FromObject(user).ToString();

            var content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json");

            //Consume API with the auth token            
            var response = await client2.PutAsync("https://localhost:44345/users/enableuser/aaplan.web", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content2 = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content2));
            }

        }

        [TestMethod]
        public async System.Threading.Tasks.Task Get_Value_Okta_Api()
        {
            var client = new HttpClient();
       


            //Obtain API auth token by passing in client Secret
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://usaid-eval.okta.com/oauth2/aus1hojtgdwFd1g7T0h8/v1/token",

                ClientId = "0oa1hojw7b3X3Jioc0h8",
                ClientSecret = "Mib8Ver6qmpugjC3pbqilyojsWzFpPGy03qHu_wN",
                Scope = "openid,profile,email,address,phone,offline_access",
                GrantType = "client_credentials"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }


            // call api
            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);

            //Consume API with the auth token
            var response = await client2.GetAsync("https://identitydev-test.usaid.gov/api/users/bykim@usaid.gov");
            //var response = await client2.GetAsync("https://localhost:44345/users/bykim@usaid.gov");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

        }
    }
}
