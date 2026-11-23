#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmPhieuPhat
    {
        private TableLayoutPanel tlpRoot = null!;
        private Label lblTitle = null!;
        private GroupBox grpMaster = null!;
        private TableLayoutPanel tlpMaster = null!;
        private TableLayoutPanel tlpThongTinCoBan = null!;
        private TableLayoutPanel tlpThongTinNguoiMuon = null!;
        private TableLayoutPanel tlpNhanVienVaThaoTac = null!;
        private TableLayoutPanel tlpNhanVien = null!;
        private FlowLayoutPanel pnlThaoTacPhieu = null!;
        private TableLayoutPanel tlpTimKiem = null!;
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
        private Button btnThemPhieu = null!;
        private Button btnSuaPhieu = null!;
        private Button btnXoaPhieu = null!;
        private Button btnThanhToan = null!;
        private Button btnLamMoiPhieu = null!;
        private Button btnXuatPdf = null!;
        private Label lblTimKiem = null!;
        private TextBox txtTimKiem = null!;
        private Button btnTimKiem = null!;
        private Button btnTatCa = null!;
        private DataGridView dgvPhieuPhat = null!;
        private GroupBox grpChiTiet = null!;
        private TableLayoutPanel tlpChiTiet = null!;
        private TableLayoutPanel tlpTruongChiTiet = null!;
        private TableLayoutPanel tlpTruongChiTietTren = null!;
        private TableLayoutPanel tlpTruongChiTietDuoi = null!;
        private FlowLayoutPanel pnlThaoTacChiTiet = null!;
        private Label lblSach = null!;
        private ComboBox cboSach = null!;
        private Label lblQuyDinh = null!;
        private ComboBox cboQuyDinh = null!;
        private Label lblSoNgayTre = null!;
        private TextBox txtSoNgayTre = null!;
        private Label lblDonGia = null!;
        private TextBox txtDonGia = null!;
        private Label lblTienPhat = null!;
        private TextBox txtTienPhat = null!;
        private Label lblGhiChu = null!;
        private TextBox txtGhiChu = null!;
        private Button btnThemViPham = null!;
        private Button btnSuaViPham = null!;
        private Button btnXoaViPham = null!;
        private Button btnLamMoiChiTiet = null!;
        private DataGridView dgvChiTiet = null!;
        private Label lblTongTien = null!;

        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            lblTitle = new Label();
            grpMaster = new GroupBox();
            tlpMaster = new TableLayoutPanel();
            tlpThongTinCoBan = new TableLayoutPanel();
            tlpThongTinNguoiMuon = new TableLayoutPanel();
            tlpNhanVienVaThaoTac = new TableLayoutPanel();
            tlpNhanVien = new TableLayoutPanel();
            pnlThaoTacPhieu = new FlowLayoutPanel();
            tlpTimKiem = new TableLayoutPanel();
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
            btnThemPhieu = new Button();
            btnSuaPhieu = new Button();
            btnXoaPhieu = new Button();
            btnThanhToan = new Button();
            btnLamMoiPhieu = new Button();
            btnXuatPdf = new Button();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnTatCa = new Button();
            dgvPhieuPhat = new DataGridView();
            grpChiTiet = new GroupBox();
            tlpChiTiet = new TableLayoutPanel();
            tlpTruongChiTiet = new TableLayoutPanel();
            tlpTruongChiTietTren = new TableLayoutPanel();
            tlpTruongChiTietDuoi = new TableLayoutPanel();
            pnlThaoTacChiTiet = new FlowLayoutPanel();
            lblSach = new Label();
            cboSach = new ComboBox();
            lblQuyDinh = new Label();
            cboQuyDinh = new ComboBox();
            lblSoNgayTre = new Label();
            txtSoNgayTre = new TextBox();
            lblDonGia = new Label();
            txtDonGia = new TextBox();
            lblTienPhat = new Label();
            txtTienPhat = new TextBox();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();
            btnThemViPham = new Button();
            btnSuaViPham = new Button();
            btnXoaViPham = new Button();
            btnLamMoiChiTiet = new Button();
            dgvChiTiet = new DataGridView();
            lblTongTien = new Label();

            SuspendLayout();
            tlpRoot.SuspendLayout();
            grpMaster.SuspendLayout();
            tlpMaster.SuspendLayout();
            tlpThongTinCoBan.SuspendLayout();
            tlpThongTinNguoiMuon.SuspendLayout();
            tlpNhanVienVaThaoTac.SuspendLayout();
            tlpNhanVien.SuspendLayout();
            pnlThaoTacPhieu.SuspendLayout();
            tlpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuPhat).BeginInit();
            grpChiTiet.SuspendLayout();
            tlpChiTiet.SuspendLayout();
            tlpTruongChiTiet.SuspendLayout();
            tlpTruongChiTietTren.SuspendLayout();
            tlpTruongChiTietDuoi.SuspendLayout();
            pnlThaoTacChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();

            Text = "Quản lý phiếu phạt";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1280, 820);
            MinimumSize = new Size(1000, 680);
            Font = new Font("Segoe UI", 9F);

            tlpRoot.ColumnCount = 1;
            tlpRoot.RowCount = 4;
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Padding = new Padding(8);
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.Text = "QUẢN LÝ PHIẾU PHẠT";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            grpMaster.Dock = DockStyle.Fill;
            grpMaster.Padding = new Padding(8);
            grpMaster.Text = "1. Danh sách phiếu phạt (Master)";

            tlpMaster.ColumnCount = 1;
            tlpMaster.RowCount = 5;
            tlpMaster.Dock = DockStyle.Fill;
            tlpMaster.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMaster.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpMaster.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpMaster.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tlpMaster.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpMaster.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tlpThongTinCoBan.ColumnCount = 6;
            tlpThongTinCoBan.RowCount = 1;
            tlpThongTinCoBan.Dock = DockStyle.Fill;
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 68F));
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
            tlpThongTinCoBan.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            lblMaPhieu.Text = "Mã phiếu phạt:";
            lblMaPhieu.TextAlign = ContentAlignment.MiddleLeft;
            txtMaPhieuPhat.Dock = DockStyle.Fill;
            txtMaPhieuPhat.ReadOnly = true;
            txtMaPhieuPhat.TabStop = false;
            lblNgayLap.Text = "Ngày lập:";
            lblNgayLap.TextAlign = ContentAlignment.MiddleLeft;
            dtpNgayLap.Dock = DockStyle.Fill;
            dtpNgayLap.Format = DateTimePickerFormat.Short;
            dtpNgayLap.MaxDate = DateTime.Today;
            lblTrangThai.Text = "Trạng thái:";
            lblTrangThai.TextAlign = ContentAlignment.MiddleLeft;
            cboTrangThai.Dock = DockStyle.Fill;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[] { "Chưa thanh toán", "Đã thanh toán" });
            tlpThongTinCoBan.Controls.Add(lblMaPhieu, 0, 0);
            tlpThongTinCoBan.Controls.Add(txtMaPhieuPhat, 1, 0);
            tlpThongTinCoBan.Controls.Add(lblNgayLap, 2, 0);
            tlpThongTinCoBan.Controls.Add(dtpNgayLap, 3, 0);
            tlpThongTinCoBan.Controls.Add(lblTrangThai, 4, 0);
            tlpThongTinCoBan.Controls.Add(cboTrangThai, 5, 0);

            tlpThongTinNguoiMuon.ColumnCount = 5;
            tlpThongTinNguoiMuon.RowCount = 1;
            tlpThongTinNguoiMuon.Dock = DockStyle.Fill;
            tlpThongTinNguoiMuon.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
            tlpThongTinNguoiMuon.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tlpThongTinNguoiMuon.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
            tlpThongTinNguoiMuon.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));
            tlpThongTinNguoiMuon.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            lblDocGia.Text = "Độc giả:";
            lblDocGia.TextAlign = ContentAlignment.MiddleLeft;
            cboDocGia.Dock = DockStyle.Fill;
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            btnChonDocGia.Dock = DockStyle.Fill;
            btnChonDocGia.Margin = new Padding(2);
            btnChonDocGia.Text = "...";
            lblPhieuMuon.Text = "Phiếu mượn:";
            lblPhieuMuon.TextAlign = ContentAlignment.MiddleLeft;
            cboPhieuMuon.Dock = DockStyle.Fill;
            cboPhieuMuon.DropDownStyle = ComboBoxStyle.DropDownList;
            tlpThongTinNguoiMuon.Controls.Add(lblDocGia, 0, 0);
            tlpThongTinNguoiMuon.Controls.Add(cboDocGia, 1, 0);
            tlpThongTinNguoiMuon.Controls.Add(btnChonDocGia, 2, 0);
            tlpThongTinNguoiMuon.Controls.Add(lblPhieuMuon, 3, 0);
            tlpThongTinNguoiMuon.Controls.Add(cboPhieuMuon, 4, 0);

            tlpNhanVienVaThaoTac.ColumnCount = 2;
            tlpNhanVienVaThaoTac.RowCount = 1;
            tlpNhanVienVaThaoTac.Dock = DockStyle.Fill;
            tlpNhanVienVaThaoTac.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tlpNhanVienVaThaoTac.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            tlpNhanVien.ColumnCount = 3;
            tlpNhanVien.RowCount = 1;
            tlpNhanVien.Dock = DockStyle.Fill;
            tlpNhanVien.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
            tlpNhanVien.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpNhanVien.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
            lblNhanVien.Text = "Nhân viên:";
            lblNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            cboNhanVien.Dock = DockStyle.Fill;
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            btnChonNhanVien.Dock = DockStyle.Fill;
            btnChonNhanVien.Margin = new Padding(2);
            btnChonNhanVien.Text = "...";
            tlpNhanVien.Controls.Add(lblNhanVien, 0, 0);
            tlpNhanVien.Controls.Add(cboNhanVien, 1, 0);
            tlpNhanVien.Controls.Add(btnChonNhanVien, 2, 0);

            pnlThaoTacPhieu.Dock = DockStyle.Fill;
            pnlThaoTacPhieu.FlowDirection = FlowDirection.LeftToRight;
            pnlThaoTacPhieu.WrapContents = false;
            pnlThaoTacPhieu.AutoScroll = true;
            pnlThaoTacPhieu.Padding = new Padding(2, 1, 2, 1);
            btnThemPhieu.Size = new Size(82, 30);
            btnThemPhieu.Text = "Thêm phiếu";
            btnSuaPhieu.Size = new Size(76, 30);
            btnSuaPhieu.Text = "Sửa phiếu";
            btnXoaPhieu.Size = new Size(76, 30);
            btnXoaPhieu.Text = "Xóa phiếu";
            btnThanhToan.Size = new Size(104, 30);
            btnThanhToan.Text = "Thanh toán";
            btnLamMoiPhieu.Size = new Size(112, 30);
            btnLamMoiPhieu.Text = "Làm mới phiếu";
            btnXuatPdf.Size = new Size(96, 30);
            btnXuatPdf.Text = "Xuất PDF";
            pnlThaoTacPhieu.Controls.AddRange(new Control[]
            {
                btnThemPhieu, btnSuaPhieu, btnXoaPhieu, btnThanhToan, btnLamMoiPhieu, btnXuatPdf
            });
            tlpNhanVienVaThaoTac.Controls.Add(tlpNhanVien, 0, 0);
            tlpNhanVienVaThaoTac.Controls.Add(pnlThaoTacPhieu, 1, 0);

            tlpTimKiem.ColumnCount = 4;
            tlpTimKiem.RowCount = 1;
            tlpTimKiem.Dock = DockStyle.Fill;
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128F));
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82F));
            tlpTimKiem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));
            lblTimKiem.Text = "Tìm phiếu phạt:";
            lblTimKiem.TextAlign = ContentAlignment.MiddleLeft;
            txtTimKiem.Dock = DockStyle.Fill;
            txtTimKiem.PlaceholderText = "Mã phiếu, độc giả, phiếu mượn hoặc trạng thái";
            btnTimKiem.Dock = DockStyle.Fill;
            btnTimKiem.Margin = new Padding(3, 2, 3, 2);
            btnTimKiem.Text = "Tìm kiếm";
            btnTatCa.Dock = DockStyle.Fill;
            btnTatCa.Margin = new Padding(3, 2, 3, 2);
            btnTatCa.Text = "Tất cả";
            tlpTimKiem.Controls.Add(lblTimKiem, 0, 0);
            tlpTimKiem.Controls.Add(txtTimKiem, 1, 0);
            tlpTimKiem.Controls.Add(btnTimKiem, 2, 0);
            tlpTimKiem.Controls.Add(btnTatCa, 3, 0);

            dgvPhieuPhat.AllowUserToAddRows = false;
            dgvPhieuPhat.AllowUserToDeleteRows = false;
            dgvPhieuPhat.AutoGenerateColumns = false;
            dgvPhieuPhat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuPhat.BackgroundColor = Color.White;
            dgvPhieuPhat.ColumnHeadersHeight = 30;
            dgvPhieuPhat.Dock = DockStyle.Fill;
            dgvPhieuPhat.MultiSelect = false;
            dgvPhieuPhat.ReadOnly = true;
            dgvPhieuPhat.RowHeadersVisible = false;
            dgvPhieuPhat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuPhat.Columns.AddRange(new DataGridViewColumn[]
            {
                TaoCot("MaPhieuPhat", "Mã phiếu phạt", 105),
                TaoCot("MaDocGia", "Mã độc giả", 85),
                TaoCot("MaPhieuMuon", "Mã phiếu mượn", 105),
                TaoCot("MaNV", "Mã nhân viên", 85),
                TaoCot("NgayLap", "Ngày lập", 78, "d"),
                TaoCot("TrangThai", "Trạng thái", 130),
                TaoCot("TongTienPhat", "Tổng tiền phạt", 110, "N0", true)
            });

            tlpMaster.Controls.Add(tlpThongTinCoBan, 0, 0);
            tlpMaster.Controls.Add(tlpThongTinNguoiMuon, 0, 1);
            tlpMaster.Controls.Add(tlpNhanVienVaThaoTac, 0, 2);
            tlpMaster.Controls.Add(tlpTimKiem, 0, 3);
            tlpMaster.Controls.Add(dgvPhieuPhat, 0, 4);
            grpMaster.Controls.Add(tlpMaster);

            grpChiTiet.Dock = DockStyle.Fill;
            grpChiTiet.Padding = new Padding(8);
            grpChiTiet.Text = "2. Chi tiết vi phạm (Detail)";
            grpChiTiet.Enabled = false;

            tlpChiTiet.ColumnCount = 1;
            tlpChiTiet.RowCount = 3;
            tlpChiTiet.Dock = DockStyle.Fill;
            tlpChiTiet.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpChiTiet.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
            tlpChiTiet.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tlpChiTiet.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tlpTruongChiTietTren.ColumnCount = 4;
            tlpTruongChiTietTren.RowCount = 1;
            tlpTruongChiTietTren.Dock = DockStyle.Fill;
            tlpTruongChiTietTren.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            tlpTruongChiTietTren.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29F));
            tlpTruongChiTietTren.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tlpTruongChiTietTren.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            lblSach.Text = "Cuốn sách";
            lblQuyDinh.Text = "Quy định";
            lblSoNgayTre.Text = "Số ngày trễ";
            lblDonGia.Text = "Đơn giá";
            cboSach.Dock = DockStyle.Fill;
            cboSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyDinh.Dock = DockStyle.Fill;
            cboQuyDinh.DropDownStyle = ComboBoxStyle.DropDownList;
            txtSoNgayTre.Dock = DockStyle.Fill;
            txtSoNgayTre.ReadOnly = true;
            txtSoNgayTre.TextAlign = HorizontalAlignment.Right;
            txtDonGia.Dock = DockStyle.Fill;
            txtDonGia.ReadOnly = true;
            txtDonGia.TextAlign = HorizontalAlignment.Right;
            tlpTruongChiTietTren.Controls.Add(TaoTruong(lblSach, cboSach), 0, 0);
            tlpTruongChiTietTren.Controls.Add(TaoTruong(lblQuyDinh, cboQuyDinh), 1, 0);
            tlpTruongChiTietTren.Controls.Add(TaoTruong(lblSoNgayTre, txtSoNgayTre), 2, 0);
            tlpTruongChiTietTren.Controls.Add(TaoTruong(lblDonGia, txtDonGia), 3, 0);

            tlpTruongChiTietDuoi.ColumnCount = 2;
            tlpTruongChiTietDuoi.RowCount = 1;
            tlpTruongChiTietDuoi.Dock = DockStyle.Fill;
            tlpTruongChiTietDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            tlpTruongChiTietDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
            lblTienPhat.Text = "Tiền phạt";
            lblGhiChu.Text = "Ghi chú";
            txtTienPhat.Dock = DockStyle.Fill;
            txtTienPhat.ReadOnly = true;
            txtTienPhat.TextAlign = HorizontalAlignment.Right;
            txtGhiChu.Dock = DockStyle.Fill;
            txtGhiChu.MaxLength = 200;
            tlpTruongChiTietDuoi.Controls.Add(TaoTruong(lblTienPhat, txtTienPhat), 0, 0);
            tlpTruongChiTietDuoi.Controls.Add(TaoTruong(lblGhiChu, txtGhiChu), 1, 0);

            tlpTruongChiTiet.ColumnCount = 1;
            tlpTruongChiTiet.RowCount = 2;
            tlpTruongChiTiet.Dock = DockStyle.Fill;
            tlpTruongChiTiet.RowStyles.Add(new RowStyle(SizeType.Percent, 54F));
            tlpTruongChiTiet.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            tlpTruongChiTiet.Controls.Add(tlpTruongChiTietTren, 0, 0);
            tlpTruongChiTiet.Controls.Add(tlpTruongChiTietDuoi, 0, 1);

            pnlThaoTacChiTiet.Dock = DockStyle.Fill;
            pnlThaoTacChiTiet.FlowDirection = FlowDirection.LeftToRight;
            pnlThaoTacChiTiet.WrapContents = false;
            pnlThaoTacChiTiet.Padding = new Padding(2, 1, 2, 1);
            btnThemViPham.Size = new Size(124, 30);
            btnThemViPham.Text = "Thêm vi phạm";
            btnSuaViPham.Size = new Size(124, 30);
            btnSuaViPham.Text = "Sửa vi phạm";
            btnSuaViPham.Enabled = false;
            btnXoaViPham.Size = new Size(124, 30);
            btnXoaViPham.Text = "Xóa vi phạm";
            btnXoaViPham.Enabled = false;
            btnLamMoiChiTiet.Size = new Size(100, 30);
            btnLamMoiChiTiet.Text = "Làm mới";
            pnlThaoTacChiTiet.Controls.AddRange(new Control[]
            {
                btnThemViPham, btnSuaViPham, btnXoaViPham, btnLamMoiChiTiet
            });

            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.ColumnHeadersHeight = 30;
            dgvChiTiet.Dock = DockStyle.Fill;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Columns.AddRange(new DataGridViewColumn[]
            {
                TaoCotChiTiet("MaCTPP", "MãCTPP", 1, visible: false),
                TaoCotChiTiet("MaPhieuPhat", "MaPhieuPhat", 1, visible: false),
                TaoCotChiTiet("MaQD", "MaQD", 1, visible: false),
                TaoCotChiTiet("MaSach", "Mã cuốn", 90),
                TaoCotChiTiet("TenSach", "Tên sách", 160),
                TaoCotChiTiet("QuyDinh", "Quy định", 150),
                TaoCotChiTiet("SoNgayTre", "Số ngày trễ", 78),
                TaoCotChiTiet("DonGia", "Đơn giá", 95, "N0", rightAligned: true),
                TaoCotChiTiet("TienPhat", "Tiền phạt", 100, "N0", rightAligned: true),
                TaoCotChiTiet("GhiChu", "Ghi chú", 145)
            });

            tlpChiTiet.Controls.Add(tlpTruongChiTiet, 0, 0);
            tlpChiTiet.Controls.Add(pnlThaoTacChiTiet, 0, 1);
            tlpChiTiet.Controls.Add(dgvChiTiet, 0, 2);
            grpChiTiet.Controls.Add(tlpChiTiet);

            lblTongTien.Dock = DockStyle.Fill;
            lblTongTien.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTongTien.Text = "Tổng tiền phạt: 0 VNĐ";
            lblTongTien.TextAlign = ContentAlignment.MiddleLeft;

            tlpRoot.Controls.Add(lblTitle, 0, 0);
            tlpRoot.Controls.Add(grpMaster, 0, 1);
            tlpRoot.Controls.Add(grpChiTiet, 0, 2);
            tlpRoot.Controls.Add(lblTongTien, 0, 3);
            Controls.Add(tlpRoot);

            tlpTimKiem.ResumeLayout(false);
            tlpNhanVien.ResumeLayout(false);
            pnlThaoTacPhieu.ResumeLayout(false);
            tlpNhanVienVaThaoTac.ResumeLayout(false);
            tlpThongTinNguoiMuon.ResumeLayout(false);
            tlpThongTinCoBan.ResumeLayout(false);
            tlpMaster.ResumeLayout(false);
            grpMaster.ResumeLayout(false);
            pnlThaoTacChiTiet.ResumeLayout(false);
            tlpTruongChiTietDuoi.ResumeLayout(false);
            tlpTruongChiTietTren.ResumeLayout(false);
            tlpTruongChiTiet.ResumeLayout(false);
            tlpChiTiet.ResumeLayout(false);
            grpChiTiet.ResumeLayout(false);
            tlpRoot.ResumeLayout(false);
            ResumeLayout(false);
        }

        private static TableLayoutPanel TaoTruong(Label nhan, Control dieuKhien)
        {
            TableLayoutPanel truong = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 4, 0)
            };
            truong.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            truong.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            truong.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            nhan.Dock = DockStyle.Fill;
            nhan.TextAlign = ContentAlignment.MiddleLeft;
            nhan.Margin = new Padding(2, 0, 2, 0);
            dieuKhien.Dock = DockStyle.Fill;
            dieuKhien.Margin = new Padding(2, 0, 2, 0);
            truong.Controls.Add(nhan, 0, 0);
            truong.Controls.Add(dieuKhien, 0, 1);
            return truong;
        }

        private static DataGridViewTextBoxColumn TaoCot(string property, string tieuDe, int fillWeight, string? format = null, bool rightAligned = false)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = tieuDe,
                FillWeight = fillWeight,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = format ?? string.Empty,
                    Alignment = rightAligned ? DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft
                }
            };
        }

        private static DataGridViewTextBoxColumn TaoCotChiTiet(
            string property,
            string tieuDe,
            int fillWeight,
            string? format = null,
            bool visible = true,
            bool rightAligned = false)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = tieuDe,
                FillWeight = fillWeight,
                Visible = visible,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = format ?? string.Empty,
                    Alignment = rightAligned ? DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft
                }
            };
        }
    }
}
