using System;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>Lớp BUS kiểm tra kỳ thống kê, tổng hợp giao dịch và xây dựng các bảng quỹ từ lớp DAO.</summary>
    public class ThongKeBUS
    {
        private readonly ThongKeDAO dao = new ThongKeDAO();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly DocGiaBUS busDocGia = new DocGiaBUS();

        /// <summary>Kiểm tra khoảng ngày báo cáo theo ngày lịch và không nhận ngày tương lai.</summary>
        /// <param name="tuNgay">Ngày bắt đầu của kỳ thống kê.</param>
        /// <param name="denNgay">Ngày kết thúc của kỳ thống kê.</param>
        /// <exception cref="BusinessRuleException">Ngày bắt đầu sau ngày kết thúc hoặc một trong hai ngày ở tương lai.</exception>
        public static void KiemTraKhoangNgay(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
                throw new BusinessRuleException("Từ ngày phải trước hoặc bằng Đến ngày.");
            if (tuNgay.Date > DateTime.Today || denNgay.Date > DateTime.Today)
                throw new BusinessRuleException("Ngày thống kê không được ở tương lai.");
        }

        /// <summary>Lấy danh sách giao dịch và tính tổng thu, tổng chi trong khoảng ngày.</summary>
        /// <param name="tuNgay">Ngày bắt đầu, được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc, được tính gồm cả ngày này.</param>
        /// <returns>DTO gồm các giao dịch và tổng tiền VNĐ; tổng bằng 0 nếu không có giao dịch tương ứng.</returns>
        /// <exception cref="BusinessRuleException">Khoảng ngày không hợp lệ hoặc chứa ngày tương lai.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Truy vấn giao dịch thu-chi thất bại.</exception>
        /// <remarks>Tiền thu là tiền phạt đã thanh toán; tiền chi là tiền nhập sách theo dữ liệu DAO. Ví dụ 30.000 VNĐ thu và 20.000 VNĐ chi cho chênh lệch 10.000 VNĐ.</remarks>
        public ThongKeTongHopDTO LayTongHop(DateTime tuNgay, DateTime denNgay)
        {
            KiemTraKhoangNgay(tuNgay, denNgay);
            List<ThongKeGiaoDichDTO> giaoDich = dao.LayGiaoDichThuChi(tuNgay, denNgay);
            return new ThongKeTongHopDTO
            {
                TongThu = giaoDich.Where(x => x.Loai == "Thu").Sum(x => x.SoTien),
                TongChi = giaoDich.Where(x => x.Loai == "Chi").Sum(x => x.SoTien),
                GiaoDich = giaoDich
            };
        }

        /// <summary>Tổng hợp tiền phạt và tiền nhập sách theo từng nhân viên trong bốn quý của năm.</summary>
        /// <param name="nam">Năm dương lịch được thống kê, từ 2020 đến năm hiện tại cộng một.</param>
        /// <returns>Mỗi nhân viên một dòng gồm tiền phạt và tiền nhập theo quý cùng tổng năm; rỗng nếu không có nhân viên.</returns>
        /// <exception cref="BusinessRuleException">Năm nằm ngoài phạm vi được hỗ trợ.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Không đọc được nhân viên, phiếu phạt hoặc phiếu nhập.</exception>
        /// <remarks>Tiền phạt được nhóm theo ngày lập phiếu, tiền nhập theo ngày nhập; các phiếu phạt được cộng dù trạng thái chưa thanh toán.</remarks>
        public List<ThongKeNhanVienQuyDTO> LayThongKeNhanVienTheoQuy(int nam)
        {
            KiemTraNamThongKe(nam);
            DateTime tuNgay = new DateTime(nam, 1, 1);
            DateTime denNgay = new DateTime(nam, 12, 31);
            List<ThongKePhieuPhatDTO> phieuPhat = dao.LayPhieuPhat(tuNgay, denNgay);
            List<ThongKePhieuNhapDTO> phieuNhap = dao.LayPhieuNhap(tuNgay, denNgay);

            return busNhanVien.LayDanhSach(reload: true)
                .Select(nv =>
                {
                    List<ThongKePhieuPhatDTO> phieuPhatNV = phieuPhat.Where(x => Same(x.MaNV, nv.MaNV)).ToList();
                    List<ThongKePhieuNhapDTO> phieuNhapNV = phieuNhap.Where(x => Same(x.MaNV, nv.MaNV)).ToList();
                    return new ThongKeNhanVienQuyDTO
                    {
                        MaNV = nv.MaNV,
                        TenNhanVien = nv.HoTen,
                        TienPhatQ1 = TinhTienPhatTheoQuy(phieuPhatNV, 1),
                        TienNhapQ1 = TinhTienNhapTheoQuy(phieuNhapNV, 1),
                        TienPhatQ2 = TinhTienPhatTheoQuy(phieuPhatNV, 2),
                        TienNhapQ2 = TinhTienNhapTheoQuy(phieuNhapNV, 2),
                        TienPhatQ3 = TinhTienPhatTheoQuy(phieuPhatNV, 3),
                        TienNhapQ3 = TinhTienNhapTheoQuy(phieuNhapNV, 3),
                        TienPhatQ4 = TinhTienPhatTheoQuy(phieuPhatNV, 4),
                        TienNhapQ4 = TinhTienNhapTheoQuy(phieuNhapNV, 4),
                        TongTienPhat = phieuPhatNV.Sum(x => x.TongTienPhat),
                        TongTienNhap = phieuNhapNV.Sum(x => x.TongTien)
                    };
                })
                .OrderBy(x => x.MaNV, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>Tổng hợp tiền phạt theo từng độc giả trong bốn quý của năm.</summary>
        /// <param name="nam">Năm dương lịch được thống kê, từ 2020 đến năm hiện tại cộng một.</param>
        /// <returns>Mỗi độc giả một dòng gồm tiền phạt từng quý và tổng năm; rỗng nếu không có độc giả.</returns>
        /// <exception cref="BusinessRuleException">Năm nằm ngoài phạm vi được hỗ trợ.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Không đọc được độc giả hoặc phiếu phạt.</exception>
        /// <remarks>Phiếu được nhóm theo ngày lập và vẫn được tính khi chưa thanh toán; tháng 1–3 thuộc quý 1, tháng 4–6 quý 2, tháng 7–9 quý 3 và tháng 10–12 quý 4.</remarks>
        public List<ThongKeDocGiaQuyDTO> LayThongKeDocGiaTheoQuy(int nam)
        {
            KiemTraNamThongKe(nam);
            DateTime tuNgay = new DateTime(nam, 1, 1);
            DateTime denNgay = new DateTime(nam, 12, 31);
            List<ThongKePhieuPhatDTO> phieuPhat = dao.LayPhieuPhat(tuNgay, denNgay);

            return busDocGia.DocDSDocGia().Cast<DocGiaDTO>()
                .Select(dg =>
                {
                    List<ThongKePhieuPhatDTO> phieuPhatDG = phieuPhat.Where(x => Same(x.MaDocGia, dg.MaDG)).ToList();
                    return new ThongKeDocGiaQuyDTO
                    {
                        MaDocGia = dg.MaDG,
                        TenDocGia = dg.HoTen,
                        TienPhatQ1 = TinhTienPhatTheoQuy(phieuPhatDG, 1),
                        TienPhatQ2 = TinhTienPhatTheoQuy(phieuPhatDG, 2),
                        TienPhatQ3 = TinhTienPhatTheoQuy(phieuPhatDG, 3),
                        TienPhatQ4 = TinhTienPhatTheoQuy(phieuPhatDG, 4),
                        TongCong = phieuPhatDG.Sum(x => x.TongTienPhat)
                    };
                })
                .OrderBy(x => x.MaDocGia, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>Tổng hợp tiền nhập và lượt mượn theo từng đầu sách trong bốn quý của năm.</summary>
        /// <param name="nam">Năm dương lịch được thống kê, từ 2020 đến năm hiện tại cộng một.</param>
        /// <returns>Mỗi đầu sách một dòng gồm tiền nhập VNĐ và lượt mượn theo quý cùng tổng năm.</returns>
        /// <exception cref="BusinessRuleException">Năm nằm ngoài phạm vi được hỗ trợ.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Không đọc được danh mục, dòng nhập hoặc lượt mượn.</exception>
        public List<ThongKeDauSachQuyDTO> LayThongKeDauSachTheoQuy(int nam)
        {
            KiemTraNamThongKe(nam);
            DateTime tuNgay = new DateTime(nam, 1, 1);
            DateTime denNgay = new DateTime(nam, 12, 31);
            return TongHopDauSachTheoQuy(
                dao.LayDanhSachDauSach(),
                dao.LayTienNhapTheoDauSach(tuNgay, denNgay),
                dao.LayLuotMuonTheoDauSach(tuNgay, denNgay),
                nam);
        }

        /// <summary>Gộp dữ liệu nhập và mượn theo mã đầu sách và chia số liệu vào bốn quý.</summary>
        /// <param name="danhSachDauSach">Danh mục đầu sách làm tập dòng đầu ra.</param>
        /// <param name="tienNhap">Các dòng nhập gồm đầu sách, ngày nhập và thành tiền VNĐ.</param>
        /// <param name="luotMuon">Các lượt mượn gồm đầu sách và ngày mượn.</param>
        /// <param name="nam">Năm dùng để loại dữ liệu ngoài kỳ, từ 2020 đến năm hiện tại cộng một.</param>
        /// <returns>Danh sách đã sắp xếp theo mã đầu sách với tổng nhập và lượt mượn từng quý.</returns>
        /// <exception cref="ArgumentNullException">Một trong các tập dữ liệu đầu vào là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Năm nằm ngoài phạm vi được hỗ trợ.</exception>
        /// <remarks>Ngày được chia theo tháng: 1–3 là Q1, 4–6 là Q2, 7–9 là Q3, 10–12 là Q4. Ví dụ thành tiền 25.000 VNĐ ngày 15/02 được cộng vào tiền nhập Q1.</remarks>
        public static List<ThongKeDauSachQuyDTO> TongHopDauSachTheoQuy(
            IEnumerable<ThongKeDauSachQuyDTO> danhSachDauSach,
            IEnumerable<ThongKeNhapDauSachDTO> tienNhap,
            IEnumerable<ThongKeLuotMuonDauSachDTO> luotMuon,
            int nam)
        {
            ArgumentNullException.ThrowIfNull(danhSachDauSach);
            ArgumentNullException.ThrowIfNull(tienNhap);
            ArgumentNullException.ThrowIfNull(luotMuon);
            KiemTraNamThongKe(nam);
            // Lọc theo năm trước khi gom theo đầu sách để dữ liệu đầu vào rộng hơn không làm sai kỳ báo cáo.
            List<ThongKeNhapDauSachDTO> nhapTrongNam = tienNhap.Where(x => x.NgayNhap.Year == nam).ToList();
            List<ThongKeLuotMuonDauSachDTO> muonTrongNam = luotMuon.Where(x => x.NgayMuon.Year == nam).ToList();

            return danhSachDauSach.Select(book =>
            {
                List<ThongKeNhapDauSachDTO> nhapTheoDauSach = nhapTrongNam
                    .Where(x => Same(x.MaDauSach, book.MaDauSach)).ToList();
                List<ThongKeLuotMuonDauSachDTO> muonTheoDauSach = muonTrongNam
                    .Where(x => Same(x.MaDauSach, book.MaDauSach)).ToList();
                return new ThongKeDauSachQuyDTO
                {
                    MaDauSach = book.MaDauSach,
                    TenDauSach = book.TenDauSach,
                    TienNhapQ1 = TinhTienNhapDauSachTheoQuy(nhapTheoDauSach, 1),
                    LuotMuonQ1 = DemLuotMuonTheoQuy(muonTheoDauSach, 1),
                    TienNhapQ2 = TinhTienNhapDauSachTheoQuy(nhapTheoDauSach, 2),
                    LuotMuonQ2 = DemLuotMuonTheoQuy(muonTheoDauSach, 2),
                    TienNhapQ3 = TinhTienNhapDauSachTheoQuy(nhapTheoDauSach, 3),
                    LuotMuonQ3 = DemLuotMuonTheoQuy(muonTheoDauSach, 3),
                    TienNhapQ4 = TinhTienNhapDauSachTheoQuy(nhapTheoDauSach, 4),
                    LuotMuonQ4 = DemLuotMuonTheoQuy(muonTheoDauSach, 4),
                    TongTienNhap = nhapTheoDauSach.Sum(x => x.ThanhTien),
                    TongLuotMuon = muonTheoDauSach.Count
                };
            }).OrderBy(x => x.MaDauSach, StringComparer.OrdinalIgnoreCase).ToList();
        }

        /// <summary>Kiểm tra năm thống kê có nằm trong phạm vi hỗ trợ hay không.</summary>
        /// <param name="nam">Năm dương lịch cần kiểm tra.</param>
        /// <exception cref="BusinessRuleException">Năm nhỏ hơn 2020 hoặc lớn hơn năm hiện tại cộng một.</exception>
        public static void KiemTraNamThongKe(int nam)
        {
            if (nam < 2020 || nam > DateTime.Today.Year + 1)
                throw new BusinessRuleException($"Năm thống kê phải từ 2020 đến {DateTime.Today.Year + 1}.");
        }

        /// <summary>Lập báo cáo gồm thu-chi theo ngày và các bảng quỹ theo năm.</summary>
        /// <param name="tuNgay">Ngày bắt đầu của phần thu-chi, được tính gồm cả ngày này.</param>
        /// <param name="denNgay">Ngày kết thúc của phần thu-chi, được tính gồm cả ngày này.</param>
        /// <param name="namThongKe">Năm áp dụng cho các bảng quỹ, trong phạm vi được hỗ trợ.</param>
        /// <returns>DTO báo cáo có tổng thu, tổng chi, giao dịch và dữ liệu thống kê theo quý.</returns>
        /// <exception cref="BusinessRuleException">Khoảng ngày hoặc năm thống kê không hợp lệ.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Một truy vấn nguồn cho báo cáo thất bại.</exception>
        /// <remarks>Kiểm tra năm trước, sau đó dựng phần thu-chi rồi lần lượt nạp quỹ nhân viên, quỹ độc giả và thống kê đầu sách.</remarks>
        public ThongKeTongHopDTO LayBaoCaoThuChi(DateTime tuNgay, DateTime denNgay, int namThongKe)
        {
            KiemTraNamThongKe(namThongKe);
            ThongKeTongHopDTO report = LayTongHop(tuNgay, denNgay);
            report.NamThongKe = namThongKe;
            report.ThongKeNhanVienTheoQuy = LayThongKeNhanVienTheoQuy(namThongKe);
            report.ThongKeDocGiaTheoQuy = LayThongKeDocGiaTheoQuy(namThongKe);
            report.ThongKeDauSachTheoQuy = LayThongKeDauSachTheoQuy(namThongKe);
            return report;
        }

        private static bool Same(string? left, string? right) =>
            string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

        /// <summary>Tính tổng tiền phạt các phiếu có ngày lập thuộc quý được chọn.</summary>
        /// <param name="rows">Các phiếu phạt đã giới hạn theo năm và đối tượng.</param>
        /// <param name="quy">Số quý từ 1 đến 4.</param>
        /// <returns>Tổng tiền phạt của các phiếu phù hợp, tính bằng VNĐ; bằng 0 nếu không có phiếu.</returns>
        private static decimal TinhTienPhatTheoQuy(IEnumerable<ThongKePhieuPhatDTO> rows, int quy) =>
            rows.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTienPhat);

        /// <summary>Tính tổng tiền nhập của các phiếu có ngày nhập thuộc quý được chọn.</summary>
        /// <param name="rows">Các phiếu nhập đã giới hạn theo năm và nhân viên.</param>
        /// <param name="quy">Số quý từ 1 đến 4.</param>
        /// <returns>Tổng tiền nhập, tính bằng VNĐ; bằng 0 nếu không có phiếu.</returns>
        private static decimal TinhTienNhapTheoQuy(IEnumerable<ThongKePhieuNhapDTO> rows, int quy) =>
            rows.Where(x => (x.NgayNhap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTien);

        /// <summary>Tính tổng thành tiền nhập của một đầu sách trong quý.</summary>
        /// <param name="rows">Các dòng nhập của cùng đầu sách trong năm thống kê.</param>
        /// <param name="quy">Số quý từ 1 đến 4.</param>
        /// <returns>Tổng thành tiền, tính bằng VNĐ; bằng 0 nếu không có dòng nhập.</returns>
        private static decimal TinhTienNhapDauSachTheoQuy(IEnumerable<ThongKeNhapDauSachDTO> rows, int quy) =>
            rows.Where(x => (x.NgayNhap.Month - 1) / 3 + 1 == quy).Sum(x => x.ThanhTien);

        /// <summary>Đếm số dòng mượn có ngày mượn thuộc quý được chọn.</summary>
        /// <param name="rows">Các lượt mượn của cùng đầu sách trong năm thống kê.</param>
        /// <param name="quy">Số quý từ 1 đến 4.</param>
        /// <returns>Số lượt mượn; bằng 0 nếu không có lượt trong quý.</returns>
        private static int DemLuotMuonTheoQuy(IEnumerable<ThongKeLuotMuonDauSachDTO> rows, int quy) =>
            rows.Count(x => (x.NgayMuon.Month - 1) / 3 + 1 == quy);
    }
}
