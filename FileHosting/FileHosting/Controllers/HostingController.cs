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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BL;
using Web.ViewModels;
using Web.Enums;

namespace Web.Controllers
{
    [Authorize]
    public class HostingController : Controller
    {
        private readonly IHostingCore _hostingCore;
        private readonly IWebHostEnvironment _appEnvironment;

        public HostingController(IHostingCore hostingCore, IWebHostEnvironment appEnvironment)
        {
            _hostingCore = hostingCore;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_hostingCore.GetAllUsers());
        }

        [HttpGet]
        public IActionResult UserPage(long userId)
        {         
            var user = _hostingCore.GetUserById(userId);

            if (user == null)
            {
                return Content("There is no user with this id");
            }

            var userFiles = new UserFilesModel
            {
                User = user,
                Files = _hostingCore.GetUserFiles(userId)
            };

            return View(userFiles);
        }

        [HttpGet]
        public IActionResult FilePage(long fileId)
        {
            var file = _hostingCore.GetFileById(fileId);

            if (file == null)
            {
                return Content("There is no file with this id");
            }

            var userFileInfo = new UserFileModel
            {
                File = file,
                User = _hostingCore.GetUserById(file.AuthorId)
            };

            return View(userFileInfo);
        }

        [HttpGet]
        public IActionResult UsersSearch(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return View();
            }

            return View(_hostingCore.GetUsersByLogin(login));
        }

        [HttpGet]
        public IActionResult FilesSearch(FilesSearchTypes searchType, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View();
            }

            IEnumerable<UserFileModel> userFileCollection = null;

            switch(searchType)
            {
                case FilesSearchTypes.ByName:
                    userFileCollection = GetUserFileCollection(_hostingCore.GetFilesByName, searchQuery);
                    break;

                case FilesSearchTypes.ByExtension:
                    userFileCollection = GetUserFileCollection(_hostingCore.GetFilesByExtension, searchQuery);
                    break;

                case FilesSearchTypes.ByCategory:
                    userFileCollection = GetUserFileCollection(_hostingCore.GetFilesByCategory, searchQuery);
                    break;
            }

            return View(userFileCollection);
        }

        [HttpGet]
        public IActionResult ChangeUserInfo(long userId)
        {
            var user = _hostingCore.GetUserById(userId);

            var userInfo = new UserInfoModel
            {
                Id = user.Id,
                Login = user.Login,
                Role = user.Role
            };

            return View(userInfo);
        }

        [HttpPost]
        public IActionResult ChangeUserInfo(UserInfoModel userInfo)
        {
            var user = _hostingCore.GetUserById(userInfo.Id);

            user.Login = userInfo.Login;
            user.Role = userInfo.Role;

            _hostingCore.UpdateUser(user.Id, user);

            return RedirectToAction("UserPage", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ChangeFileInfo(long fileId)
        {
            var file = _hostingCore.GetFileById(fileId);

            var fileInfo = new FileInfoModel
            {
                Id = file.Id,
                Name = file.Name,
                Category = file.Category,
                Description = file.Description
            };

            return View(fileInfo);
        }

        [HttpPost]
        public IActionResult ChangeFileInfo(FileInfoModel fileInfo)
        {
            var file = _hostingCore.GetFileById(fileInfo.Id);

            file.Name = fileInfo.Name;
            file.Category = fileInfo.Category;
            file.Description = fileInfo.Description;

            _hostingCore.UpdateFile(file.Id, file);

            return RedirectToAction("FilePage", new { fileId = file.Id });
        }

        [HttpGet]
        public IActionResult DeleteFile(long fileId)
        {
            var file = _hostingCore.GetFileById(fileId);

            System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
            _hostingCore.DeleteFile(fileId);

            return RedirectToAction("UserPage", new { userId = file.AuthorId});
        }

        [HttpGet]
        public IActionResult DeleteUser(long userId)
        {
            var user = _hostingCore.GetUserById(userId);

            var files = _hostingCore.GetUserFiles(userId);

            foreach (var file in files)
            {
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
                _hostingCore.DeleteFile(file.Id);
            }

            _hostingCore.DeleteUser(user.Id);

            return RedirectToAction("Index");
        }

        [NonAction]
        private IEnumerable<UserFileModel> GetUserFileCollection(Func<string, IEnumerable<HostingFile>> filesSource, string searchQuery)
        {
            var userFileCollection = new List<UserFileModel>();

            foreach (var file in filesSource(searchQuery))
            {
                userFileCollection.Add(new UserFileModel
                {
                    File = file,
                    User = _hostingCore.GetUserById(file.AuthorId)
                });
            }

            return userFileCollection;
        }
    }
}
