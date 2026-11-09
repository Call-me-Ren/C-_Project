namespace QLThuVien.GUI
{
    partial class frmNhanVien
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
            txtChucVu = new TextBox();
            lblChucVu = new Label();
            txtTenNV = new TextBox();
            lblTenNV = new Label();
            txtHoLot = new TextBox();
            lblHoLot = new Label();
            txtMaNV = new TextBox();
            lblMaNV = new Label();
            grpChucNang = new GroupBox();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            grpTimKiem = new GroupBox();
            btnTatCa = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            dgvNhanVien = new DataGridView();
            colMaNV = new DataGridViewTextBoxColumn();
            colHoLot = new DataGridViewTextBoxColumn();
            colTenNV = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colChucVu = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            grpChucNang.SuspendLayout();
            grpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).BeginInit();
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
            lblTitle.Text = "QUẢN LÝ DANH MỤC NHÂN VIÊN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            //
            // grpThongTin
            //
            grpThongTin.Controls.Add(txtChucVu);
            grpThongTin.Controls.Add(lblChucVu);
            grpThongTin.Controls.Add(txtTenNV);
            grpThongTin.Controls.Add(lblTenNV);
            grpThongTin.Controls.Add(txtHoLot);
            grpThongTin.Controls.Add(lblHoLot);
            grpThongTin.Controls.Add(txtMaNV);
            grpThongTin.Controls.Add(lblMaNV);
            grpThongTin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTin.Location = new Point(12, 56);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(540, 130);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin nhân viên";
            //
            // txtChucVu
            //
            txtChucVu.Font = new Font("Segoe UI", 9.5F);
            txtChucVu.Location = new Point(356, 78);
            txtChucVu.MaxLength = 50;
            txtChucVu.Name = "txtChucVu";
            txtChucVu.Size = new Size(165, 24);
            txtChucVu.TabIndex = 7;
            //
            // lblChucVu
            //
            lblChucVu.AutoSize = true;
            lblChucVu.Font = new Font("Segoe UI", 9.5F);
            lblChucVu.Location = new Point(275, 81);
            lblChucVu.Name = "lblChucVu";
            lblChucVu.Size = new Size(57, 17);
            lblChucVu.TabIndex = 6;
            lblChucVu.Text = "Chức vụ:";
            //
            // txtTenNV
            //
            txtTenNV.Font = new Font("Segoe UI", 9.5F);
            txtTenNV.Location = new Point(356, 35);
            txtTenNV.MaxLength = 50;
            txtTenNV.Name = "txtTenNV";
            txtTenNV.Size = new Size(165, 24);
            txtTenNV.TabIndex = 5;
            //
            // lblTenNV
            //
            lblTenNV.AutoSize = true;
            lblTenNV.Font = new Font("Segoe UI", 9.5F);
            lblTenNV.Location = new Point(275, 38);
            lblTenNV.Name = "lblTenNV";
            lblTenNV.Size = new Size(52, 17);
            lblTenNV.TabIndex = 4;
            lblTenNV.Text = "Tên NV:";
            //
            // txtHoLot
            //
            txtHoLot.Font = new Font("Segoe UI", 9.5F);
            txtHoLot.Location = new Point(94, 78);
            txtHoLot.MaxLength = 50;
            txtHoLot.Name = "txtHoLot";
            txtHoLot.Size = new Size(165, 24);
            txtHoLot.TabIndex = 3;
            //
            // lblHoLot
            //
            lblHoLot.AutoSize = true;
            lblHoLot.Font = new Font("Segoe UI", 9.5F);
            lblHoLot.Location = new Point(15, 81);
            lblHoLot.Name = "lblHoLot";
            lblHoLot.Size = new Size(47, 17);
            lblHoLot.TabIndex = 2;
            lblHoLot.Text = "Họ lót:";
            //
            // txtMaNV
            //
            txtMaNV.Font = new Font("Segoe UI", 9.5F);
            txtMaNV.Location = new Point(94, 35);
            txtMaNV.MaxLength = 20;
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(165, 24);
            txtMaNV.TabIndex = 1;
            //
            // lblMaNV
            //
            lblMaNV.AutoSize = true;
            lblMaNV.Font = new Font("Segoe UI", 9.5F);
            lblMaNV.Location = new Point(15, 38);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(51, 17);
            lblMaNV.TabIndex = 0;
            lblMaNV.Text = "Mã NV:";
            //
            // grpChucNang
            //
            grpChucNang.Controls.Add(btnLamMoi);
            grpChucNang.Controls.Add(btnXoa);
            grpChucNang.Controls.Add(btnSua);
            grpChucNang.Controls.Add(btnThem);
            grpChucNang.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChucNang.Location = new Point(562, 56);
            grpChucNang.Name = "grpChucNang";
            grpChucNang.Size = new Size(310, 130);
            grpChucNang.TabIndex = 2;
            grpChucNang.TabStop = false;
            grpChucNang.Text = "Thao tác";
            //
            // btnLamMoi
            //
            btnLamMoi.BackColor = Color.FromArgb(236, 240, 241);
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.Location = new Point(160, 75);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(130, 32);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            //
            // btnXoa
            //
            btnXoa.BackColor = Color.FromArgb(231, 76, 60);
            btnXoa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(20, 75);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(130, 32);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            //
            // btnSua
            //
            btnSua.BackColor = Color.FromArgb(243, 156, 18);
            btnSua.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(160, 30);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(130, 32);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            //
            // btnThem
            //
            btnThem.BackColor = Color.FromArgb(46, 204, 113);
            btnThem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(20, 30);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(130, 32);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThem_Click;
            //
            // grpTimKiem
            //
            grpTimKiem.Controls.Add(btnTatCa);
            grpTimKiem.Controls.Add(btnTimKiem);
            grpTimKiem.Controls.Add(txtTimKiem);
            grpTimKiem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpTimKiem.Location = new Point(12, 192);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(860, 60);
            grpTimKiem.TabIndex = 3;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm";
            //
            // btnTatCa
            //
            btnTatCa.BackColor = Color.FromArgb(236, 240, 241);
            btnTatCa.Font = new Font("Segoe UI", 9.5F);
            btnTatCa.Location = new Point(740, 20);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(100, 28);
            btnTatCa.TabIndex = 2;
            btnTatCa.Text = "Tất cả";
            btnTatCa.UseVisualStyleBackColor = false;
            btnTatCa.Click += btnTatCa_Click;
            //
            // btnTimKiem
            //
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(625, 20);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(105, 28);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            //
            // txtTimKiem
            //
            txtTimKiem.Font = new Font("Segoe UI", 9.5F);
            txtTimKiem.Location = new Point(15, 22);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã, tên hoặc chức vụ để tìm...";
            txtTimKiem.Size = new Size(595, 24);
            txtTimKiem.TabIndex = 0;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            //
            // dgvNhanVien
            //
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.BackgroundColor = Color.White;
            dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNhanVien.Columns.AddRange(new DataGridViewColumn[] { colMaNV, colHoLot, colTenNV, colHoTen, colChucVu });
            dgvNhanVien.Location = new Point(12, 260);
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.Name = "dgvNhanVien";
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.RowHeadersWidth = 30;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.Size = new Size(860, 270);
            dgvNhanVien.TabIndex = 4;
            dgvNhanVien.CellClick += dgvNhanVien_CellClick;
            //
            // colMaNV
            //
            colMaNV.DataPropertyName = "MaNV";
            colMaNV.FillWeight = 80F;
            colMaNV.HeaderText = "Mã NV";
            colMaNV.Name = "colMaNV";
            colMaNV.ReadOnly = true;
            //
            // colHoLot
            //
            colHoLot.DataPropertyName = "HoLot";
            colHoLot.FillWeight = 110F;
            colHoLot.HeaderText = "Họ lót";
            colHoLot.Name = "colHoLot";
            colHoLot.ReadOnly = true;
            //
            // colTenNV
            //
            colTenNV.DataPropertyName = "TenNV";
            colTenNV.FillWeight = 90F;
            colTenNV.HeaderText = "Tên NV";
            colTenNV.Name = "colTenNV";
            colTenNV.ReadOnly = true;
            //
            // colHoTen
            //
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.FillWeight = 140F;
            colHoTen.HeaderText = "Họ và tên";
            colHoTen.Name = "colHoTen";
            colHoTen.ReadOnly = true;
            //
            // colChucVu
            //
            colChucVu.DataPropertyName = "ChucVu";
            colChucVu.FillWeight = 110F;
            colChucVu.HeaderText = "Chức vụ";
            colChucVu.Name = "colChucVu";
            colChucVu.ReadOnly = true;
            //
            // frmNhanVien
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(884, 542);
            Controls.Add(dgvNhanVien);
            Controls.Add(grpTimKiem);
            Controls.Add(grpChucNang);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmNhanVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Danh mục Nhân viên";
            Load += frmNhanVien_Load;
            pnlHeader.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            grpChucNang.ResumeLayout(false);
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private TextBox txtChucVu;
        private Label lblChucVu;
        private TextBox txtTenNV;
        private Label lblTenNV;
        private TextBox txtHoLot;
        private Label lblHoLot;
        private TextBox txtMaNV;
        private Label lblMaNV;
        private GroupBox grpChucNang;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private GroupBox grpTimKiem;
        private Button btnTatCa;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
        private DataGridView dgvNhanVien;
        private DataGridViewTextBoxColumn colMaNV;
        private DataGridViewTextBoxColumn colHoLot;
        private DataGridViewTextBoxColumn colTenNV;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colChucVu;
    }
}
