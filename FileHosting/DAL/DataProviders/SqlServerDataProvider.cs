using DAL.DbExpressions;
using DAL.Enums;
using DAL.Extensions;
using Common.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.DataProviders
{
    public class SqlServerDataProvider : IDbDataProvider
    {
        private readonly string _connectionString;

        public IDbExpressions Expressions { get; }

        public SqlServerDataProvider(string connectionString)
        {
            Expressions = new SqlExpressions();

            _connectionString = connectionString;
        }

        public IEnumerable<IHostingEntity> ExecuteQuery(string expression, Tables table)
        {
            var entities = new List<IHostingEntity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(expression, connection);

                if (expression.StartsWith("sp_"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            entities.Add(reader.GetHostingEntity(table));
                        }
                    }
                }
            }

            return entities;
        }

        public void ExecuteNonQuery(string expression, Tables table, IHostingEntity parameterValues)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(expression, connection);

                if (expression.StartsWith("sp_"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                }

                if (parameterValues != null)
                {
                    command.Parameters.AddHostingEntityParameters(table, parameterValues);
                }

                command.ExecuteNonQuery();
            }
        }
    }  
}
