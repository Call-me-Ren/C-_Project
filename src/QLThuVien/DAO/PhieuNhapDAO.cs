using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class PhieuNhapDAO
    {
        public static List<PhieuNhapDTO> GetAll()
        {
            List<PhieuNhapDTO> list = new List<PhieuNhapDTO>();
            string query = "SELECT MaPN, MaNCC, MaNV, NgayNhap, TongTien FROM PhieuNhap ORDER BY NgayNhap DESC";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new PhieuNhapDTO
                {
                    MaPN = row["MaPN"].ToString() ?? "",
                    MaNCC = row["MaNCC"].ToString() ?? "",
                    MaNV = row["MaNV"].ToString() ?? "",
                    NgayNhap = Convert.ToDateTime(row["NgayNhap"]),
                    TongTien = Convert.ToDecimal(row["TongTien"])
                });
            }
            return list;
        }

        public static bool Insert(PhieuNhapDTO pn)
        {
            string query = @"INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien)
                             VALUES (@MaPN, @MaNCC, @MaNV, @NgayNhap, @TongTien)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = pn.MaPN },
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = pn.MaNCC },
                new SqlParameter("@MaNV", SqlDbType.VarChar) { Value = pn.MaNV },
                new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = pn.NgayNhap },
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = pn.TongTien }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool UpdateTongTien(string maPN, decimal tongTien)
        {
            string query = "UPDATE PhieuNhap SET TongTien = @TongTien WHERE MaPN = @MaPN";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = tongTien },
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = maPN }
            };
            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
