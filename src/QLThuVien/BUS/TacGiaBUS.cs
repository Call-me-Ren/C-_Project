#nullable disable
using System.Collections.Generic;
using System.Collections;
using QLThuVien.DAO;
using DTO;
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

        // 1. Đọc danh sách Tác giả (Luôn đọc mới từ CSDL để không bị trùng dòng UI)
        public List<TacGiaDTO> DocDSSach()
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
            List<TacGiaDTO> list = DocDSSach();
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
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Them(TacGia);
        }

        // 4. Xóa Tác giả
        public void Xoa(string maTacGia)
        {
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Xoa(maTacGia);
        }

        // 5. Sửa Tác giả
        public void Sua(TacGiaDTO TacGia)
        {
            TacGiaDAO TacGiaDAO = new TacGiaDAO();
            TacGiaDAO.Sua(TacGia);
        }

        // 6. Tìm kiếm
        public List<TacGiaDTO> timKiem(string maTacGia, string tenTacGia)
        {
            if (!daNapDanhSach)
                DocDSSach();

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
