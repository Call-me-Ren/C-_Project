#nullable disable
using System.Collections.Generic;
using System.Collections;
using System;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class TheLoaiBUS
    {
        private readonly ArrayList danhSachTheLoai = new ArrayList();
        private bool daNapDanhSach;
        public TheLoaiBUS()
        {
        }

        // 1. Đọc danh sách Thể loại (Bỏ cache tĩnh để không bị lặp dòng)
        public List<TheLoaiDTO> DocDSTL()
        {
            TheLoaiDAO TheLoaiDAO = new TheLoaiDAO();
            List<TheLoaiDTO> danhSach = TheLoaiDAO.DocDSTL();
            danhSachTheLoai.Clear();
            danhSachTheLoai.AddRange(danhSach);
            daNapDanhSach = true;
            return danhSach;
        }

        private ArrayList LayDanhSachTrongBoNho()
        {
            if (!daNapDanhSach) DocDSTL();
            return danhSachTheLoai;
        }

        // 2. Kiểm tra trùng Mã thể loại
        public bool KiemTraTrungMa(string maTL)
        {
            List<TheLoaiDTO> list = DocDSTL();
            foreach (var item in list)
            {
                if (item.MaTL.Equals(maTL, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // 3. Thêm Thể loại
        public void Them(TheLoaiDTO TheLoai)
        {
            if (TheLoai == null || string.IsNullOrWhiteSpace(TheLoai.MaTL) || string.IsNullOrWhiteSpace(TheLoai.TenTL))
                throw new ArgumentException("Mã và tên thể loại không được để trống.");
            string ma = TheLoai.MaTL.Trim();
            if (KiemTraTrungMa(ma)) throw new InvalidOperationException($"Mã thể loại '{ma}' đã tồn tại.");
            TheLoai.MaTL = ma;
            TheLoai.TenTL = TheLoai.TenTL.Trim();
            TheLoaiDAO TheLoaiDAO = new TheLoaiDAO();
            TheLoaiDAO.Them(TheLoai);
            DocDSTL();
        }

        // 4. Xóa Thể loại
        public void Xoa(string maTL)
        {
            TheLoaiDAO TheLoaiDAO = new TheLoaiDAO();
            TheLoaiDAO.Xoa(maTL);
            DocDSTL();
        }

        // 5. Sửa Thể loại
        public void Sua(TheLoaiDTO TheLoai)
        {
            if (TheLoai == null || string.IsNullOrWhiteSpace(TheLoai.MaTL) || string.IsNullOrWhiteSpace(TheLoai.TenTL))
                throw new ArgumentException("Mã và tên thể loại không được để trống.");
            TheLoai.MaTL = TheLoai.MaTL.Trim();
            TheLoai.TenTL = TheLoai.TenTL.Trim();
            TheLoaiDAO TheLoaiDAO = new TheLoaiDAO();
            TheLoaiDAO.Sua(TheLoai);
            DocDSTL();
        }

        // 6. Tìm kiếm
        public List<TheLoaiDTO> timKiem(string maTheLoai, string tenTheLoai)
        {
            string ma = (maTheLoai ?? string.Empty).Trim();
            string ten = (tenTheLoai ?? string.Empty).Trim();
            List<TheLoaiDTO> ketQua = new List<TheLoaiDTO>();
            foreach (TheLoaiDTO theLoai in LayDanhSachTrongBoNho())
                if ((!string.IsNullOrEmpty(ma) && (theLoai.MaTL ?? string.Empty).IndexOf(ma, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(ten) && (theLoai.TenTL ?? string.Empty).IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0))
                    ketQua.Add(theLoai);
            return ketQua;
        }
    }
}
