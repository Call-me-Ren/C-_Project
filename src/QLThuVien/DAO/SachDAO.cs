using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QLThuVien.DAO
{
    public class SachDAO
    {
        /// <summary>
        /// Tự động sinh mã và thêm các cuốn sách vật lý mới vào bảng Sach khi nhập
        /// Ví dụ: Mã cuốn sách là DS01_001, DS01_002...
        /// </summary>
        public static bool SinhCacCuonSachMoi(string maDauSach, int soLuong)
        {
            // Lấy số lượng cuốn sách hiện có của đầu sách này để sinh mã tiếp theo
            string queryCount = "SELECT COUNT(*) FROM Sach WHERE MaDauSach = @MaDauSach";
            SqlParameter[] countParam = new SqlParameter[]
            {
                new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = maDauSach }
            };

            object? countObj = MyConnection.ExecuteScalar(queryCount, countParam);
            int currentCount = countObj != null ? Convert.ToInt32(countObj) : 0;

            for (int i = 1; i <= soLuong; i++)
            {
                int nextIndex = currentCount + i;
                string maSachMoi = $"{maDauSach}_{nextIndex:D3}"; // Định dạng: DS01_001, DS01_002...

                string insertQuery = @"INSERT INTO Sach (MaSach, MaDauSach, TinhTrang, ViTri) 
                                       VALUES (@MaSach, @MaDauSach, N'Mới', N'Kho chính')";

                SqlParameter[] insertParams = new SqlParameter[]
                {
                    new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSachMoi },
                    new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = maDauSach }
                };

                MyConnection.ExecuteNonQuery(insertQuery, insertParams);
            }

            return true;
        }
    }
}
