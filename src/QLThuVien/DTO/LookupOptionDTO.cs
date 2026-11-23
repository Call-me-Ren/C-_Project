namespace QLThuVien.DTO
{
    /// <summary>DTO dùng trong lớp GUI để hiển thị lựa chọn tra cứu lấy từ các lớp BUS và DAO.</summary>
    public class LookupOptionDTO
    {
        /// <summary>Mã định danh của lựa chọn.</summary>
        public string Ma { get; set; } = string.Empty;
        /// <summary>Nhãn hiển thị kèm mã trong danh sách chọn.</summary>
        public string HienThi { get; set; } = string.Empty;
        /// <summary>Mã độc giả liên quan nếu lựa chọn là phiếu mượn; có thể để trống.</summary>
        public string? MaDocGia { get; set; }

        /// <summary>Tạo lựa chọn tra cứu với các giá trị mặc định.</summary>
        public LookupOptionDTO()
        {
        }

        /// <summary>Tạo lựa chọn tra cứu với mã, nhãn hiển thị và mã độc giả tùy chọn.</summary>
        /// <param name="ma">Mã của đối tượng được chọn.</param>
        /// <param name="hienThi">Nội dung nhãn hiển thị.</param>
        /// <param name="maDocGia">Mã độc giả liên quan nếu có; có thể là <see langword="null"/>.</param>
        public LookupOptionDTO(string ma, string hienThi, string? maDocGia = null)
        {
            Ma = ma;
            HienThi = hienThi;
            MaDocGia = maDocGia;
        }
    }
}
