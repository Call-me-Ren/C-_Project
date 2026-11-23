using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>Biểu mẫu GUI quản lý quy định phạt, chuyển kiểm tra và lưu dữ liệu cho <see cref="QuyDinhPhatBUS"/>.</summary>
    public partial class frmQuyDinhPhat : Form
    {
        private readonly QuyDinhPhatBUS bus = new QuyDinhPhatBUS();

        /// <summary>Khởi tạo biểu mẫu quy định phạt.</summary>
        public frmQuyDinhPhat()
        {
            InitializeComponent();
        }

        /// <summary>Nạp các lựa chọn loại, cách tính, đơn vị và danh sách quy định phạt.</summary>
        /// <param name="sender">Đối tượng phát sinh sự kiện nạp biểu mẫu.</param>
        /// <param name="e">Thông tin sự kiện nạp biểu mẫu.</param>
        private void frmQuyDinhPhat_Load(object sender, EventArgs e)
        {
            try
            {
                NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, null);
                NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, null);
                NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, null);
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void HienThiDanhSach(List<QuyDinhPhatDTO> list)
        {
            dgvQuyDinhPhat.AutoGenerateColumns = false;
            dgvQuyDinhPhat.DataSource = null;
            dgvQuyDinhPhat.DataSource = list;
        }

        /// <summary>Đưa quy định được chọn trên bảng vào các trường để xem hoặc sửa.</summary>
        /// <param name="sender">Bảng quy định phát sinh thao tác chọn dòng.</param>
        /// <param name="e">Vị trí dòng được chọn; dòng tiêu đề bị bỏ qua.</param>
        private void dgvQuyDinhPhat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvQuyDinhPhat.Rows.Count) return;
            DataGridViewRow row = dgvQuyDinhPhat.Rows[e.RowIndex];
            txtMaQD.Text = row.Cells["colMaQD"].Value?.ToString() ?? string.Empty;
            txtTenQD.Text = row.Cells["colTenQD"].Value?.ToString() ?? string.Empty;
            NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, row.Cells["colLoaiPhat"].Value?.ToString());
            NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, row.Cells["colCachTinh"].Value?.ToString());
            NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, row.Cells["colDonViTinh"].Value?.ToString());

            if (decimal.TryParse(row.Cells["colMucPhat"].Value?.ToString(), out decimal mucPhat))
                numMucPhat.Value = Math.Max(numMucPhat.Minimum, Math.Min(numMucPhat.Maximum, mucPhat));
            else
                numMucPhat.Value = 0;
            txtMaQD.ReadOnly = true;
        }

        /// <summary>Tạo quy định từ biểu mẫu rồi nhờ BUS kiểm tra và lưu mã mới.</summary>
        /// <param name="sender">Nút thêm quy định.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Sau khi thêm, bảng được đọc lại để hiển thị mã do BUS sinh.</remarks>
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                QuyDinhPhatDTO qd = TaoQuyDinhTuForm(string.Empty);
                if (!bus.Them(qd)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Gửi quy định đang chọn sang BUS để kiểm tra và cập nhật.</summary>
        /// <param name="sender">Nút sửa quy định.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Mã phải là một quy định đang tồn tại; BUS giữ các ràng buộc của cách tính và đơn vị.</remarks>
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaQD.Text))
                    throw new BusinessRuleException("Hãy chọn quy định phạt cần sửa.");
                QuyDinhPhatDTO qd = TaoQuyDinhTuForm(txtMaQD.Text);
                if (!bus.Sua(qd)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Yêu cầu xác nhận rồi nhờ BUS xóa quy định đang chọn.</summary>
        /// <param name="sender">Nút xóa quy định.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Thao tác dừng nếu người dùng chọn Không hoặc BUS xác định quy định còn được vi phạm tham chiếu.</remarks>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maQD = txtMaQD.Text.Trim();
                if (maQD.Length == 0) throw new BusinessRuleException("Hãy chọn quy định phạt cần xóa.");
                DialogResult result = MessageBox.Show(
                    $"Xóa quy định {maQD}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
                if (!bus.Xoa(maQD)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Nạp mới danh sách quy định và đặt lại các trường nhập.</summary>
        /// <param name="sender">Nút làm mới danh sách.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void LamMoiForm()
        {
            txtMaQD.Text = bus.TaoMaQuyDinhTiepTheo();
            txtMaQD.ReadOnly = true;
            txtTenQD.Clear();
            NapLuaChon(cboLoaiPhat, QuyDinhPhatBUS.DanhSachLoaiPhat, null);
            NapLuaChon(cboCachTinh, QuyDinhPhatBUS.DanhSachCachTinh, null);
            NapLuaChon(cboDonViTinh, QuyDinhPhatBUS.DanhSachDonViTinh, null);
            numMucPhat.Value = 0;
            txtTimKiem.Clear();
            txtTenQD.Focus();
        }

        private QuyDinhPhatDTO TaoQuyDinhTuForm(string maQD)
        {
            return new QuyDinhPhatDTO(
                maQD,
                txtTenQD.Text,
                cboLoaiPhat.SelectedItem?.ToString(),
                cboCachTinh.SelectedItem?.ToString(),
                cboDonViTinh.SelectedItem?.ToString(),
                numMucPhat.Value);
        }

        private static void NapLuaChon(ComboBox combo, IReadOnlyList<string> options, string? selected)
        {
            combo.Items.Clear();
            foreach (string option in options) combo.Items.Add(option);
            if (!string.IsNullOrWhiteSpace(selected) && !combo.Items.Contains(selected)) combo.Items.Add(selected);
            combo.SelectedIndex = selected == null ? -1 : combo.Items.IndexOf(selected);
        }

        /// <summary>Tìm quy định theo nội dung nhập và hiển thị các kết quả BUS trả về.</summary>
        /// <param name="sender">Nút tìm kiếm.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            HienThiDanhSach(bus.TimKiem(txtTimKiem.Text.Trim()));
        }

        /// <summary>Xóa từ khóa và nạp lại toàn bộ danh sách quy định từ BUS.</summary>
        /// <param name="sender">Nút hiện tất cả.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Kích hoạt tìm kiếm khi nhấn Enter trong ô từ khóa.</summary>
        /// <param name="sender">Ô tìm kiếm phát sinh sự kiện bàn phím.</param>
        /// <param name="e">Phím vừa nhấn; chỉ phím Enter được xử lý.</param>
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnTimKiem_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
