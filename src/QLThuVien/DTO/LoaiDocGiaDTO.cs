using System;

namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho 1 dòng dữ liệu trong bảng LoaiDocGia
    /// </summary>
    public class LoaiDocGiaDTO
    {
        // 1. CÁC THUỘC TÍNH (PROPERTIES)
        // Dùng { get; set; } để đảm bảo tính đóng gói (Encapsulation) trong OOP
        public string MaLDG { get; set; } = string.Empty;
        public string TenLDG { get; set; } = string.Empty;

        // 2. CONSTRUCTOR KHÔNG THAM SỐ (Bắt buộc phải có)
        public LoaiDocGiaDTO()
        {
        }

        // 3. CONSTRUCTOR CÓ THAM SỐ (Dùng khi đọc từ DB ra để gán nhanh)
        public LoaiDocGiaDTO(string maLDG, string tenLDG)
        {
            MaLDG = maLDG;
            TenLDG = tenLDG;
        }

        // 4. GHI ĐÈ HÀM ToString() 
        // Sau này khi đổ đối tượng LoaiDocGiaDTO vào ComboBox trên WinForms,
        // ComboBox sẽ tự động hiển thị TenLDG cho người dùng xem.
        public override string ToString()
        {
            return TenLDG;
        }
    }
}