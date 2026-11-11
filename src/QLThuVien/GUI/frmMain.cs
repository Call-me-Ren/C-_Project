using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmMain : Form
    {
        private Form? formConDangMo;

        public frmMain()
        {
            InitializeComponent();
            OpenChildForm(new frmDauSach(), "Quản lý đầu sách");
        }

        private void btnMenu_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            Form formCon = button.Tag?.ToString() switch
            {
                "TheLoai" => new frmTheLoai(),
                "TacGia" => new frmTacGia(),
                "NhaXuatBan" => new frmNhaXuatBan(),
                "DauSach" => new frmDauSach(),
                "Sach" => new frmSach(),
                "NhanVien" => new frmNhanVien(),
                "PhieuPhat" => new frmPhieuPhat(),
                "QuyDinhPhat" => new frmQuyDinhPhat(),
                _ => throw new InvalidOperationException("Không tìm thấy màn hình được chọn.")
            };

            OpenChildForm(formCon, button.Text);
        }

        private void btnThoat_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát hệ thống?",
                "Thoát hệ thống",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes) Application.Exit();
        }

        private void OpenChildForm(Form childForm, string title)
        {
            if (formConDangMo != null)
            {
                pnlContent.Controls.Remove(formConDangMo);
                formConDangMo.Close();
                formConDangMo.Dispose();
            }

            formConDangMo = childForm;
            lblTitle.Text = title;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.Show();
            childForm.BringToFront();
        }

        private Button TaoNutMenu(string text, string maManHinh)
        {
            Button button = new Button
            {
                Text = text,
                Tag = maManHinh,
                Dock = DockStyle.Top,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(37, 52, 72),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0),
                Font = new Font("Segoe UI", 9.5F)
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += btnMenu_Click;
            return button;
        }
    }
}
