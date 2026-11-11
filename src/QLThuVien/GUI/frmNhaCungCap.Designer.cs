namespace QLThuVien.GUI
{
    partial class frmNhaCungCap
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
            this.gbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMa = new System.Windows.Forms.Label();
            this.txtMaNCC = new System.Windows.Forms.TextBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTenNCC = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblTim = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.dgvNCC = new System.Windows.Forms.DataGridView();

            this.gbThongTin.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNCC)).BeginInit();
            this.SuspendLayout();

            // lblHeader
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblHeader.Height = 50;
            this.lblHeader.Text = "QUẢN LÝ DANH MỤC NHÀ CUNG CẤP";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // gbThongTin
            this.gbThongTin.Controls.Add(this.lblMa);
            this.gbThongTin.Controls.Add(this.txtMaNCC);
            this.gbThongTin.Controls.Add(this.lblTen);
            this.gbThongTin.Controls.Add(this.txtTenNCC);
            this.gbThongTin.Controls.Add(this.pnlButtons);
            this.gbThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbThongTin.Height = 135;
            this.gbThongTin.Padding = new System.Windows.Forms.Padding(15);
            this.gbThongTin.Text = "Thông tin Nhà cung cấp";

            // lblMa & txtMaNCC
            this.lblMa.AutoSize = true;
            this.lblMa.Location = new System.Drawing.Point(30, 35);
            this.lblMa.Text = "Mã NCC:";
            this.txtMaNCC.Location = new System.Drawing.Point(110, 32);
            this.txtMaNCC.Width = 200;

            // lblTen & txtTenNCC
            this.lblTen.AutoSize = true;
            this.lblTen.Location = new System.Drawing.Point(30, 80);
            this.lblTen.Text = "Tên NCC:";
            this.txtTenNCC.Location = new System.Drawing.Point(110, 77);
            this.txtTenNCC.Width = 400;

            // pnlButtons
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnLamMoi);
            this.pnlButtons.Location = new System.Drawing.Point(530, 25);
            this.pnlButtons.Size = new System.Drawing.Size(340, 95);

            // btnThem
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Margin = new System.Windows.Forms.Padding(5);
            this.btnThem.Size = new System.Drawing.Size(110, 36);
            this.btnThem.Text = "➕ Thêm";
            this.btnThem.Click += new System.EventHandler(this.BtnThem_Click);

            // btnSua
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Margin = new System.Windows.Forms.Padding(5);
            this.btnSua.Size = new System.Drawing.Size(110, 36);
            this.btnSua.Text = "✏️ Sửa";
            this.btnSua.Click += new System.EventHandler(this.BtnSua_Click);

            // btnXoa
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Margin = new System.Windows.Forms.Padding(5);
            this.btnXoa.Size = new System.Drawing.Size(110, 36);
            this.btnXoa.Text = "🗑️ Xóa";
            this.btnXoa.Click += new System.EventHandler(this.BtnXoa_Click);

            // btnLamMoi
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(5);
            this.btnLamMoi.Size = new System.Drawing.Size(110, 36);
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.BtnLamMoi_Click);

            // pnlSearch
            this.pnlSearch.Controls.Add(this.lblTim);
            this.pnlSearch.Controls.Add(this.txtTimKiem);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Height = 48;
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);

            // lblTim & txtTimKiem
            this.lblTim.AutoSize = true;
            this.lblTim.Location = new System.Drawing.Point(30, 14);
            this.lblTim.Text = "🔍 Tìm kiếm (theo Mã hoặc Tên):";
            this.txtTimKiem.Location = new System.Drawing.Point(250, 11);
            this.txtTimKiem.Width = 350;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.TxtTimKiem_TextChanged);

            // dgvNCC
            this.dgvNCC.AllowUserToAddRows = false;
            this.dgvNCC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNCC.BackgroundColor = System.Drawing.Color.White;
            this.dgvNCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNCC.GridColor = System.Drawing.Color.LightGray;
            this.dgvNCC.MultiSelect = false;
            this.dgvNCC.ReadOnly = true;
            this.dgvNCC.RowHeadersVisible = false;
            this.dgvNCC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNCC.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvNCC_CellClick);

            // frmNhaCungCap
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.dgvNCC);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.gbThongTin);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Nhà Cung Cấp";

            this.gbThongTin.ResumeLayout(false);
            this.gbThongTin.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNCC)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.GroupBox gbThongTin;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtMaNCC;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTenNCC;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblTim;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.DataGridView dgvNCC;
    }
}
