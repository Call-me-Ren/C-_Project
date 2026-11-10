#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public class TacGiaGUI : Form
    {
        private readonly TacGiaBUS tacGiaBUS = new TacGiaBUS();

        private TextBox txtMaTG, txtHoLot, txtTenTG, txtSDT, txtTimKiem;
        private DateTimePicker dtpNgaySinh;
        private CheckBox chkNgaySinh;
        private Button btnThem, btnSua, btnXoa, btnTimKiem, btnTaiLai, btnLamMoi;
        private DataGridView dgvTacGia;
        private Label lblThongTin;

        public TacGiaGUI()
        {
            InitGUI();
        }

        private void InitGUI()
        {
            Text = "Quản lý Tác giả";
            ClientSize = new Size(1080, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            GroupBox grp = new GroupBox
            {
                Text = "Thông tin tác giả",
                Location = new Point(10, 5),
                Size = new Size(1060, 110),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            Font fn = new Font("Segoe UI", 10F, FontStyle.Regular);

            grp.Controls.Add(new Label { Text = "Mã TG:", Location = new Point(15, 28), AutoSize = true, Font = fn });
            txtMaTG = new TextBox { Location = new Point(85, 25), Size = new Size(120, 25), Font = fn };
            grp.Controls.Add(txtMaTG);

            grp.Controls.Add(new Label { Text = "Họ lót:", Location = new Point(220, 28), AutoSize = true, Font = fn });
            txtHoLot = new TextBox { Location = new Point(285, 25), Size = new Size(180, 25), Font = fn };
            grp.Controls.Add(txtHoLot);

            grp.Controls.Add(new Label { Text = "Tên TG:", Location = new Point(480, 28), AutoSize = true, Font = fn });
            txtTenTG = new TextBox { Location = new Point(555, 25), Size = new Size(150, 25), Font = fn };
            grp.Controls.Add(txtTenTG);

            grp.Controls.Add(new Label { Text = "SĐT:", Location = new Point(720, 28), AutoSize = true, Font = fn });
            txtSDT = new TextBox { Location = new Point(770, 25), Size = new Size(150, 25), Font = fn };
            grp.Controls.Add(txtSDT);

            // Hàng 2
            chkNgaySinh = new CheckBox { Text = "Ngày sinh:", Location = new Point(15, 68), AutoSize = true, Font = fn, Checked = true };
            chkNgaySinh.CheckedChanged += (s, ev) => dtpNgaySinh.Enabled = chkNgaySinh.Checked;
            grp.Controls.Add(chkNgaySinh);

            dtpNgaySinh = new DateTimePicker { Location = new Point(130, 65), Size = new Size(180, 25), Format = DateTimePickerFormat.Short, Font = fn };
            grp.Controls.Add(dtpNgaySinh);

            btnThem = new Button { Text = "Thêm", Location = new Point(340, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSua = new Button { Text = "Sửa", Location = new Point(440, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnXoa = new Button { Text = "Xóa", Location = new Point(540, 63), Size = new Size(90, 30), Font = fn, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(640, 63), Size = new Size(100, 30), Font = fn, BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += (s, ev) => LamMoi();

            grp.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            Controls.Add(grp);

            // TÌM KIẾM
            Panel pnl = new Panel { Location = new Point(10, 120), Size = new Size(1060, 40) };
            pnl.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(5, 10), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(90, 7), Size = new Size(250, 25) };
            pnl.Controls.Add(txtTimKiem);

            btnTimKiem = new Button { Text = "Tìm", Location = new Point(350, 5), Size = new Size(80, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTimKiem.Click += btnTimKiem_Click;
            pnl.Controls.Add(btnTimKiem);

            btnTaiLai = new Button { Text = "Hiện tất cả", Location = new Point(440, 5), Size = new Size(110, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTaiLai.Click += btnTaiLai_Click;
            pnl.Controls.Add(btnTaiLai);

            lblThongTin = new Label { Location = new Point(570, 10), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnl.Controls.Add(lblThongTin);
            Controls.Add(pnl);

            // DATAGRIDVIEW
            dgvTacGia = new DataGridView
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
            dgvTacGia.SelectionChanged += dgvTacGia_SelectionChanged;
            Controls.Add(dgvTacGia);

            Load += TacGiaGUI_Load;
        }

        private void TacGiaGUI_Load(object sender, EventArgs e)
        {
            TaiDuLieu(tacGiaBUS.DocDSSach());
        }

        private void TaiDuLieu(List<TacGiaDTO> ds)
        {
            dgvTacGia.DataSource = null;
            dgvTacGia.DataSource = ds;
            DatTieuDe("MaTG", "Mã TG");
            DatTieuDe("HoLot", "Họ lót");
            DatTieuDe("TenTG", "Tên TG");
            DatTieuDe("NgaySinh", "Ngày sinh");
            DatTieuDe("SDT", "SĐT");
            lblThongTin.Text = "Số tác giả: " + ds.Count;
        }

        private void dgvTacGia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow != null && dgvTacGia.CurrentRow.DataBoundItem is TacGiaDTO tg)
            {
                txtMaTG.Text = tg.MaTG;
                txtMaTG.ReadOnly = true;
                txtMaTG.BackColor = Color.FromArgb(238, 242, 246);
                txtHoLot.Text = tg.HoLot;
                txtTenTG.Text = tg.TenTG;
                txtSDT.Text = tg.SDT;
                if (tg.NgaySinh.HasValue)
                {
                    chkNgaySinh.Checked = true;
                    dtpNgaySinh.Value = tg.NgaySinh.Value;
                }
                else
                {
                    chkNgaySinh.Checked = false;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTG.Text) || string.IsNullOrWhiteSpace(txtTenTG.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã tác giả và Tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra trùng mã trước khi thêm
                if (tacGiaBUS.KiemTraTrungMa(txtMaTG.Text.Trim()))
                {
                    MessageBox.Show("Mã tác giả '" + txtMaTG.Text.Trim() + "' đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenTG.Text)) { MessageBox.Show("Vui lòng nhập tên tác giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string maGoc = (dgvTacGia.CurrentRow.DataBoundItem as TacGiaDTO).MaTG;
                TacGiaDTO tg = new TacGiaDTO
                {
                    MaTG = maGoc,
                    HoLot = txtHoLot.Text.Trim(),
                    TenTG = txtTenTG.Text.Trim(),
                    NgaySinh = chkNgaySinh.Checked ? dtpNgaySinh.Value : null,
                    SDT = txtSDT.Text.Trim()
                };

                tacGiaBUS.Them(tg);
                TaiDuLieu(tacGiaBUS.DocDSSach());
                MessageBox.Show("Thêm tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm tác giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTacGia.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn tác giả cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TacGiaDTO tg = new TacGiaDTO
                {
                    MaTG = txtMaTG.Text.Trim(),
                    HoLot = txtHoLot.Text.Trim(),
                    TenTG = txtTenTG.Text.Trim(),
                    NgaySinh = chkNgaySinh.Checked ? dtpNgaySinh.Value : null,
                    SDT = txtSDT.Text.Trim()
                };

                tacGiaBUS.Sua(tg);
                TaiDuLieu(tacGiaBUS.DocDSSach());
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa tác giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tác giả cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ma = txtMaTG.Text.Trim();
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa tác giả '" + ma + "'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    tacGiaBUS.Xoa(ma);
                    TaiDuLieu(tacGiaBUS.DocDSSach());
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa tác giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TaiDuLieu(tacGiaBUS.timKiem(kw, kw));
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu(tacGiaBUS.DocDSSach());
            txtTimKiem.Clear();
        }

        private void LamMoi()
        {
            txtMaTG.Clear();
            txtMaTG.ReadOnly = false;
            txtMaTG.BackColor = SystemColors.Window;
            txtHoLot.Clear();
            txtTenTG.Clear();
            txtSDT.Clear();
            chkNgaySinh.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            txtMaTG.Focus();
        }

        private void DatTieuDe(string tenCot, string tieuDe)
        {
            if (dgvTacGia.Columns[tenCot] != null) dgvTacGia.Columns[tenCot].HeaderText = tieuDe;
        }
    }
}
