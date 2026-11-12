using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class QuyDinhPhatBUS
    {
        private static readonly Regex MaQuyDinhPattern = new(@"^QD(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly string[] LoaiPhatMacDinh = { "Phạt tiền", "Bồi thường" };
        private static readonly string[] CachTinhMacDinh = { "Theo ngày trễ", "Cố định", "100% Giá bìa sách" };
        private static readonly string[] DonViTinhMacDinh = { "Ngày", "Cuốn", "Hệ số" };
        private readonly QuyDinhPhatDAO dao = new QuyDinhPhatDAO();
        public ArrayList dsQuyDinh_arr = new ArrayList();

        public static IReadOnlyList<string> DanhSachLoaiPhat => LoaiPhatMacDinh;
        public static IReadOnlyList<string> DanhSachCachTinh => CachTinhMacDinh;
        public static IReadOnlyList<string> DanhSachDonViTinh => DonViTinhMacDinh;

        public List<QuyDinhPhatDTO> DocDS()
        {
            List<QuyDinhPhatDTO> list = dao.LayDanhSach();
            dsQuyDinh_arr.Clear();
            foreach (QuyDinhPhatDTO qd in list) dsQuyDinh_arr.Add(qd);
            return list;
        }

        public List<QuyDinhPhatDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsQuyDinh_arr.Count == 0) return DocDS();
            List<QuyDinhPhatDTO> result = new List<QuyDinhPhatDTO>();
            foreach (object item in dsQuyDinh_arr)
                if (item is QuyDinhPhatDTO qd) result.Add(qd);
            return result;
        }

        public string TaoMaQuyDinhTiepTheo()
        {
            return SinhMaQuyDinhTiepTheo(dao.LayDanhSach().Select(qd => qd.MaQD));
        }

        public static string SinhMaQuyDinhTiepTheo(IEnumerable<string> maHienCo)
        {
            int max = 0;
            foreach (string ma in maHienCo)
            {
                Match match = MaQuyDinhPattern.Match(ma ?? string.Empty);
                if (match.Success && int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out int so))
                    max = Math.Max(max, so);
            }
            if (max == int.MaxValue) throw new BusinessRuleException("Không thể tạo thêm mã quy định.");
            return "QD" + (max + 1).ToString("D2", CultureInfo.InvariantCulture);
        }

        public bool KiemTraMaDuyNhat(string maQD)
        {
            if (string.IsNullOrWhiteSpace(maQD)) return false;
            string normalized = maQD.Trim();
            return !dsQuyDinh_arr.Cast<object>().OfType<QuyDinhPhatDTO>()
                .Any(qd => Same(qd.MaQD, normalized));
        }

        public void KiemTraHopLe(QuyDinhPhatDTO qd, bool isThemMoi)
        {
            if (qd == null) throw new ArgumentNullException(nameof(qd));
            if (string.IsNullOrWhiteSpace(qd.MaQD)) throw new BusinessRuleException("Mã quy định không được để trống.");
            string ma = qd.MaQD.Trim();
            if (ma.Length > 20) throw new BusinessRuleException("Mã quy định không được vượt quá 20 ký tự.");
            if (isThemMoi && (!KiemTraMaDuyNhat(ma) || dao.KiemTraTonTai(ma)))
                throw new BusinessRuleException($"Mã quy định '{ma}' đã tồn tại.");
            KiemTraThongTin(qd, isThemMoi);
        }

        public bool Them(QuyDinhPhatDTO qd)
        {
            if (qd == null) throw new ArgumentNullException(nameof(qd));
            KiemTraThongTin(qd, isThemMoi: true);
            QuyDinhPhatDTO normalized = ChuanHoaDuLieu(qd, string.Empty);
            for (int attempt = 0; attempt < 10; attempt++)
            {
                string ma = TaoMaQuyDinhTiepTheo();
                if (dao.KiemTraTonTai(ma)) continue;
                normalized.MaQD = ma;
                try
                {
                    if (!dao.Them(normalized)) return false;
                    qd.MaQD = ma;
                    dsQuyDinh_arr.Add(normalized);
                    return true;
                }
                catch (SqlException ex) when (ex.Number is 2601 or 2627)
                {
                    DocDS();
                }
            }
            throw new BusinessRuleException("Không thể tạo mã quy định, vui lòng thử lại.");
        }

        public bool Sua(QuyDinhPhatDTO qd)
        {
            KiemTraHopLe(qd, isThemMoi: false);
            string ma = qd.MaQD.Trim();
            if (!dao.KiemTraTonTai(ma)) throw new BusinessRuleException($"Không tìm thấy quy định '{ma}'.");
            QuyDinhPhatDTO normalized = ChuanHoaDuLieu(qd, ma);
            bool success = dao.Sua(normalized);
            if (success)
                for (int i = 0; i < dsQuyDinh_arr.Count; i++)
                    if (dsQuyDinh_arr[i] is QuyDinhPhatDTO current && Same(current.MaQD, ma))
                    {
                        dsQuyDinh_arr[i] = normalized;
                        break;
                    }
            return success;
        }

        public bool Xoa(string maQD)
        {
            if (string.IsNullOrWhiteSpace(maQD)) throw new BusinessRuleException("Hãy chọn quy định phạt cần xóa.");
            string ma = maQD.Trim();
            if (dao.KiemTraKhoaNgoai(ma)) throw new BusinessRuleException("Quy định phạt đang được dùng, không thể xóa.");
            bool success = dao.Xoa(ma);
            if (success)
                for (int i = 0; i < dsQuyDinh_arr.Count; i++)
                    if (dsQuyDinh_arr[i] is QuyDinhPhatDTO current && Same(current.MaQD, ma))
                    {
                        dsQuyDinh_arr.RemoveAt(i);
                        break;
                    }
            return success;
        }

        public List<QuyDinhPhatDTO> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa)) return LayDanhSach();
            string key = tuKhoa.Trim();
            List<QuyDinhPhatDTO> result = new List<QuyDinhPhatDTO>();
            foreach (object item in dsQuyDinh_arr)
            {
                if (item is not QuyDinhPhatDTO qd) continue;
                if (qd.MaQD.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                    qd.TenQD.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                    (qd.LoaiPhat?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (qd.CachTinh?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (qd.DonViTinh?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    qd.MucPhat.ToString(CultureInfo.CurrentCulture).Contains(key, StringComparison.OrdinalIgnoreCase)) result.Add(qd);
            }
            return result;
        }

        private void KiemTraThongTin(QuyDinhPhatDTO qd, bool isThemMoi)
        {
            string ten = qd.TenQD?.Trim() ?? string.Empty;
            if (ten.Length == 0) throw new BusinessRuleException("Tên quy định không được để trống.");
            if (ten.Length > 100) throw new BusinessRuleException("Tên quy định không được vượt quá 100 ký tự.");
            if (qd.MucPhat < 0) throw new BusinessRuleException("Mức phạt phải từ 0 trở lên.");
            KiemTraLuaChon(qd.LoaiPhat, LoaiPhatMacDinh, "Loại phạt", qd.MaQD, isThemMoi);
            KiemTraLuaChon(qd.CachTinh, CachTinhMacDinh, "Cách tính", qd.MaQD, isThemMoi);
            KiemTraLuaChon(qd.DonViTinh, DonViTinhMacDinh, "Đơn vị tính", qd.MaQD, isThemMoi);
            if (Same(qd.CachTinh, "100% Giá bìa sách") &&
                (!Same(qd.LoaiPhat, "Bồi thường") || !Same(qd.DonViTinh, "Hệ số") || qd.MucPhat != 1.00m))
                throw new BusinessRuleException("Quy định bồi thường theo giá bìa dùng hệ số 1,00.");
            if (Same(qd.DonViTinh, "Hệ số") && !Same(qd.CachTinh, "100% Giá bìa sách"))
                throw new BusinessRuleException("Đơn vị hệ số chỉ dùng cho cách tính theo giá bìa.");
        }

        private void KiemTraLuaChon(string? value, IReadOnlyCollection<string> options, string tenTruong, string maQD, bool isThemMoi)
        {
            string selected = value?.Trim() ?? string.Empty;
            if (selected.Length == 0) throw new BusinessRuleException($"{tenTruong} không được để trống.");
            if (options.Contains(selected, StringComparer.OrdinalIgnoreCase)) return;
            if (!isThemMoi)
            {
                QuyDinhPhatDTO? current = dao.LayTheoMa(maQD.Trim());
                string? oldValue = tenTruong switch
                {
                    "Loại phạt" => current?.LoaiPhat,
                    "Cách tính" => current?.CachTinh,
                    _ => current?.DonViTinh
                };
                if (Same(oldValue, selected)) return;
            }
            throw new BusinessRuleException($"Hãy chọn {tenTruong.ToLowerInvariant()} trong danh sách.");
        }

        private static QuyDinhPhatDTO ChuanHoaDuLieu(QuyDinhPhatDTO qd, string ma)
        {
            return new QuyDinhPhatDTO(ma, qd.TenQD.Trim(), qd.LoaiPhat?.Trim(), qd.CachTinh?.Trim(), qd.DonViTinh?.Trim(), qd.MucPhat);
        }

        private static bool Same(string? left, string? right)
        {
            return string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
