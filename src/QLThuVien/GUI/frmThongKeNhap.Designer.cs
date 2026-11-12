namespace QLThuVien.GUI
{
    partial class frmThongKeNhap
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTieuDe     = new System.Windows.Forms.Label();
            this.lblNam        = new System.Windows.Forms.Label();
            this.cboNam        = new System.Windows.Forms.ComboBox();
            this.btnThongKe    = new System.Windows.Forms.Button();
            this.btnXuatPDF    = new System.Windows.Forms.Button();
            this.dgvThongKe    = new System.Windows.Forms.DataGridView();
            this.pnlTop        = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();

            // lblTieuDe
            this.lblTieuDe.AutoSize  = false;
            this.lblTieuDe.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTieuDe.Font      = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTieuDe.Height    = 40;
            this.lblTieuDe.Text      = "THỐNG KÊ NHẬP SÁCH THEO QUÝ";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // pnlTop
            this.pnlTop.Dock     = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height   = 50;
            this.pnlTop.Padding  = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.pnlTop.Controls.Add(this.btnXuatPDF);
            this.pnlTop.Controls.Add(this.btnThongKe);
            this.pnlTop.Controls.Add(this.cboNam);
            this.pnlTop.Controls.Add(this.lblNam);

            // lblNam
            this.lblNam.AutoSize  = true;
            this.lblNam.Location  = new System.Drawing.Point(8, 14);
            this.lblNam.Text      = "Năm thống kê:";

            // cboNam
            this.cboNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNam.Location      = new System.Drawing.Point(110, 10);
            this.cboNam.Size          = new System.Drawing.Size(90, 24);
            this.cboNam.TabIndex      = 0;

            // btnThongKe
            this.btnThongKe.Location  = new System.Drawing.Point(216, 9);
            this.btnThongKe.Size      = new System.Drawing.Size(110, 28);
            this.btnThongKe.Text      = "🔍 Thống kê";
            this.btnThongKe.TabIndex  = 1;
            this.btnThongKe.Click    += new System.EventHandler(this.btnThongKe_Click);

            // btnXuatPDF
            this.btnXuatPDF.Location  = new System.Drawing.Point(338, 9);
            this.btnXuatPDF.Size      = new System.Drawing.Size(130, 28);
            this.btnXuatPDF.Text      = "🖨️ Xuất PDF báo cáo";
            this.btnXuatPDF.TabIndex  = 2;
            this.btnXuatPDF.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnXuatPDF.ForeColor = System.Drawing.Color.White;
            this.btnXuatPDF.Click    += new System.EventHandler(this.btnXuatPDF_Click);

            // dgvThongKe
            this.dgvThongKe.AllowUserToAddRows    = false;
            this.dgvThongKe.AllowUserToDeleteRows = false;
            this.dgvThongKe.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThongKe.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.dgvThongKe.ReadOnly              = true;
            this.dgvThongKe.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongKe.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);

            // frmThongKeNhap
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(880, 520);
            this.Controls.Add(this.dgvThongKe);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lblTieuDe);
            this.MinimumSize = new System.Drawing.Size(800, 480);
            this.Name        = "frmThongKeNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text        = "Thống kê nhập sách theo Quý";
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label      lblTieuDe;
        private System.Windows.Forms.Label      lblNam;
        private System.Windows.Forms.ComboBox   cboNam;
        private System.Windows.Forms.Button     btnThongKe;
        private System.Windows.Forms.Button     btnXuatPDF;
        private System.Windows.Forms.DataGridView dgvThongKe;
        private System.Windows.Forms.Panel      pnlTop;
    }
}
