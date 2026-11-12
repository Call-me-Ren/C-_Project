#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmTheLoai
    {
        private TextBox txtMaTL, txtTenTL, txtTimKiem;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTaiLai, btnLamMoi;
        private DataGridView dgvTheLoai;
        private Label lblThongTin;

        private void InitializeComponent()
        {
            Text = "Quản lý Thể loại";
            ClientSize = new Size(1080, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            GroupBox grp = new GroupBox
            {
                Text = "Thông tin thể loại",
                Location = new Point(10, 5),
                Size = new Size(1060, 80),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            Font fn = new Font("Segoe UI", 10F, FontStyle.Regular);

            grp.Controls.Add(new Label { Text = "Mã TL:", Location = new Point(15, 30), AutoSize = true, Font = fn });
            txtMaTL = new TextBox { Location = new Point(80, 27), Size = new Size(120, 25), Font = fn };
            grp.Controls.Add(txtMaTL);

            grp.Controls.Add(new Label { Text = "Tên thể loại:", Location = new Point(220, 30), AutoSize = true, Font = fn });
            txtTenTL = new TextBox { Location = new Point(330, 27), Size = new Size(250, 25), Font = fn };
            grp.Controls.Add(txtTenTL);

            btnThem = new Button { Text = "Thêm", Location = new Point(610, 25), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White };
            btnSua = new Button { Text = "Sửa", Location = new Point(710, 25), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };
            btnXoa = new Button { Text = "Xóa", Location = new Point(810, 25), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(910, 25), Size = new Size(100, 30), Font = fn, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += (s, ev) => LamMoi();

            grp.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
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

            lblThongTin = new Label { Location = new Point(570, 10), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnl.Controls.Add(lblThongTin);
            Controls.Add(pnl);

            // DATAGRIDVIEW
            dgvTheLoai = new DataGridView
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
            dgvTheLoai.SelectionChanged += dgvTheLoai_SelectionChanged;
            Controls.Add(dgvTheLoai);

            Load += TheLoaiGUI_Load;

        }
    }
}
