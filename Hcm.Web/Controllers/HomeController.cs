using Hcm.Api.Client;
using Hcm.Database;
using Hcm.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenClient _tokenClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ITokenClient tokenClient,
            ILogger<HomeController> logger)
        {
            _tokenClient = tokenClient;
            _logger = logger;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated
                && User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Employee");
            }

            if (User.Identity.IsAuthenticated
               && User.IsInRole("Employee"))
            {
                 var employeeId = User.FindFirst("employeeId").Value;
                 return Redirect("Employee/Details/" + employeeId);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(
            [FromForm] LoginViewModel loginViewModel)
        {
            var response = await _tokenClient.PostAsync(new TokenRequestDto
            {
                Username = loginViewModel.Username,
                Password = loginViewModel.Password,
                Role = loginViewModel.Role,
            });

            HttpContext.Response.Cookies.Append("AuthCookie", response.Token, new Microsoft.AspNetCore.Http.CookieOptions
            {
                IsEssential = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                Secure = false,
                Expires = DateTime.Now.AddDays(23),
                HttpOnly = true,
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("AuthCookie");
            return RedirectToAction(nameof(Index));
        }
    }
}
