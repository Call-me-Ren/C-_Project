using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using QLThuVien.DTO;

namespace QLThuVien.GUI
{
    internal sealed class ThongKeExcelExporter
    {
        private static readonly CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");

        public void Xuat(ThongKeTongHopDTO report, DateTime tuNgay, DateTime denNgay, string duongDan)
        {
            ArgumentNullException.ThrowIfNull(report);
            if (string.IsNullOrWhiteSpace(duongDan))
                throw new ArgumentException("Đường dẫn không hợp lệ.", nameof(duongDan));

            using XLWorkbook workbook = new XLWorkbook();
            TaoTrangThuChi(workbook, report, tuNgay, denNgay);
            TaoTrangNhanVien(workbook, report);
            TaoTrangDocGia(workbook, report);
            TaoTrangDauSach(workbook, report);
            workbook.Worksheet("Thu-Chi").SetTabActive();
            workbook.SaveAs(Path.GetFullPath(duongDan));
        }

        private static void TaoTrangThuChi(XLWorkbook workbook, ThongKeTongHopDTO report, DateTime tuNgay, DateTime denNgay)
        {
            IXLWorksheet sheet = workbook.AddWorksheet("Thu-Chi");
            sheet.Cell(1, 1).Value = "BÁO CÁO THỐNG KÊ THU - CHI";
            sheet.Range(1, 1, 1, 7).Merge();
            sheet.Cell(2, 1).Value = $"Kỳ báo cáo: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}";
            sheet.Range(2, 1, 2, 4).Merge();
            sheet.Cell(2, 5).Value = $"Năm thống kê quý: {report.NamThongKe}";
            sheet.Range(2, 5, 2, 7).Merge();
            sheet.Cell(4, 1).Value = "Tổng thu";
            sheet.Cell(4, 3).Value = "Tổng chi";
            sheet.Cell(4, 5).Value = "Chênh lệch";
            sheet.Range("A1:G1").Style.Font.Bold = true;
            sheet.Range("A1:G1").Style.Font.FontSize = 16;
            sheet.Range("A2:G2").Style.Font.Bold = true;
            sheet.Range("A4:F4").Style.Font.Bold = true;
            sheet.Range("A4:F4").Style.Fill.BackgroundColor = XLColor.FromHtml("#EAF2F8");
            sheet.Range("B4").Style.NumberFormat.Format = DinhDangTien(report.TongThu);
            sheet.Range("D4").Style.NumberFormat.Format = DinhDangTien(report.TongChi);
            sheet.Range("F4").Style.NumberFormat.Format = DinhDangTien(report.ChenhLech);

            string[] headers = { "STT", "Ngày", "Loại", "Mã phiếu", "Đối tượng", "Diễn giải", "Số tiền (đ)" };
            for (int column = 0; column < headers.Length; column++)
                sheet.Cell(6, column + 1).Value = headers[column];

            int row = 7;
            foreach (ThongKeGiaoDichDTO item in report.GiaoDich)
            {
                sheet.Cell(row, 1).Value = row - 6;
                sheet.Cell(row, 2).Value = item.Ngay;
                sheet.Cell(row, 3).Value = item.Loai;
                sheet.Cell(row, 4).Value = item.MaGiaoDich;
                sheet.Cell(row, 5).Value = item.DoiTuong;
                sheet.Cell(row, 6).Value = item.DienGiai;
                sheet.Cell(row, 7).Value = item.SoTien;
                row++;
            }

            int lastTransactionRow = Math.Max(7, row - 1);
            sheet.Cell(4, 2).FormulaA1 = $"SUMIF(C7:C{lastTransactionRow},\"Thu\",G7:G{lastTransactionRow})";
            sheet.Cell(4, 4).FormulaA1 = $"SUMIF(C7:C{lastTransactionRow},\"Chi\",G7:G{lastTransactionRow})";
            sheet.Cell(4, 6).FormulaA1 = "B4-D4";

            IXLRange table = sheet.Range(6, 1, Math.Max(6, row - 1), 7);
            DinhDangBang(table, 6);
            if (row > 7)
            {
                sheet.Range(7, 2, row - 1, 2).Style.DateFormat.Format = "dd/MM/yyyy";
                for (int dataRow = 7; dataRow < row; dataRow++)
                    sheet.Cell(dataRow, 7).Style.NumberFormat.Format = DinhDangTien(report.GiaoDich[dataRow - 7].SoTien);
            }
            sheet.Column(1).Width = 8;
            sheet.Column(2).Width = 13;
            sheet.Column(3).Width = 10;
            sheet.Column(4).Width = 13;
            sheet.Column(5).Width = 24;
            sheet.Column(6).Width = 30;
            sheet.Column(7).Width = 16;
            sheet.SheetView.ZoomScale = 70;
            sheet.SheetView.FreezeRows(6);
        }

