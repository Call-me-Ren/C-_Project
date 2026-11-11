using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Service xuất PDF chuyên nghiệp cho hệ thống Thư viện sử dụng QuestPDF.
    /// Hỗ trợ tiếng Việt Unicode có dấu, layout chuẩn A4, thiết kế trang nhã.
    /// </summary>
    public static class PdfService
    {
        static PdfService()
        {
            // Kích hoạt giấy phép Community (phi thương mại / học tập đồ án)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static void XuatPhieuMuonPdf(PhieuMuonDTO pm, List<ChiTietPhieuMuonDTO> danhSachSach, string duongDanFile)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Segoe UI"));

                    // 1. HEADER (TIÊU ĐỀ THƯ VIỆN VÀ PHIẾU MƯỢN)
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("TRƯỜNG ĐẠI HỌC SÀI GÒN").Bold().FontSize(13).FontColor(Colors.Blue.Darken2);
                                c.Item().Text("THƯ VIỆN TRUNG TÂM").FontSize(10).FontColor(Colors.Grey.Darken1);
                            });

                            row.ConstantItem(180).AlignRight().Column(c =>
                            {
                                c.Item().Text($"Mã phiếu: {pm.MaPM}").Bold().FontSize(12);
                                c.Item().Text($"Ngày in: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9).Italic();
                            });
                        });

                        col.Item().PaddingTop(15).PaddingBottom(10).AlignCenter().Text("PHIẾU MƯỢN SÁCH")
                            .Bold().FontSize(20).FontColor(Colors.Blue.Darken3);

                        // Đường kẻ trang trí
                        col.Item().LineHorizontal(1.5f).LineColor(Colors.Blue.Medium);
                    });

                    // 2. NỘI DUNG (THÔNG TIN PHIẾU + BẢNG SÁCH)
                    page.Content().PaddingTop(15).Column(col =>
                    {
                        // Khung thông tin độc giả & nhân viên
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.Grey.Lighten4).Padding(12).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Họ tên độc giả: ").Bold();
                                    t.Span(pm.TenDocGia ?? pm.MaDocGia);
                                });
                                c.Item().PaddingTop(4).Text(t =>
                                {
                                    t.Span("Mã độc giả: ").Bold();
                                    t.Span(pm.MaDocGia);
                                });
                                c.Item().PaddingTop(4).Text(t =>
                                {
                                    t.Span("Nhân viên lập: ").Bold();
                                    t.Span(pm.TenNV ?? pm.MaNV);
                                });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Ngày mượn: ").Bold();
                                    t.Span($"{pm.NgayMuon:dd/MM/yyyy}");
                                });
                                c.Item().PaddingTop(4).Text(t =>
                                {
                                    t.Span("Hạn trả: ").Bold();
                                    t.Span($"{pm.HanTra:dd/MM/yyyy}").FontColor(Colors.Red.Darken2).Bold();
                                });
                                c.Item().PaddingTop(4).Text(t =>
                                {
                                    t.Span("Tổng số cuốn: ").Bold();
                                    t.Span($"{danhSachSach.Count} cuốn");
                                });
                            });
                        });

                        col.Item().PaddingTop(20).Text("DANH SÁCH CUỐN SÁCH MƯỢN:").Bold().FontSize(12);

                        // Bảng danh sách sách
                        col.Item().PaddingTop(8).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // STT
                                columns.ConstantColumn(90);  // Mã cuốn
                                columns.RelativeColumn(3);   // Tên sách
                                columns.RelativeColumn(1.5f);// Tình trạng
                                columns.ConstantColumn(100); // Trạng thái trả
                            });

                            // Header của bảng
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6).AlignCenter().Text("STT").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6).Text("Mã cuốn").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6).Text("Tên sách").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6).Text("Tình trạng").Bold().FontColor(Colors.White);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6).AlignCenter().Text("Trạng thái").Bold().FontColor(Colors.White);
                            });

                            // Dòng dữ liệu
                            for (int i = 0; i < danhSachSach.Count; i++)
                            {
                                var item = danhSachSach[i];
                                var bgColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                                table.Cell().Background(bgColor).Padding(6).AlignCenter().Text((i + 1).ToString());
                                table.Cell().Background(bgColor).Padding(6).Text(item.MaSach);
                                table.Cell().Background(bgColor).Padding(6).Text(item.TenSach ?? "Chưa rõ tên");
                                table.Cell().Background(bgColor).Padding(6).Text(item.TinhTrang ?? "Tốt");
                                table.Cell().Background(bgColor).Padding(6).AlignCenter().Text(item.NgayThucTra.HasValue ? $"Đã trả ({item.NgayThucTra.Value:dd/MM})" : "Đang mượn")
                                    .FontColor(item.NgayThucTra.HasValue ? Colors.Green.Darken2 : Colors.Orange.Darken2);
                            }
                        });

                        // Lời dặn dò độc giả
                        col.Item().PaddingTop(25).Border(1).BorderColor(Colors.Orange.Lighten1).Background(Colors.Orange.Lighten5).Padding(10).Column(c =>
                        {
                            c.Item().Text("📌 LƯU Ý DÀNH CHO BẠN ĐỌC:").Bold().FontColor(Colors.Orange.Darken3);
                            c.Item().PaddingTop(2).Text("- Vui lòng bảo quản sách cẩn thận, không làm rách, gạch xóa hoặc mất sách.");
                            c.Item().PaddingTop(2).Text($"- Trả sách đúng hạn trước ngày {pm.HanTra:dd/MM/yyyy} để tránh bị tính phí phạt trễ hạn theo quy định của Thư viện.");
                        });

                        // Phần chữ ký
                        col.Item().PaddingTop(30).Row(row =>
                        {
                            row.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Text("ĐỘC GIẢ MƯỢN SÁCH").Bold();
                                c.Item().Text("(Ký và ghi rõ họ tên)").Italic().FontSize(9);
                                c.Item().PaddingTop(50).Text(pm.TenDocGia ?? "..................................");
                            });

                            row.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Text("THỦ THƯ LẬP PHIẾU").Bold();
                                c.Item().Text("(Ký và ghi rõ họ tên)").Italic().FontSize(9);
                                c.Item().PaddingTop(50).Text(pm.TenNV ?? "..................................");
                            });
                        });
                    });

                    // 3. FOOTER
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Trang ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(duongDanFile);
        }
    }
}
