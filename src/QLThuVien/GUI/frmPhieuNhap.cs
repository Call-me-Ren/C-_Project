using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Giao diện Quản lý Nhập sách - Form Main - Sub (1 - N)
    /// Bảng Master: PhieuNhap (1)
    /// Bảng Detail: ChiTietPhieuNhap (N)
    /// </summary>
    public partial class frmPhieuNhap : Form
    {
        // Danh sách các dòng sách đang chuẩn bị lập phiếu nhập mới (giỏ hàng chi tiết)
        private List<ChiTietPhieuNhapDTO> _dsDangNhap = new List<ChiTietPhieuNhapDTO>();
        private List<DauSachDTO> _dsDauSach = new List<DauSachDTO>();

        public frmPhieuNhap()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // 1. Nạp danh sách Phiếu Nhập lên Master GridView
                dgvPhieuNhap.DataSource = PhieuNhapBUS.GetAll();
                if (dgvPhieuNhap.Columns["MaPN"] is DataGridViewColumn c1) c1.HeaderText = "Mã Phiếu Nhập";
                if (dgvPhieuNhap.Columns["MaNCC"] is DataGridViewColumn c2) c2.HeaderText = "Mã Nhà Cung Cấp";
                if (dgvPhieuNhap.Columns["MaNV"] is DataGridViewColumn c3) c3.HeaderText = "Mã Nhân Viên";
                if (dgvPhieuNhap.Columns["NgayNhap"] is DataGridViewColumn c4) c4.HeaderText = "Ngày Nhập";
                if (dgvPhieuNhap.Columns["TongTien"] is DataGridViewColumn c5) 
                {
                    c5.HeaderText = "Tổng Tiền (VNĐ)";
                    c5.DefaultCellStyle.Format = "N0";
                }

                // 2. Nạp ComboBox Nhà cung cấp
                cboNCC.DataSource = NhaCungCapBUS.GetAll();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember = "MaNCC";

                // 3. Nạp ComboBox Đầu sách
                _dsDauSach = DauSachBUS.GetAll();
                cboDauSach.DataSource = _dsDauSach;
                cboDauSach.DisplayMember = "TenSach";
                cboDauSach.ValueMember = "MaDauSach";

                // Cập nhật giá trị ban đầu cho đơn giá
                CboDauSach_SelectedIndexChanged(null, EventArgs.Empty);

                // Hiển thị lưới chi tiết rỗng
                CapNhatLuoiChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Phiếu Nhập: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// SỰ KIỆN MAIN - SUB (1-N):
        /// Khi người dùng click chọn 1 dòng Phiếu nhập ở bảng Master
        /// -> Tự động nạp toàn bộ các cuốn sách thuộc phiếu đó lên bảng Detail
        /// </summary>
        private void DgvPhieuNhap_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvPhieuNhap.Rows[e.RowIndex].DataBoundItem is PhieuNhapDTO pn)
                {
                    // Lấy chi tiết của phiếu nhập đã lưu từ CSDL thông qua BUS
                    var chiTietList = PhieuNhapBUS.LayChiTietTheoMaPN(pn.MaPN);
                    dgvChiTiet.DataSource = null;
                    dgvChiTiet.DataSource = chiTietList;

                    DinhDangCotChiTiet();
                    lblTongTien.Text = $"{pn.TongTien:N0} VNĐ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DinhDangCotChiTiet()
        {
            if (dgvChiTiet.Columns["MaPN"] is DataGridViewColumn c1) c1.Visible = false;
            if (dgvChiTiet.Columns["MaDauSach"] is DataGridViewColumn c2) c2.HeaderText = "Mã Đầu Sách";
            if (dgvChiTiet.Columns["TenSach"] is DataGridViewColumn c3) c3.HeaderText = "Tên Đầu Sách";
            if (dgvChiTiet.Columns["DonGia"] is DataGridViewColumn c4)
            {
                c4.HeaderText = "Đơn Giá (VNĐ)";
                c4.DefaultCellStyle.Format = "N0";
            }
            if (dgvChiTiet.Columns["SoLuong"] is DataGridViewColumn c5) c5.HeaderText = "Số Lượng";
            if (dgvChiTiet.Columns["ThanhTien"] is DataGridViewColumn c6)
            {
                c6.HeaderText = "Thành Tiền (VNĐ)";
                c6.DefaultCellStyle.Format = "N0";
            }
        }

        private void CboDauSach_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDauSach.SelectedItem is DauSachDTO ds)
            {
                txtDonGia.Text = ds.DonGia.ToString("N0");
                TinhThanhTienHienTai();
            }
        }

        private void NumSoLuong_ValueChanged(object? sender, EventArgs e)
        {
            TinhThanhTienHienTai();
        }

        private void TinhThanhTienHienTai()
        {
            if (cboDauSach.SelectedItem is DauSachDTO ds)
            {
                int sl = (int)numSoLuong.Value;
                decimal thanhTien = PhieuNhapBUS.TinhThanhTien(ds.DonGia, sl);
                txtThanhTien.Text = thanhTien.ToString("N0");
            }
        }

        /// <summary>
        /// Thêm 1 đầu sách vào danh sách chuẩn bị nhập (Detail tạm thời)
        /// </summary>
        private void BtnThemSach_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cboDauSach.SelectedItem is not DauSachDTO ds)
                {
                    MessageBox.Show("Vui lòng chọn một đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int sl = (int)numSoLuong.Value;
                if (sl <= 0)
                {
                    MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra xem đầu sách này đã có trong danh sách chuẩn bị nhập chưa
                var existing = _dsDangNhap.FirstOrDefault(x => x.MaDauSach == ds.MaDauSach);
                if (existing != null)
                {
                    existing.SoLuong += sl;
                    existing.ThanhTien = PhieuNhapBUS.TinhThanhTien(existing.DonGia, existing.SoLuong);
                }
                else
                {
                    decimal thanhTien = PhieuNhapBUS.TinhThanhTien(ds.DonGia, sl);
                    _dsDangNhap.Add(new ChiTietPhieuNhapDTO
                    {
                        MaDauSach = ds.MaDauSach,
                        TenSach = ds.TenSach,
                        DonGia = ds.DonGia,
                        SoLuong = sl,
                        ThanhTien = thanhTien
                    });
                }

                CapNhatLuoiChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dòng sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatLuoiChiTiet()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = new List<ChiTietPhieuNhapDTO>(_dsDangNhap);
            DinhDangCotChiTiet();

            decimal tongTien = _dsDangNhap.Sum(x => x.ThanhTien);
            lblTongTien.Text = $"{tongTien:N0} VNĐ";
        }

        /// <summary>
        /// Lưu phiếu nhập: Gửi toàn bộ Master và Detail sang tầng BUS
        /// </summary>
        private void BtnLuuPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaPN.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaPN.Focus();
                    return;
                }

                if (cboNCC.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_dsDangNhap.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một đầu sách vào phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PhieuNhapDTO pn = new PhieuNhapDTO
                {
                    MaPN = txtMaPN.Text.Trim(),
                    MaNCC = cboNCC.SelectedValue.ToString() ?? "",
                    MaNV = txtMaNV.Text.Trim(),
                    NgayNhap = DateTime.Now
                };

                // Gọi nghiệp vụ tầng BUS xử lý lưu và tăng tồn kho
                var result = PhieuNhapBUS.TaoPhieuNhap(pn, _dsDangNhap);

                MessageBox.Show(result.Message, "Kết quả", MessageBoxButtons.OK, 
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.Success)
                {
                    BtnHuy_Click(sender, e);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi lưu phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            txtMaPN.Text = "";
            txtTimKiemPN.Text = "";
            numSoLuong.Value = 1;
            _dsDangNhap.Clear();
            CapNhatLuoiChiTiet();
            txtMaPN.Focus();
        }

        /// <summary>
        /// Xuất PDF cho Phiếu Nhập đang được chọn trên bảng Master.
        /// </summary>
        private void BtnXuatPDF_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvPhieuNhap.CurrentRow?.DataBoundItem is not PhieuNhapDTO pn)
                {
                    MessageBox.Show("Vui lòng chọn một Phiếu Nhập trên danh sách để xuất PDF!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy chi tiết phiếu từ CSDL qua BUS
                var chiTiet = PhieuNhapBUS.LayChiTietTheoMaPN(pn.MaPN);

                // Tra tên nhà cung cấp theo phiếu đang chọn, không phụ thuộc ComboBox lập phiếu mới.
                string tenNCC = NhaCungCapBUS.LayTenTheoMa(pn.MaNCC);

                ReportHelper.XuatPDFPhieuNhap(pn, chiTiet, tenNCC);

                MessageBox.Show($"Đã xuất PDF Phiếu Nhập {pn.MaPN} ra Desktop!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimKiemPN_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                dgvPhieuNhap.DataSource = PhieuNhapBUS.TimKiem(txtTimKiemPN.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThongKeNhap_Click(object? sender, EventArgs e)
        {
            using frmThongKeNhap form = new frmThongKeNhap();
            form.ShowDialog(this);
        }
    }
}
