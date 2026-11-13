#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuVien.DTO;
using QLThuVien.BUS;

namespace QLThuVien.GUI
{
    public class frmChonDauSachPopup : Form
    {
        private DataGridView dgvDauSach;
        private Button btnChon, btnHuy;
        private readonly DauSachBUS dauSachBUS = new DauSachBUS();

        public frmChonDauSachPopup()
        {
            InitPopup();
        }

        private void InitPopup()
        {
            Text = "Chọn Đầu sách";
            Size = new Size(700, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 10F);

            dgvDauSach = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(660, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvDauSach.CellDoubleClick += (s, e) => ChonDuLieu();
            Controls.Add(dgvDauSach);

            btnChon = new Button
            {
                Text = "Chọn",
                Location = new Point(480, 320),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnChon.Click += (s, e) => ChonDuLieu();

            btnHuy = new Button
            {
                Text = "Hủy",
                Location = new Point(580, 320),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(btnChon);
            Controls.Add(btnHuy);

            Load += (s, e) =>
            {
                dgvDauSach.DataSource = dauSachBUS.DocDSSach();
                if (dgvDauSach.Columns["DonGia"] != null)
                    dgvDauSach.Columns["DonGia"].Visible = false;
            };
        }

        private void ChonDuLieu()
        {
            if (dgvDauSach.CurrentRow != null && dgvDauSach.CurrentRow.DataBoundItem is DauSachDTO ds)
            {
                FormSharedData.SelectedMaDauSach = ds.MaDauSach;
                FormSharedData.SelectedTenDauSach = ds.TenSach;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}