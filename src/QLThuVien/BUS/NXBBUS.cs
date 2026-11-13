#nullable disable
using System.Collections.Generic;
using System.Collections;
using System;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class NXBBUS
    {
        private readonly ArrayList danhSachNXB = new ArrayList();
        private bool daNapDanhSach;
        public NXBBUS()
        {
        }

        // 1. Đọc danh sách NXB (Loại bỏ cache static)
        public List<NXBDTO> DocDSNXB()
        {
            NXBDAO NXBDAO = new NXBDAO();
            List<NXBDTO> danhSach = NXBDAO.DocDSNXB();
            danhSachNXB.Clear();
            danhSachNXB.AddRange(danhSach);
            daNapDanhSach = true;
            return danhSach;
        }

        private ArrayList LayDanhSachTrongBoNho()
        {
            if (!daNapDanhSach) DocDSNXB();
            return danhSachNXB;
        }

        // 2. Kiểm tra trùng Mã NXB
        public bool KiemTraTrungMa(string maNXB)
        {
            List<NXBDTO> list = DocDSNXB();
            foreach (var item in list)
            {
                if (item.MaNXB.Equals(maNXB, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // 3. Thêm NXB
        public void Them(NXBDTO NXB)
        {
            if (NXB == null || string.IsNullOrWhiteSpace(NXB.MaNXB) || string.IsNullOrWhiteSpace(NXB.TenNXB))
                throw new ArgumentException("Mã và tên nhà xuất bản không được để trống.");
            string ma = NXB.MaNXB.Trim();
            if (KiemTraTrungMa(ma))
                throw new InvalidOperationException($"Mã NXB '{ma}' đã tồn tại trong hệ thống.");
            NXB.MaNXB = ma;
            NXB.TenNXB = NXB.TenNXB.Trim();
            NXB.DiaChi = (NXB.DiaChi ?? string.Empty).Trim();
            NXBDAO NXBDAO = new NXBDAO();
            NXBDAO.Them(NXB);
            DocDSNXB();
        }

        // 4. Xóa NXB
        public void Xoa(string maNXB)
        {
            if (string.IsNullOrWhiteSpace(maNXB))
                throw new ArgumentException("Mã nhà xuất bản không được để trống.");
            NXBDAO NXBDAO = new NXBDAO();
            NXBDAO.Xoa(maNXB.Trim());
            DocDSNXB();
        }

        // 5. Sửa NXB
        public void Sua(NXBDTO NXB)
        {
            if (NXB == null || string.IsNullOrWhiteSpace(NXB.MaNXB) || string.IsNullOrWhiteSpace(NXB.TenNXB))
                throw new ArgumentException("Mã và tên nhà xuất bản không được để trống.");
            NXB.MaNXB = NXB.MaNXB.Trim();
            NXB.TenNXB = NXB.TenNXB.Trim();
            NXB.DiaChi = (NXB.DiaChi ?? string.Empty).Trim();
            NXBDAO NXBDAO = new NXBDAO();
            NXBDAO.Sua(NXB);
            DocDSNXB();
        }

        // 6. Tìm kiếm NXB
        public List<NXBDTO> timKiem(string maNXB, string tenNXB)
        {
            string ma = (maNXB ?? string.Empty).Trim();
            string ten = (tenNXB ?? string.Empty).Trim();
            List<NXBDTO> ketQua = new List<NXBDTO>();
            foreach (NXBDTO nxb in LayDanhSachTrongBoNho())
                if ((!string.IsNullOrEmpty(ma) && (nxb.MaNXB ?? string.Empty).IndexOf(ma, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(ten) && (nxb.TenNXB ?? string.Empty).IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0))
                    ketQua.Add(nxb);
            return ketQua;
        }
    }
}