        private static void TaoTrangNhanVien(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers =
            {
                "Mã nhân viên", "Nhân viên", "Q1\nTiền phạt", "Q1\nTiền nhập", "Q2\nTiền phạt", "Q2\nTiền nhập",
                "Q3\nTiền phạt", "Q3\nTiền nhập", "Q4\nTiền phạt", "Q4\nTiền nhập", "Tổng tiền\nphạt", "Tổng tiền\nnhập"
            };
            IEnumerable<object[]> rows = report.ThongKeNhanVienTheoQuy.Select(item => new object[]
            {
                item.MaNV, item.TenNhanVien, item.TienPhatQ1, item.TienNhapQ1, item.TienPhatQ2, item.TienNhapQ2,
                item.TienPhatQ3, item.TienNhapQ3, item.TienPhatQ4, item.TienNhapQ4, 0m, 0m
            });
            TaoTrangQuy(workbook, "NhanVien-Quy", "THỐNG KÊ QUÝ - NHÂN VIÊN", report.NamThongKe, headers, rows, 2,
                new Dictionary<int, int[]> { [11] = new[] { 3, 5, 7, 9 }, [12] = new[] { 4, 6, 8, 10 } });
        }

        private static void TaoTrangDocGia(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers = { "Mã độc giả", "Độc giả", "Q1\nTiền phạt", "Q2\nTiền phạt", "Q3\nTiền phạt", "Q4\nTiền phạt", "Tổng cộng" };
            IEnumerable<object[]> rows = report.ThongKeDocGiaTheoQuy.Select(item => new object[]
            {
                item.MaDocGia, item.TenDocGia, item.TienPhatQ1, item.TienPhatQ2, item.TienPhatQ3, item.TienPhatQ4, 0m
            });
            TaoTrangQuy(workbook, "DocGia-Quy", "THỐNG KÊ QUÝ - ĐỘC GIẢ", report.NamThongKe, headers, rows, 2,
                new Dictionary<int, int[]> { [7] = new[] { 3, 4, 5, 6 } });
        }

        private static void TaoTrangDauSach(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers =
            {
                "Mã đầu sách", "Đầu sách", "Q1\nTiền nhập", "Q1\nLượt mượn", "Q2\nTiền nhập", "Q2\nLượt mượn",
                "Q3\nTiền nhập", "Q3\nLượt mượn", "Q4\nTiền nhập", "Q4\nLượt mượn", "Tổng tiền\nnhập", "Tổng lượt\nmượn"
            };
            IEnumerable<object[]> rows = report.ThongKeDauSachTheoQuy.Select(item => new object[]
            {
                item.MaDauSach, item.TenDauSach, item.TienNhapQ1, item.LuotMuonQ1, item.TienNhapQ2, item.LuotMuonQ2,
                item.TienNhapQ3, item.LuotMuonQ3, item.TienNhapQ4, item.LuotMuonQ4, 0m, 0
            });
            TaoTrangQuy(workbook, "DauSach-Quy", "THỐNG KÊ QUÝ - ĐẦU SÁCH", report.NamThongKe, headers, rows, 2,
                new Dictionary<int, int[]> { [11] = new[] { 3, 5, 7, 9 }, [12] = new[] { 4, 6, 8, 10 } });
        }

