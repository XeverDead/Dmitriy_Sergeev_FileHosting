﻿using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Common.Enums;
using Web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BL;


namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHostingCore _hostingCore;

        public AccountController(IHostingCore hostingCore)
        {
            _hostingCore = hostingCore;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = _hostingCore.GetUserByEmail(loginModel.Email);

                if (user != null)
                {
                    if (loginModel.Password.Equals(user.Password))
                    {
                        Authenticate(user);

                        return RedirectToAction("Index", "Hosting");
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
        public IActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User user = _hostingCore.GetUserByEmail(registerModel.Email);

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

                    return RedirectToAction("Index", "Hosting");
                }

                ModelState.AddModelError("", "Entered email is already used");
            }

            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [NonAction]
        private void Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName)
            };

            var claimId = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
        }
    }
}
