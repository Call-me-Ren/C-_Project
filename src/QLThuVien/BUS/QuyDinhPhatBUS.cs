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
    /// <summary>Lớp BUS quản lý quy định phạt, xác thực cấu hình và phối hợp với <see cref="QuyDinhPhatDAO"/>.</summary>
    public class QuyDinhPhatBUS
    {
        private static readonly Regex MaQuyDinhPattern = new(@"^QD(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly string[] LoaiPhatMacDinh = { "Phạt tiền", "Bồi thường" };
        private static readonly string[] CachTinhMacDinh = { "Theo ngày trễ", "Cố định", "100% Giá bìa sách" };
        private static readonly string[] DonViTinhMacDinh = { "Ngày", "Cuốn", "Hệ số" };
        private readonly QuyDinhPhatDAO dao = new QuyDinhPhatDAO();
        /// <summary>Bộ nhớ đệm các quy định phạt đã đọc để lớp GUI tra cứu và hiển thị.</summary>
        public ArrayList dsQuyDinh_arr = new ArrayList();

        /// <summary>Các loại phạt được dùng khi tạo hoặc sửa quy định.</summary>
        public static IReadOnlyList<string> DanhSachLoaiPhat => LoaiPhatMacDinh;
        /// <summary>Các cách tính được dùng khi tạo hoặc sửa quy định.</summary>
        public static IReadOnlyList<string> DanhSachCachTinh => CachTinhMacDinh;
        /// <summary>Các đơn vị tính được dùng khi tạo hoặc sửa quy định.</summary>
        public static IReadOnlyList<string> DanhSachDonViTinh => DonViTinhMacDinh;

        /// <summary>Đọc quy định phạt từ DAO và thay thế bộ nhớ đệm hiện tại.</summary>
        /// <returns>Danh sách quy định theo mã; rỗng nếu chưa có quy định.</returns>
        /// <exception cref="SqlException">Lỗi truy vấn được truyền lên từ DAO.</exception>
        public List<QuyDinhPhatDTO> DocDS()
        {
            List<QuyDinhPhatDTO> list = dao.LayDanhSach();
            dsQuyDinh_arr.Clear();
            foreach (QuyDinhPhatDTO qd in list) dsQuyDinh_arr.Add(qd);
            return list;
        }

        /// <summary>Lấy danh sách quy định từ bộ nhớ đệm hoặc đọc lại theo yêu cầu.</summary>
        /// <param name="reload"><see langword="true"/> để đọc lại từ DAO; mặc định là <see langword="false"/>.</param>
        /// <returns>Bản sao danh sách quy định hiện có; có thể rỗng.</returns>
        /// <exception cref="SqlException">Đọc dữ liệu thất bại khi bộ nhớ đệm cần nạp mới.</exception>
        public List<QuyDinhPhatDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsQuyDinh_arr.Count == 0) return DocDS();
            List<QuyDinhPhatDTO> result = new List<QuyDinhPhatDTO>();
            foreach (object item in dsQuyDinh_arr)
                if (item is QuyDinhPhatDTO qd) result.Add(qd);
            return result;
        }

        /// <summary>Sinh mã quy định tiếp theo dựa trên mã đang lưu trong cơ sở dữ liệu.</summary>
        /// <returns>Mã dạng QDnn với phần số lớn hơn mã hợp lệ lớn nhất một đơn vị.</returns>
        /// <exception cref="SqlException">Không đọc được danh sách mã từ DAO.</exception>
        /// <exception cref="BusinessRuleException">Không thể tăng thêm mã khi đã đạt giới hạn số nguyên.</exception>
        public string TaoMaQuyDinhTiepTheo()
        {
            return SinhMaQuyDinhTiepTheo(dao.LayDanhSach().Select(qd => qd.MaQD));
        }

        /// <summary>Sinh mã QDnn kế tiếp từ tập mã hiện có mà không truy vấn cơ sở dữ liệu.</summary>
        /// <param name="maHienCo">Tập mã cần xét; chỉ mã dạng QD theo sau bởi chữ số được dùng.</param>
        /// <returns>Mã có số lớn hơn giá trị QD hợp lệ lớn nhất một đơn vị, đệm tối thiểu hai chữ số.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="maHienCo"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Giá trị số lớn nhất đã đạt giới hạn số nguyên.</exception>
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

        /// <summary>Kiểm tra mã quy định chưa xuất hiện trong bộ nhớ đệm.</summary>
        /// <param name="maQD">Mã cần kiểm tra; rỗng hoặc chỉ có khoảng trắng luôn trả về <see langword="false"/>.</param>
        /// <returns><see langword="true"/> nếu mã chưa được dùng; ngược lại là <see langword="false"/>.</returns>
        public bool KiemTraMaDuyNhat(string maQD)
        {
            if (string.IsNullOrWhiteSpace(maQD)) return false;
            string normalized = maQD.Trim();
            return !dsQuyDinh_arr.Cast<object>().OfType<QuyDinhPhatDTO>()
                .Any(qd => Same(qd.MaQD, normalized));
        }

        /// <summary>Kiểm tra mã và cấu hình quy định trước khi thêm mới hoặc cập nhật.</summary>
        /// <param name="qd">Quy định cần kiểm tra.</param>
        /// <param name="isThemMoi"><see langword="true"/> để áp dụng kiểm tra trùng mã khi thêm mới.</param>
        /// <exception cref="ArgumentNullException"><paramref name="qd"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Mã hoặc cấu hình không hợp lệ, mức phạt âm, hoặc cách tính không khớp loại/đơn vị.</exception>
        /// <exception cref="SqlException">Không kiểm tra được mã quy định trong cơ sở dữ liệu.</exception>
        /// <remarks>Quy định tính theo 100% giá bìa bắt buộc dùng loại bồi thường, đơn vị hệ số và mức 1,00. Mức phạt không được âm; quy tắc không đặt trần nhỏ hơn giới hạn kiểu dữ liệu/cơ sở dữ liệu.</remarks>
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

        /// <summary>Kiểm tra, chuẩn hóa mã và lưu quy định phạt mới.</summary>
        /// <param name="qd">Thông tin quy định cần thêm; mã được thay bằng mã tự sinh khi thành công.</param>
        /// <returns><see langword="true"/> nếu lưu thành công; <see langword="false"/> nếu DAO không thêm dòng.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qd"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Dữ liệu không hợp lệ hoặc không sinh được mã sau số lần thử giới hạn.</exception>
        /// <exception cref="SqlException">Lỗi lưu dữ liệu không thuộc trường hợp trùng khóa được xử lý để thử lại.</exception>
        /// <remarks>Mã được sinh lại tối đa 10 lần; khi gặp trùng khóa do thêm đồng thời, bộ nhớ đệm được đọc mới trước lần thử sau.</remarks>
        public bool Them(QuyDinhPhatDTO qd)
        {
            if (qd == null) throw new ArgumentNullException(nameof(qd));
            KiemTraThongTin(qd, isThemMoi: true);
            QuyDinhPhatDTO normalized = ChuanHoaDuLieu(qd, string.Empty);
            // Làm mới danh sách trước lần thử tiếp theo để tránh lặp mã vừa được giao dịch khác ghi.
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

        /// <summary>Kiểm tra và cập nhật cấu hình của quy định phạt hiện có.</summary>
        /// <param name="qd">Thông tin quy định cần sửa, kèm mã bản ghi đã tồn tại.</param>
        /// <returns><see langword="true"/> nếu DAO cập nhật thành công; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qd"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Dữ liệu không hợp lệ hoặc không tìm thấy mã quy định.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra hoặc cập nhật dữ liệu được truyền từ DAO.</exception>
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

        /// <summary>Xóa quy định nếu chưa được dùng trong chi tiết phiếu phạt.</summary>
        /// <param name="maQD">Mã quy định cần xóa.</param>
        /// <returns><see langword="true"/> nếu DAO xóa được bản ghi; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="BusinessRuleException">Mã rỗng hoặc quy định đang được tham chiếu bởi vi phạm.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra ràng buộc hoặc xóa dữ liệu được truyền từ DAO.</exception>
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

        /// <summary>Tìm quy định theo mã, tên, loại, cách tính, đơn vị hoặc mức phạt.</summary>
        /// <param name="tuKhoa">Từ khóa không phân biệt hoa thường; rỗng sẽ trả danh sách đang có.</param>
        /// <returns>Các quy định khớp từ khóa; rỗng nếu không tìm thấy kết quả.</returns>
        /// <exception cref="SqlException">Lỗi nạp danh sách khi bộ nhớ đệm chưa có dữ liệu.</exception>
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
