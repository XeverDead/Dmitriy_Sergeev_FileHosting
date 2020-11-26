using DAL.Enums;
using System;
using System.Linq;

namespace DAL.Models
{
    public class User : IHostingEntity
    {
        public ulong Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }  
        public string RoleName
        {
            get => Role.ToString();
            set
            {
                if (Enum.GetNames(typeof(Roles)).Contains(value))
                {
                    Role = (Roles)Enum.Parse(typeof(Roles), value);
                }
                else
                {
                    Role = Roles.Guest;
                }
            }
        }
    }
}
