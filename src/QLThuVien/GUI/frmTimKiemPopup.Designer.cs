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
            this.lblPhepLogic = new System.Windows.Forms.Label();
            this.cboPhepLogic = new System.Windows.Forms.ComboBox();
            this.lblMaDauSach = new System.Windows.Forms.Label();
            this.txtMaDauSach = new System.Windows.Forms.TextBox();
            this.lblTenSach = new System.Windows.Forms.Label();
            this.txtTenSach = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.cboToanTuSoLuong = new System.Windows.Forms.ComboBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblNamXB = new System.Windows.Forms.Label();
            this.cboToanTuNamXB = new System.Windows.Forms.ComboBox();
            this.txtNamXB = new System.Windows.Forms.TextBox();
            this.lblMaTG = new System.Windows.Forms.Label();
            this.txtMaTG = new System.Windows.Forms.TextBox();
            this.lblMaTL = new System.Windows.Forms.Label();
            this.txtMaTL = new System.Windows.Forms.TextBox();
            this.lblTuGia = new System.Windows.Forms.Label();
            this.cboToanTuGia = new System.Windows.Forms.ComboBox();
            this.txtTuGia = new System.Windows.Forms.TextBox();
            this.lblDenGia = new System.Windows.Forms.Label();
            this.txtDenGia = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblPhepLogic
            this.lblPhepLogic.AutoSize = true;
            this.lblPhepLogic.Location = new System.Drawing.Point(20, 20);
            this.lblPhepLogic.Text = "Phép toán logic:";

            // cboPhepLogic
            this.cboPhepLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhepLogic.Items.AddRange(new object[] {
                "AND (Thỏa tất cả điều kiện)",
                "OR (Thỏa 1 trong các điều kiện)",
                "NOT (Phủ định các điều kiện)"
            });
            this.cboPhepLogic.Location = new System.Drawing.Point(130, 17);
            this.cboPhepLogic.Size = new System.Drawing.Size(220, 25);
            this.cboPhepLogic.SelectedIndex = 0;

            // lblMaDauSach
            this.lblMaDauSach.AutoSize = true;
            this.lblMaDauSach.Location = new System.Drawing.Point(20, 55);
            this.lblMaDauSach.Text = "Mã đầu sách:";

            // txtMaDauSach
            this.txtMaDauSach.Location = new System.Drawing.Point(130, 52);
            this.txtMaDauSach.Size = new System.Drawing.Size(220, 25);

            // lblTenSach
            this.lblTenSach.AutoSize = true;
            this.lblTenSach.Location = new System.Drawing.Point(20, 90);
            this.lblTenSach.Text = "Tên sách:";

            // txtTenSach
            this.txtTenSach.Location = new System.Drawing.Point(130, 87);
            this.txtTenSach.Size = new System.Drawing.Size(220, 25);

            // lblSoLuong
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(20, 125);
            this.lblSoLuong.Text = "Số lượng:";

            // cboToanTuSoLuong
            this.cboToanTuSoLuong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToanTuSoLuong.Items.AddRange(new object[] { "=", "<>", ">", ">=", "<", "<=" });
            this.cboToanTuSoLuong.Location = new System.Drawing.Point(130, 122);
            this.cboToanTuSoLuong.Size = new System.Drawing.Size(60, 25);
            this.cboToanTuSoLuong.SelectedIndex = 0;

            // txtSoLuong
            this.txtSoLuong.Location = new System.Drawing.Point(196, 122);
            this.txtSoLuong.Size = new System.Drawing.Size(154, 25);

            // lblNamXB
            this.lblNamXB.AutoSize = true;
            this.lblNamXB.Location = new System.Drawing.Point(20, 160);
            this.lblNamXB.Text = "Năm XB:";

            // cboToanTuNamXB
            this.cboToanTuNamXB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToanTuNamXB.Items.AddRange(new object[] { "=", "<>", ">", ">=", "<", "<=" });
            this.cboToanTuNamXB.Location = new System.Drawing.Point(130, 157);
            this.cboToanTuNamXB.Size = new System.Drawing.Size(60, 25);
            this.cboToanTuNamXB.SelectedIndex = 0;

            // txtNamXB
            this.txtNamXB.Location = new System.Drawing.Point(196, 157);
            this.txtNamXB.Size = new System.Drawing.Size(154, 25);

            // lblMaTG
            this.lblMaTG.AutoSize = true;
            this.lblMaTG.Location = new System.Drawing.Point(20, 195);
            this.lblMaTG.Text = "Mã Tác giả:";

            // txtMaTG
            this.txtMaTG.Location = new System.Drawing.Point(130, 192);
            this.txtMaTG.Size = new System.Drawing.Size(220, 25);

            // lblMaTL
            this.lblMaTL.AutoSize = true;
            this.lblMaTL.Location = new System.Drawing.Point(20, 230);
            this.lblMaTL.Text = "Mã Thể loại:";

            // txtMaTL
            this.txtMaTL.Location = new System.Drawing.Point(130, 227);
            this.txtMaTL.Size = new System.Drawing.Size(220, 25);

            // lblTuGia
            this.lblTuGia.AutoSize = true;
            this.lblTuGia.Location = new System.Drawing.Point(20, 265);
            this.lblTuGia.Text = "Đơn giá / Từ:";

            // cboToanTuGia
            this.cboToanTuGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToanTuGia.Items.AddRange(new object[] { ">=", "=", "<>", ">", "<", "<=" });
            this.cboToanTuGia.Location = new System.Drawing.Point(130, 262);
            this.cboToanTuGia.Size = new System.Drawing.Size(60, 25);
            this.cboToanTuGia.SelectedIndex = 0;

            // txtTuGia
            this.txtTuGia.Location = new System.Drawing.Point(196, 262);
            this.txtTuGia.Size = new System.Drawing.Size(154, 25);

            // lblDenGia
            this.lblDenGia.AutoSize = true;
            this.lblDenGia.Location = new System.Drawing.Point(20, 300);
            this.lblDenGia.Text = "Đến đơn giá (<=):";

            // txtDenGia
            this.txtDenGia.Location = new System.Drawing.Point(130, 297);
            this.txtDenGia.Size = new System.Drawing.Size(220, 25);

            // btnTim
            this.btnTim.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnTim.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(130, 340);
            this.btnTim.Size = new System.Drawing.Size(105, 32);
            this.btnTim.Text = "🔍 Tìm kiếm";

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(245, 340);
            this.btnHuy.Size = new System.Drawing.Size(105, 32);
            this.btnHuy.Text = "✖ Hủy";

            // frmTimKiemPopup
            this.AcceptButton = this.btnTim;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(380, 395);
            this.Controls.Add(this.lblPhepLogic);
            this.Controls.Add(this.cboPhepLogic);
            this.Controls.Add(this.lblMaDauSach);
            this.Controls.Add(this.txtMaDauSach);
            this.Controls.Add(this.lblTenSach);
            this.Controls.Add(this.txtTenSach);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.cboToanTuSoLuong);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.lblNamXB);
            this.Controls.Add(this.cboToanTuNamXB);
            this.Controls.Add(this.txtNamXB);
            this.Controls.Add(this.lblMaTG);
            this.Controls.Add(this.txtMaTG);
            this.Controls.Add(this.lblMaTL);
            this.Controls.Add(this.txtMaTL);
            this.Controls.Add(this.lblTuGia);
            this.Controls.Add(this.cboToanTuGia);
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

        private System.Windows.Forms.Label lblPhepLogic;
        private System.Windows.Forms.ComboBox cboPhepLogic;
        private System.Windows.Forms.Label lblMaDauSach;
        private System.Windows.Forms.TextBox txtMaDauSach;
        private System.Windows.Forms.Label lblTenSach;
        private System.Windows.Forms.TextBox txtTenSach;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.ComboBox cboToanTuSoLuong;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblNamXB;
        private System.Windows.Forms.ComboBox cboToanTuNamXB;
        private System.Windows.Forms.TextBox txtNamXB;
        private System.Windows.Forms.Label lblMaTG;
        private System.Windows.Forms.TextBox txtMaTG;
        private System.Windows.Forms.Label lblMaTL;
        private System.Windows.Forms.TextBox txtMaTL;
        private System.Windows.Forms.Label lblTuGia;
        private System.Windows.Forms.ComboBox cboToanTuGia;
        private System.Windows.Forms.TextBox txtTuGia;
        private System.Windows.Forms.Label lblDenGia;
        private System.Windows.Forms.TextBox txtDenGia;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnHuy;
    }
}
