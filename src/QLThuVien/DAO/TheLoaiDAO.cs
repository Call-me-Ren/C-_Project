#nullable disable
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLThuVien.DAO
{
    public class TheLoaiDAO
    {
        // DOCDSTL - Đọc danh sách Thể loại
        public List<TheLoaiDTO> DocDSTL()
        {
            List<TheLoaiDTO> dsTL = new List<TheLoaiDTO>();
            try
            {
                string query = "SELECT * FROM TheLoai";

                using (SqlDataReader reader = MyConnection.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        TheLoaiDTO ds = new TheLoaiDTO
                        {
                            MaTL = reader["MaTL"].ToString(),
                            TenTL = reader["TenTL"].ToString()
                        };
                        dsTL.Add(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc danh sách Thể loại: " + ex.Message);
            }

            return dsTL;
        }

        // ThemTheLoai
        public void Them(TheLoaiDTO ds)
        {
            try
            {
                string query = "INSERT INTO TheLoai (MaTL, TenTL) VALUES ('"
                    + ds.MaTL + "', N'"
                    + ds.TenTL + "')";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- ĐÃ THÊM OPEN KẾT NỐI
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                }
            }
            catch { throw; }
        }

        // SuaTheLoai
        public void Sua(TheLoaiDTO ds)
        {
            try
            {
                string query = "UPDATE TheLoai SET TenTL=N'" + ds.TenTL
                    + "' WHERE MaTL='" + ds.MaTL + "'";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- ĐÃ THÊM OPEN KẾT NỐI
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                    if (r == 0) throw new InvalidOperationException("Không tìm thấy thể loại cần cập nhật.");
                }
            }
            catch { throw; }
        }

        // XoaTheLoai (Đã sửa MaTG thành MaTL)
        public void Xoa(string maTL)
        {
            try
            {
                const string query = "DELETE FROM TheLoai WHERE MaTL = @MaTL";
                using (SqlConnection conn = MyConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@MaTL", SqlDbType.VarChar).Value = maTL;
                    conn.Open();
                    if (command.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("Không tìm thấy thể loại cần xóa.");
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa thể loại vì thể loại này đang được dùng trong danh sách đầu sách.", ex);
            }
        }


    }
}
