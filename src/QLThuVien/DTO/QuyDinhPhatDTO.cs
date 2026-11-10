namespace QLThuVien.DTO
{
    public class QuyDinhPhatDTO
    {
        public string MaQD { get; set; } = string.Empty;
        public string TenQD { get; set; } = string.Empty;
        public string? LoaiPhat { get; set; }
        public string? CachTinh { get; set; }
        public string? DonViTinh { get; set; }
        public decimal MucPhat { get; set; } = 0;

        public QuyDinhPhatDTO()
        {
        }

        public QuyDinhPhatDTO(string maQD, string tenQD, string? loaiPhat, string? cachTinh, string? donViTinh, decimal mucPhat)
        {
            MaQD = maQD;
            TenQD = tenQD;
            LoaiPhat = loaiPhat;
            CachTinh = cachTinh;
            DonViTinh = donViTinh;
            MucPhat = mucPhat;
        }
    }
}
