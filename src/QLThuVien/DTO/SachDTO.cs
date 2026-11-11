namespace QLThuVien.DTO
{
    public class SachDTO
    {
        public string MaSach { get; set; } = string.Empty;
        public string MaDauSach { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty; // "0" hoặc "1"
        public string TinhTrang { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;

        // Thuộc tính bổ trợ tự động trả về chữ cho DataGridView
        public string TrangThaiText
        {
            get
            {
                if (TrangThai == "1" || TrangThai == "True" || TrangThai == "Đã cho mượn")
                    return "Đã cho mượn";
                return "Chưa cho mượn";
            }
        }
    }
}