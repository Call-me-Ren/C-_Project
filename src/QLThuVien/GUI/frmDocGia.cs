using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmDocGia : Form
    {
        // 1. KHỞI TẠO CÁC LỚP BUS ĐỂ XỬ LÝ NGHIỆP VỤ (Đúng mô hình 3 lớp)
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly LoaiDocGiaBUS loaiDocGiaBUS = new LoaiDocGiaBUS();

        public frmDocGia()
        {
            InitializeComponent();
        }

        // 2. SỰ KIỆN LOAD FORM
        private void frmDocGia_Load(object sender, EventArgs e)
        {
            try
            {
                // Nạp dữ liệu ComboBox Loại độc giả từ LoaiDocGiaBUS
                NapComboBoxLoaiDocGia();

                // Nạp danh sách Độc giả từ CSDL vào ArrayList và hiển thị lên bảng
                HienThiDanhSach(docGiaBUS.DocDSDocGia());

                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 3. NẠP DỮ LIỆU VÀO COMBOBOX LOẠI ĐỘC GIẢ
        private void NapComboBoxLoaiDocGia()
        {
            ArrayList dsLDG = loaiDocGiaBUS.DocDSLoaiDocGia();
            List<LoaiDocGiaDTO> list = new List<LoaiDocGiaDTO>();

            foreach (LoaiDocGiaDTO item in dsLDG)
            {
                list.Add(item);
            }

            cboLoaiDocGia.DataSource = null;
            cboLoaiDocGia.DisplayMember = "TenLDG";
            cboLoaiDocGia.ValueMember = "MaLDG";
            cboLoaiDocGia.DataSource = list;
        }

        // 4. HIỂN THỊ DỮ LIỆU LÊN DATAGRIDVIEW
        private void HienThiDanhSach(ArrayList danhSach)
        {
            dgvDocGia.AutoGenerateColumns = false;
            dgvDocGia.DataSource = null;

            List<DocGiaDTO> list = new List<DocGiaDTO>();
            foreach (DocGiaDTO item in danhSach)
            {
                list.Add(item);
            }

            dgvDocGia.DataSource = list;
        }

        // 5. CHỌN DÒNG TRÊN BẢNG
        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvDocGia.Rows.Count) return;

                DataGridViewRow row = dgvDocGia.Rows[e.RowIndex];
                txtMaDG.Text = row.Cells["colMaDG"].Value?.ToString() ?? string.Empty;
                txtHoLot.Text = row.Cells["colHoLot"].Value?.ToString() ?? string.Empty;
                txtTenDG.Text = row.Cells["colTenDG"].Value?.ToString() ?? string.Empty;
                txtSDT.Text = row.Cells["colSDT"].Value?.ToString() ?? string.Empty;

                // Xử lý Ngày sinh
                object? nsValue = row.Cells["colNgaySinh"].Value;
                if (nsValue != null && DateTime.TryParse(nsValue.ToString(), out DateTime ns))
                {
                    dtpNgaySinh.Checked = true;
                    dtpNgaySinh.Value = ns;
                }
                else
                {
                    dtpNgaySinh.Checked = false;
                }

                // Chọn loại độc giả tương ứng trong ComboBox
                if (row.DataBoundItem is DocGiaDTO selectedDG)
                {
                    cboLoaiDocGia.SelectedValue = selectedDG.MaLDG;
                }

                // Khóa ô Mã độc giả khi chọn sửa/xem
                txtMaDG.ReadOnly = true;
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 6. CHỨC NĂNG THÊM
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                DocGiaDTO dg = ThuThapDuLieuTuForm();

                if (docGiaBUS.Them(dg, out string error))
                {
                    MessageBox.Show("Thêm độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Cập nhật lại TenLDG để hiển thị trên DataGridView
                    if (cboLoaiDocGia.SelectedItem is LoaiDocGiaDTO ldg)
                    {
                        dg.TenLDG = ldg.TenLDG;
                    }
                    HienThiDanhSach(docGiaBUS.dsdg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 7. CHỨC NĂNG SỬA
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maDG = txtMaDG.Text.Trim();
                if (string.IsNullOrWhiteSpace(maDG))
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DocGiaDTO dg = ThuThapDuLieuTuForm();

                if (docGiaBUS.Sua(dg, out string error))
                {
                    MessageBox.Show("Cập nhật thông tin độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(docGiaBUS.dsdg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 8. CHỨC NĂNG XÓA
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maDG = txtMaDG.Text.Trim();
                if (string.IsNullOrWhiteSpace(maDG))
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa độc giả '{maDG}' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                if (docGiaBUS.Xoa(maDG, out string error))
                {
                    MessageBox.Show("Xóa độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(docGiaBUS.dsdg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 9. LÀM MỚI FORM
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoiForm()
        {
            txtMaDG.Clear();
            txtHoLot.Clear();
            txtTenDG.Clear();
            txtSDT.Clear();
            dtpNgaySinh.Checked = false;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-18);

            if (cboLoaiDocGia.Items.Count > 0)
            {
                cboLoaiDocGia.SelectedIndex = 0;
            }

            txtMaDG.ReadOnly = false;
            txtMaDG.Focus();
        }

        // 10. TÌM KIẾM TRÊN RAM
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                ArrayList ketQua = docGiaBUS.TimKiem(keyword);
                HienThiDanhSach(ketQua);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // 11. HIỆN TẤT CẢ
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(docGiaBUS.dsdg_arr);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // HÀM TIỆN ÍCH GOM DỮ LIỆU TỪ GIAO DIỆN THÀNH ĐỐI TƯỢNG DTO
        private DocGiaDTO ThuThapDuLieuTuForm()
        {
            string maDG = txtMaDG.Text.Trim();
            string? hoLot = string.IsNullOrWhiteSpace(txtHoLot.Text) ? null : txtHoLot.Text.Trim();
            string tenDG = txtTenDG.Text.Trim();
            DateTime? ngaySinh = dtpNgaySinh.Checked ? dtpNgaySinh.Value.Date : null;
            string? sdt = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim();
            string maLDG = cboLoaiDocGia.SelectedValue?.ToString() ?? string.Empty;
            string? tenLDG = cboLoaiDocGia.Text;

            return new DocGiaDTO(maDG, hoLot, tenDG, ngaySinh, sdt, maLDG, tenLDG);
        }

        // 12. XUẤT EXCEL DANH SÁCH ĐỘC GIẢ
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var ds = dgvDocGia.DataSource as List<DocGiaDTO>;
                if (ds == null || ds.Count == 0)
                {
                    MessageBox.Show("Danh sách độc giả đang trống, không thể xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var sfd = new SaveFileDialog();
                sfd.Filter = "Tài liệu Excel (*.xlsx)|*.xlsx";
                sfd.FileName = $"DanhSachDocGia_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExcelExportService.XuatDanhSachDocGia(ds, sfd.FileName);

                    DialogResult open = MessageBox.Show(
                        "Xuất danh sách độc giả ra Excel thành công!\nBạn có muốn mở file ngay bây giờ không?",
                        "Xuất Excel thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    if (open == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName)
                        {
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 13. XUẤT EXCEL LỊCH SỬ MƯỢN TRẢ CỦA ĐỘC GIẢ
        private void btnXuatLichSu_Click(object sender, EventArgs e)
        {
            try
            {
                string maDG = txtMaDG.Text.Trim();
                if (string.IsNullOrWhiteSpace(maDG))
                {
                    MessageBox.Show("Vui lòng chọn 1 độc giả từ bảng để xuất lịch sử mượn trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DocGiaDTO? dg = docGiaBUS.TimTheoMa(maDG);
                if (dg == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả được chọn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Data.DataTable dtLichSu = docGiaBUS.LayLichSuMuonTra(maDG);
                if (dtLichSu.Rows.Count == 0)
                {
                    MessageBox.Show($"Độc giả '{dg.HoTen}' ({dg.MaDG}) chưa có lịch sử mượn trả sách nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var sfd = new SaveFileDialog();
                sfd.Filter = "Tài liệu Excel (*.xlsx)|*.xlsx";
                sfd.FileName = $"LichSuMuon_{dg.MaDG}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExcelExportService.XuatLichSuMuonTra(dg, dtLichSu, sfd.FileName);

                    DialogResult open = MessageBox.Show(
                        "Xuất lịch sử mượn trả ra Excel thành công!\nBạn có muốn mở file ngay bây giờ không?",
                        "Xuất Excel thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    if (open == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName)
                        {
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }
    }
}
