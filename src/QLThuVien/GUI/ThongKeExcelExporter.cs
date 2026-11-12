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
            sheet.Cell(4, 2).Value = report.TongThu;
            sheet.Cell(4, 3).Value = "Tổng chi";
            sheet.Cell(4, 4).Value = report.TongChi;
            sheet.Cell(4, 5).Value = "Chênh lệch";
            sheet.Cell(4, 6).Value = report.ChenhLech;

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

            IXLRange table = sheet.Range(6, 1, Math.Max(6, row - 1), 7);
            DinhDangBang(table, 6);
            sheet.Column(2).Style.DateFormat.Format = "dd/MM/yyyy";
            sheet.Column(7).Style.NumberFormat.Format = "#,##0 \"đ\"";
            sheet.Columns(1, 7).AdjustToContents();
            sheet.Column(5).Width = Math.Max(24, sheet.Column(5).Width);
            sheet.Column(6).Width = Math.Max(34, sheet.Column(6).Width);
            sheet.SheetView.FreezeRows(6);
        }

        private static void TaoTrangNhanVien(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers =
            {
                "Mã nhân viên", "Nhân viên", "Q1 - Tiền phạt", "Q1 - Tiền nhập", "Q2 - Tiền phạt", "Q2 - Tiền nhập",
                "Q3 - Tiền phạt", "Q3 - Tiền nhập", "Q4 - Tiền phạt", "Q4 - Tiền nhập", "Tổng tiền phạt", "Tổng tiền nhập"
            };
            IEnumerable<object[]> rows = report.ThongKeNhanVienTheoQuy.Select(item => new object[]
            {
                item.MaNV, item.TenNhanVien, item.TienPhatQ1, item.TienNhapQ1, item.TienPhatQ2, item.TienNhapQ2,
                item.TienPhatQ3, item.TienNhapQ3, item.TienPhatQ4, item.TienNhapQ4, item.TongTienPhat, item.TongTienNhap
            });
            TaoTrangQuy(workbook, "NhanVien-Quy", "THỐNG KÊ QUÝ - NHÂN VIÊN", report.NamThongKe, headers, rows, 2);
        }

        private static void TaoTrangDocGia(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers = { "Mã độc giả", "Độc giả", "Q1 - Tiền phạt", "Q2 - Tiền phạt", "Q3 - Tiền phạt", "Q4 - Tiền phạt", "Tổng cộng" };
            IEnumerable<object[]> rows = report.ThongKeDocGiaTheoQuy.Select(item => new object[]
            {
                item.MaDocGia, item.TenDocGia, item.TienPhatQ1, item.TienPhatQ2, item.TienPhatQ3, item.TienPhatQ4, item.TongCong
            });
            TaoTrangQuy(workbook, "DocGia-Quy", "THỐNG KÊ QUÝ - ĐỘC GIẢ", report.NamThongKe, headers, rows, 2);
        }

        private static void TaoTrangDauSach(XLWorkbook workbook, ThongKeTongHopDTO report)
        {
            string[] headers =
            {
                "Mã đầu sách", "Đầu sách", "Q1 - Tiền nhập", "Q1 - Lượt mượn", "Q2 - Tiền nhập", "Q2 - Lượt mượn",
                "Q3 - Tiền nhập", "Q3 - Lượt mượn", "Q4 - Tiền nhập", "Q4 - Lượt mượn", "Tổng tiền nhập", "Tổng lượt mượn"
            };
            IEnumerable<object[]> rows = report.ThongKeDauSachTheoQuy.Select(item => new object[]
            {
                item.MaDauSach, item.TenDauSach, item.TienNhapQ1, item.LuotMuonQ1, item.TienNhapQ2, item.LuotMuonQ2,
                item.TienNhapQ3, item.LuotMuonQ3, item.TienNhapQ4, item.LuotMuonQ4, item.TongTienNhap, item.TongLuotMuon
            });
            TaoTrangQuy(workbook, "DauSach-Quy", "THỐNG KÊ QUÝ - ĐẦU SÁCH", report.NamThongKe, headers, rows, 2);
        }

        private static void TaoTrangQuy(XLWorkbook workbook, string tenSheet, string tieuDe, int nam, string[] headers, IEnumerable<object[]> rows, int cotSoDauTien)
        {
            IXLWorksheet sheet = workbook.AddWorksheet(tenSheet);
            sheet.Cell(1, 1).Value = tieuDe;
            sheet.Range(1, 1, 1, headers.Length).Merge();
            sheet.Cell(2, 1).Value = $"Năm thống kê: {nam}";
            for (int column = 0; column < headers.Length; column++)
                sheet.Cell(4, column + 1).Value = headers[column];

            int row = 5;
            foreach (object[] values in rows)
            {
                for (int column = 0; column < values.Length; column++)
                    sheet.Cell(row, column + 1).Value = XLCellValue.FromObject(values[column], culture);
                row++;
            }

            if (row > 5)
            {
                sheet.Cell(row, 1).Value = "Tổng cộng";
                for (int column = cotSoDauTien + 1; column <= headers.Length; column++)
                    sheet.Cell(row, column).FormulaA1 = $"SUM({sheet.Column(column).ColumnLetter()}5:{sheet.Column(column).ColumnLetter()}{row - 1})";
            }

            IXLRange table = sheet.Range(4, 1, Math.Max(4, row), headers.Length);
            DinhDangBang(table, 4);
            sheet.Range(5, cotSoDauTien + 1, Math.Max(5, row), headers.Length).Style.NumberFormat.Format = "#,##0";
            sheet.Columns(1, headers.Length).AdjustToContents();
            sheet.Column(1).Width = Math.Max(14, sheet.Column(1).Width);
            sheet.Column(2).Width = Math.Max(24, sheet.Column(2).Width);
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
            table.Worksheet.Row(headerRow).Height = 30;
            table.Range(1, 1, 1, table.ColumnCount()).Style.Alignment.WrapText = true;

            if (table.RowCount() > 1)
            {
                table.Rows(2, table.RowCount()).Style.Fill.BackgroundColor = XLColor.White;
                for (int row = 3; row <= table.RowCount(); row += 2)
                    table.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#F1F5F9");
            }
        }
    }
}
