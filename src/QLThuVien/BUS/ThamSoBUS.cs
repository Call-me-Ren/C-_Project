using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho bảng ThamSo (Bảng thứ 17 - Yêu cầu 9 của GV)
    /// Đọc và lưu trữ tham số trên ArrayList, cung cấp hàm lấy giá trị theo mã tham số.
    /// </summary>
    public class ThamSoBUS
    {
        private readonly ThamSoDAO _tsDAO;
        public ArrayList dsts_arr { get; } = new ArrayList();

        public ThamSoBUS()
        {
            _tsDAO = new ThamSoDAO();
        }

        public List<ThamSoDTO> DocDS(bool forceReload = false)
        {
            if (dsts_arr.Count == 0 || forceReload)
            {
                dsts_arr.Clear();
                List<ThamSoDTO> list = _tsDAO.DocDS();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        dsts_arr.Add(item);
                    }
                }
            }

            List<ThamSoDTO> result = new List<ThamSoDTO>();
            foreach (ThamSoDTO item in dsts_arr)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Lấy giá trị tham số cấu hình từ ArrayList trên RAM theo mã tham số (không query CSDL liên tục)
        /// </summary>
        public string LayGiaTriTheoMa(string maTS, string giaTriMacDinh = "")
        {
            DocDS();
            foreach (ThamSoDTO item in dsts_arr)
            {
                if (item.MaTS.Equals(maTS.Trim(), StringComparison.OrdinalIgnoreCase))
                    return item.GiaTri;
            }
            return giaTriMacDinh;
        }

        /// <summary>
        /// Cập nhật giá trị tham số cấu hình với kiểm tra dữ liệu hợp lệ
        /// </summary>
        public (bool Success, string Message) CapNhat(string maTS, string giaTriMoi)
        {
            if (string.IsNullOrWhiteSpace(maTS))
                return (false, "Mã tham số không hợp lệ!");

            if (string.IsNullOrWhiteSpace(giaTriMoi))
                return (false, "Giá trị tham số không được để trống!");

            maTS = maTS.Trim();
            giaTriMoi = giaTriMoi.Trim();

            // Validate logic cho các tham số dạng số
            if (maTS == "SO_SACH_MUON_TOI_DA" || maTS == "SO_NGAY_MUON_TOI_DA" || 
                maTS == "TIEN_PHAT_MOI_NGAY" || maTS == "NAM_XB_TOI_THIEU")
            {
                if (!long.TryParse(giaTriMoi, out long val) || val < 0)
                    return (false, "Giá trị của tham số này phải là một số nguyên không âm!");
            }

            if (_tsDAO.CapNhatGiaTri(maTS, giaTriMoi))
            {
                // Đồng bộ lên ArrayList
                foreach (ThamSoDTO item in dsts_arr)
                {
                    if (item.MaTS.Equals(maTS, StringComparison.OrdinalIgnoreCase))
                    {
                        item.GiaTri = giaTriMoi;
                        break;
                    }
                }
                return (true, "Cập nhật tham số cấu hình thành công!");
            }

            return (false, "Lỗi khi cập nhật cơ sở dữ liệu!");
        }
    }
}
