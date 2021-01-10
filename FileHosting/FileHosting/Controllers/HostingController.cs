using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Enums;
using System.Security.Claims;
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
        [Authorize]
        public IActionResult UserPage(long userId)
        {
            User user;

            if (userId == 0)
            {
                user = _hostingCore.GetUserByEmail(User.Identity.Name);
            }
            else
            {
                user = _hostingCore.GetUserById(userId);
            }

            if (user == null)
            {
                return NotFound();
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
            HostingFile file = _hostingCore.GetFileById(fileId);

            if (file == null)
            {
                return NotFound();
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
        public IActionResult UsersSearch(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View();
            }

            var usersSearchModel = new UsersSearchModel
            {
                Users = _hostingCore.GetUsersByLogin(searchQuery),
                SearchQuery = searchQuery
            };

            return View(usersSearchModel);
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

            var filesSearchModel = new FilesSearchModel
            {
                UserFileCollection = userFileCollection,
                SearchQuery = searchQuery,
                SearchType = searchType
            };

            return View(filesSearchModel);
        }

        [HttpGet]
        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        public IActionResult ChangeUserInfo(long userId)
        {
            User user = _hostingCore.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

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
                User user = _hostingCore.GetUserById(userInfo.Id);

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
            HostingFile file = _hostingCore.GetFileById(fileId);

            if (file == null)
            {
                return NotFound();
            }

            User user = _hostingCore.GetUserById(file.AuthorId);

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
                HostingFile file = _hostingCore.GetFileById(fileInfo.Id);
                User user = _hostingCore.GetUserById(file.AuthorId);

                if (User.Identity.Name == user.Email ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()) ||
                    User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Editor.ToString()))
                {
                    IEnumerable<HostingFile> userFiles = _hostingCore.GetUserFiles(file.AuthorId);

                    var selectedFiles = from userFile in userFiles
                                        where userFile.Name == fileInfo.Name &&
                                        userFile.Id != fileInfo.Id
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

        [HttpPost]
        [Authorize]
        public IActionResult DeleteFile(long fileId)
        {
            HostingFile file = _hostingCore.GetFileById(fileId);

            User user = _hostingCore.GetUserById(file.AuthorId);

            if (User.Identity.Name == user.Email || 
                User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin.ToString()))
            {
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
                _hostingCore.DeleteFile(fileId);
            }

            return RedirectToAction("UserPage", new { userId = file.AuthorId});
        }

        [HttpPost]
        [RoleAuthorize(Roles.Admin)]
        public IActionResult DeleteUser(long userId)
        {
            User user = _hostingCore.GetUserById(userId);

            IEnumerable<HostingFile> files = _hostingCore.GetUserFiles(userId);

            foreach (var file in files)
            {
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, file.Link));
                _hostingCore.DeleteFile(file.Id);
            }

            _hostingCore.DeleteUser(user.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [RoleAuthorize(Roles.Editor, Roles.Admin)]
        public IActionResult UserList()
        {
            return View(_hostingCore.GetAllUsers());
        }

        [HttpGet]
        [RoleAuthorize(Roles.Editor, Roles.Admin)]
        public IActionResult FileList()
        {
            return View(_hostingCore.GetAllFiles());
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
