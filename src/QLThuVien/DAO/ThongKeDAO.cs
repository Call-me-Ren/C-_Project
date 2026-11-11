using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class ThongKeDAO
    {
        public List<ThongKeGiaoDichDTO> LayGiaoDichThuChi(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"SELECT N'Thu' AS Loai, N'Thu tiền phạt' AS DienGiai, pp.MaPhieuPhat AS MaGiaoDich, pp.NgayLap AS Ngay,
                                    pp.MaNV, CONCAT(pp.MaDocGia, N' - ', COALESCE(dg.HoLot + N' ', N''), dg.TenDG) AS DoiTuong,
                                    pp.TongTienPhat AS SoTien
                             FROM PhieuPhat pp
                             INNER JOIN DocGia dg ON dg.MaDG = pp.MaDocGia
                             WHERE pp.TrangThai = N'Đã thanh toán' AND pp.NgayLap >= @TuNgay AND pp.NgayLap < @DenNgay
                             UNION ALL
                              SELECT N'Chi', N'Nhập sách', pn.MaPN, pn.NgayNhap, pn.MaNV,
                                    CONCAT(pn.MaNCC, N' - ', ncc.TenNCC), pn.TongTien
                             FROM PhieuNhap pn
                             INNER JOIN NhaCungCap ncc ON ncc.MaNCC = pn.MaNCC
                             WHERE pn.NgayNhap >= @TuNgay AND pn.NgayNhap < @DenNgay
                             ORDER BY Ngay DESC, MaGiaoDich";
            DataTable table = MyConnection.ExecuteQuery(query, TaoThamSoNgay(tuNgay, denNgay));
            List<ThongKeGiaoDichDTO> result = new List<ThongKeGiaoDichDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKeGiaoDichDTO
                {
                    Loai = row["Loai"].ToString() ?? string.Empty,
                    DienGiai = row["DienGiai"].ToString() ?? string.Empty,
                    MaGiaoDich = row["MaGiaoDich"].ToString() ?? string.Empty,
                    Ngay = Convert.ToDateTime(row["Ngay"]),
                    MaNV = row["MaNV"].ToString(),
                    DoiTuong = row["DoiTuong"].ToString() ?? string.Empty,
                    SoTien = Convert.ToDecimal(row["SoTien"])
                });
            }
            return result;
        }

        public List<ThongKePhieuPhatDTO> LayPhieuPhat(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"SELECT MaPhieuPhat, MaDocGia, MaNV, NgayLap, TrangThai, TongTienPhat
                             FROM PhieuPhat
                             WHERE NgayLap >= @TuNgay AND NgayLap < @DenNgay";
            DataTable table = MyConnection.ExecuteQuery(query, TaoThamSoNgay(tuNgay, denNgay));
            List<ThongKePhieuPhatDTO> result = new List<ThongKePhieuPhatDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKePhieuPhatDTO
                {
                    MaPhieuPhat = row["MaPhieuPhat"].ToString() ?? string.Empty,
                    MaDocGia = row["MaDocGia"].ToString() ?? string.Empty,
                    MaNV = row["MaNV"].ToString() ?? string.Empty,
                    NgayLap = Convert.ToDateTime(row["NgayLap"]),
                    TrangThai = row["TrangThai"] == DBNull.Value ? string.Empty : row["TrangThai"].ToString() ?? string.Empty,
                    TongTienPhat = Convert.ToDecimal(row["TongTienPhat"])
                });
            }
            return result;
        }

        public List<ThongKePhieuNhapDTO> LayPhieuNhap(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"SELECT MaPN, MaNV, NgayNhap, TongTien
                             FROM PhieuNhap
                             WHERE NgayNhap >= @TuNgay AND NgayNhap < @DenNgay";
            DataTable table = MyConnection.ExecuteQuery(query, TaoThamSoNgay(tuNgay, denNgay));
            List<ThongKePhieuNhapDTO> result = new List<ThongKePhieuNhapDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKePhieuNhapDTO
                {
                    MaPN = row["MaPN"].ToString() ?? string.Empty,
                    MaNV = row["MaNV"].ToString() ?? string.Empty,
                    NgayNhap = Convert.ToDateTime(row["NgayNhap"]),
                    TongTien = Convert.ToDecimal(row["TongTien"])
                });
            }
            return result;
        }

        private static SqlParameter[] TaoThamSoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return new[]
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime) { Value = tuNgay.Date },
                new SqlParameter("@DenNgay", SqlDbType.DateTime) { Value = denNgay.Date.AddDays(1) }
            };
        }
    }
}
