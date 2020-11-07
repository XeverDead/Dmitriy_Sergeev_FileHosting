using DAL.Enums;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;

namespace DAL.Extensions
{
    public static class SqlParameterCollectionExtension
    {
        public static void AddHostingEntityParameters(this SqlParameterCollection parameterCollection, Tables table, IHostingEntity parameterValues)
        {
            if (table == Tables.Users)
            {
                var userValues = (User)parameterValues;

                parameterCollection.AddWithValue("@login", userValues.Login);
                parameterCollection.AddWithValue("@password", userValues.Password);
                parameterCollection.AddWithValue("@email", userValues.Email);
                parameterCollection.AddWithValue("@role", userValues.RoleName);
            }
            else if (table == Tables.Files)
            {
                var fileValues = (HostingFile)parameterValues;

                parameterCollection.AddWithValue("@name", fileValues.Name);
                parameterCollection.AddWithValue("@size", fileValues.Size);
                parameterCollection.AddWithValue("@authorId", fileValues.AuthorId);
                parameterCollection.AddWithValue("@description", fileValues.Description);
                parameterCollection.AddWithValue("@category", fileValues.Category);
                parameterCollection.AddWithValue("@link", fileValues.Link);
            }
        }
    }
}
