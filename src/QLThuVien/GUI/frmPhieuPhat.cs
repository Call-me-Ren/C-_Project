using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    /// <summary>Biểu mẫu GUI lập phiếu phạt và quản lý vi phạm trên cùng màn hình thông qua các lớp BUS.</summary>
    public partial class frmPhieuPhat : Form
    {
        private readonly PhieuPhatBUS busPhieu = new PhieuPhatBUS();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly PhieuPhatBaoCaoBUS busBaoCao = new PhieuPhatBaoCaoBUS();
        private List<LookupOptionDTO> danhSachPhieuMuon = new List<LookupOptionDTO>();
        private List<QuyDinhPhatDTO> danhSachQuyDinh = new List<QuyDinhPhatDTO>();
        private List<SachMuonDTO> danhSachSachTheoPhieu = new List<SachMuonDTO>();
        private string maPhieuDangChon = string.Empty;
        private int? maChiTietDangChon;
        private bool dangNapDuLieu;

        /// <summary>Khởi tạo biểu mẫu và gắn sự kiện cho dữ liệu phiếu, chi tiết, thanh toán và xuất báo cáo.</summary>
        public frmPhieuPhat()
        {
            InitializeComponent();
            Load += frmPhieuPhat_Load;
            cboDocGia.SelectedValueChanged += cboDocGia_SelectedValueChanged;
            cboSach.SelectedValueChanged += cboSach_SelectedValueChanged;
            cboQuyDinh.SelectedValueChanged += cboQuyDinh_SelectedValueChanged;
            btnChonDocGia.Click += btnChonDocGia_Click;
            btnChonNhanVien.Click += btnChonNhanVien_Click;
            dgvPhieuPhat.CellClick += dgvPhieuPhat_CellClick;
            dgvChiTiet.CellClick += dgvChiTiet_CellClick;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            btnThemPhieu.Click += btnThemPhieu_Click;
            btnSuaPhieu.Click += btnSuaPhieu_Click;
            btnXoaPhieu.Click += btnXoaPhieu_Click;
            btnThanhToan.Click += btnThanhToan_Click;
            btnLamMoiPhieu.Click += btnLamMoiPhieu_Click;
            btnXuatPdf.Click += btnXuatPdf_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            btnTatCa.Click += btnTatCa_Click;
            btnThemViPham.Click += btnThemViPham_Click;
            btnSuaViPham.Click += btnSuaViPham_Click;
            btnXoaViPham.Click += btnXoaViPham_Click;
            btnLamMoiChiTiet.Click += btnLamMoiChiTiet_Click;
        }

        /// <summary>Nạp lựa chọn độc giả, phiếu mượn, nhân viên, quy định và danh sách phiếu phạt.</summary>
        /// <param name="sender">Đối tượng phát sinh sự kiện nạp biểu mẫu.</param>
        /// <param name="e">Thông tin sự kiện nạp biểu mẫu.</param>
        /// <remarks>Các danh sách tra cứu được nạp trước khi chọn phiếu để có thể ghép tên sách và quy định vào bảng chi tiết.</remarks>
        private void frmPhieuPhat_Load(object? sender, EventArgs e)
        {
            try
            {
                dangNapDuLieu = true;
                cboDocGia.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboDocGia.ValueMember = nameof(LookupOptionDTO.Ma);
                cboDocGia.DataSource = busPhieu.LayDanhSachDocGiaLookup();

                danhSachPhieuMuon = busPhieu.LayDanhSachPhieuMuonLookup();
                cboNhanVien.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboNhanVien.ValueMember = nameof(LookupOptionDTO.Ma);
                cboNhanVien.DataSource = busNhanVien.LayDanhSach(reload: true)
                    .Select(nv => new LookupOptionDTO(nv.MaNV, $"{nv.MaNV} - {nv.HoTen}"))
                    .ToList();

                danhSachQuyDinh = busPhieu.LayDanhSachQuyDinh();
                cboQuyDinh.DisplayMember = nameof(LookupOptionDTO.HienThi);
                cboQuyDinh.ValueMember = nameof(LookupOptionDTO.Ma);
                cboQuyDinh.DataSource = danhSachQuyDinh
                    .Select(qd => new LookupOptionDTO(qd.MaQD, $"{qd.MaQD} - {qd.TenQD}"))
                    .ToList();

                dangNapDuLieu = false;
                HienThiDanhSach(busPhieu.DocDS());
            }
            catch (Exception ex)
            {
                dangNapDuLieu = false;
                HienThiLoi(ex);
            }
        }

        /// <summary>Nạp các phiếu mượn thuộc độc giả vừa chọn.</summary>
        /// <param name="sender">Ô chọn độc giả phát sinh thay đổi.</param>
        /// <param name="e">Thông tin thay đổi lựa chọn.</param>
        private void cboDocGia_SelectedValueChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dangNapDuLieu) return;
                NapDanhSachPhieuMuon(GiaTri(cboDocGia));
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Cập nhật phần tiền dự kiến khi đổi cuốn sách đang chọn.</summary>
        /// <param name="sender">Ô chọn cuốn sách phát sinh thay đổi.</param>
        /// <param name="e">Thông tin thay đổi lựa chọn.</param>
        private void cboSach_SelectedValueChanged(object? sender, EventArgs e)
        {
            try
            {
                if (!dangNapDuLieu) CapNhatTienDuKien();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Cập nhật phần tiền dự kiến khi đổi quy định phạt.</summary>
        /// <param name="sender">Ô chọn quy định phát sinh thay đổi.</param>
        /// <param name="e">Thông tin thay đổi lựa chọn.</param>
        private void cboQuyDinh_SelectedValueChanged(object? sender, EventArgs e)
        {
            try
            {
                if (!dangNapDuLieu) CapNhatTienDuKien();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Mở cửa sổ tra cứu độc giả và áp dụng độc giả được xác nhận.</summary>
        /// <param name="sender">Nút chọn độc giả.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Sau khi chọn độc giả, danh sách phiếu mượn được giới hạn lại theo mã độc giả đó.</remarks>
        private void btnChonDocGia_Click(object? sender, EventArgs e)
        {
            try
            {
                frmTimKiemDocGiaPopup.SelectedMaDocGia = string.Empty;
                frmTimKiemDocGiaPopup.SelectedTenDocGia = string.Empty;
                using frmTimKiemDocGiaPopup popup = new frmTimKiemDocGiaPopup();
                if (popup.ShowDialog(this) == DialogResult.OK &&
                    !string.IsNullOrEmpty(frmTimKiemDocGiaPopup.SelectedMaDocGia))
                {
                    cboDocGia.SelectedValue = frmTimKiemDocGiaPopup.SelectedMaDocGia;
                    NapDanhSachPhieuMuon(GiaTri(cboDocGia));
                }
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Mở cửa sổ chọn nhân viên và gán nhân viên được xác nhận vào phiếu.</summary>
        /// <param name="sender">Nút chọn nhân viên.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnChonNhanVien_Click(object? sender, EventArgs e)
        {
            try
            {
                FormSharedData.SelectedMaNV = null;
                FormSharedData.SelectedTenNV = null;
                using frmChonNhanVienPopup popup = new frmChonNhanVienPopup();
                if (popup.ShowDialog(this) == DialogResult.OK &&
                    !string.IsNullOrEmpty(FormSharedData.SelectedMaNV))
                {
                    cboNhanVien.SelectedValue = FormSharedData.SelectedMaNV;
                }
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        private void NapDanhSachPhieuMuon(string? maDocGia, string? maPhieuMuon = null)
        {
            List<LookupOptionDTO> phieuMuon = string.IsNullOrWhiteSpace(maDocGia)
                ? new List<LookupOptionDTO>()
                : danhSachPhieuMuon
                    .Where(pm => string.Equals(pm.MaDocGia, maDocGia, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            cboPhieuMuon.DataSource = null;
            cboPhieuMuon.DisplayMember = nameof(LookupOptionDTO.HienThi);
            cboPhieuMuon.ValueMember = nameof(LookupOptionDTO.Ma);
            cboPhieuMuon.DataSource = phieuMuon;
            cboPhieuMuon.SelectedIndex = -1;
            if (!string.IsNullOrWhiteSpace(maPhieuMuon)) cboPhieuMuon.SelectedValue = maPhieuMuon;
        }

        private void HienThiDanhSach(List<PhieuPhatDTO> danhSach, string? maCanChon = null)
        {
            dgvPhieuPhat.DataSource = null;
            dgvPhieuPhat.DataSource = danhSach;
            dgvPhieuPhat.ClearSelection();
            dgvPhieuPhat.CurrentCell = null;

            if (!string.IsNullOrWhiteSpace(maCanChon))
            {
                foreach (DataGridViewRow row in dgvPhieuPhat.Rows)
                {
                    if (row.DataBoundItem is PhieuPhatDTO pp &&
                        string.Equals(pp.MaPhieuPhat, maCanChon, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        if (row.Cells.Count > 0) dgvPhieuPhat.CurrentCell = row.Cells[0];
                        NapPhieuLenForm(pp);
                        return;
                    }
                }
            }

            LamMoiPhieu();
        }

        /// <summary>Hiển thị danh sách vi phạm của phiếu được chọn ngay trên bảng chi tiết.</summary>
        /// <param name="sender">Bảng phiếu phạt phát sinh thao tác chọn dòng.</param>
        /// <param name="e">Vị trí dòng được chọn; dòng tiêu đề bị bỏ qua.</param>
        /// <remarks>Chọn một dòng sẽ nạp thông tin phiếu và lấy chi tiết tương ứng từ BUS, không mở cửa sổ riêng.</remarks>
        private void dgvPhieuPhat_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvPhieuPhat.Rows.Count) return;
                if (dgvPhieuPhat.Rows[e.RowIndex].DataBoundItem is PhieuPhatDTO pp) NapPhieuLenForm(pp);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Đưa đầu phiếu vào vùng nhập, tải sách thuộc phiếu mượn và hiển thị các vi phạm hiện có.</summary>
        /// <param name="pp">Phiếu đang được chọn trong bảng chính.</param>
        /// <remarks>Việc nạp dữ liệu được chặn sự kiện tính trước; sau đó vùng chi tiết được bật và tổng được dựng từ các dòng hiện có.</remarks>
        private void NapPhieuLenForm(PhieuPhatDTO pp)
        {
            dangNapDuLieu = true;
            maPhieuDangChon = pp.MaPhieuPhat;
            txtMaPhieuPhat.Text = pp.MaPhieuPhat;
            cboDocGia.SelectedValue = pp.MaDocGia;
            NapDanhSachPhieuMuon(pp.MaDocGia, pp.MaPhieuMuon);
            cboNhanVien.SelectedValue = pp.MaNV;
            dtpNgayLap.Value = pp.NgayLap;
            cboTrangThai.SelectedItem = pp.TrangThai;

            danhSachSachTheoPhieu = busPhieu.LaySachTheoPhieuMuon(pp.MaPhieuMuon ?? string.Empty);
            cboSach.DataSource = null;
            cboSach.DisplayMember = nameof(SachMuonDTO.HienThi);
            cboSach.ValueMember = nameof(SachMuonDTO.MaSach);
            cboSach.DataSource = danhSachSachTheoPhieu;
            cboSach.SelectedIndex = danhSachSachTheoPhieu.Count == 0 ? -1 : 0;
            cboQuyDinh.SelectedIndex = cboQuyDinh.Items.Count == 0 ? -1 : 0;
            dangNapDuLieu = false;

            grpChiTiet.Enabled = true;
            btnThemViPham.Enabled = danhSachSachTheoPhieu.Count > 0 && danhSachQuyDinh.Count > 0;
            btnSuaViPham.Enabled = false;
            btnXoaViPham.Enabled = false;
            HienThiChiTiet(busPhieu.LayChiTietTheoPhieu(pp.MaPhieuPhat));
            LamMoiDongChiTiet();
        }

        /// <summary>Ghép tên sách và quy định vào dữ liệu hiển thị, rồi cập nhật bảng và tổng tiền.</summary>
        /// <param name="danhSach">Các vi phạm của phiếu đang chọn.</param>
        /// <remarks>Tổng hiển thị là tổng TienPhat của các dòng đang dựng, định dạng VNĐ theo nhóm nghìn.</remarks>
        private void HienThiChiTiet(List<ChiTietPhieuPhatDTO> danhSach)
        {
            List<ChiTietHienThiDTO> dongHienThi = danhSach.Select(ct =>
            {
                SachMuonDTO? sach = danhSachSachTheoPhieu.FirstOrDefault(item =>
                    string.Equals(item.MaSach, ct.MaSach, StringComparison.OrdinalIgnoreCase));
                QuyDinhPhatDTO? quyDinh = danhSachQuyDinh.FirstOrDefault(item =>
                    string.Equals(item.MaQD, ct.MaQD, StringComparison.OrdinalIgnoreCase));
                return new ChiTietHienThiDTO
                {
                    MaCTPP = ct.MaCTPP,
                    MaPhieuPhat = ct.MaPhieuPhat,
                    MaQD = ct.MaQD,
                    MaSach = ct.MaSach,
                    TenSach = sach?.TenSach ?? ct.MaSach,
                    QuyDinh = quyDinh == null ? ct.MaQD : $"{quyDinh.MaQD} - {quyDinh.TenQD}",
                    SoNgayTre = ct.SoNgayTre,
                    DonGia = ct.DonGia,
                    TienPhat = ct.TienPhat,
                    GhiChu = ct.GhiChu ?? string.Empty
                };
            }).ToList();

            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = dongHienThi;
            decimal tongTien = dongHienThi.Sum(ct => ct.TienPhat);
            lblTongTien.Text = $"Tổng tiền phạt: {tongTien:N0} VNĐ";
        }

        /// <summary>Đưa sách, quy định và ghi chú của dòng chi tiết được chọn vào vùng nhập.</summary>
        /// <param name="sender">Bảng chi tiết phát sinh thao tác chọn dòng.</param>
        /// <param name="e">Vị trí dòng được chọn; dòng tiêu đề bị bỏ qua.</param>
        private void dgvChiTiet_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvChiTiet.Rows.Count) return;
                if (dgvChiTiet.Rows[e.RowIndex].DataBoundItem is not ChiTietHienThiDTO ct) return;

                maChiTietDangChon = ct.MaCTPP;
                cboSach.SelectedValue = ct.MaSach;
                cboQuyDinh.SelectedValue = ct.MaQD;
                txtGhiChu.Text = ct.GhiChu;
                btnSuaViPham.Enabled = true;
                btnXoaViPham.Enabled = true;
                CapNhatTienDuKien();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Lập phiếu mới từ thông tin trên biểu mẫu và chọn phiếu vừa lưu.</summary>
        /// <param name="sender">Nút thêm phiếu phạt.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>BUS lưu đầu phiếu trước; sau khi nạp lại, dòng mới được chọn để có thể thêm vi phạm ngay.</remarks>
        private void btnThemPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                PhieuPhatDTO pp = TaoPhieuTuForm();
                if (!busPhieu.ThemPhieu(pp)) throw new BusinessRuleException("Không thể lập phiếu phạt.");
                TaiLaiVaChonPhieu(pp.MaPhieuPhat, xoaTuKhoa: true);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Cập nhật độc giả, phiếu mượn, nhân viên, ngày lập hoặc trạng thái của phiếu đang chọn.</summary>
        /// <param name="sender">Nút sửa phiếu phạt.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Mã phiếu lấy từ dòng đang chọn; BUS giữ tổng tiền từ các chi tiết đã lưu và kiểm tra liên kết trước khi cập nhật.</remarks>
        private void btnSuaPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new BusinessRuleException("Hãy chọn phiếu phạt cần sửa.");

                PhieuPhatDTO pp = TaoPhieuTuForm();
                pp.MaPhieuPhat = maPhieuDangChon;
                pp.TongTienPhat = 0;
                if (!busPhieu.SuaPhieu(pp)) throw new BusinessRuleException("Không thể cập nhật phiếu phạt.");
                TaiLaiVaChonPhieu(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Yêu cầu xác nhận rồi xóa phiếu phạt đang chọn qua BUS.</summary>
        /// <param name="sender">Nút xóa phiếu phạt.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Nếu người dùng hủy xác nhận thì dữ liệu được giữ nguyên; sau khi xóa thành công, bảng được nạp lại và vùng chi tiết khóa.</remarks>
        private void btnXoaPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new BusinessRuleException("Hãy chọn phiếu phạt cần xóa.");

                DialogResult confirm = MessageBox.Show(
                    $"Xóa phiếu {maPhieuDangChon} và các vi phạm của phiếu?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                if (!busPhieu.XoaPhieu(maPhieuDangChon)) throw new BusinessRuleException("Không thể xóa phiếu phạt.");
                TaiLaiVaChonPhieu(null);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Ghi nhận trạng thái Đã thanh toán cho phiếu đang chọn và tải lại đúng phiếu đó.</summary>
        /// <param name="sender">Nút thanh toán phiếu phạt.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Thao tác này cập nhật trạng thái phiếu qua BUS; nếu phiếu đã thanh toán thì chỉ hiển thị thông báo.</remarks>
        private void btnThanhToan_Click(object? sender, EventArgs e)
        {
            try
            {
                PhieuPhatDTO? selected = dgvPhieuPhat.CurrentRow?.DataBoundItem as PhieuPhatDTO;
                if (selected == null) throw new BusinessRuleException("Hãy chọn phiếu phạt cần thanh toán.");
                if (string.Equals(selected.TrangThai, "Đã thanh toán", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Phiếu phạt này đã được thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PhieuPhatDTO phieuDaThanhToan = new PhieuPhatDTO(
                    selected.MaPhieuPhat,
                    selected.MaDocGia,
                    selected.MaPhieuMuon,
                    selected.MaNV,
                    selected.NgayLap,
                    "Đã thanh toán",
                    selected.TongTienPhat);
                if (!busPhieu.SuaPhieu(phieuDaThanhToan)) throw new BusinessRuleException("Không thể ghi nhận thanh toán.");
                TaiLaiVaChonPhieu(selected.MaPhieuPhat);
                MessageBox.Show("Đã ghi nhận thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Đọc lại các phiếu từ BUS và giữ cách hiển thị mặc định của bảng.</summary>
        /// <param name="sender">Nút làm mới phiếu.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnLamMoiPhieu_Click(object? sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(busPhieu.DocDS());
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Lấy dữ liệu phiếu đã chọn, cho người dùng chọn nơi lưu rồi xuất và mở tệp PDF.</summary>
        /// <param name="sender">Nút xuất PDF phiếu phạt.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Nếu hủy hộp thoại lưu thì không tạo tệp; lỗi cơ sở dữ liệu, đường dẫn hoặc quyền ghi được chuyển thành thông báo tiếng Việt.</remarks>
        private void btnXuatPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                    throw new BusinessRuleException("Hãy chọn phiếu phạt cần xuất.");

                PhieuPhatBaoCaoDTO report = busBaoCao.LayTheoMa(maPhieuDangChon);
                using SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "pdf",
                    FileName = PhieuPhatBaoCaoBUS.TenFileGoiY(report.MaPhieuPhat),
                    Filter = "Tệp PDF (*.pdf)|*.pdf",
                    OverwritePrompt = true
                };
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                new PhieuPhatPdfExporter(report).Xuat(dialog.FileName);
                Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(PdfExportErrors.ToVietnamese(ex), "Không thể xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Tìm phiếu phạt theo nội dung trong ô tìm kiếm và cập nhật bảng chính.</summary>
        /// <param name="sender">Nút tìm kiếm phiếu.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                HienThiDanhSach(busPhieu.TimKiemPhieu(txtTimKiem.Text));
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Xóa từ khóa và tải lại toàn bộ danh sách phiếu.</summary>
        /// <param name="sender">Nút hiện tất cả phiếu.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnTatCa_Click(object? sender, EventArgs e)
        {
            try
            {
                txtTimKiem.Clear();
                HienThiDanhSach(busPhieu.LayDanhSach(reload: true));
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Thực hiện tìm kiếm phiếu khi người dùng nhấn Enter trong ô tìm kiếm.</summary>
        /// <param name="sender">Ô tìm kiếm phát sinh sự kiện bàn phím.</param>
        /// <param name="e">Phím vừa nhấn; chỉ phím Enter được xử lý.</param>
        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Tạo và lưu một vi phạm cho phiếu đang chọn, sau đó nạp lại chi tiết và tổng tiền.</summary>
        /// <param name="sender">Nút thêm vi phạm.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Thông tin tiền và số ngày được BUS tính lại; không dùng các giá trị nhập trực tiếp từ vùng hiển thị dự kiến.</remarks>
        private void btnThemViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuPhatDTO chiTiet = TaoChiTiet();
                if (!busPhieu.ThemChiTiet(chiTiet)) throw new BusinessRuleException("Không thể thêm vi phạm.");
                TaiLaiVaChonPhieu(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Cập nhật vi phạm đang chọn theo sách, quy định và ghi chú hiện tại.</summary>
        /// <param name="sender">Nút sửa vi phạm.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Sau khi BUS lưu thành công, chi tiết cùng tổng tiền được tải lại và dòng phiếu chính được giữ chọn.</remarks>
        private void btnSuaViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!maChiTietDangChon.HasValue)
                    throw new BusinessRuleException("Hãy chọn vi phạm cần sửa.");

                ChiTietPhieuPhatDTO chiTiet = TaoChiTiet();
                if (!busPhieu.SuaChiTiet(maChiTietDangChon.Value, chiTiet))
                    throw new BusinessRuleException("Không thể cập nhật vi phạm.");
                TaiLaiVaChonPhieu(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Yêu cầu xác nhận rồi xóa vi phạm đang chọn qua BUS.</summary>
        /// <param name="sender">Nút xóa vi phạm.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        /// <remarks>Nếu người dùng hủy xác nhận thì dữ liệu không đổi; khi xóa thành công, tổng tiền được nạp lại.</remarks>
        private void btnXoaViPham_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!maChiTietDangChon.HasValue)
                    throw new BusinessRuleException("Hãy chọn vi phạm cần xóa.");

                DialogResult confirm = MessageBox.Show("Xóa vi phạm này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                if (!busPhieu.XoaChiTiet(maChiTietDangChon.Value))
                    throw new BusinessRuleException("Không thể xóa vi phạm.");
                TaiLaiVaChonPhieu(maPhieuDangChon);
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Xóa lựa chọn vi phạm và đặt lại vùng nhập để tạo một dòng mới trên phiếu hiện hành.</summary>
        /// <param name="sender">Nút làm mới vùng chi tiết.</param>
        /// <param name="e">Thông tin sự kiện nhấn nút.</param>
        private void btnLamMoiChiTiet_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieuDangChon)) return;
                LamMoiDongChiTiet();
            }
            catch (Exception ex)
            {
                HienThiLoi(ex);
            }
        }

        /// <summary>Lấy mã sách, mã quy định và ghi chú từ vùng nhập rồi nhờ BUS dựng chi tiết đã tính tiền.</summary>
        /// <returns>DTO vi phạm có số ngày trễ, đơn giá và tiền phạt do BUS xác định.</returns>
        /// <exception cref="BusinessRuleException">Chưa chọn phiếu hoặc thông tin sách/quy định không hợp lệ.</exception>
        private ChiTietPhieuPhatDTO TaoChiTiet()
        {
            if (string.IsNullOrWhiteSpace(maPhieuDangChon))
                throw new BusinessRuleException("Hãy chọn phiếu phạt trước.");

            string maSach = GiaTri(cboSach) ?? string.Empty;
            string maQD = GiaTri(cboQuyDinh) ?? string.Empty;
            return busPhieu.TinhChiTietPhat(maPhieuDangChon, maQD, maSach, txtGhiChu.Text);
        }

        /// <summary>Tính và hiển thị trước số ngày trễ, đơn giá, tiền phạt theo sách và quy định đang chọn.</summary>
        /// <remarks>Việc tính được chuyển cho BUS để dùng cùng quy tắc với lúc lưu; khi dữ liệu chưa đủ hoặc không hợp lệ, các ô dự kiến được xóa.</remarks>
        private void CapNhatTienDuKien()
        {
            if (dangNapDuLieu || string.IsNullOrWhiteSpace(maPhieuDangChon) ||
                cboSach.SelectedValue == null || cboQuyDinh.SelectedValue == null)
            {
                XoaTienDuKien();
                return;
            }

            try
            {
                ChiTietPhieuPhatDTO chiTiet = TaoChiTiet();
                txtSoNgayTre.Text = chiTiet.SoNgayTre?.ToString() ?? string.Empty;
                txtDonGia.Text = chiTiet.DonGia.ToString("N0");
                txtTienPhat.Text = chiTiet.TienPhat.ToString("N0");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                XoaTienDuKien();
            }
        }

        private void XoaTienDuKien()
        {
            txtSoNgayTre.Clear();
            txtDonGia.Clear();
            txtTienPhat.Clear();
        }

        private void LamMoiDongChiTiet()
        {
            maChiTietDangChon = null;
            if (cboSach.Items.Count > 0) cboSach.SelectedIndex = 0;
            if (cboQuyDinh.Items.Count > 0) cboQuyDinh.SelectedIndex = 0;
            txtGhiChu.Clear();
            dgvChiTiet.ClearSelection();
            btnSuaViPham.Enabled = false;
            btnXoaViPham.Enabled = false;
            btnThemViPham.Enabled = danhSachSachTheoPhieu.Count > 0 && danhSachQuyDinh.Count > 0;
            CapNhatTienDuKien();
        }

        /// <summary>Nạp lại danh sách và chọn lại phiếu theo mã sau thao tác thêm, sửa hoặc xóa chi tiết.</summary>
        /// <param name="maCanChon">Mã phiếu cần giữ lựa chọn; <see langword="null"/> để bỏ lựa chọn.</param>
        /// <param name="xoaTuKhoa"><see langword="true"/> để xóa từ khóa trước khi tìm lại phiếu.</param>
        /// <remarks>Nếu bộ lọc hiện tại ẩn phiếu cần chọn, bộ lọc được xóa để bảng vẫn giữ đúng phiếu vừa thao tác.</remarks>
        private void TaiLaiVaChonPhieu(string? maCanChon, bool xoaTuKhoa = false)
        {
            List<PhieuPhatDTO> tatCa = busPhieu.DocDS();
            if (xoaTuKhoa) txtTimKiem.Clear();

            List<PhieuPhatDTO> danhSach = string.IsNullOrWhiteSpace(txtTimKiem.Text)
                ? tatCa
                : busPhieu.TimKiemPhieu(txtTimKiem.Text);
            if (!string.IsNullOrWhiteSpace(maCanChon) &&
                !danhSach.Any(pp => string.Equals(pp.MaPhieuPhat, maCanChon, StringComparison.OrdinalIgnoreCase)))
            {
                txtTimKiem.Clear();
                danhSach = tatCa;
            }

            HienThiDanhSach(danhSach, maCanChon);
        }

        /// <summary>Đặt biểu mẫu về trạng thái lập phiếu mới và khóa vùng vi phạm khi chưa có phiếu được chọn.</summary>
        private void LamMoiPhieu()
        {
            dangNapDuLieu = true;
            maPhieuDangChon = string.Empty;
            maChiTietDangChon = null;
            txtMaPhieuPhat.Text = busPhieu.TaoMaPhieuTiepTheo();
            cboDocGia.SelectedIndex = -1;
            NapDanhSachPhieuMuon(null);
            cboNhanVien.SelectedIndex = -1;
            dtpNgayLap.Value = DateTime.Today;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            cboSach.DataSource = null;
            danhSachSachTheoPhieu = new List<SachMuonDTO>();
            dgvPhieuPhat.ClearSelection();
            dgvChiTiet.DataSource = null;
            lblTongTien.Text = "Tổng tiền phạt: 0 VNĐ";
            grpChiTiet.Enabled = false;
            dangNapDuLieu = false;
        }

        /// <summary>Tạo DTO đầu phiếu từ các trường nhập hiện tại.</summary>
        /// <returns>Phiếu phạt chưa có tổng tiền chi tiết; BUS sinh mã và kiểm tra trước khi lưu.</returns>
        private PhieuPhatDTO TaoPhieuTuForm()
        {
            return new PhieuPhatDTO(
                txtMaPhieuPhat.Text.Trim(),
                GiaTri(cboDocGia) ?? string.Empty,
                GiaTri(cboPhieuMuon),
                GiaTri(cboNhanVien) ?? string.Empty,
                dtpNgayLap.Value,
                cboTrangThai.SelectedItem?.ToString() ?? string.Empty,
                0);
        }

        private static string? GiaTri(ComboBox combo)
        {
            return combo.SelectedValue?.ToString();
        }

        private static void HienThiLoi(Exception ex)
        {
            FormErrors.Show(ex);
        }

        /// <summary>DTO trung gian cung cấp các cột đã ghép tên cho bảng chi tiết vi phạm.</summary>
        private sealed class ChiTietHienThiDTO
        {
            /// <summary>Mã dòng vi phạm dùng để sửa hoặc xóa.</summary>
            public int MaCTPP { get; set; }
            /// <summary>Mã phiếu phạt, cột kỹ thuật không hiển thị trên bảng.</summary>
            public string MaPhieuPhat { get; set; } = string.Empty;
            /// <summary>Mã quy định phạt, cột kỹ thuật dùng để nạp lại lựa chọn.</summary>
            public string MaQD { get; set; } = string.Empty;
            /// <summary>Mã cuốn sách hiển thị trên bảng.</summary>
            public string MaSach { get; set; } = string.Empty;
            /// <summary>Tên đầu sách đã ghép từ danh sách sách thuộc phiếu mượn.</summary>
            public string TenSach { get; set; } = string.Empty;
            /// <summary>Mã và tên quy định dùng để hiển thị cho người dùng.</summary>
            public string QuyDinh { get; set; } = string.Empty;
            /// <summary>Số ngày trễ; rỗng với quy định không áp dụng theo ngày.</summary>
            public int? SoNgayTre { get; set; }
            /// <summary>Đơn giá hoặc mức áp dụng, tính bằng VNĐ trừ trường hợp dùng hệ số.</summary>
            public decimal DonGia { get; set; }
            /// <summary>Tiền phạt của dòng, tính bằng VNĐ.</summary>
            public decimal TienPhat { get; set; }
            /// <summary>Ghi chú của vi phạm, có thể rỗng.</summary>
            public string GhiChu { get; set; } = string.Empty;
        }
    }
}
