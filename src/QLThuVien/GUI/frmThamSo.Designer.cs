namespace QLThuVien.GUI
{
    partial class frmThamSo
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.gbDanhSach = new System.Windows.Forms.GroupBox();
            this.dgvThamSo = new System.Windows.Forms.DataGridView();
            this.gbChiTiet = new System.Windows.Forms.GroupBox();
            this.lblMaTS = new System.Windows.Forms.Label();
            this.txtMaTS = new System.Windows.Forms.TextBox();
            this.lblTenTS = new System.Windows.Forms.Label();
            this.txtTenTS = new System.Windows.Forms.TextBox();
            this.lblGiaTri = new System.Windows.Forms.Label();
            this.txtGiaTri = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.gbDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThamSo)).BeginInit();
            this.gbChiTiet.SuspendLayout();
            this.SuspendLayout();

            // lblHeader
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblHeader.Height = 50;
            this.lblHeader.Text = "QUẢN LÝ THAM SỐ CẤU HÌNH (YÊU CẦU 9)";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // scMain
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scMain.Location = new System.Drawing.Point(10, 60);
            this.scMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.scMain.Size = new System.Drawing.Size(860, 430);
            this.scMain.SplitterDistance = 530;

            // gbDanhSach
            this.gbDanhSach.Controls.Add(this.dgvThamSo);
            this.gbDanhSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDanhSach.Text = "Danh sách Tham số hệ thống";

            // dgvThamSo
            this.dgvThamSo.AllowUserToAddRows = false;
            this.dgvThamSo.AllowUserToDeleteRows = false;
            this.dgvThamSo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThamSo.BackgroundColor = System.Drawing.Color.White;
            this.dgvThamSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThamSo.MultiSelect = false;
            this.dgvThamSo.ReadOnly = true;
            this.dgvThamSo.RowHeadersVisible = false;
            this.dgvThamSo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThamSo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvThamSo_CellClick);

            // gbChiTiet
            this.gbChiTiet.Controls.Add(this.lblMaTS);
            this.gbChiTiet.Controls.Add(this.txtMaTS);
            this.gbChiTiet.Controls.Add(this.lblTenTS);
            this.gbChiTiet.Controls.Add(this.txtTenTS);
            this.gbChiTiet.Controls.Add(this.lblGiaTri);
            this.gbChiTiet.Controls.Add(this.txtGiaTri);
            this.gbChiTiet.Controls.Add(this.btnLuu);
            this.gbChiTiet.Controls.Add(this.btnLamMoi);
            this.gbChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbChiTiet.Text = "Cập nhật giá trị tham số";

            // lblMaTS
            this.lblMaTS.AutoSize = true;
            this.lblMaTS.Location = new System.Drawing.Point(15, 35);
            this.lblMaTS.Text = "Mã tham số:";

            // txtMaTS
            this.txtMaTS.Location = new System.Drawing.Point(15, 55);
            this.txtMaTS.ReadOnly = true;
            this.txtMaTS.Width = 280;

            // lblTenTS
            this.lblTenTS.AutoSize = true;
            this.lblTenTS.Location = new System.Drawing.Point(15, 95);
            this.lblTenTS.Text = "Tên tham số (Quy định):";

            // txtTenTS
            this.txtTenTS.Location = new System.Drawing.Point(15, 115);
            this.txtTenTS.ReadOnly = true;
            this.txtTenTS.Width = 280;

            // lblGiaTri
            this.lblGiaTri.AutoSize = true;
            this.lblGiaTri.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblGiaTri.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.lblGiaTri.Location = new System.Drawing.Point(15, 155);
            this.lblGiaTri.Text = "Giá trị thiết lập mới:";

            // txtGiaTri
            this.txtGiaTri.Location = new System.Drawing.Point(15, 175);
            this.txtGiaTri.Width = 280;

            // btnLuu
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.Location = new System.Drawing.Point(15, 220);
            this.btnLuu.Size = new System.Drawing.Size(130, 35);
            this.btnLuu.Text = "💾 Lưu giá trị";
            this.btnLuu.Click += new System.EventHandler(this.BtnLuu_Click);

            // btnLamMoi
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(155, 220);
            this.btnLamMoi.Size = new System.Drawing.Size(100, 35);
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.BtnLamMoi_Click);

            // frmThamSo
            this.ClientSize = new System.Drawing.Size(880, 500);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Tham Số Cấu Hình Hệ Thống";

            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.gbDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThamSo)).EndInit();
            this.gbChiTiet.ResumeLayout(false);
            this.gbChiTiet.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.GroupBox gbDanhSach;
        private System.Windows.Forms.DataGridView dgvThamSo;
        private System.Windows.Forms.GroupBox gbChiTiet;
        private System.Windows.Forms.Label lblMaTS;
        private System.Windows.Forms.TextBox txtMaTS;
        private System.Windows.Forms.Label lblTenTS;
        private System.Windows.Forms.TextBox txtTenTS;
        private System.Windows.Forms.Label lblGiaTri;
        private System.Windows.Forms.TextBox txtGiaTri;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
