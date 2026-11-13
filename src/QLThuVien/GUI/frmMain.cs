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
            OpenChildForm(new frmMuonTraSach(), "Quản lý mượn - trả sách (Main - Sub)");
        }

        private void btnMenu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (sender is not Button button) return;

                Form formCon = button.Tag?.ToString() switch
                {
                    // 1. MƯỢN - TRẢ & ĐỘC GIẢ (Thành viên 3)
                    "MuonTraSach" => new frmMuonTraSach(),
                    "DocGia" => new frmDocGia(),
                    "LoaiDocGia" => new frmLoaiDocGia(),

                    // 2. SÁCH & DANH MỤC (Thành viên 1)
                    "DauSach" => new frmDauSach(),
                    "Sach" => new frmSach(),
                    "TheLoai" => new frmTheLoai(),
                    "TacGia" => new frmTacGia(),
                    "NhaXuatBan" => new frmNhaXuatBan(),

                    // 3. XỬ PHẠT (Thành viên 4)
                    "PhieuPhat" => new frmPhieuPhat(),
                    "QuyDinhPhat" => new frmQuyDinhPhat(),
                    "NhanVien" => new frmNhanVien(),

                    _ => throw new InvalidOperationException($"Không tìm thấy màn hình được chọn: {button.Tag}")
                };

                OpenChildForm(formCon, button.Text);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
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

        private Label TaoTieuDeNhom(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(12, 0, 0, 4),
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                BackColor = Color.FromArgb(24, 32, 47)
            };
        }

        private Button TaoNutMenu(string text, string maManHinh)
        {
            Button button = new Button
            {
                Text = text,
                Tag = maManHinh,
                Dock = DockStyle.Top,
                Height = 38,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(226, 232, 240),
                BackColor = Color.FromArgb(30, 41, 59),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Font = new Font("Segoe UI", 9.25F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 65, 85);
            button.Click += btnMenu_Click;
            return button;
        }
    }
}
