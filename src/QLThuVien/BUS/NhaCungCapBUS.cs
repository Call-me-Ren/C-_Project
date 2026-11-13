using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Business Logic Layer cho bảng NhaCungCap
    /// Lưu trữ dữ liệu vào ArrayList, tìm kiếm trên ArrayList, xử lý hợp lệ/trùng mã.
    /// </summary>
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAO _nccDAO;
        public ArrayList dsNCC_arr { get; } = new ArrayList();

        public NhaCungCapBUS()
        {
            _nccDAO = new NhaCungCapDAO();
        }

        public List<NhaCungCapDTO> GetAll(bool forceReload = false)
        {
            if (dsNCC_arr.Count == 0 || forceReload)
            {
                dsNCC_arr.Clear();
                List<NhaCungCapDTO> list = _nccDAO.GetAll();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        dsNCC_arr.Add(item);
                    }
                }
            }

            List<NhaCungCapDTO> result = new List<NhaCungCapDTO>();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                result.Add(item);
            }
            return result;
        }

        public string LayTenTheoMa(string maNCC)
        {
            GetAll();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                if (item.MaNCC.Equals(maNCC, StringComparison.OrdinalIgnoreCase))
                    return item.TenNCC;
            }

            return maNCC;
        }

        public bool KiemTraTrungMa(string maNCC)
        {
            GetAll();
            foreach (NhaCungCapDTO item in dsNCC_arr)
            {
                if (item.MaNCC.Equals(maNCC.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public (bool Success, string Message) Them(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
                return (false, "Mã nhà cung cấp không được để trống!");

            if (ncc.MaNCC.Trim().Length > 20)
                return (false, "Mã nhà cung cấp không được vượt quá 20 ký tự!");

            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                return (false, "Tên nhà cung cấp không được để trống!");

            if (ncc.TenNCC.Trim().Length > 100)
                return (false, "Tên nhà cung cấp không được vượt quá 100 ký tự!");

            ncc.MaNCC = ncc.MaNCC.Trim().ToUpper();
            ncc.TenNCC = ncc.TenNCC.Trim();

            if (KiemTraTrungMa(ncc.MaNCC))
                return (false, $"Mã nhà cung cấp '{ncc.MaNCC}' đã tồn tại!");

            if (_nccDAO.Insert(ncc))
            {
                dsNCC_arr.Add(ncc);
                return (true, "Thêm nhà cung cấp thành công!");
            }

            return (false, "Lỗi khi lưu vào cơ sở dữ liệu!");
        }

        public (bool Success, string Message) Sua(NhaCungCapDTO ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                return (false, "Tên nhà cung cấp không được để trống!");

            if (ncc.TenNCC.Trim().Length > 100)
                return (false, "Tên nhà cung cấp không được vượt quá 100 ký tự!");

            ncc.TenNCC = ncc.TenNCC.Trim();

            if (_nccDAO.Update(ncc))
            {
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

        public (bool Success, string Message) Xoa(string maNCC)
        {
            maNCC = maNCC.Trim();

            if (_nccDAO.HasPhieuNhap(maNCC))
            {
                return (false, "Không thể xóa nhà cung cấp này vì đã có Phiếu nhập liên kết trong hệ thống!");
            }

            if (_nccDAO.Delete(maNCC))
            {
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

        public List<NhaCungCapDTO> TimKiem(string keyword)
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
