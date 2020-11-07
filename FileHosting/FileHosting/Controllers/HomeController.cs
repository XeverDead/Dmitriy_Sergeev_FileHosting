using DAL.Models;
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
using DAL.DataProviders;
using DAL;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbDataProvider _dataProvider;

        public HomeController()
        {
            _dataProvider = new SqlServerDataProvider();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(GetUsers());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            _dataProvider.ExecuteNonQuery(SqlQueries.InsertUser, DAL.Enums.Tables.Users, user);

            return RedirectToAction("index");
        }

        [NonAction]
        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>();

            foreach (User user in _dataProvider.ExecuteQuery(SqlQueries.GetAllUsers, DAL.Enums.Tables.Users))
            {
                users.Add(user);
            }

            return users;
        }
    }
}
