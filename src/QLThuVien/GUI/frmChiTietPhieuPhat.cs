using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public sealed class frmChiTietPhieuPhat : Form
    {
        private readonly PhieuPhatBUS bus = new PhieuPhatBUS();
        private readonly PhieuPhatDTO phieu;
        private readonly ComboBox cboSach = new ComboBox();
        private readonly ComboBox cboQuyDinh = new ComboBox();
        private readonly TextBox txtSoNgayTre = new TextBox();
        private readonly TextBox txtDonGia = new TextBox();
        private readonly TextBox txtTienPhat = new TextBox();
        private readonly TextBox txtGhiChu = new TextBox();
        private readonly Label lblTongTien = new Label();
        private readonly DataGridView dgvChiTiet = new DataGridView();
        private readonly Button btnThem = new Button();
        private readonly Button btnSua = new Button();
        private readonly Button btnXoa = new Button();
        private readonly Button btnLamMoi = new Button();
        private readonly Button btnDong = new Button();
        private int? maCTPPDangChon;
        private bool dangNap;

        public frmChiTietPhieuPhat(PhieuPhatDTO phieu)
        {
            this.phieu = phieu ?? throw new ArgumentNullException(nameof(phieu));
            InitializeForm();
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

        private void InitializeForm()
        {
            Text = $"CHI TIẾT VI PHẠM - {phieu.MaPhieuPhat}";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(920, 620);
            ClientSize = new Size(1080, 700);
            Font = new Font("Segoe UI", 9F);

            GroupBox grp = new GroupBox { Text = "Thông tin vi phạm", Location = new Point(16, 12), Size = new Size(1048, 188), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

            grp.Controls.Add(TaoNhan("Cuốn sách:", 18, 32, 90));
            cboSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSach.Location = new Point(110, 28);
            cboSach.Size = new Size(390, 25);
            grp.Controls.Add(cboSach);

            grp.Controls.Add(TaoNhan("Quy định:", 530, 32, 85));
            cboQuyDinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyDinh.Location = new Point(620, 28);
            cboQuyDinh.Size = new Size(400, 25);
            grp.Controls.Add(cboQuyDinh);

            grp.Controls.Add(TaoNhan("Số ngày trễ:", 18, 72, 90));
            txtSoNgayTre.Location = new Point(110, 68);
            txtSoNgayTre.Size = new Size(120, 25);
            txtSoNgayTre.ReadOnly = true;
            grp.Controls.Add(txtSoNgayTre);

            grp.Controls.Add(TaoNhan("Đơn giá:", 260, 72, 65));
            txtDonGia.Location = new Point(330, 68);
            txtDonGia.Size = new Size(170, 25);
            txtDonGia.ReadOnly = true;
            txtDonGia.TextAlign = HorizontalAlignment.Right;
            grp.Controls.Add(txtDonGia);

            grp.Controls.Add(TaoNhan("Tiền phạt:", 530, 72, 85));
            txtTienPhat.Location = new Point(620, 68);
            txtTienPhat.Size = new Size(180, 25);
            txtTienPhat.ReadOnly = true;
            txtTienPhat.TextAlign = HorizontalAlignment.Right;
            grp.Controls.Add(txtTienPhat);

            grp.Controls.Add(TaoNhan("Ghi chú:", 18, 112, 90));
            txtGhiChu.Location = new Point(110, 108);
            txtGhiChu.Size = new Size(910, 25);
            txtGhiChu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grp.Controls.Add(txtGhiChu);

            btnThem.Text = "Thêm vi phạm";
            btnThem.SetBounds(18, 148, 140, 30);
            btnSua.Text = "Sửa vi phạm";
            btnSua.SetBounds(166, 148, 140, 30);
            btnXoa.Text = "Xóa vi phạm";
            btnXoa.SetBounds(314, 148, 140, 30);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.SetBounds(462, 148, 120, 30);
            grp.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.BackgroundColor = SystemColors.Window;
            dgvChiTiet.ColumnHeadersHeight = 32;
            dgvChiTiet.Location = new Point(16, 212);
            dgvChiTiet.Size = new Size(1048, 408);
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Columns.AddRange(new DataGridViewColumn[]
            {
                TaoCot(nameof(ChiTietPhieuPhatDTO.MaCTPP), "Mã vi phạm", 120),
                TaoCot("MaSach", "Mã cuốn", 110),
                TaoCot("MaQD", "Mã quy định", 120),
                TaoCot("DonGia", "Đơn giá", 100, "N0"),
                TaoCot("SoNgayTre", "Số ngày trễ", 140),
                TaoCot("GhiChu", "Ghi chú", 250),
                TaoCot("TienPhat", "Tiền phạt", 120, "N0")
            });
            dgvChiTiet.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font(Font, FontStyle.Bold);
            lblTongTien.Location = new Point(18, 642);
            lblTongTien.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDong.Text = "Đóng";
            btnDong.SetBounds(954, 634, 110, 32);
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.AddRange(new Control[] { grp, dgvChiTiet, lblTongTien, btnDong });
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
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
            if (dgvChiTiet.Rows[e.RowIndex].DataBoundItem is not ChiTietPhieuPhatDTO ct) return;

            maCTPPDangChon = ct.MaCTPP;
            cboSach.SelectedValue = ct.MaSach;
            cboQuyDinh.SelectedValue = ct.MaQD;
            txtGhiChu.Text = ct.GhiChu ?? string.Empty;
            CapNhatTienDuKien();
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
            lblTongTien.Text = $"Tổng tiền phạt: {bus.CapNhatTongTienPhieu(phieu.MaPhieuPhat):N0}";
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

        private static Label TaoNhan(string text, int x, int y, int width, int height = 22, float size = 9F, bool bold = false)
        {
            return new Label
            {
                AutoSize = false,
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular)
            };
        }

        private static DataGridViewTextBoxColumn TaoCot(string property, string header, int width, string? format = null)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = header,
                Width = width,
                DefaultCellStyle = { Format = format ?? string.Empty }
            };
        }

        private static void BaoLoi(Exception ex)
        {
            FormErrors.Show(ex);
        }
    }
}
