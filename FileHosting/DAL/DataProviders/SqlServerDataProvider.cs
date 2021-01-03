using DAL.DbExpressions;
using DAL.Enums;
using DAL.Extensions;
using Common.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.DataProviders
{
    public class SqlServerDataProvider : IDbDataProvider
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public IDbExpressions Expressions { get; }

        public SqlServerDataProvider(IConfiguration configuration)
        {
            //Насчёт DI для выражений (если я вообще правильно понял суть DI). Дело в том, что для дата провайдеров 
            //подойдут только их "родные" выражения и никакие другие. Так тут надо опредялять нужный класс выражений
            //внутри провайдера, а не шде то снаружи, иначе всё может упасть.
            Expressions = new SqlExpressions();

            _connection = new SqlConnection();

            _connectionString = configuration.GetConnectionString("SqlServerConnection");
        }

        public IEnumerable<IHostingEntity> ExecuteQuery(string expression, Tables table)
        {
            var entities = new List<IHostingEntity>();

            //Проблема со строкой подключения. Если один метод контроллера несколько раз обращается к базе данных,
            //то новый объект класса провайдера не создаётся, а в старом подключение уже закрыто и строка подключения 
            //из него убрана. Как это нормально пофиксить - без понятия.
            _connection.ConnectionString = _connectionString;
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
            _connection.ConnectionString = _connectionString;
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
