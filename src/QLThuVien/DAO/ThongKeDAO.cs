using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>Lớp DAO truy vấn giao dịch thu-chi và dữ liệu nguồn cho các bảng thống kê theo quý.</summary>
    public class ThongKeDAO
    {
        /// <summary>Lấy giao dịch thu tiền phạt đã thanh toán và chi phí nhập sách trong khoảng ngày.</summary>
        /// <param name="tuNgay">Ngày bắt đầu; được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc; được tính gồm cả ngày này.</param>
        /// <returns>Danh sách giao dịch thu và chi; rỗng nếu khoảng thời gian không có giao dịch.</returns>
        /// <exception cref="SqlException">Truy vấn hoặc ánh xạ dữ liệu cơ sở dữ liệu thất bại.</exception>
        /// <remarks>Khoản thu chỉ gồm phiếu phạt đã thanh toán; khoản chi gồm phiếu nhập sách. Truy vấn dùng khoảng nửa mở từ ngày bắt đầu đến đầu ngày kế tiếp ngày kết thúc.</remarks>
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

        /// <summary>Lấy các phiếu phạt theo ngày lập để tổng hợp theo nhân viên, độc giả và quý.</summary>
        /// <param name="tuNgay">Ngày bắt đầu; được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc; được tính gồm cả ngày này.</param>
        /// <returns>Danh sách phiếu nằm trong khoảng ngày; rỗng nếu không có phiếu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        /// <remarks>Kết quả giữ cả trạng thái chưa thanh toán vì bảng quỹ tổng hợp tiền theo phiếu lập trong kỳ.</remarks>
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

        /// <summary>Lấy các phiếu nhập theo ngày nhập để tổng hợp chi phí theo nhân viên và quý.</summary>
        /// <param name="tuNgay">Ngày bắt đầu; được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc; được tính gồm cả ngày này.</param>
        /// <returns>Danh sách phiếu nhập trong khoảng ngày; rỗng nếu không có phiếu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Lấy mã và tên các đầu sách dùng làm dòng trong bảng thống kê theo quý.</summary>
        /// <returns>Danh sách đầu sách theo mã; rỗng nếu danh mục chưa có dữ liệu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<ThongKeDauSachQuyDTO> LayDanhSachDauSach()
        {
            string query = "SELECT MaDauSach, TenSach AS TenDauSach FROM DauSach ORDER BY MaDauSach";
            DataTable table = MyConnection.ExecuteQuery(query);
            List<ThongKeDauSachQuyDTO> result = new List<ThongKeDauSachQuyDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKeDauSachQuyDTO
                {
                    MaDauSach = row["MaDauSach"].ToString() ?? string.Empty,
                    TenDauSach = row["TenDauSach"].ToString() ?? string.Empty
                });
            }
            return result;
        }

        /// <summary>Lấy thành tiền từng dòng nhập sách trong khoảng ngày để phân nhóm theo đầu sách.</summary>
        /// <param name="tuNgay">Ngày bắt đầu; được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc; được tính gồm cả ngày này.</param>
        /// <returns>Các dòng nhập gồm mã đầu sách, ngày nhập và thành tiền VNĐ.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<ThongKeNhapDauSachDTO> LayTienNhapTheoDauSach(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"SELECT ct.MaDauSach, pn.NgayNhap, ct.ThanhTien
                             FROM ChiTietPhieuNhap ct
                             INNER JOIN PhieuNhap pn ON pn.MaPN = ct.MaPN
                             WHERE pn.NgayNhap >= @TuNgay AND pn.NgayNhap < @DenNgay";
            DataTable table = MyConnection.ExecuteQuery(query, TaoThamSoNgay(tuNgay, denNgay));
            List<ThongKeNhapDauSachDTO> result = new List<ThongKeNhapDauSachDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKeNhapDauSachDTO
                {
                    MaDauSach = row["MaDauSach"].ToString() ?? string.Empty,
                    NgayNhap = Convert.ToDateTime(row["NgayNhap"]),
                    ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                });
            }
            return result;
        }

        /// <summary>Lấy ngày mượn và mã đầu sách của từng cuốn đã được ghi vào phiếu mượn trong khoảng ngày.</summary>
        /// <param name="tuNgay">Ngày bắt đầu; được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc; được tính gồm cả ngày này.</param>
        /// <returns>Các lượt mượn theo đầu sách; rỗng nếu không có lượt trong kỳ.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<ThongKeLuotMuonDauSachDTO> LayLuotMuonTheoDauSach(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"SELECT s.MaDauSach, pm.NgayMuon
                             FROM ChiTietPhieuMuon ct
                             INNER JOIN PhieuMuon pm ON pm.MaPM = ct.MaPhieuMuon
                             INNER JOIN Sach s ON s.MaSach = ct.MaSach
                             WHERE pm.NgayMuon >= @TuNgay AND pm.NgayMuon < @DenNgay";
            DataTable table = MyConnection.ExecuteQuery(query, TaoThamSoNgay(tuNgay, denNgay));
            List<ThongKeLuotMuonDauSachDTO> result = new List<ThongKeLuotMuonDauSachDTO>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ThongKeLuotMuonDauSachDTO
                {
                    MaDauSach = row["MaDauSach"].ToString() ?? string.Empty,
                    NgayMuon = Convert.ToDateTime(row["NgayMuon"])
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
