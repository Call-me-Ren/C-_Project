namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho bảng cấu hình tham số hệ thống ThamSo (bảng thứ 17).
    /// </summary>
    public class ThamSoDTO
    {
        public string MaTS { get; set; } = string.Empty;
        public string TenTS { get; set; } = string.Empty;
        public string GiaTri { get; set; } = string.Empty;

        public ThamSoDTO()
        {
        }

        public ThamSoDTO(string maTS, string tenTS, string giaTri)
        {
            MaTS = maTS;
            TenTS = tenTS;
            GiaTri = giaTri;
        }
    }
}
