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
            if (exception is BusinessRuleException business)
            {
                MessageBox.Show(business.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Debug.WriteLine(exception);
            MessageBox.Show("Không thể lưu dữ liệu, vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
