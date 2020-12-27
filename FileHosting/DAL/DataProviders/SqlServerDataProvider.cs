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
        private readonly SqlConnection _connection;

        public IDbExpressions Expressions { get; }

        public SqlServerDataProvider()
        {
            Expressions = new SqlExpressions();

            _connection = new SqlConnection()
            {
                ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True"
            };
        }

        public IEnumerable<IHostingEntity> ExecuteQuery(string expression, Tables table)
        {
            //Иногда каким-то образом строка подключения испаряется при вызове методов
            _connection.ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True";

            var entities = new List<IHostingEntity>();

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(expression, _connection);

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
            //Иногда каким-то образом строка подключения испаряется при вызове методов
            _connection.ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True";

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(expression, _connection);

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
