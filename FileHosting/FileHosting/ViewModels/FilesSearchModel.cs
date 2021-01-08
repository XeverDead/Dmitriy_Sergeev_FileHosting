using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Enums;

namespace Web.ViewModels
{
    public class FilesSearchModel
    {
        public IEnumerable<UserFileModel> UserFileCollection { get; set; }

        public FilesSearchTypes SearchType { get; set; }

        public string SearchQuery { get; set; }
    }
}
