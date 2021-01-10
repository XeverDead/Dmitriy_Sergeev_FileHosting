using Common.Models;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class UserFilesModel
    {
        public User User { get; set; }

        public IEnumerable<HostingFile> Files { get; set; }
    }
}
