using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class NhanVienBUS
    {
        private static readonly Regex MaNhanVienPattern = new(@"^NV(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly string[] ChucVuMacDinh = { "Thủ thư chính", "Thủ thư", "Quản lý kho" };
        private readonly NhanVienDAO dao = new NhanVienDAO();
        public ArrayList dsNhanVien_arr = new ArrayList();

        public static IReadOnlyList<string> DanhSachChucVu => ChucVuMacDinh;

        public List<NhanVienDTO> DocDS()
        {
            List<NhanVienDTO> list = dao.LayDanhSach();
            dsNhanVien_arr.Clear();
            foreach (NhanVienDTO nv in list) dsNhanVien_arr.Add(nv);
            return list;
        }

        public List<NhanVienDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsNhanVien_arr.Count == 0) return DocDS();
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            foreach (object item in dsNhanVien_arr)
                if (item is NhanVienDTO nv) result.Add(nv);
            return result;
        }

        public string TaoMaNhanVienTiepTheo()
        {
            return SinhMaNhanVienTiepTheo(dao.LayDanhSach().Select(nv => nv.MaNV));
        }

        public static string SinhMaNhanVienTiepTheo(IEnumerable<string> maHienCo)
        {
            int max = 0;
            foreach (string ma in maHienCo)
            {
                Match match = MaNhanVienPattern.Match(ma ?? string.Empty);
                if (match.Success && int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out int so))
                    max = Math.Max(max, so);
            }
            if (max == int.MaxValue) throw new BusinessRuleException("Không thể tạo thêm mã nhân viên.");
            return "NV" + (max + 1).ToString("D2", CultureInfo.InvariantCulture);
        }

        public static string ChuanHoaHoTen(string value)
        {
            string[] words = (value ?? string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            string normalized = string.Join(" ", words).Normalize(NormalizationForm.FormC);
            return CultureInfo.GetCultureInfo("vi-VN").TextInfo.ToTitleCase(normalized.ToLower(CultureInfo.GetCultureInfo("vi-VN")));
        }

        public static void KiemTraTen(string value, string tenTruong, bool batBuoc)
        {
            string normalized = ChuanHoaHoTen(value);
            if (batBuoc && normalized.Length == 0)
                throw new BusinessRuleException($"{tenTruong} không được để trống.");
            if (normalized.Length > 50)
                throw new BusinessRuleException($"{tenTruong} không được vượt quá 50 ký tự.");
            if (normalized.Length > 0 && !TenHopLe(normalized))
                throw new BusinessRuleException($"{tenTruong} chỉ gồm chữ cái và khoảng trắng.");
        }

        public static bool KyTuTenHopLe(char value)
        {
            return char.IsWhiteSpace(value) || LaChuCaiTiengViet(value) || LaDauTiengViet(value);
        }

        public bool KiemTraMaDuyNhat(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return false;
            string normalized = maNV.Trim();
            return !dsNhanVien_arr.Cast<object>().OfType<NhanVienDTO>()
                .Any(nv => string.Equals(nv.MaNV.Trim(), normalized, StringComparison.OrdinalIgnoreCase));
        }

        public void KiemTraHopLe(NhanVienDTO nv, bool isThemMoi)
        {
            if (nv == null) throw new ArgumentNullException(nameof(nv));
            KiemTraThongTin(nv);
            if (string.IsNullOrWhiteSpace(nv.MaNV)) throw new BusinessRuleException("Mã nhân viên không được để trống.");
            string ma = nv.MaNV.Trim();
            if (ma.Length > 20) throw new BusinessRuleException("Mã nhân viên không được vượt quá 20 ký tự.");
            if (isThemMoi && (!KiemTraMaDuyNhat(ma) || dao.KiemTraTonTai(ma)))
                throw new BusinessRuleException($"Mã nhân viên '{ma}' đã tồn tại.");
        }

        public bool Them(NhanVienDTO nv)
        {
            if (nv == null) throw new ArgumentNullException(nameof(nv));
            NhanVienDTO normalized = ChuanHoaDuLieu(nv, string.Empty);
            KiemTraThongTin(normalized);

            for (int attempt = 0; attempt < 10; attempt++)
            {
                string ma = TaoMaNhanVienTiepTheo();
                if (dao.KiemTraTonTai(ma)) continue;
                normalized.MaNV = ma;
                try
                {
                    if (!dao.Them(normalized)) return false;
                    nv.MaNV = ma;
                    dsNhanVien_arr.Add(normalized);
                    return true;
                }
                catch (SqlException ex) when (ex.Number is 2601 or 2627)
                {
                    DocDS();
                }
            }
            throw new BusinessRuleException("Không thể tạo mã nhân viên, vui lòng thử lại.");
        }

        public bool Sua(NhanVienDTO nv)
        {
            KiemTraHopLe(nv, isThemMoi: false);
            string ma = nv.MaNV.Trim();
            if (!dao.KiemTraTonTai(ma)) throw new BusinessRuleException($"Không tìm thấy nhân viên '{ma}'.");
            NhanVienDTO normalized = ChuanHoaDuLieu(nv, ma);
            KiemTraChucVu(normalized, isThemMoi: false);
            bool success = dao.Sua(normalized);
            if (success)
            {
                for (int i = 0; i < dsNhanVien_arr.Count; i++)
                {
                    if (dsNhanVien_arr[i] is NhanVienDTO current && Same(current.MaNV, ma))
                    {
                        dsNhanVien_arr[i] = normalized;
                        break;
                    }
                }
            }
            return success;
        }

        public bool Xoa(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) throw new BusinessRuleException("Hãy chọn nhân viên cần xóa.");
            string ma = maNV.Trim();
            if (dao.KiemTraKhoaNgoai(ma)) throw new BusinessRuleException("Nhân viên đang có giao dịch, không thể xóa.");
            bool success = dao.Xoa(ma);
            if (success)
                for (int i = 0; i < dsNhanVien_arr.Count; i++)
                    if (dsNhanVien_arr[i] is NhanVienDTO current && Same(current.MaNV, ma))
                    {
                        dsNhanVien_arr.RemoveAt(i);
                        break;
                    }
            return success;
        }

        public List<NhanVienDTO> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa)) return LayDanhSach();
            string key = tuKhoa.Trim();
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            foreach (object item in dsNhanVien_arr)
            {
                if (item is not NhanVienDTO nv) continue;
                if (nv.MaNV.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                    nv.TenNV.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                    (nv.HoLot?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    nv.HoTen.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                    (nv.ChucVu?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false)) result.Add(nv);
            }
            return result;
        }

        private static void KiemTraThongTin(NhanVienDTO nv)
        {
            string hoLot = ChuanHoaHoTen(nv.HoLot ?? string.Empty);
            string ten = ChuanHoaHoTen(nv.TenNV);
            KiemTraTen(hoLot, "Họ lót", batBuoc: false);
            KiemTraTen(ten, "Tên nhân viên", batBuoc: true);
            KiemTraChucVu(nv, isThemMoi: string.IsNullOrWhiteSpace(nv.MaNV));
        }

        private static void KiemTraChucVu(NhanVienDTO nv, bool isThemMoi)
        {
            string chucVu = nv.ChucVu?.Trim() ?? string.Empty;
            if (chucVu.Length == 0) throw new BusinessRuleException("Chức vụ không được để trống.");
            if (chucVu.Length > 50) throw new BusinessRuleException("Chức vụ không được vượt quá 50 ký tự.");
            if (ChucVuMacDinh.Contains(chucVu, StringComparer.OrdinalIgnoreCase)) return;
            if (!isThemMoi)
            {
                string? chucVuHienTai = new NhanVienDAO().LayTheoMa(nv.MaNV.Trim())?.ChucVu?.Trim();
                if (string.Equals(chucVuHienTai, chucVu, StringComparison.OrdinalIgnoreCase)) return;
            }
            throw new BusinessRuleException("Hãy chọn chức vụ trong danh sách.");
        }

        private static NhanVienDTO ChuanHoaDuLieu(NhanVienDTO nv, string ma)
        {
            string hoLot = ChuanHoaHoTen(nv.HoLot ?? string.Empty);
            string ten = ChuanHoaHoTen(nv.TenNV);
            return new NhanVienDTO(ma, hoLot.Length == 0 ? null : hoLot, ten, nv.ChucVu?.Trim());
        }

        private static bool TenHopLe(string value)
        {
            bool truocLaChu = false;
            foreach (char c in value)
            {
                if (c == ' ')
                {
                    if (!truocLaChu) return false;
                    truocLaChu = false;
                    continue;
                }
                if (LaChuCaiTiengViet(c))
                {
                    truocLaChu = true;
                    continue;
                }
                if (LaDauTiengViet(c) && truocLaChu) continue;
                return false;
            }
            return truocLaChu;
        }

        private static bool LaChuCaiTiengViet(char value)
        {
            if (!char.IsLetter(value)) return false;
            return value is >= 'A' and <= 'Z' or >= 'a' and <= 'z' ||
                   value is >= '\u00C0' and <= '\u024F' or >= '\u1E00' and <= '\u1EFF';
        }

        private static bool LaDauTiengViet(char value)
        {
            UnicodeCategory category = char.GetUnicodeCategory(value);
            return category is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark
                && value is >= '\u0300' and <= '\u036F';
        }

        private static bool Same(string? left, string? right)
        {
            return string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
