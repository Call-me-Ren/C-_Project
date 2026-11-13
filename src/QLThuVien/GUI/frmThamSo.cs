using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmThamSo : Form
    {
        private readonly ThamSoBUS _tsBUS;

        public frmThamSo()
        {
            InitializeComponent();
            _tsBUS = new ThamSoBUS();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvThamSo.AutoGenerateColumns = true;
                dgvThamSo.DataSource = null;
                var list = _tsBUS.DocDS(forceReload: true);
                dgvThamSo.DataSource = list;

                if (dgvThamSo.Columns["MaTS"] is DataGridViewColumn c1)
                    c1.HeaderText = "Mã Tham Số";
                if (dgvThamSo.Columns["TenTS"] is DataGridViewColumn c2)
                    c2.HeaderText = "Tên Quy Định / Tham Số";
                if (dgvThamSo.Columns["GiaTri"] is DataGridViewColumn c3)
                    c3.HeaderText = "Giá Trị Cấu Hình";

                if (dgvThamSo.Rows.Count > 0)
                {
                    dgvThamSo.Rows[0].Selected = true;
                    DgvThamSo_CellClick(dgvThamSo, new DataGridViewCellEventArgs(0, 0));
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void DgvThamSo_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvThamSo.Rows[e.RowIndex].DataBoundItem is ThamSoDTO ts)
                {
                    txtMaTS.Text = ts.MaTS;
                    txtTenTS.Text = ts.TenTS;
                    txtGiaTri.Text = ts.GiaTri;
                    txtGiaTri.Focus();
                    txtGiaTri.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chọn tham số: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTS.Text))
                {
                    MessageBox.Show("Vui lòng chọn một tham số từ bảng danh sách để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = _tsBUS.CapNhat(txtMaTS.Text, txtGiaTri.Text);

                MessageBox.Show(result.Message, "Kết quả", MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật tham số: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            txtMaTS.Text = "";
            txtTenTS.Text = "";
            txtGiaTri.Text = "";
            LoadData();
        }
    }
}
