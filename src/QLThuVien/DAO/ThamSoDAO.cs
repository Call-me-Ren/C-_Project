using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Data Access Object cho bảng ThamSo
    /// </summary>
    public class ThamSoDAO
    {
        public List<ThamSoDTO> DocDS()
        {
            List<ThamSoDTO> list = new List<ThamSoDTO>();
            string query = "SELECT MaTS, TenTS, GiaTri FROM ThamSo ORDER BY MaTS";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ThamSoDTO(
                    row["MaTS"].ToString() ?? "",
                    row["TenTS"].ToString() ?? "",
                    row["GiaTri"].ToString() ?? ""
                ));
            }

            return list;
        }

        public ThamSoDTO? LayTheoMa(string maTS)
        {
            string query = "SELECT MaTS, TenTS, GiaTri FROM ThamSo WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTS", SqlDbType.VarChar) { Value = maTS }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new ThamSoDTO(
                    row["MaTS"].ToString() ?? "",
                    row["TenTS"].ToString() ?? "",
                    row["GiaTri"].ToString() ?? ""
                );
            }

            return null;
        }

        public bool CapNhatGiaTri(string maTS, string giaTriMoi)
        {
            string query = "UPDATE ThamSo SET GiaTri = @GiaTri WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GiaTri", SqlDbType.NVarChar) { Value = giaTriMoi },
                new SqlParameter("@MaTS", SqlDbType.VarChar) { Value = maTS }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }
    }
}
