using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>Biểu mẫu GUI quản lý danh sách nhân viên, chuyển thao tác kiểm tra và lưu cho <see cref="NhanVienBUS"/>.</summary>
    public partial class frmNhanVien : Form
    {
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private string hoLotHopLe = string.Empty;
        private string tenHopLe = string.Empty;
        private bool dangGanTen;

        /// <summary>Khởi tạo biểu mẫu và gắn sự kiện kiểm soát nhập họ tên.</summary>
        public frmNhanVien()
        {
            InitializeComponent();
            txtHoLot.TextChanged += (_, _) => KiemTraKyTuTen(txtHoLot, ref hoLotHopLe);
            txtTenNV.TextChanged += (_, _) => KiemTraKyTuTen(txtTenNV, ref tenHopLe);
            txtHoLot.Leave += (_, _) => ChuanHoaO(txtHoLot, ref hoLotHopLe);
            txtTenNV.Leave += (_, _) => ChuanHoaO(txtTenNV, ref tenHopLe);
        }

        /// <summary>Nạp chức vụ, đọc danh sách nhân viên và đặt biểu mẫu về trạng thái nhập mới.</summary>
        /// <param name="sender">Đối tượng phát sinh sự kiện nạp biểu mẫu.</param>
        /// <param name="e">Thông tin sự kiện nạp biểu mẫu.</param>
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                NapDanhSachChucVu(null);
                HienThiDanhSach(bus.DocDS());
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void HienThiDanhSach(List<NhanVienDTO> list)
        {
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = list;
        }

        /// <summary>Đưa nhân viên được chọn trên bảng vào các ô thông tin để xem hoặc sửa.</summary>
        /// <param name="sender">Bảng nhân viên phát sinh thao tác chọn dòng.</param>
        /// <param name="e">Vị trí dòng được chọn; dòng tiêu đề bị bỏ qua.</param>
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNhanVien.Rows.Count) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            txtMaNV.Text = row.Cells["colMaNV"].Value?.ToString() ?? string.Empty;
            GanTen(txtHoLot, row.Cells["colHoLot"].Value?.ToString() ?? string.Empty, ref hoLotHopLe);
            GanTen(txtTenNV, row.Cells["colTenNV"].Value?.ToString() ?? string.Empty, ref tenHopLe);
            string? chucVu = row.Cells["colChucVu"].Value?.ToString();
            NapDanhSachChucVu(chucVu);
            txtMaNV.ReadOnly = true;
        }

        /// <summary>Lấy thông tin trên biểu mẫu, nhờ BUS kiểm tra và thêm nhân viên mới.</summary>
        /// <param name="sender">Nút thêm nhân viên.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Sau khi thêm thành công, danh sách được nạp lại để nhận mã do BUS sinh và biểu mẫu trở về trạng thái nhập mới.</remarks>
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO(
                    string.Empty,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    cboChucVu.SelectedItem?.ToString());

                if (!bus.Them(nv)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Gửi thông tin nhân viên đang chọn sang BUS để kiểm tra và cập nhật.</summary>
        /// <param name="sender">Nút sửa nhân viên.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Mã nhân viên phải được chọn trước; sau khi cập nhật thành công, bảng được nạp lại từ BUS.</remarks>
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                    throw new BusinessRuleException("Hãy chọn nhân viên cần sửa.");

                NhanVienDTO nv = new NhanVienDTO(
                    txtMaNV.Text,
                    txtHoLot.Text,
                    txtTenNV.Text,
                    cboChucVu.SelectedItem?.ToString());

                if (!bus.Sua(nv)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Yêu cầu xác nhận rồi nhờ BUS xóa nhân viên đang chọn.</summary>
        /// <param name="sender">Nút xóa nhân viên.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Thao tác dừng nếu người dùng chọn Không; BUS từ chối xóa nhân viên đang được giao dịch tham chiếu.</remarks>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maNV = txtMaNV.Text.Trim();
                if (maNV.Length == 0) throw new BusinessRuleException("Hãy chọn nhân viên cần xóa.");
                DialogResult result = MessageBox.Show(
                    $"Xóa nhân viên {maNV}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
                if (!bus.Xoa(maNV)) throw new InvalidOperationException();
                HienThiDanhSach(bus.LayDanhSach(reload: true));
                LamMoiForm();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        /// <summary>Đọc mới danh sách nhân viên và xóa nội dung tìm kiếm hiện tại.</summary>
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
            txtMaNV.Text = bus.TaoMaNhanVienTiepTheo();
            txtMaNV.ReadOnly = true;
            GanTen(txtHoLot, string.Empty, ref hoLotHopLe);
            GanTen(txtTenNV, string.Empty, ref tenHopLe);
            NapDanhSachChucVu(null);
            txtTimKiem.Clear();
            txtTenNV.Focus();
        }

        private void NapDanhSachChucVu(string? selected)
        {
            cboChucVu.Items.Clear();
            foreach (string chucVu in NhanVienBUS.DanhSachChucVu) cboChucVu.Items.Add(chucVu);
            if (!string.IsNullOrWhiteSpace(selected) && !cboChucVu.Items.Contains(selected))
                cboChucVu.Items.Add(selected);
            cboChucVu.SelectedIndex = selected == null ? -1 : cboChucVu.Items.IndexOf(selected);
        }

        /// <summary>Tìm nhân viên theo từ khóa nhập trên biểu mẫu và hiển thị kết quả BUS trả về.</summary>
        /// <param name="sender">Nút tìm kiếm.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            HienThiDanhSach(bus.TimKiem(txtTimKiem.Text.Trim()));
        }

        /// <summary>Xóa từ khóa và nạp lại toàn bộ danh sách nhân viên từ BUS.</summary>
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

        /// <summary>Kích hoạt tìm kiếm khi người dùng nhấn Enter trong ô từ khóa.</summary>
        /// <param name="sender">Ô tìm kiếm phát sinh sự kiện bàn phím.</param>
        /// <param name="e">Phím vừa nhấn; chỉ phím Enter được xử lý.</param>
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnTimKiem_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void KiemTraKyTuTen(TextBox textBox, ref string lastValid)
        {
            if (dangGanTen) return;
            foreach (char c in textBox.Text)
            {
                if (NhanVienBUS.KyTuTenHopLe(c)) continue;
                int selectionStart = Math.Min(textBox.SelectionStart, lastValid.Length);
                GanTen(textBox, lastValid, ref lastValid);
                textBox.SelectionStart = selectionStart;
                return;
            }
            lastValid = textBox.Text;
        }

        private void ChuanHoaO(TextBox item, ref string lastValid)
        {
            GanTen(item, NhanVienBUS.ChuanHoaHoTen(item.Text), ref lastValid);
        }

        private void GanTen(TextBox textBox, string value, ref string lastValid)
        {
            dangGanTen = true;
            textBox.Text = value;
            lastValid = value;
            dangGanTen = false;
        }
    }
}
