namespace QLThuVien.DTO
{
    public class ChiTietPhieuPhatDTO
    {
        public int MaCTPP { get; set; }
        public string MaPhieuPhat { get; set; } = string.Empty;
        public string MaQD { get; set; } = string.Empty;
        public string MaSach { get; set; } = string.Empty;
        public decimal DonGia { get; set; } = 0;
        public int? SoNgayTre { get; set; } = 0;
        public string? GhiChu { get; set; }
        public decimal TienPhat { get; set; } = 0;

        public ChiTietPhieuPhatDTO()
        {
        }

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
