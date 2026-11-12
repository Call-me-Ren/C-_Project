#nullable disable
using QLThuVien.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
                const string query = "INSERT INTO TacGia (MaTG, HoLot, TenTG, NgaySinh, SDT) VALUES (@MaTG, @HoLot, @TenTG, @NgaySinh, @SDT)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaTG", SqlDbType.VarChar) { Value = ds.MaTG },
                    new SqlParameter("@HoLot", SqlDbType.NVarChar) { Value = ds.HoLot },
                    new SqlParameter("@TenTG", SqlDbType.NVarChar) { Value = ds.TenTG },
                    new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = ds.NgaySinh.HasValue ? (object)ds.NgaySinh.Value : DBNull.Value },
                    new SqlParameter("@SDT", SqlDbType.VarChar) { Value = ds.SDT }
                };
                MyConnection.ExecuteNonQuery(query, parameters);
            }
            catch { throw; }
        }

        //suaTG
        public void Sua(TacGiaDTO ds)
        {
            try
            {
                const string query = "UPDATE TacGia SET HoLot=@HoLot, TenTG=@TenTG, NgaySinh=@NgaySinh, SDT=@SDT WHERE MaTG=@MaTG";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@HoLot", SqlDbType.NVarChar) { Value = ds.HoLot },
                    new SqlParameter("@TenTG", SqlDbType.NVarChar) { Value = ds.TenTG },
                    new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = ds.NgaySinh.HasValue ? (object)ds.NgaySinh.Value : DBNull.Value },
                    new SqlParameter("@SDT", SqlDbType.VarChar) { Value = ds.SDT },
                    new SqlParameter("@MaTG", SqlDbType.VarChar) { Value = ds.MaTG }
                };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy tác giả cần cập nhật.");
            }
            catch { throw; }
        }

        //xoaTG
        public void Xoa(string maTG)
        {
            try
            {
                const string query = "DELETE FROM TacGia WHERE MaTG = @MaTG";
                SqlParameter[] parameters = { new SqlParameter("@MaTG", SqlDbType.VarChar) { Value = maTG } };
                if (MyConnection.ExecuteNonQuery(query, parameters) == 0)
                    throw new InvalidOperationException("Không tìm thấy tác giả cần xóa.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa tác giả vì tác giả này đang được dùng trong danh sách đầu sách.", ex);
            }
        }

    }
}
