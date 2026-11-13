using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    partial class frmTimKiemDocGiaPopup
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlSearch;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private DataGridView dgvDocGia;
        private DataGridViewTextBoxColumn colMaDG;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colTenLDG;
        private DataGridViewTextBoxColumn colSDT;
        private Panel pnlFooter;
        private Label lblThongKe;
        private Button btnChon;
        private Button btnHuy;

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
            pnlSearch = new Panel();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            dgvDocGia = new DataGridView();
            colMaDG = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colTenLDG = new DataGridViewTextBoxColumn();
            colSDT = new DataGridViewTextBoxColumn();
            pnlFooter = new Panel();
            lblThongKe = new Label();
            btnChon = new Button();
            btnHuy = new Button();
            pnlHeader.SuspendLayout();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocGia).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(620, 45);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(620, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "TÌM KIẾM ĐỘC GIẢ (POPUP BIẾN STATIC)";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(241, 245, 249);
            pnlSearch.Controls.Add(btnTimKiem);
            pnlSearch.Controls.Add(txtTimKiem);
            pnlSearch.Controls.Add(lblTimKiem);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 45);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(620, 45);
            pnlSearch.TabIndex = 1;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTimKiem.Location = new Point(12, 13);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(118, 17);
            lblTimKiem.TabIndex = 0;
            lblTimKiem.Text = "Từ khóa tìm kiếm:";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 9.5F);
            txtTimKiem.Location = new Point(136, 10);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập mã, họ tên hoặc SĐT độc giả...";
            txtTimKiem.Size = new Size(380, 24);
            txtTimKiem.TabIndex = 1;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(524, 8);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(84, 28);
            btnTimKiem.TabIndex = 2;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // dgvDocGia
            // 
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.BackgroundColor = Color.White;
            dgvDocGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDocGia.Columns.AddRange(new DataGridViewColumn[] { colMaDG, colHoTen, colTenLDG, colSDT });
            dgvDocGia.Dock = DockStyle.Fill;
            dgvDocGia.Location = new Point(0, 90);
            dgvDocGia.MultiSelect = false;
            dgvDocGia.Name = "dgvDocGia";
            dgvDocGia.ReadOnly = true;
            dgvDocGia.RowHeadersVisible = false;
            dgvDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocGia.Size = new Size(620, 260);
            dgvDocGia.TabIndex = 2;
            dgvDocGia.CellDoubleClick += dgvDocGia_CellDoubleClick;
            // 
            // colMaDG
            // 
            colMaDG.DataPropertyName = "MaDG";
            colMaDG.HeaderText = "Mã độc giả";
            colMaDG.Name = "colMaDG";
            colMaDG.ReadOnly = true;
            colMaDG.Width = 110;
            // 
            // colHoTen
            // 
            colHoTen.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.HeaderText = "Họ và tên";
            colHoTen.Name = "colHoTen";
            colHoTen.ReadOnly = true;
            // 
            // colTenLDG
            // 
            colTenLDG.DataPropertyName = "TenLDG";
            colTenLDG.HeaderText = "Loại độc giả";
            colTenLDG.Name = "colTenLDG";
            colTenLDG.ReadOnly = true;
            colTenLDG.Width = 140;
            // 
            // colSDT
            // 
            colSDT.DataPropertyName = "SDT";
            colSDT.HeaderText = "Số điện thoại";
            colSDT.Name = "colSDT";
            colSDT.ReadOnly = true;
            colSDT.Width = 120;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(241, 245, 249);
            pnlFooter.Controls.Add(btnHuy);
            pnlFooter.Controls.Add(btnChon);
            pnlFooter.Controls.Add(lblThongKe);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 350);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(620, 48);
            pnlFooter.TabIndex = 3;
            // 
            // lblThongKe
            // 
            lblThongKe.AutoSize = true;
            lblThongKe.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblThongKe.ForeColor = Color.DimGray;
            lblThongKe.Location = new Point(12, 16);
            lblThongKe.Name = "lblThongKe";
            lblThongKe.Size = new Size(54, 15);
            lblThongKe.TabIndex = 0;
            lblThongKe.Text = "Tổng số: ";
            // 
            // btnChon
            // 
            btnChon.BackColor = Color.FromArgb(39, 174, 96);
            btnChon.FlatStyle = FlatStyle.Flat;
            btnChon.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnChon.ForeColor = Color.White;
            btnChon.Location = new Point(395, 8);
            btnChon.Name = "btnChon";
            btnChon.Size = new Size(110, 32);
            btnChon.TabIndex = 1;
            btnChon.Text = "✔ Chọn độc giả";
            btnChon.UseVisualStyleBackColor = false;
            btnChon.Click += btnChon_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(149, 165, 166);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 9.5F);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(515, 8);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(93, 32);
            btnHuy.TabIndex = 2;
            btnHuy.Text = "Hủy bỏ";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // frmTimKiemDocGiaPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(620, 398);
            Controls.Add(dgvDocGia);
            Controls.Add(pnlFooter);
            Controls.Add(pnlSearch);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmTimKiemDocGiaPopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tìm kiếm độc giả";
            Load += frmTimKiemDocGiaPopup_Load;
            pnlHeader.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocGia).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
