namespace QLThuVien.DTO
{
    public class LookupOptionDTO
    {
        public string Ma { get; set; } = string.Empty;
        public string HienThi { get; set; } = string.Empty;
        public string? MaDocGia { get; set; }

        public LookupOptionDTO()
        {
        }

        public LookupOptionDTO(string ma, string hienThi, string? maDocGia = null)
        {
            Ma = ma;
            HienThi = hienThi;
            MaDocGia = maDocGia;
        }
    }
}
