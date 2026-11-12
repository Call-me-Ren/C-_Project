using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmQuyDinhPhat : Form
    {
        private readonly QuyDinhPhatBUS bus = new QuyDinhPhatBUS();

        public frmQuyDinhPhat()
        {
            InitializeComponent();
        }

        private void frmQuyDinhPhat_Load(object sender, EventArgs e)
        {
            try
            {
                NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, null);
                NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, null);
                NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, null);
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void HienThiDanhSach(List<QuyDinhPhatDTO> list)
        {
            dgvQuyDinhPhat.AutoGenerateColumns = false;
            dgvQuyDinhPhat.DataSource = null;
            dgvQuyDinhPhat.DataSource = list;
        }

        private void dgvQuyDinhPhat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvQuyDinhPhat.Rows.Count) return;
            DataGridViewRow row = dgvQuyDinhPhat.Rows[e.RowIndex];
            txtMaQD.Text = row.Cells["colMaQD"].Value?.ToString() ?? string.Empty;
            txtTenQD.Text = row.Cells["colTenQD"].Value?.ToString() ?? string.Empty;
            NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, row.Cells["colLoaiPhat"].Value?.ToString());
            NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, row.Cells["colCachTinh"].Value?.ToString());
            NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, row.Cells["colDonViTinh"].Value?.ToString());

            if (decimal.TryParse(row.Cells["colMucPhat"].Value?.ToString(), out decimal mucPhat))
                numMucPhat.Value = Math.Max(numMucPhat.Minimum, Math.Min(numMucPhat.Maximum, mucPhat));
            else
                numMucPhat.Value = 0;
            txtMaQD.ReadOnly = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                QuyDinhPhatDTO qd = TaoQuyDinhTuForm(string.Empty);
                if (!bus.Them(qd)) throw new InvalidOperationException();
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
                if (string.IsNullOrWhiteSpace(txtMaQD.Text))
                    throw new BusinessRuleException("Hãy chọn quy định phạt cần sửa.");
                QuyDinhPhatDTO qd = TaoQuyDinhTuForm(txtMaQD.Text);
                if (!bus.Sua(qd)) throw new InvalidOperationException();
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
                string maQD = txtMaQD.Text.Trim();
                if (maQD.Length == 0) throw new BusinessRuleException("Hãy chọn quy định phạt cần xóa.");
                DialogResult result = MessageBox.Show(
                    $"Xóa quy định {maQD}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
                if (!bus.Xoa(maQD)) throw new InvalidOperationException();
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
            txtMaQD.Text = bus.TaoMaQuyDinhTiepTheo();
            txtMaQD.ReadOnly = true;
            txtTenQD.Clear();
            NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, null);
            NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, null);
            NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, null);
            numMucPhat.Value = 0;
            txtTimKiem.Clear();
            txtTenQD.Focus();
        }

        private QuyDinhPhatDTO TaoQuyDinhTuForm(string maQD)
        {
            return new QuyDinhPhatDTO(
                maQD,
                txtTenQD.Text,
                cboLoaiPhat.SelectedItem?.ToString(),
                cboCachTinh.SelectedItem?.ToString(),
                cboDonViTinh.SelectedItem?.ToString(),
                numMucPhat.Value);
        }

        private static void NapLuaChon(ComboBox combo, IReadOnlyList<string> options, string? selected)
        {
            combo.Items.Clear();
            foreach (string option in options) combo.Items.Add(option);
            if (!string.IsNullOrWhiteSpace(selected) && !combo.Items.Contains(selected)) combo.Items.Add(selected);
            combo.SelectedIndex = selected == null ? -1 : combo.Items.IndexOf(selected);
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
    }
}
