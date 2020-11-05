using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;

namespace DAL.DataProviders
{
    public class SqlServerDataProvider : IDataProvider
    {
        private SqlConnection _connection;

        public SqlServerDataProvider()
        {
            _connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=HostingDb;Trusted_Connection=True");
        }

        public List<HostingFile> GetFiles()
        {
            var result = new List<HostingFile>();

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand("SELECT * FROM Files", _connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new HostingFile
                    {
                        Id = (ulong)reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Size = (ulong)reader.GetInt64(2),
                        AuthorId = (ulong)reader.GetInt64(3),
                        Description = reader.GetString(4),
                        Category = reader.GetString(5),
                        Link = reader.GetString(6)
                    });
                }
            }

            return result;
        }

        public List<User> GetUsers()
        {
            var result = new List<User>();

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand("SELECT * FROM Users", _connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new User
                    {
                        Id = (ulong)reader.GetInt64(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        Email = reader.GetString(3),
                        RoleName = reader.GetString(4)
                    });
                }
            }

            return result;
        }

        public bool SetFile(HostingFile file, bool isNew)
        {
            var result = true;

            if (file.Id != 0 && isNew)
            {
                return false;
            }

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand()
                {
                    Connection = _connection
                };

                if (isNew)
                {
                    command.CommandText = "INSERT INTO Files (Name, Size, AuthorId, Description, Category, Link)" +
                    "VALUES (@name, @size, @authorId, @description, @category, @link)";                   
                }
                else
                {
                    command.CommandText = "UPDATE Files SET Name=@name, Size=@size, AuthorId=@authorId, Description=@description," +
                        $"Category=@category, Link=@link WHERE Id={file.Id}";
                }

                command.Parameters.AddWithValue("@name", file.Name);
                command.Parameters.AddWithValue("@size", file.Size);
                command.Parameters.AddWithValue("@authorId", file.AuthorId);
                command.Parameters.AddWithValue("@description", file.Description);
                command.Parameters.AddWithValue("@category", file.Category);
                command.Parameters.AddWithValue("@link", file.Link);

                result = command.ExecuteNonQuery() == 1;
            }

            return result;
        }

        public bool SetUser(User user, bool isNew)
        {
            var result = true;

            if (user.Id != 0 && isNew)
            {
                return false;
            }

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand()
                {
                    Connection = _connection
                };

                if (isNew)
                {
                    command.CommandText = "INSERT INTO Users (Login, Password, Email, Role)" +
                    "VALUES (@login, @password, @email, @role)";
                }
                else
                {
                    command.CommandText = "UPDATE Users SET Login=@login, Password=@password, Email=@email, Role=@role" +
                        $"WHERE Id={user.Id}";
                }

                command.Parameters.AddWithValue("@login", user.Login);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@role", user.RoleName);

                result = command.ExecuteNonQuery() == 1;
            }

            return result;
        }

        public bool DeleteUser(User user)
        {
            var result = true;

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand($"DELETE Users WHERE Id={user.Id}", _connection);

                result = command.ExecuteNonQuery() == 1;
            }

            return result;
        }

        public bool DeleteFile(HostingFile file)
        {
            var result = true;

            using (_connection)
            {
                _connection.Open();

                var command = new SqlCommand($"DELETE Files WHERE Id={file.Id}", _connection);

                result = command.ExecuteNonQuery() == 1;
            }

            return result;
        }
    }
}
