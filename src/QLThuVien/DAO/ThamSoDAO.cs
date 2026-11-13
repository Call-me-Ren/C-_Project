using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// DAO truy vấn bảng ThamSo (bảng cấu hình hệ thống).
    /// </summary>
    public class ThamSoDAO
    {
        public static string LayGiaTriTheoMa(string maTS, string macDinh = "")
        {
            try
            {
                string query = "SELECT GiaTri FROM ThamSo WHERE MaTS = @MaTS";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaTS", SqlDbType.VarChar, 50) { Value = maTS }
                };

                object? result = MyConnection.ExecuteScalar(query, parameters);
                return result != null && result != DBNull.Value ? result.ToString() ?? macDinh : macDinh;
            }
            catch
            {
                return macDinh;
            }
        }

        public List<ThamSoDTO> DocDSThamSo()
        {
            List<ThamSoDTO> list = new List<ThamSoDTO>();
            string query = "SELECT MaTS, TenTS, GiaTri FROM ThamSo ORDER BY MaTS";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                ThamSoDTO ts = new ThamSoDTO(
                    row["MaTS"].ToString() ?? string.Empty,
                    row["TenTS"].ToString() ?? string.Empty,
                    row["GiaTri"].ToString() ?? string.Empty
                );
                list.Add(ts);
            }

            return list;
        }

        public bool CapNhatGiaTri(string maTS, string giaTri)
        {
            string query = "UPDATE ThamSo SET GiaTri = @GiaTri WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTS", SqlDbType.VarChar, 50) { Value = maTS },
                new SqlParameter("@GiaTri", SqlDbType.NVarChar, 100) { Value = giaTri }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool ThemThamSo(ThamSoDTO ts)
        {
            string query = "INSERT INTO ThamSo (MaTS, TenTS, GiaTri) VALUES (@MaTS, @TenTS, @GiaTri)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTS", SqlDbType.VarChar, 50) { Value = ts.MaTS },
                new SqlParameter("@TenTS", SqlDbType.NVarChar, 100) { Value = ts.TenTS },
                new SqlParameter("@GiaTri", SqlDbType.NVarChar, 100) { Value = ts.GiaTri }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool XoaThamSo(string maTS)
        {
            string query = "DELETE FROM ThamSo WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTS", SqlDbType.VarChar, 50) { Value = maTS }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }
    }
}
