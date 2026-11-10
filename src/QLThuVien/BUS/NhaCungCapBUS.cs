using System;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho bảng NhaCungCap
    /// Tuân thủ quy tắc giáo viên: Lưu dữ liệu vào List/ArrayList, tìm kiếm trên List, xử lý hợp lệ/trùng mã tại đây
    /// </summary>
    public class NhaCungCapBUS
    {
        // Danh sách cache trong bộ nhớ theo yêu cầu của Giáo viên (Quy tắc 4)
        private static List<NhaCungCapDTO> _listNCC = new List<NhaCungCapDTO>();

        /// <summary>
        /// Lấy toàn bộ danh sách Nhà cung cấp (nếu đã có trong bộ nhớ thì lấy ra, hoặc nạp mới từ DAO)
        /// </summary>
        public static List<NhaCungCapDTO> GetAll(bool forceReload = false)
        {
            if (_listNCC == null || _listNCC.Count == 0 || forceReload)
            {
                _listNCC = NhaCungCapDAO.GetAll();
            }
            return new List<NhaCungCapDTO>(_listNCC);
        }

        /// <summary>
        /// Kiểm tra tính duy nhất của mã (Yêu cầu số 5 của Giáo viên)
        /// </summary>
        public static bool KiemTraTrungMa(string maNCC)
        {
            GetAll(); // Đảm bảo danh sách đã được nạp
            return _listNCC.Any(x => x.MaNCC.Equals(maNCC.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Thêm mới Nhà cung cấp với đầy đủ kiểm tra nghiệp vụ
        /// </summary>
        public static (bool Success, string Message) Them(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
                return (false, "Mã nhà cung cấp không được để trống!");

            if (ncc.MaNCC.Trim().Length > 20)
                return (false, "Mã nhà cung cấp không được vượt quá 20 ký tự!");

            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                return (false, "Tên nhà cung cấp không được để trống!");

            ncc.MaNCC = ncc.MaNCC.Trim().ToUpper();
            ncc.TenNCC = ncc.TenNCC.Trim();

            // Kiểm tra trùng mã
            if (KiemTraTrungMa(ncc.MaNCC))
                return (false, $"Mã nhà cung cấp '{ncc.MaNCC}' đã tồn tại!");

            // Gọi DAO thực thi lệnh INSERT thật xuống CSDL
            if (NhaCungCapDAO.Insert(ncc))
            {
                _listNCC.Add(ncc);
                return (true, "Thêm nhà cung cấp thành công!");
            }

            return (false, "Lỗi khi lưu vào cơ sở dữ liệu!");
        }

        /// <summary>
        /// Cập nhật thông tin Nhà cung cấp
        /// </summary>
        public static (bool Success, string Message) Sua(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                return (false, "Tên nhà cung cấp không được để trống!");

            ncc.TenNCC = ncc.TenNCC.Trim();

            if (NhaCungCapDAO.Update(ncc))
            {
                var item = _listNCC.FirstOrDefault(x => x.MaNCC.Equals(ncc.MaNCC, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                {
                    item.TenNCC = ncc.TenNCC;
                }
                return (true, "Cập nhật nhà cung cấp thành công!");
            }

            return (false, "Lỗi khi cập nhật cơ sở dữ liệu!");
        }

        /// <summary>
        /// Xóa Nhà cung cấp, kiểm tra ràng buộc không được xóa dữ liệu đã phát sinh giao dịch
        /// </summary>
        public static (bool Success, string Message) Xoa(string maNCC)
        {
            maNCC = maNCC.Trim();

            // Kiểm tra khóa ngoại xem có phiếu nhập nào tham chiếu không (Quy tắc "Không làm lủng CSDL")
            if (NhaCungCapDAO.HasPhieuNhap(maNCC))
            {
                return (false, "Không thể xóa nhà cung cấp này vì đã có Phiếu nhập liên kết trong hệ thống!");
            }

            if (NhaCungCapDAO.Delete(maNCC))
            {
                _listNCC.RemoveAll(x => x.MaNCC.Equals(maNCC, StringComparison.OrdinalIgnoreCase));
                return (true, "Xóa nhà cung cấp thành công!");
            }

            return (false, "Lỗi khi xóa khỏi cơ sở dữ liệu!");
        }

        /// <summary>
        /// Tìm kiếm lọc trên bộ nhớ List/ArrayList ở tầng BUS (Yêu cầu số 2 của Giáo viên)
        /// Không gọi câu lệnh SELECT liên tục xuống CSDL
        /// </summary>
        public static List<NhaCungCapDTO> TimKiem(string keyword)
        {
            GetAll();
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<NhaCungCapDTO>(_listNCC);

            keyword = keyword.Trim().ToLower();
            return _listNCC.Where(x => 
                x.MaNCC.ToLower().Contains(keyword) || 
                x.TenNCC.ToLower().Contains(keyword)
            ).ToList();
        }
    }
}
