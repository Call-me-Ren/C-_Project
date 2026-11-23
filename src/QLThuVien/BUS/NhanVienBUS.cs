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
    /// <summary>Lớp BUS kiểm tra nghiệp vụ nhân viên, quản lý bộ nhớ đệm và phối hợp với <see cref="NhanVienDAO"/>.</summary>
    public class NhanVienBUS
    {
        private static readonly Regex MaNhanVienPattern = new(@"^NV(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly string[] ChucVuMacDinh = { "Thủ thư chính", "Thủ thư", "Quản lý kho" };
        private readonly NhanVienDAO dao = new NhanVienDAO();
        /// <summary>Bộ nhớ đệm các nhân viên đã đọc để lớp GUI tra cứu và hiển thị.</summary>
        public ArrayList dsNhanVien_arr = new ArrayList();

        /// <summary>Danh sách chức vụ chuẩn được lớp BUS chấp nhận khi thêm nhân viên.</summary>
        public static IReadOnlyList<string> DanhSachChucVu => ChucVuMacDinh;

        /// <summary>Đọc danh sách nhân viên từ DAO và thay thế bộ nhớ đệm hiện tại.</summary>
        /// <returns>Danh sách nhân viên theo kết quả truy vấn; rỗng nếu chưa có nhân viên.</returns>
        /// <exception cref="SqlException">Lỗi truy vấn được truyền lên từ DAO.</exception>
        public List<NhanVienDTO> DocDS()
        {
            List<NhanVienDTO> list = dao.LayDanhSach();
            dsNhanVien_arr.Clear();
            foreach (NhanVienDTO nv in list) dsNhanVien_arr.Add(nv);
            return list;
        }

        /// <summary>Lấy danh sách nhân viên từ bộ nhớ đệm hoặc đọc lại dữ liệu theo yêu cầu.</summary>
        /// <param name="reload"><see langword="true"/> để đọc lại từ DAO; mặc định là <see langword="false"/>.</param>
        /// <returns>Bản sao danh sách nhân viên hiện có; có thể rỗng nếu chưa có dữ liệu.</returns>
        /// <exception cref="SqlException">Đọc dữ liệu thất bại khi bộ nhớ đệm cần nạp mới.</exception>
        public List<NhanVienDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsNhanVien_arr.Count == 0) return DocDS();
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            foreach (object item in dsNhanVien_arr)
                if (item is NhanVienDTO nv) result.Add(nv);
            return result;
        }

        /// <summary>Sinh mã nhân viên tiếp theo dựa trên các mã đang lưu trong cơ sở dữ liệu.</summary>
        /// <returns>Mã dạng NVnn với phần số tăng sau mã lớn nhất nhận diện được.</returns>
        /// <exception cref="SqlException">Không đọc được danh sách mã từ DAO.</exception>
        /// <exception cref="BusinessRuleException">Không thể tăng thêm mã khi đã đạt giới hạn số nguyên.</exception>
        public string TaoMaNhanVienTiepTheo()
        {
            return SinhMaNhanVienTiepTheo(dao.LayDanhSach().Select(nv => nv.MaNV));
        }

        /// <summary>Sinh mã NVnn kế tiếp từ tập mã hiện có mà không truy vấn cơ sở dữ liệu.</summary>
        /// <param name="maHienCo">Tập mã cần xét; các mã không đúng dạng NV theo sau bởi chữ số sẽ bị bỏ qua.</param>
        /// <returns>Mã có số lớn hơn giá trị NV hợp lệ lớn nhất một đơn vị, phần số được đệm tối thiểu hai chữ số.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="maHienCo"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Giá trị số lớn nhất đã đạt giới hạn số nguyên.</exception>
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

        /// <summary>Chuẩn hóa họ tên bằng cách gom khoảng trắng và viết hoa đầu mỗi từ theo tiếng Việt.</summary>
        /// <param name="value">Chuỗi họ tên đầu vào; chuỗi <see langword="null"/> được xem như rỗng.</param>
        /// <returns>Họ tên đã chuẩn hóa Unicode và kiểu chữ; rỗng nếu đầu vào không có ký tự.</returns>
        public static string ChuanHoaHoTen(string value)
        {
            string[] words = (value ?? string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            string normalized = string.Join(" ", words).Normalize(NormalizationForm.FormC);
            return CultureInfo.GetCultureInfo("vi-VN").TextInfo.ToTitleCase(normalized.ToLower(CultureInfo.GetCultureInfo("vi-VN")));
        }

        /// <summary>Kiểm tra độ dài, tính bắt buộc và tập ký tự được phép của một trường tên.</summary>
        /// <param name="value">Giá trị tên cần kiểm tra.</param>
        /// <param name="tenTruong">Nhãn trường dùng trong thông báo lỗi.</param>
        /// <param name="batBuoc"><see langword="true"/> nếu không chấp nhận giá trị rỗng sau chuẩn hóa.</param>
        /// <exception cref="BusinessRuleException">Tên rỗng khi bắt buộc, dài quá 50 ký tự hoặc có ký tự không hợp lệ.</exception>
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

        /// <summary>Cho biết một ký tự có thể xuất hiện trong họ tên theo quy tắc nhập liệu hay không.</summary>
        /// <param name="value">Ký tự cần xét.</param>
        /// <returns><see langword="true"/> nếu là khoảng trắng, chữ tiếng Việt hoặc dấu kết hợp tiếng Việt.</returns>
        public static bool KyTuTenHopLe(char value)
        {
            return char.IsWhiteSpace(value) || LaChuCaiTiengViet(value) || LaDauTiengViet(value);
        }

        /// <summary>Kiểm tra mã nhân viên có chưa được dùng trong bộ nhớ đệm hay không.</summary>
        /// <param name="maNV">Mã cần kiểm tra; rỗng hoặc chỉ có khoảng trắng được xem là không hợp lệ.</param>
        /// <returns><see langword="true"/> khi mã chưa xuất hiện trong bộ nhớ đệm; ngược lại là <see langword="false"/>.</returns>
        public bool KiemTraMaDuyNhat(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return false;
            string normalized = maNV.Trim();
            return !dsNhanVien_arr.Cast<object>().OfType<NhanVienDTO>()
                .Any(nv => string.Equals(nv.MaNV.Trim(), normalized, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Kiểm tra mã và các thông tin nhân viên trước khi thêm mới hoặc cập nhật.</summary>
        /// <param name="nv">Thông tin nhân viên cần kiểm tra.</param>
        /// <param name="isThemMoi"><see langword="true"/> để kiểm tra trùng mã theo quy tắc thêm mới.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nv"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Mã, họ tên hoặc chức vụ không đáp ứng quy tắc nghiệp vụ.</exception>
        /// <exception cref="SqlException">Không kiểm tra được mã nhân viên trong cơ sở dữ liệu.</exception>
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

        /// <summary>Chuẩn hóa, sinh mã và lưu một nhân viên mới qua DAO.</summary>
        /// <param name="nv">Thông tin nhân viên cần thêm; mã đầu vào được thay bằng mã tự sinh khi lưu thành công.</param>
        /// <returns><see langword="true"/> nếu DAO đã lưu và bộ nhớ đệm được cập nhật; <see langword="false"/> nếu DAO không thêm dòng.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nv"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Dữ liệu không hợp lệ hoặc không sinh được mã sau số lần thử giới hạn.</exception>
        /// <exception cref="SqlException">Lỗi lưu dữ liệu không thuộc trường hợp trùng khóa được xử lý để thử lại.</exception>
        /// <remarks>Mã được sinh lại tối đa 10 lần; nếu phát sinh trùng khóa do ghi đồng thời, danh sách được đọc mới trước lần thử tiếp theo.</remarks>
        public bool Them(NhanVienDTO nv)
        {
            if (nv == null) throw new ArgumentNullException(nameof(nv));
            NhanVienDTO normalized = ChuanHoaDuLieu(nv, string.Empty);
            KiemTraThongTin(normalized);

            // Đọc lại mã và thử lại khi giao dịch khác vừa chiếm cùng mã tự sinh.
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

        /// <summary>Kiểm tra và lưu thay đổi nhân viên, sau đó đồng bộ mục tương ứng trong bộ nhớ đệm.</summary>
        /// <param name="nv">Thông tin nhân viên cần cập nhật, bao gồm mã của bản ghi hiện có.</param>
        /// <returns><see langword="true"/> nếu DAO cập nhật được bản ghi; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nv"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Dữ liệu không hợp lệ hoặc không tìm thấy mã nhân viên.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra hoặc cập nhật dữ liệu được truyền từ DAO.</exception>
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

        /// <summary>Xóa nhân viên nếu mã tồn tại và chưa được dùng trong giao dịch.</summary>
        /// <param name="maNV">Mã nhân viên cần xóa.</param>
        /// <returns><see langword="true"/> nếu DAO xóa được bản ghi; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="BusinessRuleException">Mã rỗng hoặc nhân viên đang được phiếu nhập, phiếu mượn hay phiếu phạt tham chiếu.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra ràng buộc hoặc xóa dữ liệu được truyền từ DAO.</exception>
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

        /// <summary>Tìm nhân viên theo mã, họ tên hoặc chức vụ trong bộ nhớ đệm hiện tại.</summary>
        /// <param name="tuKhoa">Từ khóa tìm không phân biệt hoa thường; rỗng sẽ trả toàn bộ danh sách đang có.</param>
        /// <returns>Các nhân viên khớp từ khóa; rỗng nếu không tìm thấy kết quả.</returns>
        /// <exception cref="SqlException">Lỗi nạp danh sách khi bộ nhớ đệm chưa có dữ liệu.</exception>
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
