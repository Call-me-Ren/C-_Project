using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmLoaiDocGia : Form
    {
        // Khởi tạo đối tượng BUS để xử lý nghiệp vụ (Tuân thủ mô hình 3 lớp)
        private readonly LoaiDocGiaBUS bus = new LoaiDocGiaBUS();

        public frmLoaiDocGia()
        {
            InitializeComponent();
        }

        // 1. SỰ KIỆN LOAD FORM
        private void frmLoaiDocGia_Load(object sender, EventArgs e)
        {
            try
            {
                // Nạp dữ liệu từ CSDL vào ArrayList ở BUS và hiển thị lên DataGridView
                HienThiDanhSach(bus.DocDSLoaiDocGia());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 2. HÀM HIỂN THỊ DỮ LIỆU LÊN DATAGRIDVIEW
        private void HienThiDanhSach(ArrayList danhSach)
        {
            dgvLoaiDocGia.AutoGenerateColumns = false;
            dgvLoaiDocGia.DataSource = null;

            // Chuyển đổi ArrayList sang List<LoaiDocGiaDTO> để DataGridView bind dữ liệu chuẩn xác
            List<LoaiDocGiaDTO> list = new List<LoaiDocGiaDTO>();
            foreach (LoaiDocGiaDTO item in danhSach)
            {
                list.Add(item);
            }

            dgvLoaiDocGia.DataSource = list;
        }

        // 3. SỰ KIỆN CLICK CHỌN DÒNG TRÊN BẢNG
        private void dgvLoaiDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvLoaiDocGia.Rows.Count) return;

                DataGridViewRow row = dgvLoaiDocGia.Rows[e.RowIndex];
                txtMaLDG.Text = row.Cells["colMaLDG"].Value?.ToString() ?? string.Empty;
                txtTenLDG.Text = row.Cells["colTenLDG"].Value?.ToString() ?? string.Empty;

                // Khóa ô Mã khi đang chọn sửa/xem (Khóa chính không được sửa)
                txtMaLDG.ReadOnly = true;
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 4. CHỨC NĂNG THÊM
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maLDG = txtMaLDG.Text.Trim();
                string tenLDG = txtTenLDG.Text.Trim();

                LoaiDocGiaDTO ldg = new LoaiDocGiaDTO(maLDG, tenLDG);

                // Gọi BUS xử lý nghiệp vụ (kiểm tra rỗng, kiểm tra trùng mã)
                if (bus.Them(ldg, out string error))
                {
                    MessageBox.Show("Thêm loại độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(bus.dsldg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 5. CHỨC NĂNG SỬA
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maLDG = txtMaLDG.Text.Trim();
                string tenLDG = txtTenLDG.Text.Trim();

                if (string.IsNullOrWhiteSpace(maLDG))
                {
                    MessageBox.Show("Vui lòng chọn loại độc giả cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoaiDocGiaDTO ldg = new LoaiDocGiaDTO(maLDG, tenLDG);

                if (bus.Sua(ldg, out string error))
                {
                    MessageBox.Show("Cập nhật loại độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(bus.dsldg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 6. CHỨC NĂNG XÓA
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maLDG = txtMaLDG.Text.Trim();
                if (string.IsNullOrWhiteSpace(maLDG))
                {
                    MessageBox.Show("Vui lòng chọn loại độc giả cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa loại độc giả '{maLDG}' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                if (bus.Xoa(maLDG, out string error))
                {
                    MessageBox.Show("Xóa loại độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach(bus.dsldg_arr);
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        // 7. CHỨC NĂNG LÀM MỚI (XÓA TRẮNG FORM)
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoiForm()
        {
            txtMaLDG.Clear();
            txtTenLDG.Clear();
            txtMaLDG.ReadOnly = false;
            txtMaLDG.Focus();
        }

        // 8. TÌM KIẾM TRÊN ARRAYLIST TẠI BUS
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                ArrayList ketQua = bus.TimKiem(keyword);
                HienThiDanhSach(ketQua);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // 9. HIỂN THỊ TẤT CẢ (HỦY BỘ LỌC TÌM KIẾM)
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(bus.dsldg_arr);
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }
    }
}
