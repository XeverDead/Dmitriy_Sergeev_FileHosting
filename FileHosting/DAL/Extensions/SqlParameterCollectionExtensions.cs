using DAL.Enums;
using Common.Models;
using System.Data.SqlClient;

namespace DAL.Extensions
{
    public static class SqlParameterCollectionExtensions
    {
        public static void AddHostingEntityParameters(this SqlParameterCollection parameterCollection, Tables table, IHostingEntity parameterValues)
        {
            switch (table)
            {
                case Tables.Users:
                    AddUserParameters(parameterCollection, parameterValues);
                    break;

                case Tables.Files:
                    AddHostingFileParameters(parameterCollection, parameterValues);
                    break;
            }
        }

        private static void AddUserParameters(SqlParameterCollection parameterCollection, IHostingEntity parameterValues)
        {
            var userValues = (User)parameterValues;

            parameterCollection.AddWithValue("@login", userValues.Login);
            parameterCollection.AddWithValue("@password", userValues.Password);
            parameterCollection.AddWithValue("@email", userValues.Email);
            parameterCollection.AddWithValue("@role", userValues.RoleName);
        }

        private static void AddHostingFileParameters(SqlParameterCollection parameterCollection, IHostingEntity parameterValues)
        {
            var fileValues = (HostingFile)parameterValues;

            if (fileValues.Category == null)
            {
                fileValues.Category = string.Empty;
            }

            if (fileValues.Description == null)
            {
                fileValues.Description = string.Empty;
            }

            parameterCollection.AddWithValue("@name", fileValues.Name);
            parameterCollection.AddWithValue("@size", fileValues.Size);
            parameterCollection.AddWithValue("@authorId", fileValues.AuthorId);
            parameterCollection.AddWithValue("@description", fileValues.Description);
            parameterCollection.AddWithValue("@category", fileValues.Category);
            parameterCollection.AddWithValue("@link", fileValues.Link);
        }
    }
}
