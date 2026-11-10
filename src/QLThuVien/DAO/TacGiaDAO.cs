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
            catch { throw; }
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
                    if (r == 0) throw new InvalidOperationException("Không tìm thấy tác giả cần cập nhật.");
                }
            }
            catch { throw; }
        }

        //xoaTG
        public void Xoa(string maTG)
        {
            try
            {
                const string query = "DELETE FROM TacGia WHERE MaTG = @MaTG";
                using (SqlConnection conn = MyConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@MaTG", SqlDbType.VarChar).Value = maTG;
                    conn.Open();
                    if (command.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("Không tìm thấy tác giả cần xóa.");
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa tác giả vì tác giả này đang được dùng trong danh sách đầu sách.", ex);
            }
        }

    }
}
