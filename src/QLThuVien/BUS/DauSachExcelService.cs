#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace QLThuVien.BUS
{
    // Đọc/ghi workbook .xlsx tối giản bằng các thư viện có sẵn của .NET.
    internal static class DauSachExcelService
    {
        private static readonly XNamespace SpreadsheetNs = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private static readonly XNamespace OfficeRelNs = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        private static readonly XNamespace PackageRelNs = "http://schemas.openxmlformats.org/package/2006/relationships";
        private static readonly XNamespace ContentTypeNs = "http://schemas.openxmlformats.org/package/2006/content-types";

        public static void Ghi(string duongDan, IReadOnlyList<string> tieuDe, IReadOnlyList<IReadOnlyList<string>> dong)
        {
            using FileStream stream = File.Create(duongDan);
            using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create);

            GhiXml(archive, "[Content_Types].xml", new XDocument(
                new XElement(ContentTypeNs + "Types",
                    new XElement(ContentTypeNs + "Default", new XAttribute("Extension", "rels"), new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
                    new XElement(ContentTypeNs + "Default", new XAttribute("Extension", "xml"), new XAttribute("ContentType", "application/xml")),
                    new XElement(ContentTypeNs + "Override", new XAttribute("PartName", "/xl/workbook.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")),
                    new XElement(ContentTypeNs + "Override", new XAttribute("PartName", "/xl/worksheets/sheet1.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml")))));
            GhiXml(archive, "_rels/.rels", new XDocument(
                new XElement(PackageRelNs + "Relationships",
                    new XElement(PackageRelNs + "Relationship", new XAttribute("Id", "rId1"), new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"), new XAttribute("Target", "xl/workbook.xml")))));
            GhiXml(archive, "xl/workbook.xml", new XDocument(
                new XElement(SpreadsheetNs + "workbook",
                    new XAttribute(XNamespace.Xmlns + "r", OfficeRelNs),
                    new XElement(SpreadsheetNs + "sheets",
                        new XElement(SpreadsheetNs + "sheet", new XAttribute("name", "DanhSachDauSach"), new XAttribute("sheetId", "1"), new XAttribute(OfficeRelNs + "id", "rId1"))))));
            GhiXml(archive, "xl/_rels/workbook.xml.rels", new XDocument(
                new XElement(PackageRelNs + "Relationships",
                    new XElement(PackageRelNs + "Relationship", new XAttribute("Id", "rId1"), new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"), new XAttribute("Target", "worksheets/sheet1.xml")))));

            XElement sheetData = new XElement(SpreadsheetNs + "sheetData");
            ThemDong(sheetData, 1, tieuDe);
            for (int i = 0; i < dong.Count; i++) ThemDong(sheetData, i + 2, dong[i]);
            GhiXml(archive, "xl/worksheets/sheet1.xml", new XDocument(new XElement(SpreadsheetNs + "worksheet", sheetData)));
        }

        public static List<string[]> Doc(string duongDan)
        {
            using ZipArchive archive = ZipFile.OpenRead(duongDan);
            XDocument workbook = DocXml(archive, "xl/workbook.xml");
            XElement firstSheet = workbook.Descendants(SpreadsheetNs + "sheet").FirstOrDefault()
                ?? throw new InvalidDataException("File Excel không có trang tính.");
            string relationId = (string)firstSheet.Attribute(OfficeRelNs + "id");
            XDocument relationships = DocXml(archive, "xl/_rels/workbook.xml.rels");
            XElement relation = relationships.Root.Elements(PackageRelNs + "Relationship")
                .FirstOrDefault(x => (string)x.Attribute("Id") == relationId)
                ?? throw new InvalidDataException("Không tìm thấy dữ liệu trang tính trong file Excel.");
            string target = (string)relation.Attribute("Target");
            string sheetPath = target.StartsWith("/", StringComparison.Ordinal)
                ? target.TrimStart('/')
                : "xl/" + target.Replace("../", string.Empty);
            XDocument sheet = DocXml(archive, sheetPath);
            List<string> sharedStrings = DocXml(archive, "xl/sharedStrings.xml", batBuoc: false)?
                .Descendants(SpreadsheetNs + "si")
                .Select(si => string.Concat(si.Descendants(SpreadsheetNs + "t").Select(t => t.Value)))
                .ToList() ?? new List<string>();

            List<string[]> rows = new List<string[]>();
            foreach (XElement row in sheet.Descendants(SpreadsheetNs + "row"))
            {
                Dictionary<int, string> cells = new Dictionary<int, string>();
                foreach (XElement cell in row.Elements(SpreadsheetNs + "c"))
                {
                    string address = (string)cell.Attribute("r") ?? string.Empty;
                    int column = LayChiSoCot(address);
                    string type = (string)cell.Attribute("t");
                    XElement valueNode = cell.Element(SpreadsheetNs + "v");
                    string value = type == "inlineStr"
                        ? string.Concat(cell.Descendants(SpreadsheetNs + "t").Select(t => t.Value))
                        : valueNode?.Value ?? string.Empty;
                    if (type == "s" && int.TryParse(value, out int index) && index >= 0 && index < sharedStrings.Count)
                        value = sharedStrings[index];
                    cells[column] = value;
                }

                if (cells.Count == 0) continue;
                string[] values = new string[cells.Keys.Max() + 1];
                foreach (var cell in cells) values[cell.Key] = cell.Value;
                rows.Add(values);
            }
            return rows;
        }

        private static void ThemDong(XElement sheetData, int soDong, IReadOnlyList<string> values)
        {
            XElement row = new XElement(SpreadsheetNs + "row", new XAttribute("r", soDong));
            for (int i = 0; i < values.Count; i++)
            {
                string value = values[i] ?? string.Empty;
                string address = LayTenCot(i + 1) + soDong.ToString(CultureInfo.InvariantCulture);
                if (soDong > 1 && i >= 5 && decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal number))
                    row.Add(new XElement(SpreadsheetNs + "c", new XAttribute("r", address),
                        new XElement(SpreadsheetNs + "v", number.ToString(CultureInfo.InvariantCulture))));
                else
                    row.Add(new XElement(SpreadsheetNs + "c", new XAttribute("r", address), new XAttribute("t", "inlineStr"),
                        new XElement(SpreadsheetNs + "is", new XElement(SpreadsheetNs + "t", new XAttribute(XNamespace.Xml + "space", "preserve"), value))));
            }
            sheetData.Add(row);
        }

        private static int LayChiSoCot(string diaChi)
        {
            int cot = 0;
            foreach (char kyTu in diaChi)
            {
                if (!char.IsLetter(kyTu)) break;
                cot = cot * 26 + (char.ToUpperInvariant(kyTu) - 'A' + 1);
            }
            return Math.Max(0, cot - 1);
        }

        private static string LayTenCot(int cot)
        {
            StringBuilder tenCot = new StringBuilder();
            while (cot > 0)
            {
                cot--;
                tenCot.Insert(0, (char)('A' + cot % 26));
                cot /= 26;
            }
            return tenCot.ToString();
        }

        private static XDocument DocXml(ZipArchive archive, string ten, bool batBuoc = true)
        {
            ZipArchiveEntry entry = archive.GetEntry(ten);
            if (entry == null)
            {
                if (!batBuoc) return null;
                throw new InvalidDataException($"File Excel thiếu thành phần '{ten}'.");
            }
            using Stream stream = entry.Open();
            return XDocument.Load(stream);
        }

        private static void GhiXml(ZipArchive archive, string ten, XDocument document)
        {
            ZipArchiveEntry entry = archive.CreateEntry(ten, CompressionLevel.Optimal);
            using Stream stream = entry.Open();
            document.Save(stream);
        }
    }
}
