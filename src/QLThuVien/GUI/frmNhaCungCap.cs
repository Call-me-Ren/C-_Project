using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Giao diện Quản lý Nhà Cung Cấp (Thành viên 2)
    /// Tuân thủ quy tắc 3 lớp: Chỉ gọi BUS, không gọi DAO hoặc SQL trực tiếp
    /// </summary>
    public class frmNhaCungCap : Form
    {
        private DataGridView dgvNCC = null!;
        private TextBox txtMaNCC = null!;
        private TextBox txtTenNCC = null!;
        private TextBox txtTimKiem = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        public frmNhaCungCap()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Nhà Cung Cấp";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // Tiêu đề
            Label lblHeader = new Label
            {
                Text = "QUẢN LÝ DANH MỤC NHÀ CUNG CẤP",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // GroupBox thông tin nhập liệu
            GroupBox gbThongTin = new GroupBox
            {
                Text = "Thông tin Nhà cung cấp",
                Dock = DockStyle.Top,
                Height = 135,
                Padding = new Padding(15)
            };

            Label lblMa = new Label { Text = "Mã NCC:", Location = new Point(30, 35), AutoSize = true };
            txtMaNCC = new TextBox { Location = new Point(110, 32), Width = 200 };

            Label lblTen = new Label { Text = "Tên NCC:", Location = new Point(30, 80), AutoSize = true };
            txtTenNCC = new TextBox { Location = new Point(110, 77), Width = 400 };

            // Panel chứa các nút chức năng
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Location = new Point(530, 25),
                Size = new Size(340, 95),
                FlowDirection = FlowDirection.LeftToRight
            };

            btnThem = new Button
            {
                Text = "➕ Thêm",
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnThem.Click += BtnThem_Click;

            btnSua = new Button
            {
                Text = "✏️ Sửa",
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(243, 156, 18),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnSua.Click += BtnSua_Click;

            btnXoa = new Button
            {
                Text = "🗑️ Xóa",
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnXoa.Click += BtnXoa_Click;

            btnLamMoi = new Button
            {
                Text = "🔄 Làm mới",
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlButtons.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            gbThongTin.Controls.AddRange(new Control[] { lblMa, txtMaNCC, lblTen, txtTenNCC, pnlButtons });

            // Panel tìm kiếm
            Panel pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(10, 8, 10, 8)
            };

            Label lblTim = new Label { Text = "🔍 Tìm kiếm (theo Mã hoặc Tên):", Location = new Point(30, 14), AutoSize = true };
            txtTimKiem = new TextBox { Location = new Point(250, 11), Width = 350 };
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;

            pnlSearch.Controls.AddRange(new Control[] { lblTim, txtTimKiem });

            // DataGridView
            dgvNCC = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                GridColor = Color.LightGray
            };
            dgvNCC.CellClick += DgvNCC_CellClick;

            this.Controls.Add(dgvNCC);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(gbThongTin);
            this.Controls.Add(lblHeader);
        }

        private void LoadData()
        {
            dgvNCC.DataSource = NhaCungCapBUS.GetAll();
            if (dgvNCC.Columns["MaNCC"] is DataGridViewColumn colMa) colMa.HeaderText = "Mã Nhà Cung Cấp";
            if (dgvNCC.Columns["TenNCC"] is DataGridViewColumn colTen) colTen.HeaderText = "Tên Nhà Cung Cấp";
        }

        private void DgvNCC_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNCC.Rows[e.RowIndex].DataBoundItem is NhaCungCapDTO item)
            {
                txtMaNCC.Text = item.MaNCC;
                txtTenNCC.Text = item.TenNCC;
                txtMaNCC.ReadOnly = true; // Khi chọn dòng cũ thì không cho sửa Mã PK
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
            var result = NhaCungCapBUS.Them(ncc);
            MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.Success)
            {
                LoadData();
                BtnLamMoi_Click(sender, e);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
            var result = NhaCungCapBUS.Sua(ncc);
            MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.Success)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{txtMaNCC.Text}' không?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                var result = NhaCungCapBUS.Xoa(txtMaNCC.Text);
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    LoadData();
                    BtnLamMoi_Click(sender, e);
                }
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtTimKiem.Text = "";
            txtMaNCC.ReadOnly = false;
            txtMaNCC.Focus();
            LoadData();
        }

        private void TxtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            dgvNCC.DataSource = NhaCungCapBUS.TimKiem(txtTimKiem.Text);
        }
    }
}
