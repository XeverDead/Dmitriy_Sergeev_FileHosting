using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace Web.ViewModels
{
    public class UserFileModel
    {
        public User User { get; set; }
        public HostingFile File { get; set; }
    }
}
