#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Đọc và ghi workbook .xlsx cho Đầu sách sử dụng thư viện ClosedXML chuẩn của dự án.
    /// </summary>
    internal static class DauSachExcelService
    {
        public static void Ghi(string duongDan, IReadOnlyList<string> tieuDe, IReadOnlyList<IReadOnlyList<string>> dong)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DanhSachDauSach");

            int colCount = Math.Max(1, tieuDe.Count);

            // 1. Tiêu đề chính
            ws.Cell("A1").Value = "DANH SÁCH ĐẦU SÁCH THƯ VIỆN";
            ws.Range(1, 1, 1, colCount).Merge();
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(41, 128, 185);
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Row(1).Height = 35;

            // 2. Ngày xuất
            ws.Cell("A2").Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Range(2, 1, 2, colCount).Merge();
            ws.Cell("A2").Style.Font.Italic = true;
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            // 3. Header bảng dữ liệu
            for (int i = 0; i < tieuDe.Count; i++)
            {
                var cell = ws.Cell(4, i + 1);
                cell.Value = tieuDe[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(41, 128, 185);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
            ws.Row(4).Height = 26;

            // 4. Dữ liệu
            int startRow = 5;
            for (int r = 0; r < dong.Count; r++)
            {
                int currentRow = startRow + r;
                var hang = dong[r];
                for (int c = 0; c < hang.Count; c++)
                {
                    string giaTri = hang[c] ?? string.Empty;
                    var cell = ws.Cell(currentRow, c + 1);

                    // Nếu là cột số (Năm XB, Đơn giá, Số lượng, Đã mượn, Còn lại - từ cột 5 trở đi)
                    if (c >= 5 && decimal.TryParse(giaTri, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal so))
                    {
                        cell.Value = so;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                    else
                    {
                        cell.Value = giaTri;
                        if (c == 0 || c == 2 || c == 3 || c == 4)
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        else
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    }
                }

                // Tô màu xen kẽ dòng
                if (currentRow % 2 == 0)
                {
                    ws.Range(currentRow, 1, currentRow, colCount).Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 252);
                }
            }

            int endRow = startRow + dong.Count - 1;
            if (endRow >= startRow)
            {
                var tableRange = ws.Range(4, 1, endRow, colCount);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(203, 213, 225);
                tableRange.Style.Border.InsideBorderColor = XLColor.FromArgb(226, 232, 240);
            }

            ws.Columns().AdjustToContents();
            workbook.SaveAs(duongDan);
        }

        public static List<string[]> Doc(string duongDan)
        {
            if (!File.Exists(duongDan))
                throw new FileNotFoundException("Không tìm thấy file Excel.", duongDan);

            using var workbook = new XLWorkbook(duongDan);
            var ws = workbook.Worksheets.FirstOrDefault()
                ?? throw new InvalidDataException("File Excel không có trang tính nào.");

            var usedRange = ws.RangeUsed();
            if (usedRange == null)
                return new List<string[]>();

            int firstRow = usedRange.FirstRow().RowNumber();
            int lastRow = usedRange.LastRow().RowNumber();
            int firstCol = usedRange.FirstColumn().ColumnNumber();
            int lastCol = usedRange.LastColumn().ColumnNumber();

            // Tìm hàng tiêu đề chứa các cột (ví dụ 'madausach', 'ma', 'mã', 'tensach', 'tên')
            int headerRow = firstRow;
            for (int r = firstRow; r <= Math.Min(lastRow, firstRow + 5); r++)
            {
                bool coTieuDe = false;
                for (int c = firstCol; c <= lastCol; c++)
                {
                    string txt = ws.Cell(r, c).Value.ToString().Trim().ToLowerInvariant();
                    if (txt.Contains("madausach") || txt.Contains("mã") || txt.Contains("tensach") || txt.Contains("tên"))
                    {
                        coTieuDe = true;
                        break;
                    }
                }
                if (coTieuDe)
                {
                    headerRow = r;
                    break;
                }
            }

            List<string[]> rows = new List<string[]>();
            for (int r = headerRow; r <= lastRow; r++)
            {
                string[] rowValues = new string[lastCol - firstCol + 1];
                bool hasValue = false;
                for (int c = firstCol; c <= lastCol; c++)
                {
                    var cell = ws.Cell(r, c);
                    string val = cell.Value.ToString()?.Trim() ?? string.Empty;
                    rowValues[c - firstCol] = val;
                    if (!string.IsNullOrEmpty(val)) hasValue = true;
                }

                if (hasValue)
                {
                    rows.Add(rowValues);
                }
            }

            return rows;
        }
    }
}
