using System;
using System.Diagnostics;
using System.Windows.Forms;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    /// <summary>Lớp GUI dùng chung để chuyển lỗi nghiệp vụ hoặc lỗi hệ thống thành thông báo phù hợp.</summary>
    internal static class FormErrors
    {
        /// <summary>Hiển thị thông báo nghiệp vụ trực tiếp hoặc thông báo chung cho lỗi không dự kiến.</summary>
        /// <param name="exception">Lỗi phát sinh trong thao tác của biểu mẫu.</param>
        internal static void Show(Exception exception)
        {
            if (exception is BusinessRuleException or InvalidOperationException or ArgumentException or FormatException)
            {
                MessageBox.Show(exception.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Debug.WriteLine(exception);
            MessageBox.Show("Không thể xử lý dữ liệu, vui lòng thử lại.\nChi tiết: " + exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
