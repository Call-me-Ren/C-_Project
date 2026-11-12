namespace QLThuVien.GUI
{
    partial class frmLoaiDocGia
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
            txtTenLDG = new TextBox();
            lblTenLDG = new Label();
            txtMaLDG = new TextBox();
            lblMaLDG = new Label();
            grpChucNang = new GroupBox();
            btnLamMoi = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            grpTimKiem = new GroupBox();
            btnTatCa = new Button();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            dgvLoaiDocGia = new DataGridView();
            colMaLDG = new DataGridViewTextBoxColumn();
            colTenLDG = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            grpChucNang.SuspendLayout();
            grpTimKiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoaiDocGia).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(800, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(800, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ LOẠI ĐỘC GIẢ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpThongTin
            // 
            grpThongTin.Controls.Add(txtTenLDG);
            grpThongTin.Controls.Add(lblTenLDG);
            grpThongTin.Controls.Add(txtMaLDG);
            grpThongTin.Controls.Add(lblMaLDG);
            grpThongTin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTin.Location = new Point(12, 56);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(480, 115);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin loại độc giả";
            // 
            // txtTenLDG
            // 
            txtTenLDG.Font = new Font("Segoe UI", 9.5F);
            txtTenLDG.Location = new Point(145, 68);
            txtTenLDG.Name = "txtTenLDG";
            txtTenLDG.Size = new Size(310, 25);
            txtTenLDG.TabIndex = 3;
            // 
            // lblTenLDG
            // 
            lblTenLDG.AutoSize = true;
            lblTenLDG.Font = new Font("Segoe UI", 9.5F);
            lblTenLDG.Location = new Point(18, 71);
            lblTenLDG.Name = "lblTenLDG";
            lblTenLDG.Size = new Size(106, 17);
            lblTenLDG.TabIndex = 2;
            lblTenLDG.Text = "Tên loại độc giả:";
            // 
            // txtMaLDG
            // 
            txtMaLDG.Font = new Font("Segoe UI", 9.5F);
            txtMaLDG.Location = new Point(145, 29);
            txtMaLDG.Name = "txtMaLDG";
            txtMaLDG.Size = new Size(310, 25);
            txtMaLDG.TabIndex = 1;
            // 
            // lblMaLDG
            // 
            lblMaLDG.AutoSize = true;
            lblMaLDG.Font = new Font("Segoe UI", 9.5F);
            lblMaLDG.Location = new Point(18, 32);
            lblMaLDG.Name = "lblMaLDG";
            lblMaLDG.Size = new Size(105, 17);
            lblMaLDG.TabIndex = 0;
            lblMaLDG.Text = "Mã loại độc giả:";
            // 
            // grpChucNang
            // 
            grpChucNang.Controls.Add(btnLamMoi);
            grpChucNang.Controls.Add(btnXoa);
            grpChucNang.Controls.Add(btnSua);
            grpChucNang.Controls.Add(btnThem);
            grpChucNang.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpChucNang.Location = new Point(500, 56);
            grpChucNang.Name = "grpChucNang";
            grpChucNang.Size = new Size(288, 115);
            grpChucNang.TabIndex = 2;
            grpChucNang.TabStop = false;
            grpChucNang.Text = "Chức năng";
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.Location = new Point(150, 68);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 32);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Segoe UI", 9.5F);
            btnXoa.Location = new Point(150, 26);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(120, 32);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Segoe UI", 9.5F);
            btnSua.Location = new Point(18, 68);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(120, 32);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Segoe UI", 9.5F);
            btnThem.Location = new Point(18, 26);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(120, 32);
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
            grpTimKiem.Location = new Point(12, 177);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(776, 65);
            grpTimKiem.TabIndex = 3;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm (theo mã hoặc tên)";
            // 
            // btnTatCa
            // 
            btnTatCa.Font = new Font("Segoe UI", 9.5F);
            btnTatCa.Location = new Point(638, 22);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(120, 30);
            btnTatCa.TabIndex = 2;
            btnTatCa.Text = "Hiện tất cả";
            btnTatCa.UseVisualStyleBackColor = true;
            btnTatCa.Click += btnTatCa_Click;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Font = new Font("Segoe UI", 9.5F);
            btnTimKiem.Location = new Point(506, 22);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(120, 30);
            btnTimKiem.TabIndex = 1;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 9.5F);
            txtTimKiem.Location = new Point(18, 25);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã hoặc tên loại độc giả cần tìm...";
            txtTimKiem.Size = new Size(470, 24);
            txtTimKiem.TabIndex = 0;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // dgvLoaiDocGia
            // 
            dgvLoaiDocGia.AllowUserToAddRows = false;
            dgvLoaiDocGia.AllowUserToDeleteRows = false;
            dgvLoaiDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLoaiDocGia.BackgroundColor = Color.White;
            dgvLoaiDocGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoaiDocGia.Columns.AddRange(new DataGridViewColumn[] { colMaLDG, colTenLDG });
            dgvLoaiDocGia.Location = new Point(12, 248);
            dgvLoaiDocGia.MultiSelect = false;
            dgvLoaiDocGia.Name = "dgvLoaiDocGia";
            dgvLoaiDocGia.ReadOnly = true;
            dgvLoaiDocGia.RowHeadersVisible = false;
            dgvLoaiDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoaiDocGia.Size = new Size(776, 260);
            dgvLoaiDocGia.TabIndex = 4;
            dgvLoaiDocGia.CellClick += dgvLoaiDocGia_CellClick;
            // 
            // colMaLDG
            // 
            colMaLDG.DataPropertyName = "MaLDG";
            colMaLDG.HeaderText = "Mã loại độc giả";
            colMaLDG.Name = "colMaLDG";
            colMaLDG.ReadOnly = true;
            // 
            // colTenLDG
            // 
            colTenLDG.DataPropertyName = "TenLDG";
            colTenLDG.HeaderText = "Tên loại độc giả";
            colTenLDG.Name = "colTenLDG";
            colTenLDG.ReadOnly = true;
            // 
            // frmLoaiDocGia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 520);
            Controls.Add(dgvLoaiDocGia);
            Controls.Add(grpTimKiem);
            Controls.Add(grpChucNang);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmLoaiDocGia";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Loại độc giả";
            Load += frmLoaiDocGia_Load;
            pnlHeader.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            grpChucNang.ResumeLayout(false);
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoaiDocGia).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private TextBox txtTenLDG;
        private Label lblTenLDG;
        private TextBox txtMaLDG;
        private Label lblMaLDG;
        private GroupBox grpChucNang;
        private Button btnLamMoi;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private GroupBox grpTimKiem;
        private Button btnTatCa;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
        private DataGridView dgvLoaiDocGia;
        private DataGridViewTextBoxColumn colMaLDG;
        private DataGridViewTextBoxColumn colTenLDG;
    }
}
