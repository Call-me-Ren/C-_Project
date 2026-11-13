using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmTimKiemDocGiaPopup : Form
    {
        // BIẾN STATIC TRUYỀN DỮ LIỆU VỀ FORM CHÍNH (Đúng Yêu cầu số 4 của Giáo viên)
        public static string SelectedMaDocGia = string.Empty;
        public static string SelectedTenDocGia = string.Empty;

        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();

        public frmTimKiemDocGiaPopup()
        {
            InitializeComponent();
            SelectedMaDocGia = string.Empty;
            SelectedTenDocGia = string.Empty;
        }

        private void frmTimKiemDocGiaPopup_Load(object sender, EventArgs e)
        {
            try
            {
                HienThiDanhSach(docGiaBUS.DocDSDocGia());
                txtTimKiem.Focus();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void HienThiDanhSach(ArrayList danhSach)
        {
            dgvDocGia.AutoGenerateColumns = false;
            dgvDocGia.DataSource = null;

            List<DocGiaDTO> list = new List<DocGiaDTO>();
            foreach (DocGiaDTO dg in danhSach)
            {
                list.Add(dg);
            }

            dgvDocGia.DataSource = list;
            lblThongKe.Text = $"Tổng số: {list.Count} độc giả";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                ArrayList ketQua = docGiaBUS.TimKiem(tuKhoa);
                HienThiDanhSach(ketQua);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                ArrayList ketQua = docGiaBUS.TimKiem(tuKhoa);
                HienThiDanhSach(ketQua);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            XacNhanChon();
        }

        private void dgvDocGia_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                XacNhanChon();
            }
        }

        private void XacNhanChon()
        {
            if (dgvDocGia.CurrentRow != null && dgvDocGia.CurrentRow.DataBoundItem is DocGiaDTO dg)
            {
                SelectedMaDocGia = dg.MaDG;
                SelectedTenDocGia = dg.HoTen;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một độc giả trong bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
