using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho Phiếu Nhập & Chi tiết Nhập sách
    /// Tuân thủ quy tắc giáo viên: Quản lý ArrayList trên RAM, tính toán nghiệp vụ (ThanhTien, TongTien, Tồn kho)
    /// </summary>
    public class PhieuNhapBUS
    {
        // Danh sách ArrayList lưu trữ danh sách Phiếu nhập trên bộ nhớ RAM
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
        /// Nghiệp vụ tính Thành Tiền của 1 dòng chi tiết nhập sách
        /// Công thức theo yêu cầu giáo viên: ThanhTien = DonGia * SoLuong
        /// </summary>
        public static decimal TinhThanhTien(decimal donGia, int soLuong)
        {
            if (donGia < 0 || soLuong <= 0) return 0;
            return donGia * soLuong;
        }

        /// <summary>
        /// Lấy toàn bộ chi tiết của 1 phiếu nhập
        /// </summary>
        public static List<ChiTietPhieuNhapDTO> LayChiTietTheoMaPN(string maPN)
        {
            return ChiTietPhieuNhapDAO.GetByMaPN(maPN);
        }

        /// <summary>
        /// Tạo mới một Phiếu Nhập cùng danh sách các cuốn sách nhập
        /// Xử lý nghiệp vụ trọn vẹn:
        /// 1. Kiểm tra mã, nhà cung cấp
        /// 2. Lưu Phiếu nhập
        /// 3. Lưu từng Chi tiết phiếu nhập
        /// 4. Tự động tăng tồn kho DauSach (SoLuong, SLConLai)
        /// 5. Tự động sinh các mã cuốn sách vật lý mới vào bảng Sach
        /// </summary>
        public static (bool Success, string Message) TaoPhieuNhap(PhieuNhapDTO pn, List<ChiTietPhieuNhapDTO> chiTietList)
        {
            if (string.IsNullOrWhiteSpace(pn.MaPN))
                return (false, "Mã phiếu nhập không được để trống!");

            if (pn.MaPN.Trim().Length > 20)
                return (false, "Mã phiếu nhập không được vượt quá 20 ký tự!");

            if (string.IsNullOrWhiteSpace(pn.MaNCC))
                return (false, "Vui lòng chọn Nhà cung cấp!");

            if (chiTietList == null || chiTietList.Count == 0)
                return (false, "Phiếu nhập phải có ít nhất 1 đầu sách!");

            pn.MaPN = pn.MaPN.Trim().ToUpper();

            if (KiemTraTrungMa(pn.MaPN))
                return (false, $"Mã phiếu nhập '{pn.MaPN}' đã tồn tại!");

            // 1. Tính Tổng Tiền tại tầng BUS
            decimal tongTien = 0;
            foreach (var ct in chiTietList)
            {
                ct.MaPN = pn.MaPN;
                ct.ThanhTien = TinhThanhTien(ct.DonGia, ct.SoLuong);
                tongTien += ct.ThanhTien;
            }
            pn.TongTien = tongTien;

            // 2. Lưu Master PhieuNhap
            if (!PhieuNhapDAO.Insert(pn))
                return (false, "Không thể lưu thông tin phiếu nhập vào CSDL!");

            // 3. Lưu từng Detail & Cập nhật kho
            foreach (var ct in chiTietList)
            {
                ChiTietPhieuNhapDAO.Insert(ct);

                // Tự động tăng tồn kho cho Đầu Sách
                DauSachDAO.TangTonKho(ct.MaDauSach, ct.SoLuong);

                // Tự động sinh các cuốn sách vật lý mới
                SachDAO.SinhCacCuonSachMoi(ct.MaDauSach, ct.SoLuong);
            }

            // 4. Đồng bộ vào ArrayList
            dsPN_arr.Insert(0, pn);

            return (true, "Tạo phiếu nhập và cập nhật tồn kho thành công!");
        }

        /// <summary>
        /// Tìm kiếm Phiếu nhập trên bộ nhớ RAM (ArrayList)
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
