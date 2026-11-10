namespace QLThuVien.DTO
{
    public class ChiTietPhieuPhatDTO
    {
        public string MaPhieuPhat { get; set; } = string.Empty;
        public string MaQD { get; set; } = string.Empty;
        public string? MaSach { get; set; }
        public decimal DonGia { get; set; } = 0;
        public int? SoNgayTre { get; set; } = 0;
        public string? GhiChu { get; set; }
        public decimal TienPhat { get; set; } = 0;

        public ChiTietPhieuPhatDTO()
        {
        }

        public ChiTietPhieuPhatDTO(string maPhieuPhat, string maQD, string? maSach, decimal donGia, int? soNgayTre, string? ghiChu, decimal tienPhat)
        {
            MaPhieuPhat = maPhieuPhat;
            MaQD = maQD;
            MaSach = maSach;
            DonGia = donGia;
            SoNgayTre = soNgayTre;
            GhiChu = ghiChu;
            TienPhat = tienPhat;
        }
    }
}
