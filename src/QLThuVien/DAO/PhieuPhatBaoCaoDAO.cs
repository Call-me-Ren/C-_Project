using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class PhieuPhatBaoCaoDAO
    {
        public PhieuPhatBaoCaoDTO? LayTheoMa(string maPhieuPhat)
        {
            string queryPhieu = @"SELECT pp.MaPhieuPhat, pp.NgayLap, pp.MaDocGia,
                                         CONCAT(COALESCE(dg.HoLot + N' ', N''), dg.TenDG) AS TenDocGia,
                                         pp.MaNV, CONCAT(COALESCE(nv.HoLot + N' ', N''), nv.TenNV) AS TenNhanVien,
                                         pp.MaPhieuMuon, pp.TrangThai, pp.TongTienPhat
                                  FROM PhieuPhat pp
                                  INNER JOIN DocGia dg ON dg.MaDG = pp.MaDocGia
                                  INNER JOIN NhanVien nv ON nv.MaNV = pp.MaNV
                                  WHERE pp.MaPhieuPhat = @MaPhieuPhat";
            DataTable header = MyConnection.ExecuteQuery(queryPhieu, TaoThamSo(maPhieuPhat));
            if (header.Rows.Count == 0) return null;
            DataRow row = header.Rows[0];
            PhieuPhatBaoCaoDTO report = new PhieuPhatBaoCaoDTO
            {
                MaPhieuPhat = row["MaPhieuPhat"].ToString() ?? string.Empty,
                NgayLap = Convert.ToDateTime(row["NgayLap"]),
                MaDocGia = row["MaDocGia"].ToString() ?? string.Empty,
                TenDocGia = row["TenDocGia"].ToString() ?? string.Empty,
                MaNV = row["MaNV"].ToString() ?? string.Empty,
                TenNhanVien = row["TenNhanVien"].ToString() ?? string.Empty,
                MaPhieuMuon = row["MaPhieuMuon"] == DBNull.Value ? null : row["MaPhieuMuon"].ToString(),
                TrangThai = row["TrangThai"] == DBNull.Value ? string.Empty : row["TrangThai"].ToString() ?? string.Empty,
                TongTienPhat = Convert.ToDecimal(row["TongTienPhat"])
            };

            string queryChiTiet = @"SELECT ct.MaSach, ds.TenSach, ct.MaQD, qd.TenQD, ct.SoNgayTre,
                                           ct.DonGia, ct.TienPhat, ct.GhiChu
                                    FROM ChiTietPhieuPhat ct
                                    INNER JOIN Sach s ON s.MaSach = ct.MaSach
                                    INNER JOIN DauSach ds ON ds.MaDauSach = s.MaDauSach
                                    INNER JOIN QuyDinhPhat qd ON qd.MaQD = ct.MaQD
                                    WHERE ct.MaPhieuPhat = @MaPhieuPhat
                                    ORDER BY ct.MaCTPP";
            DataTable details = MyConnection.ExecuteQuery(queryChiTiet, TaoThamSo(maPhieuPhat));
            foreach (DataRow detail in details.Rows)
            {
                report.ChiTiet.Add(new ChiTietPhieuPhatBaoCaoDTO
                {
                    MaSach = detail["MaSach"].ToString() ?? string.Empty,
                    TenSach = detail["TenSach"].ToString() ?? string.Empty,
                    MaQD = detail["MaQD"].ToString() ?? string.Empty,
                    TenQD = detail["TenQD"].ToString() ?? string.Empty,
                    SoNgayTre = detail["SoNgayTre"] == DBNull.Value ? null : Convert.ToInt32(detail["SoNgayTre"]),
                    DonGia = Convert.ToDecimal(detail["DonGia"]),
                    TienPhat = Convert.ToDecimal(detail["TienPhat"]),
                    GhiChu = detail["GhiChu"] == DBNull.Value ? null : detail["GhiChu"].ToString()
                });
            }
            return report;
        }

        private static SqlParameter[] TaoThamSo(string maPhieuPhat)
        {
            return new[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };
        }
    }
}
