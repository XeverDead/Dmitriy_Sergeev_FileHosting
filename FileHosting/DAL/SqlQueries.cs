namespace DAL
{
    public static class SqlQueries
    {
        public const string GetAllUsers = "SELECT * FROM Users";
        public const string GetAllFiles = "SELECT * FROM Files";

        public static string GetUserById(ulong id) => $"SELECT * FROM Users WHERE Id={id}";
        public static string GetUserFiles(ulong userId) => $"SELECT * FROM Files WHERE AuthorId={userId}";
        public static string GetFileById(ulong id) => $"SELECT * FROM Files WHERE Id={id}";
        public static string GetFilesByCategory(string category) => $"SELECT * FROM Files WHERE Category='{category}'";
        public static string GetFilesByName(string name) => $"SELECT * FROM Files WHERE Name='{name}'";
        public static string GetFilesByExtension(string extension) => $"SELECT * FROM Files WHERE Name LIKE '%{extension}'";

        public const string InsertUser = "INSERT INTO Users (Login, Password, Email, Role)" +
                                         "VALUES (@login, @password, @email, @role)";
        public const string InsertFile = "INSERT INTO Files (Name, Size, AuthorId, Description, Category, Link)" +
                                         "VALUES (@name, @size, @authorId, @description, @category, @link)";

        public static string UpdateUser(ulong id) => "UPDATE Users SET Login=@login, Password=@password, Email=@email, Role=@role" +
                                                    $"WHERE Id={id}";
        public static string UpdateFile(ulong id) => "UPDATE Files SET Name=@name, Size=@size, AuthorId=@authorId, Description=@description," +
                                                    $"Category=@category, Link=@link WHERE Id={id}";

        public static string DeleteUser(ulong id) => $"DELETE Users WHERE Id={id}";
        public static string DeleteFile(ulong id) => $"DELETE Files WHERE Id={id}";
    }
}
