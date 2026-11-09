using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Lớp BUS xử lý nghiệp vụ cho Loại Độc Giả.
    /// Áp dụng nguyên lý DRY: Tái sử dụng hàm TimTheoMa cho Kiểm tra trùng, Sửa và Xóa trên ArrayList.
    /// </summary>
    public class LoaiDocGiaBUS
    {
        private LoaiDocGiaDAO ldgDAO = new LoaiDocGiaDAO();

        // Thuộc tính ArrayList lưu danh sách trong RAM (đúng chuẩn slide bài giảng cô Trang)
        public ArrayList dsldg_arr = new ArrayList();

        // 1. ĐỌC DANH SÁCH TỪ CSDL VÀ LƯU VÀO ARRAYLIST
        public ArrayList DocDSLoaiDocGia()
        {
            List<LoaiDocGiaDTO> list = ldgDAO.DocDSLoaiDocGia();

            dsldg_arr.Clear();
            foreach (LoaiDocGiaDTO ldg in list)
            {
                dsldg_arr.Add(ldg);
            }

            return dsldg_arr;
        }

        // HÀM DÙNG CHUNG CỐT LÕI: Tìm đối tượng theo mã trong ArrayList
        // Trả về đối tượng LoaiDocGiaDTO nếu tìm thấy, hoặc null nếu không tồn tại
        public LoaiDocGiaDTO? TimTheoMa(string maLDG)
        {
            foreach (LoaiDocGiaDTO item in dsldg_arr)
            {
                if (item.MaLDG.Trim().Equals(maLDG.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        // 2. KIỂM TRA MÃ DUY NHẤT (Tái sử dụng TimTheoMa - chỉ 1 dòng code)
        public bool KiemTraMaDuyNhat(string maLDG)
        {
            // Nếu không tìm thấy (null) nghĩa là chưa tồn tại -> Hợp lệ (true)
            return TimTheoMa(maLDG) == null;
        }

        // 3. NGHIỆP VỤ THÊM LOẠI ĐỘC GIẢ
        public bool Them(LoaiDocGiaDTO ldg, out string error)
        {
            error = string.Empty;

            // Kiểm tra tính hợp lệ dữ liệu
            if (string.IsNullOrWhiteSpace(ldg.MaLDG))
            {
                error = "Mã loại độc giả không được để trống!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(ldg.TenLDG))
            {
                error = "Tên loại độc giả không được để trống!";
                return false;
            }

            // Kiểm tra trùng mã (gọi hàm tái sử dụng)
            if (!KiemTraMaDuyNhat(ldg.MaLDG))
            {
                error = $"Mã loại độc giả '{ldg.MaLDG}' đã tồn tại trong hệ thống!";
                return false;
            }

            // Gọi DAO ghi xuống CSDL
            bool result = ldgDAO.ThemLoaiDocGia(ldg);

            // Nếu thêm thành công vào CSDL, cập nhật ngay vào ArrayList
            if (result)
            {
                dsldg_arr.Add(ldg);
            }
            else
            {
                error = "Thêm vào cơ sở dữ liệu thất bại!";
            }

            return result;
        }

        // 4. NGHIỆP VỤ SỬA LOẠI ĐỘC GIẢ (Tái sử dụng TimTheoMa)
        public bool Sua(LoaiDocGiaDTO ldg, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(ldg.TenLDG))
            {
                error = "Tên loại độc giả không được để trống!";
                return false;
            }

            // Gọi DAO cập nhật xuống CSDL
            bool result = ldgDAO.SuaLoaiDocGia(ldg);

            // Nếu CSDL cập nhật thành công, tìm đối tượng trong ArrayList và đổi tên
            if (result)
            {
                LoaiDocGiaDTO? item = TimTheoMa(ldg.MaLDG);
                if (item != null)
                {
                    item.TenLDG = ldg.TenLDG;
                }
            }
            else
            {
                error = "Cập nhật dữ liệu thất bại!";
            }

            return result;
        }

        // 5. NGHIỆP VỤ XÓA LOẠI ĐỘC GIẢ (Tái sử dụng TimTheoMa)
        public bool Xoa(string maLDG, out string error)
        {
            error = string.Empty;

            // Kiểm tra ràng buộc: Nếu đang có Độc giả dùng mã này thì chặn lại ngay!
            if (ldgDAO.KiemTraDangSuDung(maLDG))
            {
                error = $"Không thể xóa loại độc giả '{maLDG}' vì đang có độc giả thuộc loại này! (Tránh làm CSDL bị lủng)";
                return false;
            }

            // Gọi DAO xóa khỏi CSDL
            bool result = ldgDAO.XoaLoaiDocGia(maLDG);

            // Nếu CSDL xóa thành công, tìm đối tượng và xóa khỏi ArrayList
            if (result)
            {
                LoaiDocGiaDTO? item = TimTheoMa(maLDG);
                if (item != null)
                {
                    dsldg_arr.Remove(item);
                }
            }
            else
            {
                error = "Xóa dữ liệu thất bại!";
            }

            return result;
        }

        // 6. TÌM KIẾM TRÊN ARRAYLIST (Tuyệt đối không gọi câu lệnh SQL SELECT)
        public ArrayList TimKiem(string tuKhoa)
        {
            ArrayList ketQua = new ArrayList();
            string keyword = tuKhoa.Trim().ToLower();

            foreach (LoaiDocGiaDTO item in dsldg_arr)
            {
                if (item.MaLDG.ToLower().Contains(keyword) || 
                    item.TenLDG.ToLower().Contains(keyword))
                {
                    ketQua.Add(item);
                }
            }

            return ketQua;
        }
    }
}
