using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Giao diện Quản lý Nhà Cung Cấp (Thành viên 2)
    /// Tuân thủ quy tắc 3 lớp: Chỉ gọi BUS, không gọi DAO hoặc SQL trực tiếp
    /// </summary>
    public partial class frmNhaCungCap : Form
    {
        public frmNhaCungCap()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvNCC.DataSource = NhaCungCapBUS.GetAll();
                if (dgvNCC.Columns["MaNCC"] is DataGridViewColumn colMa) colMa.HeaderText = "Mã Nhà Cung Cấp";
                if (dgvNCC.Columns["TenNCC"] is DataGridViewColumn colTen) colTen.HeaderText = "Tên Nhà Cung Cấp";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Nhà cung cấp: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNCC_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvNCC.Rows[e.RowIndex].DataBoundItem is NhaCungCapDTO item)
                {
                    txtMaNCC.Text = item.MaNCC;
                    txtTenNCC.Text = item.TenNCC;
                    txtMaNCC.ReadOnly = true; // Khi chọn dòng cũ thì không cho sửa Mã PK
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
                var result = NhaCungCapBUS.Them(ncc);
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    LoadData();
                    BtnLamMoi_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi thêm Nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NhaCungCapDTO ncc = new NhaCungCapDTO(txtMaNCC.Text, txtTenNCC.Text);
                var result = NhaCungCapBUS.Sua(ncc);
                MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi sửa Nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{txtMaNCC.Text}' không?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    var result = NhaCungCapBUS.Xoa(txtMaNCC.Text);
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, 
                        result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    if (result.Success)
                    {
                        LoadData();
                        BtnLamMoi_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi xóa Nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            try
            {
                txtMaNCC.Text = "";
                txtTenNCC.Text = "";
                txtTimKiem.Text = "";
                txtMaNCC.ReadOnly = false;
                txtMaNCC.Focus();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                dgvNCC.DataSource = NhaCungCapBUS.TimKiem(txtTimKiem.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
