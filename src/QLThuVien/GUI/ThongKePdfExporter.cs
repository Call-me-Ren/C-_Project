using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    public sealed class ThongKePdfExporter
    {
        private readonly ThongKeTongHopDTO thongKe;
        private readonly DateTime tuNgay;
        private readonly DateTime denNgay;
        private float fontSize;
        private float tableFontSize;

        public ThongKePdfExporter(ThongKeTongHopDTO thongKe, DateTime tuNgay, DateTime denNgay)
        {
            this.thongKe = thongKe ?? throw new ArgumentNullException(nameof(thongKe));
            this.tuNgay = tuNgay.Date;
            this.denNgay = denNgay.Date;
        }

        public void Xuat(string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan))
                throw new ArgumentException("Đường dẫn file không hợp lệ.", nameof(duongDan));

            string fullPath = Path.GetFullPath(duongDan);
            QuestPDF.Settings.License = LicenseType.Community;
            byte[] pdf = TaoPdf(10, false);
            if (DemSoTrang(pdf) > 1)
            {
                for (decimal size = 9.5m; size >= 8m; size -= 0.5m)
                {
                    byte[] thu = TaoPdf((float)size, false);
                    if (DemSoTrang(thu) == 1)
                    {
                        pdf = thu;
                        break;
                    }
                }
            }

            if (DemSoTrang(pdf) > 1)
                pdf = TaoPdf(10, true);

            File.WriteAllBytes(fullPath, pdf);
        }

        private byte[] TaoPdf(float fontSize, bool gomCuoi)
        {
            this.fontSize = fontSize;
            tableFontSize = Math.Max(8f, fontSize * 0.9f);
            using MemoryStream stream = new MemoryStream();
            Document.Create(document => document.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(16);
                page.DefaultTextStyle(style => style.FontFamily("Lato").FontSize(fontSize));
                page.Header().Column(header =>
                {
                    header.Item().SkipOnce().BorderBottom(1).BorderColor(Colors.Blue.Medium).Row(row =>
                    {
                        row.RelativeItem().Text($"BÁO CÁO THỐNG KÊ THU - CHI | {tuNgay:yyyy-MM-dd} - {denNgay:yyyy-MM-dd}")
                            .Bold().FontSize(fontSize);
                        row.ConstantItem(70).AlignRight().DefaultTextStyle(style => style.Bold().FontSize(fontSize)).Text(text =>
                        {
                            text.Span("Trang ");
                            text.CurrentPageNumber();
                            text.Span("/");
                            text.TotalPages();
                        });
                    });
                });
                page.Content().PaddingTop(4).Column(column =>
                {
                    column.Item().PaddingBottom(3).AlignCenter().Text("BÁO CÁO THỐNG KÊ THU - CHI")
                        .Bold().FontSize(fontSize * 1.7f).FontColor(Colors.Blue.Darken3);
                    column.Item().Element(VungTongHop);
                    column.Item().PaddingTop(4).Text("CHI TIẾT KHOẢN THU - CHI").Bold().FontSize(fontSize * 1.1f);
                    column.Item().PaddingTop(2).Element(BangGiaoDich);
                    column.Item().PaddingTop(5).Text($"THỐNG KÊ QUÝ - NHÂN VIÊN - NĂM {thongKe.NamThongKe}").Bold().FontSize(fontSize * 1.1f);
                    column.Item().PaddingTop(2).Element(BangNhanVien);
                    column.Item().PaddingTop(5).Text($"THỐNG KÊ QUÝ - ĐỘC GIẢ - NĂM {thongKe.NamThongKe}").Bold().FontSize(fontSize * 1.1f);
                    column.Item().PaddingTop(2).Element(BangDocGia);
                    column.Item().PaddingTop(5).Text($"THỐNG KÊ QUÝ - ĐẦU SÁCH - NĂM {thongKe.NamThongKe}").Bold().FontSize(fontSize * 1.1f);
                    if (!gomCuoi)
                    {
                        column.Item().PaddingTop(2).Element(container => BangDauSach(container, 0, thongKe.ThongKeDauSachTheoQuy.Count, true));
                        column.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().AlignCenter().Column(signature =>
                            {
                                signature.Item().Text("Người lập").Bold();
                                signature.Item().PaddingTop(20).Text("........................................");
                            });
                            row.RelativeItem().AlignCenter().Column(signature =>
                            {
                                signature.Item().Text("Người duyệt").Bold();
                                signature.Item().PaddingTop(20).Text("........................................");
                            });
                        });
                    }
                    else
                    {
                        int splitAt = Math.Max(0, thongKe.ThongKeDauSachTheoQuy.Count - 7);
                        if (splitAt > 0)
                            column.Item().PaddingTop(2).Element(container => BangDauSach(container, 0, splitAt, false));
                        if (splitAt > 0)
                            column.Item().PageBreak();
                        column.Item().ShowEntire().Column(last =>
                        {
                            last.Item().PaddingTop(2).Element(container => BangDauSach(container, splitAt, thongKe.ThongKeDauSachTheoQuy.Count, true));
                            last.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Column(signature =>
                                {
                                    signature.Item().Text("Người lập").Bold();
                                    signature.Item().PaddingTop(20).Text("........................................");
                                });
                                row.RelativeItem().AlignCenter().Column(signature =>
                                {
                                    signature.Item().Text("Người duyệt").Bold();
                                    signature.Item().PaddingTop(20).Text("........................................");
                                });
                            });
                        });
                    }
                });
            })).GeneratePdf(stream);
            return stream.ToArray();
        }

        private static int DemSoTrang(byte[] pdf)
        {
            string source = Encoding.Latin1.GetString(pdf);
            return Regex.Matches(source, @"/Type\s*/Page\b").Count;
        }

        private void VungTongHop(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Kỳ báo cáo: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}");
                    row.RelativeItem().AlignRight().Text($"Năm thống kê quý: {thongKe.NamThongKe}");
                });
                column.Item().PaddingTop(3).Row(row =>
                {
                    Tong(row.RelativeItem(), "Tổng thu", thongKe.TongThu);
                    Tong(row.RelativeItem(), "Tổng chi", thongKe.TongChi);
                    Tong(row.RelativeItem(), "Chênh lệch", thongKe.ChenhLech);
                });
            });
        }

        private void BangGiaoDich(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.45f);
                    columns.RelativeColumn(0.8f);
                    columns.RelativeColumn(0.65f);
                    columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1.6f);
                    columns.RelativeColumn(1.8f);
                    columns.RelativeColumn(1f);
                });
                table.Header(header =>
                {
                    Header(header.Cell(), "STT");
                    Header(header.Cell(), "Ngày");
                    Header(header.Cell(), "Loại");
                    Header(header.Cell(), "Mã phiếu");
                    Header(header.Cell(), "Đối tượng");
                    Header(header.Cell(), "Diễn giải");
                    Header(header.Cell(), "Số tiền (đ)");
                });
                if (thongKe.GiaoDich.Count == 0)
                {
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "Không có dữ liệu trong kỳ", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "0 đ", 0, true);
                }
                else
                {
                    for (int index = 0; index < thongKe.GiaoDich.Count; index++)
                    {
                        ThongKeGiaoDichDTO item = thongKe.GiaoDich[index];
                        int row = index + 1;
                        Cell(table.Cell(), row.ToString(CultureInfo.InvariantCulture), row, true);
                        Cell(table.Cell(), item.Ngay.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN")), row);
                        Cell(table.Cell(), item.Loai, row);
                        Cell(table.Cell(), item.MaGiaoDich, row);
                        Cell(table.Cell(), item.DoiTuong, row);
                        Cell(table.Cell(), item.DienGiai, row);
                        Cell(table.Cell(), Tien(item.SoTien), row, true);
                    }
                }
                TotalRow(table, "Tổng cộng tiền thu", Tien(thongKe.TongThu), 7);
                TotalRow(table, "Tổng cộng tiền chi", Tien(thongKe.TongChi), 7);
            });
        }

        private void BangNhanVien(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.8f);
                    columns.RelativeColumn(1.5f);
                    for (int i = 0; i < 8; i++) columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1f);
                    columns.RelativeColumn(1f);
                });
                table.Header(header =>
                {
                    Header(header.Cell(), "Mã nhân viên");
                    Header(header.Cell(), "Nhân viên");
                    Header(header.Cell(), "Q1\nTiền phạt");
                    Header(header.Cell(), "Q1\nTiền nhập");
                    Header(header.Cell(), "Q2\nTiền phạt");
                    Header(header.Cell(), "Q2\nTiền nhập");
                    Header(header.Cell(), "Q3\nTiền phạt");
                    Header(header.Cell(), "Q3\nTiền nhập");
                    Header(header.Cell(), "Q4\nTiền phạt");
                    Header(header.Cell(), "Q4\nTiền nhập");
                    Header(header.Cell(), "Tổng tiền phạt");
                    Header(header.Cell(), "Tổng tiền nhập");
                });
                if (thongKe.ThongKeNhanVienTheoQuy.Count == 0)
                    EmptyRow(table, 12, "Không có dữ liệu");
                for (int index = 0; index < thongKe.ThongKeNhanVienTheoQuy.Count; index++)
                {
                    ThongKeNhanVienQuyDTO item = thongKe.ThongKeNhanVienTheoQuy[index];
                    int row = index + 1;
                    Cell(table.Cell(), item.MaNV, row);
                    Cell(table.Cell(), item.TenNhanVien, row);
                    Cell(table.Cell(), So(item.TienPhatQ1), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ1), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ2), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ2), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ3), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ3), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ4), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ4), row, true);
                    Cell(table.Cell(), So(item.TongTienPhat), row, true);
                    Cell(table.Cell(), So(item.TongTienNhap), row, true);
                }
            });
        }

        private void BangDocGia(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1.8f);
                    for (int i = 0; i < 5; i++) columns.RelativeColumn(1f);
                });
                table.Header(header =>
                {
                    Header(header.Cell(), "Mã độc giả");
                    Header(header.Cell(), "Độc giả");
                    Header(header.Cell(), "Q1\nTiền phạt");
                    Header(header.Cell(), "Q2\nTiền phạt");
                    Header(header.Cell(), "Q3\nTiền phạt");
                    Header(header.Cell(), "Q4\nTiền phạt");
                    Header(header.Cell(), "Tổng cộng");
                });
                if (thongKe.ThongKeDocGiaTheoQuy.Count == 0)
                    EmptyRow(table, 7, "Không có dữ liệu");
                for (int index = 0; index < thongKe.ThongKeDocGiaTheoQuy.Count; index++)
                {
                    ThongKeDocGiaQuyDTO item = thongKe.ThongKeDocGiaTheoQuy[index];
                    int row = index + 1;
                    Cell(table.Cell(), item.MaDocGia, row);
                    Cell(table.Cell(), item.TenDocGia, row);
                    Cell(table.Cell(), So(item.TienPhatQ1), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ2), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ3), row, true);
                    Cell(table.Cell(), So(item.TienPhatQ4), row, true);
                    Cell(table.Cell(), So(item.TongCong), row, true);
                }
            });
        }

        private void BangDauSach(IContainer container, int batDau, int ketThuc, bool includeTotal)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.85f);
                    columns.RelativeColumn(1.7f);
                    for (int i = 0; i < 8; i++) columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1f);
                    columns.RelativeColumn(1f);
                });
                table.Header(header =>
                {
                    Header(header.Cell(), "Mã đầu sách");
                    Header(header.Cell(), "Đầu sách");
                    Header(header.Cell(), "Q1\nTiền nhập");
                    Header(header.Cell(), "Q1\nLượt mượn");
                    Header(header.Cell(), "Q2\nTiền nhập");
                    Header(header.Cell(), "Q2\nLượt mượn");
                    Header(header.Cell(), "Q3\nTiền nhập");
                    Header(header.Cell(), "Q3\nLượt mượn");
                    Header(header.Cell(), "Q4\nTiền nhập");
                    Header(header.Cell(), "Q4\nLượt mượn");
                    Header(header.Cell(), "Tổng tiền nhập");
                    Header(header.Cell(), "Tổng lượt mượn");
                });
                if (thongKe.ThongKeDauSachTheoQuy.Count == 0 && batDau == 0)
                    EmptyRow(table, 12, "Không có dữ liệu");
                for (int index = batDau; index < ketThuc; index++)
                {
                    ThongKeDauSachQuyDTO item = thongKe.ThongKeDauSachTheoQuy[index];
                    int row = index + 1;
                    Cell(table.Cell(), item.MaDauSach, row);
                    Cell(table.Cell(), item.TenDauSach, row);
                    Cell(table.Cell(), So(item.TienNhapQ1), row, true);
                    Cell(table.Cell(), So(item.LuotMuonQ1), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ2), row, true);
                    Cell(table.Cell(), So(item.LuotMuonQ2), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ3), row, true);
                    Cell(table.Cell(), So(item.LuotMuonQ3), row, true);
                    Cell(table.Cell(), So(item.TienNhapQ4), row, true);
                    Cell(table.Cell(), So(item.LuotMuonQ4), row, true);
                    Cell(table.Cell(), So(item.TongTienNhap), row, true);
                    Cell(table.Cell(), So(item.TongLuotMuon), row, true);
                }
                if (includeTotal)
                {
                    TotalCell(table.Cell(), "Tổng cộng");
                    TotalCell(table.Cell(), string.Empty);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TienNhapQ1)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.LuotMuonQ1)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TienNhapQ2)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.LuotMuonQ2)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TienNhapQ3)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.LuotMuonQ3)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TienNhapQ4)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.LuotMuonQ4)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TongTienNhap)), true);
                    TotalCell(table.Cell(), So(thongKe.ThongKeDauSachTheoQuy.Sum(item => item.TongLuotMuon)), true);
                }
            });
        }

        private void TotalCell(IContainer cell, string text, bool alignRight = false) =>
            cell.ShowEntire().Background(Colors.Grey.Lighten3).BorderBottom(0.5f).Padding(2)
                .AlignMiddle().DefaultTextStyle(style => style.Bold().FontSize(tableFontSize))
                .Element(inner => alignRight ? inner.AlignRight() : inner).Text(text);

        private void Header(IContainer cell, string text) =>
            cell.Background(Colors.Blue.Darken2).Border(0.5f).BorderColor(Colors.Grey.Darken1)
                .Padding(2).AlignCenter().DefaultTextStyle(style => style.FontColor(Colors.White).Bold().FontSize(tableFontSize))
                .Text(text);

        private void Cell(IContainer cell, string text, int row, bool alignRight = false) =>
            cell.ShowEntire().Background(row % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White)
                .BorderBottom(0.4f).BorderColor(Colors.Grey.Lighten2).Padding(2)
                .AlignMiddle().DefaultTextStyle(style => style.FontSize(tableFontSize))
                .Element(inner => alignRight ? inner.AlignRight() : inner).Text(text);

        private void EmptyRow(TableDescriptor table, int columnCount, string message)
        {
            for (int i = 0; i < columnCount; i++)
                Cell(table.Cell(), i == 1 ? message : "0", 0, i > 1);
        }

        private void TotalRow(TableDescriptor table, string label, string amount, int columnCount)
        {
            table.Cell().ColumnSpan((uint)(columnCount - 1)).Element(cell =>
                cell.Background(Colors.Grey.Lighten3).BorderBottom(0.5f).Padding(4).AlignRight()
                    .DefaultTextStyle(style => style.Bold())).Text(label);
            table.Cell().Element(cell =>
                cell.Background(Colors.Grey.Lighten3).BorderBottom(0.5f).Padding(4).AlignRight()
                    .DefaultTextStyle(style => style.Bold())).Text(amount);
        }

        private static void Tong(IContainer container, string label, decimal amount) =>
            container.Text($"{label}: {Tien(amount)}").Bold().FontSize(10);

        private static string So(decimal amount) =>
            amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

        private static string So(int amount) =>
            amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

        private static string Tien(decimal amount) => So(amount) + " đ";
    }
}

