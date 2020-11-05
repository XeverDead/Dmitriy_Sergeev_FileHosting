using System;
using System.Linq;
using LinqToDB.Mapping;

namespace DAL.Models
{
    public enum Roles
    {
        Guest,
        User,
        Editor,
        Admin
    }

    public class User
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
