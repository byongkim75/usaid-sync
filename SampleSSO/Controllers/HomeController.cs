using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSSO.Models;

namespace SampleSSO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var hasClaim = User.HasClaim(c => c.Type == "role");
            return View();
        }        

        public async Task Logout()
        {            
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

        }
    }
}
