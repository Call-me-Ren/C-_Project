using System;

namespace QLThuVien.DTO
{
    /// <summary>DTO chuyển thông tin phiếu phạt giữa lớp GUI, lớp BUS và lớp DAO.</summary>
    public class PhieuPhatDTO
    {
        /// <summary>Mã phiếu phạt, thường được lớp BUS sinh theo dạng PPnn.</summary>
        public string MaPhieuPhat { get; set; } = string.Empty;
        /// <summary>Mã độc giả chịu khoản phạt.</summary>
        public string MaDocGia { get; set; } = string.Empty;
        /// <summary>Mã phiếu mượn liên quan; có thể rỗng trước khi được kiểm tra nghiệp vụ.</summary>
        public string? MaPhieuMuon { get; set; }
        /// <summary>Mã nhân viên lập phiếu.</summary>
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Ngày lập phiếu phạt.</summary>
        public DateTime NgayLap { get; set; } = DateTime.Now;
        /// <summary>Trạng thái thanh toán, nhận giá trị chưa thanh toán hoặc đã thanh toán.</summary>
        public string TrangThai { get; set; } = "Chưa thanh toán";
        /// <summary>Tổng tiền phạt của các vi phạm trên phiếu, tính bằng VNĐ.</summary>
        public decimal TongTienPhat { get; set; } = 0;

        /// <summary>Tạo DTO phiếu phạt với trạng thái và giá trị mặc định.</summary>
        public PhieuPhatDTO()
        {
        }

        /// <summary>Tạo DTO phiếu phạt với đầy đủ thông tin lập phiếu và thanh toán.</summary>
        /// <param name="maPhieuPhat">Mã phiếu phạt.</param>
        /// <param name="maDocGia">Mã độc giả.</param>
        /// <param name="maPhieuMuon">Mã phiếu mượn liên quan, có thể là <see langword="null"/>.</param>
        /// <param name="maNV">Mã nhân viên lập phiếu.</param>
        /// <param name="ngayLap">Ngày lập phiếu.</param>
        /// <param name="trangThai">Trạng thái thanh toán hợp lệ của phiếu.</param>
        /// <param name="tongTienPhat">Tổng tiền phạt tính bằng VNĐ.</param>
        public PhieuPhatDTO(string maPhieuPhat, string maDocGia, string? maPhieuMuon, string maNV, DateTime ngayLap, string trangThai, decimal tongTienPhat)
        {
            MaPhieuPhat = maPhieuPhat;
            MaDocGia = maDocGia;
            MaPhieuMuon = maPhieuMuon;
            MaNV = maNV;
            NgayLap = ngayLap;
            TrangThai = trangThai;
            TongTienPhat = tongTienPhat;
        }
    }
}
