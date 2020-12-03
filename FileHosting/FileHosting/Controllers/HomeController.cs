using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Common.Enums;
using DAL.Enums;
using DAL.DbExpressions;
using Web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BL;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingCore _hostingCore;

        public HomeController(IHostingCore hostingCore)
        {
            _hostingCore = hostingCore;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View(GetUsers());
        }

        [HttpGet]
        [RoleAuthorize(Roles.Guest)]
        public IActionResult Secret()
        {
            return Content("Well hello there");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = GetUserByEmail(loginModel.Email);

                if (user != null)
                {
                    if (loginModel.Password.Equals(user.Password))
                    {
                        Authenticate(user);

                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", "Entered password is wrong");
                }
                else
                {
                    ModelState.AddModelError("", "There is no user with this email");
                }
            }

            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = GetUserByEmail(registerModel.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = registerModel.Email,
                        Login = registerModel.Login,
                        Password = registerModel.Password,
                        Role = Roles.User
                    };

                    _hostingCore.InsertUser(user);

                    Authenticate(user);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Entered email is already used");
            }

            return View(registerModel);
        }

        [NonAction]
        private void Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName)
            };

            ClaimsIdentity claimId = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
        }

        [NonAction]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [NonAction]
        private IEnumerable<User> GetUsers()
        {
            return _hostingCore.GetAllUsers();
        }

        [NonAction]
        private User GetUserByEmail(string email)
        {
            return _hostingCore.GetUserByEmail(email);
        }
    }
}
