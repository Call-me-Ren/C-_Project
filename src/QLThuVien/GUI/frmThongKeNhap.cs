using QLThuVien.BUS;
using QLThuVien.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Form thống kê nhập sách theo Quý. 
    /// Hiển thị bảng chéo: Hàng = Đầu sách, Cột = Q1 | Q2 | Q3 | Q4 | Tổng cộng.
    /// </summary>
    public partial class frmThongKeNhap : Form
    {
        private List<ThongKeQuyDTO> _dsHienTai = new List<ThongKeQuyDTO>();

        public frmThongKeNhap()
        {
            InitializeComponent();
            NapDanhSachNam();
            CauHinhBang();
        }

        // ---- Khởi tạo ----

        private void NapDanhSachNam()
        {
            try
            {
                int namHienTai = DateTime.Now.Year;
                for (int y = namHienTai; y >= namHienTai - 10; y--)
                    cboNam.Items.Add(y);
                cboNam.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách năm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CauHinhBang()
        {
            try
            {
                dgvThongKe.Columns.Clear();
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT",     HeaderText = "STT",         Width = 50,  ReadOnly = true });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenSach", HeaderText = "Đầu sách",    FillWeight = 40, ReadOnly = true });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQ1",      HeaderText = "Quý I (đ)",   FillWeight = 15, ReadOnly = true, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQ2",      HeaderText = "Quý II (đ)",  FillWeight = 15, ReadOnly = true, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQ3",      HeaderText = "Quý III (đ)", FillWeight = 15, ReadOnly = true, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQ4",      HeaderText = "Quý IV (đ)", FillWeight = 15, ReadOnly = true, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
                dgvThongKe.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTong",    HeaderText = "Tổng cộng (đ)", FillWeight = 20, ReadOnly = true, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold) } });
                dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấu hình bảng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---- Hiển thị dữ liệu ----

        private void HienThiBang(List<ThongKeQuyDTO> ds)
        {
            dgvThongKe.Rows.Clear();
            int stt = 1;
            foreach (var r in ds)
            {
                dgvThongKe.Rows.Add(
                    stt++,
                    r.TenSach,
                    r.Q1 == 0 ? "-" : r.Q1.ToString("N0"),
                    r.Q2 == 0 ? "-" : r.Q2.ToString("N0"),
                    r.Q3 == 0 ? "-" : r.Q3.ToString("N0"),
                    r.Q4 == 0 ? "-" : r.Q4.ToString("N0"),
                    r.TongCong.ToString("N0")
                );
            }

            // Dòng tổng cuối
            if (ds.Count > 0)
            {
                decimal tongQ1 = 0, tongQ2 = 0, tongQ3 = 0, tongQ4 = 0, tongTong = 0;
                foreach (var r in ds) { tongQ1 += r.Q1; tongQ2 += r.Q2; tongQ3 += r.Q3; tongQ4 += r.Q4; tongTong += r.TongCong; }

                int lastRow = dgvThongKe.Rows.Add(
                    "", "TỔNG CỘNG",
                    tongQ1.ToString("N0"), tongQ2.ToString("N0"),
                    tongQ3.ToString("N0"), tongQ4.ToString("N0"),
                    tongTong.ToString("N0")
                );

                // Tô màu dòng tổng
                var row = dgvThongKe.Rows[lastRow];
                row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                row.DefaultCellStyle.Font      = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            }
        }

        // ---- Sự kiện ----

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboNam.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn năm thống kê.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int nam = (int)cboNam.SelectedItem;
                _dsHienTai = ThongKeBUS.ThongKeNhapTheoQuy(nam);
                HienThiBang(_dsHienTai);

                if (_dsHienTai.Count == 0)
                    MessageBox.Show($"Không có dữ liệu nhập sách trong năm {nam}.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dsHienTai == null || _dsHienTai.Count == 0)
                {
                    MessageBox.Show("Vui lòng thực hiện thống kê trước khi xuất PDF.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int nam = (int)cboNam.SelectedItem!;
                ReportHelper.XuatPDFThongKeQuy(_dsHienTai, nam);
                MessageBox.Show($"Đã xuất file PDF báo cáo thống kê năm {nam} ra Desktop!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
