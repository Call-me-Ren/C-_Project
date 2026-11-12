using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QLThuVien.BUS;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public partial class frmThongKe : Form
    {
        private readonly ThongKeBUS bus = new ThongKeBUS();

        public frmThongKe()
        {
            InitializeComponent();
            Load += frmThongKe_Load;
            btnThongKe.Click += btnThongKe_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnXuatPdf.Click += btnXuatPdf_Click;
            btnXuatExcel.Click += btnXuatExcel_Click;
            numNamThongKe.ValueChanged += numNamThongKe_ValueChanged;
            pnlTongHop.SizeChanged += (_, _) => DinhDangThe(pnlTongHop);
            DinhDangThe(pnlTongHop);
        }


        private void frmThongKe_Load(object? sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                dtpTuNgay.Value = new DateTime(today.Year, today.Month, 1);
                dtpDenNgay.Value = today;
                NapThongKe();
                NapThongKeQuy();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnThongKe_Click(object? sender, EventArgs e)
        {
            try
            {
                NapThongKe();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                dtpTuNgay.Value = new DateTime(today.Year, today.Month, 1);
                dtpDenNgay.Value = today;
                NapThongKe();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void btnXuatPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                ThongKeBUS.KiemTraKhoangNgay(dtpTuNgay.Value, dtpDenNgay.Value);
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date;
                int namThongKe = decimal.ToInt32(numNamThongKe.Value);
                ThongKeTongHopDTO report = bus.LayBaoCaoThuChi(tuNgay, denNgay, namThongKe);
                using SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "pdf",
                    FileName = $"ThongKe_{tuNgay:yyyy-MM-dd}_{denNgay:yyyy-MM-dd}.pdf",
                    Filter = "Tệp PDF (*.pdf)|*.pdf",
                    OverwritePrompt = true
                };
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                new ThongKePdfExporter(report, tuNgay, denNgay).Xuat(dialog.FileName);
                Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(PdfExportErrors.ToVietnamese(ex), "Không thể xuất báo cáo PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            try
            {
                ThongKeBUS.KiemTraKhoangNgay(dtpTuNgay.Value, dtpDenNgay.Value);
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date;
                int namThongKe = decimal.ToInt32(numNamThongKe.Value);
                ThongKeTongHopDTO report = bus.LayBaoCaoThuChi(tuNgay, denNgay, namThongKe);
                using SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "xlsx",
                    FileName = $"ThongKe_{tuNgay:yyyy-MM-dd}_{denNgay:yyyy-MM-dd}.xlsx",
                    Filter = "Tệp Excel (*.xlsx)|*.xlsx",
                    OverwritePrompt = true
                };
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                new ThongKeExcelExporter().Xuat(report, tuNgay, denNgay, dialog.FileName);
                MessageBox.Show("Đã xuất báo cáo Excel.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xuất tệp Excel lúc này. Hãy thử lại, nếu vẫn lỗi hãy báo cho người quản trị.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapThongKe()
        {
            ThongKeBUS.KiemTraKhoangNgay(dtpTuNgay.Value, dtpDenNgay.Value);
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            ThongKeTongHopDTO tongHop = bus.LayTongHop(tuNgay, denNgay);
            lblTongThu.Text = tongHop.TongThu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            lblTongChi.Text = tongHop.TongChi.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            lblChenhLech.Text = tongHop.ChenhLech.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
            lblChenhLech.ForeColor = tongHop.ChenhLech < 0 ? Color.Firebrick : Color.DarkGreen;
            dgvGiaoDich.DataSource = tongHop.GiaoDich;
            DinhDangLuoi(dgvGiaoDich);
            int soKhoanThu = tongHop.GiaoDich.Count(x => x.Loai == "Thu");
            int soKhoanChi = tongHop.GiaoDich.Count(x => x.Loai == "Chi");
            lblTrangThai.Text = soKhoanThu + soKhoanChi == 0
                ? "Không có dữ liệu trong kỳ"
                : $"Đã thống kê {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}: {soKhoanThu} khoản thu, {soKhoanChi} khoản chi";
        }

        private void numNamThongKe_ValueChanged(object? sender, EventArgs e)
        {
            try
            {
                NapThongKeQuy();
            }
            catch (Exception ex)
            {
                FormErrors.Show(ex);
            }
        }

        private void NapThongKeQuy()
        {
            int nam = decimal.ToInt32(numNamThongKe.Value);
            dgvNhanVienTheoQuy.DataSource = bus.LayThongKeNhanVienTheoQuy(nam);
            DinhDangLuoi(dgvNhanVienTheoQuy);
            dgvDocGiaTheoQuy.DataSource = bus.LayThongKeDocGiaTheoQuy(nam);
            DinhDangLuoi(dgvDocGiaTheoQuy);
            dgvDauSachTheoQuy.DataSource = bus.LayThongKeDauSachTheoQuy(nam);
            DinhDangLuoi(dgvDauSachTheoQuy);
        }





        private static void DinhDangLuoi(DataGridView grid)
        {
            if (!grid.IsHandleCreated)
            {
                grid.HandleCreated -= Grid_HandleCreated;
                grid.HandleCreated += Grid_HandleCreated;
                return;
            }

            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            grid.ScrollBars = ScrollBars.Both;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grid.ColumnHeadersHeight = 52;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.FormatProvider = culture;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.FormatProvider = culture;
                column.MinimumWidth = 88;
                column.FillWeight = 100;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                string propertyName = string.IsNullOrEmpty(column.DataPropertyName) ? column.Name : column.DataPropertyName;
                string? quarterHeader = LayTieuDeQuy(grid.Name, propertyName);
                if (quarterHeader != null) column.HeaderText = quarterHeader;

                if (column.ValueType == typeof(decimal) || column.ValueType == typeof(int) ||
                    column.ValueType == typeof(long) || column.ValueType == typeof(double) ||
                    column.ValueType == typeof(float) || column.Name.Contains("Tong") ||
                    column.Name.Contains("Thu") || column.Name.Contains("Chi") || column.Name.Contains("No"))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (column.ValueType == typeof(DateTime))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                DinhDangDoRongCot(grid.Name, propertyName, column);
            }
            grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private static void Grid_HandleCreated(object? sender, EventArgs e)
        {
            if (sender is not DataGridView grid) return;
            grid.HandleCreated -= Grid_HandleCreated;
            DinhDangLuoi(grid);
        }

        private static string? LayTieuDeQuy(string gridName, string propertyName)
        {
            if (gridName == "dgvNhanVienTheoQuy")
            {
                return propertyName switch
                {
                    "TienPhatQ1" => "Q1\nTiền phạt",
                    "TienNhapQ1" => "Q1\nTiền nhập",
                    "TienPhatQ2" => "Q2\nTiền phạt",
                    "TienNhapQ2" => "Q2\nTiền nhập",
                    "TienPhatQ3" => "Q3\nTiền phạt",
                    "TienNhapQ3" => "Q3\nTiền nhập",
                    "TienPhatQ4" => "Q4\nTiền phạt",
                    "TienNhapQ4" => "Q4\nTiền nhập",
                    "TongTienPhat" => "Tổng tiền\nphạt",
                    "TongTienNhap" => "Tổng tiền\nnhập",
                    _ => null
                };
            }

            if (gridName == "dgvDocGiaTheoQuy")
            {
                return propertyName switch
                {
                    "TienPhatQ1" => "Q1\nTiền phạt",
                    "TienPhatQ2" => "Q2\nTiền phạt",
                    "TienPhatQ3" => "Q3\nTiền phạt",
                    "TienPhatQ4" => "Q4\nTiền phạt",
                    "TongCong" => "Tổng cộng",
                    _ => null
                };
            }

            if (gridName == "dgvDauSachTheoQuy")
            {
                return propertyName switch
                {
                    "TienNhapQ1" => "Q1\nTiền nhập",
                    "LuotMuonQ1" => "Q1\nLượt mượn",
                    "TienNhapQ2" => "Q2\nTiền nhập",
                    "LuotMuonQ2" => "Q2\nLượt mượn",
                    "TienNhapQ3" => "Q3\nTiền nhập",
                    "LuotMuonQ3" => "Q3\nLượt mượn",
                    "TienNhapQ4" => "Q4\nTiền nhập",
                    "LuotMuonQ4" => "Q4\nLượt mượn",
                    "TongTienNhap" => "Tổng tiền\nnhập",
                    "TongLuotMuon" => "Tổng lượt\nmượn",
                    _ => null
                };
            }

            return null;
        }

        private static void DinhDangDoRongCot(string gridName, string propertyName, DataGridViewColumn column)
        {
            if (gridName == "dgvNhanVienTheoQuy")
            {
                if (propertyName == "MaNV")
                {
                    column.MinimumWidth = 84;
                    column.FillWeight = 75;
                }
                else if (propertyName == "TenNhanVien")
                {
                    column.MinimumWidth = 150;
                    column.FillWeight = 160;
                }
                else if (propertyName.StartsWith("Tong", StringComparison.Ordinal))
                {
                    column.MinimumWidth = 120;
                    column.FillWeight = 120;
                }
                else
                {
                    column.MinimumWidth = 108;
                    column.FillWeight = 100;
                }
                return;
            }

            if (gridName == "dgvDocGiaTheoQuy")
            {
                if (propertyName == "MaDocGia")
                {
                    column.MinimumWidth = 92;
                    column.FillWeight = 80;
                }
                else if (propertyName == "TenDocGia")
                {
                    column.MinimumWidth = 180;
                    column.FillWeight = 165;
                }
                else if (propertyName == "TongCong")
                {
                    column.MinimumWidth = 120;
                    column.FillWeight = 115;
                }
                else
                {
                    column.MinimumWidth = 116;
                    column.FillWeight = 100;
                }
                return;
            }

            if (gridName == "dgvDauSachTheoQuy")
            {
                if (propertyName == "MaDauSach")
                {
                    column.MinimumWidth = 105;
                    column.FillWeight = 80;
                }
                else if (propertyName == "TenDauSach")
                {
                    column.MinimumWidth = 190;
                    column.FillWeight = 160;
                }
                else if (propertyName.StartsWith("Tong", StringComparison.Ordinal))
                {
                    column.MinimumWidth = 125;
                    column.FillWeight = 110;
                }
                else if (propertyName.StartsWith("Tien", StringComparison.Ordinal))
                {
                    column.MinimumWidth = 116;
                    column.FillWeight = 100;
                }
                else
                {
                    column.MinimumWidth = 100;
                    column.FillWeight = 85;
                }
                return;
            }

            if (propertyName == "MaGiaoDich") column.MinimumWidth = 105;
            else if (propertyName == "Ngay") column.MinimumWidth = 100;
            else if (propertyName == "Loai") column.MinimumWidth = 80;
            else if (propertyName == "DoiTuong") column.MinimumWidth = 180;
            else if (propertyName == "DienGiai") column.MinimumWidth = 160;
            else if (propertyName == "SoTien") column.MinimumWidth = 115;
        }

        private static void DinhDangThe(FlowLayoutPanel summary)
        {
            if (summary.Controls.Count == 0) return;
            int tongLe = summary.Controls.Cast<Control>().Sum(control => control.Margin.Horizontal);
            int width = Math.Max(240, (summary.ClientSize.Width - tongLe) / summary.Controls.Count);
            foreach (Panel panel in summary.Controls.OfType<Panel>())
            {
                panel.Width = width;
                foreach (Label label in panel.Controls.OfType<Label>())
                    label.Width = width - 24;
            }
        }
    }
}
