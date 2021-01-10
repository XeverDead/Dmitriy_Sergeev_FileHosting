using System.Collections.Generic;
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
