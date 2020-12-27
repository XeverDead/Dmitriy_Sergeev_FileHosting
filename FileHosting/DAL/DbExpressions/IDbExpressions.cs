namespace DAL.DbExpressions
{
    public interface IDbExpressions
    {
        string GetAllUsers { get; }
        string GetAllFiles { get; }

        string InsertUser { get; }
        string InsertFile { get; }

        string GetUserById(long id) => string.Empty;
        string GetUserByEmail(string email) => string.Empty;
        string GetUsersByLogin(string login) => string.Empty;
        string GetUserFiles(long userId) => string.Empty;
        string GetFileById(long id) => string.Empty;
        string GetFilesByCategory(string catrgory) => string.Empty;
        string GetFilesByName(string name) => string.Empty;
        string GetFilesByExtension(string extension) => string.Empty;

        string UpdateUser(long id) => string.Empty;
        string UpdateFile(long id) => string.Empty;

        string DeleteUser(long id) => string.Empty;
        string DeleteFile(long id) => string.Empty; 
    }
}
