using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public sealed class ThongKePdfExporter
    {
        private readonly ThongKeTongHopDTO thongKe;
        private readonly DateTime tuNgay;
        private readonly DateTime denNgay;
        private readonly List<BangBaoCao> cacBang;
        private int chiSoBang;
        private int chiSoDong;
        private int soTrang = 1;
        private bool daVeTieuDe;
        private bool veLaiTieuDeBang = true;

        public ThongKePdfExporter(ThongKeTongHopDTO thongKe, DateTime tuNgay, DateTime denNgay)
        {
            this.thongKe = thongKe ?? throw new ArgumentNullException(nameof(thongKe));
            this.tuNgay = tuNgay.Date;
            this.denNgay = denNgay.Date;
            cacBang = TaoCacBang();
        }

        public void Xuat(string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan)) throw new ArgumentException("Đường dẫn file không hợp lệ.", nameof(duongDan));
            const string printerName = "Microsoft Print to PDF";
            if (!PrinterSettings.InstalledPrinters.Cast<string>().Any(name =>
                    string.Equals(name, printerName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Máy in PDF chưa được cài đặt.");

            chiSoBang = 0;
            chiSoDong = 0;
            soTrang = 1;
            daVeTieuDe = false;
            veLaiTieuDeBang = true;
            using PrintDocument document = new PrintDocument();
            document.DocumentName = "Báo cáo thống kê thu chi";
            document.PrinterSettings.PrinterName = printerName;
            document.PrinterSettings.PrintToFile = true;
            document.PrinterSettings.PrintFileName = Path.GetFullPath(duongDan);
            document.DefaultPageSettings.Landscape = true;
            document.DefaultPageSettings.Margins = new Margins(42, 42, 36, 36);
            PaperSize? a4 = document.PrinterSettings.PaperSizes.Cast<PaperSize>()
                .FirstOrDefault(size => size.Kind == PaperKind.A4);
            if (a4 != null) document.DefaultPageSettings.PaperSize = a4;
            document.PrintController = new StandardPrintController();
            document.PrintPage += InTrang;
            document.Print();

            if (!File.Exists(duongDan) || new FileInfo(duongDan).Length < 5)
                throw new IOException("Không tạo được file PDF.");
            using FileStream stream = File.OpenRead(duongDan);
            byte[] signature = new byte[5];
            if (stream.Read(signature, 0, signature.Length) != signature.Length ||
                Encoding.ASCII.GetString(signature) != "%PDF-")
                throw new IOException("File xuất ra không phải PDF hợp lệ.");
        }

        private List<BangBaoCao> TaoCacBang()
        {
            List<BangBaoCao> result = new List<BangBaoCao>();
            BangBaoCao giaoDich = new BangBaoCao(
                "CHI TIẾT KHOẢN THU - CHI",
                new[] { "STT", "Ngày", "Loại", "Mã phiếu", "Đối tượng", "Diễn giải", "Số tiền (đ)" },
                new[] { 0.05F, 0.10F, 0.08F, 0.13F, 0.22F, 0.27F, 0.15F },
                new[] { 0, 6 });
            int stt = 1;
            foreach (ThongKeGiaoDichDTO item in thongKe.GiaoDich)
            {
                giaoDich.ThemDong(new[]
                {
                    (stt++).ToString(), item.Ngay.ToString("dd/MM/yyyy"), item.Loai,
                    item.MaGiaoDich, item.DoiTuong, item.DienGiai, Tien(item.SoTien)
                });
            }
            if (giaoDich.Dong.Count == 0)
                giaoDich.ThemDong(new[] { "", "", "", "", "Không có dữ liệu trong kỳ", "", "0 đ" });
            giaoDich.ThemDong(new[] { "", "", "", "", "", "Tổng cộng tiền thu", Tien(thongKe.TongThu) }, true);
            giaoDich.ThemDong(new[] { "", "", "", "", "", "Tổng cộng tiền chi", Tien(thongKe.TongChi) }, true);
            result.Add(giaoDich);

            BangBaoCao nhanVien = new BangBaoCao(
                $"THỐNG KÊ QUÝ - NHÂN VIÊN - NĂM {thongKe.NamThongKe}",
                new[] { "Mã nhân\nviên", "Nhân viên", "Q1\nTiền phạt", "Q1\nTiền nhập", "Q2\nTiền phạt", "Q2\nTiền nhập", "Q3\nTiền phạt", "Q3\nTiền nhập", "Q4\nTiền phạt", "Q4\nTiền nhập", "Tổng tiền\nphạt", "Tổng tiền\nnhập" },
                new[] { 0.07F, 0.14F, 0.075F, 0.075F, 0.075F, 0.075F, 0.075F, 0.075F, 0.075F, 0.075F, 0.095F, 0.095F },
                new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 40, 7.2F, 7.2F, 24);
            foreach (ThongKeNhanVienQuyDTO item in thongKe.ThongKeNhanVienTheoQuy)
                nhanVien.ThemDong(new[]
                {
                    item.MaNV, item.TenNhanVien, So(item.TienPhatQ1), So(item.TienNhapQ1),
                    So(item.TienPhatQ2), So(item.TienNhapQ2), So(item.TienPhatQ3), So(item.TienNhapQ3),
                    So(item.TienPhatQ4), So(item.TienNhapQ4), So(item.TongTienPhat), So(item.TongTienNhap)
                });
            if (nhanVien.Dong.Count == 0)
                nhanVien.ThemDong(new[] { "", "Không có dữ liệu", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" });
            result.Add(nhanVien);

            BangBaoCao docGia = new BangBaoCao(
                $"THỐNG KÊ QUÝ - ĐỘC GIẢ - NĂM {thongKe.NamThongKe}",
                new[] { "Mã độc giả", "Độc giả", "Q1\nTiền phạt", "Q2\nTiền phạt", "Q3\nTiền phạt", "Q4\nTiền phạt", "Tổng cộng" },
                new[] { 0.10F, 0.25F, 0.13F, 0.13F, 0.13F, 0.13F, 0.13F },
                new[] { 2, 3, 4, 5, 6 }, 36, 7.8F, 8F, 27);
            foreach (ThongKeDocGiaQuyDTO item in thongKe.ThongKeDocGiaTheoQuy)
                docGia.ThemDong(new[]
                {
                    item.MaDocGia, item.TenDocGia, So(item.TienPhatQ1), So(item.TienPhatQ2),
                    So(item.TienPhatQ3), So(item.TienPhatQ4), So(item.TongCong)
                });
            if (docGia.Dong.Count == 0)
                docGia.ThemDong(new[] { "", "Không có dữ liệu", "0", "0", "0", "0", "0" });
            result.Add(docGia);
            return result;
        }

        private void InTrang(object? sender, PrintPageEventArgs e)
        {
            Graphics? pageGraphics = e.Graphics;
            if (pageGraphics == null)
            {
                e.Cancel = true;
                return;
            }

            Graphics graphics = pageGraphics;
            Rectangle bounds = e.MarginBounds;
            float y = bounds.Top;
            using Font titleFont = new Font("Segoe UI", 18F, FontStyle.Bold);
            using Font sectionFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            using Font bodyFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            using Font headerFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using Pen borderPen = new Pen(Color.FromArgb(125, 135, 145), 0.8F);
            using Brush textBrush = new SolidBrush(Color.Black);
            using Brush headingBrush = new SolidBrush(Color.FromArgb(31, 55, 82));
            float contentBottom = bounds.Bottom - 98;

            if (!daVeTieuDe)
            {
                VeGiua(graphics, "THƯ VIỆN", sectionFont, headingBrush, bounds, y, 20);
                y += 23;
                VeGiua(graphics, "BÁO CÁO THỐNG KÊ THU - CHI", titleFont, headingBrush, bounds, y, 32);
                y += 38;
                VeChu(graphics, $"Kỳ báo cáo: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}", bodyFont, textBrush, bounds.Left, y);
                VeChu(graphics, $"Ngày giờ xuất: {DateTime.Now:dd/MM/yyyy HH:mm}", bodyFont, textBrush, bounds.Left + bounds.Width * 0.48F, y);
                y += 20;
                VeChu(graphics, $"Năm thống kê quý: {thongKe.NamThongKe}", bodyFont, textBrush, bounds.Left, y);
                y += 20;
                VeChu(graphics, "Người lập: ____________________________________", bodyFont, textBrush, bounds.Left, y);
                y += 23;
                VeChu(graphics, $"Tổng thu: {Tien(thongKe.TongThu)}", sectionFont, textBrush, bounds.Left, y);
                VeChu(graphics, $"Tổng chi: {Tien(thongKe.TongChi)}", sectionFont, textBrush, bounds.Left + bounds.Width * 0.34F, y);
                VeChu(graphics, $"Chênh lệch: {Tien(thongKe.ChenhLech)}", sectionFont, textBrush, bounds.Left + bounds.Width * 0.68F, y);
                y += 32;
                daVeTieuDe = true;
            }
            else
            {
                VeGiua(graphics, $"BÁO CÁO THỐNG KÊ THU - CHI  |  {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}", sectionFont, headingBrush, bounds, y, 24);
                y += 31;
            }

            while (chiSoBang < cacBang.Count)
            {
                BangBaoCao bang = cacBang[chiSoBang];
                using Font tableBodyFont = new Font("Segoe UI", bang.CzNoiDung, FontStyle.Regular);
                if (veLaiTieuDeBang)
                {
                    if (y + 20 + bang.ChieuCaoTieuDe + bang.ChieuCaoDong > contentBottom) break;
                    VeChu(graphics, bang.Ten, sectionFont, headingBrush, bounds.Left, y);
                    y += 20;
                    using Font tableHeaderFont = new Font("Segoe UI", bang.CzTieuDe, FontStyle.Bold);
                    VeDongBang(graphics, bounds.Left, y, bounds.Width, bang.ChieuCaoTieuDe, bang.Cot, bang.DoRong,
                        tableHeaderFont, textBrush, borderPen, null, true);
                    y += bang.ChieuCaoTieuDe;
                    veLaiTieuDeBang = false;
                }

                while (chiSoDong < bang.Dong.Count)
                {
                    if (y + bang.ChieuCaoDong > contentBottom)
                    {
                        veLaiTieuDeBang = true;
                        goto KetThucTrang;
                    }
                    VeDongBang(graphics, bounds.Left, y, bounds.Width, bang.ChieuCaoDong, bang.Dong[chiSoDong],
                        bang.DoRong, tableBodyFont, textBrush, borderPen, bang.CotCanPhai, bang.DongTongCong[chiSoDong]);
                    y += bang.ChieuCaoDong;
                    chiSoDong++;
                }

                chiSoBang++;
                chiSoDong = 0;
                veLaiTieuDeBang = true;
            }

        KetThucTrang:
            bool conTrang = chiSoBang < cacBang.Count;
            if (!conTrang) VeChuKy(graphics, bounds, sectionFont, borderPen, textBrush);
            VeChu(graphics, $"Trang {soTrang}", bodyFont, textBrush, bounds.Right - 60, bounds.Bottom - 18);
            e.HasMorePages = conTrang;
            soTrang++;
        }

        private static void VeChuKy(Graphics graphics, Rectangle bounds, Font font, Pen pen, Brush brush)
        {
            float y = bounds.Bottom - 75;
            float middle = bounds.Left + bounds.Width / 2F;
            VeGiua(graphics, "Người lập", font, brush, new Rectangle(bounds.Left, (int)y, (int)(bounds.Width / 2F), 20), y, 20);
            VeGiua(graphics, "Người duyệt", font, brush, new Rectangle((int)middle, (int)y, (int)(bounds.Width / 2F), 20), y, 20);
            graphics.DrawLine(pen, bounds.Left + 35, y + 43, middle - 35, y + 43);
            graphics.DrawLine(pen, middle + 35, y + 43, bounds.Right - 35, y + 43);
        }

        private static void VeDongBang(Graphics graphics, float x, float y, float totalWidth, float height,
            string[] values, float[] widthParts, Font font, Brush brush, Pen pen, int[]? canPhai, bool dongTongCong)
        {
            float cursor = x;
            using Brush totalBrush = new SolidBrush(dongTongCong ? Color.FromArgb(232, 239, 247) : Color.White);
            for (int i = 0; i < values.Length; i++)
            {
                float width = i == values.Length - 1 ? x + totalWidth - cursor : totalWidth * widthParts[i];
                RectangleF cell = new RectangleF(cursor, y, width, height);
                graphics.FillRectangle(totalBrush, cell);
                graphics.DrawRectangle(pen, cell.X, cell.Y, cell.Width, cell.Height);
                using StringFormat format = new StringFormat
                {
                    Alignment = canPhai != null && canPhai.Contains(i) ? StringAlignment.Far : StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisWord
                };
                using Font rowFont = dongTongCong ? new Font(font, FontStyle.Bold) : new Font(font, font.Style);
                graphics.DrawString(values[i], rowFont, brush, RectangleF.Inflate(cell, -5, -2), format);
                cursor += width;
            }
        }

        private static void VeChu(Graphics graphics, string text, Font font, Brush brush, float x, float y)
        {
            graphics.DrawString(text, font, brush, x, y);
        }

        private static void VeGiua(Graphics graphics, string text, Font font, Brush brush, Rectangle bounds, float y, float height)
        {
            using StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString(text, font, brush, new RectangleF(bounds.Left, y, bounds.Width, height), format);
        }

        private static string So(decimal amount) => amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        private static string Tien(decimal amount) => amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " đ";

        private sealed class BangBaoCao
        {
            public BangBaoCao(string ten, string[] cot, float[] doRong, int[] cotCanPhai,
                float chieuCaoTieuDe = 28, float czTieuDe = 8.5F, float czNoiDung = 8.5F, float chieuCaoDong = 27)
            {
                Ten = ten;
                Cot = cot;
                DoRong = doRong;
                CotCanPhai = cotCanPhai;
                ChieuCaoTieuDe = chieuCaoTieuDe;
                CzTieuDe = czTieuDe;
                CzNoiDung = czNoiDung;
                ChieuCaoDong = chieuCaoDong;
            }

            public string Ten { get; }
            public string[] Cot { get; }
            public float[] DoRong { get; }
            public int[] CotCanPhai { get; }
            public float ChieuCaoTieuDe { get; }
            public float CzTieuDe { get; }
            public float CzNoiDung { get; }
            public List<string[]> Dong { get; } = new List<string[]>();
            public List<bool> DongTongCong { get; } = new List<bool>();
            public float ChieuCaoDong { get; }

            public void ThemDong(string[] values, bool dongTongCong = false)
            {
                Dong.Add(values);
                DongTongCong.Add(dongTongCong);
            }
        }
    }
}
