namespace QLThuVien.DTO
{
    /// <summary>
    /// Data Transfer Object cho bảng NhaCungCap
    /// </summary>
    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; } = string.Empty;
        public string TenNCC { get; set; } = string.Empty;

        public NhaCungCapDTO() { }

        public NhaCungCapDTO(string maNCC, string tenNCC)
        {
            MaNCC = maNCC;
            TenNCC = tenNCC;
        }
    }
}
