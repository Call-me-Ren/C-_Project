#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public class NXBGUI : Form
    {
        private readonly NXBBUS nxbBUS = new NXBBUS();

        private TextBox txtMaNXB, txtTenNXB, txtDiaChi, txtTimKiem;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTaiLai, btnLamMoi;
        private DataGridView dgvNXB;
        private Label lblThongTin;

        public NXBGUI()
        {
            InitGUI();
        }

        private void InitGUI()
        {
            Text = "Quản lý Nhà xuất bản";
            ClientSize = new Size(1080, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            GroupBox grp = new GroupBox
            {
                Text = "Thông tin nhà xuất bản",
                Location = new Point(10, 5),
                Size = new Size(1060, 80),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            Font fn = new Font("Segoe UI", 10F, FontStyle.Regular);

            grp.Controls.Add(new Label { Text = "Mã NXB:", Location = new Point(15, 30), AutoSize = true, Font = fn });
            txtMaNXB = new TextBox { Location = new Point(95, 27), Size = new Size(120, 25), Font = fn };
            grp.Controls.Add(txtMaNXB);

            grp.Controls.Add(new Label { Text = "Tên NXB:", Location = new Point(230, 30), AutoSize = true, Font = fn });
            txtTenNXB = new TextBox { Location = new Point(315, 27), Size = new Size(200, 25), Font = fn };
            grp.Controls.Add(txtTenNXB);

            grp.Controls.Add(new Label { Text = "Địa chỉ:", Location = new Point(530, 30), AutoSize = true, Font = fn });
            txtDiaChi = new TextBox { Location = new Point(600, 27), Size = new Size(250, 25), Font = fn };
            grp.Controls.Add(txtDiaChi);

            btnThem = new Button { Text = "Thêm", Location = new Point(870, 25), Size = new Size(80, 30), Font = fn, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White };
            btnSua = new Button { Text = "Sửa", Location = new Point(955, 25), Size = new Size(80, 30), Font = fn, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;

            grp.Controls.AddRange(new Control[] { btnThem, btnSua });
            Controls.Add(grp);

            // TÌM KIẾM
            Panel pnl = new Panel { Location = new Point(10, 90), Size = new Size(1060, 40) };
            pnl.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(90, 7), Size = new Size(250, 25) };
            pnl.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(350, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White };
            btnTimKiem.Click += btnTimKiem_Click;
            pnl.Controls.Add(btnTimKiem);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(440, 5), Size = new Size(110, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };
            btnTaiLai.Click += btnTaiLai_Click;
            pnl.Controls.Add(btnTaiLai);

            btnXoa = new Button { Text = "Xóa", Location = new Point(560, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White };
            btnXoa.Click += btnXoa_Click;
            pnl.Controls.Add(btnXoa);

            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(650, 5), Size = new Size(100, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White };
            btnLamMoi.Click += (s, ev) => LamMoi();
            pnl.Controls.Add(btnLamMoi);

            lblThongTin = new Label { Location = new Point(770, 10), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnl.Controls.Add(lblThongTin);
            Controls.Add(pnl);

            // DATAGRIDVIEW
            dgvNXB = new DataGridView
            {
                Location = new Point(10, 135),
                Size = new Size(1060, 505),
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
            dgvNXB.SelectionChanged += dgvNXB_SelectionChanged;
            Controls.Add(dgvNXB);

            Load += NXBGUI_Load;
        }

        private void NXBGUI_Load(object sender, EventArgs e)
        {
            TaiDuLieu(nxbBUS.DocDSNXB());
        }

        private void TaiDuLieu(List<NXBDTO> ds)
        {
            dgvNXB.DataSource = null;
            dgvNXB.DataSource = ds;
            DatTieuDe("MaNXB", "Mã NXB");
            DatTieuDe("TenNXB", "Tên NXB");
            DatTieuDe("DiaChi", "Địa chỉ");
            lblThongTin.Text = "Số NXB: " + ds.Count;
        }

        private void dgvNXB_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNXB.CurrentRow != null && dgvNXB.CurrentRow.DataBoundItem is NXBDTO nxb)
            {
                txtMaNXB.Text = nxb.MaNXB;
                txtMaNXB.ReadOnly = true;
                txtMaNXB.BackColor = Color.FromArgb(238, 242, 246);
                txtTenNXB.Text = nxb.TenNXB;
                txtDiaChi.Text = nxb.DiaChi;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNXB.Text) || string.IsNullOrWhiteSpace(txtTenNXB.Text))
                { MessageBox.Show("Nhập đầy đủ mã và tên NXB.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                // KIỂM TRA TRÙNG MÃ
                if (nxbBUS.KiemTraTrungMa(txtMaNXB.Text.Trim()))
                {
                    MessageBox.Show("Mã NXB '" + txtMaNXB.Text.Trim() + "' đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                nxbBUS.Them(new NXBDTO(txtMaNXB.Text.Trim(), txtTenNXB.Text.Trim(), txtDiaChi.Text.Trim()));
                TaiDuLieu(nxbBUS.DocDSNXB());
                MessageBox.Show("Thêm NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNXB.CurrentRow == null) { MessageBox.Show("Chọn NXB cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (string.IsNullOrWhiteSpace(txtTenNXB.Text)) { MessageBox.Show("Vui lòng nhập tên nhà xuất bản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string maGoc = (dgvNXB.CurrentRow.DataBoundItem as NXBDTO).MaNXB;
                nxbBUS.Sua(new NXBDTO(maGoc, txtTenNXB.Text.Trim(), txtDiaChi.Text.Trim()));
                TaiDuLieu(nxbBUS.DocDSNXB());
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNXB.CurrentRow == null) { MessageBox.Show("Chọn NXB cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string ma = txtMaNXB.Text.Trim();
            if (MessageBox.Show("Xóa NXB '" + ma + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    nxbBUS.Xoa(ma);
                    TaiDuLieu(nxbBUS.DocDSNXB());
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw)) { MessageBox.Show("Nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            TaiDuLieu(nxbBUS.timKiem(kw, kw));
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu(nxbBUS.DocDSNXB());
            txtTimKiem.Clear();
        }

        private void LamMoi()
        {
            txtMaNXB.Clear(); txtTenNXB.Clear(); txtDiaChi.Clear(); txtMaNXB.Focus();
            txtMaNXB.ReadOnly = false; txtMaNXB.BackColor = SystemColors.Window;
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvNXB.Columns[tenCot] != null) dgvNXB.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
