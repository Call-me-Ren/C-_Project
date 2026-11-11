#nullable disable
namespace QLThuVien.DTO
{
    public class NXBDTO
    {
        public string MaNXB { get; set; }
        public string TenNXB{ get; set; }
        public string DiaChi { get; set; }

        public NXBDTO() { }

        public NXBDTO( string maNXB, string tenNXB, string diaChi)
        {
            MaNXB = maNXB;
            TenNXB = tenNXB;
            DiaChi = diaChi;
        }
    }
}