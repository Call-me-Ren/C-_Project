using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLThuVien.GUI
{
    partial class frmThongKe
    {
        private readonly DateTimePicker dtpTuNgay = new DateTimePicker();
        private readonly DateTimePicker dtpDenNgay = new DateTimePicker();
        private readonly Button btnThongKe = new Button();
        private readonly Button btnLamMoi = new Button();
        private readonly Button btnXuatPdf = new Button();
        private readonly Label lblTrangThai = new Label();
        private readonly TabControl tabsThongKe = new TabControl();
        private readonly Label lblTongThu = new Label();
        private readonly Label lblTongChi = new Label();
        private readonly Label lblChenhLech = new Label();
        private readonly DataGridView dgvGiaoDich = TaoLuoi();
        private readonly NumericUpDown numNamThongKe = new NumericUpDown();
        private readonly DataGridView dgvNhanVienTheoQuy = TaoLuoi();
        private readonly DataGridView dgvDocGiaTheoQuy = TaoLuoi();
        private readonly FlowLayoutPanel pnlTongHop = new FlowLayoutPanel();

        private void InitializeComponent()
        {
            Text = "Thống kê thu chi";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1100, 720);
            ClientSize = new Size(1320, 860);
            Font = new Font("Segoe UI", 9F);

            Label title = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 19F, FontStyle.Bold),
                Location = new Point(20, 14),
                Size = new Size(1260, 42),
                Text = "THỐNG KÊ THU - CHI"
            };

            GroupBox filters = new GroupBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(18, 64),
                Size = new Size(1284, 94),
                Text = "Khoảng thời gian"
            };
            Label fromLabel = new Label { AutoSize = true, Location = new Point(18, 34), Text = "Từ ngày:" };
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.MaxDate = DateTime.Today;
            dtpTuNgay.Location = new Point(82, 30);
            dtpTuNgay.Size = new Size(140, 25);
            Label toLabel = new Label { AutoSize = true, Location = new Point(250, 34), Text = "Đến ngày:" };
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.MaxDate = DateTime.Today;
            dtpDenNgay.Location = new Point(322, 30);
            dtpDenNgay.Size = new Size(140, 25);
            btnThongKe.Location = new Point(485, 27);
            btnThongKe.Size = new Size(110, 31);
            btnThongKe.Text = "Thống kê";
            btnLamMoi.Location = new Point(606, 27);
            btnLamMoi.Size = new Size(110, 31);
            btnLamMoi.Text = "Đặt lại kỳ";
            btnXuatPdf.Location = new Point(720, 27);
            btnXuatPdf.Size = new Size(145, 31);
            btnXuatPdf.Text = "Xuất báo cáo PDF";
            lblTrangThai.AutoEllipsis = true;
            lblTrangThai.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            lblTrangThai.Location = new Point(485, 64);
            lblTrangThai.Size = new Size(770, 20);
            lblTrangThai.ForeColor = Color.DimGray;
            filters.Controls.AddRange(new Control[] { fromLabel, dtpTuNgay, toLabel, dtpDenNgay, btnThongKe, btnLamMoi, btnXuatPdf, lblTrangThai });

            numNamThongKe.Name = "numNamThongKe";
            numNamThongKe.Maximum = DateTime.Today.Year + 1;
            numNamThongKe.Minimum = 2020;
            numNamThongKe.Value = DateTime.Today.Year;
            numNamThongKe.ReadOnly = false;
            numNamThongKe.Enabled = true;
            numNamThongKe.Width = 110;
            dgvNhanVienTheoQuy.Name = "dgvNhanVienTheoQuy";
            dgvDocGiaTheoQuy.Name = "dgvDocGiaTheoQuy";

            pnlTongHop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlTongHop.Location = new Point(18, 166);
            pnlTongHop.Size = new Size(1284, 82);
            pnlTongHop.WrapContents = false;
            pnlTongHop.Controls.Add(TaoThe("Tổng thu", lblTongThu));
            pnlTongHop.Controls.Add(TaoThe("Tổng chi", lblTongChi));
            pnlTongHop.Controls.Add(TaoThe("Chênh lệch", lblChenhLech));

            tabsThongKe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabsThongKe.Location = new Point(18, 260);
            tabsThongKe.Size = new Size(1284, 578);
            tabsThongKe.TabPages.Add(TaoTrang("Thu - Chi", dgvGiaoDich));
            tabsThongKe.TabPages.Add(TaoTrangQuy());

            Controls.AddRange(new Control[] { title, filters, pnlTongHop, tabsThongKe });
        }

        private TabPage TaoTrangQuy()
        {
            TabPage page = new TabPage("Thống kê quý");
            Panel filter = new Panel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(10, 10, 8, 4) };
            Label yearLabel = new Label { AutoSize = true, Location = new Point(10, 14), Text = "Năm:" };
            numNamThongKe.Location = new Point(54, 10);
            TabControl quarterTabs = new TabControl { Dock = DockStyle.Fill, Name = "tabsThongKeQuy" };
            quarterTabs.TabPages.Add(TaoTrang("Nhân viên", dgvNhanVienTheoQuy));
            quarterTabs.TabPages.Add(TaoTrang("Độc giả", dgvDocGiaTheoQuy));
            filter.Controls.AddRange(new Control[] { yearLabel, numNamThongKe });
            page.Controls.Add(quarterTabs);
            page.Controls.Add(filter);
            return page;
        }

        private static TabPage TaoTrang(string title, DataGridView grid)
        {
            TabPage page = new TabPage(title);
            grid.Dock = DockStyle.Fill;
            page.Controls.Add(grid);
            return page;
        }

        private static Panel TaoThe(string title, Label value, int width = 390)
        {
            Panel panel = new Panel
            {
                BackColor = Color.FromArgb(246, 248, 251),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 14, 0),
                Size = new Size(width, 78)
            };
            Label caption = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(12, 8),
                Size = new Size(width - 24, 20),
                Text = title
            };
            value.AutoSize = false;
            value.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            value.Location = new Point(12, 31);
            value.Size = new Size(width - 24, 34);
            value.Text = "0 đ";
            value.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.AddRange(new Control[] { caption, value });
            return panel;
        }

        private static DataGridView TaoLuoi()
        {
            return new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                ColumnHeadersHeight = 34,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }
    }
}
