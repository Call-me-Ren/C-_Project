using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    partial class frmMain
    {
        private Panel pnlMenuLeft;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlMenuContainer;
        private Label lblAppName;
        private Label lblTitle;
        private Button btnThoat;

        private void InitializeComponent()
        {
            pnlMenuLeft = new Panel();
            pnlMenuContainer = new Panel();
            lblAppName = new Label();
            btnThoat = new Button();
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlContent = new Panel();
            SuspendLayout();

            // 
            // pnlMenuLeft
            // 
            pnlMenuLeft.BackColor = Color.FromArgb(30, 41, 59);
            pnlMenuLeft.Dock = DockStyle.Left;
            pnlMenuLeft.Width = 240;
            pnlMenuLeft.Controls.Add(pnlMenuContainer);
            pnlMenuLeft.Controls.Add(btnThoat);
            pnlMenuLeft.Controls.Add(lblAppName);

            // 
            // lblAppName
            // 
            lblAppName.Text = "📚 QL THƯ VIỆN";
            lblAppName.Dock = DockStyle.Top;
            lblAppName.Height = 60;
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;
            lblAppName.ForeColor = Color.White;
            lblAppName.BackColor = Color.FromArgb(15, 23, 42);
            lblAppName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // 
            // btnThoat
            // 
            btnThoat.Text = "🚪  Thoát hệ thống";
            btnThoat.Dock = DockStyle.Bottom;
            btnThoat.Height = 44;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.ForeColor = Color.FromArgb(248, 113, 113);
            btnThoat.BackColor = Color.FromArgb(15, 23, 42);
            btnThoat.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThoat.Cursor = Cursors.Hand;
            btnThoat.Click += btnThoat_Click;

            // 
            // pnlMenuContainer
            // 
            pnlMenuContainer.Dock = DockStyle.Fill;
            pnlMenuContainer.AutoScroll = true;
            pnlMenuContainer.BackColor = Color.FromArgb(30, 41, 59);

            // Thêm các mục menu và tiêu đề nhóm theo thứ tự hiển thị từ trên xuống
            ThemVaoMenu(TaoTieuDeNhom("MƯỢN - TRẢ & ĐỘC GIẢ"));
            ThemVaoMenu(TaoNutMenu("📖  Mượn - Trả sách", "MuonTraSach"));
            ThemVaoMenu(TaoNutMenu("👥  Quản lý độc giả", "DocGia"));
            ThemVaoMenu(TaoNutMenu("🏷️  Loại độc giả", "LoaiDocGia"));

            ThemVaoMenu(TaoTieuDeNhom("SÁCH & DANH MỤC"));
            ThemVaoMenu(TaoNutMenu("📚  Quản lý đầu sách", "DauSach"));
            ThemVaoMenu(TaoNutMenu("📑  Quản lý cuốn sách", "Sach"));
            ThemVaoMenu(TaoNutMenu("🗂️  Thể loại sách", "TheLoai"));
            ThemVaoMenu(TaoNutMenu("✍️  Tác giả", "TacGia"));
            ThemVaoMenu(TaoNutMenu("🏢  Nhà xuất bản", "NhaXuatBan"));

            ThemVaoMenu(TaoTieuDeNhom("XỬ PHẠT"));
            ThemVaoMenu(TaoNutMenu("⚖️  Xử lý phiếu phạt", "PhieuPhat"));
            ThemVaoMenu(TaoNutMenu("📜  Quy định xử phạt", "QuyDinhPhat"));
            ThemVaoMenu(TaoNutMenu("🧑‍💼  Quản lý nhân viên", "NhanVien"));

            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 56;
            pnlHeader.Padding = new Padding(20, 0, 0, 0);
            pnlHeader.BorderStyle = BorderStyle.FixedSingle;
            pnlHeader.Controls.Add(lblTitle);

            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Text = "Quản lý mượn - trả sách (Main - Sub)";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 41, 59);

            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(241, 245, 249);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(10);

            // 
            // frmMain
            // 
            Text = "Hệ Thống Quản Lý Thư Viện Đại Học Sài Gòn (SGU)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 750);
            ClientSize = new Size(1366, 820);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlMenuLeft);
            Name = "frmMain";
            ResumeLayout(false);
        }

        private void ThemVaoMenu(Control control)
        {
            pnlMenuContainer.Controls.Add(control);
            control.SendToBack();
        }
    }
}
