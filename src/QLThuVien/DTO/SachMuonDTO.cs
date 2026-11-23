using System;

namespace QLThuVien.DTO
{
    /// <summary>DTO biểu diễn cuốn sách thuộc một phiếu mượn để lớp BUS tính vi phạm và lớp GUI hiển thị lựa chọn.</summary>
    public class SachMuonDTO
    {
        /// <summary>Mã cuốn sách.</summary>
        public string MaSach { get; set; } = string.Empty;
        /// <summary>Tên đầu sách của cuốn sách.</summary>
        public string TenSach { get; set; } = string.Empty;
        /// <summary>Ngày đến hạn trả của phiếu mượn.</summary>
        public DateTime HanTra { get; set; }
        /// <summary>Ngày trả thực tế; <see langword="null"/> khi cuốn sách chưa được trả.</summary>
        public DateTime? NgayThucTra { get; set; }
        /// <summary>Nhãn lựa chọn gồm mã và tên sách nếu tên có sẵn.</summary>
        public string HienThi => string.IsNullOrWhiteSpace(TenSach) ? MaSach : $"{MaSach} - {TenSach}";

        /// <summary>Tạo thông tin cuốn sách mượn với các giá trị mặc định.</summary>
        public SachMuonDTO()
        {
        }

        /// <summary>Tạo thông tin cuốn sách mượn kèm hạn trả và ngày trả thực tế.</summary>
        /// <param name="maSach">Mã cuốn sách thuộc phiếu mượn.</param>
        /// <param name="tenSach">Tên đầu sách dùng để hiển thị.</param>
        /// <param name="hanTra">Ngày đến hạn trả.</param>
        /// <param name="ngayThucTra">Ngày trả thực tế hoặc <see langword="null"/> nếu chưa trả.</param>
        public SachMuonDTO(string maSach, string tenSach, DateTime hanTra, DateTime? ngayThucTra)
        {
            MaSach = maSach;
            TenSach = tenSach;
            HanTra = hanTra;
            NgayThucTra = ngayThucTra;
        }
    }
}
