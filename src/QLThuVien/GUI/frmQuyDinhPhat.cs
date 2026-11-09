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
            HienThiDanhSach(bus.DocDS());
        }

        private void HienThiDanhSach(List<QuyDinhPhatDTO> list)
        {
            dgvQuyDinhPhat.AutoGenerateColumns = false;
            dgvQuyDinhPhat.DataSource = null;
            dgvQuyDinhPhat.DataSource = list;
        }

        private void dgvQuyDinhPhat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvQuyDinhPhat.Rows.Count)
            {
                DataGridViewRow row = dgvQuyDinhPhat.Rows[e.RowIndex];
                txtMaQD.Text = row.Cells["colMaQD"].Value?.ToString() ?? string.Empty;
                txtTenQD.Text = row.Cells["colTenQD"].Value?.ToString() ?? string.Empty;
                txtLoaiPhat.Text = row.Cells["colLoaiPhat"].Value?.ToString() ?? string.Empty;
                txtCachTinh.Text = row.Cells["colCachTinh"].Value?.ToString() ?? string.Empty;
                txtDonViTinh.Text = row.Cells["colDonViTinh"].Value?.ToString() ?? string.Empty;

                if (decimal.TryParse(row.Cells["colMucPhat"].Value?.ToString(), out decimal mucPhat))
                {
                    numMucPhat.Value = Math.Max(numMucPhat.Minimum, Math.Min(numMucPhat.Maximum, mucPhat));
                }
                else
                {
                    numMucPhat.Value = 0;
                }

                txtMaQD.ReadOnly = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                QuyDinhPhatDTO qd = new QuyDinhPhatDTO(
                    txtMaQD.Text,
                    txtTenQD.Text,
                    txtLoaiPhat.Text,
                    txtCachTinh.Text,
                    txtDonViTinh.Text,
                    numMucPhat.Value
                );

                if (bus.Them(qd))
                {
                    MessageBox.Show("Thêm quy định phạt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(bus.LayDanhSach());
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaQD.Text))
                {
                    MessageBox.Show("Vui lòng chọn quy định phạt cần sửa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                QuyDinhPhatDTO qd = new QuyDinhPhatDTO(
                    txtMaQD.Text,
                    txtTenQD.Text,
                    txtLoaiPhat.Text,
                    txtCachTinh.Text,
                    txtDonViTinh.Text,
                    numMucPhat.Value
                );

                if (bus.Sua(qd))
                {
                    MessageBox.Show("Cập nhật quy định phạt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(bus.LayDanhSach());
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maQD = txtMaQD.Text.Trim();
                if (string.IsNullOrWhiteSpace(maQD))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã quy định cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa quy định phạt có mã '{maQD}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (bus.Xoa(maQD))
                    {
                        MessageBox.Show("Xóa quy định phạt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSach(bus.LayDanhSach());
                        LamMoiForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            HienThiDanhSach(bus.DocDS());
        }

        private void LamMoiForm()
        {
            txtMaQD.Clear();
            txtTenQD.Clear();
            txtLoaiPhat.Clear();
            txtCachTinh.Clear();
            txtDonViTinh.Clear();
            numMucPhat.Value = 0;
            txtTimKiem.Clear();

            txtMaQD.ReadOnly = false;
            txtMaQD.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            List<QuyDinhPhatDTO> ketQua = bus.TimKiem(tuKhoa);
            HienThiDanhSach(ketQua);
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            HienThiDanhSach(bus.LayDanhSach());
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
