#nullable disable
namespace DTO
{
    public class TheLoaiDTO
    {
        public string MaTL { get; set; }
        public string TenTL { get; set; }


        public TheLoaiDTO() { }

        public TheLoaiDTO(string maTL, string tenTL)
        {
            MaTL = maTL;
            TenTL = tenTL;
        }
    }
}