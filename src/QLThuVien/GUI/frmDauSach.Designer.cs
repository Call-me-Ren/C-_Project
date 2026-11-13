#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmDauSach
    {
        private TextBox txtMaDauSach, txtTenSach, txtMaTG, txtMaTL, txtMaNXB, txtNamXB, txtSoLuong, txtDonGia;
        private TextBox txtTimKiem;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTimNangCao, btnTaiLai, btnLamMoi, btnXuatExcel, btnNhapExcel;
        private Button btnChonTG, btnChonTL, btnChonNXB;
        private DataGridView dgvDauSach, dgvSach;
        private Label lblThongTin;
        private ComboBox cboTrangThai;

        private void InitializeComponent()
        {
            Text = "Quản lý Đầu sách & Sách";
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9.5F);

            // GroupBox Nhập Liệu Căn Chỉnh Chuẩn
            GroupBox grpNhapLieu = new GroupBox
            {
                Text = "Thông tin đầu sách",
                Location = new Point(10, 5),
                Size = new Size(1160, 135),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            Font fontNormal = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // HÀNG 1: Mã đầu sách | Tên sách | Đơn giá | Số lượng
            grpNhapLieu.Controls.Add(new Label { Text = "Mã đầu sách:", Location = new Point(12, 28), AutoSize = true, Font = fontNormal });
            txtMaDauSach = new TextBox { Location = new Point(110, 25), Size = new Size(110, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaDauSach);

            grpNhapLieu.Controls.Add(new Label { Text = "Tên sách:", Location = new Point(235, 28), AutoSize = true, Font = fontNormal });
            txtTenSach = new TextBox { Location = new Point(305, 25), Size = new Size(230, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtTenSach);

            grpNhapLieu.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(550, 28), AutoSize = true, Font = fontNormal });
            txtDonGia = new TextBox { Location = new Point(615, 25), Size = new Size(125, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtDonGia);

            grpNhapLieu.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(760, 28), AutoSize = true, Font = fontNormal });
            txtSoLuong = new TextBox { Location = new Point(830, 25), Size = new Size(70, 24), Font = fontNormal, ReadOnly = true, BackColor = Color.FromArgb(238, 242, 246) };
            grpNhapLieu.Controls.Add(txtSoLuong);


            // HÀNG 2: ĐÃ CĂN TỌA ĐỘ RỘNG RÃI CHỐNG ĐÈ CHỮ
            // 1. Mã tác giả
            grpNhapLieu.Controls.Add(new Label { Text = "Mã tác giả:", Location = new Point(12, 63), AutoSize = true, Font = fontNormal });
            txtMaTG = new TextBox { Location = new Point(95, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaTG);
            btnChonTG = CreateLookupButton(173, 59);
            btnChonTG.Click += btnChonTG_Click;
            grpNhapLieu.Controls.Add(btnChonTG);

            // 2. Mã thể loại (Đã nới rộng nhãn sang X=215, ô TextBox sang X=300 để không che chữ T)
            grpNhapLieu.Controls.Add(new Label { Text = "Mã thể loại:", Location = new Point(205, 63), AutoSize = true, Font = fontNormal });
            txtMaTL = new TextBox { Location = new Point(300, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaTL);
            btnChonTL = CreateLookupButton(378, 59);
            btnChonTL.Click += btnChonTL_Click;
            grpNhapLieu.Controls.Add(btnChonTL);

            // 3. Mã NXB (Dời X sang 420, ô TextBox sang 485)
            grpNhapLieu.Controls.Add(new Label { Text = "Mã NXB:", Location = new Point(420, 63), AutoSize = true, Font = fontNormal });
            txtMaNXB = new TextBox { Location = new Point(485, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaNXB);
            btnChonNXB = CreateLookupButton(563, 59);
            btnChonNXB.Click += btnChonNXB_Click;
            grpNhapLieu.Controls.Add(btnChonNXB);

            // 4. Năm XB (Dời X sang 605, ô TextBox sang 670)
            grpNhapLieu.Controls.Add(new Label { Text = "Năm XB:", Location = new Point(605, 63), AutoSize = true, Font = fontNormal });
            txtNamXB = new TextBox { Location = new Point(670, 60), Size = new Size(70, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtNamXB);


            // HÀNG 3: Nút bấm
            btnThem = new Button { Text = "Thêm", Location = new Point(12, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSua = new Button { Text = "Sửa", Location = new Point(105, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnXoa = new Button { Text = "Xóa", Location = new Point(198, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(291, 96), Size = new Size(95, 30), Font = fontNormal, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

            btnThem.FlatAppearance.BorderSize = 0;
            btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatAppearance.BorderSize = 0;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;

            grpNhapLieu.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            Controls.Add(grpNhapLieu);

            // Panel Tìm kiếm
            Panel pnlTimKiem = new Panel
            {
                Location = new Point(10, 145),
                Size = new Size(1160, 40)
            };

            pnlTimKiem.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(85, 7), Size = new Size(250, 24) };
            pnlTimKiem.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(345, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += btnTimKiem_Click;
            pnlTimKiem.Controls.Add(btnTimKiem);

            btnTimNangCao = new Button { Text = "Tìm nâng cao", Location = new Point(433, 5), Size = new Size(120, 30), BackColor = Color.FromArgb(155, 89, 182), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTimNangCao.FlatAppearance.BorderSize = 0;
            btnTimNangCao.Click += btnTimNangCao_Click;
            pnlTimKiem.Controls.Add(btnTimNangCao);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(561, 5), Size = new Size(100, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTaiLai.FlatAppearance.BorderSize = 0;
            btnTaiLai.Click += btnTaiLai_Click;
            pnlTimKiem.Controls.Add(btnTaiLai);

            btnXuatExcel = new Button { Text = "Xuất Excel (.xlsx)", Location = new Point(855, 5), Size = new Size(145, 30), BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.Click += btnXuatExcel_Click;
            pnlTimKiem.Controls.Add(btnXuatExcel);

            btnNhapExcel = new Button { Text = "Nhập Excel (.xlsx)", Location = new Point(1005, 5), Size = new Size(145, 30), BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnNhapExcel.FlatAppearance.BorderSize = 0;
            btnNhapExcel.Click += btnNhapExcel_Click;
            pnlTimKiem.Controls.Add(btnNhapExcel);

            lblThongTin = new Label { Location = new Point(680, 10), AutoSize = true, ForeColor = Color.FromArgb(30, 41, 59), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            pnlTimKiem.Controls.Add(lblThongTin);

            Controls.Add(pnlTimKiem);

            // DataGridView Đầu Sách
            dgvDauSach = new DataGridView
            {
                Location = new Point(10, 190),
                Size = new Size(1160, 310),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvDauSach.SelectionChanged += dgvDauSach_SelectionChanged;
            Controls.Add(dgvDauSach);

            // Panel Lọc Cuốn Sách
            Panel pnlSach = new Panel
            {
                Location = new Point(10, 505),
                Size = new Size(1160, 30)
            };
            pnlSach.Controls.Add(new Label { Text = "Danh sách cuốn sách - lọc theo trạng thái:", Location = new Point(5, 5), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });

            cboTrangThai = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(365, 2),
                Size = new Size(160, 24)
            };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa cho mượn", "Đã cho mượn" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
            pnlSach.Controls.Add(cboTrangThai);
            Controls.Add(pnlSach);

            // DataGridView Cuốn Sách
            dgvSach = new DataGridView
            {
                Location = new Point(10, 538),
                Size = new Size(1160, 205),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(dgvSach);

            Load += DauSachGUI_Load;

        }
    }
}
