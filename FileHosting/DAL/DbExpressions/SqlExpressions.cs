namespace DAL.DbExpressions
{
    public class SqlExpressions : IDbExpressions
    {
        public string GetAllUsers => "SELECT * FROM Users";
        public string GetAllFiles => "SELECT * FROM Files";

        public string InsertUser => "sp_InsertUser";
        public string InsertFile => "sp_InsertFile";

        public string GetUserById(long id) => $"SELECT * FROM Users WHERE Id={id}";
        public string GetUserByEmail(string email) => $"SELECT * FROM Users WHERE Email='{email}'";
        public string GetUsersByLogin(string login) => $"SELECT * FROM Users WHERE Login LIKE '%{login}%'";
        public string GetUserFiles(long userId) => $"SELECT * FROM Files WHERE AuthorId={userId}";
        public string GetFileById(long id) => $"SELECT * FROM Files WHERE Id={id}";
        public string GetFilesByCategory(string category) => $"SELECT * FROM Files WHERE Category LIKE '%{category}%'";
        public string GetFilesByName(string name) => $"SELECT * FROM Files WHERE Name LIKE '%{name}%.'";
        public string GetFilesByExtension(string extension) => $"SELECT * FROM Files WHERE Name LIKE '%{extension}'";      

        public string UpdateUser(long id) => "UPDATE Users SET Login=@login, Password=@password, Email=@email, Role=@role " +
                                                    $"WHERE Id={id}";
        public string UpdateFile(long id) => "UPDATE Files SET Name=@name, Size=@size, AuthorId=@authorId, Description=@description," +
                                                    $"Category=@category, Link=@link WHERE Id={id}";

        public string DeleteUser(long id) => $"DELETE Users WHERE Id={id}";
        public string DeleteFile(long id) => $"DELETE Files WHERE Id={id}";
    }
}
