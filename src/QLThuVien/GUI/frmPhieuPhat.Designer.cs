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
        private Button btnChonDocGia = null!;
        private Label lblPhieuMuon = null!;
        private ComboBox cboPhieuMuon = null!;
        private Label lblNhanVien = null!;
        private ComboBox cboNhanVien = null!;
        private Button btnChonNhanVien = null!;
        private Label lblTongTienPhat = null!;
        private TextBox txtTongTienPhat = null!;
        private Button btnThemPhieu = null!;
        private Button btnSuaPhieu = null!;
        private Button btnXoaPhieu = null!;
        private Button btnChiTiet = null!;
        private Button btnXuatPdf = null!;
        private Button btnLamMoi = null!;
        private TextBox txtTimKiem = null!;
        private Button btnTimKiem = null!;
        private Button btnTatCa = null!;
        private DataGridView dgvPhieuPhat = null!;

        // Vùng Detail (Main - Sub trên cùng 1 Form)
        private GroupBox grpChiTiet = null!;
        private Label lblSachDetail = null!;
        private ComboBox cboSachDetail = null!;
        private Label lblQuyDinhDetail = null!;
        private ComboBox cboQuyDinhDetail = null!;
        private Label lblSoNgayTreDetail = null!;
        private TextBox txtSoNgayTreDetail = null!;
        private Label lblDonGiaDetail = null!;
        private TextBox txtDonGiaDetail = null!;
        private Label lblTienPhatDetail = null!;
        private TextBox txtTienPhatDetail = null!;
        private Label lblGhiChuDetail = null!;
        private TextBox txtGhiChuDetail = null!;
        private Button btnThemViPham = null!;
        private Button btnSuaViPham = null!;
        private Button btnXoaViPham = null!;
        private Button btnLamMoiViPham = null!;
        private DataGridView dgvChiTiet = null!;

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
            btnChonDocGia = new Button();
            lblPhieuMuon = new Label();
            cboPhieuMuon = new ComboBox();
            lblNhanVien = new Label();
            cboNhanVien = new ComboBox();
            btnChonNhanVien = new Button();
            lblTongTienPhat = new Label();
            txtTongTienPhat = new TextBox();
            btnThemPhieu = new Button();
            btnSuaPhieu = new Button();
            btnXoaPhieu = new Button();
            btnChiTiet = new Button();
            btnXuatPdf = new Button();
            btnLamMoi = new Button();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnTatCa = new Button();
            dgvPhieuPhat = new DataGridView();

            grpChiTiet = new GroupBox();
            lblSachDetail = new Label();
            cboSachDetail = new ComboBox();
            lblQuyDinhDetail = new Label();
            cboQuyDinhDetail = new ComboBox();
            lblSoNgayTreDetail = new Label();
            txtSoNgayTreDetail = new TextBox();
            lblDonGiaDetail = new Label();
            txtDonGiaDetail = new TextBox();
            lblTienPhatDetail = new Label();
            txtTienPhatDetail = new TextBox();
            lblGhiChuDetail = new Label();
            txtGhiChuDetail = new TextBox();
            btnThemViPham = new Button();
            btnSuaViPham = new Button();
            btnXoaViPham = new Button();
            btnLamMoiViPham = new Button();
            dgvChiTiet = new DataGridView();

            grpPhieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuPhat).BeginInit();
            grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();

            Text = "Quản lý phiếu phạt (Main - Sub)";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 760);
            MinimumSize = new Size(1000, 680);
            Font = new Font("Segoe UI", 9F);

            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(16, 8);
            lblTitle.Size = new Size(1088, 30);
            lblTitle.Text = "QUẢN LÝ PHIẾU PHẠT (MAIN - SUB 1-N)";

            // ---- MASTER: THÔNG TIN PHIẾU PHẠT ----
            grpPhieu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpPhieu.Location = new Point(16, 42);
            grpPhieu.Size = new Size(1088, 178);
            grpPhieu.Text = "1. Thông tin Phiếu phạt (Master)";

            lblMaPhieu.AutoSize = true;
            lblMaPhieu.Location = new Point(16, 26);
            lblMaPhieu.Text = "Mã phiếu:";
            txtMaPhieuPhat.Location = new Point(105, 23);
            txtMaPhieuPhat.Size = new Size(165, 23);
            txtMaPhieuPhat.ReadOnly = true;
            txtMaPhieuPhat.TabStop = false;

            lblNgayLap.AutoSize = true;
            lblNgayLap.Location = new Point(295, 26);
            lblNgayLap.Text = "Ngày lập:";
            dtpNgayLap.Format = DateTimePickerFormat.Short;
            dtpNgayLap.Location = new Point(365, 23);
            dtpNgayLap.Size = new Size(155, 23);

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(545, 26);
            lblTrangThai.Text = "Trạng thái:";
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[] { "Chưa thanh toán", "Đã thanh toán" });
            cboTrangThai.Location = new Point(620, 23);
            cboTrangThai.Size = new Size(190, 23);

            lblTongTienPhat.AutoSize = true;
            lblTongTienPhat.Location = new Point(825, 26);
            lblTongTienPhat.Text = "Tổng phạt:";
            txtTongTienPhat.Location = new Point(900, 23);
            txtTongTienPhat.Size = new Size(170, 23);
            txtTongTienPhat.ReadOnly = true;
            txtTongTienPhat.TextAlign = HorizontalAlignment.Right;

            lblDocGia.AutoSize = true;
            lblDocGia.Location = new Point(16, 60);
            lblDocGia.Text = "Độc giả:";
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(105, 57);
            cboDocGia.Size = new Size(300, 23);

            btnChonDocGia.Location = new Point(410, 56);
            btnChonDocGia.Size = new Size(34, 25);
            btnChonDocGia.Text = "...";
            btnChonDocGia.UseVisualStyleBackColor = true;

            lblPhieuMuon.AutoSize = true;
            lblPhieuMuon.Location = new Point(465, 60);
            lblPhieuMuon.Text = "Phiếu mượn:";
            cboPhieuMuon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhieuMuon.Location = new Point(550, 57);
            cboPhieuMuon.Size = new Size(520, 23);

            lblNhanVien.AutoSize = true;
            lblNhanVien.Location = new Point(16, 94);
            lblNhanVien.Text = "Nhân viên:";
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Location = new Point(105, 91);
            cboNhanVien.Size = new Size(300, 23);

            btnChonNhanVien.Location = new Point(410, 90);
            btnChonNhanVien.Size = new Size(34, 25);
            btnChonNhanVien.Text = "...";
            btnChonNhanVien.UseVisualStyleBackColor = true;

            btnThemPhieu.Location = new Point(16, 130);
            btnThemPhieu.Size = new Size(95, 32);
            btnThemPhieu.Text = "Thêm phiếu";
            btnSuaPhieu.Location = new Point(118, 130);
            btnSuaPhieu.Size = new Size(95, 32);
            btnSuaPhieu.Text = "Sửa phiếu";
            btnXoaPhieu.Location = new Point(220, 130);
            btnXoaPhieu.Size = new Size(95, 32);
            btnXoaPhieu.Text = "Xóa phiếu";
            btnLamMoi.Location = new Point(322, 130);
            btnLamMoi.Size = new Size(100, 32);
            btnLamMoi.Text = "Làm mới";
            btnXuatPdf.Location = new Point(430, 130);
            btnXuatPdf.Size = new Size(110, 32);
            btnXuatPdf.Text = "Xuất PDF";
            btnChiTiet.Location = new Point(548, 130);
            btnChiTiet.Size = new Size(145, 32);
            btnChiTiet.Text = "Mở popup chi tiết";
            btnChiTiet.Enabled = false;

            grpPhieu.Controls.AddRange(new Control[]
            {
                lblMaPhieu, txtMaPhieuPhat, lblNgayLap, dtpNgayLap, lblTrangThai, cboTrangThai,
                lblDocGia, cboDocGia, btnChonDocGia, lblPhieuMuon, cboPhieuMuon, lblNhanVien, cboNhanVien, btnChonNhanVien,
                lblTongTienPhat, txtTongTienPhat, btnThemPhieu, btnSuaPhieu, btnXoaPhieu,
                btnChiTiet, btnLamMoi, btnXuatPdf
            });

            // ---- TÌM KIẾM MASTER ----
            txtTimKiem.Location = new Point(16, 228);
            txtTimKiem.Size = new Size(380, 23);
            txtTimKiem.PlaceholderText = "Tìm phiếu, độc giả, phiếu mượn...";
            btnTimKiem.Location = new Point(404, 226);
            btnTimKiem.Size = new Size(80, 27);
            btnTimKiem.Text = "Tìm";
            btnTatCa.Location = new Point(490, 226);
            btnTatCa.Size = new Size(90, 27);
            btnTatCa.Text = "Tất cả";

            // ---- BẢNG MASTER (dgvPhieuPhat) ----
            dgvPhieuPhat.AllowUserToAddRows = false;
            dgvPhieuPhat.AllowUserToDeleteRows = false;
            dgvPhieuPhat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPhieuPhat.AutoGenerateColumns = false;
            dgvPhieuPhat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuPhat.BackgroundColor = SystemColors.Window;
            dgvPhieuPhat.ColumnHeadersHeight = 30;
            dgvPhieuPhat.Location = new Point(16, 258);
            dgvPhieuPhat.MultiSelect = false;
            dgvPhieuPhat.ReadOnly = true;
            dgvPhieuPhat.RowHeadersVisible = false;
            dgvPhieuPhat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuPhat.Size = new Size(1088, 170);
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

            // ---- DETAIL: CHI TIẾT VI PHẠM (MAIN - SUB) ----
            grpChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpChiTiet.Location = new Point(16, 434);
            grpChiTiet.Size = new Size(1088, 314);
            grpChiTiet.Text = "2. Chi tiết vi phạm của Phiếu phạt đang chọn (Detail)";

            lblSachDetail.AutoSize = true;
            lblSachDetail.Location = new Point(14, 26);
            lblSachDetail.Text = "Cuốn sách:";
            cboSachDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSachDetail.Location = new Point(92, 23);
            cboSachDetail.Size = new Size(340, 23);

            lblQuyDinhDetail.AutoSize = true;
            lblQuyDinhDetail.Location = new Point(450, 26);
            lblQuyDinhDetail.Text = "Quy định:";
            cboQuyDinhDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyDinhDetail.Location = new Point(520, 23);
            cboQuyDinhDetail.Size = new Size(320, 23);

            lblSoNgayTreDetail.AutoSize = true;
            lblSoNgayTreDetail.Location = new Point(855, 26);
            lblSoNgayTreDetail.Text = "Trễ (ngày):";
            txtSoNgayTreDetail.Location = new Point(930, 23);
            txtSoNgayTreDetail.Size = new Size(140, 23);
            txtSoNgayTreDetail.ReadOnly = true;

            lblDonGiaDetail.AutoSize = true;
            lblDonGiaDetail.Location = new Point(14, 58);
            lblDonGiaDetail.Text = "Đơn giá:";
            txtDonGiaDetail.Location = new Point(92, 55);
            txtDonGiaDetail.Size = new Size(140, 23);
            txtDonGiaDetail.ReadOnly = true;
            txtDonGiaDetail.TextAlign = HorizontalAlignment.Right;

            lblTienPhatDetail.AutoSize = true;
            lblTienPhatDetail.Location = new Point(250, 58);
            lblTienPhatDetail.Text = "Tiền phạt:";
            txtTienPhatDetail.Location = new Point(320, 55);
            txtTienPhatDetail.Size = new Size(140, 23);
            txtTienPhatDetail.ReadOnly = true;
            txtTienPhatDetail.TextAlign = HorizontalAlignment.Right;

            lblGhiChuDetail.AutoSize = true;
            lblGhiChuDetail.Location = new Point(480, 58);
            lblGhiChuDetail.Text = "Ghi chú:";
            txtGhiChuDetail.Location = new Point(540, 55);
            txtGhiChuDetail.Size = new Size(530, 23);

            btnThemViPham.Location = new Point(14, 88);
            btnThemViPham.Size = new Size(115, 28);
            btnThemViPham.Text = "+ Thêm vi phạm";
            btnSuaViPham.Location = new Point(136, 88);
            btnSuaViPham.Size = new Size(110, 28);
            btnSuaViPham.Text = "Sửa vi phạm";
            btnXoaViPham.Location = new Point(253, 88);
            btnXoaViPham.Size = new Size(110, 28);
            btnXoaViPham.Text = "Xóa vi phạm";
            btnLamMoiViPham.Location = new Point(370, 88);
            btnLamMoiViPham.Size = new Size(100, 28);
            btnLamMoiViPham.Text = "Làm mới";

            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = SystemColors.Window;
            dgvChiTiet.ColumnHeadersHeight = 28;
            dgvChiTiet.Location = new Point(14, 124);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(1058, 178);
            dgvChiTiet.Columns.AddRange(new DataGridViewColumn[]
            {
                TaoCot("MaCTPP", "Mã VP", 70),
                TaoCot("MaSach", "Mã cuốn", 100),
                TaoCot("MaQD", "Mã QĐ", 80),
                TaoCot("DonGia", "Đơn giá", 110, "N0"),
                TaoCot("SoNgayTre", "Ngày trễ", 80),
                TaoCot("GhiChu", "Ghi chú", 220),
                TaoCot("TienPhat", "Tiền phạt", 120, "N0")
            });

            grpChiTiet.Controls.AddRange(new Control[]
            {
                lblSachDetail, cboSachDetail, lblQuyDinhDetail, cboQuyDinhDetail,
                lblSoNgayTreDetail, txtSoNgayTreDetail, lblDonGiaDetail, txtDonGiaDetail,
                lblTienPhatDetail, txtTienPhatDetail, lblGhiChuDetail, txtGhiChuDetail,
                btnThemViPham, btnSuaViPham, btnXoaViPham, btnLamMoiViPham, dgvChiTiet
            });

            Controls.Add(lblTitle);
            Controls.Add(grpPhieu);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnTatCa);
            Controls.Add(dgvPhieuPhat);
            Controls.Add(grpChiTiet);
            grpPhieu.ResumeLayout(false);
            grpPhieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuPhat).EndInit();
            grpChiTiet.ResumeLayout(false);
            grpChiTiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
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
