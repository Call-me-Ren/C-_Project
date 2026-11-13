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
        public int? NamXB => int.TryParse(txtNamXB.Text.Trim(), out int nxb) ? nxb : null;
        public string MaTG => string.IsNullOrWhiteSpace(txtMaTG.Text) ? null : txtMaTG.Text.Trim();
        public string MaTL => string.IsNullOrWhiteSpace(txtMaTL.Text) ? null : txtMaTL.Text.Trim();
        public decimal? TuGia => decimal.TryParse(txtTuGia.Text.Trim(), out decimal gia) ? gia : null;
        public decimal? DenGia => decimal.TryParse(txtDenGia.Text.Trim(), out decimal gia) ? gia : null;

        public frmTimKiemPopup()
        {
            InitializeComponent();
        }
    }
}
