using System;
using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class QuyDinhPhatBUS
    {
        private readonly QuyDinhPhatDAO dao = new QuyDinhPhatDAO();
        public ArrayList dsQuyDinh_arr = new ArrayList();

        public List<QuyDinhPhatDTO> DocDS()
        {
            List<QuyDinhPhatDTO> list = dao.LayDanhSach();
            dsQuyDinh_arr.Clear();
            foreach (QuyDinhPhatDTO qd in list)
            {
                dsQuyDinh_arr.Add(qd);
            }
            return list;
        }

        public List<QuyDinhPhatDTO> LayDanhSach(bool reload = false)
        {
            if (reload || dsQuyDinh_arr.Count == 0)
            {
                return DocDS();
            }

            List<QuyDinhPhatDTO> result = new List<QuyDinhPhatDTO>();
            foreach (object item in dsQuyDinh_arr)
            {
                if (item is QuyDinhPhatDTO qd)
                {
                    result.Add(qd);
                }
            }
            return result;
        }

        public bool KiemTraMaDuyNhat(string maQD)
        {
            if (string.IsNullOrWhiteSpace(maQD)) return false;

            string normalized = maQD.Trim();
            foreach (object item in dsQuyDinh_arr)
            {
                if (item is QuyDinhPhatDTO qd && string.Equals(qd.MaQD?.Trim(), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public void KiemTraHopLe(QuyDinhPhatDTO qd, bool isThemMoi)
        {
            if (qd == null)
            {
                throw new ArgumentNullException(nameof(qd), "Dữ liệu quy định phạt không được null.");
            }

            if (string.IsNullOrWhiteSpace(qd.MaQD))
            {
                throw new ArgumentException("Mã quy định không được để trống.");
            }

            string maTrim = qd.MaQD.Trim();
            if (maTrim.Length > 20)
            {
                throw new ArgumentException("Mã quy định không được vượt quá 20 ký tự.");
            }

            if (isThemMoi)
            {
                if (!KiemTraMaDuyNhat(maTrim))
                {
                    throw new InvalidOperationException($"Mã quy định '{maTrim}' đã tồn tại trong danh sách.");
                }
                if (dao.KiemTraTonTai(maTrim))
                {
                    throw new InvalidOperationException($"Mã quy định '{maTrim}' đã tồn tại trong cơ sở dữ liệu.");
                }
            }

            if (string.IsNullOrWhiteSpace(qd.TenQD))
            {
                throw new ArgumentException("Tên quy định không được để trống.");
            }

            if (qd.TenQD.Trim().Length > 100)
            {
                throw new ArgumentException("Tên quy định không được vượt quá 100 ký tự.");
            }

            if (!string.IsNullOrWhiteSpace(qd.LoaiPhat) && qd.LoaiPhat.Trim().Length > 50)
            {
                throw new ArgumentException("Loại phạt không được vượt quá 50 ký tự.");
            }

            if (!string.IsNullOrWhiteSpace(qd.CachTinh) && qd.CachTinh.Trim().Length > 100)
            {
                throw new ArgumentException("Cách tính không được vượt quá 100 ký tự.");
            }

            if (!string.IsNullOrWhiteSpace(qd.DonViTinh) && qd.DonViTinh.Trim().Length > 20)
            {
                throw new ArgumentException("Đơn vị tính không được vượt quá 20 ký tự.");
            }

            if (qd.MucPhat < 0)
            {
                throw new ArgumentException("Mức phạt phải là số không âm (>= 0).");
            }
        }

        public bool Them(QuyDinhPhatDTO qd)
        {
            KiemTraHopLe(qd, isThemMoi: true);

            QuyDinhPhatDTO normalized = new QuyDinhPhatDTO(
                qd.MaQD.Trim(),
                qd.TenQD.Trim(),
                string.IsNullOrWhiteSpace(qd.LoaiPhat) ? null : qd.LoaiPhat.Trim(),
                string.IsNullOrWhiteSpace(qd.CachTinh) ? null : qd.CachTinh.Trim(),
                string.IsNullOrWhiteSpace(qd.DonViTinh) ? null : qd.DonViTinh.Trim(),
                qd.MucPhat
            );

            bool success = dao.Them(normalized);
            if (success)
            {
                dsQuyDinh_arr.Add(normalized);
            }
            return success;
        }

        public bool Sua(QuyDinhPhatDTO qd)
        {
            KiemTraHopLe(qd, isThemMoi: false);

            string maTrim = qd.MaQD.Trim();
            if (!dao.KiemTraTonTai(maTrim))
            {
                throw new KeyNotFoundException($"Không tìm thấy quy định phạt có mã '{maTrim}' để cập nhật.");
            }

            QuyDinhPhatDTO normalized = new QuyDinhPhatDTO(
                maTrim,
                qd.TenQD.Trim(),
                string.IsNullOrWhiteSpace(qd.LoaiPhat) ? null : qd.LoaiPhat.Trim(),
                string.IsNullOrWhiteSpace(qd.CachTinh) ? null : qd.CachTinh.Trim(),
                string.IsNullOrWhiteSpace(qd.DonViTinh) ? null : qd.DonViTinh.Trim(),
                qd.MucPhat
            );

            bool success = dao.Sua(normalized);
            if (success)
            {
                for (int i = 0; i < dsQuyDinh_arr.Count; i++)
                {
                    if (dsQuyDinh_arr[i] is QuyDinhPhatDTO current && string.Equals(current.MaQD?.Trim(), maTrim, StringComparison.OrdinalIgnoreCase))
                    {
                        dsQuyDinh_arr[i] = normalized;
                        break;
                    }
                }
            }
            return success;
        }

        public bool Xoa(string maQD)
        {
            if (string.IsNullOrWhiteSpace(maQD))
            {
                throw new ArgumentException("Mã quy định cần xóa không được để trống.");
            }

            string maTrim = maQD.Trim();
            if (dao.KiemTraKhoaNgoai(maTrim))
            {
                throw new InvalidOperationException("Không thể xóa quy định phạt này vì đã được áp dụng trong Chi tiết phiếu phạt.");
            }

            bool success = dao.Xoa(maTrim);
            if (success)
            {
                for (int i = 0; i < dsQuyDinh_arr.Count; i++)
                {
                    if (dsQuyDinh_arr[i] is QuyDinhPhatDTO current && string.Equals(current.MaQD?.Trim(), maTrim, StringComparison.OrdinalIgnoreCase))
                    {
                        dsQuyDinh_arr.RemoveAt(i);
                        break;
                    }
                }
            }
            return success;
        }

        public List<QuyDinhPhatDTO> TimKiem(string tuKhoa)
        {
            List<QuyDinhPhatDTO> ketQua = new List<QuyDinhPhatDTO>();

            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayDanhSach();
            }

            string key = tuKhoa.Trim();
            foreach (object item in dsQuyDinh_arr)
            {
                if (item is QuyDinhPhatDTO qd)
                {
                    bool matchMa = qd.MaQD.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchTen = qd.TenQD.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchLoai = qd.LoaiPhat != null && qd.LoaiPhat.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchCachTinh = qd.CachTinh != null && qd.CachTinh.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchDVT = qd.DonViTinh != null && qd.DonViTinh.Contains(key, StringComparison.OrdinalIgnoreCase);
                    bool matchMucPhat = qd.MucPhat.ToString().Contains(key, StringComparison.OrdinalIgnoreCase);

                    if (matchMa || matchTen || matchLoai || matchCachTinh || matchDVT || matchMucPhat)
                    {
                        ketQua.Add(qd);
                    }
                }
            }

            return ketQua;
        }
    }
}
