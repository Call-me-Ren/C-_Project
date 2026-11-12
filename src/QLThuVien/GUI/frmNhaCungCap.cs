using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmNhaCungCap : Form
    {
        private readonly NhaCungCapBUS _nccBUS;

        public frmNhaCungCap()
        {
            InitializeComponent();
            _nccBUS = new NhaCungCapBUS();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvNCC.DataSource = _nccBUS.GetAll();

                if (dgvNCC.Columns["MaNCC"] is DataGridViewColumn c1)
                    c1.HeaderText = "Mã Nhà Cung Cấp";
                if (dgvNCC.Columns["TenNCC"] is DataGridViewColumn c2)
                    c2.HeaderText = "Tên Nhà Cung Cấp";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNCC_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvNCC.Rows[e.RowIndex].DataBoundItem is NhaCungCapDTO ncc)
                {
                    txtMaNCC.Text = ncc.MaNCC;
                    txtTenNCC.Text = ncc.TenNCC;
                    txtMaNCC.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chọn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
                var result = _nccBUS.Them(ncc);

                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    BtnLamMoi_Click(sender, e);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!txtMaNCC.ReadOnly)
                {
                    MessageBox.Show("Vui lòng chọn Nhà cung cấp từ danh sách để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
                var result = _nccBUS.Sua(ncc);

                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    BtnLamMoi_Click(sender, e);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
                {
                    MessageBox.Show("Vui lòng chọn Nhà cung cấp cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa Nhà cung cấp '{txtMaNCC.Text}' không?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    var result = _nccBUS.Xoa(txtMaNCC.Text);
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK,
                        result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    if (result.Success)
                    {
                        BtnLamMoi_Click(sender, e);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtTimKiem.Text = "";
            txtMaNCC.ReadOnly = false;
            txtMaNCC.Focus();
            LoadData();
        }

        private void TxtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                dgvNCC.DataSource = _nccBUS.TimKiem(txtTimKiem.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
