namespace QLThuVien.DTO
{
    public class NhanVienDTO
    {
        public string MaNV { get; set; } = string.Empty;
        public string? HoLot { get; set; }
        public string TenNV { get; set; } = string.Empty;
        public string? ChucVu { get; set; }

        public string HoTen => string.IsNullOrWhiteSpace(HoLot) ? TenNV : $"{HoLot.Trim()} {TenNV.Trim()}";

        public NhanVienDTO()
        {
        }

        public NhanVienDTO(string maNV, string? hoLot, string tenNV, string? chucVu)
        {
            MaNV = maNV;
            HoLot = hoLot;
            TenNV = tenNV;
            ChucVu = chucVu;
        }
    }
}
