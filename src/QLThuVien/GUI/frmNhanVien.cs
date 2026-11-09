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

        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            HienThiDanhSach(bus.DocDS());
        }

        private void HienThiDanhSach(List<NhanVienDTO> list)
        {
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = list;
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["colMaNV"].Value?.ToString() ?? string.Empty;
                txtHoLot.Text = row.Cells["colHoLot"].Value?.ToString() ?? string.Empty;
                txtTenNV.Text = row.Cells["colTenNV"].Value?.ToString() ?? string.Empty;
                txtChucVu.Text = row.Cells["colChucVu"].Value?.ToString() ?? string.Empty;

                txtMaNV.ReadOnly = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO(
                    txtMaNV.Text,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    txtChucVu.Text
                );

                if (bus.Them(nv))
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NhanVienDTO nv = new NhanVienDTO(
                    txtMaNV.Text,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    txtChucVu.Text
                );

                if (bus.Sua(nv))
                {
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string maNV = txtMaNV.Text.Trim();
                if (string.IsNullOrWhiteSpace(maNV))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên có mã '{maNV}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (bus.Xoa(maNV))
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtMaNV.Clear();
            txtHoLot.Clear();
            txtTenNV.Clear();
            txtChucVu.Clear();
            txtTimKiem.Clear();

            txtMaNV.ReadOnly = false;
            txtMaNV.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            List<NhanVienDTO> ketQua = bus.TimKiem(tuKhoa);
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
