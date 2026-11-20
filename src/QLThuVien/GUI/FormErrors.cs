using System;
using System.Diagnostics;
using System.Windows.Forms;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    internal static class FormErrors
    {
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
