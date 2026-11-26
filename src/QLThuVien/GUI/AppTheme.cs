using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Hệ thống màu sắc và kiểu dáng thống nhất cho toàn bộ giao diện WinForms,
    /// đồng bộ với cổng Web (Slate #0F172A/#1E293B + Blue #2563EB + Canvas #F8FAFC).
    /// </summary>
    internal static class AppTheme
    {
        public static readonly Color Canvas = Color.FromArgb(248, 250, 252);       // #F8FAFC
        public static readonly Color Surface = Color.White;                         // #FFFFFF
        public static readonly Color Sidebar = Color.FromArgb(30, 41, 59);          // #1E293B
        public static readonly Color SidebarDark = Color.FromArgb(15, 23, 42);      // #0F172A
        public static readonly Color Primary = Color.FromArgb(37, 99, 235);         // #2563EB
        public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);    // #1D4ED8
        public static readonly Color PrimarySoft = Color.FromArgb(239, 246, 255);   // #EFF6FF
        public static readonly Color SelectionBg = Color.FromArgb(219, 234, 254);   // #DBEAFE
        public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);      // #0F172A
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);     // #64748B
        public static readonly Color Border = Color.FromArgb(226, 232, 240);        // #E2E8F0

        // Semantic Button Colors
        private static readonly Color WarningBg = Color.FromArgb(217, 119, 6);      // #D97706 (Sửa / Cập nhật)
        private static readonly Color WarningHover = Color.FromArgb(180, 83, 9);
        private static readonly Color DangerBg = Color.FromArgb(220, 38, 38);       // #DC2626 (Xóa / Từ chối)
        private static readonly Color DangerHover = Color.FromArgb(185, 28, 28);
        private static readonly Color SuccessBg = Color.FromArgb(22, 163, 74);      // #16A34A (Excel / Duyệt)
        private static readonly Color SuccessHover = Color.FromArgb(21, 128, 61);
        private static readonly Color DarkActionBg = Color.FromArgb(51, 65, 85);    // #334155 (PDF / In)
        private static readonly Color DarkActionHover = Color.FromArgb(30, 41, 59);

        public static void ApplyToForm(Form form)
        {
            if (form == null) return;

            form.BackColor = Canvas;
            form.ForeColor = TextPrimary;
            if (form.Font.Name != "Segoe UI")
            {
                form.Font = new Font("Segoe UI", 9.5F);
            }

            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case DataGridView dgv:
                        StyleDataGridView(dgv);
                        break;
                    case GroupBox gb:
                        gb.ForeColor = TextPrimary;
                        gb.BackColor = Surface;
                        break;
                    case Button btn:
                        StyleButton(btn);
                        break;
                    case TextBoxBase tb:
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        if (!tb.ReadOnly && tb.Enabled) tb.BackColor = Surface;
                        break;
                }

                if (ctrl.HasChildren)
                {
                    ApplyToControls(ctrl.Controls);
                }
            }
        }

        private static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Surface;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Border;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            if (dgv.ColumnHeadersHeight < 34) dgv.ColumnHeadersHeight = 34;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Canvas;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextMuted;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Canvas;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextMuted;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            dgv.DefaultCellStyle.BackColor = Surface;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = SelectionBg;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Canvas;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectionBg;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;

            if (dgv.RowTemplate.Height < 30) dgv.RowTemplate.Height = 30;
        }

        private static void StyleButton(Button btn)
        {
            btn.Cursor = Cursors.Hand;
            btn.FlatStyle = FlatStyle.Flat;

            string text = (btn.Text ?? string.Empty).Trim().ToLowerInvariant();
            if (text == "..." || text == "×" || text == "x")
            {
                ApplyNeutralButton(btn);
                return;
            }

            if (text.Contains("excel") || text.Contains("xls") || text.Contains("thêm") || text.Contains("tạo"))
            {
                ApplySolidButton(btn, SuccessBg, SuccessHover);
            }
            else if (text.Contains("pdf") || text.Contains("in phiếu") || text.Contains("báo cáo"))
            {
                ApplySolidButton(btn, DarkActionBg, DarkActionHover);
            }
            else if (text.Contains("xóa") || text.Contains("hủy") || text.Contains("từ chối") || text.Contains("báo mất"))
            {
                ApplySolidButton(btn, DangerBg, DangerHover);
            }
            else if (text.Contains("sửa") || text.Contains("cập nhật"))
            {
                ApplySolidButton(btn, WarningBg, WarningHover);
            }
            else if (text.Contains("lưu") || text.Contains("tìm")
                  || text.Contains("duyệt") || text.Contains("mượn") || text.Contains("trả") || text.Contains("xác nhận")
                  || text.Contains("lập") || text.Contains("chọn") || text.Contains("thống kê"))
            {
                ApplySolidButton(btn, Primary, PrimaryHover);
            }
            else
            {
                ApplyNeutralButton(btn);
            }
        }

        private static void ApplySolidButton(Button btn, Color backColor, Color hoverColor)
        {
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
        }

        private static void ApplyNeutralButton(Button btn)
        {
            btn.BackColor = Surface;
            btn.ForeColor = TextPrimary;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Border;
            btn.FlatAppearance.MouseOverBackColor = Canvas;
        }
    }
}
