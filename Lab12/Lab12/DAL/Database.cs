namespace Lab12.DAL
{
    public static class Database
    {
        // Chuỗi kết nối đến SQL Server (Tên server tùy thuộc vào máy của bạn, thường là "." hoặc ".\SQLEXPRESS")
        public static string ConnectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True";
    }
}