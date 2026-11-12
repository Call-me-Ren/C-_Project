#nullable disable
namespace QLThuVien.DTO
{
    public class DauSachDTO
    {
        public string MaDauSach { get; set; }
        public string MaNXB { get; set; }
        public string MaTG { get; set; }
        public string MaTL { get; set; }
        public string TenSach { get; set; }
        public int? NamXB { get; set; }
        public int SoLuong { get; set; }
        public int SLChoMuon { get; set; }
        public int SLConLai { get; set; }
        public decimal DonGia { get; set; }

        public DauSachDTO() { }

        public DauSachDTO(string maDauSach, string maNXB, string maTG, string maTL, string tenSach, int? namXB, int soLuong, decimal donGia)
        {
            MaDauSach = maDauSach;
            MaNXB = maNXB;
            MaTG = maTG;
            MaTL = maTL;
            TenSach = tenSach;
            NamXB = namXB;
            SoLuong = soLuong;
            DonGia = donGia;
        }
    }
}