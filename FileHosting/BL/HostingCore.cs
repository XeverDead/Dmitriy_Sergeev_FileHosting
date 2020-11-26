using Common.Enums;
using Common.Models;
using DAL.DataProviders;
using System.Collections.Generic;

namespace BL
{
    public class HostingCore : IHostingCore
    {
        private IDbDataProvider _dbDataProvider;

        public HostingCore()
        {
            _dbDataProvider = new SqlServerDataProvider();
        }

        public void ExecuteNonQuery(string expression, Tables table, IHostingEntity parameterValues, bool isStoredProcedure)
        {
            _dbDataProvider.ExecuteNonQuery(expression, table, parameterValues, isStoredProcedure);
        }

        public List<IHostingEntity> ExecuteQuery(string expression, Tables table, bool isStoredProcedure)
        {
            return _dbDataProvider.ExecuteQuery(expression, table, isStoredProcedure);
        }
    }
}
