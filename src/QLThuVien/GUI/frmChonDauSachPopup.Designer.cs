namespace QLThuVien.GUI
{
    partial class frmChonDauSachPopup
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.dgvDauSach = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnChon = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDauSach)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Controls.Add(this.lblTimKiem);
            this.pnlTop.Controls.Add(this.txtTimKiem);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 50;

            // lblTimKiem
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Location = new System.Drawing.Point(15, 16);
            this.lblTimKiem.Text = "🔍 Tìm đầu sách (mã hoặc tên):";

            // txtTimKiem
            this.txtTimKiem.Location = new System.Drawing.Point(210, 13);
            this.txtTimKiem.Size = new System.Drawing.Size(320, 25);
            this.txtTimKiem.TextChanged += new System.EventHandler(this.TxtTimKiem_TextChanged);

            // dgvDauSach
            this.dgvDauSach.AllowUserToAddRows = false;
            this.dgvDauSach.AllowUserToDeleteRows = false;
            this.dgvDauSach.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDauSach.BackgroundColor = System.Drawing.Color.White;
            this.dgvDauSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDauSach.MultiSelect = false;
            this.dgvDauSach.ReadOnly = true;
            this.dgvDauSach.RowHeadersVisible = false;
            this.dgvDauSach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDauSach.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvDauSach_CellDoubleClick);

            // pnlBottom
            this.pnlBottom.Controls.Add(this.btnChon);
            this.pnlBottom.Controls.Add(this.btnHuy);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Height = 50;

            // btnChon
            this.btnChon.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnChon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnChon.ForeColor = System.Drawing.Color.White;
            this.btnChon.Location = new System.Drawing.Point(490, 10);
            this.btnChon.Size = new System.Drawing.Size(100, 32);
            this.btnChon.Text = "✔ Chọn";
            this.btnChon.Click += new System.EventHandler(this.BtnChon_Click);

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(600, 10);
            this.btnHuy.Size = new System.Drawing.Size(90, 32);
            this.btnHuy.Text = "✖ Hủy";
            this.btnHuy.Click += new System.EventHandler(this.BtnHuy_Click);

            // frmChonDauSachPopup
            this.AcceptButton = this.btnChon;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(710, 420);
            this.Controls.Add(this.dgvDauSach);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tra cứu và Chọn Đầu sách";
            this.Load += new System.EventHandler(this.FrmChonDauSachPopup_Load);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDauSach)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.DataGridView dgvDauSach;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnChon;
        private System.Windows.Forms.Button btnHuy;
    }
}
