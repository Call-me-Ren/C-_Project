#nullable disable
using System.Collections.Generic;
using QLThuVien.DAO;
using DTO;
using System;
using System.Linq;

namespace QLThuVien.BUS
{
    public class DauSachBUS
    {
        public DauSachBUS()
        {
        }

        // 1. Đọc danh sách Đầu sách (Nạp mới từ CSDL)
        public List<DauSachDTO> DocDSSach()
        {
            DauSachDAO dauSachDAO = new DauSachDAO();
            return dauSachDAO.DocDSSach();
        }

        // Kiểm tra trùng Mã Đầu Sách
        public bool KiemTraTrungMa(string maDauSach)
        {
            List<DauSachDTO> list = DocDSSach();
            foreach (var item in list)
            {
                if (item.MaDauSach.Equals(maDauSach, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // 2. Thêm Đầu sách -> Thêm xong tự gọi đồng bộ lại số lượng
        public void Them(DauSachDTO dauSach)
        {
            DauSachDAO dauSachDAO = new DauSachDAO();
            dauSachDAO.Them(dauSach);
            CapNhatSoLuong(dauSach.MaDauSach);
        }

        // 3. Xóa Đầu sách (Chặn nếu SLChoMuon > 0)
        public void Xoa(string maDauSach)
        {
            DauSachDAO dauSachDAO = new DauSachDAO();

            DauSachDTO dauSach = dauSachDAO.DocTheoMa(maDauSach);
            if (dauSach != null && dauSach.SLChoMuon > 0)
            {
                throw new Exception($"Đầu sách '{maDauSach}' hiện đang có {dauSach.SLChoMuon} cuốn sách được mượn, không được phép xóa!");
            }

            dauSachDAO.Xoa(maDauSach);
        }

        // 4. Sửa Đầu sách
        public void Sua(DauSachDTO dauSach, string maCu)
        {
            DauSachDAO dauSachDAO = new DauSachDAO();

            // Nếu đổi mã đầu sách mới thì phải kiểm tra xem mã mới có bị trùng với mã khác không
            if (!dauSach.MaDauSach.Equals(maCu, StringComparison.OrdinalIgnoreCase))
            {
                if (KiemTraTrungMa(dauSach.MaDauSach))
                {
                    throw new Exception($"Mã đầu sách mới '{dauSach.MaDauSach}' đã tồn tại trong hệ thống!");
                }
            }

            dauSachDAO.Sua(dauSach, maCu);
            CapNhatSoLuong(dauSach.MaDauSach);
        }

        // 5. Đồng bộ số lượng từ CSDL vào CSDL
        public void CapNhatSoLuong(string maDauSach)
        {
            if (string.IsNullOrEmpty(maDauSach)) return;
            DauSachDAO dauSachDAO = new DauSachDAO();
            dauSachDAO.DongBoSoLuong(maDauSach);
        }

        // 6. Tìm kiếm cơ bản (Lọc trên RAM)
        public List<DauSachDTO> TimKiem(string maDauSach, string tenSach)
        {
            List<DauSachDTO> ds = DocDSSach();
            List<DauSachDTO> dsKetQua = new List<DauSachDTO>();
            foreach (DauSachDTO s in ds)
            {
                if ((!string.IsNullOrEmpty(maDauSach) && s.MaDauSach.ToLower().Contains(maDauSach.ToLower())) ||
                    (!string.IsNullOrEmpty(tenSach) && s.TenSach.ToLower().Contains(tenSach.ToLower())))
                {
                    dsKetQua.Add(s);
                }
            }
            return dsKetQua;
        }

        // 7. Tìm kiếm nâng cao
        public List<DauSachDTO> TimKiemNangCao(string maDauSach, string tenSach, int? soLuong, int? namXB, string maTG, string maTL = null)
        {
            List<DauSachDTO> ds = DocDSSach();
            var ketQua = ds.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(maDauSach))
            {
                ketQua = ketQua.Where(s => !string.IsNullOrEmpty(s.MaDauSach) &&
                                           s.MaDauSach.ToLower().Contains(maDauSach.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(tenSach))
            {
                ketQua = ketQua.Where(s => !string.IsNullOrEmpty(s.TenSach) &&
                                           s.TenSach.ToLower().Contains(tenSach.Trim().ToLower()));
            }

            if (soLuong.HasValue)
            {
                ketQua = ketQua.Where(s => s.SoLuong == soLuong.Value);
            }

            if (namXB.HasValue)
            {
                ketQua = ketQua.Where(s => s.NamXB.HasValue && s.NamXB.Value == namXB.Value);
            }

            if (!string.IsNullOrWhiteSpace(maTG))
            {
                ketQua = ketQua.Where(s => !string.IsNullOrEmpty(s.MaTG) &&
                                           s.MaTG.Equals(maTG.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(maTL))
            {
                ketQua = ketQua.Where(s => !string.IsNullOrEmpty(s.MaTL) &&
                                           s.MaTL.Equals(maTL.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return ketQua.ToList();
        }
    }
}