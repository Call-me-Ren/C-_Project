using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public sealed class PhieuPhatPdfExporter
    {
        private readonly PhieuPhatBaoCaoDTO phieu;
        private int chiTietIndex;
        private bool daVeBang;

        public PhieuPhatPdfExporter(PhieuPhatBaoCaoDTO phieu)
        {
            this.phieu = phieu ?? throw new ArgumentNullException(nameof(phieu));
        }

        public void Xuat(string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan)) throw new ArgumentException("Đường dẫn file không hợp lệ.", nameof(duongDan));
            const string printerName = "Microsoft Print to PDF";
            if (!PrinterSettings.InstalledPrinters.Cast<string>().Any(name =>
                    string.Equals(name, printerName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Máy in PDF chưa được cài đặt.");

            chiTietIndex = 0;
            daVeBang = false;
            using PrintDocument document = new PrintDocument();
            document.DocumentName = $"Phiếu phạt {phieu.MaPhieuPhat}";
            document.PrinterSettings.PrinterName = printerName;
            document.PrinterSettings.PrintToFile = true;
            document.PrinterSettings.PrintFileName = Path.GetFullPath(duongDan);
            document.DefaultPageSettings.Landscape = true;
            document.DefaultPageSettings.Margins = new Margins(45, 45, 40, 40);
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
            using Font titleFont = new Font("Arial", 24F, FontStyle.Bold);
            using Font sectionFont = new Font("Arial", 11F, FontStyle.Bold);
            using Font bodyFont = new Font("Arial", 10F, FontStyle.Regular);
            using Pen borderPen = new Pen(Color.FromArgb(100, 100, 100), 1F);
            using Brush textBrush = new SolidBrush(Color.Black);

            if (!daVeBang)
            {
                DrawCentered(graphics, "PHIẾU PHẠT", titleFont, textBrush, bounds, y, 38);
                y += 55;
                DrawText(graphics, $"Mã phiếu: {phieu.MaPhieuPhat}", sectionFont, textBrush, bounds.Left, y);
                DrawText(graphics, $"Ngày lập: {phieu.NgayLap:dd/MM/yyyy}", bodyFont, textBrush, bounds.Left + 360, y);
                DrawText(graphics, $"Trạng thái: {phieu.TrangThai}", bodyFont, textBrush, bounds.Left + 700, y);
                y += 28;
                DrawText(graphics, $"Độc giả: {phieu.MaDocGia} - {phieu.TenDocGia}", bodyFont, textBrush, bounds.Left, y);
                y += 24;
                DrawText(graphics, $"Nhân viên lập: {phieu.MaNV} - {phieu.TenNhanVien}", bodyFont, textBrush, bounds.Left, y);
                y += 24;
                DrawText(graphics, $"Phiếu mượn: {phieu.MaPhieuMuon ?? "Không có"}", bodyFont, textBrush, bounds.Left, y);
                y += 34;
                daVeBang = true;
            }

            string[] headers = { "Cuốn sách", "Quy định", "Ngày trễ", "Đơn giá", "Tiền phạt", "Ghi chú" };
            float[] widths = { 0.22F, 0.22F, 0.09F, 0.15F, 0.15F, 0.17F };
            VeDongBang(graphics, bounds.Left, y, bounds.Width, 42, headers, widths, sectionFont, textBrush, borderPen, true);
            y += 42;

            if (phieu.ChiTiet.Count == 0 && chiTietIndex == 0)
            {
                string[] empty = { "", "", "", "", "", "Chưa có vi phạm." };
                VeDongBang(graphics, bounds.Left, y, bounds.Width, 42, empty, widths, bodyFont, textBrush, borderPen, true);
                y += 42;
                chiTietIndex = 1;
            }
            else
            {
                while (chiTietIndex < phieu.ChiTiet.Count && y + 48 < bounds.Bottom - 155)
                {
                    ChiTietPhieuPhatBaoCaoDTO item = phieu.ChiTiet[chiTietIndex];
                    string[] values =
                    {
                        $"{item.MaSach}\n{item.TenSach}",
                        $"{item.MaQD}\n{item.TenQD}",
                        item.SoNgayTre?.ToString() ?? "—",
                        Tien(item.DonGia),
                        Tien(item.TienPhat),
                        item.GhiChu ?? string.Empty
                    };
                    VeDongBang(graphics, bounds.Left, y, bounds.Width, 48, values, widths, bodyFont, textBrush, borderPen, true);
                    y += 48;
                    chiTietIndex++;
                }
            }

            if (chiTietIndex < phieu.ChiTiet.Count)
            {
                e.HasMorePages = true;
                return;
            }
            if (y + 150 > bounds.Bottom)
            {
                e.HasMorePages = true;
                return;
            }

            y += 26;
            DrawText(graphics, $"Tổng tiền phạt: {Tien(phieu.TongTienPhat)}", sectionFont, textBrush, bounds.Left, y);
            y += 66;
            DrawCentered(graphics, "Người lập phiếu", sectionFont, textBrush,
                new Rectangle(bounds.Left, (int)y, bounds.Width / 2, 30), y, 22);
            DrawCentered(graphics, "Người nộp", sectionFont, textBrush,
                new Rectangle(bounds.Left + bounds.Width / 2, (int)y, bounds.Width / 2, 30), y, 22);
            y += 82;
            using Pen signaturePen = new Pen(Color.Gray, 1F);
            graphics.DrawLine(signaturePen, bounds.Left + 30, y, bounds.Left + bounds.Width / 2 - 30, y);
            graphics.DrawLine(signaturePen, bounds.Left + bounds.Width / 2 + 30, y, bounds.Right - 30, y);
            e.HasMorePages = false;
        }

        private static void VeDongBang(Graphics graphics, float x, float y, float totalWidth, float height,
            string[] values, float[] widthParts, Font font, Brush brush, Pen pen, bool drawValues)
        {
            float cursor = x;
            for (int i = 0; i < values.Length; i++)
            {
                float width = i == values.Length - 1 ? x + totalWidth - cursor : totalWidth * widthParts[i];
                RectangleF cell = new RectangleF(cursor, y, width, height);
                graphics.DrawRectangle(pen, cell.X, cell.Y, cell.Width, cell.Height);
                if (drawValues && values[i].Length > 0)
                {
                    using StringFormat format = new StringFormat
                    {
                        Alignment = i == 2 || i == 3 || i == 4 ? StringAlignment.Far : StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisWord
                    };
                    RectangleF textBounds = RectangleF.Inflate(cell, -5, -3);
                    graphics.DrawString(values[i], font, brush, textBounds, format);
                }
                cursor += width;
            }
        }

        private static void DrawText(Graphics graphics, string text, Font font, Brush brush, float x, float y)
        {
            graphics.DrawString(text, font, brush, x, y);
        }

        private static void DrawCentered(Graphics graphics, string text, Font font, Brush brush,
            Rectangle bounds, float y, float height)
        {
            using StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString(text, font, brush, new RectangleF(bounds.Left, y, bounds.Width, height), format);
        }

        private static string Tien(decimal amount) => amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " đ";
    }
}
