using DAL.Enums;
using Common.Models;
using DAL.DataProviders;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class HostingCore : IHostingCore
    {
        private readonly IDbDataProvider _dbDataProvider;

        public HostingCore()
        {
            _dbDataProvider = new SqlServerDataProvider();
        }

        public void DeleteFile(long id)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.DeleteFile(id), Tables.Files, null);
        }

        public void DeleteUser(long id)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.DeleteUser(id), Tables.Users, null);
        }

        public IEnumerable<HostingFile> GetAllFiles()
        {
            return (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetAllFiles, Tables.Files);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return (IEnumerable<User>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetAllUsers, Tables.Users);
        }

        public HostingFile GetFileById(long id)
        {
            var files = (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFileById(id), Tables.Files);

            return files.FirstOrDefault();
        }

        public IEnumerable<HostingFile> GetFilesByCategory(string category)
        {
            return (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByCategory(category), Tables.Files);
        }

        public IEnumerable<HostingFile> GetFilesByExtension(string extension)
        {
            return (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByExtension(extension), Tables.Files);
        }

        public IEnumerable<HostingFile> GetFilesByName(string name)
        {
            return (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByName(name), Tables.Files);
        }

        public User GetUserByEmail(string email)
        {
            var users = (IEnumerable<User>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserByEmail(email), Tables.Users);

            return users.FirstOrDefault();
        }

        public User GetUserById(long id)
        {
            var users = (IEnumerable<User>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserById(id), Tables.Users);

            return users.FirstOrDefault();
        }

        public IEnumerable<HostingFile> GetUserFiles(long userId)
        {
            return (IEnumerable<HostingFile>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserFiles(userId), Tables.Files);
        }

        public IEnumerable<User> GetUsersByLogin(string login)
        {
            return (IEnumerable<User>)_dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUsersByLogin(login), Tables.Users);
        }

        public void InsertFile(HostingFile file)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.InsertFile, Tables.Files, file);
        }

        public void InsertUser(User user)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.InsertUser, Tables.Users, user);
        }

        public void UpdateFile(long id, HostingFile file)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.UpdateFile(id), Tables.Files, file);
        }

        public void UpdateUser(long id, User user)
        {
            _dbDataProvider.ExecuteNonQuery(_dbDataProvider.Expressions.UpdateUser(id), Tables.Users, user);
        }
    }
}

       