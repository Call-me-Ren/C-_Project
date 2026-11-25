#nullable disable
using System;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    public partial class frmTimKiemPopup : Form
    {
        public string MaDauSach => string.IsNullOrWhiteSpace(txtMaDauSach.Text) ? null : txtMaDauSach.Text.Trim();
        public string TenSach => string.IsNullOrWhiteSpace(txtTenSach.Text) ? null : txtTenSach.Text.Trim();
        public int? SoLuong => int.TryParse(txtSoLuong.Text.Trim(), out int sl) ? sl : null;
        public string ToanTuSoLuong => cboToanTuSoLuong?.SelectedItem?.ToString() ?? "=";
        public int? NamXB => int.TryParse(txtNamXB.Text.Trim(), out int nxb) ? nxb : null;
        public string ToanTuNamXB => cboToanTuNamXB?.SelectedItem?.ToString() ?? "=";
        public string MaTG => string.IsNullOrWhiteSpace(txtMaTG.Text) ? null : txtMaTG.Text.Trim();
        public string MaTL => string.IsNullOrWhiteSpace(txtMaTL.Text) ? null : txtMaTL.Text.Trim();
        public decimal? TuGia => decimal.TryParse(txtTuGia.Text.Trim(), out decimal gia) ? gia : null;
        public string ToanTuGia => cboToanTuGia?.SelectedItem?.ToString() ?? ">=";
        public decimal? DenGia => decimal.TryParse(txtDenGia.Text.Trim(), out decimal gia) ? gia : null;
        public string PhepLogic
        {
            get
            {
                string raw = cboPhepLogic?.SelectedItem?.ToString() ?? "AND";
                if (raw.StartsWith("OR", StringComparison.OrdinalIgnoreCase)) return "OR";
                if (raw.StartsWith("NOT", StringComparison.OrdinalIgnoreCase)) return "NOT";
                return "AND";
            }
        }

        public frmTimKiemPopup()
        {
            InitializeComponent();
        }
    }
}
