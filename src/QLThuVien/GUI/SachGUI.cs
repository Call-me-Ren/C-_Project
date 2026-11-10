#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public class SachGUI : Form
    {
        private readonly SachBUS sachBUS = new SachBUS();
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();

        private bool isBinding = false;

        private TextBox txtMaSach, txtMaDauSach, txtTinhTrang, txtViTri, txtTimKiem;
        private ComboBox cboTrangThai;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTaiLai, btnLamMoi;
        private Button btnChonDauSach;
        private DataGridView dgvSach;
        private Label lblThongTin;

        public SachGUI()
        {
            InitGUI();
        }

        private void InitGUI()
        {
            Text = "Quản lý Sách";
            ClientSize = new Size(1080, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            GroupBox grp = new GroupBox
            {
                Text = "Thông tin sách",
                Location = new Point(10, 5),
                Size = new Size(1060, 110),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            Font fn = new Font("Segoe UI", 10F, FontStyle.Regular);

            // HÀNG 1: Nới rộng tọa độ các nhãn và ô nhập liệu
            // 1. Mã sách
            grp.Controls.Add(new Label { Text = "Mã sách:", Location = new Point(12, 28), AutoSize = true, Font = fn });
            txtMaSach = new TextBox { Location = new Point(85, 25), Size = new Size(95, 25), Font = fn };
            grp.Controls.Add(txtMaSach);

            // 2. Mã đầu sách (Đã nới rộng X lên 195 và ô nhập X lên 305 để KHÔNG bị mất chữ D)
            grp.Controls.Add(new Label { Text = "Mã đầu sách:", Location = new Point(195, 28), AutoSize = true, Font = fn });
            txtMaDauSach = new TextBox { Location = new Point(305, 25), Size = new Size(115, 25), Font = fn };
            grp.Controls.Add(txtMaDauSach);

            btnChonDauSach = new Button
            {
                Text = "...",
                Location = new Point(425, 24),
                Size = new Size(35, 27),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(236, 240, 241),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnChonDauSach.FlatAppearance.BorderColor = Color.FromArgb(127, 140, 141);
            btnChonDauSach.FlatAppearance.BorderSize = 1;
            btnChonDauSach.Click += btnChonDauSach_Click;
            grp.Controls.Add(btnChonDauSach);

            // 3. Tình trạng (Đã nới rộng X lên 475 và ô nhập X lên 565)
            grp.Controls.Add(new Label { Text = "Tình trạng:", Location = new Point(475, 28), AutoSize = true, Font = fn });
            txtTinhTrang = new TextBox { Location = new Point(565, 25), Size = new Size(180, 25), Font = fn };
            grp.Controls.Add(txtTinhTrang);

            // 4. Vị trí
            grp.Controls.Add(new Label { Text = "Vị trí:", Location = new Point(765, 28), AutoSize = true, Font = fn });
            txtViTri = new TextBox { Location = new Point(815, 25), Size = new Size(180, 25), Font = fn };
            grp.Controls.Add(txtViTri);

            // HÀNG 2: Trạng thái (Đã nới rộng ô ComboBox X lên 100 để không bị che chữ)
            grp.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(12, 68), AutoSize = true, Font = fn });
            cboTrangThai = new ComboBox { Location = new Point(100, 65), Size = new Size(150, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = fn };
            cboTrangThai.Items.AddRange(new object[] { "Chưa cho mượn", "Đã cho mượn" });
            cboTrangThai.SelectedIndex = 0;
            grp.Controls.Add(cboTrangThai);

            btnThem = new Button { Text = "Thêm", Location = new Point(270, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSua = new Button { Text = "Sửa", Location = new Point(370, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnXoa = new Button { Text = "Xóa", Location = new Point(470, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(570, 63), Size = new Size(100, 30), Font = fn, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

            btnThem.FlatAppearance.BorderSize = 0;
            btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatAppearance.BorderSize = 0;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += (s, ev) => LamMoi();

            grp.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            Controls.Add(grp);

            // Panel Tìm kiếm
            Panel pnl = new Panel { Location = new Point(10, 120), Size = new Size(1060, 40) };
            pnl.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(90, 7), Size = new Size(250, 25) };
            pnl.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(350, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += btnTimKiem_Click;
            pnl.Controls.Add(btnTimKiem);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(440, 5), Size = new Size(110, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTaiLai.FlatAppearance.BorderSize = 0;
            btnTaiLai.Click += btnTaiLai_Click;
            pnl.Controls.Add(btnTaiLai);

            lblThongTin = new Label { Location = new Point(570, 10), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnl.Controls.Add(lblThongTin);

            Controls.Add(pnl);

            // DataGridView
            dgvSach = new DataGridView
            {
                Location = new Point(10, 165),
                Size = new Size(1060, 475),
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

            dgvSach.DataError += (s, ev) => { ev.Cancel = true; };
            dgvSach.SelectionChanged += dgvSach_SelectionChanged;
            dgvSach.CellFormatting += dgvSach_CellFormatting;
            Controls.Add(dgvSach);

            Load += SachGUI_Load;
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
            btnXoa.Enabled = !isMuon;
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
