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
    public sealed class PhieuPhatPdfExporter
    {
        private readonly PhieuPhatBaoCaoDTO phieu;
        private float fontSize;
        private float tableFontSize;

        public PhieuPhatPdfExporter(PhieuPhatBaoCaoDTO phieu)
        {
            this.phieu = phieu ?? throw new ArgumentNullException(nameof(phieu));
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
                page.Margin(18);
                page.DefaultTextStyle(style => style.FontFamily("Lato").FontSize(fontSize));
                page.Header().Column(header =>
                {
                    header.Item().SkipOnce().BorderBottom(1).BorderColor(Colors.Blue.Medium).Row(row =>
                    {
                        row.RelativeItem().Text($"PHIẾU PHẠT {phieu.MaPhieuPhat}").Bold().FontSize(fontSize);
                        row.ConstantItem(70).AlignRight().DefaultTextStyle(style => style.Bold().FontSize(fontSize)).Text(text =>
                        {
                            text.Span("Trang ");
                            text.CurrentPageNumber();
                            text.Span("/");
                            text.TotalPages();
                        });
                    });
                });
                page.Content().PaddingTop(5).Column(column =>
                {
                    column.Item().PaddingBottom(3).AlignCenter().Text("PHIẾU PHẠT")
                        .Bold().FontSize(fontSize * 2).FontColor(Colors.Blue.Darken3);
                    column.Item().Element(VungThongTin);
                    column.Item().PaddingTop(4).Text("CHI TIẾT VI PHẠM").Bold().FontSize(fontSize * 1.15f);
                    if (!gomCuoi)
                    {
                        column.Item().PaddingTop(2).Element(container => BangChiTiet(container, 0, phieu.ChiTiet.Count));
                        column.Item().PaddingTop(4).AlignRight()
                            .Text($"Tổng tiền phạt: {Tien(phieu.TongTienPhat)}")
                            .Bold().FontSize(fontSize * 1.15f);
                        column.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().AlignCenter().Column(signature =>
                            {
                                signature.Item().Text("Người lập phiếu").Bold();
                                signature.Item().PaddingTop(22).Text("........................................");
                            });
                            row.RelativeItem().AlignCenter().Column(signature =>
                            {
                                signature.Item().Text("Người nộp").Bold();
                                signature.Item().PaddingTop(22).Text("........................................");
                            });
                        });
                    }
                    else
                    {
                        int splitAt = Math.Max(0, phieu.ChiTiet.Count - 7);
                        if (splitAt > 0)
                            column.Item().PaddingTop(2).Element(container => BangChiTiet(container, 0, splitAt));
                        if (splitAt > 0)
                            column.Item().PageBreak();
                        column.Item().ShowEntire().Column(last =>
                        {
                            last.Item().PaddingTop(2).Element(container => BangChiTiet(container, splitAt, phieu.ChiTiet.Count));
                            last.Item().PaddingTop(4).AlignRight()
                                .Text($"Tổng tiền phạt: {Tien(phieu.TongTienPhat)}")
                                .Bold().FontSize(fontSize * 1.15f);
                            last.Item().PaddingTop(8).Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Column(signature =>
                                {
                                    signature.Item().Text("Người lập phiếu").Bold();
                                    signature.Item().PaddingTop(22).Text("........................................");
                                });
                                row.RelativeItem().AlignCenter().Column(signature =>
                                {
                                    signature.Item().Text("Người nộp").Bold();
                                    signature.Item().PaddingTop(22).Text("........................................");
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

        private void VungThongTin(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Lighten2)
                .Background(Colors.Grey.Lighten4).Padding(10).Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Mã phiếu: ").Bold();
                            text.Span(phieu.MaPhieuPhat);
                        });
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Ngày lập: ").Bold();
                            text.Span(phieu.NgayLap.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN")));
                        });
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Trạng thái: ").Bold();
                            text.Span(phieu.TrangThai);
                        });
                    });
                    column.Item().PaddingTop(3).Row(row =>
                    {
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Độc giả: ").Bold();
                            text.Span($"{phieu.MaDocGia} - {phieu.TenDocGia}");
                        });
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Nhân viên lập: ").Bold();
                            text.Span($"{phieu.MaNV} - {phieu.TenNhanVien}");
                        });
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Phiếu mượn: ").Bold();
                            text.Span(phieu.MaPhieuMuon ?? "Không có");
                        });
                    });
                });
        }

        private void BangChiTiet(IContainer container, int batDau, int ketThuc)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1.6f);
                    columns.RelativeColumn(1.8f);
                    columns.RelativeColumn(0.65f);
                    columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(0.9f);
                    columns.RelativeColumn(1.7f);
                });
                table.Header(header =>
                {
                    Header(header.Cell(), "Cuốn sách");
                    Header(header.Cell(), "Quy định");
                    Header(header.Cell(), "Ngày trễ");
                    Header(header.Cell(), "Đơn giá");
                    Header(header.Cell(), "Tiền phạt");
                    Header(header.Cell(), "Ghi chú");
                });

                if (phieu.ChiTiet.Count == 0)
                {
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "Chưa có vi phạm.", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    Cell(table.Cell(), "", 0);
                    return;
                }

                for (int index = batDau; index < ketThuc; index++)
                {
                    ChiTietPhieuPhatBaoCaoDTO detail = phieu.ChiTiet[index];
                    int row = index + 1;
                    Cell(table.Cell(), $"{detail.MaSach}\n{detail.TenSach}", row);
                    Cell(table.Cell(), $"{detail.MaQD}\n{detail.TenQD}", row);
                    Cell(table.Cell(), detail.SoNgayTre?.ToString() ?? "—", row, true);
                    Cell(table.Cell(), Tien(detail.DonGia), row, true);
                    Cell(table.Cell(), Tien(detail.TienPhat), row, true);
                    Cell(table.Cell(), detail.GhiChu ?? string.Empty, row);
                }
            });
        }

        private void Header(IContainer container, string text)
        {
            container.Background(Colors.Blue.Darken2).Border(0.5f).BorderColor(Colors.Grey.Darken1)
                .Padding(3).AlignCenter().DefaultTextStyle(style => style.FontColor(Colors.White).Bold().FontSize(tableFontSize))
                .Text(text);
        }

        private void Cell(IContainer container, string text, int row, bool alignRight = false)
        {
            container.ShowEntire().Background(row % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White)
                .BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                .AlignMiddle().DefaultTextStyle(style => style.FontSize(tableFontSize))
                .Element(inner => alignRight ? inner.AlignRight() : inner).Text(text);
        }

        private static string Tien(decimal amount) =>
            amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
    }
}

