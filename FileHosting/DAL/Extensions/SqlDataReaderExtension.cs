using DAL.Enums;
using DAL.Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static IHostingEntity GetHostingEntity(this SqlDataReader reader, Tables table)
        {
            IHostingEntity entity = null;

            if (table == Tables.Users)
            {
                entity = new User
                {
                    Id = (ulong)reader.GetInt64("Id"),
                    Login = reader.GetString("Login"),
                    Password = reader.GetString("Password"),
                    Email = reader.GetString("Email"),
                    RoleName = reader.GetString("Role")
                };
            }
            else if (table == Tables.Files)
            {
                entity = new HostingFile
                {
                    Id = (ulong)reader.GetInt64("Id"),
                    Name = reader.GetString("Name"),
                    Size = (ulong)reader.GetInt64("Size"),
                    AuthorId = (ulong)reader.GetInt64("AuthorId"),
                    Description = reader.GetString("Description"),
                    Category = reader.GetString("Category"),
                    Link = reader.GetString("Link")
                };
            }

            return entity;
        }
    }
}
