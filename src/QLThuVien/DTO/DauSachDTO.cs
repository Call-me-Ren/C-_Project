namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho bảng DauSach trong CSDL
    /// </summary>
    public class DauSachDTO
    {
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string MaNXB { get; set; } = string.Empty;
        public string MaTG { get; set; } = string.Empty;
        public string MaTL { get; set; } = string.Empty;
        public int NamXB { get; set; }
        public int SoLuong { get; set; }
        public int SLChoMuon { get; set; }
        public int SLConLai { get; set; }
        public decimal DonGia { get; set; }

        public DauSachDTO() { }

        public DauSachDTO(string maDauSach, string tenSach, decimal donGia, int soLuong, int slConLai)
        {
            MaDauSach = maDauSach;
            TenSach = tenSach;
            DonGia = donGia;
            SoLuong = soLuong;
            SLConLai = slConLai;
        }
    }
}
