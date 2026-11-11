using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Lớp BUS xử lý nghiệp vụ cho Độc Giả.
    /// - Quản lý danh sách trong RAM bằng ArrayList (chuẩn giáo viên).
    /// - Kiểm tra tính hợp lệ dữ liệu (SĐT, Ngày sinh, Rỗng).
    /// - Kiểm tra tính duy nhất của Mã Độc Giả.
    /// - Tìm kiếm trực tiếp trên ArrayList không gọi SQL.
    /// </summary>
    public class DocGiaBUS
    {
        private readonly DocGiaDAO dgDAO = new DocGiaDAO();

        // Thuộc tính ArrayList lưu danh sách trong RAM
        public ArrayList dsdg_arr = new ArrayList();

        // 1. ĐỌC DANH SÁCH TỪ CSDL VÀ LƯU VÀO ARRAYLIST
        public ArrayList DocDSDocGia()
        {
            List<DocGiaDTO> list = dgDAO.DocDSDocGia();

            dsdg_arr.Clear();
            foreach (DocGiaDTO dg in list)
            {
                dsdg_arr.Add(dg);
            }

            return dsdg_arr;
        }

        // HÀM TÌM ĐỐI TƯỢNG THEO MÃ TRONG ARRAYLIST
        public DocGiaDTO? TimTheoMa(string maDG)
        {
            foreach (DocGiaDTO item in dsdg_arr)
            {
                if (item.MaDG.Trim().Equals(maDG.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        // 2. KIỂM TRA MÃ DUY NHẤT (Tái sử dụng TimTheoMa)
        public bool KiemTraMaDuyNhat(string maDG)
        {
            return TimTheoMa(maDG) == null;
        }

        // 3. KIỂM TRA SỐ ĐIỆN THOẠI HỢP LỆ
        // Quy chuẩn: Nếu có nhập thì phải gồm 10 chữ số và bắt đầu bằng số 0
        public static bool KiemTraSDTHopLe(string? sdt, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(sdt))
            {
                return true; // Cho phép rỗng (theo CSDL SDT là NULL)
            }

            string cleanSDT = sdt.Trim();
            if (!Regex.IsMatch(cleanSDT, @"^0\d{9}$"))
            {
                message = "Số điện thoại không hợp lệ! SĐT phải gồm đúng 10 chữ số và bắt đầu bằng số 0.";
                return false;
            }

            return true;
        }

        // 4. KIỂM TRA NGÀY SINH HỢP LỆ
        public static bool KiemTraNgaySinh(DateTime? ngaySinh, out string message)
        {
            message = string.Empty;
            if (!ngaySinh.HasValue) return true;

            if (ngaySinh.Value.Date > DateTime.Today)
            {
                message = "Ngày sinh không được lớn hơn ngày hiện tại!";
                return false;
            }

            // Độc giả phải từ đủ 5 tuổi trở lên
            if (DateTime.Today.Year - ngaySinh.Value.Year < 5)
            {
                message = "Độc giả phải từ 5 tuổi trở lên!";
                return false;
            }

            return true;
        }

        // 5. NGHIỆP VỤ THÊM ĐỘC GIẢ
        public bool Them(DocGiaDTO dg, out string error)
        {
            error = string.Empty;

            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(dg.MaDG))
            {
                error = "Mã độc giả không được để trống!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(dg.TenDG))
            {
                error = "Tên độc giả không được để trống!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(dg.MaLDG))
            {
                error = "Vui lòng chọn loại độc giả!";
                return false;
            }

            // Kiểm tra trùng mã
            if (!KiemTraMaDuyNhat(dg.MaDG))
            {
                error = $"Mã độc giả '{dg.MaDG}' đã tồn tại trong hệ thống!";
                return false;
            }

            // Kiểm tra SĐT
            if (!KiemTraSDTHopLe(dg.SDT, out string errSDT))
            {
                error = errSDT;
                return false;
            }

            // Kiểm tra ngày sinh
            if (!KiemTraNgaySinh(dg.NgaySinh, out string errNS))
            {
                error = errNS;
                return false;
            }

            // Gọi DAO ghi xuống CSDL
            bool result = dgDAO.ThemDocGia(dg);
            if (result)
            {
                dsdg_arr.Add(dg);
            }
            else
            {
                error = "Thêm độc giả vào cơ sở dữ liệu thất bại!";
            }

            return result;
        }

        // 6. NGHIỆP VỤ SỬA ĐỘC GIẢ
        public bool Sua(DocGiaDTO dg, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(dg.TenDG))
            {
                error = "Tên độc giả không được để trống!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(dg.MaLDG))
            {
                error = "Vui lòng chọn loại độc giả!";
                return false;
            }

            // Kiểm tra SĐT
            if (!KiemTraSDTHopLe(dg.SDT, out string errSDT))
            {
                error = errSDT;
                return false;
            }

            // Kiểm tra ngày sinh
            if (!KiemTraNgaySinh(dg.NgaySinh, out string errNS))
            {
                error = errNS;
                return false;
            }

            // Gọi DAO cập nhật CSDL
            bool result = dgDAO.SuaDocGia(dg);
            if (result)
            {
                DocGiaDTO? item = TimTheoMa(dg.MaDG);
                if (item != null)
                {
                    item.HoLot = dg.HoLot;
                    item.TenDG = dg.TenDG;
                    item.NgaySinh = dg.NgaySinh;
                    item.SDT = dg.SDT;
                    item.MaLDG = dg.MaLDG;
                    item.TenLDG = dg.TenLDG;
                }
            }
            else
            {
                error = "Cập nhật độc giả thất bại!";
            }

            return result;
        }

        // 7. NGHIỆP VỤ XÓA ĐỘC GIẢ
        public bool Xoa(string maDG, out string error)
        {
            error = string.Empty;

            // Kiểm tra ràng buộc khóa ngoại (đang có phiếu mượn hoặc phiếu phạt)
            if (dgDAO.KiemTraDangSuDung(maDG))
            {
                error = $"Không thể xóa độc giả '{maDG}' vì đã phát sinh lịch sử Mượn sách hoặc Phạt! (Tránh làm CSDL bị lủng)";
                return false;
            }

            bool result = dgDAO.XoaDocGia(maDG);
            if (result)
            {
                DocGiaDTO? item = TimTheoMa(maDG);
                if (item != null)
                {
                    dsdg_arr.Remove(item);
                }
            }
            else
            {
                error = "Xóa độc giả thất bại!";
            }

            return result;
        }

        // 8. TÌM KIẾM TRÊN ARRAYLIST
        public ArrayList TimKiem(string tuKhoa)
        {
            ArrayList ketQua = new ArrayList();
            string keyword = tuKhoa.Trim().ToLower();

            foreach (DocGiaDTO item in dsdg_arr)
            {
                if (item.MaDG.ToLower().Contains(keyword) ||
                    item.HoTen.ToLower().Contains(keyword) ||
                    (!string.IsNullOrEmpty(item.SDT) && item.SDT.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(item.TenLDG) && item.TenLDG.ToLower().Contains(keyword)))
                {
                    ketQua.Add(item);
                }
            }

            return ketQua;
        }
    }
}
