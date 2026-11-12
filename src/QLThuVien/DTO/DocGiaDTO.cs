using System;

namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho 1 dòng dữ liệu trong bảng DocGia
    /// </summary>
    public class DocGiaDTO
    {
        // 1. CÁC THUỘC TÍNH ÁNH XẠ CSDL
        public string MaDG { get; set; } = string.Empty;
        public string? HoLot { get; set; }
        public string TenDG { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? SDT { get; set; }
        public string MaLDG { get; set; } = string.Empty;

        // 2. THUỘC TÍNH MỞ RỘNG (Dùng để hiển thị tiện lợi trên GUI)
        public string? TenLDG { get; set; }

        public string HoTen
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HoLot)) return TenDG;
                return $"{HoLot.Trim()} {TenDG.Trim()}";
            }
        }

        // 3. CONSTRUCTOR KHÔNG THAM SỐ
        public DocGiaDTO()
        {
        }

        // 4. CONSTRUCTOR ĐẦY ĐỦ THAM SỐ
        public DocGiaDTO(string maDG, string? hoLot, string tenDG, DateTime? ngaySinh, string? sdt, string maLDG, string? tenLDG = null)
        {
            MaDG = maDG;
            HoLot = hoLot;
            TenDG = tenDG;
            NgaySinh = ngaySinh;
            SDT = sdt;
            MaLDG = maLDG;
            TenLDG = tenLDG;
        }

        public override string ToString()
        {
            return $"{MaDG} - {HoTen}";
        }
    }
}
