using Common.Enums;
using Common.Models;
using System.Collections.Generic;

namespace BL
{
    public interface IHostingCore
    {
        List<IHostingEntity> ExecuteQuery(string expression, Tables table, bool isStoredProcedure);

        void ExecuteNonQuery(string expression, Tables table, IHostingEntity parameterValues, bool isStoredProcedure);
    }
}
