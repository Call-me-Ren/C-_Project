using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Dịch vụ xuất báo cáo, danh sách ra file Excel (.xlsx) sử dụng ClosedXML.
    /// Tuân thủ quy tắc 3 lớp: Tách biệt logic tạo file khỏi giao diện WinForms.
    /// </summary>
    public static class ExcelExportService
    {
        public static void XuatDanhSachDocGia(IEnumerable<DocGiaDTO> danhSach, string filePath)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DanhSachDocGia");

            // 1. Tiêu đề
            ws.Cell("A1").Value = "DANH SÁCH ĐỘC GIẢ THƯ VIỆN";
            ws.Range("A1:G1").Merge();
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(41, 128, 185);
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Row(1).Height = 35;

            // 2. Ngày xuất
            ws.Cell("A2").Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Range("A2:G2").Merge();
            ws.Cell("A2").Style.Font.Italic = true;
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            // 3. Header bảng dữ liệu
            string[] headers = new[] { "STT", "Mã ĐG", "Họ và tên", "Ngày sinh", "Số điện thoại", "Mã loại", "Loại độc giả" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(4, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(41, 128, 185);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
            ws.Row(4).Height = 26;

            // 4. Đổ dữ liệu
            int row = 5;
            int stt = 1;
            foreach (var dg in danhSach)
            {
                ws.Cell(row, 1).Value = stt++;
                ws.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 2).Value = dg.MaDG;
                ws.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 3).Value = dg.HoTen;

                if (dg.NgaySinh.HasValue)
                {
                    ws.Cell(row, 4).Value = dg.NgaySinh.Value.ToString("dd/MM/yyyy");
                    ws.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(row, 4).Value = "-";
                    ws.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                ws.Cell(row, 5).Value = dg.SDT ?? "-";
                ws.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 6).Value = dg.MaLDG;
                ws.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 7).Value = dg.TenLDG ?? "-";

                // Tô màu xen kẽ dòng (Zebra striping)
                if (row % 2 == 0)
                {
                    ws.Range(row, 1, row, 7).Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 252);
                }

                row++;
            }

            // Kẻ viền bảng dữ liệu
            if (row > 5)
            {
                var tableRange = ws.Range(4, 1, row - 1, 7);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(203, 213, 225);
                tableRange.Style.Border.InsideBorderColor = XLColor.FromArgb(226, 232, 240);
            }

            ws.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        }

        public static void XuatLichSuMuonTra(DocGiaDTO docGia, DataTable dtLichSu, string filePath)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("LichSuMuonTra");

            // 1. Tiêu đề
            ws.Cell("A1").Value = "LỊCH SỬ MƯỢN TRẢ SÁCH";
            ws.Range("A1:H1").Merge();
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Font.FontColor = XLColor.FromArgb(39, 174, 96);
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Row(1).Height = 32;

            // 2. Thông tin độc giả
            ws.Cell("A2").Value = $"Độc giả: {docGia.HoTen} (Mã: {docGia.MaDG}) | SĐT: {docGia.SDT ?? "Không có"} | Loại: {docGia.TenLDG ?? docGia.MaLDG}";
            ws.Range("A2:H2").Merge();
            ws.Cell("A2").Style.Font.Bold = true;
            ws.Cell("A2").Style.Font.FontSize = 11;
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("A3").Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Range("A3:H3").Merge();
            ws.Cell("A3").Style.Font.Italic = true;
            ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            // 3. Header bảng dữ liệu
            string[] headers = new[] { "STT", "Mã PM", "Ngày mượn", "Hạn trả", "Mã sách", "Tên sách", "Ngày thực trả", "Trạng thái" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(5, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(39, 174, 96);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
            ws.Row(5).Height = 26;

            // 4. Đổ dữ liệu
            int row = 6;
            int stt = 1;
            foreach (DataRow r in dtLichSu.Rows)
            {
                ws.Cell(row, 1).Value = stt++;
                ws.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 2).Value = r["MaPM"]?.ToString() ?? string.Empty;
                ws.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                if (r["NgayMuon"] != DBNull.Value && DateTime.TryParse(r["NgayMuon"].ToString(), out DateTime nm))
                {
                    ws.Cell(row, 3).Value = nm.ToString("dd/MM/yyyy");
                    ws.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                if (r["HanTra"] != DBNull.Value && DateTime.TryParse(r["HanTra"].ToString(), out DateTime ht))
                {
                    ws.Cell(row, 4).Value = ht.ToString("dd/MM/yyyy");
                    ws.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                ws.Cell(row, 5).Value = r["MaSach"]?.ToString() ?? string.Empty;
                ws.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(row, 6).Value = r["TenSach"]?.ToString() ?? string.Empty;

                if (r["NgayThucTra"] != DBNull.Value && DateTime.TryParse(r["NgayThucTra"].ToString(), out DateTime ntt))
                {
                    ws.Cell(row, 7).Value = ntt.ToString("dd/MM/yyyy");
                    ws.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(row, 7).Value = "Chưa trả";
                    ws.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(row, 7).Style.Font.FontColor = XLColor.FromArgb(231, 76, 60);
                }

                string trangThai = r["TrangThai"]?.ToString() ?? string.Empty;
                var cellTT = ws.Cell(row, 8);
                cellTT.Value = trangThai;
                cellTT.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                if (trangThai == "Quá hạn")
                {
                    cellTT.Style.Font.Bold = true;
                    cellTT.Style.Font.FontColor = XLColor.FromArgb(192, 57, 43);
                }
                else if (trangThai == "Đã trả")
                {
                    cellTT.Style.Font.FontColor = XLColor.FromArgb(39, 174, 96);
                }

                if (row % 2 == 1)
                {
                    ws.Range(row, 1, row, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(248, 250, 252);
                }

                row++;
            }

            if (row > 6)
            {
                var tableRange = ws.Range(5, 1, row - 1, 8);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(203, 213, 225);
                tableRange.Style.Border.InsideBorderColor = XLColor.FromArgb(226, 232, 240);
            }

            ws.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        }

        /// <summary>
        /// Đọc danh sách độc giả từ file Excel (.xlsx).
        /// Hỗ trợ cả file mẫu xuất từ phần mềm hoặc bảng tính tùy chỉnh.
        /// </summary>
        public static List<DocGiaDTO> NhapDanhSachDocGiaTuExcel(string filePath)
        {
            var result = new List<DocGiaDTO>();
            using var workbook = new XLWorkbook(filePath);
            var ws = workbook.Worksheet(1);
            if (ws == null) return result;

            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 2) return result;

            // Tìm hàng tiêu đề (quét từ hàng 1 đến 10)
            int headerRowIndex = -1;
            int colMaDG = -1;
            int colHoTen = -1;
            int colHoLot = -1;
            int colTenDG = -1;
            int colNgaySinh = -1;
            int colSDT = -1;
            int colMaLDG = -1;

            for (int r = 1; r <= Math.Min(10, lastRow); r++)
            {
                var row = ws.Row(r);
                int lastCol = row.LastCellUsed()?.Address.ColumnNumber ?? 0;
                for (int c = 1; c <= lastCol; c++)
                {
                    string text = row.Cell(c).GetString().Trim().ToLower();
                    if (text.Contains("mã đg") || text.Contains("madg") || text.Contains("mã độc giả"))
                    {
                        headerRowIndex = r;
                        break;
                    }
                }
                if (headerRowIndex != -1) break;
            }

            // Nếu tìm thấy hàng tiêu đề, xác định vị trí từng cột
            if (headerRowIndex != -1)
            {
                var hRow = ws.Row(headerRowIndex);
                int lastCol = hRow.LastCellUsed()?.Address.ColumnNumber ?? 0;
                for (int c = 1; c <= lastCol; c++)
                {
                    string text = hRow.Cell(c).GetString().Trim().ToLower();
                    if (text.Contains("mã đg") || text.Contains("madg") || text.Contains("mã độc giả"))
                        colMaDG = c;
                    else if (text.Contains("họ lót") || text.Contains("holot"))
                        colHoLot = c;
                    else if (text.Contains("tên đg") || text == "tên" || text.Contains("tendg"))
                        colTenDG = c;
                    else if (text.Contains("họ và tên") || text.Contains("họ tên") || text.Contains("hoten"))
                        colHoTen = c;
                    else if (text.Contains("ngày sinh") || text.Contains("ngaysinh"))
                        colNgaySinh = c;
                    else if (text.Contains("số điện thoại") || text.Contains("sđt") || text.Contains("sdt") || text.Contains("điện thoại"))
                        colSDT = c;
                    else if (text.Contains("mã loại") || text.Contains("maldg") || text.Contains("loại độc giả"))
                        colMaLDG = c;
                }
            }
            else
            {
                headerRowIndex = 4;
                colMaDG = 2;
                colHoTen = 3;
                colNgaySinh = 4;
                colSDT = 5;
                colMaLDG = 6;
            }

            if (colMaDG == -1) colMaDG = 2;

            for (int r = headerRowIndex + 1; r <= lastRow; r++)
            {
                var row = ws.Row(r);
                string maDG = colMaDG != -1 ? row.Cell(colMaDG).GetString().Trim() : string.Empty;
                if (string.IsNullOrWhiteSpace(maDG)) continue;

                string? hoLot = null;
                string tenDG = string.Empty;

                if (colHoLot != -1 && colTenDG != -1)
                {
                    hoLot = row.Cell(colHoLot).GetString().Trim();
                    tenDG = row.Cell(colTenDG).GetString().Trim();
                }
                else if (colHoTen != -1)
                {
                    string hoTen = row.Cell(colHoTen).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(hoTen))
                    {
                        int lastSpace = hoTen.LastIndexOf(' ');
                        if (lastSpace > 0)
                        {
                            hoLot = hoTen.Substring(0, lastSpace).Trim();
                            tenDG = hoTen.Substring(lastSpace + 1).Trim();
                        }
                        else
                        {
                            tenDG = hoTen;
                        }
                    }
                }
                else if (colTenDG != -1)
                {
                    tenDG = row.Cell(colTenDG).GetString().Trim();
                }

                if (string.IsNullOrWhiteSpace(tenDG))
                {
                    tenDG = "Độc giả";
                }

                DateTime? ngaySinh = null;
                if (colNgaySinh != -1)
                {
                    var cellNS = row.Cell(colNgaySinh);
                    if (cellNS.DataType == XLDataType.DateTime)
                    {
                        ngaySinh = cellNS.GetDateTime();
                    }
                    else
                    {
                        string strNS = cellNS.GetString().Trim();
                        if (DateTime.TryParse(strNS, out DateTime parsedNS))
                        {
                            ngaySinh = parsedNS;
                        }
                    }
                }

                string? sdt = null;
                if (colSDT != -1)
                {
                    string rawSDT = row.Cell(colSDT).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(rawSDT) && rawSDT != "-")
                    {
                        sdt = rawSDT;
                        if (!sdt.StartsWith("0") && sdt.Length == 9)
                        {
                            sdt = "0" + sdt;
                        }
                    }
                }

                string maLDG = "LDG01";
                if (colMaLDG != -1)
                {
                    string rawLDG = row.Cell(colMaLDG).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(rawLDG) && rawLDG != "-")
                    {
                        maLDG = rawLDG;
                    }
                }

                result.Add(new DocGiaDTO(maDG, hoLot, tenDG, ngaySinh, sdt, maLDG, null));
            }

            return result;
        }
    }
}
