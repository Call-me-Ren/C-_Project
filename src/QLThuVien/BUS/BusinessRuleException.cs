namespace QLThuVien.BUS
{
    /// <summary>Biểu diễn lỗi vi phạm quy tắc nghiệp vụ do lớp BUS phát hiện và chuyển cho lớp GUI xử lý.</summary>
    public sealed class BusinessRuleException : Exception
    {
        /// <summary>Tạo lỗi nghiệp vụ với nội dung thông báo dành cho người gọi.</summary>
        /// <param name="message">Nội dung lỗi, không nên chứa thông tin kỹ thuật của cơ sở dữ liệu.</param>
        public BusinessRuleException(string message) : base(message)
        {
        }

        /// <summary>Tạo lỗi nghiệp vụ và giữ lại nguyên nhân kỹ thuật phát sinh trước đó.</summary>
        /// <param name="message">Nội dung nghiệp vụ dùng để giải thích lỗi cho lớp gọi.</param>
        /// <param name="innerException">Lỗi gốc cần lưu để phục vụ ghi nhận và chẩn đoán.</param>
        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
