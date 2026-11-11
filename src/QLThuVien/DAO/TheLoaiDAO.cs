#nullable disable
using QLThuVien.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
            catch { throw; }

            return dsTL;
        }

        // ThemTheLoai
        public void Them(TheLoaiDTO ds)
        {
            try
            {
                const string query = "INSERT INTO TheLoai (MaTL, TenTL) VALUES (@MaTL, @TenTL)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = ds.MaTL },
                    new SqlParameter("@TenTL", SqlDbType.NVarChar) { Value = ds.TenTL }
                };
                MyConnection.ExecuteNonQuery(query, parameters);
            }
            catch { throw; }
        }

        // SuaTheLoai
        public void Sua(TheLoaiDTO ds)
        {
            try
            {
                const string query = "UPDATE TheLoai SET TenTL=@TenTL WHERE MaTL=@MaTL";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenTL", SqlDbType.NVarChar) { Value = ds.TenTL },
                    new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = ds.MaTL }
                };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy thể loại cần cập nhật.");
            }
            catch { throw; }
        }

        // XoaTheLoai (Đã sửa MaTG thành MaTL)
        public void Xoa(string maTL)
        {
            try
            {
                const string query = "DELETE FROM TheLoai WHERE MaTL = @MaTL";
                SqlParameter[] parameters = { new SqlParameter("@MaTL", SqlDbType.VarChar) { Value = maTL } };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy thể loại cần xóa.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa thể loại vì thể loại này đang được dùng trong danh sách đầu sách.", ex);
            }
        }


    }
}
