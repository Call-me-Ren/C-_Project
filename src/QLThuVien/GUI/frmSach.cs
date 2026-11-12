#nullable disable
using System;
using System.Collections.Generic;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public partial class frmSach : Form
    {
        private readonly SachBUS sachBUS = new SachBUS();
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();

        private bool isBinding = false;


        public frmSach()
        {
            InitializeComponent();
        }



        private void SachGUI_Load(object sender, EventArgs e)
        {
            TaiDuLieu(sachBUS.DocDSSach());
        }

        private void btnChonDauSach_Click(object sender, EventArgs e)
        {
            FormSharedData.SelectedMaDauSach = null;
            using (frmChonDauSachPopup popup = new frmChonDauSachPopup())
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(FormSharedData.SelectedMaDauSach))
                    {
                        txtMaDauSach.Text = FormSharedData.SelectedMaDauSach;
                    }
                }
            }
        }

        private void TaiDuLieu(List<SachDTO> ds)
        {
            isBinding = true;
            dgvSach.DataSource = null;
            dgvSach.DataSource = ds;

            if (dgvSach.Columns["TrangThai"] != null)
                dgvSach.Columns["TrangThai"].Visible = false;

            DatTieuDe("MaSach", "Mã sách");
            DatTieuDe("MaDauSach", "Mã đầu sách");
            DatTieuDe("TinhTrang", "Tình trạng");
            DatTieuDe("ViTri", "Vị trí");
            DatTieuDe("TrangThaiText", "Trạng thái");

            lblThongTin.Text = "Số sách: " + (ds != null ? ds.Count : 0);
            isBinding = false;

            if (dgvSach.CurrentRow != null && dgvSach.CurrentRow.DataBoundItem is SachDTO sSelect)
            {
                CapNhatTrangThaiNut(sSelect);
            }
        }

        private void dgvSach_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSach.Columns[e.ColumnIndex].Name == "TrangThaiText" || dgvSach.Columns[e.ColumnIndex].HeaderText == "Trạng thái")
            {
                if (dgvSach.Rows[e.RowIndex].DataBoundItem is SachDTO item)
                {
                    bool isMuon = (item.TrangThai == "True" || item.TrangThai == "1" || item.TrangThai == "Đã cho mượn");
                    e.Value = isMuon ? "Đã cho mượn" : "Chưa cho mượn";
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvSach_SelectionChanged(object sender, EventArgs e)
        {
            if (isBinding) return;

            if (dgvSach.CurrentRow != null && dgvSach.CurrentRow.DataBoundItem is SachDTO s)
            {
                txtMaSach.Text = s.MaSach;
                txtMaSach.ReadOnly = true;
                txtMaDauSach.Text = s.MaDauSach;
                txtTinhTrang.Text = s.TinhTrang;
                txtViTri.Text = s.ViTri;

                CapNhatTrangThaiNut(s);
            }
        }

        private void CapNhatTrangThaiNut(SachDTO s)
        {
            bool isMuon = (s.TrangThai == "True" || s.TrangThai == "1" || s.TrangThai == "Đã cho mượn");

            cboTrangThai.SelectedIndex = isMuon ? 1 : 0;

            btnSua.Enabled = !isMuon;
            btnXoa.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSach.Text) || string.IsNullOrWhiteSpace(txtMaDauSach.Text))
                {
                    MessageBox.Show("Nhập đầy đủ mã sách và mã đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SachDTO s = new SachDTO
                {
                    MaSach = txtMaSach.Text.Trim(),
                    MaDauSach = txtMaDauSach.Text.Trim(),
                    TinhTrang = txtTinhTrang.Text.Trim(),
                    TrangThai = cboTrangThai.SelectedIndex == 1 ? "1" : "0",
                    ViTri = txtViTri.Text.Trim()
                };

                if (sachBUS.Them(s))
                {
                    TaiDuLieu(sachBUS.DocDSSach());
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Không thể thêm sách", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSach.CurrentRow == null)
                {
                    MessageBox.Show("Chọn sách cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SachDTO s = new SachDTO
                {
                    MaSach = txtMaSach.Text.Trim(),
                    MaDauSach = txtMaDauSach.Text.Trim(),
                    TinhTrang = txtTinhTrang.Text.Trim(),
                    TrangThai = cboTrangThai.SelectedIndex == 1 ? "1" : "0",
                    ViTri = txtViTri.Text.Trim()
                };

                if (sachBUS.Sua(s))
                {
                    TaiDuLieu(sachBUS.DocDSSach());
                    MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể sửa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null)
            {
                MessageBox.Show("Chọn sách cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSach = txtMaSach.Text.Trim();
            string maDauSach = txtMaDauSach.Text.Trim();

            if (MessageBox.Show("Xóa sách '" + maSach + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (sachBUS.Xoa(maSach, maDauSach))
                    {
                        TaiDuLieu(sachBUS.DocDSSach());
                        LamMoi();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy sách '{maSach}' để xóa.", "Không thể xóa sách", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Nhập từ khóa mã sách hoặc mã đầu sách cần tìm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDuLieu(sachBUS.TimKiemTuKhoa(kw));
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu(sachBUS.DocDSSach());
            txtTimKiem.Clear();
        }

        private void LamMoi()
        {
            txtMaSach.Clear();
            txtMaSach.ReadOnly = false;
            txtMaDauSach.Clear();
            txtTinhTrang.Clear();
            txtViTri.Clear();
            cboTrangThai.SelectedIndex = 0;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            txtMaSach.Focus();
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvSach.Columns[tenCot] != null)
                dgvSach.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
