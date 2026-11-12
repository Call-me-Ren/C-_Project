using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho bảng NhaCungCap
    /// Tuân thủ quy tắc giáo viên: Lưu dữ liệu vào ArrayList, tìm kiếm trên ArrayList, xử lý hợp lệ/trùng mã tại đây
    /// </summary>
    public class NhaCungCapBUS
    {
        // 1. Khai báo ArrayList lưu trữ dữ liệu theo yêu cầu bài giảng của GV (Quy tắc 4)
        public static ArrayList dsNCC_arr = new ArrayList();

        /// <summary>
        /// Nạp toàn bộ danh sách Nhà cung cấp từ DAO vào ArrayList
        /// </summary>
        public static List<NhaCungCapDTO> GetAll(bool forceReload = false)
        {
            if (dsNCC_arr.Count == 0 || forceReload)
            {
                dsNCC_arr.Clear();
                List<NhaCungCapDTO> list = NhaCungCapDAO.GetAll();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        dsNCC_arr.Add(item);
                    }
                }
            }

            // Trả về List để gán DataSource cho DataGridView hiển thị
            List<NhaCungCapDTO> result = new List<NhaCungCapDTO>();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Lấy tên nhà cung cấp từ danh sách đã nạp ở BUS theo mã.
        /// </summary>
        public static string LayTenTheoMa(string maNCC)
        {
            GetAll();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                if (item.MaNCC.Equals(maNCC, StringComparison.OrdinalIgnoreCase))
                    return item.TenNCC;
            }

            return maNCC;
        }

        /// <summary>
        /// Kiểm tra tính duy nhất của mã trên ArrayList (Yêu cầu số 5 của Giáo viên)
        /// </summary>
        public static bool KiemTraTrungMa(string maNCC)
        {
            GetAll(); // Đảm bảo dsNCC_arr đã được nạp
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                if (item.MaNCC.Equals(maNCC.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Thêm mới Nhà cung cấp với đầy đủ kiểm tra nghiệp vụ và độ dài trường
        /// </summary>
        public static (bool Success, string Message) Them(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
                return (false, "Mã nhà cung cấp không được để trống!");

            if (ncc.MaNCC.Trim().Length > 20)
                return (false, "Mã nhà cung cấp không được vượt quá 20 ký tự!");

            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                return (false, "Tên nhà cung cấp không được để trống!");

            // Kiểm tra độ dài TenNCC <= 100 theo đúng CSDL NVARCHAR(100)
            if (ncc.TenNCC.Trim().Length > 100)
                return (false, "Tên nhà cung cấp không được vượt quá 100 ký tự!");

            ncc.MaNCC = ncc.MaNCC.Trim().ToUpper();
            ncc.TenNCC = ncc.TenNCC.Trim();

            // Kiểm tra trùng mã
            if (KiemTraTrungMa(ncc.MaNCC))
                return (false, $"Mã nhà cung cấp '{ncc.MaNCC}' đã tồn tại!");

            // Gọi DAO thực thi lệnh INSERT thật xuống CSDL
            if (NhaCungCapDAO.Insert(ncc))
            {
                dsNCC_arr.Add(ncc); // Đồng bộ vào ArrayList
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

            // Kiểm tra độ dài TenNCC <= 100 theo đúng CSDL NVARCHAR(100)
            if (ncc.TenNCC.Trim().Length > 100)
                return (false, "Tên nhà cung cấp không được vượt quá 100 ký tự!");

            ncc.TenNCC = ncc.TenNCC.Trim();

            if (NhaCungCapDAO.Update(ncc))
            {
                // Đồng bộ cập nhật trên ArrayList
                foreach (NhaCungCapDTO item in dsNCC_arr)
                {
                    if (item.MaNCC.Equals(ncc.MaNCC, StringComparison.OrdinalIgnoreCase))
                    {
                        item.TenNCC = ncc.TenNCC;
                        break;
                    }
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
                // Đồng bộ xóa trên ArrayList
                for (int i = dsNCC_arr.Count - 1; i >= 0; i--)
                {
                    if (dsNCC_arr[i] is NhaCungCapDTO item && item.MaNCC.Equals(maNCC, StringComparison.OrdinalIgnoreCase))
                    {
                        dsNCC_arr.RemoveAt(i);
                        break;
                    }
                }
                return (true, "Xóa nhà cung cấp thành công!");
            }

            return (false, "Lỗi khi xóa khỏi cơ sở dữ liệu!");
        }

        /// <summary>
        /// Tìm kiếm lọc trên bộ nhớ ArrayList ở tầng BUS (Yêu cầu số 2 của Giáo viên)
        /// Không gọi câu lệnh SELECT liên tục xuống CSDL
        /// </summary>
        public static List<NhaCungCapDTO> TimKiem(string keyword)
        {
            GetAll();
            List<NhaCungCapDTO> ketQua = new List<NhaCungCapDTO>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                foreach (NhaCungCapDTO item in dsNCC_arr)
                {
                    ketQua.Add(item);
                }
                return ketQua;
            }

            keyword = keyword.Trim().ToLower();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                if (item.MaNCC.ToLower().Contains(keyword) || item.TenNCC.ToLower().Contains(keyword))
                {
                    ketQua.Add(item);
                }
            }

            return ketQua;
        }
    }
}
