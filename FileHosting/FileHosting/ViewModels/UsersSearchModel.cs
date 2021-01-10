using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class UsersSearchModel
    {
        public IEnumerable<User> Users { get; set; }

        public string SearchQuery { get; set; }
    }
}
