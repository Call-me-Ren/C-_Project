using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class DauSachDAO
    {
        public static List<DauSachDTO> GetAll()
        {
            List<DauSachDTO> list = new List<DauSachDTO>();
            string query = "SELECT MaDauSach, TenSach, DonGia, SoLuong, SLChoMuon, SLConLai FROM DauSach";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DauSachDTO
                {
                    MaDauSach = row["MaDauSach"].ToString() ?? "",
                    TenSach = row["TenSach"].ToString() ?? "",
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    SLChoMuon = Convert.ToInt32(row["SLChoMuon"]),
                    SLConLai = Convert.ToInt32(row["SLConLai"])
                });
            }
            return list;
        }

        /// <summary>
        /// Tự động cập nhật tăng SoLuong và SLConLai khi nhập sách
        /// </summary>
        public static bool TangTonKho(string maDauSach, int soLuongThem)
        {
            string query = @"UPDATE DauSach 
                             SET SoLuong = SoLuong + @SoLuongThem,
                                 SLConLai = SLConLai + @SoLuongThem
                             WHERE MaDauSach = @MaDauSach";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SoLuongThem", SqlDbType.Int) { Value = soLuongThem },
                new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = maDauSach }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
