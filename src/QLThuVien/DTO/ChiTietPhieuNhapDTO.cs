namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho bảng ChiTietPhieuNhap trong CSDL
    /// Khóa chính phức hợp: (MaPN, MaDauSach)
    /// </summary>
    public class ChiTietPhieuNhapDTO
    {
        public string MaPN { get; set; } = string.Empty;
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty; // Thuộc tính hiển thị mở rộng
        public decimal DonGia { get; set; } = 0;
        public int SoLuong { get; set; } = 0;
        public decimal ThanhTien { get; set; } = 0; // DonGia * SoLuong

        public ChiTietPhieuNhapDTO() { }

        public ChiTietPhieuNhapDTO(string maPN, string maDauSach, decimal donGia, int soLuong, decimal thanhTien, string tenSach = "")
        {
            MaPN = maPN;
            MaDauSach = maDauSach;
            DonGia = donGia;
            SoLuong = soLuong;
            ThanhTien = thanhTien;
            TenSach = tenSach;
        }
    }
}
