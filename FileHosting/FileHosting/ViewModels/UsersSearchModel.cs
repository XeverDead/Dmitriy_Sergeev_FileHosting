using Common.Models;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class UsersSearchModel
    {
        public IEnumerable<User> Users { get; set; }

        public string SearchQuery { get; set; }
    }
}
