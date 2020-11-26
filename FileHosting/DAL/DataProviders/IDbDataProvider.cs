using DAL.Enums;
using DAL.Models;
using System.Collections.Generic;

namespace DAL.DataProviders
{
    public interface IDbDataProvider
    {
        List<IHostingEntity> ExecuteQuery(string query, Tables table);

        void ExecuteNonQuery(string query, Tables table, IHostingEntity parameterValues);
    }
}
