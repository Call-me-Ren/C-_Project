#nullable disable
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmChonNhaCungCapPopup : Form
    {
        // Biến static hỗ trợ theo Yêu cầu 4 của GV
        public static string SelectedMaNCC = string.Empty;
        public static string SelectedTenNCC = string.Empty;

        private readonly NhaCungCapBUS _nccBUS;

        public frmChonNhaCungCapPopup()
        {
            InitializeComponent();
            _nccBUS = new NhaCungCapBUS();
            SelectedMaNCC = string.Empty;
            SelectedTenNCC = string.Empty;
        }

        private void FrmChonNhaCungCapPopup_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDanhSach(_nccBUS.GetAll());
                txtTimKiem.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDanhSach(List<NhaCungCapDTO> ds)
        {
            dgvNCC.DataSource = null;
            dgvNCC.DataSource = ds;

            if (dgvNCC.Columns["MaNCC"] is DataGridViewColumn c1) c1.HeaderText = "Mã Nhà Cung Cấp";
            if (dgvNCC.Columns["TenNCC"] is DataGridViewColumn c2) c2.HeaderText = "Tên Nhà Cung Cấp";
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                TaiDanhSach(_nccBUS.TimKiem(tuKhoa));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChon_Click(object sender, EventArgs e)
        {
            ChonDuLieu();
        }

        private void DgvNCC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonDuLieu();
            }
        }

        private void ChonDuLieu()
        {
            try
            {
                if (dgvNCC.CurrentRow != null && dgvNCC.CurrentRow.DataBoundItem is NhaCungCapDTO ncc)
                {
                    SelectedMaNCC = ncc.MaNCC;
                    SelectedTenNCC = ncc.TenNCC;

                    FormSharedData.SelectedMaNCC = ncc.MaNCC;
                    FormSharedData.SelectedTenNCC = ncc.TenNCC;

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một nhà cung cấp từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
