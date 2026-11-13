#nullable disable
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmChonNhanVienPopup : Form
    {
        public static string SelectedMaNV = string.Empty;
        public static string SelectedTenNV = string.Empty;

        private readonly NhanVienBUS _nhanVienBUS;

        public frmChonNhanVienPopup()
        {
            InitializeComponent();
            _nhanVienBUS = new NhanVienBUS();
            SelectedMaNV = string.Empty;
            SelectedTenNV = string.Empty;
        }

        private void FrmChonNhanVienPopup_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDanhSach(_nhanVienBUS.DocDS());
                txtTimKiem.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDanhSach(List<NhanVienDTO> ds)
        {
            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = ds;

            if (dgvNhanVien.Columns["MaNV"] is DataGridViewColumn c1) c1.HeaderText = "Mã Nhân Viên";
            if (dgvNhanVien.Columns["HoLot"] is DataGridViewColumn c2) c2.HeaderText = "Họ Lót";
            if (dgvNhanVien.Columns["TenNV"] is DataGridViewColumn c3) c3.HeaderText = "Tên NV";
            if (dgvNhanVien.Columns["HoTen"] is DataGridViewColumn c4) c4.HeaderText = "Họ và Tên";
            if (dgvNhanVien.Columns["ChucVu"] is DataGridViewColumn c5) c5.HeaderText = "Chức Vụ";
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                TaiDanhSach(_nhanVienBUS.TimKiem(tuKhoa));
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

        private void DgvNhanVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                if (dgvNhanVien.CurrentRow != null && dgvNhanVien.CurrentRow.DataBoundItem is NhanVienDTO nv)
                {
                    SelectedMaNV = nv.MaNV;
                    SelectedTenNV = nv.HoTen;

                    FormSharedData.SelectedMaNV = nv.MaNV;
                    FormSharedData.SelectedTenNV = nv.HoTen;

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
