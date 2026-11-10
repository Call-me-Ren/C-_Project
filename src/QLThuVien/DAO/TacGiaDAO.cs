#nullable disable
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLThuVien.DAO
{
    public class TacGiaDAO
    {
        //DOCDSSTG
        public List<TacGiaDTO> DocDSTG()
        {
            List<TacGiaDTO> dsTG = new List<TacGiaDTO>();
            string query = @"SELECT * FROM TacGia";

            using (SqlDataReader reader = MyConnection.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    TacGiaDTO ds = new TacGiaDTO
                    {
                        MaTG = reader["MaTG"].ToString(),
                        HoLot = reader["HoLot"].ToString(),
                        TenTG = reader["TenTG"].ToString(),
                        NgaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]) : (DateTime?)null,
                        SDT = reader["SDT"].ToString()
                    };
                    dsTG.Add(ds);
                }
            }
            return dsTG;
        }

        //ThemTG
        public void Them(TacGiaDTO ds)
        {
            try
            {
                string query = "INSERT INTO TacGia VALUES ('"
                    + ds.MaTG + "', N'"
                    + ds.HoLot + "', N'"
                    + ds.TenTG + "', "
                    + (ds.NgaySinh.HasValue ? "'" + ds.NgaySinh.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", '"
                    + ds.SDT + "')";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- BỔ SUNG: Mở kết nối trước khi thực thi
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi thông tin Tác giả: " + ex.Message);
            }
        }

        //suaTG
        public void Sua(TacGiaDTO ds)
        {
            try
            {
                string query = "UPDATE TacGia SET HoLot=N'" + ds.HoLot
                    + "', TenTG=N'" + ds.TenTG
                    + "', NgaySinh=" + (ds.NgaySinh.HasValue ? "'" + ds.NgaySinh.Value.ToString("yyyy-MM-dd") + "'" : "NULL")
                    + ", SDT='" + ds.SDT + "'"
                    + " WHERE MaTG='" + ds.MaTG + "'";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- BỔ SUNG: Mở kết nối trước khi thực thi
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật Tác giả: " + ex.Message);
            }
        }

        //xoaTG
        public void Xoa(string maTG)
        {
            try
            {
                string query = "DELETE FROM TacGia WHERE MaTG='" + maTG + "'";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- BỔ SUNG: Mở kết nối trước khi thực thi
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa Tác giả: " + ex.Message);
            }
        }

        //timTG
        public List<TacGiaDTO> timKiem(string maTG, string tenTG)
        {
            List<TacGiaDTO> dsSach = new List<TacGiaDTO>();
            try
            {
                string query = "SELECT * FROM TacGia WHERE MaTG='" + maTG + "' OR TenTG=N'" + tenTG + "'";

                using (SqlDataReader reader = MyConnection.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        TacGiaDTO ds = new TacGiaDTO
                        {
                            MaTG = reader["MaTG"].ToString(),
                            HoLot = reader["HoLot"].ToString(),
                            TenTG = reader["TenTG"].ToString(),
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]) : (DateTime?)null,
                            SDT = reader["SDT"].ToString()
                        };
                        dsSach.Add(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }

            return dsSach;
        }
    }
}