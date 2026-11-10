using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmNhanVien : Form
    {
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private string hoLotHopLe = string.Empty;
        private string tenHopLe = string.Empty;
        private bool dangGanTen;

        public frmNhanVien()
        {
            InitializeComponent();
            txtHoLot.TextChanged += (_, _) => KiemTraKyTuTen(txtHoLot, ref hoLotHopLe);
            txtTenNV.TextChanged += (_, _) => KiemTraKyTuTen(txtTenNV, ref tenHopLe);
            txtHoLot.Leave += (_, _) => ChuanHoaO(txtHoLot, ref hoLotHopLe);
            txtTenNV.Leave += (_, _) => ChuanHoaO(txtTenNV, ref tenHopLe);
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                NapDanhSachChucVu(null);
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void HienThiDanhSach(List<NhanVienDTO> list)
        {
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = list;
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhanVien.Rows.Count) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            txtMaNV.Text = row.Cells["colMaNV"].Value?.ToString() ?? string.Empty;
            GanTen(txtHoLot, row.Cells["colHoLot"].Value?.ToString() ?? string.Empty, ref hoLotHopLe);
            GanTen(txtTenNV, row.Cells["colTenNV"].Value?.ToString() ?? string.Empty, ref tenHopLe);
            string? chucVu = row.Cells["colChucVu"].Value?.ToString();
            NapDanhSachChucVu(chucVu);
            txtMaNV.ReadOnly = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO(
                    string.Empty,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    cboChucVu.SelectedItem?.ToString());

                if (!bus.Them(nv)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                    throw new BusinessRuleException("Hãy chọn nhân viên cần sửa.");

                NhanVienDTO nv = new NhanVienDTO(
                    txtMaNV.Text,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    cboChucVu.SelectedItem?.ToString());

                if (!bus.Sua(nv)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maNV = txtMaNV.Text.Trim();
                if (maNV.Length == 0) throw new BusinessRuleException("Hãy chọn nhân viên cần xóa.");
                DialogResult result = MessageBox.Show(
                    $"Xóa nhân viên {maNV}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
                if (!bus.Xoa(maNV)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoiForm()
        {
            txtMaNV.Text = bus.TaoMaNhanVienTiepTheo();
            txtMaNV.ReadOnly = true;
            GanTen(txtHoLot, string.Empty, ref hoLotHopLe);
            GanTen(txtTenNV, string.Empty, ref tenHopLe);
            NapDanhSachChucVu(null);
            txtTimKiem.Clear();
            txtTenNV.Focus();
        }

        private void NapDanhSachChucVu(string? selected)
        {
            cboChucVu.Items.Clear();
            foreach (string chucVu in NhanVienBUS.DanhSachChucVu) cboChucVu.Items.Add(chucVu);
            if (!string.IsNullOrWhiteSpace(selected) && !cboChucVu.Items.Contains(selected))
                cboChucVu.Items.Add(selected);
            cboChucVu.SelectedIndex = selected == null ? -1 : cboChucVu.Items.IndexOf(selected);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            HienThiDanhSach(bus.TimKiem(txtTimKiem.Text.Trim()));
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnTimKiem_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void KiemTraKyTuTen(TextBox textBox, ref string lastValid)
        {
            if (dangGanTen) return;
            foreach (char c in textBox.Text)
            {
                if (NhanVienBUS.KyTuTenHopLe(c)) continue;
                int selectionStart = Math.Min(textBox.SelectionStart, lastValid.Length);
                GanTen(textBox, lastValid, ref lastValid);
                textBox.SelectionStart = selectionStart;
                return;
            }
            lastValid = textBox.Text;
        }

        private void ChuanHoaO(TextBox item, ref string lastValid)
        {
            GanTen(item, NhanVienBUS.ChuanHoaHoTen(item.Text), ref lastValid);
        }

        private void GanTen(TextBox textBox, string value, ref string lastValid)
        {
            dangGanTen = true;
            textBox.Text = value;
            lastValid = value;
            dangGanTen = false;
        }
    }
}
