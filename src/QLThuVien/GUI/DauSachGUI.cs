#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public class DauSachGUI : Form
    {
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();
        private readonly SachBUS sachBUS = new SachBUS();

        private bool isBinding = false;

        private TextBox txtMaDauSach, txtTenSach, txtMaTG, txtMaTL, txtMaNXB, txtNamXB, txtSoLuong, txtDonGia;
        private TextBox txtTimKiem;

        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTimNangCao, btnTaiLai, btnLamMoi, btnXuatExcel;
        private Button btnChonTG, btnChonTL, btnChonNXB;
        private DataGridView dgvDauSach, dgvSach;
        private Label lblThongTin;
        private ComboBox cboTrangThai;

        public DauSachGUI()
        {
            InitGUI();
        }

        private void InitGUI()
        {
            Text = "Quản lý Đầu sách & Sách";
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9.5F);

            // GroupBox Nhập Liệu Căn Chỉnh Chuẩn
            GroupBox grpNhapLieu = new GroupBox
            {
                Text = "Thông tin đầu sách",
                Location = new Point(10, 5),
                Size = new Size(1160, 135),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            Font fontNormal = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // HÀNG 1: Mã đầu sách | Tên sách | Đơn giá | Số lượng
            grpNhapLieu.Controls.Add(new Label { Text = "Mã đầu sách:", Location = new Point(12, 28), AutoSize = true, Font = fontNormal });
            txtMaDauSach = new TextBox { Location = new Point(110, 25), Size = new Size(110, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaDauSach);

            grpNhapLieu.Controls.Add(new Label { Text = "Tên sách:", Location = new Point(235, 28), AutoSize = true, Font = fontNormal });
            txtTenSach = new TextBox { Location = new Point(305, 25), Size = new Size(230, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtTenSach);

            grpNhapLieu.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(550, 28), AutoSize = true, Font = fontNormal });
            txtDonGia = new TextBox { Location = new Point(615, 25), Size = new Size(125, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtDonGia);

            grpNhapLieu.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(760, 28), AutoSize = true, Font = fontNormal });
            txtSoLuong = new TextBox { Location = new Point(830, 25), Size = new Size(70, 24), Font = fontNormal, ReadOnly = true, BackColor = Color.FromArgb(238, 242, 246) };
            grpNhapLieu.Controls.Add(txtSoLuong);


            // HÀNG 2: ĐÃ CĂN TỌA ĐỘ RỘNG RÃI CHỐNG ĐÈ CHỮ
            // 1. Mã tác giả
            grpNhapLieu.Controls.Add(new Label { Text = "Mã tác giả:", Location = new Point(12, 63), AutoSize = true, Font = fontNormal });
            txtMaTG = new TextBox { Location = new Point(95, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaTG);
            btnChonTG = CreateLookupButton(173, 59);
            btnChonTG.Click += (s, e) => { using (TacGiaGUI frm = new TacGiaGUI()) frm.ShowDialog(this); };
            grpNhapLieu.Controls.Add(btnChonTG);

            // 2. Mã thể loại (Đã nới rộng nhãn sang X=215, ô TextBox sang X=300 để không che chữ T)
            grpNhapLieu.Controls.Add(new Label { Text = "Mã thể loại:", Location = new Point(205, 63), AutoSize = true, Font = fontNormal });
            txtMaTL = new TextBox { Location = new Point(300, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaTL);
            btnChonTL = CreateLookupButton(378, 59);
            btnChonTL.Click += (s, e) => { using (TheLoaiGUI frm = new TheLoaiGUI()) frm.ShowDialog(this); };
            grpNhapLieu.Controls.Add(btnChonTL);

            // 3. Mã NXB (Dời X sang 420, ô TextBox sang 485)
            grpNhapLieu.Controls.Add(new Label { Text = "Mã NXB:", Location = new Point(420, 63), AutoSize = true, Font = fontNormal });
            txtMaNXB = new TextBox { Location = new Point(485, 60), Size = new Size(75, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtMaNXB);
            btnChonNXB = CreateLookupButton(563, 59);
            btnChonNXB.Click += (s, e) => { using (NXBGUI frm = new NXBGUI()) frm.ShowDialog(this); };
            grpNhapLieu.Controls.Add(btnChonNXB);

            // 4. Năm XB (Dời X sang 605, ô TextBox sang 670)
            grpNhapLieu.Controls.Add(new Label { Text = "Năm XB:", Location = new Point(605, 63), AutoSize = true, Font = fontNormal });
            txtNamXB = new TextBox { Location = new Point(670, 60), Size = new Size(70, 24), Font = fontNormal };
            grpNhapLieu.Controls.Add(txtNamXB);


            // HÀNG 3: Nút bấm
            btnThem = new Button { Text = "Thêm", Location = new Point(12, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSua = new Button { Text = "Sửa", Location = new Point(105, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnXoa = new Button { Text = "Xóa", Location = new Point(198, 96), Size = new Size(85, 30), Font = fontNormal, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(291, 96), Size = new Size(95, 30), Font = fontNormal, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

            btnThem.FlatAppearance.BorderSize = 0;
            btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatAppearance.BorderSize = 0;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;

            grpNhapLieu.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            Controls.Add(grpNhapLieu);

            // Panel Tìm kiếm
            Panel pnlTimKiem = new Panel
            {
                Location = new Point(10, 145),
                Size = new Size(1160, 40)
            };

            pnlTimKiem.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(85, 7), Size = new Size(250, 24) };
            pnlTimKiem.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(345, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += btnTimKiem_Click;
            pnlTimKiem.Controls.Add(btnTimKiem);

            btnTimNangCao = new Button { Text = "Tìm nâng cao", Location = new Point(433, 5), Size = new Size(120, 30), BackColor = Color.FromArgb(155, 89, 182), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTimNangCao.FlatAppearance.BorderSize = 0;
            btnTimNangCao.Click += btnTimNangCao_Click;
            pnlTimKiem.Controls.Add(btnTimNangCao);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(561, 5), Size = new Size(100, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTaiLai.FlatAppearance.BorderSize = 0;
            btnTaiLai.Click += btnTaiLai_Click;
            pnlTimKiem.Controls.Add(btnTaiLai);

            btnXuatExcel = new Button { Text = "Xuất Excel (CSV)", Location = new Point(860, 5), Size = new Size(140, 30), BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.Click += btnXuatExcel_Click;
            pnlTimKiem.Controls.Add(btnXuatExcel);

            lblThongTin = new Label { Location = new Point(680, 10), AutoSize = true, ForeColor = Color.FromArgb(30, 41, 59), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            pnlTimKiem.Controls.Add(lblThongTin);

            Controls.Add(pnlTimKiem);

            // DataGridView Đầu Sách
            dgvDauSach = new DataGridView
            {
                Location = new Point(10, 190),
                Size = new Size(1160, 310),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvDauSach.SelectionChanged += dgvDauSach_SelectionChanged;
            Controls.Add(dgvDauSach);

            // Panel Lọc Cuốn Sách
            Panel pnlSach = new Panel
            {
                Location = new Point(10, 505),
                Size = new Size(1160, 30)
            };
            pnlSach.Controls.Add(new Label { Text = "Danh sách cuốn sách - lọc theo trạng thái:", Location = new Point(5, 5), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });

            cboTrangThai = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(365, 2),
                Size = new Size(160, 24)
            };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa cho mượn", "Đã cho mượn" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
            pnlSach.Controls.Add(cboTrangThai);
            Controls.Add(pnlSach);

            // DataGridView Cuốn Sách
            dgvSach = new DataGridView
            {
                Location = new Point(10, 538),
                Size = new Size(1160, 205),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(dgvSach);

            Load += DauSachGUI_Load;
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
                        popup.MaTL
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
                Filter = "CSV mở bằng Excel (*.csv)|*.csv",
                FileName = "DanhSachDauSach.csv",
                DefaultExt = "csv",
                AddExtension = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("sep=;");
                    csv.AppendLine(string.Join(";", new[] { "Mã đầu sách", "Tên sách", "Mã tác giả", "Mã thể loại", "Mã NXB", "Năm xuất bản", "Đơn giá", "Số lượng", "Đã mượn", "Còn lại" }.Select(GiaTriCsv)));

                    foreach (DauSachDTO sach in danhSach)
                    {
                        string[] cot =
                        {
                            sach.MaDauSach,
                            sach.TenSach,
                            sach.MaTG,
                            sach.MaTL,
                            sach.MaNXB,
                            sach.NamXB?.ToString(CultureInfo.InvariantCulture) ?? "",
                            sach.DonGia.ToString(CultureInfo.InvariantCulture),
                            sach.SoLuong.ToString(CultureInfo.InvariantCulture),
                            sach.SLChoMuon.ToString(CultureInfo.InvariantCulture),
                            sach.SLConLai.ToString(CultureInfo.InvariantCulture)
                        };
                        csv.AppendLine(string.Join(";", cot.Select(GiaTriCsv)));
                    }

                    File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                    MessageBox.Show($"Đã xuất {danhSach.Count} đầu sách ra file CSV mở bằng Excel.", "Xuất thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xuất file: " + ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string GiaTriCsv(string value)
        {
            string text = value ?? string.Empty;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
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

    public class frmTimKiemPopup : Form
    {
        private TextBox txtMaDauSach, txtTenSach, txtSoLuong, txtNamXB, txtMaTG, txtMaTL;
        private Button btnTim, btnHuy;

        public string MaDauSach => string.IsNullOrWhiteSpace(txtMaDauSach.Text) ? null : txtMaDauSach.Text.Trim();
        public string TenSach => string.IsNullOrWhiteSpace(txtTenSach.Text) ? null : txtTenSach.Text.Trim();
        public int? SoLuong => int.TryParse(txtSoLuong.Text.Trim(), out int sl) ? sl : null;
        public int? NamXB => int.TryParse(txtNamXB.Text.Trim(), out int nxb) ? nxb : null;
        public string MaTG => string.IsNullOrWhiteSpace(txtMaTG.Text) ? null : txtMaTG.Text.Trim();
        public string MaTL => string.IsNullOrWhiteSpace(txtMaTL.Text) ? null : txtMaTL.Text.Trim();

        public frmTimKiemPopup()
        {
            Text = "Tìm kiếm nâng cao";
            Size = new Size(360, 310);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Font f = new Font("Segoe UI", 9.5F);

            Controls.Add(new Label { Text = "Mã đầu sách:", Location = new Point(20, 20), AutoSize = true, Font = f });
            txtMaDauSach = new TextBox { Location = new Point(130, 17), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaDauSach);

            Controls.Add(new Label { Text = "Tên sách:", Location = new Point(20, 55), AutoSize = true, Font = f });
            txtTenSach = new TextBox { Location = new Point(130, 52), Size = new Size(180, 24), Font = f };
            Controls.Add(txtTenSach);

            Controls.Add(new Label { Text = "Số lượng:", Location = new Point(20, 90), AutoSize = true, Font = f });
            txtSoLuong = new TextBox { Location = new Point(130, 87), Size = new Size(180, 24), Font = f };
            Controls.Add(txtSoLuong);

            Controls.Add(new Label { Text = "Năm XB:", Location = new Point(20, 125), AutoSize = true, Font = f });
            txtNamXB = new TextBox { Location = new Point(130, 122), Size = new Size(180, 24), Font = f };
            Controls.Add(txtNamXB);

            Controls.Add(new Label { Text = "Mã Tác giả:", Location = new Point(20, 160), AutoSize = true, Font = f });
            txtMaTG = new TextBox { Location = new Point(130, 157), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaTG);

            Controls.Add(new Label { Text = "Mã Thể loại:", Location = new Point(20, 195), AutoSize = true, Font = f });
            txtMaTL = new TextBox { Location = new Point(130, 192), Size = new Size(180, 24), Font = f };
            Controls.Add(txtMaTL);

            btnTim = new Button { Text = "Tìm kiếm", Location = new Point(130, 235), Size = new Size(85, 30), DialogResult = DialogResult.OK, Font = f, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnHuy = new Button { Text = "Hủy", Location = new Point(225, 235), Size = new Size(85, 30), DialogResult = DialogResult.Cancel, Font = f, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTim.FlatAppearance.BorderSize = 0;
            btnHuy.FlatAppearance.BorderSize = 0;

            Controls.Add(btnTim);
            Controls.Add(btnHuy);
            AcceptButton = btnTim;
            CancelButton = btnHuy;
        }
    }
}
