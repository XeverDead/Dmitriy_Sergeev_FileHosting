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

namespace Web.Controllers
{
    [Authorize]
    public class HostingController : Controller
    {
        private readonly IHostingCore _hostingCore;
        private readonly IWebHostEnvironment _appEnvironemt;

        public HostingController(IHostingCore hostingCore, IWebHostEnvironment appEnviroment)
        {
            _hostingCore = hostingCore;
            _appEnvironemt = appEnviroment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_hostingCore.GetAllFiles());
        }

        [HttpGet]
        public IActionResult LoadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadFile(FileLoadModel loadModel)
        {
            var uploadedFile = loadModel.UploadedFile;

            var user = _hostingCore.GetUserByEmail(User.Identity.Name);

            if (user != null && uploadedFile != null)
            {
                if (uploadedFile.FileName.Length > 100)
                {
                    ModelState.AddModelError("", "Max file name length is 100 symbols");

                    return View(loadModel);
                }

                IEnumerable<HostingFile> userFiles = _hostingCore.GetUserFiles(user.Id);

                var selectedFiles = from userFile in userFiles
                                    where userFile.Name == uploadedFile.FileName
                                    select userFile;

                if (selectedFiles?.Count() > 0)
                {
                    ModelState.AddModelError("", "You already uploaded a file with this name");

                    return View(loadModel);
                }

                var path = _appEnvironemt.WebRootPath + $"/Files/{user.Id}/";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += uploadedFile.FileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var link = $"Files/{user.Id}/{uploadedFile.FileName}";

                var file = new HostingFile
                {
                    AuthorId = user.Id,
                    Name = uploadedFile.FileName,
                    Size = uploadedFile.Length,
                    Category = loadModel.Category,
                    Description = loadModel.Description,
                    Link = link
                };

                _hostingCore.InsertFile(file);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "An error occurred during loading");
            return View(loadModel);
        }
    }
}
