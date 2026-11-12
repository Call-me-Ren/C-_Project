namespace QLThuVien.GUI
{
    partial class frmChiTietPhieuPhat
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
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            dgvChiTiet = new DataGridView();
            colMaCTPP = new DataGridViewTextBoxColumn();
            colMaSach = new DataGridViewTextBoxColumn();
            colMaQD = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();
            colSoNgayTre = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();
            colTienPhat = new DataGridViewTextBoxColumn();
            lblTongTien = new Label();
            btnDong = new Button();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1064, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1064, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHI TIẾT VI PHẠM PHIẾU PHẠT";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpThongTin
            // 
            grpThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpThongTin.Controls.Add(lblSach);
            grpThongTin.Controls.Add(cboSach);
            grpThongTin.Controls.Add(lblQuyDinh);
            grpThongTin.Controls.Add(cboQuyDinh);
            grpThongTin.Controls.Add(lblSoNgayTre);
            grpThongTin.Controls.Add(txtSoNgayTre);
            grpThongTin.Controls.Add(lblDonGia);
            grpThongTin.Controls.Add(txtDonGia);
            grpThongTin.Controls.Add(lblTienPhat);
            grpThongTin.Controls.Add(txtTienPhat);
            grpThongTin.Controls.Add(lblGhiChu);
            grpThongTin.Controls.Add(txtGhiChu);
            grpThongTin.Controls.Add(btnThem);
            grpThongTin.Controls.Add(btnSua);
            grpThongTin.Controls.Add(btnXoa);
            grpThongTin.Controls.Add(btnLamMoi);
            grpThongTin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpThongTin.Location = new Point(12, 56);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(1040, 190);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin vi phạm";
            // 
            // lblSach
            // 
            lblSach.AutoSize = true;
            lblSach.Font = new Font("Segoe UI", 9.5F);
            lblSach.Location = new Point(18, 30);
            lblSach.Name = "lblSach";
            lblSach.Size = new Size(71, 17);
            lblSach.TabIndex = 0;
            lblSach.Text = "Cuốn sách:";
            // 
            // cboSach
            // 
            cboSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSach.Font = new Font("Segoe UI", 9.5F);
            cboSach.Location = new Point(105, 26);
            cboSach.Name = "cboSach";
            cboSach.Size = new Size(390, 25);
            cboSach.TabIndex = 1;
            // 
            // lblQuyDinh
            // 
            lblQuyDinh.AutoSize = true;
            lblQuyDinh.Font = new Font("Segoe UI", 9.5F);
            lblQuyDinh.Location = new Point(530, 30);
            lblQuyDinh.Name = "lblQuyDinh";
            lblQuyDinh.Size = new Size(64, 17);
            lblQuyDinh.TabIndex = 2;
            lblQuyDinh.Text = "Quy định:";
            // 
            // cboQuyDinh
            // 
            cboQuyDinh.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboQuyDinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyDinh.Font = new Font("Segoe UI", 9.5F);
            cboQuyDinh.Location = new Point(610, 26);
            cboQuyDinh.Name = "cboQuyDinh";
            cboQuyDinh.Size = new Size(410, 25);
            cboQuyDinh.TabIndex = 3;
            // 
            // lblSoNgayTre
            // 
            lblSoNgayTre.AutoSize = true;
            lblSoNgayTre.Font = new Font("Segoe UI", 9.5F);
            lblSoNgayTre.Location = new Point(18, 70);
            lblSoNgayTre.Name = "lblSoNgayTre";
            lblSoNgayTre.Size = new Size(82, 17);
            lblSoNgayTre.TabIndex = 4;
            lblSoNgayTre.Text = "Số ngày trễ:";
            // 
            // txtSoNgayTre
            // 
            txtSoNgayTre.Font = new Font("Segoe UI", 9.5F);
            txtSoNgayTre.Location = new Point(105, 66);
            txtSoNgayTre.Name = "txtSoNgayTre";
            txtSoNgayTre.ReadOnly = true;
            txtSoNgayTre.Size = new Size(120, 25);
            txtSoNgayTre.TabIndex = 5;
            // 
            // lblDonGia
            // 
            lblDonGia.AutoSize = true;
            lblDonGia.Font = new Font("Segoe UI", 9.5F);
            lblDonGia.Location = new Point(260, 70);
            lblDonGia.Name = "lblDonGia";
            lblDonGia.Size = new Size(57, 17);
            lblDonGia.TabIndex = 6;
            lblDonGia.Text = "Đơn giá:";
            // 
            // txtDonGia
            // 
            txtDonGia.Font = new Font("Segoe UI", 9.5F);
            txtDonGia.Location = new Point(330, 66);
            txtDonGia.Name = "txtDonGia";
            txtDonGia.ReadOnly = true;
            txtDonGia.Size = new Size(165, 25);
            txtDonGia.TabIndex = 7;
            txtDonGia.TextAlign = HorizontalAlignment.Right;
            // 
            // lblTienPhat
            // 
            lblTienPhat.AutoSize = true;
            lblTienPhat.Font = new Font("Segoe UI", 9.5F);
            lblTienPhat.Location = new Point(530, 70);
            lblTienPhat.Name = "lblTienPhat";
            lblTienPhat.Size = new Size(67, 17);
            lblTienPhat.TabIndex = 8;
            lblTienPhat.Text = "Tiền phạt:";
            // 
            // txtTienPhat
            // 
            txtTienPhat.Font = new Font("Segoe UI", 9.5F);
            txtTienPhat.Location = new Point(610, 66);
            txtTienPhat.Name = "txtTienPhat";
            txtTienPhat.ReadOnly = true;
            txtTienPhat.Size = new Size(180, 25);
            txtTienPhat.TabIndex = 9;
            txtTienPhat.TextAlign = HorizontalAlignment.Right;
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.Font = new Font("Segoe UI", 9.5F);
            lblGhiChu.Location = new Point(18, 110);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(55, 17);
            lblGhiChu.TabIndex = 10;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtGhiChu.Font = new Font("Segoe UI", 9.5F);
            txtGhiChu.Location = new Point(105, 106);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(915, 25);
            txtGhiChu.TabIndex = 11;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Segoe UI", 9.5F);
            btnThem.Location = new Point(18, 146);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(130, 32);
            btnThem.TabIndex = 12;
            btnThem.Text = "Thêm vi phạm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Segoe UI", 9.5F);
            btnSua.Location = new Point(160, 146);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(130, 32);
            btnSua.TabIndex = 13;
            btnSua.Text = "Sửa vi phạm";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Segoe UI", 9.5F);
            btnXoa.Location = new Point(302, 146);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(130, 32);
            btnXoa.TabIndex = 14;
            btnXoa.Text = "Xóa vi phạm";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.Location = new Point(444, 146);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 32);
            btnLamMoi.TabIndex = 15;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Columns.AddRange(new DataGridViewColumn[] { colMaCTPP, colMaSach, colMaQD, colDonGia, colSoNgayTre, colGhiChu, colTienPhat });
            dgvChiTiet.Location = new Point(12, 256);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(1040, 360);
            dgvChiTiet.TabIndex = 2;
            // 
            // colMaCTPP
            // 
            colMaCTPP.DataPropertyName = "MaCTPP";
            colMaCTPP.HeaderText = "Mã vi phạm";
            colMaCTPP.Name = "colMaCTPP";
            colMaCTPP.ReadOnly = true;
            // 
            // colMaSach
            // 
            colMaSach.DataPropertyName = "MaSach";
            colMaSach.HeaderText = "Mã cuốn";
            colMaSach.Name = "colMaSach";
            colMaSach.ReadOnly = true;
            // 
            // colMaQD
            // 
            colMaQD.DataPropertyName = "MaQD";
            colMaQD.HeaderText = "Mã quy định";
            colMaQD.Name = "colMaQD";
            colMaQD.ReadOnly = true;
            // 
            // colDonGia
            // 
            colDonGia.DataPropertyName = "DonGia";
            colDonGia.DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" };
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.Name = "colDonGia";
            colDonGia.ReadOnly = true;
            // 
            // colSoNgayTre
            // 
            colSoNgayTre.DataPropertyName = "SoNgayTre";
            colSoNgayTre.HeaderText = "Số ngày trễ";
            colSoNgayTre.Name = "colSoNgayTre";
            colSoNgayTre.ReadOnly = true;
            // 
            // colGhiChu
            // 
            colGhiChu.DataPropertyName = "GhiChu";
            colGhiChu.HeaderText = "Ghi chú";
            colGhiChu.Name = "colGhiChu";
            colGhiChu.ReadOnly = true;
            // 
            // colTienPhat
            // 
            colTienPhat.DataPropertyName = "TienPhat";
            colTienPhat.DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" };
            colTienPhat.HeaderText = "Tiền phạt";
            colTienPhat.Name = "colTienPhat";
            colTienPhat.ReadOnly = true;
            // 
            // lblTongTien
            // 
            lblTongTien.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Location = new Point(12, 630);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(150, 20);
            lblTongTien.TabIndex = 3;
            lblTongTien.Text = "Tổng tiền phạt: 0 đ";
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Font = new Font("Segoe UI", 9.5F);
            btnDong.Location = new Point(932, 624);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(120, 32);
            btnDong.TabIndex = 4;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            // 
            // frmChiTietPhieuPhat
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 668);
            Controls.Add(btnDong);
            Controls.Add(lblTongTien);
            Controls.Add(dgvChiTiet);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(950, 600);
            Name = "frmChiTietPhieuPhat";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết vi phạm";
            pnlHeader.ResumeLayout(false);
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private Label lblSach;
        private ComboBox cboSach;
        private Label lblQuyDinh;
        private ComboBox cboQuyDinh;
        private Label lblSoNgayTre;
        private TextBox txtSoNgayTre;
        private Label lblDonGia;
        private TextBox txtDonGia;
        private Label lblTienPhat;
        private TextBox txtTienPhat;
        private Label lblGhiChu;
        private TextBox txtGhiChu;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private DataGridView dgvChiTiet;
        private DataGridViewTextBoxColumn colMaCTPP;
        private DataGridViewTextBoxColumn colMaSach;
        private DataGridViewTextBoxColumn colMaQD;
        private DataGridViewTextBoxColumn colDonGia;
        private DataGridViewTextBoxColumn colSoNgayTre;
        private DataGridViewTextBoxColumn colGhiChu;
        private DataGridViewTextBoxColumn colTienPhat;
        private Label lblTongTien;
        private Button btnDong;
    }
}
