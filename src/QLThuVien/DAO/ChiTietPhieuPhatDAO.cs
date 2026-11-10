using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class ChiTietPhieuPhatDAO
    {
        private const string CacCot = "MaCTPP, MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat";

        public List<ChiTietPhieuPhatDTO> LayTatCa()
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat ORDER BY MaPhieuPhat, MaCTPP";
            return DocDanhSach(query);
        }

        public List<ChiTietPhieuPhatDTO> LayDanhSachTheoPhieu(string maPhieuPhat)
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat ORDER BY MaCTPP";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };
            return DocDanhSach(query, parameters);
        }

        public ChiTietPhieuPhatDTO? LayTheoMaCTPP(int maCTPP)
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat WHERE MaCTPP = @MaCTPP";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = maCTPP }
            };
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            return dt.Rows.Count == 0 ? null : DocDongDuLieu(dt.Rows[0]);
        }

        public int Them(ChiTietPhieuPhatDTO ct)
        {
            string query = @"INSERT INTO ChiTietPhieuPhat (MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat)
                             OUTPUT INSERTED.MaCTPP
                             VALUES (@MaPhieuPhat, @MaQD, @MaSach, @DonGia, @SoNgayTre, @GhiChu, @TienPhat)";
            object? result = MyConnection.ExecuteScalar(query, TaoThamSo(ct));
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        public bool Sua(ChiTietPhieuPhatDTO ct)
        {
            string query = @"UPDATE ChiTietPhieuPhat
                             SET MaQD = @MaQD, MaSach = @MaSach, DonGia = @DonGia,
                                 SoNgayTre = @SoNgayTre, GhiChu = @GhiChu, TienPhat = @TienPhat
                             WHERE MaCTPP = @MaCTPP";
            List<SqlParameter> parameters = new List<SqlParameter>(TaoThamSo(ct))
            {
                new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = ct.MaCTPP }
            };
            return MyConnection.ExecuteNonQuery(query, parameters.ToArray()) > 0;
        }

        public bool Xoa(int maCTPP)
        {
            string query = "DELETE FROM ChiTietPhieuPhat WHERE MaCTPP = @MaCTPP";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = maCTPP }
            };
            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool KiemTraTrung(string maPhieuPhat, string maSach, string maQD, int? boQuaMaCTPP = null)
        {
            string query = @"SELECT COUNT(1)
                             FROM ChiTietPhieuPhat
                             WHERE MaPhieuPhat = @MaPhieuPhat
                               AND MaQD = @MaQD
                               AND MaSach = @MaSach
                               AND (@MaCTPPBoQua IS NULL OR MaCTPP <> @MaCTPPBoQua)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach },
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD },
                new SqlParameter("@MaCTPPBoQua", SqlDbType.Int) { Value = (object?)boQuaMaCTPP ?? DBNull.Value }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public List<SachMuonDTO> LaySachTheoPhieuMuon(string maPhieuMuon)
        {
            string query = @"SELECT cpm.MaSach, ds.TenSach, pm.HanTra, cpm.NgayThucTra
                             FROM PhieuMuon pm
                             INNER JOIN ChiTietPhieuMuon cpm ON cpm.MaPhieuMuon = pm.MaPM
                             INNER JOIN Sach s ON s.MaSach = cpm.MaSach
                             INNER JOIN DauSach ds ON ds.MaDauSach = s.MaDauSach
                             WHERE pm.MaPM = @MaPhieuMuon
                             ORDER BY cpm.MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuMuon", SqlDbType.VarChar, 20) { Value = maPhieuMuon }
            };
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            List<SachMuonDTO> list = new List<SachMuonDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SachMuonDTO(
                    row["MaSach"].ToString() ?? string.Empty,
                    row["TenSach"].ToString() ?? string.Empty,
                    Convert.ToDateTime(row["HanTra"]),
                    row["NgayThucTra"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayThucTra"])));
            }
            return list;
        }

        public decimal LayDonGiaSach(string maSach)
        {
            string query = "SELECT ISNULL(ds.DonGia, 0) FROM Sach s JOIN DauSach ds ON s.MaDauSach = ds.MaDauSach WHERE s.MaSach = @MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public bool KiemTraSachTonTai(string maSach)
        {
            string query = "SELECT COUNT(1) FROM Sach WHERE MaSach = @MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        private static SqlParameter[] TaoThamSo(ChiTietPhieuPhatDTO ct)
        {
            return new[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = ct.MaPhieuPhat },
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = ct.MaQD },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = ct.MaSach },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ct.DonGia },
                new SqlParameter("@SoNgayTre", SqlDbType.Int) { Value = (object?)ct.SoNgayTre ?? DBNull.Value },
                new SqlParameter("@GhiChu", SqlDbType.NVarChar, 200) { Value = (object?)ct.GhiChu ?? DBNull.Value },
                new SqlParameter("@TienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ct.TienPhat }
            };
        }

        private static List<ChiTietPhieuPhatDTO> DocDanhSach(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            List<ChiTietPhieuPhatDTO> list = new List<ChiTietPhieuPhatDTO>();
            foreach (DataRow row in dt.Rows) list.Add(DocDongDuLieu(row));
            return list;
        }

        private static ChiTietPhieuPhatDTO DocDongDuLieu(DataRow row)
        {
            return new ChiTietPhieuPhatDTO(
                row["MaPhieuPhat"].ToString() ?? string.Empty,
                row["MaQD"].ToString() ?? string.Empty,
                row["MaSach"].ToString() ?? string.Empty,
                row["DonGia"] == DBNull.Value ? 0 : Convert.ToDecimal(row["DonGia"]),
                row["SoNgayTre"] == DBNull.Value ? null : Convert.ToInt32(row["SoNgayTre"]),
                row["GhiChu"] == DBNull.Value ? null : row["GhiChu"].ToString(),
                row["TienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TienPhat"]),
                Convert.ToInt32(row["MaCTPP"]));
        }
    }
}
