#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public partial class frmDauSach : Form
    {
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();
        private readonly SachBUS sachBUS = new SachBUS();

        private bool isBinding = false;



        public frmDauSach()
        {
            InitializeComponent();
        }

        private Button CreateLookupButton(int x, int y)
        {
            Button btn = new Button
            {
                Text = "...",
                Location = new Point(x, y),
                Size = new Size(26, 25),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        private void DauSachGUI_Load(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        private void LoadDuLieu(string selectMaDauSach = null)
        {
            List<DauSachDTO> ds = dauSachBUS.DocDSSach();
            TaiDauSach(ds, selectMaDauSach);
        }

        private void TaiDauSach(List<DauSachDTO> ds, string selectMaDauSach = null)
        {
            isBinding = true;
            dgvDauSach.DataSource = null;
            dgvDauSach.DataSource = ds;
            dgvDauSach.Refresh();

            DatTieuDe(dgvDauSach, "MaDauSach", "Mã đầu sách");
            DatTieuDe(dgvDauSach, "TenSach", "Tên sách");
            DatTieuDe(dgvDauSach, "MaTG", "Mã TG");
            DatTieuDe(dgvDauSach, "MaTL", "Mã TL");
            DatTieuDe(dgvDauSach, "MaNXB", "Mã NXB");
            DatTieuDe(dgvDauSach, "NamXB", "Năm XB");
            DatTieuDe(dgvDauSach, "SoLuong", "Số lượng");
            DatTieuDe(dgvDauSach, "SLChoMuon", "Đã mượn");
            DatTieuDe(dgvDauSach, "SLConLai", "Còn lại");
            DatTieuDe(dgvDauSach, "DonGia", "Đơn giá");

            if (dgvDauSach.Columns["DonGia"] != null)
                dgvDauSach.Columns["DonGia"].DefaultCellStyle.Format = "#,##0 VNĐ";

            lblThongTin.Text = "Tổng số: " + ds.Count + " đầu sách";
            isBinding = false;

            if (dgvDauSach.Rows.Count > 0)
            {
                int targetIndex = 0;
                if (!string.IsNullOrEmpty(selectMaDauSach))
                {
                    for (int i = 0; i < dgvDauSach.Rows.Count; i++)
                    {
                        if (dgvDauSach.Rows[i].DataBoundItem is DauSachDTO item && item.MaDauSach == selectMaDauSach)
                        {
                            targetIndex = i;
                            break;
                        }
                    }
                }
                dgvDauSach.ClearSelection();
                dgvDauSach.Rows[targetIndex].Selected = true;
                HienThiThongTinDauSach(dgvDauSach.Rows[targetIndex].DataBoundItem as DauSachDTO);
            }
            else
            {
                LamMoi();
                dgvSach.DataSource = null;
            }
        }

        private void dgvDauSach_SelectionChanged(object sender, EventArgs e)
        {
            if (isBinding) return;

            if (dgvDauSach.CurrentRow != null && dgvDauSach.CurrentRow.DataBoundItem is DauSachDTO ds)
            {
                HienThiThongTinDauSach(ds);
            }
        }

        private void HienThiThongTinDauSach(DauSachDTO ds)
        {
            if (ds == null) return;

            txtMaDauSach.ReadOnly = true;
            txtMaDauSach.BackColor = Color.FromArgb(238, 242, 246);
            txtMaDauSach.Text = ds.MaDauSach;

            txtTenSach.Text = ds.TenSach;
            txtMaTG.Text = ds.MaTG;
            txtMaTL.Text = ds.MaTL;
            txtMaNXB.Text = ds.MaNXB;
            txtNamXB.Text = ds.NamXB?.ToString() ?? "";
            txtSoLuong.Text = ds.SoLuong.ToString();
            txtDonGia.Text = ds.DonGia.ToString("G0");

            TaiSachCuaDauSachDangChon();
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaiSachCuaDauSachDangChon();
        }

        private void TaiSachCuaDauSachDangChon()
        {
            string maDauSach = null;
            if (dgvDauSach.CurrentRow != null && dgvDauSach.CurrentRow.DataBoundItem is DauSachDTO dsChon)
                maDauSach = dsChon.MaDauSach;

            if (string.IsNullOrEmpty(maDauSach))
            {
                dgvSach.DataSource = null;
                return;
            }

            bool? trangThai = null;
            if (cboTrangThai.SelectedIndex == 1) trangThai = false;
            else if (cboTrangThai.SelectedIndex == 2) trangThai = true;

            dgvSach.DataSource = null;
            dgvSach.DataSource = sachBUS.TimTheoTrangThai(trangThai, maDauSach);

            if (dgvSach.Columns["TrangThai"] != null) dgvSach.Columns["TrangThai"].Visible = false;
            DatTieuDe(dgvSach, "MaSach", "Mã sách");
            DatTieuDe(dgvSach, "MaDauSach", "Mã đầu sách");
            DatTieuDe(dgvSach, "TinhTrang", "Tình trạng");
            DatTieuDe(dgvSach, "ViTri", "Vị trí");
            DatTieuDe(dgvSach, "TrangThaiText", "Trạng thái mượn");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaDauSach.Text) || string.IsNullOrWhiteSpace(txtTenSach.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã đầu sách và Tên sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dauSachBUS.KiemTraTrungMa(txtMaDauSach.Text.Trim()))
                {
                    MessageBox.Show("Mã đầu sách '" + txtMaDauSach.Text.Trim() + "' đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DauSachDTO ds = new DauSachDTO
                {
                    MaDauSach = txtMaDauSach.Text.Trim(),
                    TenSach = txtTenSach.Text.Trim(),
                    MaTG = txtMaTG.Text.Trim(),
                    MaTL = txtMaTL.Text.Trim(),
                    MaNXB = txtMaNXB.Text.Trim(),
                    NamXB = string.IsNullOrWhiteSpace(txtNamXB.Text) ? null : int.Parse(txtNamXB.Text.Trim()),
                    DonGia = string.IsNullOrWhiteSpace(txtDonGia.Text) ? 0 : decimal.Parse(txtDonGia.Text.Trim())
                };

                dauSachBUS.Them(ds);
                LoadDuLieu(ds.MaDauSach);
                MessageBox.Show("Thêm đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDauSach.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn đầu sách cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maDauSachCu = (dgvDauSach.CurrentRow.DataBoundItem as DauSachDTO)?.MaDauSach;

                if (string.IsNullOrWhiteSpace(txtMaDauSach.Text) || string.IsNullOrWhiteSpace(txtTenSach.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã đầu sách và Tên sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DauSachDTO ds = new DauSachDTO
                {
                    MaDauSach = txtMaDauSach.Text.Trim(),
                    TenSach = txtTenSach.Text.Trim(),
                    MaTG = txtMaTG.Text.Trim(),
                    MaTL = txtMaTL.Text.Trim(),
                    MaNXB = txtMaNXB.Text.Trim(),
                    NamXB = string.IsNullOrWhiteSpace(txtNamXB.Text) ? null : int.Parse(txtNamXB.Text.Trim()),
                    DonGia = string.IsNullOrWhiteSpace(txtDonGia.Text) ? 0 : decimal.Parse(txtDonGia.Text.Trim())
                };

                dauSachBUS.Sua(ds, maDauSachCu);
                LoadDuLieu(ds.MaDauSach);
                MessageBox.Show("Cập nhật thông tin đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDauSach.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đầu sách cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ma = txtMaDauSach.Text.Trim();
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa đầu sách '" + ma + "'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    dauSachBUS.Xoa(ma);
                    LoadDuLieu();
                    LamMoi();
                    MessageBox.Show("Xóa đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm (mã hoặc tên sách).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<DauSachDTO> ketQua = dauSachBUS.TimKiem(keyword, keyword);
            TaiDauSach(ketQua);
            lblThongTin.Text = "Kết quả: " + ketQua.Count + " đầu sách";
        }

        private void btnTimNangCao_Click(object sender, EventArgs e)
        {
            using (frmTimKiemPopup popup = new frmTimKiemPopup())
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    List<DauSachDTO> ketQua = dauSachBUS.TimKiemNangCao(
                        popup.MaDauSach,
                        popup.TenSach,
                        popup.SoLuong,
                        popup.NamXB,
                        popup.MaTG,
                        popup.MaTL,
                        popup.TuGia,
                        popup.DenGia
                    );

                    TaiDauSach(ketQua);
                    lblThongTin.Text = "Tìm nâng cao: " + ketQua.Count + " đầu sách";

                    if (ketQua.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy đầu sách nào phù hợp điều kiện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            List<DauSachDTO> danhSach = dgvDauSach.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(row => row.DataBoundItem as DauSachDTO)
                .Where(item => item != null)
                .ToList();

            if (danhSach.Count == 0)
            {
                MessageBox.Show("Không có đầu sách để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Lưu danh sách đầu sách",
                Filter = "Sổ làm việc Excel (*.xlsx)|*.xlsx",
                FileName = "DanhSachDauSach.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    string[] tieuDe = { "Mã đầu sách", "Tên sách", "Mã tác giả", "Mã thể loại", "Mã NXB", "Năm xuất bản", "Đơn giá", "Số lượng", "Đã mượn", "Còn lại" };
                    List<IReadOnlyList<string>> cacDong = danhSach.Select(sach => (IReadOnlyList<string>)new[]
                    {
                        sach.MaDauSach, sach.TenSach, sach.MaTG, sach.MaTL, sach.MaNXB,
                        sach.NamXB?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
                        sach.DonGia.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        sach.SoLuong.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        sach.SLChoMuon.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        sach.SLConLai.ToString(System.Globalization.CultureInfo.InvariantCulture)
                    }).ToList();
                    DauSachExcelService.Ghi(dialog.FileName, tieuDe, cacDong);
                    MessageBox.Show($"Đã xuất {danhSach.Count} đầu sách ra file Excel .xlsx.", "Xuất thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xuất file: " + ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "Chọn file Excel chứa đầu sách",
                    Filter = "Sổ làm việc Excel (*.xlsx)|*.xlsx",
                    DefaultExt = "xlsx",
                    CheckFileExists = true
                };
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                string ketQua = dauSachBUS.NhapExcel(dialog.FileName, out int soLuongThem, out int soLuongBoQua, out List<string> loi);
                LoadDuLieu();
                string chiTiet = loi.Count == 0 ? string.Empty : Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, loi.Take(10));
                if (loi.Count > 10) chiTiet += Environment.NewLine + $"... còn {loi.Count - 10} dòng có thông báo.";
                MessageBox.Show(ketQua + chiTiet, "Kết quả nhập Excel", MessageBoxButtons.OK,
                    soLuongBoQua > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể nhập file Excel: " + ex.Message, "Lỗi nhập Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
            txtTimKiem.Clear();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            txtMaDauSach.ReadOnly = false;
            txtMaDauSach.BackColor = SystemColors.Window;
            txtMaDauSach.Clear();
            txtTenSach.Clear();
            txtMaTG.Clear();
            txtMaTL.Clear();
            txtMaNXB.Clear();
            txtNamXB.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtMaDauSach.Focus();
        }

        private void DatTieuDe(DataGridView dgv, string tenCot, string tieuDe)
        {
            if (dgv.Columns[tenCot] != null) dgv.Columns[tenCot].HeaderText = tieuDe;
        }
    }

}
