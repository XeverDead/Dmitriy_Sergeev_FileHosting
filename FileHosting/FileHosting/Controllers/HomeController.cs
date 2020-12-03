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
using Common.SqlExpressions;
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
        public IActionResult Login(LoginModel loginData)
        {
            if (ModelState.IsValid)
            {
                var user = GetUserByEmail(loginData.Email);

                if (user != null)
                {
                    if (loginData.Password.Equals(user.Password))
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

            return View(loginData);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel registerData)
        {
            if (ModelState.IsValid)
            {
                var user = GetUserByEmail(registerData.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = registerData.Email,
                        Login = registerData.Login,
                        Password = registerData.Password,
                        Role = Roles.User
                    };

                    _hostingCore.ExecuteNonQuery(SqlStoredProcedures.InsertUser, Tables.Users, user, true);

                    Authenticate(user);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Entered email is already used");
            }

            return View(registerData);
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
            var users = new List<User>();

            foreach (User user in _hostingCore.ExecuteQuery(SqlQueries.GetAllUsers, Tables.Users, false))
            {
                users.Add(user);
            }

            return users;
        }

        [NonAction]
        private User GetUserByEmail(string email)
        {
            User user = null;

            List<IHostingEntity> users = _hostingCore.ExecuteQuery(SqlQueries.GetUserByEmail(email), Tables.Users, false);

            if (users?.Count > 0)
            {
                user =  (User)users[0];
            }

            return user;
        }
    }
}
