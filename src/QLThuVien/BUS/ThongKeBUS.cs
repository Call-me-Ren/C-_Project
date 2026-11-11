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
            return report;
        }

        private static bool Same(string? left, string? right) =>
            string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

        private static decimal TinhTienPhatTheoQuy(IEnumerable<ThongKePhieuPhatDTO> rows, int quy) =>
            rows.Where(x => (x.NgayLap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTienPhat);

        private static decimal TinhTienNhapTheoQuy(IEnumerable<ThongKePhieuNhapDTO> rows, int quy) =>
            rows.Where(x => (x.NgayNhap.Month - 1) / 3 + 1 == quy).Sum(x => x.TongTien);
    }
}
