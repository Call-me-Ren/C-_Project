using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class ChiTietPhieuNhapDAO
    {
        /// <summary>
        /// Lấy toàn bộ danh sách chi tiết của một Phiếu nhập cụ thể (JOIN với DauSach để lấy TenSach)
        /// </summary>
        public static List<ChiTietPhieuNhapDTO> GetByMaPN(string maPN)
        {
            List<ChiTietPhieuNhapDTO> list = new List<ChiTietPhieuNhapDTO>();
            string query = @"SELECT ct.MaPN, ct.MaDauSach, ds.TenSach, ct.DonGia, ct.SoLuong, ct.ThanhTien 
                             FROM ChiTietPhieuNhap ct
                             LEFT JOIN DauSach ds ON ct.MaDauSach = ds.MaDauSach
                             WHERE ct.MaPN = @MaPN";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = maPN }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ChiTietPhieuNhapDTO
                {
                    MaPN = row["MaPN"].ToString() ?? "",
                    MaDauSach = row["MaDauSach"].ToString() ?? "",
                    TenSach = row["TenSach"].ToString() ?? "",
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                });
            }
            return list;
        }

        public static bool Insert(ChiTietPhieuNhapDTO ct)
        {
            string query = @"INSERT INTO ChiTietPhieuNhap (MaPN, MaDauSach, DonGia, SoLuong, ThanhTien)
                             VALUES (@MaPN, @MaDauSach, @DonGia, @SoLuong, @ThanhTien)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = ct.MaPN },
                new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = ct.MaDauSach },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = ct.DonGia },
                new SqlParameter("@SoLuong", SqlDbType.Int) { Value = ct.SoLuong },
                new SqlParameter("@ThanhTien", SqlDbType.Decimal) { Value = ct.ThanhTien }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool DeleteByMaPN(string maPN)
        {
            string query = "DELETE FROM ChiTietPhieuNhap WHERE MaPN = @MaPN";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = maPN }
            };
            return MyConnection.ExecuteNonQuery(query, parameters) >= 0;
        }
    }
}
