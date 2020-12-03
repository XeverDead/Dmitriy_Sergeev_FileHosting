namespace Common.SqlExpressions
{
    public static class SqlQueries
    {
        public static string GetAllUsers => "SELECT * FROM Users";
        public static string GetAllFiles => "SELECT * FROM Files";

        public static string GetUserById(long id) => $"SELECT * FROM Users WHERE Id={id}";
        public static string GetUserByEmail(string email) => $"SELECT * FROM Users WHERE Email='{email}'";
        public static string GetUserFiles(long userId) => $"SELECT * FROM Files WHERE AuthorId={userId}";
        public static string GetFileById(long id) => $"SELECT * FROM Files WHERE Id={id}";
        public static string GetFilesByCategory(string category) => $"SELECT * FROM Files WHERE Category='{category}'";
        public static string GetFilesByName(string name) => $"SELECT * FROM Files WHERE Name='{name}'";
        public static string GetFilesByExtension(string extension) => $"SELECT * FROM Files WHERE Name LIKE '%{extension}'";      

        public static string UpdateUser(long id) => "UPDATE Users SET Login=@login, Password=@password, Email=@email, Role=@role" +
                                                    $"WHERE Id={id}";
        public static string UpdateFile(long id) => "UPDATE Files SET Name=@name, Size=@size, AuthorId=@authorId, Description=@description," +
                                                    $"Category=@category, Link=@link WHERE Id={id}";

        public static string DeleteUser(long id) => $"DELETE Users WHERE Id={id}";
        public static string DeleteFile(long id) => $"DELETE Files WHERE Id={id}";
    }
}
