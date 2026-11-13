#nullable disable
using System;
using System.Collections.Generic;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public partial class frmTheLoai : Form
    {
        private readonly TheLoaiBUS theLoaiBUS = new TheLoaiBUS();


        public frmTheLoai()
        {
            InitializeComponent();
        }



        private void TheLoaiGUI_Load(object sender, EventArgs e)
        {
            TaiDuLieu(theLoaiBUS.DocDSTL());
        }

        private void TaiDuLieu(List<TheLoaiDTO> ds)
        {
            dgvTheLoai.DataSource = null;
            dgvTheLoai.DataSource = ds;
            DatTieuDe("MaTL", "Mã thể loại");
            DatTieuDe("TenTL", "Tên thể loại");
            lblThongTin.Text = "Số thể loại: " + ds.Count;
        }

        private void dgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow != null && dgvTheLoai.CurrentRow.DataBoundItem is TheLoaiDTO tl)
            {
                txtMaTL.Text = tl.MaTL;
                txtMaTL.ReadOnly = true;
                txtMaTL.BackColor = Color.FromArgb(238, 242, 246);
                txtTenTL.Text = tl.TenTL;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTL.Text) || string.IsNullOrWhiteSpace(txtTenTL.Text))
                { MessageBox.Show("Nhập đầy đủ mã và tên thể loại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                theLoaiBUS.Them(new TheLoaiDTO(txtMaTL.Text.Trim(), txtTenTL.Text.Trim()));
                TaiDuLieu(theLoaiBUS.DocDSTL());
                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { FormErrors.Show(ex); }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTheLoai.CurrentRow == null) { MessageBox.Show("Chọn thể loại cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (string.IsNullOrWhiteSpace(txtTenTL.Text)) { MessageBox.Show("Vui lòng nhập tên thể loại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string maGoc = (dgvTheLoai.CurrentRow.DataBoundItem as TheLoaiDTO).MaTL;
                theLoaiBUS.Sua(new TheLoaiDTO(maGoc, txtTenTL.Text.Trim()));
                TaiDuLieu(theLoaiBUS.DocDSTL());
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { FormErrors.Show(ex); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow == null) { MessageBox.Show("Chọn thể loại cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string ma = txtMaTL.Text.Trim();
            if (MessageBox.Show("Xóa thể loại '" + ma + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    theLoaiBUS.Xoa(ma);
                    TaiDuLieu(theLoaiBUS.DocDSTL());
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { FormErrors.Show(ex); }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw)) { MessageBox.Show("Nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            TaiDuLieu(theLoaiBUS.timKiem(kw, kw));
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu(theLoaiBUS.DocDSTL());
            txtTimKiem.Clear();
        }

        private void LamMoi()
        {
            txtMaTL.Clear(); txtTenTL.Clear(); txtMaTL.Focus();
            txtMaTL.ReadOnly = false; txtMaTL.BackColor = SystemColors.Window;
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvTheLoai.Columns[tenCot] != null) dgvTheLoai.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
