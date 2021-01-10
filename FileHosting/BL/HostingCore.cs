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

        public HostingCore(IDbDataProvider dbDataProvider)
        {
            _dbDataProvider = dbDataProvider;
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
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetAllFiles, Tables.Files).Cast<HostingFile>();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetAllUsers, Tables.Users).Cast<User>();
        }

        public HostingFile GetFileById(long id)
        {
            IEnumerable<HostingFile> files = _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFileById(id), Tables.Files).Cast<HostingFile>();

            return files.FirstOrDefault();
        }

        public IEnumerable<HostingFile> GetFilesByCategory(string category)
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByCategory(category), Tables.Files).Cast<HostingFile>();
        }

        public IEnumerable<HostingFile> GetFilesByExtension(string extension)
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByExtension(extension), Tables.Files).Cast<HostingFile>();
        }

        public IEnumerable<HostingFile> GetFilesByName(string name)
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetFilesByName(name), Tables.Files).Cast<HostingFile>();
        }

        public User GetUserByEmail(string email)
        {
            IEnumerable<User> users = _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserByEmail(email), Tables.Users).Cast<User>();

            return users.FirstOrDefault();
        }

        public User GetUserById(long id)
        {
            IEnumerable<User> users = _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserById(id), Tables.Users).Cast<User>();

            return users.FirstOrDefault();
        }

        public IEnumerable<HostingFile> GetUserFiles(long userId)
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUserFiles(userId), Tables.Files).Cast<HostingFile>();
        }

        public IEnumerable<User> GetUsersByLogin(string login)
        {
            return _dbDataProvider.ExecuteQuery(_dbDataProvider.Expressions.GetUsersByLogin(login), Tables.Users).Cast<User>();
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

       