#nullable disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class SachBUS
    {
        public static List<SachDTO> ds = new List<SachDTO>();
        private static readonly ArrayList danhSachSachTrongBoNho = new ArrayList();
        private static bool daNapDanhSach;
        private DauSachBUS dauSachBUS = new DauSachBUS();

        public SachBUS()
        {
        }

        // 1. Đọc danh sách Sách
        public List<SachDTO> DocDSSach()
        {
            SachDAO sachDAO = new SachDAO();
            ds = sachDAO.DocDSSach();
            danhSachSachTrongBoNho.Clear();
            danhSachSachTrongBoNho.AddRange(ds);
            daNapDanhSach = true;
            return ds;
        }

        private ArrayList LayDanhSachTrongBoNho()
        {
            if (!daNapDanhSach) DocDSSach();
            return danhSachSachTrongBoNho;
        }

        public bool KiemTraTrungMa(string maSach)
        {
            if (string.IsNullOrWhiteSpace(maSach)) return false;
            return LayDanhSachTrongBoNho().Cast<SachDTO>()
                .Any(s => string.Equals(s.MaSach, maSach.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // 2. Thêm Sách & tự động cập nhật số lượng của Đầu sách
        public bool Them(SachDTO sach)
        {
            if (KiemTraTrungMa(sach.MaSach))
                throw new InvalidOperationException($"Mã sách '{sach.MaSach}' đã tồn tại. Vui lòng nhập mã sách khác.");

            SachDAO sachDAO = new SachDAO();
            bool ketQua = sachDAO.Them(sach);
            if (ketQua)
            {
                dauSachBUS.CapNhatSoLuong(sach.MaDauSach);
                DocDSSach();
            }
            return ketQua;
        }

        // 3. Xóa Sách (CSDL sẽ chặn nếu còn dữ liệu phiếu mượn tham chiếu)
        public bool Xoa(string maSach, string maDauSach)
        {
            SachDAO sachDAO = new SachDAO();

            SachDTO sachCanXoa = LayDanhSachTrongBoNho().Cast<SachDTO>()
                .FirstOrDefault(s => string.Equals((s.MaSach ?? string.Empty).Trim(), (maSach ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase));

            if (sachCanXoa != null && LaSachDangDuocMuon(sachCanXoa.TrangThai))
            {
                throw new InvalidOperationException(
                    $"Không thể xóa sách '{maSach}' vì cuốn sách này đang được cho mượn. Hãy ghi nhận trả sách trước.");
            }

            bool ketQua = sachDAO.Xoa(maSach);
            if (ketQua)
            {
                dauSachBUS.CapNhatSoLuong(maDauSach);
                DocDSSach();
            }
            return ketQua;
        }

        private static bool LaSachDangDuocMuon(string trangThai)
        {
            string trangThaiChuanHoa = (trangThai ?? string.Empty).Trim();
            return trangThaiChuanHoa == "1"
                || string.Equals(trangThaiChuanHoa, "True", StringComparison.OrdinalIgnoreCase)
                || string.Equals(trangThaiChuanHoa, "Đã cho mượn", StringComparison.OrdinalIgnoreCase);
        }

        // 4. Sửa Sách (Chặn nếu ĐÃ CHO MƯỢN)
        public bool Sua(SachDTO sach)
        {
            SachDAO sachDAO = new SachDAO();

            List<SachDTO> listTim = timKiem(sach.MaSach, null);
            if (listTim.Count > 0)
            {
                SachDTO s = listTim[0];
                if (s.TrangThai == "1" || s.TrangThai == "True" || s.TrangThai == "Đã cho mượn")
                {
                    throw new Exception($"Sách '{sach.MaSach}' đang trong trạng thái ĐÃ CHO MƯỢN, không được phép sửa!");
                }
            }

            bool ketQua = sachDAO.Sua(sach);
            if (ketQua)
            {
                dauSachBUS.CapNhatSoLuong(sach.MaDauSach);
                DocDSSach();
            }
            return ketQua;
        }

        // 5. Cập nhật trạng thái mượn trả
        public bool CapNhatTrangThai(string maSach, string maDauSach, bool trangThai)
        {
            SachDAO sachDAO = new SachDAO();
            bool ketQua = sachDAO.CapNhatTrangThai(maSach, trangThai);
            if (ketQua)
            {
                dauSachBUS.CapNhatSoLuong(maDauSach);
                DocDSSach();
            }
            return ketQua;
        }

        // 6. Tìm kiếm Sách
        public List<SachDTO> timKiem(string maSach, string maDauSach)
        {
            return LayDanhSachTrongBoNho().Cast<SachDTO>().Where(s =>
                (string.IsNullOrWhiteSpace(maSach) || string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(maDauSach) || string.Equals(s.MaDauSach, maDauSach, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public List<SachDTO> TimKiemTuKhoa(string tuKhoa)
        {
            string keyword = (tuKhoa ?? string.Empty).Trim();
            return LayDanhSachTrongBoNho().Cast<SachDTO>().Where(s =>
                (s.MaSach ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (s.MaDauSach ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // 7. Tìm theo trạng thái
        public List<SachDTO> TimTheoTrangThai(bool? trangThai, string maDauSach)
        {
            return LayDanhSachTrongBoNho().Cast<SachDTO>().Where(s =>
                (string.IsNullOrWhiteSpace(maDauSach) || string.Equals(s.MaDauSach, maDauSach, StringComparison.OrdinalIgnoreCase)) &&
                (!trangThai.HasValue ||
                    (trangThai.Value && (s.TrangThai == "1" || s.TrangThai == "True" || s.TrangThai == "Đã cho mượn")) ||
                    (!trangThai.Value && (s.TrangThai == "0" || s.TrangThai == "False" || s.TrangThai == "Chưa cho mượn")))).ToList();
        }
    }
}
