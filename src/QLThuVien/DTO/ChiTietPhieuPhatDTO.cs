namespace QLThuVien.DTO
{
    /// <summary>DTO chuyển một vi phạm cụ thể giữa lớp GUI, lớp BUS và lớp DAO.</summary>
    public class ChiTietPhieuPhatDTO
    {
        /// <summary>Mã số tự tăng của dòng vi phạm.</summary>
        public int MaCTPP { get; set; }
        /// <summary>Mã phiếu phạt chứa vi phạm.</summary>
        public string MaPhieuPhat { get; set; } = string.Empty;
        /// <summary>Mã quy định phạt áp dụng cho cuốn sách.</summary>
        public string MaQD { get; set; } = string.Empty;
        /// <summary>Mã cuốn sách phát sinh vi phạm.</summary>
        public string MaSach { get; set; } = string.Empty;
        /// <summary>Đơn giá VNĐ áp dụng cho quy định; có thể là mức phạt/ngày, mức cố định hoặc giá sách.</summary>
        public decimal DonGia { get; set; } = 0;
        /// <summary>Số ngày trễ; để <see langword="null"/> với quy định không tính theo ngày.</summary>
        public int? SoNgayTre { get; set; } = 0;
        /// <summary>Ghi chú vi phạm, tối đa 200 ký tự khi lưu qua lớp BUS.</summary>
        public string? GhiChu { get; set; }
        /// <summary>Tiền phạt của vi phạm, tính bằng VNĐ.</summary>
        public decimal TienPhat { get; set; } = 0;

        /// <summary>Tạo DTO chi tiết vi phạm với các giá trị mặc định.</summary>
        public ChiTietPhieuPhatDTO()
        {
        }

        /// <summary>Tạo DTO chi tiết vi phạm với quy định, cuốn sách và số tiền đã tính.</summary>
        /// <param name="maPhieuPhat">Mã phiếu phạt chứa dòng vi phạm.</param>
        /// <param name="maQD">Mã quy định phạt.</param>
        /// <param name="maSach">Mã cuốn sách bị vi phạm.</param>
        /// <param name="donGia">Đơn giá hoặc mức áp dụng không âm, tính bằng VNĐ trừ khi quy định dùng hệ số.</param>
        /// <param name="soNgayTre">Số ngày trễ không âm hoặc <see langword="null"/> nếu không áp dụng.</param>
        /// <param name="ghiChu">Ghi chú tùy chọn, tối đa 200 ký tự khi lưu.</param>
        /// <param name="tienPhat">Tiền phạt đã tính, tính bằng VNĐ.</param>
        /// <param name="maCTPP">Mã dòng đã có; bằng 0 khi chưa được cơ sở dữ liệu cấp mã.</param>
        public ChiTietPhieuPhatDTO(string maPhieuPhat, string maQD, string maSach, decimal donGia, int? soNgayTre, string? ghiChu, decimal tienPhat, int maCTPP = 0)
        {
            MaCTPP = maCTPP;
            MaPhieuPhat = maPhieuPhat;
            MaQD = maQD;
            MaSach = maSach ?? string.Empty;
            DonGia = donGia;
            SoNgayTre = soNgayTre;
            GhiChu = ghiChu;
            TienPhat = tienPhat;
        }
    }
}
