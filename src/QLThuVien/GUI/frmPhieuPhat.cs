using System;
using System.Collections.Generic;
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
        private List<LookupOptionDTO> danhSachPhieuMuon = new List<LookupOptionDTO>();
        private string maPhieuDangChon = string.Empty;
        private bool dangNapDuLieu;

        public frmPhieuPhat()
        {
            InitializeComponent();
            Load += frmPhieuPhat_Load;
            cboDocGia.SelectedValueChanged += cboDocGia_SelectedValueChanged;
            dgvPhieuPhat.CellClick += dgvPhieuPhat_CellClick;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            btnThemPhieu.Click += btnThemPhieu_Click;
            btnSuaPhieu.Click += btnSuaPhieu_Click;
            btnXoaPhieu.Click += btnXoaPhieu_Click;
            btnChiTiet.Click += btnChiTiet_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            btnTatCa.Click += btnTatCa_Click;
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

                HienThiDanhSach(busPhieu.DocDS());
                dangNapDuLieu = false;
                LamMoiForm();
            }
            catch (Exception ex)
            {
                dangNapDuLieu = false;
                MessageBox.Show(ex.Message, "Không thể tải dữ liệu phiếu phạt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboDocGia_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (dangNapDuLieu) return;
            NapDanhSachPhieuMuon(GiaTri(cboDocGia));
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
            if (e.RowIndex < 0 || e.RowIndex >= dgvPhieuPhat.Rows.Count) return;
            if (dgvPhieuPhat.Rows[e.RowIndex].DataBoundItem is PhieuPhatDTO pp) NapPhieuLenForm(pp);
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
        }

        private void btnThemPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                PhieuPhatDTO pp = TaoPhieuTuForm();
                if (!busPhieu.ThemPhieu(pp)) throw new InvalidOperationException("Không thể thêm phiếu phạt.");
                TaiLaiVaLamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thêm phiếu phạt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSuaPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new InvalidOperationException("Hãy chọn phiếu phạt cần sửa trong danh sách.");

                PhieuPhatDTO pp = TaoPhieuTuForm();
                pp.MaPhieuPhat = maPhieuDangChon;
                pp.TongTienPhat = 0;
                if (!busPhieu.SuaPhieu(pp)) throw new InvalidOperationException("Không thể cập nhật phiếu phạt.");
                TaiLaiVaLamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể sửa phiếu phạt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoaPhieu_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDangChon))
            {
                MessageBox.Show("Hãy chọn phiếu phạt cần xóa trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Xóa phiếu '{maPhieuDangChon}' và toàn bộ vi phạm của phiếu?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (!busPhieu.XoaPhieu(maPhieuDangChon)) throw new InvalidOperationException("Không thể xóa phiếu phạt.");
                TaiLaiVaLamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa phiếu phạt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChiTiet_Click(object? sender, EventArgs e)
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

            string maCanChon = maPhieuDangChon;
            HienThiDanhSach(busPhieu.DocDS());
            PhieuPhatDTO? phieuMoi = busPhieu.LayDanhSach().FirstOrDefault(item =>
                string.Equals(item.MaPhieuPhat, maCanChon, StringComparison.OrdinalIgnoreCase));
            if (phieuMoi != null)
            {
                foreach (DataGridViewRow row in dgvPhieuPhat.Rows)
                {
                    if (row.DataBoundItem is PhieuPhatDTO rowPhieu &&
                        string.Equals(rowPhieu.MaPhieuPhat, maCanChon, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        NapPhieuLenForm(phieuMoi);
                        break;
                    }
                }
            }
            else
            {
                LamMoiForm();
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            TaiLaiVaLamMoi();
        }

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            HienThiDanhSach(busPhieu.TimKiemPhieu(txtTimKiem.Text));
        }

        private void btnTatCa_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            HienThiDanhSach(busPhieu.LayDanhSach(reload: true));
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
            dangNapDuLieu = false;
        }

        private static string? GiaTri(ComboBox combo)
        {
            return combo.SelectedValue?.ToString();
        }
    }
}
