using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace Web.ViewModels
{
    public class UserFilesModel
    {
        public User User { get; set; }

        public IEnumerable<HostingFile> Files { get; set; }
    }
}
