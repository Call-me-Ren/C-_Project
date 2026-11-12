namespace QLThuVien.GUI
{
    partial class frmMuonTraSach
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitle = new Label();
            grpThongTinPM = new GroupBox();
            dtpHanTra = new DateTimePicker();
            lblHanTra = new Label();
            dtpNgayMuon = new DateTimePicker();
            lblNgayMuon = new Label();
            cboNhanVien = new ComboBox();
            lblNhanVien = new Label();
            cboDocGia = new ComboBox();
            lblDocGia = new Label();
            txtMaPM = new TextBox();
            lblMaPM = new Label();
            grpChucNangPM = new GroupBox();
            btnLamMoiPM = new Button();
            btnXoaPM = new Button();
            btnThemPM = new Button();
            btnXuatPdf = new Button();
            grpTimKiem = new GroupBox();
            btnTatCa = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            dgvPhieuMuon = new DataGridView();
            colMaPM = new DataGridViewTextBoxColumn();
            colTenDocGia = new DataGridViewTextBoxColumn();
            colTenNV = new DataGridViewTextBoxColumn();
            colNgayMuon = new DataGridViewTextBoxColumn();
            colHanTra = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            grpChiTiet = new GroupBox();
            btnTraSach = new Button();
            btnThemSach = new Button();
            txtTinhTrang = new TextBox();
            lblTinhTrang = new Label();
            cboSach = new ComboBox();
            lblSach = new Label();
            lblTieuDeChiTiet = new Label();
            dgvChiTietMuon = new DataGridView();
            colMaSach = new DataGridViewTextBoxColumn();
            colTenSach = new DataGridViewTextBoxColumn();
            colTinhTrang = new DataGridViewTextBoxColumn();
            colNgayThucTra = new DataGridViewTextBoxColumn();
            colTrangThaiTra = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            grpThongTinPM.SuspendLayout();
            grpChucNangPM.SuspendLayout();
            grpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuMuon).BeginInit();
            grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietMuon).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1100, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1100, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ MƯỢN - TRẢ SÁCH (MAIN - SUB)";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpThongTinPM
            // 
            grpThongTinPM.Controls.Add(dtpHanTra);
            grpThongTinPM.Controls.Add(lblHanTra);
            grpThongTinPM.Controls.Add(dtpNgayMuon);
            grpThongTinPM.Controls.Add(lblNgayMuon);
            grpThongTinPM.Controls.Add(cboNhanVien);
            grpThongTinPM.Controls.Add(lblNhanVien);
            grpThongTinPM.Controls.Add(cboDocGia);
            grpThongTinPM.Controls.Add(lblDocGia);
            grpThongTinPM.Controls.Add(txtMaPM);
            grpThongTinPM.Controls.Add(lblMaPM);
            grpThongTinPM.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTinPM.Location = new Point(12, 56);
            grpThongTinPM.Name = "grpThongTinPM";
            grpThongTinPM.Size = new Size(680, 105);
            grpThongTinPM.TabIndex = 1;
            grpThongTinPM.TabStop = false;
            grpThongTinPM.Text = "Thông tin phiếu mượn (Bảng Master)";
            // 
            // dtpHanTra
            // 
            dtpHanTra.CustomFormat = "dd/MM/yyyy";
            dtpHanTra.Font = new Font("Segoe UI", 9.5F);
            dtpHanTra.Format = DateTimePickerFormat.Custom;
            dtpHanTra.Location = new Point(530, 65);
            dtpHanTra.Name = "dtpHanTra";
            dtpHanTra.Size = new Size(135, 25);
            dtpHanTra.TabIndex = 9;
            // 
            // lblHanTra
            // 
            lblHanTra.AutoSize = true;
            lblHanTra.Font = new Font("Segoe UI", 9.5F);
            lblHanTra.Location = new Point(465, 68);
            lblHanTra.Name = "lblHanTra";
            lblHanTra.Size = new Size(54, 17);
            lblHanTra.TabIndex = 8;
            lblHanTra.Text = "Hạn trả:";
            // 
            // dtpNgayMuon
            // 
            dtpNgayMuon.CustomFormat = "dd/MM/yyyy";
            dtpNgayMuon.Font = new Font("Segoe UI", 9.5F);
            dtpNgayMuon.Format = DateTimePickerFormat.Custom;
            dtpNgayMuon.Location = new Point(530, 26);
            dtpNgayMuon.Name = "dtpNgayMuon";
            dtpNgayMuon.Size = new Size(135, 25);
            dtpNgayMuon.TabIndex = 7;
            dtpNgayMuon.ValueChanged += dtpNgayMuon_ValueChanged;
            // 
            // lblNgayMuon
            // 
            lblNgayMuon.AutoSize = true;
            lblNgayMuon.Font = new Font("Segoe UI", 9.5F);
            lblNgayMuon.Location = new Point(448, 30);
            lblNgayMuon.Name = "lblNgayMuon";
            lblNgayMuon.Size = new Size(80, 17);
            lblNgayMuon.TabIndex = 6;
            lblNgayMuon.Text = "Ngày mượn:";
            // 
            // cboNhanVien
            // 
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Font = new Font("Segoe UI", 9.5F);
            cboNhanVien.FormattingEnabled = true;
            cboNhanVien.Location = new Point(275, 65);
            cboNhanVien.Name = "cboNhanVien";
            cboNhanVien.Size = new Size(160, 25);
            cboNhanVien.TabIndex = 5;
            // 
            // lblNhanVien
            // 
            lblNhanVien.AutoSize = true;
            lblNhanVien.Font = new Font("Segoe UI", 9.5F);
            lblNhanVien.Location = new Point(205, 68);
            lblNhanVien.Name = "lblNhanVien";
            lblNhanVien.Size = new Size(69, 17);
            lblNhanVien.TabIndex = 4;
            lblNhanVien.Text = "Nhân viên:";
            // 
            // cboDocGia
            // 
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Font = new Font("Segoe UI", 9.5F);
            cboDocGia.FormattingEnabled = true;
            cboDocGia.Location = new Point(275, 26);
            cboDocGia.Name = "cboDocGia";
            cboDocGia.Size = new Size(160, 25);
            cboDocGia.TabIndex = 3;
            // 
            // lblDocGia
            // 
            lblDocGia.AutoSize = true;
            lblDocGia.Font = new Font("Segoe UI", 9.5F);
            lblDocGia.Location = new Point(205, 30);
            lblDocGia.Name = "lblDocGia";
            lblDocGia.Size = new Size(56, 17);
            lblDocGia.TabIndex = 2;
            lblDocGia.Text = "Độc giả:";
            // 
            // txtMaPM
            // 
            txtMaPM.Font = new Font("Segoe UI", 9.5F);
            txtMaPM.Location = new Point(78, 26);
            txtMaPM.Name = "txtMaPM";
            txtMaPM.Size = new Size(115, 25);
            txtMaPM.TabIndex = 1;
            // 
            // lblMaPM
            // 
            lblMaPM.AutoSize = true;
            lblMaPM.Font = new Font("Segoe UI", 9.5F);
            lblMaPM.Location = new Point(15, 30);
            lblMaPM.Name = "lblMaPM";
            lblMaPM.Size = new Size(51, 17);
            lblMaPM.TabIndex = 0;
            lblMaPM.Text = "Mã PM:";
            // 
            // grpChucNangPM
            // 
            grpChucNangPM.Controls.Add(btnXuatPdf);
            grpChucNangPM.Controls.Add(btnLamMoiPM);
            grpChucNangPM.Controls.Add(btnXoaPM);
            grpChucNangPM.Controls.Add(btnThemPM);
            grpChucNangPM.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChucNangPM.Location = new Point(695, 56);
            grpChucNangPM.Name = "grpChucNangPM";
            grpChucNangPM.Size = new Size(393, 105);
            grpChucNangPM.TabIndex = 2;
            grpChucNangPM.TabStop = false;
            grpChucNangPM.Text = "Thao tác Phiếu mượn";
            // 
            // btnXuatPdf
            // 
            btnXuatPdf.BackColor = Color.FromArgb(231, 76, 60);
            btnXuatPdf.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXuatPdf.ForeColor = Color.White;
            btnXuatPdf.Location = new Point(290, 35);
            btnXuatPdf.Name = "btnXuatPdf";
            btnXuatPdf.Size = new Size(95, 38);
            btnXuatPdf.TabIndex = 3;
            btnXuatPdf.Text = "📄 Xuất PDF";
            btnXuatPdf.UseVisualStyleBackColor = false;
            btnXuatPdf.Click += btnXuatPdf_Click;
            // 
            // btnLamMoiPM
            // 
            btnLamMoiPM.Font = new Font("Segoe UI", 9.5F);
            btnLamMoiPM.Location = new Point(200, 35);
            btnLamMoiPM.Name = "btnLamMoiPM";
            btnLamMoiPM.Size = new Size(85, 38);
            btnLamMoiPM.TabIndex = 2;
            btnLamMoiPM.Text = "Làm mới";
            btnLamMoiPM.UseVisualStyleBackColor = true;
            btnLamMoiPM.Click += btnLamMoiPM_Click;
            // 
            // btnXoaPM
            // 
            btnXoaPM.Font = new Font("Segoe UI", 9.5F);
            btnXoaPM.Location = new Point(110, 35);
            btnXoaPM.Name = "btnXoaPM";
            btnXoaPM.Size = new Size(85, 38);
            btnXoaPM.TabIndex = 1;
            btnXoaPM.Text = "Xóa phiếu";
            btnXoaPM.UseVisualStyleBackColor = true;
            btnXoaPM.Click += btnXoaPM_Click;
            // 
            // btnThemPM
            // 
            btnThemPM.Font = new Font("Segoe UI", 9.5F);
            btnThemPM.Location = new Point(10, 35);
            btnThemPM.Name = "btnThemPM";
            btnThemPM.Size = new Size(95, 38);
            btnThemPM.TabIndex = 0;
            btnThemPM.Text = "+ Tạo phiếu";
            btnThemPM.UseVisualStyleBackColor = true;
            btnThemPM.Click += btnThemPM_Click;
            // 
            // grpTimKiem
            // 
            grpTimKiem.Controls.Add(btnTatCa);
            grpTimKiem.Controls.Add(btnTimKiem);
            grpTimKiem.Controls.Add(txtTimKiem);
            grpTimKiem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpTimKiem.Location = new Point(12, 167);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(1076, 55);
            grpTimKiem.TabIndex = 3;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm phiếu mượn (theo mã PM, tên độc giả, tên nhân viên)";
            // 
            // btnTatCa
            // 
            btnTatCa.Font = new Font("Segoe UI", 9.5F);
            btnTatCa.Location = new Point(945, 18);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(115, 28);
            btnTatCa.TabIndex = 2;
            btnTatCa.Text = "Hiện tất cả";
            btnTatCa.UseVisualStyleBackColor = true;
            btnTatCa.Click += btnTatCa_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Segoe UI", 9.5F);
            btnTimKiem.Location = new Point(820, 18);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(115, 28);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 9.5F);
            txtTimKiem.Location = new Point(15, 20);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã phiếu mượn, tên độc giả hoặc nhân viên để tìm...";
            txtTimKiem.Size = new Size(790, 24);
            txtTimKiem.TabIndex = 0;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // dgvPhieuMuon
            // 
            dgvPhieuMuon.AllowUserToAddRows = false;
            dgvPhieuMuon.AllowUserToDeleteRows = false;
            dgvPhieuMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuMuon.BackgroundColor = Color.White;
            dgvPhieuMuon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhieuMuon.Columns.AddRange(new DataGridViewColumn[] { colMaPM, colTenDocGia, colTenNV, colNgayMuon, colHanTra, colTrangThai });
            dgvPhieuMuon.Location = new Point(12, 228);
            dgvPhieuMuon.MultiSelect = false;
            dgvPhieuMuon.Name = "dgvPhieuMuon";
            dgvPhieuMuon.ReadOnly = true;
            dgvPhieuMuon.RowHeadersVisible = false;
            dgvPhieuMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuMuon.Size = new Size(1076, 170);
            dgvPhieuMuon.TabIndex = 4;
            dgvPhieuMuon.CellClick += dgvPhieuMuon_CellClick;
            // 
            // colMaPM
            // 
            colMaPM.DataPropertyName = "MaPM";
            colMaPM.FillWeight = 80F;
            colMaPM.HeaderText = "Mã PM";
            colMaPM.Name = "colMaPM";
            colMaPM.ReadOnly = true;
            // 
            // colTenDocGia
            // 
            colTenDocGia.DataPropertyName = "TenDocGia";
            colTenDocGia.FillWeight = 130F;
            colTenDocGia.HeaderText = "Độc giả";
            colTenDocGia.Name = "colTenDocGia";
            colTenDocGia.ReadOnly = true;
            // 
            // colTenNV
            // 
            colTenNV.DataPropertyName = "TenNV";
            colTenNV.FillWeight = 110F;
            colTenNV.HeaderText = "Nhân viên lập";
            colTenNV.Name = "colTenNV";
            colTenNV.ReadOnly = true;
            // 
            // colNgayMuon
            // 
            colNgayMuon.DataPropertyName = "NgayMuon";
            colNgayMuon.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" };
            colNgayMuon.FillWeight = 90F;
            colNgayMuon.HeaderText = "Ngày mượn";
            colNgayMuon.Name = "colNgayMuon";
            colNgayMuon.ReadOnly = true;
            // 
            // colHanTra
            // 
            colHanTra.DataPropertyName = "HanTra";
            colHanTra.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" };
            colHanTra.FillWeight = 90F;
            colHanTra.HeaderText = "Hạn trả";
            colHanTra.Name = "colHanTra";
            colHanTra.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.DataPropertyName = "TrangThai";
            colTrangThai.FillWeight = 100F;
            colTrangThai.HeaderText = "Trạng thái mượn";
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // grpChiTiet
            // 
            grpChiTiet.Controls.Add(btnTraSach);
            grpChiTiet.Controls.Add(btnThemSach);
            grpChiTiet.Controls.Add(txtTinhTrang);
            grpChiTiet.Controls.Add(lblTinhTrang);
            grpChiTiet.Controls.Add(cboSach);
            grpChiTiet.Controls.Add(lblSach);
            grpChiTiet.Controls.Add(lblTieuDeChiTiet);
            grpChiTiet.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChiTiet.Location = new Point(12, 404);
            grpChiTiet.Name = "grpChiTiet";
            grpChiTiet.Size = new Size(1076, 95);
            grpChiTiet.TabIndex = 5;
            grpChiTiet.TabStop = false;
            grpChiTiet.Text = "Thao tác mượn / trả cuốn sách (Bảng Detail)";
            // 
            // btnTraSach
            // 
            btnTraSach.BackColor = Color.FromArgb(46, 204, 113);
            btnTraSach.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnTraSach.ForeColor = Color.White;
            btnTraSach.Location = new Point(925, 48);
            btnTraSach.Name = "btnTraSach";
            btnTraSach.Size = new Size(135, 34);
            btnTraSach.TabIndex = 6;
            btnTraSach.Text = "✓ TRẢ SÁCH";
            btnTraSach.UseVisualStyleBackColor = false;
            btnTraSach.Click += btnTraSach_Click;
            // 
            // btnThemSach
            // 
            btnThemSach.BackColor = Color.FromArgb(52, 152, 219);
            btnThemSach.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThemSach.ForeColor = Color.White;
            btnThemSach.Location = new Point(775, 48);
            btnThemSach.Name = "btnThemSach";
            btnThemSach.Size = new Size(140, 34);
            btnThemSach.TabIndex = 5;
            btnThemSach.Text = "+ Thêm mượn sách";
            btnThemSach.UseVisualStyleBackColor = false;
            btnThemSach.Click += btnThemSach_Click;
            // 
            // txtTinhTrang
            // 
            txtTinhTrang.Font = new Font("Segoe UI", 9.5F);
            txtTinhTrang.Location = new Point(480, 53);
            txtTinhTrang.Name = "txtTinhTrang";
            txtTinhTrang.PlaceholderText = "Tình trạng sách lúc mượn (VD: Tốt, Mới 100%)...";
            txtTinhTrang.Size = new Size(275, 24);
            txtTinhTrang.TabIndex = 4;
            txtTinhTrang.Text = "Tốt";
            // 
            // lblTinhTrang
            // 
            lblTinhTrang.AutoSize = true;
            lblTinhTrang.Font = new Font("Segoe UI", 9.5F);
            lblTinhTrang.Location = new Point(405, 56);
            lblTinhTrang.Name = "lblTinhTrang";
            lblTinhTrang.Size = new Size(71, 17);
            lblTinhTrang.TabIndex = 3;
            lblTinhTrang.Text = "Tình trạng:";
            // 
            // cboSach
            // 
            cboSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSach.Font = new Font("Segoe UI", 9.5F);
            cboSach.FormattingEnabled = true;
            cboSach.Location = new Point(95, 53);
            cboSach.Name = "cboSach";
            cboSach.Size = new Size(295, 25);
            cboSach.TabIndex = 2;
            // 
            // lblSach
            // 
            lblSach.AutoSize = true;
            lblSach.Font = new Font("Segoe UI", 9.5F);
            lblSach.Location = new Point(15, 56);
            lblSach.Name = "lblSach";
            lblSach.Size = new Size(73, 17);
            lblSach.TabIndex = 1;
            lblSach.Text = "Chọn sách:";
            // 
            // lblTieuDeChiTiet
            // 
            lblTieuDeChiTiet.AutoSize = true;
            lblTieuDeChiTiet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTieuDeChiTiet.ForeColor = Color.FromArgb(41, 128, 185);
            lblTieuDeChiTiet.Location = new Point(15, 23);
            lblTieuDeChiTiet.Name = "lblTieuDeChiTiet";
            lblTieuDeChiTiet.Size = new Size(350, 19);
            lblTieuDeChiTiet.TabIndex = 0;
            lblTieuDeChiTiet.Text = "DANH SÁCH CUỐN SÁCH CỦA PHIẾU ĐANG CHỌN";
            // 
            // dgvChiTietMuon
            // 
            dgvChiTietMuon.AllowUserToAddRows = false;
            dgvChiTietMuon.AllowUserToDeleteRows = false;
            dgvChiTietMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTietMuon.BackgroundColor = Color.White;
            dgvChiTietMuon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTietMuon.Columns.AddRange(new DataGridViewColumn[] { colMaSach, colTenSach, colTinhTrang, colNgayThucTra, colTrangThaiTra });
            dgvChiTietMuon.Location = new Point(12, 505);
            dgvChiTietMuon.MultiSelect = false;
            dgvChiTietMuon.Name = "dgvChiTietMuon";
            dgvChiTietMuon.ReadOnly = true;
            dgvChiTietMuon.RowHeadersVisible = false;
            dgvChiTietMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietMuon.Size = new Size(1076, 180);
            dgvChiTietMuon.TabIndex = 6;
            // 
            // colMaSach
            // 
            colMaSach.DataPropertyName = "MaSach";
            colMaSach.FillWeight = 80F;
            colMaSach.HeaderText = "Mã cuốn";
            colMaSach.Name = "colMaSach";
            colMaSach.ReadOnly = true;
            // 
            // colTenSach
            // 
            colTenSach.DataPropertyName = "TenSach";
            colTenSach.FillWeight = 160F;
            colTenSach.HeaderText = "Tên cuốn sách";
            colTenSach.Name = "colTenSach";
            colTenSach.ReadOnly = true;
            // 
            // colTinhTrang
            // 
            colTinhTrang.DataPropertyName = "TinhTrang";
            colTinhTrang.FillWeight = 100F;
            colTinhTrang.HeaderText = "Tình trạng khi mượn";
            colTinhTrang.Name = "colTinhTrang";
            colTinhTrang.ReadOnly = true;
            // 
            // colNgayThucTra
            // 
            colNgayThucTra.DataPropertyName = "NgayThucTra";
            colNgayThucTra.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" };
            colNgayThucTra.FillWeight = 100F;
            colNgayThucTra.HeaderText = "Ngày thực trả";
            colNgayThucTra.Name = "colNgayThucTra";
            colNgayThucTra.ReadOnly = true;
            // 
            // colTrangThaiTra
            // 
            colTrangThaiTra.DataPropertyName = "TrangThaiTra";
            colTrangThaiTra.FillWeight = 110F;
            colTrangThaiTra.HeaderText = "Trạng thái";
            colTrangThaiTra.Name = "colTrangThaiTra";
            colTrangThaiTra.ReadOnly = true;
            // 
            // frmMuonTraSach
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 698);
            Controls.Add(dgvChiTietMuon);
            Controls.Add(grpChiTiet);
            Controls.Add(dgvPhieuMuon);
            Controls.Add(grpTimKiem);
            Controls.Add(grpChucNangPM);
            Controls.Add(grpThongTinPM);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmMuonTraSach";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Mượn - Trả sách";
            Load += frmMuonTraSach_Load;
            pnlHeader.ResumeLayout(false);
            grpThongTinPM.ResumeLayout(false);
            grpThongTinPM.PerformLayout();
            grpChucNangPM.ResumeLayout(false);
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieuMuon).EndInit();
            grpChiTiet.ResumeLayout(false);
            grpChiTiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietMuon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTinPM;
        private TextBox txtMaPM;
        private Label lblMaPM;
        private ComboBox cboDocGia;
        private Label lblDocGia;
        private ComboBox cboNhanVien;
        private Label lblNhanVien;
        private DateTimePicker dtpNgayMuon;
        private Label lblNgayMuon;
        private DateTimePicker dtpHanTra;
        private Label lblHanTra;
        private GroupBox grpChucNangPM;
        private Button btnThemPM;
        private Button btnXoaPM;
        private Button btnLamMoiPM;
        private Button btnXuatPdf;
        private GroupBox grpTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnTatCa;
        private DataGridView dgvPhieuMuon;
        private DataGridViewTextBoxColumn colMaPM;
        private DataGridViewTextBoxColumn colTenDocGia;
        private DataGridViewTextBoxColumn colTenNV;
        private DataGridViewTextBoxColumn colNgayMuon;
        private DataGridViewTextBoxColumn colHanTra;
        private DataGridViewTextBoxColumn colTrangThai;
        private GroupBox grpChiTiet;
        private Label lblTieuDeChiTiet;
        private Label lblSach;
        private ComboBox cboSach;
        private Label lblTinhTrang;
        private TextBox txtTinhTrang;
        private Button btnThemSach;
        private Button btnTraSach;
        private DataGridView dgvChiTietMuon;
        private DataGridViewTextBoxColumn colMaSach;
        private DataGridViewTextBoxColumn colTenSach;
        private DataGridViewTextBoxColumn colTinhTrang;
        private DataGridViewTextBoxColumn colNgayThucTra;
        private DataGridViewTextBoxColumn colTrangThaiTra;
    }
}
