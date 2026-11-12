using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    partial class frmMain
    {
        private Panel pnlMenuLeft;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Label lblAppName;
        private Label lblTitle;
        private Button btnThoat;

        private void InitializeComponent()
        {
            pnlMenuLeft = new Panel();
            pnlHeader = new Panel();
            pnlContent = new Panel();
            lblAppName = new Label();
            lblTitle = new Label();
            btnThoat = new Button();
            SuspendLayout();

            pnlMenuLeft.BackColor = Color.FromArgb(30, 41, 59);
            pnlMenuLeft.Dock = DockStyle.Left;
            pnlMenuLeft.Width = 225;
            pnlMenuLeft.Padding = new Padding(0);

            lblAppName.Text = "QUẢN LÝ THƯ VIỆN";
            lblAppName.Dock = DockStyle.Top;
            lblAppName.Height = 72;
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;
            lblAppName.ForeColor = Color.White;
            lblAppName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            pnlMenuLeft.Controls.Add(TaoNutMenu("Sách", "Sach"));
            pnlMenuLeft.Controls.Add(TaoNutMenu("Đầu sách", "DauSach"));
            pnlMenuLeft.Controls.Add(TaoNutMenu("Nhà xuất bản", "NhaXuatBan"));
            pnlMenuLeft.Controls.Add(TaoNutMenu("Tác giả", "TacGia"));
            pnlMenuLeft.Controls.Add(TaoNutMenu("Thể loại", "TheLoai"));
            pnlMenuLeft.Controls.Add(lblAppName);

            btnThoat.Text = "Thoát hệ thống";
            btnThoat.Dock = DockStyle.Bottom;
            btnThoat.Height = 48;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.ForeColor = Color.FromArgb(255, 112, 112);
            btnThoat.BackColor = Color.FromArgb(15, 23, 42);
            btnThoat.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThoat.Click += btnThoat_Click;
            pnlMenuLeft.Controls.Add(btnThoat);

            pnlHeader.BackColor = Color.White;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 64;
            pnlHeader.Padding = new Padding(24, 0, 0, 0);
            pnlHeader.BorderStyle = BorderStyle.FixedSingle;

            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Text = "Quản lý thư viện";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            pnlHeader.Controls.Add(lblTitle);

            pnlContent.BackColor = Color.FromArgb(241, 245, 249);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(12);

            Text = "Quản lý thư viện";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1100, 700);
            ClientSize = new Size(1280, 800);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlMenuLeft);
            Name = "frmMain";
            ResumeLayout(false);
        }
    }
}
