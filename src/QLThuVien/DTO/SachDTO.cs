namespace DTO
{
    public class SachDTO
    {
        public string MaSach { get; set; }
        public string MaDauSach { get; set; }
        public string TrangThai { get; set; } // "0" hoặc "1"
        public string TinhTrang { get; set; }
        public string ViTri { get; set; }

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