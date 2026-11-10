#nullable disable
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

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
                string query = "INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChi) VALUES ('"
                    + ds.MaNXB + "', N'"
                    + ds.TenNXB + "', N'"
                    + ds.DiaChi + "')";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- ĐÃ THÊM OPEN KẾT NỐI
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                }
            }
            catch { throw; }
        }

        // SuaNXB (Đã sửa tên bảng thành NhaXuatBan)
        public void Sua(NXBDTO ds)
        {
            try
            {
                string query = "UPDATE NhaXuatBan SET TenNXB=N'" + ds.TenNXB
                    + "', DiaChi=N'" + ds.DiaChi
                    + "' WHERE MaNXB='" + ds.MaNXB + "'";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open(); // <-- ĐÃ THÊM OPEN KẾT NỐI
                    SqlCommand command = new SqlCommand(query, conn);
                    int r = command.ExecuteNonQuery();
                    if (r == 0) throw new InvalidOperationException("Không tìm thấy nhà xuất bản cần cập nhật.");
                }
            }
            catch { throw; }
        }

        // XoaNXB (Đã sửa tên bảng thành NhaXuatBan)
        public void Xoa(string maNXB)
        {
            try
            {
                const string query = "DELETE FROM NhaXuatBan WHERE MaNXB = @MaNXB";
                using (SqlConnection conn = MyConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@MaNXB", SqlDbType.VarChar).Value = maNXB;
                    conn.Open();
                    if (command.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("Không tìm thấy nhà xuất bản cần xóa.");
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa nhà xuất bản vì nhà xuất bản này đang được dùng trong danh sách đầu sách.", ex);
            }
        }


    }
}
