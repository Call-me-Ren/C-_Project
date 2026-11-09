namespace QLThuVien.GUI
{
    partial class frmQuyDinhPhat
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
            numMucPhat = new NumericUpDown();
            lblMucPhat = new Label();
            txtDonViTinh = new TextBox();
            lblDonViTinh = new Label();
            txtCachTinh = new TextBox();
            lblCachTinh = new Label();
            txtLoaiPhat = new TextBox();
            lblLoaiPhat = new Label();
            txtTenQD = new TextBox();
            lblTenQD = new Label();
            txtMaQD = new TextBox();
            lblMaQD = new Label();
            grpChucNang = new GroupBox();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            grpTimKiem = new GroupBox();
            btnTatCa = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            dgvQuyDinhPhat = new DataGridView();
            colMaQD = new DataGridViewTextBoxColumn();
            colTenQD = new DataGridViewTextBoxColumn();
            colLoaiPhat = new DataGridViewTextBoxColumn();
            colCachTinh = new DataGridViewTextBoxColumn();
            colDonViTinh = new DataGridViewTextBoxColumn();
            colMucPhat = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMucPhat).BeginInit();
            grpChucNang.SuspendLayout();
            grpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuyDinhPhat).BeginInit();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(920, 50);
            pnlHeader.TabIndex = 0;
            //
            // lblTitle
            //
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(920, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ QUY ĐỊNH PHẠT";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            //
            // grpThongTin
            //
            grpThongTin.Controls.Add(numMucPhat);
            grpThongTin.Controls.Add(lblMucPhat);
            grpThongTin.Controls.Add(txtDonViTinh);
            grpThongTin.Controls.Add(lblDonViTinh);
            grpThongTin.Controls.Add(txtCachTinh);
            grpThongTin.Controls.Add(lblCachTinh);
            grpThongTin.Controls.Add(txtLoaiPhat);
            grpThongTin.Controls.Add(lblLoaiPhat);
            grpThongTin.Controls.Add(txtTenQD);
            grpThongTin.Controls.Add(lblTenQD);
            grpThongTin.Controls.Add(txtMaQD);
            grpThongTin.Controls.Add(lblMaQD);
            grpThongTin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTin.Location = new Point(12, 56);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(580, 150);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin quy định phạt";
            //
            // numMucPhat
            //
            numMucPhat.DecimalPlaces = 2;
            numMucPhat.Font = new Font("Segoe UI", 9.5F);
            numMucPhat.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numMucPhat.Location = new Point(390, 110);
            numMucPhat.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numMucPhat.Name = "numMucPhat";
            numMucPhat.Size = new Size(175, 24);
            numMucPhat.TabIndex = 11;
            numMucPhat.ThousandsSeparator = true;
            //
            // lblMucPhat
            //
            lblMucPhat.AutoSize = true;
            lblMucPhat.Font = new Font("Segoe UI", 9.5F);
            lblMucPhat.Location = new Point(300, 113);
            lblMucPhat.Name = "lblMucPhat";
            lblMucPhat.Size = new Size(67, 17);
            lblMucPhat.TabIndex = 10;
            lblMucPhat.Text = "Mức phạt:";
            //
            // txtDonViTinh
            //
            txtDonViTinh.Font = new Font("Segoe UI", 9.5F);
            txtDonViTinh.Location = new Point(95, 110);
            txtDonViTinh.MaxLength = 20;
            txtDonViTinh.Name = "txtDonViTinh";
            txtDonViTinh.Size = new Size(185, 24);
            txtDonViTinh.TabIndex = 9;
            //
            // lblDonViTinh
            //
            lblDonViTinh.AutoSize = true;
            lblDonViTinh.Font = new Font("Segoe UI", 9.5F);
            lblDonViTinh.Location = new Point(15, 113);
            lblDonViTinh.Name = "lblDonViTinh";
            lblDonViTinh.Size = new Size(73, 17);
            lblDonViTinh.TabIndex = 8;
            lblDonViTinh.Text = "Đơn vị tính:";
            //
            // txtCachTinh
            //
            txtCachTinh.Font = new Font("Segoe UI", 9.5F);
            txtCachTinh.Location = new Point(390, 70);
            txtCachTinh.MaxLength = 100;
            txtCachTinh.Name = "txtCachTinh";
            txtCachTinh.Size = new Size(175, 24);
            txtCachTinh.TabIndex = 7;
            //
            // lblCachTinh
            //
            lblCachTinh.AutoSize = true;
            lblCachTinh.Font = new Font("Segoe UI", 9.5F);
            lblCachTinh.Location = new Point(300, 73);
            lblCachTinh.Name = "lblCachTinh";
            lblCachTinh.Size = new Size(64, 17);
            lblCachTinh.TabIndex = 6;
            lblCachTinh.Text = "Cách tính:";
            //
            // txtLoaiPhat
            //
            txtLoaiPhat.Font = new Font("Segoe UI", 9.5F);
            txtLoaiPhat.Location = new Point(95, 70);
            txtLoaiPhat.MaxLength = 50;
            txtLoaiPhat.Name = "txtLoaiPhat";
            txtLoaiPhat.Size = new Size(185, 24);
            txtLoaiPhat.TabIndex = 5;
            //
            // lblLoaiPhat
            //
            lblLoaiPhat.AutoSize = true;
            lblLoaiPhat.Font = new Font("Segoe UI", 9.5F);
            lblLoaiPhat.Location = new Point(15, 73);
            lblLoaiPhat.Name = "lblLoaiPhat";
            lblLoaiPhat.Size = new Size(65, 17);
            lblLoaiPhat.TabIndex = 4;
            lblLoaiPhat.Text = "Loại phạt:";
            //
            // txtTenQD
            //
            txtTenQD.Font = new Font("Segoe UI", 9.5F);
            txtTenQD.Location = new Point(390, 30);
            txtTenQD.MaxLength = 100;
            txtTenQD.Name = "txtTenQD";
            txtTenQD.Size = new Size(175, 24);
            txtTenQD.TabIndex = 3;
            //
            // lblTenQD
            //
            lblTenQD.AutoSize = true;
            lblTenQD.Font = new Font("Segoe UI", 9.5F);
            lblTenQD.Location = new Point(300, 33);
            lblTenQD.Name = "lblTenQD";
            lblTenQD.Size = new Size(54, 17);
            lblTenQD.TabIndex = 2;
            lblTenQD.Text = "Tên QĐ:";
            //
            // txtMaQD
            //
            txtMaQD.Font = new Font("Segoe UI", 9.5F);
            txtMaQD.Location = new Point(95, 30);
            txtMaQD.MaxLength = 20;
            txtMaQD.Name = "txtMaQD";
            txtMaQD.Size = new Size(185, 24);
            txtMaQD.TabIndex = 1;
            //
            // lblMaQD
            //
            lblMaQD.AutoSize = true;
            lblMaQD.Font = new Font("Segoe UI", 9.5F);
            lblMaQD.Location = new Point(15, 33);
            lblMaQD.Name = "lblMaQD";
            lblMaQD.Size = new Size(53, 17);
            lblMaQD.TabIndex = 0;
            lblMaQD.Text = "Mã QĐ:";
            //
            // grpChucNang
            //
            grpChucNang.Controls.Add(btnLamMoi);
            grpChucNang.Controls.Add(btnXoa);
            grpChucNang.Controls.Add(btnSua);
            grpChucNang.Controls.Add(btnThem);
            grpChucNang.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChucNang.Location = new Point(602, 56);
            grpChucNang.Name = "grpChucNang";
            grpChucNang.Size = new Size(306, 150);
            grpChucNang.TabIndex = 2;
            grpChucNang.TabStop = false;
            grpChucNang.Text = "Thao tác";
            //
            // btnLamMoi
            //
            btnLamMoi.BackColor = Color.FromArgb(236, 240, 241);
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.Location = new Point(155, 85);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(135, 35);
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
            btnXoa.Location = new Point(15, 85);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(130, 35);
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
            btnSua.Location = new Point(155, 32);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(135, 35);
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
            btnThem.Location = new Point(15, 32);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(130, 35);
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
            grpTimKiem.Location = new Point(12, 212);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(896, 60);
            grpTimKiem.TabIndex = 3;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm";
            //
            // btnTatCa
            //
            btnTatCa.BackColor = Color.FromArgb(236, 240, 241);
            btnTatCa.Font = new Font("Segoe UI", 9.5F);
            btnTatCa.Location = new Point(780, 20);
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
            btnTimKiem.Location = new Point(660, 20);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(110, 28);
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
            txtTimKiem.PlaceholderText = "Nhập mã, tên quy định hoặc loại phạt để tìm...";
            txtTimKiem.Size = new Size(630, 24);
            txtTimKiem.TabIndex = 0;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            //
            // dgvQuyDinhPhat
            //
            dgvQuyDinhPhat.AllowUserToAddRows = false;
            dgvQuyDinhPhat.AllowUserToDeleteRows = false;
            dgvQuyDinhPhat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuyDinhPhat.BackgroundColor = Color.White;
            dgvQuyDinhPhat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuyDinhPhat.Columns.AddRange(new DataGridViewColumn[] { colMaQD, colTenQD, colLoaiPhat, colCachTinh, colDonViTinh, colMucPhat });
            dgvQuyDinhPhat.Location = new Point(12, 280);
            dgvQuyDinhPhat.MultiSelect = false;
            dgvQuyDinhPhat.Name = "dgvQuyDinhPhat";
            dgvQuyDinhPhat.ReadOnly = true;
            dgvQuyDinhPhat.RowHeadersWidth = 30;
            dgvQuyDinhPhat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuyDinhPhat.Size = new Size(896, 260);
            dgvQuyDinhPhat.TabIndex = 4;
            dgvQuyDinhPhat.CellClick += dgvQuyDinhPhat_CellClick;
            //
            // colMaQD
            //
            colMaQD.DataPropertyName = "MaQD";
            colMaQD.FillWeight = 75F;
            colMaQD.HeaderText = "Mã QĐ";
            colMaQD.Name = "colMaQD";
            colMaQD.ReadOnly = true;
            //
            // colTenQD
            //
            colTenQD.DataPropertyName = "TenQD";
            colTenQD.FillWeight = 160F;
            colTenQD.HeaderText = "Tên quy định";
            colTenQD.Name = "colTenQD";
            colTenQD.ReadOnly = true;
            //
            // colLoaiPhat
            //
            colLoaiPhat.DataPropertyName = "LoaiPhat";
            colLoaiPhat.FillWeight = 90F;
            colLoaiPhat.HeaderText = "Loại phạt";
            colLoaiPhat.Name = "colLoaiPhat";
            colLoaiPhat.ReadOnly = true;
            //
            // colCachTinh
            //
            colCachTinh.DataPropertyName = "CachTinh";
            colCachTinh.FillWeight = 110F;
            colCachTinh.HeaderText = "Cách tính";
            colCachTinh.Name = "colCachTinh";
            colCachTinh.ReadOnly = true;
            //
            // colDonViTinh
            //
            colDonViTinh.DataPropertyName = "DonViTinh";
            colDonViTinh.FillWeight = 75F;
            colDonViTinh.HeaderText = "Đơn vị tính";
            colDonViTinh.Name = "colDonViTinh";
            colDonViTinh.ReadOnly = true;
            //
            // colMucPhat
            //
            colMucPhat.DataPropertyName = "MucPhat";
            colMucPhat.DefaultCellStyle.Format = "N2";
            colMucPhat.FillWeight = 90F;
            colMucPhat.HeaderText = "Mức phạt";
            colMucPhat.Name = "colMucPhat";
            colMucPhat.ReadOnly = true;
            //
            // frmQuyDinhPhat
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(920, 552);
            Controls.Add(dgvQuyDinhPhat);
            Controls.Add(grpTimKiem);
            Controls.Add(grpChucNang);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmQuyDinhPhat";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Quy định phạt";
            Load += frmQuyDinhPhat_Load;
            pnlHeader.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMucPhat).EndInit();
            grpChucNang.ResumeLayout(false);
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuyDinhPhat).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private NumericUpDown numMucPhat;
        private Label lblMucPhat;
        private TextBox txtDonViTinh;
        private Label lblDonViTinh;
        private TextBox txtCachTinh;
        private Label lblCachTinh;
        private TextBox txtLoaiPhat;
        private Label lblLoaiPhat;
        private TextBox txtTenQD;
        private Label lblTenQD;
        private TextBox txtMaQD;
        private Label lblMaQD;
        private GroupBox grpChucNang;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private GroupBox grpTimKiem;
        private Button btnTatCa;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
        private DataGridView dgvQuyDinhPhat;
        private DataGridViewTextBoxColumn colMaQD;
        private DataGridViewTextBoxColumn colTenQD;
        private DataGridViewTextBoxColumn colLoaiPhat;
        private DataGridViewTextBoxColumn colCachTinh;
        private DataGridViewTextBoxColumn colDonViTinh;
        private DataGridViewTextBoxColumn colMucPhat;
    }
}
