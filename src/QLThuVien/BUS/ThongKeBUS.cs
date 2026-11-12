using System;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class ThongKeBUS
    {
        private readonly ThongKeDAO dao = new ThongKeDAO();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly DocGiaBUS busDocGia = new DocGiaBUS();

        public static void KiemTraKhoangNgay(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
                throw new BusinessRuleException("Từ ngày phải trước hoặc bằng Đến ngày.");
            if (tuNgay.Date > DateTime.Today || denNgay.Date > DateTime.Today)
                throw new BusinessRuleException("Ngày thống kê không được ở tương lai.");
        }

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

        public static void KiemTraNamThongKe(int nam)
        {
            if (nam < 2020 || nam > DateTime.Today.Year + 1)
                throw new BusinessRuleException($"Năm thống kê phải từ 2020 đến {DateTime.Today.Year + 1}.");
        }

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

        private static decimal TinhTienPhatTheoQuy(IEnumerable<ThongKePhieuPhatDTO> rows, int quy) =>
            rows.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTienPhat);

        private static decimal TinhTienNhapTheoQuy(IEnumerable<ThongKePhieuNhapDTO> rows, int quy) =>
            rows.Where(x => (x.NgayNhap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTien);

        private static decimal TinhTienNhapDauSachTheoQuy(IEnumerable<ThongKeNhapDauSachDTO> rows, int quy) =>
            rows.Where(x => (x.NgayNhap.Month - 1) / 3 + 1 == quy).Sum(x => x.ThanhTien);

        private static int DemLuotMuonTheoQuy(IEnumerable<ThongKeLuotMuonDauSachDTO> rows, int quy) =>
            rows.Count(x => (x.NgayMuon.Month - 1) / 3 + 1 == quy);
    }
}
