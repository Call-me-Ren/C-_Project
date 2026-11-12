using Microsoft.Data.SqlClient;
using QLThuVien.DTO;
using System.Data;
using System.Collections.Generic;

namespace QLThuVien.DAO
{
    // DAO thực hiện truy vấn thống kê nhập sách theo Quý
    public static class ThongKeDAO
    {
        // Thống kê tổng tiền nhập theo từng Đầu sách, chia theo Quý, lọc theo năm
        public static List<ThongKeQuyDTO> ThongKeNhapTheoQuy(int nam)
        {
            var dsKetQua = new List<ThongKeQuyDTO>();

            string sql = @"
                SELECT
                    ct.MaDauSach,
                    ds.TenSach,
                    SUM(CASE WHEN DATEPART(QUARTER, pn.NgayNhap) = 1 THEN ct.ThanhTien ELSE 0 END) AS Q1,
                    SUM(CASE WHEN DATEPART(QUARTER, pn.NgayNhap) = 2 THEN ct.ThanhTien ELSE 0 END) AS Q2,
                    SUM(CASE WHEN DATEPART(QUARTER, pn.NgayNhap) = 3 THEN ct.ThanhTien ELSE 0 END) AS Q3,
                    SUM(CASE WHEN DATEPART(QUARTER, pn.NgayNhap) = 4 THEN ct.ThanhTien ELSE 0 END) AS Q4
                FROM ChiTietPhieuNhap ct
                JOIN PhieuNhap pn ON ct.MaPN = pn.MaPN
                JOIN DauSach ds   ON ct.MaDauSach = ds.MaDauSach
                WHERE YEAR(pn.NgayNhap) = @Nam
                GROUP BY ct.MaDauSach, ds.TenSach
                ORDER BY ct.MaDauSach";

            using (var conn = MyConnection.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Nam", SqlDbType.Int) { Value = nam });
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dsKetQua.Add(new ThongKeQuyDTO
                        {
                            MaDauSach = reader["MaDauSach"].ToString()!,
                            TenSach   = reader["TenSach"].ToString()!,
                            Q1 = reader["Q1"] == DBNull.Value ? 0 : (decimal)reader["Q1"],
                            Q2 = reader["Q2"] == DBNull.Value ? 0 : (decimal)reader["Q2"],
                            Q3 = reader["Q3"] == DBNull.Value ? 0 : (decimal)reader["Q3"],
                            Q4 = reader["Q4"] == DBNull.Value ? 0 : (decimal)reader["Q4"],
                        });
                    }
                }
            }

            return dsKetQua;
        }
    }
}
