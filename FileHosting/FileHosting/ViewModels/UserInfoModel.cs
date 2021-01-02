using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class UserInfoModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Nickname should not be empty")]
        [StringLength(50, ErrorMessage = "Max nickname size is 50 symbols")]
        public string Login { get; set; }

        public Roles Role { get; set; }
    }
}
