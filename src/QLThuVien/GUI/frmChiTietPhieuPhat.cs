using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmChiTietPhieuPhat : Form
    {
        private readonly PhieuPhatBUS bus = new PhieuPhatBUS();
        private readonly PhieuPhatDTO phieu;
        private int? maCTPPDangChon;
        private bool dangNap;

        public frmChiTietPhieuPhat(PhieuPhatDTO phieu)
        {
            this.phieu = phieu ?? throw new ArgumentNullException(nameof(phieu));
            InitializeComponent();

            Text = $"CHI TIẾT VI PHẠM - {phieu.MaPhieuPhat}";
            lblTitle.Text = $"CHI TIẾT VI PHẠM - {phieu.MaPhieuPhat}";

            Load += Form_Load;
            cboSach.SelectedValueChanged += (_, _) => CapNhatTienDuKien();
            cboQuyDinh.SelectedValueChanged += (_, _) => CapNhatTienDuKien();
            dgvChiTiet.CellClick += DgvChiTiet_CellClick;

            btnThem.Click += (_, _) => ThucHien(() =>
            {
                bus.ThemChiTiet(TaoChiTiet());
                NapDanhSachVaLamMoi();
            });

            btnSua.Click += (_, _) => ThucHien(() =>
            {
                if (!maCTPPDangChon.HasValue) throw new BusinessRuleException("Hãy chọn vi phạm cần sửa.");
                bus.SuaChiTiet(maCTPPDangChon.Value, TaoChiTiet());
                NapDanhSachVaLamMoi();
            });

            btnXoa.Click += (_, _) => XoaChiTiet();
            btnLamMoi.Click += (_, _) => NapDanhSachVaLamMoi();
            btnDong.Click += (_, _) => Close();
        }

        private void Form_Load(object? sender, EventArgs e)
        {
            try
            {
                dangNap = true;
                bus.DocDS();
                cboSach.DisplayMember = nameof(SachMuonDTO.HienThi);
                cboSach.ValueMember = nameof(SachMuonDTO.MaSach);
                cboSach.DataSource = bus.LaySachTheoPhieuMuon(phieu.MaPhieuMuon ?? string.Empty);

                List<QuyDinhPhatDTO> quyDinhs = bus.LayDanhSachQuyDinh();
                cboQuyDinh.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboQuyDinh.ValueMember = nameof(LookupOptionDTO.Ma);
                cboQuyDinh.DataSource = quyDinhs
                    .Select(qd => new LookupOptionDTO(qd.MaQD, $"{qd.MaQD} - {qd.TenQD}"))
                    .ToList();

                cboSach.SelectedIndex = cboSach.Items.Count == 0 ? -1 : 0;
                cboQuyDinh.SelectedIndex = cboQuyDinh.Items.Count == 0 ? -1 : 0;
                dangNap = false;
                NapDanhSachVaLamMoi();
            }
            catch (Exception ex)
            {
                dangNap = false;
                BaoLoi(ex);
            }
        }

        private void DgvChiTiet_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
                if (dgvChiTiet.Rows[e.RowIndex].DataBoundItem is not ChiTietPhieuPhatDTO ct) return;

                maCTPPDangChon = ct.MaCTPP;
                cboSach.SelectedValue = ct.MaSach;
                cboQuyDinh.SelectedValue = ct.MaQD;
                txtGhiChu.Text = ct.GhiChu ?? string.Empty;
                CapNhatTienDuKien();
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        private ChiTietPhieuPhatDTO TaoChiTiet()
        {
            string maSach = cboSach.SelectedValue?.ToString() ?? string.Empty;
            string maQD = cboQuyDinh.SelectedValue?.ToString() ?? string.Empty;
            return bus.TinhChiTietPhat(phieu.MaPhieuPhat, maQD, maSach, txtGhiChu.Text);
        }

        private void CapNhatTienDuKien()
        {
            if (dangNap || cboSach.SelectedValue == null || cboQuyDinh.SelectedValue == null)
            {
                txtSoNgayTre.Clear();
                txtDonGia.Clear();
                txtTienPhat.Clear();
                return;
            }

            try
            {
                ChiTietPhieuPhatDTO ct = TaoChiTiet();
                txtSoNgayTre.Text = ct.SoNgayTre?.ToString() ?? string.Empty;
                txtDonGia.Text = ct.DonGia.ToString("N0");
                txtTienPhat.Text = ct.TienPhat.ToString("N0");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                txtSoNgayTre.Clear();
                txtDonGia.Clear();
                txtTienPhat.Clear();
            }
        }

        private void XoaChiTiet()
        {
            try
            {
                if (!maCTPPDangChon.HasValue)
                {
                    MessageBox.Show("Hãy chọn vi phạm cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show("Xóa vi phạm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                ThucHien(() =>
                {
                    bus.XoaChiTiet(maCTPPDangChon.Value);
                    NapDanhSachVaLamMoi();
                });
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        private void ThucHien(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        private void NapDanhSachVaLamMoi()
        {
            List<ChiTietPhieuPhatDTO> chiTiet = bus.LayChiTietTheoPhieu(phieu.MaPhieuPhat);
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = chiTiet;
            lblTongTien.Text = $"Tổng tiền phạt: {bus.CapNhatTongTienPhieu(phieu.MaPhieuPhat):N0} VNĐ";
            LamMoiDong();
        }

        private void LamMoiDong()
        {
            maCTPPDangChon = null;
            if (cboSach.Items.Count > 0) cboSach.SelectedIndex = 0;
            if (cboQuyDinh.Items.Count > 0) cboQuyDinh.SelectedIndex = 0;
            txtGhiChu.Clear();
            dgvChiTiet.ClearSelection();
            CapNhatTienDuKien();
        }

        private static void BaoLoi(Exception ex)
        {
            FormErrors.Show(ex);
        }
    }
}
