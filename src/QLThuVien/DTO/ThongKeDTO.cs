using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace QLThuVien.DTO
{
    /// <summary>DTO biểu diễn một giao dịch thu hoặc chi để lớp BUS chuyển sang biểu mẫu và tệp báo cáo.</summary>
    public class ThongKeGiaoDichDTO
    {
        /// <summary>Phân loại giao dịch là Thu hoặc Chi.</summary>
        [DisplayName("Loại")]
        public string Loai { get; set; } = string.Empty;
        /// <summary>Mã phiếu phát sinh giao dịch.</summary>
        [DisplayName("Mã phiếu")]
        public string MaGiaoDich { get; set; } = string.Empty;
        /// <summary>Ngày ghi nhận giao dịch.</summary>
        [DisplayName("Ngày")]
        public DateTime Ngay { get; set; }
        /// <summary>Mã nhân viên gắn với giao dịch; không hiển thị trên lưới.</summary>
        [Browsable(false)]
        public string? MaNV { get; set; }
        /// <summary>Độc giả hoặc nhà cung cấp liên quan.</summary>
        [DisplayName("Đối tượng")]
        public string DoiTuong { get; set; } = string.Empty;
        /// <summary>Nội dung mô tả giao dịch.</summary>
        [DisplayName("Diễn giải")]
        public string DienGiai { get; set; } = string.Empty;
        /// <summary>Giá trị giao dịch, tính bằng VNĐ.</summary>
        [DisplayName("Số tiền")]
        public decimal SoTien { get; set; }
    }

    /// <summary>DTO dữ liệu nguồn của một phiếu phạt dùng để tổng hợp quỹ theo nhân viên hoặc độc giả.</summary>
    public class ThongKePhieuPhatDTO
    {
        /// <summary>Mã phiếu phạt.</summary>
        public string MaPhieuPhat { get; set; } = string.Empty;
        /// <summary>Mã độc giả chịu khoản phạt.</summary>
        public string MaDocGia { get; set; } = string.Empty;
        /// <summary>Mã nhân viên lập phiếu.</summary>
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Ngày lập phiếu dùng để xác định quý.</summary>
        public DateTime NgayLap { get; set; }
        /// <summary>Trạng thái thanh toán của phiếu.</summary>
        public string TrangThai { get; set; } = string.Empty;
        /// <summary>Tổng tiền phạt của phiếu, tính bằng VNĐ.</summary>
        public decimal TongTienPhat { get; set; }
    }

    /// <summary>DTO dữ liệu nguồn của một phiếu nhập dùng để tổng hợp chi phí theo nhân viên hoặc đầu sách.</summary>
    public class ThongKePhieuNhapDTO
    {
        /// <summary>Mã phiếu nhập.</summary>
        public string MaPN { get; set; } = string.Empty;
        /// <summary>Mã nhân viên lập phiếu nhập.</summary>
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Ngày nhập dùng để xác định quý.</summary>
        public DateTime NgayNhap { get; set; }
        /// <summary>Tổng tiền phiếu nhập, tính bằng VNĐ.</summary>
        public decimal TongTien { get; set; }
    }

    /// <summary>DTO kết quả báo cáo tổng hợp thu-chi và các bảng quỹ theo quý.</summary>
    public class ThongKeTongHopDTO
    {
        /// <summary>Tổng các giao dịch thu, tính bằng VNĐ.</summary>
        public decimal TongThu { get; set; }
        /// <summary>Tổng các giao dịch chi, tính bằng VNĐ.</summary>
        public decimal TongChi { get; set; }
        /// <summary>Chênh lệch thu trừ chi, tính bằng VNĐ.</summary>
        public decimal ChenhLech => TongThu - TongChi;
        /// <summary>Các giao dịch trong khoảng ngày được yêu cầu.</summary>
        public List<ThongKeGiaoDichDTO> GiaoDich { get; set; } = new List<ThongKeGiaoDichDTO>();
        /// <summary>Năm áp dụng cho các bảng thống kê theo quý.</summary>
        public int NamThongKe { get; set; }
        /// <summary>Số liệu tiền phạt và tiền nhập theo từng nhân viên, chia theo quý.</summary>
        public List<ThongKeNhanVienQuyDTO> ThongKeNhanVienTheoQuy { get; set; } = new List<ThongKeNhanVienQuyDTO>();
        /// <summary>Số liệu tiền phạt theo từng độc giả, chia theo quý.</summary>
        public List<ThongKeDocGiaQuyDTO> ThongKeDocGiaTheoQuy { get; set; } = new List<ThongKeDocGiaQuyDTO>();
        /// <summary>Số liệu tiền nhập và lượt mượn theo từng đầu sách, chia theo quý.</summary>
        public List<ThongKeDauSachQuyDTO> ThongKeDauSachTheoQuy { get; set; } = new List<ThongKeDauSachQuyDTO>();
    }

    /// <summary>DTO một dòng quỹ nhân viên gồm tiền phạt và chi phí nhập sách theo từng quý.</summary>
    public class ThongKeNhanVienQuyDTO
    {
        /// <summary>Mã nhân viên.</summary>
        [DisplayName("Mã nhân viên")]
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Họ tên nhân viên.</summary>
        [DisplayName("Nhân viên")]
        public string TenNhanVien { get; set; } = string.Empty;
        /// <summary>Tổng tiền phạt các phiếu do nhân viên lập trong quý 1, tính bằng VNĐ.</summary>
        [DisplayName("Q1 - Tiền phạt")]
        public decimal TienPhatQ1 { get; set; }
        /// <summary>Tổng tiền nhập sách do nhân viên lập trong quý 1, tính bằng VNĐ.</summary>
        [DisplayName("Q1 - Tiền nhập")]
        public decimal TienNhapQ1 { get; set; }
        /// <summary>Tổng tiền phạt các phiếu do nhân viên lập trong quý 2, tính bằng VNĐ.</summary>
        [DisplayName("Q2 - Tiền phạt")]
        public decimal TienPhatQ2 { get; set; }
        /// <summary>Tổng tiền nhập sách do nhân viên lập trong quý 2, tính bằng VNĐ.</summary>
        [DisplayName("Q2 - Tiền nhập")]
        public decimal TienNhapQ2 { get; set; }
        /// <summary>Tổng tiền phạt các phiếu do nhân viên lập trong quý 3, tính bằng VNĐ.</summary>
        [DisplayName("Q3 - Tiền phạt")]
        public decimal TienPhatQ3 { get; set; }
        /// <summary>Tổng tiền nhập sách do nhân viên lập trong quý 3, tính bằng VNĐ.</summary>
        [DisplayName("Q3 - Tiền nhập")]
        public decimal TienNhapQ3 { get; set; }
        /// <summary>Tổng tiền phạt các phiếu do nhân viên lập trong quý 4, tính bằng VNĐ.</summary>
        [DisplayName("Q4 - Tiền phạt")]
        public decimal TienPhatQ4 { get; set; }
        /// <summary>Tổng tiền nhập sách do nhân viên lập trong quý 4, tính bằng VNĐ.</summary>
        [DisplayName("Q4 - Tiền nhập")]
        public decimal TienNhapQ4 { get; set; }
        /// <summary>Tổng tiền phạt các phiếu của nhân viên trong cả năm, tính bằng VNĐ.</summary>
        [DisplayName("Tổng tiền phạt")]
        public decimal TongTienPhat { get; set; }
        /// <summary>Tổng tiền nhập sách do nhân viên lập trong cả năm, tính bằng VNĐ.</summary>
        [DisplayName("Tổng tiền nhập")]
        public decimal TongTienNhap { get; set; }
    }

    /// <summary>DTO một dòng quỹ độc giả gồm tiền phạt theo quý và tổng cả năm.</summary>
    public class ThongKeDocGiaQuyDTO
    {
        /// <summary>Mã độc giả.</summary>
        [DisplayName("Mã độc giả")]
        public string MaDocGia { get; set; } = string.Empty;
        /// <summary>Họ tên độc giả.</summary>
        [DisplayName("Độc giả")]
        public string TenDocGia { get; set; } = string.Empty;
        /// <summary>Tổng tiền phạt của độc giả trong quý 1, tính bằng VNĐ.</summary>
        [DisplayName("Q1 - Tiền phạt")]
        public decimal TienPhatQ1 { get; set; }
        /// <summary>Tổng tiền phạt của độc giả trong quý 2, tính bằng VNĐ.</summary>
        [DisplayName("Q2 - Tiền phạt")]
        public decimal TienPhatQ2 { get; set; }
        /// <summary>Tổng tiền phạt của độc giả trong quý 3, tính bằng VNĐ.</summary>
        [DisplayName("Q3 - Tiền phạt")]
        public decimal TienPhatQ3 { get; set; }
        /// <summary>Tổng tiền phạt của độc giả trong quý 4, tính bằng VNĐ.</summary>
        [DisplayName("Q4 - Tiền phạt")]
        public decimal TienPhatQ4 { get; set; }
        /// <summary>Tổng tiền phạt của độc giả trong cả năm, tính bằng VNĐ.</summary>
        [DisplayName("Tổng cộng")]
        public decimal TongCong { get; set; }
    }

    /// <summary>DTO một dòng thống kê tiền nhập và lượt mượn theo đầu sách trong bốn quý.</summary>
    public class ThongKeDauSachQuyDTO
    {
        /// <summary>Mã đầu sách.</summary>
        [DisplayName("Mã đầu sách")]
        public string MaDauSach { get; set; } = string.Empty;
        /// <summary>Tên đầu sách.</summary>
        [DisplayName("Đầu sách")]
        public string TenDauSach { get; set; } = string.Empty;
        /// <summary>Tổng thành tiền nhập đầu sách trong quý 1, tính bằng VNĐ.</summary>
        [DisplayName("Q1 - Tiền nhập")]
        public decimal TienNhapQ1 { get; set; }
        /// <summary>Số dòng sách được mượn trong quý 1.</summary>
        [DisplayName("Q1 - Lượt mượn")]
        public int LuotMuonQ1 { get; set; }
        /// <summary>Tổng thành tiền nhập đầu sách trong quý 2, tính bằng VNĐ.</summary>
        [DisplayName("Q2 - Tiền nhập")]
        public decimal TienNhapQ2 { get; set; }
        /// <summary>Số dòng sách được mượn trong quý 2.</summary>
        [DisplayName("Q2 - Lượt mượn")]
        public int LuotMuonQ2 { get; set; }
        /// <summary>Tổng thành tiền nhập đầu sách trong quý 3, tính bằng VNĐ.</summary>
        [DisplayName("Q3 - Tiền nhập")]
        public decimal TienNhapQ3 { get; set; }
        /// <summary>Số dòng sách được mượn trong quý 3.</summary>
        [DisplayName("Q3 - Lượt mượn")]
        public int LuotMuonQ3 { get; set; }
        /// <summary>Tổng thành tiền nhập đầu sách trong quý 4, tính bằng VNĐ.</summary>
        [DisplayName("Q4 - Tiền nhập")]
        public decimal TienNhapQ4 { get; set; }
        /// <summary>Số dòng sách được mượn trong quý 4.</summary>
        [DisplayName("Q4 - Lượt mượn")]
        public int LuotMuonQ4 { get; set; }
        /// <summary>Tổng thành tiền nhập đầu sách trong cả năm, tính bằng VNĐ.</summary>
        [DisplayName("Tổng tiền nhập")]
        public decimal TongTienNhap { get; set; }
        /// <summary>Tổng số dòng sách được mượn trong cả năm.</summary>
        [DisplayName("Tổng lượt mượn")]
        public int TongLuotMuon { get; set; }
    }

    /// <summary>DTO một dòng nhập sách gồm đầu sách, ngày nhập và thành tiền.</summary>
    public class ThongKeNhapDauSachDTO
    {
        /// <summary>Mã đầu sách được nhập.</summary>
        public string MaDauSach { get; set; } = string.Empty;
        /// <summary>Ngày ghi nhận phiếu nhập.</summary>
        public DateTime NgayNhap { get; set; }
        /// <summary>Thành tiền của dòng nhập, tính bằng VNĐ.</summary>
        public decimal ThanhTien { get; set; }
    }

    /// <summary>DTO một lượt mượn dùng để đếm số sách mượn theo đầu sách và quý.</summary>
    public class ThongKeLuotMuonDauSachDTO
    {
        /// <summary>Mã đầu sách của cuốn được mượn.</summary>
        public string MaDauSach { get; set; } = string.Empty;
        /// <summary>Ngày lập phiếu mượn dùng để xác định quý.</summary>
        public DateTime NgayMuon { get; set; }
    }

    /// <summary>DTO đầu phiếu và chi tiết dùng riêng cho lớp BUS và bộ xuất PDF phiếu phạt.</summary>
    public class PhieuPhatBaoCaoDTO
    {
        /// <summary>Mã phiếu phạt.</summary>
        public string MaPhieuPhat { get; set; } = string.Empty;
        /// <summary>Ngày lập phiếu.</summary>
        public DateTime NgayLap { get; set; }
        /// <summary>Mã độc giả bị lập phiếu.</summary>
        public string MaDocGia { get; set; } = string.Empty;
        /// <summary>Họ tên độc giả hiển thị trên báo cáo.</summary>
        public string TenDocGia { get; set; } = string.Empty;
        /// <summary>Mã nhân viên lập phiếu.</summary>
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Họ tên nhân viên hiển thị trên báo cáo.</summary>
        public string TenNhanVien { get; set; } = string.Empty;
        /// <summary>Mã phiếu mượn liên quan; có thể <see langword="null"/>.</summary>
        public string? MaPhieuMuon { get; set; }
        /// <summary>Trạng thái thanh toán tại thời điểm đọc dữ liệu.</summary>
        public string TrangThai { get; set; } = string.Empty;
        /// <summary>Tổng tiền phạt của phiếu, tính bằng VNĐ.</summary>
        public decimal TongTienPhat { get; set; }
        /// <summary>Các dòng vi phạm sẽ được trình bày trên phiếu PDF.</summary>
        public List<ChiTietPhieuPhatBaoCaoDTO> ChiTiet { get; set; } = new List<ChiTietPhieuPhatBaoCaoDTO>();
    }

    /// <summary>DTO dữ liệu sách, quy định và tiền phạt của một dòng trên phiếu PDF.</summary>
    public class ChiTietPhieuPhatBaoCaoDTO
    {
        /// <summary>Mã cuốn sách vi phạm.</summary>
        public string MaSach { get; set; } = string.Empty;
        /// <summary>Tên đầu sách hiển thị trên báo cáo.</summary>
        public string TenSach { get; set; } = string.Empty;
        /// <summary>Mã quy định phạt.</summary>
        public string MaQD { get; set; } = string.Empty;
        /// <summary>Tên quy định hiển thị trên báo cáo.</summary>
        public string TenQD { get; set; } = string.Empty;
        /// <summary>Số ngày trễ; <see langword="null"/> với quy định không theo ngày.</summary>
        public int? SoNgayTre { get; set; }
        /// <summary>Đơn giá hoặc mức phạt áp dụng, tính bằng VNĐ trừ khi dùng hệ số.</summary>
        public decimal DonGia { get; set; }
        /// <summary>Tiền phạt của dòng, tính bằng VNĐ.</summary>
        public decimal TienPhat { get; set; }
        /// <summary>Ghi chú hiển thị trên báo cáo; có thể <see langword="null"/>.</summary>
        public string? GhiChu { get; set; }
    }
}
