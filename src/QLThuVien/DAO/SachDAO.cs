#nullable disable
using QLThuVien.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLThuVien.DAO
{
    public class SachDAO
    {
        // 1. Đọc danh sách Sách từ CSDL
        public List<SachDTO> DocDSSach()
        {
            List<SachDTO> dsSach = new List<SachDTO>();
            string query = @"SELECT MaSach, MaDauSach, TrangThai, TinhTrang, ViTri FROM Sach";

            using (SqlDataReader reader = MyConnection.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    SachDTO ds = new SachDTO
                    {
                        MaSach = reader["MaSach"].ToString(),
                        MaDauSach = reader["MaDauSach"].ToString(),
                        TrangThai = reader["TrangThai"].ToString(),
                        TinhTrang = reader["TinhTrang"].ToString(),
                        ViTri = reader["ViTri"].ToString()
                    };
                    dsSach.Add(ds);
                }
            }
            return dsSach;
        }

        // 2. Thêm Sách mới
        public bool Them(SachDTO ds)
        {
            try
            {
                string trangThaiVal = (ds.TrangThai == "Đã cho mượn" || ds.TrangThai == "1" || ds.TrangThai == "True") ? "1" : "0";
                string query = @"INSERT INTO Sach (MaSach, MaDauSach, TrangThai, TinhTrang, ViTri) 
                                 VALUES (@MaSach, @MaDauSach, @TrangThai, @TinhTrang, @ViTri)";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSach", ds.MaSach);
                    cmd.Parameters.AddWithValue("@MaDauSach", ds.MaDauSach);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThaiVal);
                    cmd.Parameters.AddWithValue("@TinhTrang", (object)ds.TinhTrang ?? "");
                    cmd.Parameters.AddWithValue("@ViTri", (object)ds.ViTri ?? "");

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new InvalidOperationException($"Mã sách '{ds.MaSach}' đã tồn tại. Vui lòng nhập mã sách khác.", ex);
            }
            catch
            {
                throw;
            }
        }

        // 3. Sửa thông tin Sách
        public bool Sua(SachDTO ds)
        {
            try
            {
                string trangThaiVal = (ds.TrangThai == "Đã cho mượn" || ds.TrangThai == "1" || ds.TrangThai == "True") ? "1" : "0";
                string query = @"UPDATE Sach 
                                 SET MaDauSach = @MaDauSach, TrangThai = @TrangThai, TinhTrang = @TinhTrang, ViTri = @ViTri 
                                 WHERE MaSach = @MaSach";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSach", ds.MaSach);
                    cmd.Parameters.AddWithValue("@MaDauSach", ds.MaDauSach);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThaiVal);
                    cmd.Parameters.AddWithValue("@TinhTrang", (object)ds.TinhTrang ?? "");
                    cmd.Parameters.AddWithValue("@ViTri", (object)ds.ViTri ?? "");

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                throw;
            }
        }

        // 4. Xóa Sách
        public bool Xoa(string maSach)
        {
            try
            {
                string query = "DELETE FROM Sach WHERE MaSach = @MaSach";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException(
                    $"Không thể xóa sách '{maSach}' vì sách này đang được tham chiếu trong phiếu mượn hoặc lịch sử mượn trả. Hãy xử lý dữ liệu phiếu mượn liên quan trước.",
                    ex);
            }
        }

        // 5. Cập nhật trạng thái mượn/trả
        public bool CapNhatTrangThai(string maSach, bool trangThai)
        {
            try
            {
                string query = "UPDATE Sach SET TrangThai = @TrangThai WHERE MaSach = @MaSach";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai ? "1" : "0");
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
