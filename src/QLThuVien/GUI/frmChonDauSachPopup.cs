#nullable disable
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmChonDauSachPopup : Form
    {
        // Biến static hỗ trợ theo Yêu cầu 4 của GV
        public static string SelectedMaDauSach = null;
        public static string SelectedTenDauSach = null;

        private readonly DauSachBUS _dauSachBUS;

        public frmChonDauSachPopup()
        {
            InitializeComponent();
            _dauSachBUS = new DauSachBUS();
            SelectedMaDauSach = null;
            SelectedTenDauSach = null;
        }

        private void FrmChonDauSachPopup_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDanhSach(_dauSachBUS.DocDSDauSach());
                txtTimKiem.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDanhSach(List<DauSachDTO> ds)
        {
            dgvDauSach.DataSource = null;
            dgvDauSach.DataSource = ds;

            if (dgvDauSach.Columns["MaDauSach"] is DataGridViewColumn c1) c1.HeaderText = "Mã Đầu Sách";
            if (dgvDauSach.Columns["TenSach"] is DataGridViewColumn c2) c2.HeaderText = "Tên Sách";
            if (dgvDauSach.Columns["DonGia"] is DataGridViewColumn c3)
            {
                c3.HeaderText = "Đơn Giá (VNĐ)";
                c3.DefaultCellStyle.Format = "N0";
            }
            if (dgvDauSach.Columns["SoLuong"] is DataGridViewColumn c4) c4.HeaderText = "Số Lượng";
            if (dgvDauSach.Columns["SLChoMuon"] is DataGridViewColumn c5) c5.HeaderText = "Đang Mượn";
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    TaiDanhSach(_dauSachBUS.DocDSDauSach());
                }
                else
                {
                    TaiDanhSach(_dauSachBUS.TimKiem(tuKhoa, tuKhoa));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChon_Click(object sender, EventArgs e)
        {
            ChonDuLieu();
        }

        private void DgvDauSach_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                if (dgvDauSach.CurrentRow != null && dgvDauSach.CurrentRow.DataBoundItem is DauSachDTO ds)
                {
                    SelectedMaDauSach = ds.MaDauSach;
                    SelectedTenDauSach = ds.TenSach;

                    FormSharedData.SelectedMaDauSach = ds.MaDauSach;
                    FormSharedData.SelectedTenDauSach = ds.TenSach;

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn 1 đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
