using DAL.Enums;
using Common.Models;
using DAL.DbExpressions;
using System.Collections.Generic;

namespace DAL.DataProviders
{
    public interface IDbDataProvider
    {
        IDbExpressions Expressions { get; }

        IEnumerable<IHostingEntity> ExecuteQuery(string expression, Tables table);

        void ExecuteNonQuery(string expression, Tables table, IHostingEntity parameterValues);
    }
}
