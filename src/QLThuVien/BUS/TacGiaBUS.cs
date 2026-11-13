#nullable disable
using System.Collections.Generic;
using System.Collections;
using QLThuVien.DAO;
using QLThuVien.DTO;
using System;

namespace QLThuVien.BUS
{
    public class TacGiaBUS
    {
        private readonly ArrayList danhSachTacGia = new ArrayList();
        private bool daNapDanhSach;

        public TacGiaBUS()
        {
        }

        // 1. Đọc danh sách Tác giả
        public List<TacGiaDTO> DocDSTacGia()
        {
            TacGiaDAO tacGiaDAO = new TacGiaDAO();
            List<TacGiaDTO> danhSach = tacGiaDAO.DocDSTG();
            danhSachTacGia.Clear();
            danhSachTacGia.AddRange(danhSach);
            daNapDanhSach = true;
            return danhSach;
        }

        // 2. KIỂM TRA TRÙNG MÃ TÁC GIẢ
        public bool KiemTraTrungMa(string maTG)
        {
            List<TacGiaDTO> list = DocDSTacGia();
            foreach (var item in list)
            {
                if (item.MaTG.Equals(maTG, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // 3. Thêm Tác giả
        public void Them(TacGiaDTO TacGia)
        {
            if (TacGia == null || string.IsNullOrWhiteSpace(TacGia.MaTG) || string.IsNullOrWhiteSpace(TacGia.TenTG))
                throw new ArgumentException("Mã và tên tác giả không được để trống.");
            string ma = TacGia.MaTG.Trim();
            if (KiemTraTrungMa(ma)) throw new InvalidOperationException($"Mã tác giả '{ma}' đã tồn tại.");
            TacGia.MaTG = ma;
            TacGia.HoLot = (TacGia.HoLot ?? string.Empty).Trim();
            TacGia.TenTG = TacGia.TenTG.Trim();
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Them(TacGia);
            DocDSTacGia();
        }

        // 4. Xóa Tác giả
        public void Xoa(string maTacGia)
        {
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Xoa(maTacGia);
            DocDSTacGia();
        }

        // 5. Sửa Tác giả
        public void Sua(TacGiaDTO TacGia)
        {
            if (TacGia == null || string.IsNullOrWhiteSpace(TacGia.MaTG) || string.IsNullOrWhiteSpace(TacGia.TenTG))
                throw new ArgumentException("Mã và tên tác giả không được để trống.");
            TacGia.MaTG = TacGia.MaTG.Trim();
            TacGia.HoLot = (TacGia.HoLot ?? string.Empty).Trim();
            TacGia.TenTG = TacGia.TenTG.Trim();
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Sua(TacGia);
            DocDSTacGia();
        }

        // 6. Tìm kiếm
        public List<TacGiaDTO> timKiem(string maTacGia, string tenTacGia)
        {
            if (!daNapDanhSach)
                DocDSTacGia();

            string tuKhoa = (maTacGia ?? tenTacGia ?? string.Empty).Trim();
            List<TacGiaDTO> ketQua = new List<TacGiaDTO>();
            foreach (TacGiaDTO tacGia in danhSachTacGia)
            {
                if ((tacGia.MaTG ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (tacGia.HoLot ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (tacGia.TenTG ?? string.Empty).IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ketQua.Add(tacGia);
                }
            }
            return ketQua;
        }
    }
}
