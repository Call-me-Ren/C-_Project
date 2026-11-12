using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Class MyConnection: Quản lý kết nối CSDL SQL Server duy nhất của toàn bộ hệ thống.
    /// Tuân thủ quy tắc giáo viên: Chỉ viết kết nối 1 lần duy nhất tại đây, các lớp DAO khác sẽ gọi dùng lại.
    /// </summary>
    public class MyConnection
    {
        // Chuỗi kết nối tới SQL Server:
        // - Server: . (hoặc localhost)
        // - Database: QLThuVien
        // - Integrated Security=True (Windows Authentication)
        // - TrustServerCertificate=True: Tránh lỗi chứng chỉ bảo mật trên môi trường local
        private static string connectionString = @"Server=.;Database=QLThuVien;Integrated Security=True;TrustServerCertificate=True;";

        /// <summary>
        /// Cho phép thay đổi chuỗi kết nối nếu tên Server của bạn khác (ví dụ: .\SQLEXPRESS)
        /// </summary>
        public static void SetConnectionString(string newConnString)
        {
            connectionString = newConnString;
        }

        public static string GetConnectionString()
        {
            return connectionString;
        }

        /// <summary>
        /// Tạo và trả về một đối tượng SqlConnection mới (chưa mở kết nối)
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// 1. Thực thi câu lệnh SELECT trả về DataTable (đóng kết nối ngay, an toàn không sợ leak connection).
        /// Dùng để nạp dữ liệu lên DataGridView hoặc chuyển đổi sang ArrayList trong lớp BUS.
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 2. Thực thi câu lệnh SELECT trả về SqlDataReader dạng đọc tuần tự (Stream), đúng chuẩn slide bài giảng của GV.
        /// Sử dụng CommandBehavior.CloseConnection để tự động đóng kết nối khi SqlDataReader đóng hoặc ra khỏi khối using.
        /// </summary>
        public static SqlDataReader ExecuteReader(string query, SqlParameter[]? parameters = null)
        {
            SqlConnection conn = GetConnection();
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 3. Thực thi các câu lệnh thay đổi dữ liệu: INSERT, UPDATE, DELETE.
        /// Trả về số dòng bị ảnh hưởng (int).
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// 4. Thực thi câu lệnh trả về giá trị đơn lẻ (Ví dụ: SELECT COUNT(*), SELECT MAX(Ma)...)
        /// </summary>
        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            object? result = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    result = cmd.ExecuteScalar();
                }
            }

            return result;
        }
    }
}
