#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public class frmTimKiemPopup : Form
    {
        private TextBox txtMaDauSach, txtTenSach, txtSoLuong, txtNamXB, txtMaTG, txtMaTL;
        private Button btnTim, btnHuy;

        public string MaDauSach => string.IsNullOrWhiteSpace(txtMaDauSach.Text) ? null : txtMaDauSach.Text.Trim();
        public string TenSach => string.IsNullOrWhiteSpace(txtTenSach.Text) ? null : txtTenSach.Text.Trim();
        public int? SoLuong => int.TryParse(txtSoLuong.Text.Trim(), out int sl) ? sl : null;
        public int? NamXB => int.TryParse(txtNamXB.Text.Trim(), out int nxb) ? nxb : null;
        public string MaTG => string.IsNullOrWhiteSpace(txtMaTG.Text) ? null : txtMaTG.Text.Trim();
        public string MaTL => string.IsNullOrWhiteSpace(txtMaTL.Text) ? null : txtMaTL.Text.Trim();

        public frmTimKiemPopup()
        {
            Text = "Tìm kiếm nâng cao";
            Size = new Size(360, 310);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Font f = new Font("Segoe UI", 9.5F);

            Controls.Add(new Label { Text = "Mã đầu sách:", Location = new Point(20, 20), AutoSize = true, Font = f });
            txtMaDauSach = new TextBox { Location = new Point(130, 17), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaDauSach);

            Controls.Add(new Label { Text = "Tên sách:", Location = new Point(20, 55), AutoSize = true, Font = f });
            txtTenSach = new TextBox { Location = new Point(130, 52), Size = new Size(180, 24), Font = f };
            Controls.Add(txtTenSach);

            Controls.Add(new Label { Text = "Số lượng:", Location = new Point(20, 90), AutoSize = true, Font = f });
            txtSoLuong = new TextBox { Location = new Point(130, 87), Size = new Size(180, 24), Font = f };
            Controls.Add(txtSoLuong);

            Controls.Add(new Label { Text = "Năm XB:", Location = new Point(20, 125), AutoSize = true, Font = f });
            txtNamXB = new TextBox { Location = new Point(130, 122), Size = new Size(180, 24), Font = f };
            Controls.Add(txtNamXB);

            Controls.Add(new Label { Text = "Mã Tác giả:", Location = new Point(20, 160), AutoSize = true, Font = f });
            txtMaTG = new TextBox { Location = new Point(130, 157), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaTG);

            Controls.Add(new Label { Text = "Mã Thể loại:", Location = new Point(20, 195), AutoSize = true, Font = f });
            txtMaTL = new TextBox { Location = new Point(130, 192), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaTL);

            btnTim = new Button { Text = "Tìm kiếm", Location = new Point(130, 235), Size = new Size(85, 30), DialogResult = DialogResult.OK, Font = f, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnHuy = new Button { Text = "Hủy", Location = new Point(225, 235), Size = new Size(85, 30), DialogResult = DialogResult.Cancel, Font = f, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTim.FlatAppearance.BorderSize = 0;
            btnHuy.FlatAppearance.BorderSize = 0;

            Controls.Add(btnTim);
            Controls.Add(btnHuy);
            AcceptButton = btnTim;
            CancelButton = btnHuy;
        }
    }
}
