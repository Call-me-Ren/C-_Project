using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho Phiếu Nhập & Chi tiết Nhập sách
    /// </summary>
    public class PhieuNhapBUS
    {
        private readonly PhieuNhapDAO _pnDAO;
        private readonly ChiTietPhieuNhapDAO _ctpnDAO;
        public ArrayList dsPN_arr { get; } = new ArrayList();

        public PhieuNhapBUS()
        {
            _pnDAO = new PhieuNhapDAO();
            _ctpnDAO = new ChiTietPhieuNhapDAO();
        }

        public List<PhieuNhapDTO> GetAll(bool forceReload = false)
        {
            if (dsPN_arr.Count == 0 || forceReload)
            {
                dsPN_arr.Clear();
                List<PhieuNhapDTO> list = _pnDAO.GetAll();
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

        public bool KiemTraTrungMa(string maPN)
        {
            GetAll();
            foreach (PhieuNhapDTO item in dsPN_arr)
            {
                if (item.MaPN.Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public decimal TinhThanhTien(decimal donGia, int soLuong)
        {
            if (donGia < 0 || soLuong <= 0) return 0;
            return donGia * soLuong;
        }

        public decimal TinhTongTien(List<ChiTietPhieuNhapDTO> chiTietList)
        {
            if (chiTietList == null || chiTietList.Count == 0) return 0;
            decimal tong = 0;
            foreach (var ct in chiTietList)
            {
                tong += ct.ThanhTien;
            }
            return tong;
        }

        public List<ChiTietPhieuNhapDTO> LayChiTietTheoMaPN(string maPN)
        {
            return _ctpnDAO.GetByMaPN(maPN);
        }

        public (bool Success, string Message) TaoPhieuNhap(PhieuNhapDTO pn, List<ChiTietPhieuNhapDTO> chiTietList)
        {
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

            if (KiemTraTrungMa(pn.MaPN))
                return (false, $"Mã phiếu nhập '{pn.MaPN}' đã tồn tại trong hệ thống!");

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

            var ketQua = _pnDAO.LuuPhieuNhapTransaction(pn, chiTietList);
            if (ketQua.Success)
            {
                dsPN_arr.Insert(0, pn);
            }

            return ketQua;
        }

        public List<PhieuNhapDTO> TimKiem(string keyword)
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
