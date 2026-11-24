using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmPhieuPhat : Form
    {
        private readonly PhieuPhatBUS busPhieu = new PhieuPhatBUS();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly PhieuPhatBaoCaoBUS busBaoCao = new PhieuPhatBaoCaoBUS();
        private List<LookupOptionDTO> danhSachPhieuMuon = new List<LookupOptionDTO>();
        private string maPhieuDangChon = string.Empty;
        private int? maCTPPDangChon;
        private bool dangNapDuLieu;
        private bool dangNapChiTiet;

        // Dữ liệu gợi ý khi mở nhanh từ màn hình Trả sách
        private string? pendingMaDocGia;
        private string? pendingMaPhieuMuon;
        private string? pendingMaSach;
        private string? pendingMaQD;

        public frmPhieuPhat()
        {
            InitializeComponent();
            Load += frmPhieuPhat_Load;
            cboDocGia.SelectedValueChanged += cboDocGia_SelectedValueChanged;
            btnChonDocGia.Click += btnChonDocGia_Click;
            btnChonNhanVien.Click += btnChonNhanVien_Click;
            dgvPhieuPhat.CellClick += dgvPhieuPhat_CellClick;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            btnThemPhieu.Click += btnThemPhieu_Click;
            btnSuaPhieu.Click += btnSuaPhieu_Click;
            btnXoaPhieu.Click += btnXoaPhieu_Click;
            btnChiTiet.Click += btnChiTiet_Click;
            btnXuatPdf.Click += btnXuatPdf_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            btnTatCa.Click += btnTatCa_Click;

            // Sự kiện vùng Detail (Main - Sub trên cùng 1 Form)
            cboSachDetail.SelectedValueChanged += (_, _) => CapNhatTienDuKien();
            cboQuyDinhDetail.SelectedValueChanged += (_, _) => CapNhatTienDuKien();
            dgvChiTiet.CellClick += dgvChiTiet_CellClick;
            btnThemViPham.Click += btnThemViPham_Click;
            btnSuaViPham.Click += btnSuaViPham_Click;
            btnXoaViPham.Click += btnXoaViPham_Click;
            btnLamMoiViPham.Click += (_, _) => LamMoiVungChiTiet(giuDanhSach: true);
        }

        public void ChuanBiLapPhieuNhanh(string maDocGia, string maPhieuMuon, string maSach, string? goiYMaQD = null)
        {
            pendingMaDocGia = maDocGia;
            pendingMaPhieuMuon = maPhieuMuon;
            pendingMaSach = maSach;
            pendingMaQD = goiYMaQD;

            if (IsHandleCreated)
            {
                ApDungDuLieuLapNhanh();
            }
        }

        private void frmPhieuPhat_Load(object? sender, EventArgs e)
        {
            try
            {
                dangNapDuLieu = true;
                cboDocGia.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboDocGia.ValueMember = nameof(LookupOptionDTO.Ma);
                cboDocGia.DataSource = busPhieu.LayDanhSachDocGiaLookup();

                danhSachPhieuMuon = busPhieu.LayDanhSachPhieuMuonLookup();
                cboNhanVien.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboNhanVien.ValueMember = nameof(LookupOptionDTO.Ma);
                cboNhanVien.DataSource = busNhanVien.LayDanhSach(reload: true)
                    .Select(nv => new LookupOptionDTO(nv.MaNV, $"{nv.MaNV} - {nv.HoTen}"))
                    .ToList();

                cboQuyDinhDetail.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboQuyDinhDetail.ValueMember = nameof(LookupOptionDTO.Ma);
                cboQuyDinhDetail.DataSource = busPhieu.LayDanhSachQuyDinh()
                    .Select(qd => new LookupOptionDTO(qd.MaQD, $"{qd.MaQD} - {qd.TenQD}"))
                    .ToList();

                HienThiDanhSach(busPhieu.DocDS());
                dangNapDuLieu = false;
                LamMoiForm();
                ApDungDuLieuLapNhanh();
            }
            catch (Exception ex)
            {
                dangNapDuLieu = false;
                HienThiLoi(ex);
            }
        }

        private void ApDungDuLieuLapNhanh()
        {
            if (string.IsNullOrWhiteSpace(pendingMaDocGia)) return;

            dangNapDuLieu = true;
            cboDocGia.SelectedValue = pendingMaDocGia;
            NapDanhSachPhieuMuon(pendingMaDocGia, pendingMaPhieuMuon);
            if (cboNhanVien.Items.Count > 0 && cboNhanVien.SelectedIndex < 0)
            {
                cboNhanVien.SelectedIndex = 0;
            }
            dangNapDuLieu = false;
        }

        private void cboDocGia_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (dangNapDuLieu) return;
            NapDanhSachPhieuMuon(GiaTri(cboDocGia));
        }

        private void btnChonDocGia_Click(object? sender, EventArgs e)
        {
            try
            {
                frmTimKiemDocGiaPopup.SelectedMaDocGia = string.Empty;
                frmTimKiemDocGiaPopup.SelectedTenDocGia = string.Empty;
                using var popup = new frmTimKiemDocGiaPopup();
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmTimKiemDocGiaPopup.SelectedMaDocGia))
                    {
                        cboDocGia.SelectedValue = frmTimKiemDocGiaPopup.SelectedMaDocGia;
                        cboDocGia_SelectedValueChanged(cboDocGia, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnChonNhanVien_Click(object? sender, EventArgs e)
        {
            try
            {
                FormSharedData.SelectedMaNV = null;
                FormSharedData.SelectedTenNV = null;
                using var popup = new frmChonNhanVienPopup();
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(FormSharedData.SelectedMaNV))
                    {
                        cboNhanVien.SelectedValue = FormSharedData.SelectedMaNV;
                    }
                }
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void NapDanhSachPhieuMuon(string? maDocGia, string? maPhieuMuon = null)
        {
            List<LookupOptionDTO> phieuMuon = string.IsNullOrWhiteSpace(maDocGia)
                ? new List<LookupOptionDTO>()
                : danhSachPhieuMuon.Where(pm => string.Equals(pm.MaDocGia, maDocGia, StringComparison.OrdinalIgnoreCase)).ToList();

            cboPhieuMuon.DataSource = null;
            cboPhieuMuon.DisplayMember = nameof(LookupOptionDTO.HienThi);
            cboPhieuMuon.ValueMember = nameof(LookupOptionDTO.Ma);
            cboPhieuMuon.DataSource = phieuMuon;
            cboPhieuMuon.SelectedIndex = -1;
            if (!string.IsNullOrWhiteSpace(maPhieuMuon)) cboPhieuMuon.SelectedValue = maPhieuMuon;
        }

        private void HienThiDanhSach(List<PhieuPhatDTO> danhSach)
        {
            dgvPhieuPhat.DataSource = null;
            dgvPhieuPhat.DataSource = danhSach;
            dgvPhieuPhat.ClearSelection();
            btnChiTiet.Enabled = false;
        }

        private void dgvPhieuPhat_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvPhieuPhat.Rows.Count) return;
                if (dgvPhieuPhat.Rows[e.RowIndex].DataBoundItem is PhieuPhatDTO pp) NapPhieuLenForm(pp);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void NapPhieuLenForm(PhieuPhatDTO pp)
        {
            dangNapDuLieu = true;
            maPhieuDangChon = pp.MaPhieuPhat;
            txtMaPhieuPhat.Text = pp.MaPhieuPhat;
            cboDocGia.SelectedValue = pp.MaDocGia;
            NapDanhSachPhieuMuon(pp.MaDocGia, pp.MaPhieuMuon);
            cboNhanVien.SelectedValue = pp.MaNV;
            dtpNgayLap.Value = pp.NgayLap;
            cboTrangThai.SelectedItem = pp.TrangThai;
            txtTongTienPhat.Text = pp.TongTienPhat.ToString("N0");
            btnChiTiet.Enabled = true;
            dangNapDuLieu = false;

            // Nạp ngay phần Detail (Main - Sub) của phiếu vừa chọn
            NapVungChiTietTheoPhieu(pp);
        }

        private void NapVungChiTietTheoPhieu(PhieuPhatDTO pp)
        {
            dangNapChiTiet = true;
            maCTPPDangChon = null;
            grpChiTiet.Text = $"2. Chi tiết vi phạm của Phiếu phạt: {pp.MaPhieuPhat} (Detail)";
            grpChiTiet.Enabled = true;

            cboSachDetail.DisplayMember = nameof(SachMuonDTO.HienThi);
            cboSachDetail.ValueMember = nameof(SachMuonDTO.MaSach);
            cboSachDetail.DataSource = busPhieu.LaySachTheoPhieuMuon(pp.MaPhieuMuon ?? string.Empty);

            if (cboSachDetail.Items.Count > 0) cboSachDetail.SelectedIndex = 0;
            if (cboQuyDinhDetail.Items.Count > 0) cboQuyDinhDetail.SelectedIndex = 0;
            txtGhiChuDetail.Clear();

            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = busPhieu.LayChiTietTheoPhieu(pp.MaPhieuPhat);
            dgvChiTiet.ClearSelection();
            dangNapChiTiet = false;
            CapNhatTienDuKien();
        }

        private void LamMoiVungChiTiet(bool giuDanhSach)
        {
            maCTPPDangChon = null;
            if (cboSachDetail.Items.Count > 0) cboSachDetail.SelectedIndex = 0;
            if (cboQuyDinhDetail.Items.Count > 0) cboQuyDinhDetail.SelectedIndex = 0;
            txtGhiChuDetail.Clear();
            dgvChiTiet.ClearSelection();
            if (!giuDanhSach)
            {
                grpChiTiet.Text = "2. Chi tiết vi phạm của Phiếu phạt đang chọn (Detail)";
                cboSachDetail.DataSource = null;
                dgvChiTiet.DataSource = null;
                grpChiTiet.Enabled = false;
            }
            CapNhatTienDuKien();
        }

        private void dgvChiTiet_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
                if (dgvChiTiet.Rows[e.RowIndex].DataBoundItem is not ChiTietPhieuPhatDTO ct) return;

                maCTPPDangChon = ct.MaCTPP;
                cboSachDetail.SelectedValue = ct.MaSach;
                cboQuyDinhDetail.SelectedValue = ct.MaQD;
                txtGhiChuDetail.Text = ct.GhiChu ?? string.Empty;
                CapNhatTienDuKien();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void CapNhatTienDuKien()
        {
            if (dangNapChiTiet || string.IsNullOrWhiteSpace(maPhieuDangChon) ||
                cboSachDetail.SelectedValue == null || cboQuyDinhDetail.SelectedValue == null)
            {
                txtSoNgayTreDetail.Clear();
                txtDonGiaDetail.Clear();
                txtTienPhatDetail.Clear();
                return;
            }

            try
            {
                string maSach = cboSachDetail.SelectedValue.ToString() ?? string.Empty;
                string maQD = cboQuyDinhDetail.SelectedValue.ToString() ?? string.Empty;
                ChiTietPhieuPhatDTO ct = busPhieu.TinhChiTietPhat(maPhieuDangChon, maQD, maSach, txtGhiChuDetail.Text);
                txtSoNgayTreDetail.Text = ct.SoNgayTre?.ToString() ?? string.Empty;
                txtDonGiaDetail.Text = ct.DonGia.ToString("N0");
                txtTienPhatDetail.Text = ct.TienPhat.ToString("N0");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                txtSoNgayTreDetail.Clear();
                txtDonGiaDetail.Clear();
                txtTienPhatDetail.Clear();
            }
        }

        private void btnThemViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new BusinessRuleException("Vui lòng chọn 1 phiếu phạt ở bảng trên trước khi thêm vi phạm.");
                string maSach = cboSachDetail.SelectedValue?.ToString() ?? string.Empty;
                string maQD = cboQuyDinhDetail.SelectedValue?.ToString() ?? string.Empty;
                ChiTietPhieuPhatDTO ct = busPhieu.TinhChiTietPhat(maPhieuDangChon, maQD, maSach, txtGhiChuDetail.Text);
                if (!busPhieu.ThemChiTiet(ct)) throw new InvalidOperationException("Không thể thêm vi phạm.");
                DongBoSauThayDoiChiTiet(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnSuaViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon) || !maCTPPDangChon.HasValue)
                    throw new BusinessRuleException("Hãy chọn vi phạm cần sửa ở bảng dưới.");
                string maSach = cboSachDetail.SelectedValue?.ToString() ?? string.Empty;
                string maQD = cboQuyDinhDetail.SelectedValue?.ToString() ?? string.Empty;
                ChiTietPhieuPhatDTO ct = busPhieu.TinhChiTietPhat(maPhieuDangChon, maQD, maSach, txtGhiChuDetail.Text);
                if (!busPhieu.SuaChiTiet(maCTPPDangChon.Value, ct)) throw new InvalidOperationException("Không thể sửa vi phạm.");
                DongBoSauThayDoiChiTiet(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnXoaViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon) || !maCTPPDangChon.HasValue)
                {
                    MessageBox.Show("Hãy chọn vi phạm cần xóa ở bảng dưới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Xóa vi phạm đang chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                if (!busPhieu.XoaChiTiet(maCTPPDangChon.Value)) throw new InvalidOperationException("Không thể xóa vi phạm.");
                DongBoSauThayDoiChiTiet(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void DongBoSauThayDoiChiTiet(string maPhieu)
        {
            HienThiDanhSach(busPhieu.DocDS());
            ChonDongPhieuPhat(maPhieu);
        }

        private void ChonDongPhieuPhat(string maPhieu)
        {
            PhieuPhatDTO? phieuMoi = busPhieu.LayDanhSach().FirstOrDefault(item =>
                string.Equals(item.MaPhieuPhat, maPhieu, StringComparison.OrdinalIgnoreCase));
            if (phieuMoi == null)
            {
                LamMoiForm();
                return;
            }

            foreach (DataGridViewRow row in dgvPhieuPhat.Rows)
            {
                if (row.DataBoundItem is PhieuPhatDTO rowPhieu &&
                    string.Equals(rowPhieu.MaPhieuPhat, maPhieu, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    NapPhieuLenForm(phieuMoi);
                    break;
                }
            }
        }

        private void btnThemPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                PhieuPhatDTO pp = TaoPhieuTuForm();
                if (!busPhieu.ThemPhieu(pp)) throw new InvalidOperationException("Không thể thêm phiếu phạt.");

                string maVuaTao = pp.MaPhieuPhat;
                HienThiDanhSach(busPhieu.DocDS());
                ChonDongPhieuPhat(maVuaTao);

                if (!string.IsNullOrWhiteSpace(pendingMaSach))
                {
                    cboSachDetail.SelectedValue = pendingMaSach;
                    if (!string.IsNullOrWhiteSpace(pendingMaQD))
                    {
                        cboQuyDinhDetail.SelectedValue = pendingMaQD;
                    }
                    pendingMaDocGia = null;
                    pendingMaPhieuMuon = null;
                    pendingMaSach = null;
                    pendingMaQD = null;
                    CapNhatTienDuKien();
                }
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnSuaPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new BusinessRuleException("Hãy chọn phiếu phạt cần sửa.");

                PhieuPhatDTO pp = TaoPhieuTuForm();
                pp.MaPhieuPhat = maPhieuDangChon;
                pp.TongTienPhat = 0;
                if (!busPhieu.SuaPhieu(pp)) throw new InvalidOperationException("Không thể cập nhật phiếu phạt.");
                TaiLaiVaLamMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnXoaPhieu_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDangChon))
            {
                MessageBox.Show("Hãy chọn phiếu phạt cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Xóa phiếu {maPhieuDangChon} và các vi phạm của phiếu?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (!busPhieu.XoaPhieu(maPhieuDangChon)) throw new InvalidOperationException("Không thể xóa phiếu phạt.");
                TaiLaiVaLamMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnChiTiet_Click(object? sender, EventArgs e)
        {
            try
            {
                PhieuPhatDTO? pp = busPhieu.LayDanhSach().FirstOrDefault(item =>
                    string.Equals(item.MaPhieuPhat, maPhieuDangChon, StringComparison.OrdinalIgnoreCase));
                if (pp == null)
                {
                    MessageBox.Show("Hãy chọn phiếu phạt cần mở chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (frmChiTietPhieuPhat formChiTiet = new frmChiTietPhieuPhat(pp))
                {
                    formChiTiet.ShowDialog(this);
                }

                DongBoSauThayDoiChiTiet(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            TaiLaiVaLamMoi();
        }

        private void btnXuatPdf_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDangChon))
            {
                MessageBox.Show("Hãy chọn phiếu phạt cần xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PhieuPhatBaoCaoDTO report = busBaoCao.LayTheoMa(maPhieuDangChon);
                using SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "pdf",
                    FileName = PhieuPhatBaoCaoBUS.TenFileGoiY(report.MaPhieuPhat),
                    Filter = "Tệp PDF (*.pdf)|*.pdf",
                    OverwritePrompt = true
                };
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                new PhieuPhatPdfExporter(report).Xuat(dialog.FileName);
                Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(PdfExportErrors.ToVietnamese(ex), "Không thể xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                HienThiDanhSach(busPhieu.TimKiemPhieu(txtTimKiem.Text));
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void btnTatCa_Click(object? sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(busPhieu.LayDanhSach(reload: true));
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnTimKiem_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private PhieuPhatDTO TaoPhieuTuForm()
        {
            return new PhieuPhatDTO(
                txtMaPhieuPhat.Text,
                GiaTri(cboDocGia) ?? string.Empty,
                GiaTri(cboPhieuMuon),
                GiaTri(cboNhanVien) ?? string.Empty,
                dtpNgayLap.Value,
                cboTrangThai.SelectedItem?.ToString() ?? string.Empty,
                0);
        }

        private void TaiLaiVaLamMoi()
        {
            HienThiDanhSach(busPhieu.DocDS());
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            dangNapDuLieu = true;
            maPhieuDangChon = string.Empty;
            txtMaPhieuPhat.Text = busPhieu.TaoMaPhieuTiepTheo();
            cboDocGia.SelectedIndex = -1;
            NapDanhSachPhieuMuon(null);
            cboNhanVien.SelectedIndex = -1;
            dtpNgayLap.Value = DateTime.Today;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            txtTongTienPhat.Text = "0";
            txtTimKiem.Clear();
            dgvPhieuPhat.ClearSelection();
            btnChiTiet.Enabled = false;
            LamMoiVungChiTiet(giuDanhSach: false);
            dangNapDuLieu = false;
        }

        private static string? GiaTri(ComboBox combo)
        {
            return combo.SelectedValue?.ToString();
        }

        private static void HienThiLoi(Exception ex)
        {
            FormErrors.Show(ex);
        }
    }
}
