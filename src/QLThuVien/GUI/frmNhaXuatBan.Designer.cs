#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmNhaXuatBan
    {
        private TextBox txtMaNXB, txtTenNXB, txtDiaChi, txtTimKiem;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTaiLai, btnLamMoi;
        private DataGridView dgvNXB;
        private Label lblThongTin;

        private void InitializeComponent()
        {
            Text = "Quản lý Nhà xuất bản";
            ClientSize = new Size(1080, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            GroupBox grp = new GroupBox
            {
                Text = "Thông tin nhà xuất bản",
                Location = new Point(10, 5),
                Size = new Size(1060, 80),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            Font fn = new Font("Segoe UI", 10F, FontStyle.Regular);

            grp.Controls.Add(new Label { Text = "Mã NXB:", Location = new Point(15, 30), AutoSize = true, Font = fn });
            txtMaNXB = new TextBox { Location = new Point(95, 27), Size = new Size(120, 25), Font = fn };
            grp.Controls.Add(txtMaNXB);

            grp.Controls.Add(new Label { Text = "Tên NXB:", Location = new Point(230, 30), AutoSize = true, Font = fn });
            txtTenNXB = new TextBox { Location = new Point(315, 27), Size = new Size(200, 25), Font = fn };
            grp.Controls.Add(txtTenNXB);

            grp.Controls.Add(new Label { Text = "Địa chỉ:", Location = new Point(530, 30), AutoSize = true, Font = fn });
            txtDiaChi = new TextBox { Location = new Point(600, 27), Size = new Size(250, 25), Font = fn };
            grp.Controls.Add(txtDiaChi);

            btnThem = new Button { Text = "Thêm", Location = new Point(870, 25), Size = new Size(80, 30), Font = fn, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White };
            btnSua = new Button { Text = "Sửa", Location = new Point(955, 25), Size = new Size(80, 30), Font = fn, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;

            grp.Controls.AddRange(new Control[] { btnThem, btnSua });
            Controls.Add(grp);

            // TÌM KIẾM
            Panel pnl = new Panel { Location = new Point(10, 90), Size = new Size(1060, 40) };
            pnl.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(90, 7), Size = new Size(250, 25) };
            pnl.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(350, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };
            btnTimKiem.Click += btnTimKiem_Click;
            pnl.Controls.Add(btnTimKiem);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(440, 5), Size = new Size(110, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };
            btnTaiLai.Click += btnTaiLai_Click;
            pnl.Controls.Add(btnTaiLai);

            btnXoa = new Button { Text = "Xóa", Location = new Point(560, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White };
            btnXoa.Click += btnXoa_Click;
            pnl.Controls.Add(btnXoa);

            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(650, 5), Size = new Size(100, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };
            btnLamMoi.Click += (s, ev) => LamMoi();
            pnl.Controls.Add(btnLamMoi);

            lblThongTin = new Label { Location = new Point(770, 10), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnl.Controls.Add(lblThongTin);
            Controls.Add(pnl);

            // DATAGRIDVIEW
            dgvNXB = new DataGridView
            {
                Location = new Point(10, 135),
                Size = new Size(1060, 505),
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
            dgvNXB.SelectionChanged += dgvNXB_SelectionChanged;
            Controls.Add(dgvNXB);

            Load += NXBGUI_Load;

        }
    }
}
