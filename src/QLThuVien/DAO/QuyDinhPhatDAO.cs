using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class QuyDinhPhatDAO
    {
        public List<QuyDinhPhatDTO> LayDanhSach()
        {
            List<QuyDinhPhatDTO> list = new List<QuyDinhPhatDTO>();
            string query = "SELECT MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat FROM QuyDinhPhat ORDER BY MaQD ASC";
            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                QuyDinhPhatDTO qd = new QuyDinhPhatDTO(
                    row["MaQD"].ToString() ?? string.Empty,
                    row["TenQD"].ToString() ?? string.Empty,
                    row["LoaiPhat"] == DBNull.Value ? null : row["LoaiPhat"].ToString(),
                    row["CachTinh"] == DBNull.Value ? null : row["CachTinh"].ToString(),
                    row["DonViTinh"] == DBNull.Value ? null : row["DonViTinh"].ToString(),
                    row["MucPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["MucPhat"])
                );
                list.Add(qd);
            }

            return list;
        }

        public QuyDinhPhatDTO? LayTheoMa(string maQD)
        {
            string query = "SELECT MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new QuyDinhPhatDTO(
                    row["MaQD"].ToString() ?? string.Empty,
                    row["TenQD"].ToString() ?? string.Empty,
                    row["LoaiPhat"] == DBNull.Value ? null : row["LoaiPhat"].ToString(),
                    row["CachTinh"] == DBNull.Value ? null : row["CachTinh"].ToString(),
                    row["DonViTinh"] == DBNull.Value ? null : row["DonViTinh"].ToString(),
                    row["MucPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["MucPhat"])
                );
            }

            return null;
        }

        public bool Them(QuyDinhPhatDTO qd)
        {
            string query = @"INSERT INTO QuyDinhPhat (MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat)
                             VALUES (@MaQD, @TenQD, @LoaiPhat, @CachTinh, @DonViTinh, @MucPhat)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = qd.MaQD },
                new SqlParameter("@TenQD", SqlDbType.NVarChar, 100) { Value = qd.TenQD },
                new SqlParameter("@LoaiPhat", SqlDbType.NVarChar, 50) { Value = (object?)qd.LoaiPhat ?? DBNull.Value },
                new SqlParameter("@CachTinh", SqlDbType.NVarChar, 100) { Value = (object?)qd.CachTinh ?? DBNull.Value },
                new SqlParameter("@DonViTinh", SqlDbType.NVarChar, 20) { Value = (object?)qd.DonViTinh ?? DBNull.Value },
                new SqlParameter("@MucPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = qd.MucPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Sua(QuyDinhPhatDTO qd)
        {
            string query = @"UPDATE QuyDinhPhat
                             SET TenQD = @TenQD, LoaiPhat = @LoaiPhat, CachTinh = @CachTinh, DonViTinh = @DonViTinh, MucPhat = @MucPhat
                             WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = qd.MaQD },
                new SqlParameter("@TenQD", SqlDbType.NVarChar, 100) { Value = qd.TenQD },
                new SqlParameter("@LoaiPhat", SqlDbType.NVarChar, 50) { Value = (object?)qd.LoaiPhat ?? DBNull.Value },
                new SqlParameter("@CachTinh", SqlDbType.NVarChar, 100) { Value = (object?)qd.CachTinh ?? DBNull.Value },
                new SqlParameter("@DonViTinh", SqlDbType.NVarChar, 20) { Value = (object?)qd.DonViTinh ?? DBNull.Value },
                new SqlParameter("@MucPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = qd.MucPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Xoa(string maQD)
        {
            string query = "DELETE FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool KiemTraTonTai(string maQD)
        {
            string query = "SELECT COUNT(1) FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool KiemTraKhoaNgoai(string maQD)
        {
            string query = "SELECT COUNT(1) FROM ChiTietPhieuPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }
    }
}
