using System;

namespace QLThuVien.DTO
{
    public class SachMuonDTO
    {
        public string MaSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public DateTime HanTra { get; set; }
        public DateTime? NgayThucTra { get; set; }
        public string HienThi => string.IsNullOrWhiteSpace(TenSach) ? MaSach : $"{MaSach} - {TenSach}";

        public SachMuonDTO()
        {
        }

        public SachMuonDTO(string maSach, string tenSach, DateTime hanTra, DateTime? ngayThucTra)
        {
            MaSach = maSach;
            TenSach = tenSach;
            HanTra = hanTra;
            NgayThucTra = ngayThucTra;
        }
    }
}