        private static void TaoTrangQuy(XLWorkbook workbook, string tenSheet, string tieuDe, int nam, string[] headers, IEnumerable<object[]> rows, int cotSoDauTien, IReadOnlyDictionary<int, int[]> congThucTongTheoDong)
        {
            IXLWorksheet sheet = workbook.AddWorksheet(tenSheet);
            sheet.Cell(1, 1).Value = tieuDe;
            sheet.Range(1, 1, 1, headers.Length).Merge();
            sheet.Cell(2, 1).Value = $"Năm thống kê: {nam}";
            sheet.Cell(3, 1).Value = "Bảng quý tính tổng tiền phạt của tất cả phiếu phạt đã lập, kể cả phiếu chưa thanh toán. Tổng thu ở bảng Thu - Chi chỉ tính phiếu đã thanh toán.";
            sheet.Range(3, 1, 3, headers.Length).Merge();
            sheet.Range(3, 1, 3, headers.Length).Style.Font.FontColor = XLColor.DimGray;
            sheet.Range(3, 1, 3, headers.Length).Style.Font.Italic = true;
            sheet.Range(3, 1, 3, headers.Length).Style.Alignment.WrapText = true;
            sheet.Row(3).Height = 30;
            for (int column = 0; column < headers.Length; column++)
                sheet.Cell(4, column + 1).Value = headers[column];

            List<object[]> dataRows = rows.ToList();
            int row = 5;
            foreach (object[] values in dataRows)
            {
                for (int column = 0; column < values.Length; column++)
                {
                    sheet.Cell(row, column + 1).Value = XLCellValue.FromObject(values[column], culture);
                    if (values[column] is decimal amount)
                        sheet.Cell(row, column + 1).Style.NumberFormat.Format = DinhDangTien(amount);
                    else if (values[column] is int count)
                        sheet.Cell(row, column + 1).Style.NumberFormat.Format = DinhDangSo(count);
                }
                foreach (KeyValuePair<int, int[]> formula in congThucTongTheoDong)
                {
                    string cellReferences = string.Join(",", formula.Value.Select(column => $"{sheet.Column(column).ColumnLetter()}{row}"));
                    sheet.Cell(row, formula.Key).FormulaA1 = $"SUM({cellReferences})";
                    if (values[formula.Key - 1] is decimal)
                    {
                        decimal total = formula.Value.Sum(column => Convert.ToDecimal(values[column - 1], culture));
                        sheet.Cell(row, formula.Key).Style.NumberFormat.Format = DinhDangTien(total);
                    }
                    else
                    {
                        int total = formula.Value.Sum(column => Convert.ToInt32(values[column - 1], culture));
                        sheet.Cell(row, formula.Key).Style.NumberFormat.Format = DinhDangSo(total);
                    }
                }
                row++;
            }

            int totalRow = row;
            sheet.Cell(totalRow, 1).Value = "Tổng cộng";
            for (int column = cotSoDauTien + 1; column <= headers.Length; column++)
            {
                int firstSumRow = dataRows.Count == 0 ? totalRow + 1 : 5;
                int lastSumRow = dataRows.Count == 0 ? totalRow + 1 : totalRow - 1;
                sheet.Cell(totalRow, column).FormulaA1 = $"SUM({sheet.Column(column).ColumnLetter()}{firstSumRow}:{sheet.Column(column).ColumnLetter()}{lastSumRow})";
                bool isMoney = dataRows.Count > 0
                    ? dataRows[0][column - 1] is decimal
                    : !headers[column - 1].Contains("Lượt mượn", StringComparison.Ordinal);
                congThucTongTheoDong.TryGetValue(column, out int[]? totalSources);
                if (isMoney)
                {
                    decimal total = dataRows.Count == 0
                        ? 0
                        : dataRows.Sum(values => totalSources == null
                            ? Convert.ToDecimal(values[column - 1], culture)
                            : totalSources.Sum(source => Convert.ToDecimal(values[source - 1], culture)));
                    sheet.Cell(totalRow, column).Style.NumberFormat.Format = DinhDangTien(total);
                }
                else
                {
                    int total = dataRows.Count == 0
                        ? 0
                        : dataRows.Sum(values => totalSources == null
                            ? Convert.ToInt32(values[column - 1], culture)
                            : totalSources.Sum(source => Convert.ToInt32(values[source - 1], culture)));
                    sheet.Cell(totalRow, column).Style.NumberFormat.Format = DinhDangSo(total);
                }
            }
            sheet.Row(totalRow).Style.Font.Bold = true;

            IXLRange table = sheet.Range(4, 1, totalRow, headers.Length);
            DinhDangBang(table, 4);
            for (int column = 1; column <= headers.Length; column++)
            {
                sheet.Column(column).Width = column switch
                {
                    1 => 18,
                    2 => 28,
                    _ when headers[column - 1].Contains("Lượt mượn", StringComparison.Ordinal) || headers[column - 1] == "Tổng lượt mượn" => 14,
                    _ => 15
                };
            }
            sheet.Range(1, 1, 1, headers.Length).Style.Font.Bold = true;
            sheet.Range(1, 1, 1, headers.Length).Style.Font.FontSize = 15;
            sheet.Range(2, 1, 2, headers.Length).Style.Font.Italic = true;
            sheet.Row(4).Height = 46;
            sheet.SheetView.ZoomScale = 70;
            sheet.SheetView.FreezeRows(4);
        }

        private static void DinhDangBang(IXLRange table, int headerRow)
        {
            int headerRowInTable = headerRow - table.RangeAddress.FirstAddress.RowNumber + 1;
            table.SetAutoFilter();
            table.Style.Font.FontName = "Arial";
            table.Style.Font.FontSize = 10;
            table.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            table.Row(headerRowInTable).Style.Font.Bold = true;
            table.Row(headerRowInTable).Style.Fill.BackgroundColor = XLColor.FromHtml("#1F70C1");
            table.Row(headerRowInTable).Style.Font.FontColor = XLColor.White;
            table.Row(headerRowInTable).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            table.Worksheet.Row(headerRow).Height = 44;
            table.Range(1, 1, 1, table.ColumnCount()).Style.Alignment.WrapText = true;

            if (table.RowCount() > 1)
            {
                table.Rows(2, table.RowCount()).Style.Fill.BackgroundColor = XLColor.White;
                for (int row = 3; row <= table.RowCount(); row += 2)
                    table.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F1F5F9");
            }
        }

        private static string DinhDangTien(decimal value)
        {
            decimal absolute = Math.Abs(value);
            if (value == 0) return "0 \"đ\"";
            if (absolute >= 1_000_000_000m) return "#0\\.###\\.###\\.##0 \"đ\"";
            if (absolute >= 1_000_000m) return "#0\\.###\\.##0 \"đ\"";
            if (absolute >= 1_000m) return "#0\\.##0 \"đ\"";
            return "#0 \"đ\"";
        }

        private static string DinhDangSo(int value)
        {
            int absolute = Math.Abs(value);
            if (absolute >= 1_000) return "#0\\.##0";
            return "0";
        }
    }
}
