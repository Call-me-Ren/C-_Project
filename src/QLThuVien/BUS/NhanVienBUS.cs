using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAO dao = new NhanVienDAO();
        public ArrayList dsNhanVien_arr = new ArrayList();

        public List<NhanVienDTO> DocDS()
        {
            List<NhanVienDTO> list = dao.LayDanhSach();
            dsNhanVien_arr.Clear();
            foreach (NhanVienDTO nv in list)
            {
                dsNhanVien_arr.Add(nv);
            }
            return list;
        }

        public List<NhanVienDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsNhanVien_arr.Count == 0)
            {
                return DocDS();
            }

            List<NhanVienDTO> result = new List<NhanVienDTO>();
            foreach (object item in dsNhanVien_arr)
            {
                if (item is NhanVienDTO nv)
                {
                    result.Add(nv);
                }
            }
            return result;
        }

        public bool KiemTraMaDuyNhat(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return false;

            string normalized = maNV.Trim();
            foreach (object item in dsNhanVien_arr)
            {
                if (item is NhanVienDTO nv && string.Equals(nv.MaNV?.Trim(), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public void KiemTraHopLe(NhanVienDTO nv, bool isThemMoi)
        {
            if (nv == null)
            {
                throw new ArgumentNullException(nameof(nv), "Dữ liệu nhân viên không được null.");
            }

            if (string.IsNullOrWhiteSpace(nv.MaNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }

            string maTrim = nv.MaNV.Trim();
            if (maTrim.Length > 20)
            {
                throw new ArgumentException("Mã nhân viên không được vượt quá 20 ký tự.");
            }

            if (isThemMoi)
            {
                if (!KiemTraMaDuyNhat(maTrim))
                {
                    throw new InvalidOperationException($"Mã nhân viên '{maTrim}' đã tồn tại trong danh sách.");
                }
                if (dao.KiemTraTonTai(maTrim))
                {
                    throw new InvalidOperationException($"Mã nhân viên '{maTrim}' đã tồn tại trong cơ sở dữ liệu.");
                }
            }

            if (string.IsNullOrWhiteSpace(nv.TenNV))
            {
                throw new ArgumentException("Tên nhân viên không được để trống.");
            }

            if (nv.TenNV.Trim().Length > 50)
            {
                throw new ArgumentException("Tên nhân viên không được vượt quá 50 ký tự.");
            }

            if (!string.IsNullOrWhiteSpace(nv.HoLot) && nv.HoLot.Trim().Length > 50)
            {
                throw new ArgumentException("Họ lót không được vượt quá 50 ký tự.");
            }

            if (!string.IsNullOrWhiteSpace(nv.ChucVu) && nv.ChucVu.Trim().Length > 50)
            {
                throw new ArgumentException("Chức vụ không được vượt quá 50 ký tự.");
            }
        }

        public bool Them(NhanVienDTO nv)
        {
            KiemTraHopLe(nv, isThemMoi: true);

            NhanVienDTO normalized = new NhanVienDTO(
                nv.MaNV.Trim(),
                string.IsNullOrWhiteSpace(nv.HoLot) ? null : nv.HoLot.Trim(),
                nv.TenNV.Trim(),
                string.IsNullOrWhiteSpace(nv.ChucVu) ? null : nv.ChucVu.Trim()
            );

            bool success = dao.Them(normalized);
            if (success)
            {
                dsNhanVien_arr.Add(normalized);
            }
            return success;
        }

        public bool Sua(NhanVienDTO nv)
        {
            KiemTraHopLe(nv, isThemMoi: false);

            string maTrim = nv.MaNV.Trim();
            if (!dao.KiemTraTonTai(maTrim))
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên có mã '{maTrim}' để cập nhật.");
            }

            NhanVienDTO normalized = new NhanVienDTO(
                maTrim,
                string.IsNullOrWhiteSpace(nv.HoLot) ? null : nv.HoLot.Trim(),
                nv.TenNV.Trim(),
                string.IsNullOrWhiteSpace(nv.ChucVu) ? null : nv.ChucVu.Trim()
            );

            bool success = dao.Sua(normalized);
            if (success)
            {
                for (int i = 0; i < dsNhanVien_arr.Count; i++)
                {
                    if (dsNhanVien_arr[i] is NhanVienDTO current && string.Equals(current.MaNV?.Trim(), maTrim, StringComparison.OrdinalIgnoreCase))
                    {
                        dsNhanVien_arr[i] = normalized;
                        break;
                    }
                }
            }
            return success;
        }

        public bool Xoa(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên cần xóa không được để trống.");
            }

            string maTrim = maNV.Trim();
            if (dao.KiemTraKhoaNgoai(maTrim))
            {
                throw new InvalidOperationException("Không thể xóa nhân viên này vì đã phát sinh giao dịch liên quan (Phiếu nhập, Phiếu mượn hoặc Phiếu phạt).");
            }

            bool success = dao.Xoa(maTrim);
            if (success)
            {
                for (int i = 0; i < dsNhanVien_arr.Count; i++)
                {
                    if (dsNhanVien_arr[i] is NhanVienDTO current && string.Equals(current.MaNV?.Trim(), maTrim, StringComparison.OrdinalIgnoreCase))
                    {
                        dsNhanVien_arr.RemoveAt(i);
                        break;
                    }
                }
            }
            return success;
        }

        public List<NhanVienDTO> TimKiem(string tuKhoa)
        {
            List<NhanVienDTO> ketQua = new List<NhanVienDTO>();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayDanhSach();
            }

            string key = tuKhoa.Trim();
            foreach (object item in dsNhanVien_arr)
            {
                if (item is NhanVienDTO nv)
                {
                    bool matchMa = nv.MaNV.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchTen = nv.TenNV.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchHoLot = nv.HoLot != null && nv.HoLot.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchHoTen = nv.HoTen.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchChucVu = nv.ChucVu != null && nv.ChucVu.Contains(key, StringComparison.OrdinalIgnoreCase);

                    if (matchMa || matchTen || matchHoLot || matchHoTen || matchChucVu)
                    {
                        ketQua.Add(nv);
                    }
                }
            }

            return ketQua;
        }
    }
}
