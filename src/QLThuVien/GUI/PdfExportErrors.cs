using System;
using System.IO;
using Microsoft.Data.SqlClient;

namespace QLThuVien.GUI
{
    internal static class PdfExportErrors
    {
        public static string ToVietnamese(Exception exception)
        {
            for (Exception? current = exception; current != null; current = current.InnerException)
            {
                if (current is SqlException)
                    return "Không đọc được dữ liệu từ cơ sở dữ liệu. Hãy thử lại sau hoặc báo cho người quản trị.";
            }

            if (exception is UnauthorizedAccessException)
                return "Bạn không có quyền lưu tệp ở thư mục này. Hãy chọn thư mục khác hoặc kiểm tra quyền ghi.";

            if (exception is DirectoryNotFoundException)
                return "Thư mục lưu không tồn tại. Hãy chọn một thư mục khác.";

            if (exception is PathTooLongException || exception is ArgumentException)
                return "Tên tệp không hợp lệ. Hãy chọn tên tệp ngắn hơn và không chứa ký tự đặc biệt.";

            if (exception is IOException)
            {
                int nativeCode = exception.HResult & 0xFFFF;
                if (nativeCode is 32 or 33)
                    return "Không lưu được vì tệp này đang được mở ở chương trình khác. Hãy đóng tệp đó rồi thử lại, hoặc chọn tên tệp khác.";
                if (nativeCode is 39 or 112)
                    return "Ổ đĩa không còn đủ dung lượng. Hãy giải phóng dung lượng rồi thử lại.";
                if (nativeCode is 123 or 206)
                    return "Tên tệp không hợp lệ. Hãy chọn tên tệp ngắn hơn và không chứa ký tự đặc biệt.";
            }

            return "Không thể xuất báo cáo lúc này. Hãy thử lại, nếu vẫn lỗi hãy báo cho người quản trị.";
        }
    }
}
