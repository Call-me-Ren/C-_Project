using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho Phiếu Nhập & Chi tiết Nhập sách (Issue #8)
    /// Đảm nhận trọn vẹn các nghiệp vụ:
    /// 1. Tự tính Thành tiền = Đơn giá * Số lượng
    /// 2. Tự tính Tổng tiền = Tổng các Thành tiền
    /// 3. Kiểm tra tính hợp lệ dữ liệu (Validate) & Kiểm tra trùng mã
    /// 4. Quản lý lưu trữ trên RAM (ArrayList dsPN_arr)
    /// 5. Gọi DAO thực hiện giao dịch cập nhật tồn kho DauSach và sinh mã Sach mới
    /// </summary>
    public class PhieuNhapBUS
    {
        // 1. Danh sách ArrayList lưu trữ dữ liệu Phiếu nhập trên bộ nhớ RAM (Quy tắc của GV)
        public static ArrayList dsPN_arr = new ArrayList();

        public static List<PhieuNhapDTO> GetAll(bool forceReload = false)
        {
            if (dsPN_arr.Count == 0 || forceReload)
            {
                dsPN_arr.Clear();
                List<PhieuNhapDTO> list = PhieuNhapDAO.GetAll();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        dsPN_arr.Add(item);
                    }
                }
            }

            List<PhieuNhapDTO> result = new List<PhieuNhapDTO>();
            foreach (PhieuNhapDTO item in dsPN_arr)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra trùng mã Phiếu nhập trên bộ nhớ RAM (Yêu cầu tính duy nhất của GV)
        /// </summary>
        public static bool KiemTraTrungMa(string maPN)
        {
            GetAll();
            foreach (PhieuNhapDTO item in dsPN_arr)
            {
                if (item.MaPN.Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Nghiệp vụ 1: Tự tính Thành Tiền của 1 dòng chi tiết
        /// Công thức: Thành tiền = Đơn giá * Số lượng
        /// </summary>
        public static decimal TinhThanhTien(decimal donGia, int soLuong)
        {
            if (donGia < 0 || soLuong <= 0) return 0;
            return donGia * soLuong;
        }

        /// <summary>
        /// Nghiệp vụ 2: Tự tính Tổng Tiền của toàn bộ phiếu nhập
        /// Công thức: Tổng tiền = SUM(Thành tiền)
        /// </summary>
        public static decimal TinhTongTien(List<ChiTietPhieuNhapDTO> chiTietList)
        {
            if (chiTietList == null || chiTietList.Count == 0) return 0;
            decimal tong = 0;
            foreach (var ct in chiTietList)
            {
                tong += ct.ThanhTien;
            }
            return tong;
        }

        /// <summary>
        /// Lấy toàn bộ chi tiết sách nhập của 1 phiếu nhập
        /// </summary>
        public static List<ChiTietPhieuNhapDTO> LayChiTietTheoMaPN(string maPN)
        {
            return ChiTietPhieuNhapDAO.GetByMaPN(maPN);
        }

        /// <summary>
        /// Nghiệp vụ 3: Tạo mới Phiếu Nhập & Kích hoạt chuỗi xử lý nghiệp vụ CSDL
        /// - Validate dữ liệu
        /// - Tính toán Thành tiền, Tổng tiền
        /// - Gửi xuống DAO thực thi Transaction (Thêm PN, CTPN, tăng tồn kho DauSach, sinh mã Sach mới)
        /// - Đồng bộ dữ liệu mới vào ArrayList trên RAM
        /// </summary>
        public static (bool Success, string Message) TaoPhieuNhap(PhieuNhapDTO pn, List<ChiTietPhieuNhapDTO> chiTietList)
        {
            // Kiểm tra tính hợp lệ của Phiếu nhập (Validation)
            if (string.IsNullOrWhiteSpace(pn.MaPN))
                return (false, "Mã phiếu nhập không được để trống!");

            if (pn.MaPN.Trim().Length > 20)
                return (false, "Mã phiếu nhập không được vượt quá 20 ký tự!");

            if (string.IsNullOrWhiteSpace(pn.MaNCC))
                return (false, "Vui lòng chọn Nhà cung cấp!");

            if (string.IsNullOrWhiteSpace(pn.MaNV))
                return (false, "Vui lòng chỉ định Nhân viên lập phiếu!");

            if (chiTietList == null || chiTietList.Count == 0)
                return (false, "Phiếu nhập phải có ít nhất 1 đầu sách!");

            pn.MaPN = pn.MaPN.Trim().ToUpper();

            // Kiểm tra tính duy nhất của mã phiếu nhập
            if (KiemTraTrungMa(pn.MaPN))
                return (false, $"Mã phiếu nhập '{pn.MaPN}' đã tồn tại trong hệ thống!");

            // Kiểm tra hợp lệ từng dòng chi tiết và tính tiền
            decimal tongTien = 0;
            foreach (var ct in chiTietList)
            {
                if (string.IsNullOrWhiteSpace(ct.MaDauSach))
                    return (false, "Có dòng chi tiết chưa chọn Đầu sách!");

                if (ct.SoLuong <= 0)
                    return (false, $"Số lượng nhập của đầu sách '{ct.TenSach}' phải lớn hơn 0!");

                if (ct.DonGia < 0)
                    return (false, $"Đơn giá của đầu sách '{ct.TenSach}' không được âm!");

                ct.MaPN = pn.MaPN;
                ct.ThanhTien = TinhThanhTien(ct.DonGia, ct.SoLuong);
                tongTien += ct.ThanhTien;
            }
            pn.TongTien = tongTien;

            // Gọi DAO thực thi toàn bộ giao dịch an toàn (Transaction)
            var ketQua = PhieuNhapDAO.LuuPhieuNhapTransaction(pn, chiTietList);
            if (ketQua.Success)
            {
                // Đồng bộ vào ArrayList trên RAM
                dsPN_arr.Insert(0, pn);
            }

            return ketQua;
        }

        /// <summary>
        /// Tìm kiếm Phiếu nhập lọc trên bộ nhớ RAM (ArrayList)
        /// </summary>
        public static List<PhieuNhapDTO> TimKiem(string keyword)
        {
            GetAll();
            List<PhieuNhapDTO> ketQua = new List<PhieuNhapDTO>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                foreach (PhieuNhapDTO item in dsPN_arr)
                    ketQua.Add(item);
                return ketQua;
            }

            keyword = keyword.Trim().ToLower();
            foreach (PhieuNhapDTO item in dsPN_arr)
            {
                if (item.MaPN.ToLower().Contains(keyword) || 
                    item.MaNCC.ToLower().Contains(keyword) || 
                    item.MaNV.ToLower().Contains(keyword))
                {
                    ketQua.Add(item);
                }
            }

            return ketQua;
        }
    }
}
