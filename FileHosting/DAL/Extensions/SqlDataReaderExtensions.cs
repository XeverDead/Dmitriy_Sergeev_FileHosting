using DAL.Enums;
using Common.Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static IHostingEntity GetHostingEntity(this SqlDataReader reader, Tables table)
        {
            IHostingEntity entity = null;

            switch(table)
            {
                case Tables.Users:
                    entity = GetUser(reader);
                    break;

                case Tables.Files:
                    entity = GetHostingFile(reader);
                    break;
            }

            return entity;
        }

        private static User GetUser(SqlDataReader reader)
        {
            var user = new User
            {
                Id = reader.GetInt64("Id"),
                Login = reader.GetString("Login"),
                Password = reader.GetString("Password"),
                Email = reader.GetString("Email"),
                RoleName = reader.GetString("Role")
            };

            return user;
        }

        private static HostingFile GetHostingFile(SqlDataReader reader)
        {
            var file = new HostingFile
            {
                Id = reader.GetInt64("Id"),
                Name = reader.GetString("Name"),
                Size = (ulong)reader.GetInt64("Size"),
                AuthorId = (ulong)reader.GetInt64("AuthorId"),
                Description = reader.GetString("Description"),
                Category = reader.GetString("Category"),
                Link = reader.GetString("Link")
            };

            return file;
        }
    }
}
