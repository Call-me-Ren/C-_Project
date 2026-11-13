namespace QLThuVien.GUI
{
    partial class frmTimKiemPopup
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
            this.lblMaDauSach = new System.Windows.Forms.Label();
            this.txtMaDauSach = new System.Windows.Forms.TextBox();
            this.lblTenSach = new System.Windows.Forms.Label();
            this.txtTenSach = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblNamXB = new System.Windows.Forms.Label();
            this.txtNamXB = new System.Windows.Forms.TextBox();
            this.lblMaTG = new System.Windows.Forms.Label();
            this.txtMaTG = new System.Windows.Forms.TextBox();
            this.lblMaTL = new System.Windows.Forms.Label();
            this.txtMaTL = new System.Windows.Forms.TextBox();
            this.lblTuGia = new System.Windows.Forms.Label();
            this.txtTuGia = new System.Windows.Forms.TextBox();
            this.lblDenGia = new System.Windows.Forms.Label();
            this.txtDenGia = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblMaDauSach
            this.lblMaDauSach.AutoSize = true;
            this.lblMaDauSach.Location = new System.Drawing.Point(20, 20);
            this.lblMaDauSach.Text = "Mã đầu sách:";

            // txtMaDauSach
            this.txtMaDauSach.Location = new System.Drawing.Point(130, 17);
            this.txtMaDauSach.Size = new System.Drawing.Size(200, 25);

            // lblTenSach
            this.lblTenSach.AutoSize = true;
            this.lblTenSach.Location = new System.Drawing.Point(20, 55);
            this.lblTenSach.Text = "Tên sách:";

            // txtTenSach
            this.txtTenSach.Location = new System.Drawing.Point(130, 52);
            this.txtTenSach.Size = new System.Drawing.Size(200, 25);

            // lblSoLuong
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(20, 90);
            this.lblSoLuong.Text = "Số lượng:";

            // txtSoLuong
            this.txtSoLuong.Location = new System.Drawing.Point(130, 87);
            this.txtSoLuong.Size = new System.Drawing.Size(200, 25);

            // lblNamXB
            this.lblNamXB.AutoSize = true;
            this.lblNamXB.Location = new System.Drawing.Point(20, 125);
            this.lblNamXB.Text = "Năm XB:";

            // txtNamXB
            this.txtNamXB.Location = new System.Drawing.Point(130, 122);
            this.txtNamXB.Size = new System.Drawing.Size(200, 25);

            // lblMaTG
            this.lblMaTG.AutoSize = true;
            this.lblMaTG.Location = new System.Drawing.Point(20, 160);
            this.lblMaTG.Text = "Mã Tác giả:";

            // txtMaTG
            this.txtMaTG.Location = new System.Drawing.Point(130, 157);
            this.txtMaTG.Size = new System.Drawing.Size(200, 25);

            // lblMaTL
            this.lblMaTL.AutoSize = true;
            this.lblMaTL.Location = new System.Drawing.Point(20, 195);
            this.lblMaTL.Text = "Mã Thể loại:";

            // txtMaTL
            this.txtMaTL.Location = new System.Drawing.Point(130, 192);
            this.txtMaTL.Size = new System.Drawing.Size(200, 25);

            // lblTuGia
            this.lblTuGia.AutoSize = true;
            this.lblTuGia.Location = new System.Drawing.Point(20, 230);
            this.lblTuGia.Text = "Đơn giá từ:";

            // txtTuGia
            this.txtTuGia.Location = new System.Drawing.Point(130, 227);
            this.txtTuGia.Size = new System.Drawing.Size(200, 25);

            // lblDenGia
            this.lblDenGia.AutoSize = true;
            this.lblDenGia.Location = new System.Drawing.Point(20, 265);
            this.lblDenGia.Text = "Đến đơn giá:";

            // txtDenGia
            this.txtDenGia.Location = new System.Drawing.Point(130, 262);
            this.txtDenGia.Size = new System.Drawing.Size(200, 25);

            // btnTim
            this.btnTim.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnTim.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(130, 305);
            this.btnTim.Size = new System.Drawing.Size(95, 32);
            this.btnTim.Text = "🔍 Tìm kiếm";

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(235, 305);
            this.btnHuy.Size = new System.Drawing.Size(95, 32);
            this.btnHuy.Text = "✖ Hủy";

            // frmTimKiemPopup
            this.AcceptButton = this.btnTim;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(360, 360);
            this.Controls.Add(this.lblMaDauSach);
            this.Controls.Add(this.txtMaDauSach);
            this.Controls.Add(this.lblTenSach);
            this.Controls.Add(this.txtTenSach);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.lblNamXB);
            this.Controls.Add(this.txtNamXB);
            this.Controls.Add(this.lblMaTG);
            this.Controls.Add(this.txtMaTG);
            this.Controls.Add(this.lblMaTL);
            this.Controls.Add(this.txtMaTL);
            this.Controls.Add(this.lblTuGia);
            this.Controls.Add(this.txtTuGia);
            this.Controls.Add(this.lblDenGia);
            this.Controls.Add(this.txtDenGia);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.btnHuy);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tìm kiếm nâng cao";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblMaDauSach;
        private System.Windows.Forms.TextBox txtMaDauSach;
        private System.Windows.Forms.Label lblTenSach;
        private System.Windows.Forms.TextBox txtTenSach;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblNamXB;
        private System.Windows.Forms.TextBox txtNamXB;
        private System.Windows.Forms.Label lblMaTG;
        private System.Windows.Forms.TextBox txtMaTG;
        private System.Windows.Forms.Label lblMaTL;
        private System.Windows.Forms.TextBox txtMaTL;
        private System.Windows.Forms.Label lblTuGia;
        private System.Windows.Forms.TextBox txtTuGia;
        private System.Windows.Forms.Label lblDenGia;
        private System.Windows.Forms.TextBox txtDenGia;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnHuy;
    }
}
