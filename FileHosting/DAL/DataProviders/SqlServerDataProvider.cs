using Common.Enums;
using DAL.Extensions;
using Common.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.DataProviders
{
    public class SqlServerDataProvider : IDbDataProvider
    {
        private readonly SqlConnection _connection;

        public SqlServerDataProvider()
        {
            _connection = new SqlConnection()
            {
                ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True"
            };
        }

        public List<IHostingEntity> ExecuteQuery(string expression, Tables table, bool isStoredProcedure)
        {
            //Иногда каким-то образом строка подключения испаряется при вызове методов
            _connection.ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True";

            var entities = new List<IHostingEntity>();

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(expression, _connection);

                if (isStoredProcedure)
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

        public void ExecuteNonQuery(string expression, Tables table, IHostingEntity parameterValues, bool isStoredProcedure)
        {
            //Иногда каким-то образом строка подключения испаряется при вызове методов
            _connection.ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True";

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(expression, _connection);

                if (isStoredProcedure)
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                }

                command.Parameters.AddHostingEntityParameters(table, parameterValues);

                command.ExecuteNonQuery();
            }
        }
    }  
}
