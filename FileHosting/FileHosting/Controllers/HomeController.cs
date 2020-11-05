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

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private IDataProvider _dataProvider;

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
            _dataProvider.SetUser(user, true);

            return RedirectToAction("index");
        }

        [NonAction]
        private IEnumerable<User> GetUsers()
        {
            return _dataProvider.GetUsers();
        }
    }
}
