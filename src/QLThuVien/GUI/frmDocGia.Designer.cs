namespace QLThuVien.GUI
{
    partial class frmDocGia
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
            grpThongTin = new GroupBox();
            cboLoaiDocGia = new ComboBox();
            lblLoaiDocGia = new Label();
            txtSDT = new TextBox();
            lblSDT = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblNgaySinh = new Label();
            txtTenDG = new TextBox();
            lblTenDG = new Label();
            txtHoLot = new TextBox();
            lblHoLot = new Label();
            txtMaDG = new TextBox();
            lblMaDG = new Label();
            grpChucNang = new GroupBox();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            grpTimKiem = new GroupBox();
            btnTatCa = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            dgvDocGia = new DataGridView();
            colMaDG = new DataGridViewTextBoxColumn();
            colHoLot = new DataGridViewTextBoxColumn();
            colTenDG = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colNgaySinh = new DataGridViewTextBoxColumn();
            colSDT = new DataGridViewTextBoxColumn();
            colTenLDG = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            grpChucNang.SuspendLayout();
            grpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocGia).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(884, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(884, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ ĐỘC GIẢ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpThongTin
            // 
            grpThongTin.Controls.Add(cboLoaiDocGia);
            grpThongTin.Controls.Add(lblLoaiDocGia);
            grpThongTin.Controls.Add(txtSDT);
            grpThongTin.Controls.Add(lblSDT);
            grpThongTin.Controls.Add(dtpNgaySinh);
            grpThongTin.Controls.Add(lblNgaySinh);
            grpThongTin.Controls.Add(txtTenDG);
            grpThongTin.Controls.Add(lblTenDG);
            grpThongTin.Controls.Add(txtHoLot);
            grpThongTin.Controls.Add(lblHoLot);
            grpThongTin.Controls.Add(txtMaDG);
            grpThongTin.Controls.Add(lblMaDG);
            grpThongTin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTin.Location = new Point(12, 56);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(570, 145);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin độc giả";
            // 
            // cboLoaiDocGia
            // 
            cboLoaiDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiDocGia.Font = new Font("Segoe UI", 9.5F);
            cboLoaiDocGia.FormattingEnabled = true;
            cboLoaiDocGia.Location = new Point(380, 103);
            cboLoaiDocGia.Name = "cboLoaiDocGia";
            cboLoaiDocGia.Size = new Size(170, 25);
            cboLoaiDocGia.TabIndex = 11;
            // 
            // lblLoaiDocGia
            // 
            lblLoaiDocGia.AutoSize = true;
            lblLoaiDocGia.Font = new Font("Segoe UI", 9.5F);
            lblLoaiDocGia.Location = new Point(292, 106);
            lblLoaiDocGia.Name = "lblLoaiDocGia";
            lblLoaiDocGia.Size = new Size(82, 17);
            lblLoaiDocGia.TabIndex = 10;
            lblLoaiDocGia.Text = "Loại độc giả:";
            // 
            // txtSDT
            // 
            txtSDT.Font = new Font("Segoe UI", 9.5F);
            txtSDT.Location = new Point(380, 65);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(170, 25);
            txtSDT.TabIndex = 9;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new Font("Segoe UI", 9.5F);
            lblSDT.Location = new Point(292, 68);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(87, 17);
            lblSDT.TabIndex = 8;
            lblSDT.Text = "Số điện thoại:";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Font = new Font("Segoe UI", 9.5F);
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(380, 26);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.ShowCheckBox = true;
            dtpNgaySinh.Size = new Size(170, 25);
            dtpNgaySinh.TabIndex = 7;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Font = new Font("Segoe UI", 9.5F);
            lblNgaySinh.Location = new Point(292, 30);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(69, 17);
            lblNgaySinh.TabIndex = 6;
            lblNgaySinh.Text = "Ngày sinh:";
            // 
            // txtTenDG
            // 
            txtTenDG.Font = new Font("Segoe UI", 9.5F);
            txtTenDG.Location = new Point(95, 103);
            txtTenDG.Name = "txtTenDG";
            txtTenDG.Size = new Size(175, 25);
            txtTenDG.TabIndex = 5;
            // 
            // lblTenDG
            // 
            lblTenDG.AutoSize = true;
            lblTenDG.Font = new Font("Segoe UI", 9.5F);
            lblTenDG.Location = new Point(14, 106);
            lblTenDG.Name = "lblTenDG";
            lblTenDG.Size = new Size(54, 17);
            lblTenDG.TabIndex = 4;
            lblTenDG.Text = "Tên ĐG:";
            // 
            // txtHoLot
            // 
            txtHoLot.Font = new Font("Segoe UI", 9.5F);
            txtHoLot.Location = new Point(95, 65);
            txtHoLot.Name = "txtHoLot";
            txtHoLot.Size = new Size(175, 25);
            txtHoLot.TabIndex = 3;
            // 
            // lblHoLot
            // 
            lblHoLot.AutoSize = true;
            lblHoLot.Font = new Font("Segoe UI", 9.5F);
            lblHoLot.Location = new Point(14, 68);
            lblHoLot.Name = "lblHoLot";
            lblHoLot.Size = new Size(49, 17);
            lblHoLot.TabIndex = 2;
            lblHoLot.Text = "Họ lót:";
            // 
            // txtMaDG
            // 
            txtMaDG.Font = new Font("Segoe UI", 9.5F);
            txtMaDG.Location = new Point(95, 26);
            txtMaDG.Name = "txtMaDG";
            txtMaDG.Size = new Size(175, 25);
            txtMaDG.TabIndex = 1;
            // 
            // lblMaDG
            // 
            lblMaDG.AutoSize = true;
            lblMaDG.Font = new Font("Segoe UI", 9.5F);
            lblMaDG.Location = new Point(14, 30);
            lblMaDG.Name = "lblMaDG";
            lblMaDG.Size = new Size(53, 17);
            lblMaDG.TabIndex = 0;
            lblMaDG.Text = "Mã ĐG:";
            // 
            // grpChucNang
            // 
            grpChucNang.Controls.Add(btnLamMoi);
            grpChucNang.Controls.Add(btnXoa);
            grpChucNang.Controls.Add(btnSua);
            grpChucNang.Controls.Add(btnThem);
            grpChucNang.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChucNang.Location = new Point(590, 56);
            grpChucNang.Name = "grpChucNang";
            grpChucNang.Size = new Size(282, 145);
            grpChucNang.TabIndex = 2;
            grpChucNang.TabStop = false;
            grpChucNang.Text = "Chức năng";
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.Location = new Point(148, 85);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(118, 36);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Segoe UI", 9.5F);
            btnXoa.Location = new Point(148, 32);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(118, 36);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Segoe UI", 9.5F);
            btnSua.Location = new Point(16, 85);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(118, 36);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Segoe UI", 9.5F);
            btnThem.Location = new Point(16, 32);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(118, 36);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // grpTimKiem
            // 
            grpTimKiem.Controls.Add(btnTatCa);
            grpTimKiem.Controls.Add(btnTimKiem);
            grpTimKiem.Controls.Add(txtTimKiem);
            grpTimKiem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpTimKiem.Location = new Point(12, 207);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(860, 65);
            grpTimKiem.TabIndex = 3;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm độc giả (theo mã, tên, SĐT, loại độc giả)";
            // 
            // btnTatCa
            // 
            btnTatCa.Font = new Font("Segoe UI", 9.5F);
            btnTatCa.Location = new Point(720, 22);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(125, 30);
            btnTatCa.TabIndex = 2;
            btnTatCa.Text = "Hiện tất cả";
            btnTatCa.UseVisualStyleBackColor = true;
            btnTatCa.Click += btnTatCa_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Segoe UI", 9.5F);
            btnTimKiem.Location = new Point(585, 22);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(125, 30);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 9.5F);
            txtTimKiem.Location = new Point(14, 25);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã, tên độc giả, số điện thoại hoặc loại độc giả cần tìm...";
            txtTimKiem.Size = new Size(550, 24);
            txtTimKiem.TabIndex = 0;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // dgvDocGia
            // 
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocGia.BackgroundColor = Color.White;
            dgvDocGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDocGia.Columns.AddRange(new DataGridViewColumn[] { colMaDG, colHoLot, colTenDG, colHoTen, colNgaySinh, colSDT, colTenLDG });
            dgvDocGia.Location = new Point(12, 278);
            dgvDocGia.MultiSelect = false;
            dgvDocGia.Name = "dgvDocGia";
            dgvDocGia.ReadOnly = true;
            dgvDocGia.RowHeadersVisible = false;
            dgvDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocGia.Size = new Size(860, 310);
            dgvDocGia.TabIndex = 4;
            dgvDocGia.CellClick += dgvDocGia_CellClick;
            // 
            // colMaDG
            // 
            colMaDG.DataPropertyName = "MaDG";
            colMaDG.HeaderText = "Mã ĐG";
            colMaDG.Name = "colMaDG";
            colMaDG.ReadOnly = true;
            // 
            // colHoLot
            // 
            colHoLot.DataPropertyName = "HoLot";
            colHoLot.HeaderText = "Họ lót";
            colHoLot.Name = "colHoLot";
            colHoLot.ReadOnly = true;
            // 
            // colTenDG
            // 
            colTenDG.DataPropertyName = "TenDG";
            colTenDG.HeaderText = "Tên";
            colTenDG.Name = "colTenDG";
            colTenDG.ReadOnly = true;
            // 
            // colHoTen
            // 
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.HeaderText = "Họ và tên";
            colHoTen.Name = "colHoTen";
            colHoTen.ReadOnly = true;
            // 
            // colNgaySinh
            // 
            colNgaySinh.DataPropertyName = "NgaySinh";
            colNgaySinh.HeaderText = "Ngày sinh";
            colNgaySinh.Name = "colNgaySinh";
            colNgaySinh.ReadOnly = true;
            // 
            // colSDT
            // 
            colSDT.DataPropertyName = "SDT";
            colSDT.HeaderText = "Số điện thoại";
            colSDT.Name = "colSDT";
            colSDT.ReadOnly = true;
            // 
            // colTenLDG
            // 
            colTenLDG.DataPropertyName = "TenLDG";
            colTenLDG.HeaderText = "Loại độc giả";
            colTenLDG.Name = "colTenLDG";
            colTenLDG.ReadOnly = true;
            // 
            // frmDocGia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 600);
            Controls.Add(dgvDocGia);
            Controls.Add(grpTimKiem);
            Controls.Add(grpChucNang);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmDocGia";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Độc giả";
            Load += frmDocGia_Load;
            pnlHeader.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            grpChucNang.ResumeLayout(false);
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocGia).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private TextBox txtMaDG;
        private Label lblMaDG;
        private TextBox txtHoLot;
        private Label lblHoLot;
        private TextBox txtTenDG;
        private Label lblTenDG;
        private DateTimePicker dtpNgaySinh;
        private Label lblNgaySinh;
        private TextBox txtSDT;
        private Label lblSDT;
        private ComboBox cboLoaiDocGia;
        private Label lblLoaiDocGia;
        private GroupBox grpChucNang;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private GroupBox grpTimKiem;
        private Button btnTatCa;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
        private DataGridView dgvDocGia;
        private DataGridViewTextBoxColumn colMaDG;
        private DataGridViewTextBoxColumn colHoLot;
        private DataGridViewTextBoxColumn colTenDG;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colNgaySinh;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colTenLDG;
    }
}
