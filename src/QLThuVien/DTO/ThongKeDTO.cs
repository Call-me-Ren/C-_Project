using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace QLThuVien.DTO
{
    public class ThongKeGiaoDichDTO
    {
        [DisplayName("Loại")]
        public string Loai { get; set; } = string.Empty;
        [DisplayName("Mã phiếu")]
        public string MaGiaoDich { get; set; } = string.Empty;
        [DisplayName("Ngày")]
        public DateTime Ngay { get; set; }
        [Browsable(false)]
        public string? MaNV { get; set; }
        [DisplayName("Đối tượng")]
        public string DoiTuong { get; set; } = string.Empty;
        [DisplayName("Diễn giải")]
        public string DienGiai { get; set; } = string.Empty;
        [DisplayName("Số tiền")]
        public decimal SoTien { get; set; }
    }

    public class ThongKePhieuPhatDTO
    {
        public string MaPhieuPhat { get; set; } = string.Empty;
        public string MaDocGia { get; set; } = string.Empty;
        public string MaNV { get; set; } = string.Empty;
        public DateTime NgayLap { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal TongTienPhat { get; set; }
    }

    public class ThongKePhieuNhapDTO
    {
        public string MaPN { get; set; } = string.Empty;
        public string MaNV { get; set; } = string.Empty;
        public DateTime NgayNhap { get; set; }
        public decimal TongTien { get; set; }
    }

    public class ThongKeTongHopDTO
    {
        public decimal TongThu { get; set; }
        public decimal TongChi { get; set; }
        public decimal ChenhLech => TongThu - TongChi;
        public List<ThongKeGiaoDichDTO> GiaoDich { get; set; } = new List<ThongKeGiaoDichDTO>();
        public int NamThongKe { get; set; }
        public List<ThongKeNhanVienQuyDTO> ThongKeNhanVienTheoQuy { get; set; } = new List<ThongKeNhanVienQuyDTO>();
        public List<ThongKeDocGiaQuyDTO> ThongKeDocGiaTheoQuy { get; set; } = new List<ThongKeDocGiaQuyDTO>();
    }

    public class ThongKeNhanVienQuyDTO
    {
        [DisplayName("Mã nhân viên")]
        public string MaNV { get; set; } = string.Empty;
        [DisplayName("Nhân viên")]
        public string TenNhanVien { get; set; } = string.Empty;
        [DisplayName("Q1 - Tiền phạt")]
        public decimal TienPhatQ1 { get; set; }
        [DisplayName("Q1 - Tiền nhập")]
        public decimal TienNhapQ1 { get; set; }
        [DisplayName("Q2 - Tiền phạt")]
        public decimal TienPhatQ2 { get; set; }
        [DisplayName("Q2 - Tiền nhập")]
        public decimal TienNhapQ2 { get; set; }
        [DisplayName("Q3 - Tiền phạt")]
        public decimal TienPhatQ3 { get; set; }
        [DisplayName("Q3 - Tiền nhập")]
        public decimal TienNhapQ3 { get; set; }
        [DisplayName("Q4 - Tiền phạt")]
        public decimal TienPhatQ4 { get; set; }
        [DisplayName("Q4 - Tiền nhập")]
        public decimal TienNhapQ4 { get; set; }
        [DisplayName("Tổng tiền phạt")]
        public decimal TongTienPhat { get; set; }
        [DisplayName("Tổng tiền nhập")]
        public decimal TongTienNhap { get; set; }
    }

    public class ThongKeDocGiaQuyDTO
    {
        [DisplayName("Mã độc giả")]
        public string MaDocGia { get; set; } = string.Empty;
        [DisplayName("Độc giả")]
        public string TenDocGia { get; set; } = string.Empty;
        [DisplayName("Q1 - Tiền phạt")]
        public decimal TienPhatQ1 { get; set; }
        [DisplayName("Q2 - Tiền phạt")]
        public decimal TienPhatQ2 { get; set; }
        [DisplayName("Q3 - Tiền phạt")]
        public decimal TienPhatQ3 { get; set; }
        [DisplayName("Q4 - Tiền phạt")]
        public decimal TienPhatQ4 { get; set; }
        [DisplayName("Tổng cộng")]
        public decimal TongCong { get; set; }
    }

    public class PhieuPhatBaoCaoDTO
    {
        public string MaPhieuPhat { get; set; } = string.Empty;
        public DateTime NgayLap { get; set; }
        public string MaDocGia { get; set; } = string.Empty;
        public string TenDocGia { get; set; } = string.Empty;
        public string MaNV { get; set; } = string.Empty;
        public string TenNhanVien { get; set; } = string.Empty;
        public string? MaPhieuMuon { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal TongTienPhat { get; set; }
        public List<ChiTietPhieuPhatBaoCaoDTO> ChiTiet { get; set; } = new List<ChiTietPhieuPhatBaoCaoDTO>();
    }

    public class ChiTietPhieuPhatBaoCaoDTO
    {
        public string MaSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string MaQD { get; set; } = string.Empty;
        public string TenQD { get; set; } = string.Empty;
        public int? SoNgayTre { get; set; }
        public decimal DonGia { get; set; }
        public decimal TienPhat { get; set; }
        public string? GhiChu { get; set; }
    }
}
