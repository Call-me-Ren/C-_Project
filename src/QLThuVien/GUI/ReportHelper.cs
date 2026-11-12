using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QLThuVien.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace QLThuVien.GUI
{
    /// <summary>
    /// Helper tĩnh hỗ trợ xuất PDF cho các nghiệp vụ trong module Nhập kho.
    /// </summary>
    public static class ReportHelper
    {
        static ReportHelper()
        {
            // Khai báo license cộng đồng (miễn phí)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        /// <summary>
        /// Xuất Phiếu Nhập ra file PDF, lưu vào Desktop và tự mở sau khi xuất.
        /// </summary>
        public static void XuatPDFPhieuNhap(PhieuNhapDTO pn, List<ChiTietPhieuNhapDTO> dsChiTiet, string tenNCC = "")
        {
            string fileName = $"PhieuNhap_{pn.MaPN}_{pn.NgayNhap:yyyyMMdd}.pdf";
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // ---- HEADER ----
                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("THƯ VIỆN TRƯỜNG ĐẠI HỌC SÀI GÒN")
                            .Bold().FontSize(13);
                        col.Item().AlignCenter().Text("PHIẾU NHẬP SÁCH")
                            .Bold().FontSize(16).FontColor(Colors.Blue.Medium);
                        col.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    });

                    // ---- CONTENT ----
                    page.Content().PaddingTop(12).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Mã phiếu nhập: {pn.MaPN}").SemiBold();
                            row.RelativeItem().AlignRight()
                                .Text($"Ngày nhập: {pn.NgayNhap:dd/MM/yyyy}").SemiBold();
                        });

                        col.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Text($"Nhà cung cấp: {tenNCC}");
                            row.RelativeItem().AlignRight().Text($"Mã NV phụ trách: {pn.MaNV}");
                        });

                        col.Item().PaddingTop(10).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                        col.Item().PaddingTop(10);

                        // ---- Bảng chi tiết ----
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(30);    // STT
                                cols.RelativeColumn(3);     // Tên sách
                                cols.RelativeColumn(1.5f);  // Đơn giá
                                cols.ConstantColumn(60);    // SL
                                cols.RelativeColumn(1.5f);  // Thành tiền
                            });

                            // Header bảng
                            table.Header(header =>
                            {
                                static IContainer HC(IContainer c) =>
                                    c.Background(Colors.Blue.Lighten3).Padding(5).AlignCenter();

                                header.Cell().Element(HC).Text("STT").Bold();
                                header.Cell().Element(HC).Text("Tên sách").Bold();
                                header.Cell().Element(HC).Text("Đơn giá").Bold();
                                header.Cell().Element(HC).Text("SL").Bold();
                                header.Cell().Element(HC).Text("Thành tiền").Bold();
                            });

                            // Các dòng chi tiết
                            for (int i = 0; i < dsChiTiet.Count; i++)
                            {
                                var ct = dsChiTiet[i];
                                string bg = (i % 2 == 0) ? Colors.White : Colors.Grey.Lighten4;

                                static IContainer DC(IContainer c, string color) =>
                                    c.Background(color).Padding(5);

                                table.Cell().Element(c => DC(c, bg)).AlignCenter()
                                    .Text((i + 1).ToString());
                                table.Cell().Element(c => DC(c, bg))
                                    .Text(ct.TenSach);
                                table.Cell().Element(c => DC(c, bg)).AlignRight()
                                    .Text(ct.DonGia.ToString("N0") + " đ");
                                table.Cell().Element(c => DC(c, bg)).AlignCenter()
                                    .Text(ct.SoLuong.ToString());
                                table.Cell().Element(c => DC(c, bg)).AlignRight()
                                    .Text(ct.ThanhTien.ToString("N0") + " đ");
                            }
                        });

                        // Tổng tiền
                        col.Item().PaddingTop(12).AlignRight()
                            .Text($"TỔNG TIỀN: {pn.TongTien:N0} đ")
                            .Bold().FontSize(13).FontColor(Colors.Red.Medium);

                        col.Item().PaddingTop(20).Row(row =>
                        {
                            row.RelativeItem();
                            row.ConstantItem(200).AlignCenter().Column(sig =>
                            {
                                sig.Item().AlignCenter().Text(
                                    $"TP. HCM, ngày {pn.NgayNhap:dd} tháng {pn.NgayNhap:MM} năm {pn.NgayNhap:yyyy}");
                                sig.Item().PaddingTop(4).AlignCenter().Text("Người lập phiếu").SemiBold();
                                sig.Item().PaddingTop(50).AlignCenter().Text(pn.MaNV);
                            });
                        });
                    });

                    // ---- FOOTER ----
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Trang ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(filePath);

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }

        /// <summary>
        /// Xuất Báo cáo Thống kê Nhập theo Quý ra file PDF.
        /// </summary>
        public static void XuatPDFThongKeQuy(List<ThongKeQuyDTO> dsThongKe, int nam)
        {
            string fileName = $"ThongKeNhap_Nam{nam}.pdf";
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            // Tính dòng tổng
            decimal tongQ1 = 0, tongQ2 = 0, tongQ3 = 0, tongQ4 = 0, tongTong = 0;
            foreach (var row in dsThongKe)
            {
                tongQ1   += row.Q1;
                tongQ2   += row.Q2;
                tongQ3   += row.Q3;
                tongQ4   += row.Q4;
                tongTong += row.TongCong;
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("THƯ VIỆN TRƯỜNG ĐẠI HỌC SÀI GÒN")
                            .Bold().FontSize(13);
                        col.Item().AlignCenter()
                            .Text($"THỐNG KÊ NHẬP SÁCH THEO QUÝ — NĂM {nam}")
                            .Bold().FontSize(14).FontColor(Colors.Blue.Medium);
                        col.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    });

                    page.Content().PaddingTop(12).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(30);   // STT
                            cols.RelativeColumn(3);    // Tên sách
                            cols.RelativeColumn(2);    // Q1
                            cols.RelativeColumn(2);    // Q2
                            cols.RelativeColumn(2);    // Q3
                            cols.RelativeColumn(2);    // Q4
                            cols.RelativeColumn(2);    // Tổng
                        });

                        table.Header(header =>
                        {
                            static IContainer HC(IContainer c) =>
                                c.Background(Colors.Blue.Lighten3).Padding(5).AlignCenter();

                            header.Cell().Element(HC).Text("STT").Bold();
                            header.Cell().Element(HC).Text("Đầu sách").Bold();
                            header.Cell().Element(HC).Text("Quý I").Bold();
                            header.Cell().Element(HC).Text("Quý II").Bold();
                            header.Cell().Element(HC).Text("Quý III").Bold();
                            header.Cell().Element(HC).Text("Quý IV").Bold();
                            header.Cell().Element(HC).Text("Tổng cộng").Bold();
                        });

                        for (int i = 0; i < dsThongKe.Count; i++)
                        {
                            var r = dsThongKe[i];
                            string bg = (i % 2 == 0) ? Colors.White : Colors.Grey.Lighten4;

                            static IContainer DC(IContainer c, string color) =>
                                c.Background(color).Padding(5);

                            table.Cell().Element(c => DC(c, bg)).AlignCenter().Text((i + 1).ToString());
                            table.Cell().Element(c => DC(c, bg)).Text(r.TenSach);
                            table.Cell().Element(c => DC(c, bg)).AlignRight()
                                .Text(r.Q1 == 0 ? "-" : r.Q1.ToString("N0"));
                            table.Cell().Element(c => DC(c, bg)).AlignRight()
                                .Text(r.Q2 == 0 ? "-" : r.Q2.ToString("N0"));
                            table.Cell().Element(c => DC(c, bg)).AlignRight()
                                .Text(r.Q3 == 0 ? "-" : r.Q3.ToString("N0"));
                            table.Cell().Element(c => DC(c, bg)).AlignRight()
                                .Text(r.Q4 == 0 ? "-" : r.Q4.ToString("N0"));
                            table.Cell().Element(c => DC(c, bg)).AlignRight()
                                .Text(r.TongCong.ToString("N0")).Bold();
                        }

                        // Dòng tổng cuối bảng
                        static IContainer TC(IContainer c) =>
                            c.Background(Colors.Blue.Lighten4).Padding(5);

                        table.Cell().ColumnSpan(2).Element(TC).AlignCenter().Text("TỔNG CỘNG").Bold();
                        table.Cell().Element(TC).AlignRight().Text(tongQ1.ToString("N0")).Bold();
                        table.Cell().Element(TC).AlignRight().Text(tongQ2.ToString("N0")).Bold();
                        table.Cell().Element(TC).AlignRight().Text(tongQ3.ToString("N0")).Bold();
                        table.Cell().Element(TC).AlignRight().Text(tongQ4.ToString("N0")).Bold();
                        table.Cell().Element(TC).AlignRight().Text(tongTong.ToString("N0")).Bold();
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Trang ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(filePath);

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}
