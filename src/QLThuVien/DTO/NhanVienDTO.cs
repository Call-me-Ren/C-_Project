namespace QLThuVien.DTO
{
    /// <summary>DTO chuyển thông tin nhân viên giữa lớp GUI, lớp BUS và lớp DAO.</summary>
    public class NhanVienDTO
    {
        /// <summary>Mã định danh nhân viên, tối đa 20 ký tự.</summary>
        public string MaNV { get; set; } = string.Empty;
        /// <summary>Họ lót nhân viên; có thể để trống.</summary>
        public string? HoLot { get; set; }
        /// <summary>Tên nhân viên, bắt buộc và tối đa 50 ký tự.</summary>
        public string TenNV { get; set; } = string.Empty;
        /// <summary>Chức vụ nhân viên, tối đa 50 ký tự.</summary>
        public string? ChucVu { get; set; }

        /// <summary>Họ tên đầy đủ, ghép họ lót và tên sau khi bỏ khoảng trắng ở hai đầu.</summary>
        public string HoTen => string.IsNullOrWhiteSpace(HoLot) ? TenNV : $"{HoLot.Trim()} {TenNV.Trim()}";

        /// <summary>Tạo DTO nhân viên với các giá trị mặc định.</summary>
        public NhanVienDTO()
        {
        }

        /// <summary>Tạo DTO nhân viên với thông tin định danh và chức vụ.</summary>
        /// <param name="maNV">Mã nhân viên, tối đa 20 ký tự.</param>
        /// <param name="hoLot">Họ lót; có thể là <see langword="null"/> hoặc rỗng.</param>
        /// <param name="tenNV">Tên nhân viên, tối đa 50 ký tự.</param>
        /// <param name="chucVu">Chức vụ; có thể là <see langword="null"/> nếu chưa được gán.</param>
        public NhanVienDTO(string maNV, string? hoLot, string tenNV, string? chucVu)
        {
            MaNV = maNV;
            HoLot = hoLot;
            TenNV = tenNV;
            ChucVu = chucVu;
        }
    }
}
