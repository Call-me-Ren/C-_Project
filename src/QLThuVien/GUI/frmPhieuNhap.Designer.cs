namespace QLThuVien.GUI
{
    partial class frmPhieuNhap
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

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.gbPhieuNhap = new System.Windows.Forms.GroupBox();
            this.dgvPhieuNhap = new System.Windows.Forms.DataGridView();
            this.pnlSearchPN = new System.Windows.Forms.Panel();
            this.lblTimPN = new System.Windows.Forms.Label();
            this.txtTimKiemPN = new System.Windows.Forms.TextBox();
            this.gbLapPhieu = new System.Windows.Forms.GroupBox();
            this.pnlTopInputs = new System.Windows.Forms.Panel();
            this.lblMaPN = new System.Windows.Forms.Label();
            this.txtMaPN = new System.Windows.Forms.TextBox();
            this.lblNCC = new System.Windows.Forms.Label();
            this.cboNCC = new System.Windows.Forms.ComboBox();
            this.lblNV = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.pnlBookInputs = new System.Windows.Forms.Panel();
            this.lblDauSach = new System.Windows.Forms.Label();
            this.cboDauSach = new System.Windows.Forms.ComboBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.lblThanhTien = new System.Windows.Forms.Label();
            this.txtThanhTien = new System.Windows.Forms.TextBox();
            this.btnThemSach = new System.Windows.Forms.Button();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblTongTienText = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.btnLuuPhieu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.gbPhieuNhap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).BeginInit();
            this.pnlSearchPN.SuspendLayout();
            this.gbLapPhieu.SuspendLayout();
            this.pnlTopInputs.SuspendLayout();
            this.pnlBookInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();

            // lblHeader
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblHeader.Height = 45;
            this.lblHeader.Text = "QUẢN LÝ NHẬP SÁCH (FORM MAIN - SUB 1-N)";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // scMain
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.scMain.SplitterDistance = 240;

            // --- PANEL 1: MASTER (DANH SÁCH PHIẾU NHẬP) ---
            this.scMain.Panel1.Controls.Add(this.gbPhieuNhap);

            // gbPhieuNhap
            this.gbPhieuNhap.Controls.Add(this.dgvPhieuNhap);
            this.gbPhieuNhap.Controls.Add(this.pnlSearchPN);
            this.gbPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPhieuNhap.Text = "1. Danh sách Phiếu nhập (Master)";

            // pnlSearchPN
            this.pnlSearchPN.Controls.Add(this.lblTimPN);
            this.pnlSearchPN.Controls.Add(this.txtTimKiemPN);
            this.pnlSearchPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchPN.Height = 40;

            this.lblTimPN.AutoSize = true;
            this.lblTimPN.Location = new System.Drawing.Point(15, 10);
            this.lblTimPN.Text = "🔍 Tìm kiếm phiếu:";
            this.txtTimKiemPN.Location = new System.Drawing.Point(140, 7);
            this.txtTimKiemPN.Width = 300;
            this.txtTimKiemPN.TextChanged += new System.EventHandler(this.TxtTimKiemPN_TextChanged);

            // dgvPhieuNhap
            this.dgvPhieuNhap.AllowUserToAddRows = false;
            this.dgvPhieuNhap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieuNhap.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhieuNhap.MultiSelect = false;
            this.dgvPhieuNhap.ReadOnly = true;
            this.dgvPhieuNhap.RowHeadersVisible = false;
            this.dgvPhieuNhap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhieuNhap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPhieuNhap_CellClick);

            // --- PANEL 2: DETAIL (CHI TIẾT PHIẾU NHẬP & LẬP PHIẾU MỚI) ---
            this.scMain.Panel2.Controls.Add(this.gbLapPhieu);

            // gbLapPhieu
            this.gbLapPhieu.Controls.Add(this.dgvChiTiet);
            this.gbLapPhieu.Controls.Add(this.pnlBottom);
            this.gbLapPhieu.Controls.Add(this.pnlBookInputs);
            this.gbLapPhieu.Controls.Add(this.pnlTopInputs);
            this.gbLapPhieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLapPhieu.Text = "2. Chi tiết Sách nhập (Detail) & Lập phiếu mới";

            // pnlTopInputs
            this.pnlTopInputs.Controls.Add(this.lblMaPN);
            this.pnlTopInputs.Controls.Add(this.txtMaPN);
            this.pnlTopInputs.Controls.Add(this.lblNCC);
            this.pnlTopInputs.Controls.Add(this.cboNCC);
            this.pnlTopInputs.Controls.Add(this.lblNV);
            this.pnlTopInputs.Controls.Add(this.txtMaNV);
            this.pnlTopInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopInputs.Height = 45;

            this.lblMaPN.AutoSize = true;
            this.lblMaPN.Location = new System.Drawing.Point(15, 12);
            this.lblMaPN.Text = "Mã PN:";
            this.txtMaPN.Location = new System.Drawing.Point(70, 9);
            this.txtMaPN.Width = 110;

            this.lblNCC.AutoSize = true;
            this.lblNCC.Location = new System.Drawing.Point(200, 12);
            this.lblNCC.Text = "Nhà CC:";
            this.cboNCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNCC.Location = new System.Drawing.Point(265, 9);
            this.cboNCC.Width = 220;

            this.lblNV.AutoSize = true;
            this.lblNV.Location = new System.Drawing.Point(505, 12);
            this.lblNV.Text = "Mã NV:";
            this.txtMaNV.Location = new System.Drawing.Point(565, 9);
            this.txtMaNV.Width = 100;
            this.txtMaNV.Text = "NV01";

            // pnlBookInputs
            this.pnlBookInputs.Controls.Add(this.lblDauSach);
            this.pnlBookInputs.Controls.Add(this.cboDauSach);
            this.pnlBookInputs.Controls.Add(this.lblDonGia);
            this.pnlBookInputs.Controls.Add(this.txtDonGia);
            this.pnlBookInputs.Controls.Add(this.lblSoLuong);
            this.pnlBookInputs.Controls.Add(this.numSoLuong);
            this.pnlBookInputs.Controls.Add(this.lblThanhTien);
            this.pnlBookInputs.Controls.Add(this.txtThanhTien);
            this.pnlBookInputs.Controls.Add(this.btnThemSach);
            this.pnlBookInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBookInputs.Height = 45;

            this.lblDauSach.AutoSize = true;
            this.lblDauSach.Location = new System.Drawing.Point(15, 12);
            this.lblDauSach.Text = "Đầu sách:";
            this.cboDauSach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDauSach.Location = new System.Drawing.Point(85, 9);
            this.cboDauSach.Width = 220;
            this.cboDauSach.SelectedIndexChanged += new System.EventHandler(this.CboDauSach_SelectedIndexChanged);

            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Location = new System.Drawing.Point(320, 12);
            this.lblDonGia.Text = "Đơn giá:";
            this.txtDonGia.Location = new System.Drawing.Point(380, 9);
            this.txtDonGia.ReadOnly = true;
            this.txtDonGia.Width = 90;

            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(485, 12);
            this.lblSoLuong.Text = "SL:";
            this.numSoLuong.Location = new System.Drawing.Point(515, 10);
            this.numSoLuong.Minimum = 1;
            this.numSoLuong.Maximum = 1000;
            this.numSoLuong.Value = 1;
            this.numSoLuong.Width = 60;
            this.numSoLuong.ValueChanged += new System.EventHandler(this.NumSoLuong_ValueChanged);

            this.lblThanhTien.AutoSize = true;
            this.lblThanhTien.Location = new System.Drawing.Point(590, 12);
            this.lblThanhTien.Text = "Thành tiền:";
            this.txtThanhTien.Location = new System.Drawing.Point(670, 9);
            this.txtThanhTien.ReadOnly = true;
            this.txtThanhTien.Width = 110;

            this.btnThemSach.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnThemSach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemSach.ForeColor = System.Drawing.Color.White;
            this.btnThemSach.Location = new System.Drawing.Point(795, 7);
            this.btnThemSach.Size = new System.Drawing.Size(100, 30);
            this.btnThemSach.Text = "➕ Thêm dòng";
            this.btnThemSach.Click += new System.EventHandler(this.BtnThemSach_Click);

            // dgvChiTiet
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.MultiSelect = false;
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // pnlBottom
            this.pnlBottom.Controls.Add(this.lblTongTienText);
            this.pnlBottom.Controls.Add(this.lblTongTien);
            this.pnlBottom.Controls.Add(this.btnLuuPhieu);
            this.pnlBottom.Controls.Add(this.btnHuy);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Height = 50;

            this.lblTongTienText.AutoSize = true;
            this.lblTongTienText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTienText.Location = new System.Drawing.Point(20, 15);
            this.lblTongTienText.Text = "TỔNG TIỀN PHIẾU NHẬP:";

            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.lblTongTien.Location = new System.Drawing.Point(220, 12);
            this.lblTongTien.Text = "0 VNĐ";

            this.btnLuuPhieu.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnLuuPhieu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuuPhieu.ForeColor = System.Drawing.Color.White;
            this.btnLuuPhieu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLuuPhieu.Location = new System.Drawing.Point(620, 10);
            this.btnLuuPhieu.Size = new System.Drawing.Size(140, 32);
            this.btnLuuPhieu.Text = "💾 LƯU PHIẾU";
            this.btnLuuPhieu.Click += new System.EventHandler(this.BtnLuuPhieu_Click);

            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(770, 10);
            this.btnHuy.Size = new System.Drawing.Size(90, 32);
            this.btnHuy.Text = "🔄 Làm mới";
            this.btnHuy.Click += new System.EventHandler(this.BtnHuy_Click);

            // frmPhieuNhap
            this.ClientSize = new System.Drawing.Size(960, 680);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Nhập Sách (Form Main - Sub)";

            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.gbPhieuNhap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).EndInit();
            this.pnlSearchPN.ResumeLayout(false);
            this.pnlSearchPN.PerformLayout();
            this.gbLapPhieu.ResumeLayout(false);
            this.pnlTopInputs.ResumeLayout(false);
            this.pnlTopInputs.PerformLayout();
            this.pnlBookInputs.ResumeLayout(false);
            this.pnlBookInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.GroupBox gbPhieuNhap;
        private System.Windows.Forms.DataGridView dgvPhieuNhap;
        private System.Windows.Forms.Panel pnlSearchPN;
        private System.Windows.Forms.Label lblTimPN;
        private System.Windows.Forms.TextBox txtTimKiemPN;
        private System.Windows.Forms.GroupBox gbLapPhieu;
        private System.Windows.Forms.Panel pnlTopInputs;
        private System.Windows.Forms.Label lblMaPN;
        private System.Windows.Forms.TextBox txtMaPN;
        private System.Windows.Forms.Label lblNCC;
        private System.Windows.Forms.ComboBox cboNCC;
        private System.Windows.Forms.Label lblNV;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Panel pnlBookInputs;
        private System.Windows.Forms.Label lblDauSach;
        private System.Windows.Forms.ComboBox cboDauSach;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.Label lblThanhTien;
        private System.Windows.Forms.TextBox txtThanhTien;
        private System.Windows.Forms.Button btnThemSach;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblTongTienText;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Button btnLuuPhieu;
        private System.Windows.Forms.Button btnHuy;
    }
}
