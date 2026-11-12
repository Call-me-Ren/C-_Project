#nullable disable
using QLThuVien.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLThuVien.DAO
{
    public class NXBDAO
    {
        // DOCDSNXB
        public List<NXBDTO> DocDSNXB()
        {
            List<NXBDTO> dsNXB = new List<NXBDTO>();
            string query = @"SELECT * FROM NhaXuatBan"; // Tên bảng chuẩn trong SQL là NhaXuatBan

            using (SqlDataReader reader = MyConnection.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    NXBDTO ds = new NXBDTO
                    {
                        MaNXB = reader["MaNXB"].ToString(),
                        TenNXB = reader["TenNXB"].ToString(),
                        DiaChi = reader["DiaChi"] != DBNull.Value ? reader["DiaChi"].ToString() : ""
                    };
                    dsNXB.Add(ds);
                }
            }
            return dsNXB;
        }

        // ThemNXB (Đã sửa tên bảng NXB thành NhaXuatBan và thêm conn.Open())
        public void Them(NXBDTO ds)
        {
            try
            {
                const string query = "INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChi) VALUES (@MaNXB, @TenNXB, @DiaChi)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaNXB", SqlDbType.VarChar) { Value = ds.MaNXB },
                    new SqlParameter("@TenNXB", SqlDbType.NVarChar) { Value = ds.TenNXB },
                    new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = ds.DiaChi }
                };
                MyConnection.ExecuteNonQuery(query, parameters);
            }
            catch { throw; }
        }

        // SuaNXB (Đã sửa tên bảng thành NhaXuatBan)
        public void Sua(NXBDTO ds)
        {
            try
            {
                const string query = "UPDATE NhaXuatBan SET TenNXB=@TenNXB, DiaChi=@DiaChi WHERE MaNXB=@MaNXB";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TenNXB", SqlDbType.NVarChar) { Value = ds.TenNXB },
                    new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = ds.DiaChi },
                    new SqlParameter("@MaNXB", SqlDbType.VarChar) { Value = ds.MaNXB }
                };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy nhà xuất bản cần cập nhật.");
            }
            catch { throw; }
        }

        // XoaNXB (Đã sửa tên bảng thành NhaXuatBan)
        public void Xoa(string maNXB)
        {
            try
            {
                const string query = "DELETE FROM NhaXuatBan WHERE MaNXB = @MaNXB";
                SqlParameter[] parameters = { new SqlParameter("@MaNXB", SqlDbType.VarChar) { Value = maNXB } };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy nhà xuất bản cần xóa.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa nhà xuất bản vì nhà xuất bản này đang được dùng trong danh sách đầu sách.", ex);
            }
        }


    }
}
