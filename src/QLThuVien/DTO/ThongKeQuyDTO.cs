namespace QLThuVien.DTO
{
    // DTO lưu kết quả thống kê nhập sách theo Quý cho một Đầu sách
    public class ThongKeQuyDTO
    {
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach   { get; set; } = string.Empty;
        public decimal Q1 { get; set; }  // Tổng tiền nhập Quý 1
        public decimal Q2 { get; set; }  // Tổng tiền nhập Quý 2
        public decimal Q3 { get; set; }  // Tổng tiền nhập Quý 3
        public decimal Q4 { get; set; }  // Tổng tiền nhập Quý 4
        public decimal TongCong => Q1 + Q2 + Q3 + Q4;
    }
}
