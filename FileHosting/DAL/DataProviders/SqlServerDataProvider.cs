using DAL.Enums;
using DAL.Extensions;
using DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.DataProviders
{
    public class SqlServerDataProvider : IDbDataProvider
    {
        private readonly SqlConnection _connection;

        public SqlServerDataProvider()
        {
            _connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True");
        }

        public List<IHostingEntity> ExecuteQuery(string query, Tables table)
        {
            var entities = new List<IHostingEntity>();

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(query, _connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entities.Add(reader.GetHostingEntity(table));
                    }
                }
            }

            return entities;
        }

        public void ExecuteNonQuery(string query, Tables table, IHostingEntity parameterValues)
        {
            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand(query, _connection);

                command.Parameters.AddHostingEntityParameters(table, parameterValues);

                command.ExecuteNonQuery();
            }
        }
    }  
}
