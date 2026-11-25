#nullable disable
using System;
using System.Collections.Generic;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public partial class frmNhaXuatBan : Form
    {
        private readonly NXBBUS nxbBUS = new NXBBUS();
        private bool isBinding = false;


        public frmNhaXuatBan()
        {
            InitializeComponent();
        }



        private void NXBGUI_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDuLieu(nxbBUS.DocDSNXB());
                LamMoi();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void TaiDuLieu(List<NXBDTO> ds)
        {
            isBinding = true;
            dgvNXB.DataSource = null;
            dgvNXB.DataSource = ds;
            DatTieuDe("MaNXB", "Mã NXB");
            DatTieuDe("TenNXB", "Tên NXB");
            DatTieuDe("DiaChi", "Địa chỉ");
            lblThongTin.Text = "Số NXB: " + ds.Count;
            dgvNXB.ClearSelection();
            isBinding = false;
        }

        private void dgvNXB_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (isBinding) return;
                if (dgvNXB.CurrentRow != null && dgvNXB.CurrentRow.DataBoundItem is NXBDTO nxb)
                {
                    txtMaNXB.Text = nxb.MaNXB;
                    txtMaNXB.ReadOnly = false;
                    txtMaNXB.BackColor = Color.FromArgb(238, 242, 246);
                    txtTenNXB.Text = nxb.TenNXB;
                    txtDiaChi.Text = nxb.DiaChi;
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNXB.Text) || string.IsNullOrWhiteSpace(txtTenNXB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ mã và tên NXB.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                nxbBUS.Them(new NXBDTO(txtMaNXB.Text.Trim(), txtTenNXB.Text.Trim(), txtDiaChi.Text.Trim()));
                TaiDuLieu(nxbBUS.DocDSNXB());
                LamMoi();
                MessageBox.Show("Thêm NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgvNXB.CurrentRow == null)
                {
                    MessageBox.Show("Chọn NXB cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtTenNXB.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhà xuất bản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maGoc = (dgvNXB.CurrentRow.DataBoundItem as NXBDTO).MaNXB;
                nxbBUS.Sua(new NXBDTO(maGoc, txtTenNXB.Text.Trim(), txtDiaChi.Text.Trim()));
                TaiDuLieu(nxbBUS.DocDSNXB());
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNXB.CurrentRow == null)
            {
                MessageBox.Show("Chọn NXB cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ma = txtMaNXB.Text.Trim();
            if (MessageBox.Show("Xóa NXB '" + ma + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    nxbBUS.Xoa(ma);
                    TaiDuLieu(nxbBUS.DocDSNXB());
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    FormErrors.Show(ex);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string kw = txtTimKiem.Text.Trim();
                if (string.IsNullOrWhiteSpace(kw)) { MessageBox.Show("Nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                TaiDuLieu(nxbBUS.timKiem(kw, kw));
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            try
            {
                TaiDuLieu(nxbBUS.DocDSNXB());
                txtTimKiem.Clear();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoi()
        {
            dgvNXB.ClearSelection();
            txtMaNXB.Text = nxbBUS.TaoMaNXBTiepTheo();
            txtMaNXB.ReadOnly = false;
            txtMaNXB.BackColor = Color.FromArgb(238, 242, 246);
            txtTenNXB.Clear();
            txtDiaChi.Clear();
            txtTenNXB.Focus();
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvNXB.Columns[tenCot] != null) dgvNXB.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
