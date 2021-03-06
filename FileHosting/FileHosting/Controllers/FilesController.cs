﻿using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Enums;
using Web.ViewModels;
using BL;
using Microsoft.AspNetCore.Http;

namespace Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly IHostingCore _hostingCore;
        private readonly IWebHostEnvironment _appEnvironment;

        public FilesController(IHostingCore hostingCore, IWebHostEnvironment appEnvironment)
        {
            _hostingCore = hostingCore;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Download(long fileId)
        {
            HostingFile file = _hostingCore.GetFileById(fileId);

            if (file == null)
            {
                return NotFound();
            }

            return File(Path.Combine("~", file.Link), "application/octet-stream", file.Name);
        }

        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [RoleAuthorize(Roles.User, Roles.Editor, Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Upload(FileLoadModel loadModel)
        {
            if (ModelState.IsValid)
            { 
            IFormFile uploadedFile = loadModel.UploadedFile;

            User user = _hostingCore.GetUserByEmail(User.Identity.Name);

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

                    var link = $"Files/{user.Id}/";

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

                    userFiles = _hostingCore.GetUserFiles(user.Id);

                    selectedFiles = from userFile in userFiles
                                    where userFile.Name == uploadedFile.FileName
                                    select userFile;

                    file = selectedFiles.FirstOrDefault();

                    file.Link += file.Id;

                    _hostingCore.UpdateFile(file.Id, file);

                    var path = _appEnvironment.WebRootPath + $"/Files/{user.Id}/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path += file.Id;

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    return RedirectToAction("UserPage", "Hosting", new { userId = file.AuthorId });
                }

                return View(loadModel);
            }

            ModelState.AddModelError("", "An error occurred during uploading");
            return View(loadModel);
        }
    }
}
