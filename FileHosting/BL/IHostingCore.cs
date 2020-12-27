using Common.Models;
using System.Collections.Generic;

namespace BL
{
    public interface IHostingCore
    {
        IEnumerable<User> GetAllUsers();

        IEnumerable<HostingFile> GetAllFiles();

        User GetUserById(long id);

        HostingFile GetFileById(long id);

        void InsertUser(User user);

        void InsertFile(HostingFile file);

        void UpdateUser(long id, User user);

        void UpdateFile(long id, HostingFile file);

        void DeleteUser(long id);

        void DeleteFile(long id);

        User GetUserByEmail(string email);

        IEnumerable<User> GetUsersByLogin(string login);

        IEnumerable<HostingFile> GetUserFiles(long userId);

        IEnumerable<HostingFile> GetFilesByCategory(string category);

        IEnumerable<HostingFile> GetFilesByName(string name);

        IEnumerable<HostingFile> GetFilesByExtension(string extension);
    }
}
