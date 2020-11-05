using DAL.Models;
using LinqToDB;
using System.Collections.Generic;

namespace DAL.DataProviders
{
    public interface IDataProvider
    {
        List<User> GetUsers();
        List<HostingFile> GetFiles();


        bool SetUser(User user, bool isNew);
        bool SetFile(HostingFile file, bool isNew);

        bool DeleteUser(User user);
        bool DeleteFile(HostingFile file);
    }
}
