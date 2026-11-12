using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmMuonTraSach : Form
    {
        // 1. KHỞI TẠO CÁC LỚP BUS ĐỂ XỬ LÝ NGHIỆP VỤ (Đúng chuẩn mô hình 3 lớp)
        private readonly PhieuMuonBUS pmBUS = new PhieuMuonBUS();
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly NhanVienBUS nhanVienBUS = new NhanVienBUS();

        private string? maPMDangChon = null;

        public frmMuonTraSach()
        {
            InitializeComponent();
        }

        // 2. SỰ KIỆN LOAD FORM
        private void frmMuonTraSach_Load(object sender, EventArgs e)
        {
            try
            {
                // Nạp các ComboBox lựa chọn từ các BUS tương ứng
                NapComboBoxDocGia();
                NapComboBoxNhanVien();
                NapComboBoxSachCoSan();

                // Nạp danh sách Phiếu mượn lên bảng Master
                HienThiDanhSachPhieuMuon(pmBUS.DocDSPhieuMuon());

                // Khởi tạo ngày mượn và hạn trả theo cấu hình bảng ThamSo
                dtpNgayMuon.Value = DateTime.Today;
                CapNhatHanTraMacDinh();

                LamMoiPhieu();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 3. NẠP COMBOBOX ĐỘC GIẢ
        private void NapComboBoxDocGia()
        {
            ArrayList dsDG = docGiaBUS.DocDSDocGia();
            List<DocGiaDTO> list = new List<DocGiaDTO>();
            foreach (DocGiaDTO dg in dsDG)
            {
                list.Add(dg);
            }

            cboDocGia.DataSource = null;
            cboDocGia.DisplayMember = "HoTen";
            cboDocGia.ValueMember = "MaDG";
            cboDocGia.DataSource = list;
        }

        // 4. NẠP COMBOBOX NHÂN VIÊN
        private void NapComboBoxNhanVien()
        {
            List<NhanVienDTO> list = nhanVienBUS.DocDS();
            cboNhanVien.DataSource = null;
            cboNhanVien.DisplayMember = "HoTen";
            cboNhanVien.ValueMember = "MaNV";
            cboNhanVien.DataSource = list;
        }

        // 5. NẠP COMBOBOX SÁCH CÓ SẴN (ĐỦ ĐIỀU KIỆN MƯỢN)
        private void NapComboBoxSachCoSan()
        {
            DataTable dt = pmBUS.LayDanhSachSachCoSan();
            cboSach.DataSource = null;
            cboSach.DisplayMember = "HienThi";
            cboSach.ValueMember = "MaSach";
            cboSach.DataSource = dt;
        }

        // 6. CẬP NHẬT HẠN TRẢ THEO CẤU HÌNH THAM SỐ
        private void CapNhatHanTraMacDinh()
        {
            int soNgayMuon = pmBUS.LaySoNgayMuonToiDa();
            dtpHanTra.Value = dtpNgayMuon.Value.AddDays(soNgayMuon);
        }

        private void dtpNgayMuon_ValueChanged(object sender, EventArgs e)
        {
            CapNhatHanTraMacDinh();
        }

        // 7. HIỂN THỊ DANH SÁCH BẢNG MASTER (PHIẾU MƯỢN)
        private void HienThiDanhSachPhieuMuon(ArrayList danhSach)
        {
            dgvPhieuMuon.AutoGenerateColumns = false;
            dgvPhieuMuon.DataSource = null;

            List<PhieuMuonDTO> list = new List<PhieuMuonDTO>();
            foreach (PhieuMuonDTO item in danhSach)
            {
                list.Add(item);
            }

            dgvPhieuMuon.DataSource = list;
        }

        // 8. SỰ KIỆN CLICK CHỌN 1 PHIẾU MƯỢN (LIÊN KẾT MAIN - SUB)
        private void dgvPhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvPhieuMuon.Rows.Count) return;

                DataGridViewRow row = dgvPhieuMuon.Rows[e.RowIndex];
                maPMDangChon = row.Cells["colMaPM"].Value?.ToString() ?? string.Empty;

                txtMaPM.Text = maPMDangChon;
                txtMaPM.ReadOnly = true;

                if (row.DataBoundItem is PhieuMuonDTO pm)
                {
                    cboDocGia.SelectedValue = pm.MaDocGia;
                    cboNhanVien.SelectedValue = pm.MaNV;
                    dtpNgayMuon.Value = pm.NgayMuon;
                    dtpHanTra.Value = pm.HanTra;
                }

                // CẬP NHẬT TIÊU ĐỀ VÀ NẠP DANH SÁCH BẢNG CON (DETAIL)
                lblTieuDeChiTiet.Text = $"DANH SÁCH CUỐN SÁCH CỦA PHIẾU MƯỢN: {maPMDangChon}";
                NapBangChiTiet(maPMDangChon);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 9. NẠP DỮ LIỆU BẢNG DETAIL (CHI TIẾT SÁCH MƯỢN)
        private void NapBangChiTiet(string maPM)
        {
            List<ChiTietPhieuMuonDTO> list = pmBUS.LayChiTietTheoPhieu(maPM);
            dgvChiTietMuon.AutoGenerateColumns = false;
            dgvChiTietMuon.DataSource = null;
            dgvChiTietMuon.DataSource = list;
        }

        // 10. THÊM PHIẾU MƯỢN MỚI (MASTER)
        private void btnThemPM_Click(object sender, EventArgs e)
        {
            try
            {
                string maPM = txtMaPM.Text.Trim();
                string maDG = cboDocGia.SelectedValue?.ToString() ?? string.Empty;
                string maNV = cboNhanVien.SelectedValue?.ToString() ?? string.Empty;
                DateTime ngayMuon = dtpNgayMuon.Value;
                DateTime hanTra = dtpHanTra.Value;

                string? tenDG = cboDocGia.Text;
                string? tenNV = cboNhanVien.Text;

                PhieuMuonDTO pm = new PhieuMuonDTO(maPM, maDG, maNV, ngayMuon, hanTra, tenDG, tenNV);

                if (pmBUS.ThemPhieuMuon(pm, out string error))
                {
                    MessageBox.Show($"Tạo phiếu mượn '{maPM}' thành công!\nBây giờ bạn có thể chọn các cuốn sách ở bảng dưới để thêm vào phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachPhieuMuon(pmBUS.dsPhieuMuon_arr);
                    maPMDangChon = maPM;
                    lblTieuDeChiTiet.Text = $"DANH SÁCH CUỐN SÁCH CỦA PHIẾU MƯỢN: {maPM}";
                    NapBangChiTiet(maPM);
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

        // 11. THÊM CUỐN SÁCH VÀO PHIẾU MƯỢN (DETAIL - CẬP NHẬT TỒN KHO)
        private void btnThemSach_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPMDangChon))
                {
                    MessageBox.Show("Vui lòng chọn 1 phiếu mượn ở bảng trên trước khi thêm sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboSach.SelectedValue == null)
                {
                    MessageBox.Show("Hiện không còn cuốn sách nào có sẵn trong kho để mượn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maSach = cboSach.SelectedValue.ToString() ?? string.Empty;
                string tinhTrang = string.IsNullOrWhiteSpace(txtTinhTrang.Text) ? "Tốt" : txtTinhTrang.Text.Trim();

                ChiTietPhieuMuonDTO ct = new ChiTietPhieuMuonDTO(maPMDangChon, maSach, tinhTrang, null);

                // Gọi BUS thực thi nghiệp vụ (kiểm tra giới hạn số lượng và trừ tồn kho DauSach)
                if (pmBUS.ThemChiTietMuon(ct, out string error))
                {
                    MessageBox.Show($"Đã mượn cuốn sách '{maSach}' thành công!\n(Tồn kho đầu sách đã tự động giảm 1 và tăng số lượng cho mượn).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Nạp lại dữ liệu bảng Detail và ComboBox sách có sẵn
                    NapBangChiTiet(maPMDangChon);
                    NapComboBoxSachCoSan();

                    // Cập nhật lại bảng Master để thấy số lượng sách thay đổi
                    HienThiDanhSachPhieuMuon(pmBUS.dsPhieuMuon_arr);
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 12. NÚT TRẢ SÁCH (HOÀN TRẢ TỒN KHO + KIỂM TRA TRỄ HẠN)
        private void btnTraSach_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPMDangChon))
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn ở bảng trên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvChiTietMuon.CurrentRow == null || dgvChiTietMuon.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn cuốn sách cần trả trong bảng chi tiết ở dưới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow row = dgvChiTietMuon.CurrentRow;
                string maSach = row.Cells["colMaSach"].Value?.ToString() ?? string.Empty;
                object? ngayThucTra = row.Cells["colNgayThucTra"].Value;

                if (ngayThucTra != null && !string.IsNullOrWhiteSpace(ngayThucTra.ToString()))
                {
                    MessageBox.Show($"Cuốn sách '{maSach}' này đã được trả trước đó rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Xác nhận ghi nhận TRẢ cuốn sách '{maSach}' vào ngày hôm nay?",
                    "Xác nhận trả sách",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                DateTime ngayTra = DateTime.Today;
                string tinhTrangTra = "Đã trả";

                // Gọi BUS xử lý trả sách: cộng lại SLConLai, trừ SLChoMuon, kiểm tra quá hạn
                if (pmBUS.TraSach(maPMDangChon, maSach, ngayTra, tinhTrangTra, out int soNgayTre, out string message))
                {
                    MessageBoxIcon icon = soNgayTre > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                    MessageBox.Show(message, "Kết quả trả sách", MessageBoxButtons.OK, icon);

                    // Làm mới giao diện
                    NapBangChiTiet(maPMDangChon);
                    NapComboBoxSachCoSan();
                    HienThiDanhSachPhieuMuon(pmBUS.dsPhieuMuon_arr);
                }
                else
                {
                    MessageBox.Show(message, "Lỗi trả sách", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 13. XÓA PHIẾU MƯỢN (CHỐNG LỦNG CSDL)
        private void btnXoaPM_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPMDangChon))
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn cần xóa từ bảng trên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phiếu mượn '{maPMDangChon}' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                if (pmBUS.XoaPhieuMuon(maPMDangChon, out string error))
                {
                    MessageBox.Show("Xóa phiếu mượn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachPhieuMuon(pmBUS.dsPhieuMuon_arr);
                    LamMoiPhieu();
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

        // 14. LÀM MỚI PHIẾU
        private void btnLamMoiPM_Click(object sender, EventArgs e)
        {
            try
            {
                LamMoiPhieu();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoiPhieu()
        {
            maPMDangChon = null;
            txtMaPM.Text = pmBUS.TaoMaPhieuTiepTheo();
            txtMaPM.ReadOnly = false;
            dtpNgayMuon.Value = DateTime.Today;
            CapNhatHanTraMacDinh();

            if (cboDocGia.Items.Count > 0) cboDocGia.SelectedIndex = 0;
            if (cboNhanVien.Items.Count > 0) cboNhanVien.SelectedIndex = 0;

            lblTieuDeChiTiet.Text = "DANH SÁCH CUỐN SÁCH CỦA PHIẾU ĐANG CHỌN";
            dgvChiTietMuon.DataSource = null;
            txtTinhTrang.Text = "Tốt";

            txtMaPM.Focus();
        }

        // 15. TÌM KIẾM PHIẾU MƯỢN TRÊN RAM
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                ArrayList ketQua = pmBUS.TimKiem(keyword);
                HienThiDanhSachPhieuMuon(ketQua);
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

        // 16. HIỆN TẤT CẢ
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSachPhieuMuon(pmBUS.dsPhieuMuon_arr);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 17. XUẤT PHIẾU MƯỢN RA FILE PDF
        private void btnXuatPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPMDangChon))
                {
                    MessageBox.Show("Vui lòng chọn 1 phiếu mượn từ danh sách để xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PhieuMuonDTO? pm = pmBUS.TimTheoMa(maPMDangChon);
                if (pm == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiếu mượn cần xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<ChiTietPhieuMuonDTO> chiTiets = pmBUS.LayChiTietTheoPhieu(maPMDangChon);
                if (chiTiets.Count == 0)
                {
                    MessageBox.Show("Phiếu mượn này chưa có cuốn sách nào, không thể xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Tài liệu PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"PhieuMuon_{pm.MaPM}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        PdfService.XuatPhieuMuonPdf(pm, chiTiets, sfd.FileName);

                        DialogResult open = MessageBox.Show(
                            "Xuất file PDF phiếu mượn thành công!\nBạn có muốn mở xem ngay bây giờ không?",
                            "Xuất PDF thành công",
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
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }
    }
}
