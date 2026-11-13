namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho bảng ThamSo (Bảng thứ 17 - Yêu cầu 9 của GV)
    /// </summary>
    public class ThamSoDTO
    {
        public string MaTS { get; set; } = string.Empty;
        public string TenTS { get; set; } = string.Empty;
        public string GiaTri { get; set; } = string.Empty;

        public ThamSoDTO() { }

        public ThamSoDTO(string maTS, string tenTS, string giaTri)
        {
            MaTS = maTS;
            TenTS = tenTS;
            GiaTri = giaTri;
        }
    }
}
