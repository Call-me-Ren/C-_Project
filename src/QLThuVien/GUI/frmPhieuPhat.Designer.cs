#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmPhieuPhat
    {
        private Label lblTitle = null!;
        private GroupBox grpPhieu = null!;
        private Label lblMaPhieu = null!;
        private TextBox txtMaPhieuPhat = null!;
        private Label lblNgayLap = null!;
        private DateTimePicker dtpNgayLap = null!;
        private Label lblTrangThai = null!;
        private ComboBox cboTrangThai = null!;
        private Label lblDocGia = null!;
        private ComboBox cboDocGia = null!;
        private Label lblPhieuMuon = null!;
        private ComboBox cboPhieuMuon = null!;
        private Label lblNhanVien = null!;
        private ComboBox cboNhanVien = null!;
        private Label lblTongTienPhat = null!;
        private TextBox txtTongTienPhat = null!;
        private Button btnThemPhieu = null!;
        private Button btnSuaPhieu = null!;
        private Button btnXoaPhieu = null!;
        private Button btnChiTiet = null!;
        private Button btnLamMoi = null!;
        private TextBox txtTimKiem = null!;
        private Button btnTimKiem = null!;
        private Button btnTatCa = null!;
        private DataGridView dgvPhieuPhat = null!;

        private void InitializeComponent()
        {
            lblTitle = new Label();
            grpPhieu = new GroupBox();
            lblMaPhieu = new Label();
            txtMaPhieuPhat = new TextBox();
            lblNgayLap = new Label();
            dtpNgayLap = new DateTimePicker();
            lblTrangThai = new Label();
            cboTrangThai = new ComboBox();
            lblDocGia = new Label();
            cboDocGia = new ComboBox();
            lblPhieuMuon = new Label();
            cboPhieuMuon = new ComboBox();
            lblNhanVien = new Label();
            cboNhanVien = new ComboBox();
            lblTongTienPhat = new Label();
            txtTongTienPhat = new TextBox();
            btnThemPhieu = new Button();
            btnSuaPhieu = new Button();
            btnXoaPhieu = new Button();
            btnChiTiet = new Button();
            btnLamMoi = new Button();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnTatCa = new Button();
            dgvPhieuPhat = new DataGridView();
            grpPhieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuPhat).BeginInit();
            SuspendLayout();

            Text = "Quản lý phiếu phạt";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 760);
            MinimumSize = new Size(1000, 680);
            Font = new Font("Segoe UI", 9F);

            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.Location = new Point(18, 12);
            lblTitle.Size = new Size(1060, 34);
            lblTitle.Text = "QUẢN LÝ PHIẾU PHẠT";

            grpPhieu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpPhieu.Location = new Point(16, 54);
            grpPhieu.Size = new Size(1088, 205);
            grpPhieu.Text = "Thông tin phiếu phạt";

            lblMaPhieu.AutoSize = true;
            lblMaPhieu.Location = new Point(18, 34);
            lblMaPhieu.Text = "Mã phiếu phạt:";
            txtMaPhieuPhat.Location = new Point(115, 30);
            txtMaPhieuPhat.Size = new Size(190, 23);
            txtMaPhieuPhat.ReadOnly = true;
            txtMaPhieuPhat.TabStop = false;

            lblNgayLap.AutoSize = true;
            lblNgayLap.Location = new Point(335, 34);
            lblNgayLap.Text = "Ngày lập:";
            dtpNgayLap.Format = DateTimePickerFormat.Short;
            dtpNgayLap.MaxDate = DateTime.Today;
            dtpNgayLap.Location = new Point(410, 30);
            dtpNgayLap.Size = new Size(180, 23);

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(630, 34);
            lblTrangThai.Text = "Trạng thái:";
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[] { "Chưa thanh toán", "Đã thanh toán" });
            cboTrangThai.Location = new Point(710, 30);
            cboTrangThai.Size = new Size(210, 23);

            lblDocGia.AutoSize = true;
            lblDocGia.Location = new Point(18, 73);
            lblDocGia.Text = "Độc giả:";
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(115, 69);
            cboDocGia.Size = new Size(360, 23);

            lblPhieuMuon.AutoSize = true;
            lblPhieuMuon.Location = new Point(505, 73);
            lblPhieuMuon.Text = "Phiếu mượn:";
            cboPhieuMuon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhieuMuon.Location = new Point(600, 69);
            cboPhieuMuon.Size = new Size(440, 23);

            lblNhanVien.AutoSize = true;
            lblNhanVien.Location = new Point(18, 112);
            lblNhanVien.Text = "Nhân viên:";
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Location = new Point(115, 108);
            cboNhanVien.Size = new Size(360, 23);

            lblTongTienPhat.AutoSize = true;
            lblTongTienPhat.Location = new Point(505, 112);
            lblTongTienPhat.Text = "Tổng tiền phạt:";
            txtTongTienPhat.Location = new Point(600, 108);
            txtTongTienPhat.Size = new Size(180, 23);
            txtTongTienPhat.ReadOnly = true;
            txtTongTienPhat.TextAlign = HorizontalAlignment.Right;

            btnThemPhieu.Location = new Point(18, 151);
            btnThemPhieu.Size = new Size(100, 30);
            btnThemPhieu.Text = "Thêm";
            btnSuaPhieu.Location = new Point(126, 151);
            btnSuaPhieu.Size = new Size(100, 30);
            btnSuaPhieu.Text = "Sửa";
            btnXoaPhieu.Location = new Point(234, 151);
            btnXoaPhieu.Size = new Size(100, 30);
            btnXoaPhieu.Text = "Xóa";
            btnChiTiet.Location = new Point(342, 151);
            btnChiTiet.Size = new Size(165, 30);
            btnChiTiet.Text = "Chi tiết vi phạm";
            btnChiTiet.Enabled = false;
            btnLamMoi.Location = new Point(515, 151);
            btnLamMoi.Size = new Size(110, 30);
            btnLamMoi.Text = "Làm mới";

            grpPhieu.Controls.AddRange(new Control[]
            {
                lblMaPhieu, txtMaPhieuPhat, lblNgayLap, dtpNgayLap, lblTrangThai, cboTrangThai,
                lblDocGia, cboDocGia, lblPhieuMuon, cboPhieuMuon, lblNhanVien, cboNhanVien,
                lblTongTienPhat, txtTongTienPhat, btnThemPhieu, btnSuaPhieu, btnXoaPhieu,
                btnChiTiet, btnLamMoi
            });

            txtTimKiem.Location = new Point(20, 276);
            txtTimKiem.Size = new Size(360, 23);
            txtTimKiem.PlaceholderText = "Tìm phiếu, độc giả, phiếu mượn";
            btnTimKiem.Location = new Point(390, 273);
            btnTimKiem.Size = new Size(80, 29);
            btnTimKiem.Text = "Tìm";
            btnTatCa.Location = new Point(478, 273);
            btnTatCa.Size = new Size(90, 29);
            btnTatCa.Text = "Tất cả";

            dgvPhieuPhat.AllowUserToAddRows = false;
            dgvPhieuPhat.AllowUserToDeleteRows = false;
            dgvPhieuPhat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPhieuPhat.AutoGenerateColumns = false;
            dgvPhieuPhat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuPhat.BackgroundColor = SystemColors.Window;
            dgvPhieuPhat.ColumnHeadersHeight = 32;
            dgvPhieuPhat.Location = new Point(16, 311);
            dgvPhieuPhat.MultiSelect = false;
            dgvPhieuPhat.ReadOnly = true;
            dgvPhieuPhat.RowHeadersVisible = false;
            dgvPhieuPhat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuPhat.Size = new Size(1088, 430);
            dgvPhieuPhat.Columns.AddRange(new DataGridViewColumn[]
            {
                TaoCot("MaPhieuPhat", "Mã phiếu phạt", 125),
                TaoCot("MaDocGia", "Mã độc giả", 105),
                TaoCot("MaPhieuMuon", "Mã phiếu mượn", 125),
                TaoCot("MaNV", "Mã nhân viên", 110),
                TaoCot("NgayLap", "Ngày lập", 105, "d"),
                TaoCot("TrangThai", "Trạng thái", 145),
                TaoCot("TongTienPhat", "Tổng tiền phạt", 145, "N0")
            });

            Controls.Add(lblTitle);
            Controls.Add(grpPhieu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnTatCa);
            Controls.Add(dgvPhieuPhat);
            grpPhieu.ResumeLayout(false);
            grpPhieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuPhat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private static DataGridViewTextBoxColumn TaoCot(string property, string tieuDe, int width, string? format = null)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = tieuDe,
                Width = width,
                DefaultCellStyle = new DataGridViewCellStyle { Format = format ?? string.Empty }
            };
        }
    }
}
