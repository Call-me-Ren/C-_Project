namespace QLThuVien.DTO
{
    /// <summary>DTO chuyển cấu hình một quy định phạt giữa lớp GUI, lớp BUS và lớp DAO.</summary>
    public class QuyDinhPhatDTO
    {
        /// <summary>Mã quy định phạt, tối đa 20 ký tự.</summary>
        public string MaQD { get; set; } = string.Empty;
        /// <summary>Tên quy định phạt, tối đa 100 ký tự.</summary>
        public string TenQD { get; set; } = string.Empty;
        /// <summary>Loại xử lý của quy định, chẳng hạn phạt tiền hoặc bồi thường.</summary>
        public string? LoaiPhat { get; set; }
        /// <summary>Cách xác định tiền phạt, chẳng hạn theo ngày trễ, cố định hoặc theo giá bìa.</summary>
        public string? CachTinh { get; set; }
        /// <summary>Đơn vị áp dụng cho mức phạt, chẳng hạn ngày, cuốn hoặc hệ số.</summary>
        public string? DonViTinh { get; set; }
        /// <summary>Mức phạt bằng VNĐ hoặc hệ số tùy theo cách tính; giá trị không âm.</summary>
        public decimal MucPhat { get; set; } = 0;

        /// <summary>Tạo DTO quy định phạt với các giá trị mặc định.</summary>
        public QuyDinhPhatDTO()
        {
        }

        /// <summary>Tạo DTO quy định phạt với đầy đủ cấu hình áp dụng.</summary>
        /// <param name="maQD">Mã quy định, tối đa 20 ký tự.</param>
        /// <param name="tenQD">Tên quy định, tối đa 100 ký tự.</param>
        /// <param name="loaiPhat">Loại xử lý theo danh sách được lớp BUS hỗ trợ.</param>
        /// <param name="cachTinh">Cách tính theo danh sách được lớp BUS hỗ trợ.</param>
        /// <param name="donViTinh">Đơn vị tính theo danh sách được lớp BUS hỗ trợ.</param>
        /// <param name="mucPhat">Số tiền VNĐ hoặc hệ số không âm theo cách tính.</param>
        public QuyDinhPhatDTO(string maQD, string tenQD, string? loaiPhat, string? cachTinh, string? donViTinh, decimal mucPhat)
        {
            MaQD = maQD;
            TenQD = tenQD;
            LoaiPhat = loaiPhat;
            CachTinh = cachTinh;
            DonViTinh = donViTinh;
            MucPhat = mucPhat;
        }
    }
}
