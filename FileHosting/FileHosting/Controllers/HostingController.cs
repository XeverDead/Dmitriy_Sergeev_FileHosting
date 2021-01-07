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
            return View();
        }

        [HttpGet]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult UserPage(long userId)
        {
            User user;

            if (userId == -1)
            {
                user = _hostingCore.GetUserByEmail(User.Identity.Name);
            }
            else
            {
                user = _hostingCore.GetUserById(userId);
            }

            if (user == null)
            {
                return Content("There is no user with this id");
            }

            var userFiles = new UserFilesModel
            {
                User = user,
                Files = _hostingCore.GetUserFiles(user.Id)
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
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
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
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult ChangeUserInfo(long userId)
        {
            var user = _hostingCore.GetUserById(userId);

            if (User.Identity.Name == user.Email ||
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()) ||
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Editor.ToString()))
            {
                var userInfo = new UserInfoModel
                {
                    Id = user.Id,
                    Login = user.Login,
                    Role = user.Role
                };

                return View(userInfo);
            }

            return RedirectToAction("UserPage", new { userId = user.Id });
        }

        [HttpPost]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult ChangeUserInfo(UserInfoModel userInfo)
        {
            if (ModelState.IsValid)
            {
                var user = _hostingCore.GetUserById(userInfo.Id);

                if (User.Identity.Name == user.Email ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()) ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Editor.ToString()))
                {
                    user.Login = userInfo.Login;
                    user.Role = userInfo.Role;

                    _hostingCore.UpdateUser(user.Id, user);
                }

                return RedirectToAction("UserPage", new { userId = user.Id });
            }

            return View(userInfo);
        }

        [HttpGet]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult ChangeFileInfo(long fileId)
        {
            var file = _hostingCore.GetFileById(fileId);
            var user = _hostingCore.GetUserById(file.AuthorId);

            if (User.Identity.Name == user.Email ||
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()) ||
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Editor.ToString()))
            {
                var fileInfo = new FileInfoModel
                {
                    Id = file.Id,
                    Name = file.Name,
                    Category = file.Category,
                    Description = file.Description
                };

                return View(fileInfo);
            }

            return RedirectToAction("FilePage", new { fileId = file.Id });
        }

        [HttpPost]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult ChangeFileInfo(FileInfoModel fileInfo)
        {
            if (ModelState.IsValid)
            {
                var file = _hostingCore.GetFileById(fileInfo.Id);
                var user = _hostingCore.GetUserById(file.AuthorId);

                if (User.Identity.Name == user.Email ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()) ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Editor.ToString()))
                {
                    var userFiles = _hostingCore.GetUserFiles(file.AuthorId);

                    var selectedFiles = from userFile in userFiles
                                        where userFile.Name == fileInfo.Name
                                        select userFile;

                    if (selectedFiles?.Count() > 0)
                    {
                        ModelState.AddModelError("", "You already have a file with this name");

                        return View(fileInfo);
                    }

                    file.Name = fileInfo.Name;
                    file.Category = fileInfo.Category;
                    file.Description = fileInfo.Description;

                    _hostingCore.UpdateFile(file.Id, file);
                }

                return RedirectToAction("FilePage", new { fileId = file.Id });
            }

            return View(fileInfo);
        }

        [HttpGet]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult DeleteFile(long fileId)
        {
            var file = _hostingCore.GetFileById(fileId);
            var user = _hostingCore.GetUserById(file.AuthorId);

            if (User.Identity.Name == user.Email || 
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()))
            {
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
                _hostingCore.DeleteFile(fileId);
            }

            return RedirectToAction("UserPage", new { userId = file.AuthorId});
        }

        [HttpGet]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult DeleteUser(long userId)
        {
            if (User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()))
            {
                var user = _hostingCore.GetUserById(userId);

                var files = _hostingCore.GetUserFiles(userId);

                foreach (var file in files)
                {
                    System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
                    _hostingCore.DeleteFile(file.Id);
                }

                _hostingCore.DeleteUser(user.Id);

            }

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
