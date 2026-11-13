#nullable disable
using System;
using System.Collections.Generic;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public partial class frmTacGia : Form
    {
        private readonly TacGiaBUS tacGiaBUS = new TacGiaBUS();


        public frmTacGia()
        {
            InitializeComponent();
        }



        private void TacGiaGUI_Load(object sender, EventArgs e)
        {
            TaiDuLieu(tacGiaBUS.DocDSTacGia());
        }

        private void TaiDuLieu(List<TacGiaDTO> ds)
        {
            dgvTacGia.DataSource = null;
            dgvTacGia.DataSource = ds;
            DatTieuDe("MaTG", "Mã TG");
            DatTieuDe("HoLot", "Họ lót");
            DatTieuDe("TenTG", "Tên TG");
            DatTieuDe("NgaySinh", "Ngày sinh");
            DatTieuDe("SDT", "SĐT");
            lblThongTin.Text = "Số tác giả: " + ds.Count;
        }

        private void dgvTacGia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow != null && dgvTacGia.CurrentRow.DataBoundItem is TacGiaDTO tg)
            {
                txtMaTG.Text = tg.MaTG;
                txtMaTG.ReadOnly = true;
                txtMaTG.BackColor = Color.FromArgb(238, 242, 246);
                txtHoLot.Text = tg.HoLot;
                txtTenTG.Text = tg.TenTG;
                txtSDT.Text = tg.SDT;
                if (tg.NgaySinh.HasValue)
                {
                    chkNgaySinh.Checked = true;
                    dtpNgaySinh.Value = tg.NgaySinh.Value;
                }
                else
                {
                    chkNgaySinh.Checked = false;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTG.Text) || string.IsNullOrWhiteSpace(txtTenTG.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã tác giả và Tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TacGiaDTO tg = new TacGiaDTO
                {
                    MaTG = txtMaTG.Text.Trim(),
                    HoLot = txtHoLot.Text.Trim(),
                    TenTG = txtTenTG.Text.Trim(),
                    NgaySinh = chkNgaySinh.Checked ? dtpNgaySinh.Value : null,
                    SDT = txtSDT.Text.Trim()
                };

                tacGiaBUS.Them(tg);
                TaiDuLieu(tacGiaBUS.DocDSTacGia());
                LamMoi();
                MessageBox.Show("Thêm tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgvTacGia.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn tác giả cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenTG.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maGoc = (dgvTacGia.CurrentRow.DataBoundItem as TacGiaDTO)?.MaTG ?? txtMaTG.Text.Trim();
                TacGiaDTO tg = new TacGiaDTO
                {
                    MaTG = maGoc,
                    HoLot = txtHoLot.Text.Trim(),
                    TenTG = txtTenTG.Text.Trim(),
                    NgaySinh = chkNgaySinh.Checked ? dtpNgaySinh.Value : null,
                    SDT = txtSDT.Text.Trim()
                };

                tacGiaBUS.Sua(tg);
                TaiDuLieu(tacGiaBUS.DocDSTacGia());
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tác giả cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ma = txtMaTG.Text.Trim();
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa tác giả '" + ma + "'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    tacGiaBUS.Xoa(ma);
                    TaiDuLieu(tacGiaBUS.DocDSTacGia());
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa tác giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TaiDuLieu(tacGiaBUS.timKiem(kw, kw));
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu(tacGiaBUS.DocDSTacGia());
            txtTimKiem.Clear();
        }

        private void LamMoi()
        {
            txtMaTG.Clear();
            txtMaTG.ReadOnly = false;
            txtMaTG.BackColor = SystemColors.Window;
            txtHoLot.Clear();
            txtTenTG.Clear();
            txtSDT.Clear();
            chkNgaySinh.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            txtMaTG.Focus();
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvTacGia.Columns[tenCot] != null) dgvTacGia.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
