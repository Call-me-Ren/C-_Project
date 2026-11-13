using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Lớp BUS xử lý nghiệp vụ Mượn - Trả sách:
    /// - Quản lý danh sách trong RAM bằng ArrayList.
    /// - Kiểm tra số sách mượn tối đa theo cấu hình bảng ThamSo.
    /// - Tự động tính hạn trả dựa trên số ngày mượn tối đa từ ThamSo.
    /// - Nghiệp vụ cập nhật tồn kho DauSach (cộng/trừ SLConLai và SLChoMuon).
    /// - Nghiệp vụ trả sách và phát hiện quá hạn trả (NgayThucTra > HanTra).
    /// </summary>
    public class PhieuMuonBUS
    {
        private static readonly Regex MaPhieuMuonPattern = new(@"^PM(\d+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private readonly PhieuMuonDAO pmDAO = new PhieuMuonDAO();
        private readonly ChiTietPhieuMuonDAO ctDAO = new ChiTietPhieuMuonDAO();

        // Thuộc tính ArrayList lưu danh sách phiếu mượn trong RAM
        public ArrayList dsPhieuMuon_arr = new ArrayList();

        // 1. ĐỌC DANH SÁCH TẤT CẢ PHIẾU MƯỢN VÀ LƯU VÀO ARRAYLIST
        public ArrayList DocDSPhieuMuon()
        {
            List<PhieuMuonDTO> list = pmDAO.DocDSPhieuMuon();

            dsPhieuMuon_arr.Clear();
            foreach (PhieuMuonDTO pm in list)
            {
                dsPhieuMuon_arr.Add(pm);
            }

            return dsPhieuMuon_arr;
        }

        // HÀM TÌM PHIẾU THEO MÃ TRÊN ARRAYLIST
        public PhieuMuonDTO? TimTheoMa(string maPM)
        {
            foreach (PhieuMuonDTO item in dsPhieuMuon_arr)
            {
                if (item.MaPM.Trim().Equals(maPM.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        // 2. TỰ ĐỘNG SINH MÃ PHIẾU MƯỢN TIẾP THEO (PM01, PM02...)
        public string TaoMaPhieuTiepTheo()
        {
            int max = 0;
            foreach (PhieuMuonDTO pm in dsPhieuMuon_arr)
            {
                Match match = MaPhieuMuonPattern.Match(pm.MaPM ?? string.Empty);
                if (match.Success && int.TryParse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out int so))
                {
                    max = Math.Max(max, so);
                }
            }
            return "PM" + (max + 1).ToString("D2", CultureInfo.InvariantCulture);
        }

        // 3. ĐỌC THAM SỐ CẤU HÌNH HỆ THỐNG
        public int LaySoSachMuonToiDa()
        {
            string val = ThamSoBUS.LayGiaTriTheoMa("SO_SACH_MUON_TOI_DA", "5");
            return int.TryParse(val, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result) ? result : 5;
        }

        public int LaySoNgayMuonToiDa()
        {
            string val = ThamSoBUS.LayGiaTriTheoMa("SO_NGAY_MUON_TOI_DA", "7");
            return int.TryParse(val, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result) ? result : 7;
        }

        // 4. LẤY CHI TIẾT SÁCH MƯỢN CỦA 1 PHIẾU
        public List<ChiTietPhieuMuonDTO> LayChiTietTheoPhieu(string maPM)
        {
            return ctDAO.LayChiTietTheoPhieu(maPM);
        }

        // 5. LẤY DANH SÁCH CUỐN SÁCH CÓ SẴN ĐỂ MƯỢN
        public DataTable LayDanhSachSachCoSan()
        {
            return ctDAO.LayDanhSachSachCoSan();
        }

        // 6. TẠO PHIẾU MƯỢN MỚI (CHỈ TẠO PHẦN MASTER)
        public bool ThemPhieuMuon(PhieuMuonDTO pm, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(pm.MaPM))
            {
                error = "Mã phiếu mượn không được để trống!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(pm.MaDocGia))
            {
                error = "Vui lòng chọn độc giả!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(pm.MaNV))
            {
                error = "Vui lòng chọn nhân viên lập phiếu!";
                return false;
            }

            if (TimTheoMa(pm.MaPM) != null)
            {
                error = $"Mã phiếu mượn '{pm.MaPM}' đã tồn tại!";
                return false;
            }

            if (pm.HanTra.Date < pm.NgayMuon.Date)
            {
                error = "Hạn trả sách không được nhỏ hơn ngày mượn!";
                return false;
            }

            bool result = pmDAO.ThemPhieuMuon(pm);
            if (result)
            {
                dsPhieuMuon_arr.Insert(0, pm);
            }
            else
            {
                error = "Thêm phiếu mượn vào cơ sở dữ liệu thất bại!";
            }

            return result;
        }

        // 7. THÊM CUỐN SÁCH VÀO PHIẾU MƯỢN (NGHIỆP VỤ CỘNG TRỪ TỒN KHO)
        public bool ThemChiTietMuon(ChiTietPhieuMuonDTO ct, out string error)
        {
            error = string.Empty;

            PhieuMuonDTO? pm = TimTheoMa(ct.MaPhieuMuon);
            if (pm == null)
            {
                error = "Không tìm thấy thông tin phiếu mượn!";
                return false;
            }

            // KIỂM TRA QUY ĐỊNH 1: Độc giả không được mượn quá số lượng tối đa (bảng ThamSo)
            int soSachDangMuon = pmDAO.DemSoSachDangMuonCuaDocGia(pm.MaDocGia);
            int soSachToiDa = LaySoSachMuonToiDa();

            if (soSachDangMuon >= soSachToiDa)
            {
                error = $"Độc giả đang mượn {soSachDangMuon} cuốn sách chưa trả. Theo quy định chỉ được mượn tối đa {soSachToiDa} cuốn!";
                return false;
            }

            // KIỂM TRA QUY ĐỊNH 2: Cuốn sách có đang bị ai mượn chưa trả không?
            if (ctDAO.KiemTraSachDangMuon(ct.MaSach))
            {
                error = $"Cuốn sách '{ct.MaSach}' hiện đang có người mượn và chưa được trả lại thư viện!";
                return false;
            }

            // Thực hiện ghi xuống CSDL
            bool result = ctDAO.ThemChiTiet(ct);
            if (result)
            {
                // CẬP NHẬT TỒN KHO: Giảm SLConLai đi 1, tăng SLChoMuon lên 1 ở DauSach
                ctDAO.CapNhatTonKhoKhiMuon(ct.MaSach);

                // Cập nhật lại số lượng sách hiển thị trên phiếu mượn ở RAM
                pm.TongSoSach++;
                pm.SoSachChuaTra++;
            }
            else
            {
                error = "Thêm chi tiết mượn sách thất bại! Có thể cuốn sách này đã có trong phiếu mượn.";
            }

            return result;
        }

        // 8. NGHIỆP VỤ TRẢ SÁCH (HOÀN TRẢ TỒN KHO + KIỂM TRA QUÁ HẠN)
        public bool TraSach(string maPM, string maSach, DateTime ngayThucTra, string? tinhTrang, out int soNgayTre, out string message)
        {
            soNgayTre = 0;
            message = string.Empty;

            PhieuMuonDTO? pm = TimTheoMa(maPM);
            if (pm == null)
            {
                message = "Không tìm thấy phiếu mượn tương ứng!";
                return false;
            }

            // Kiểm tra trễ hạn: NgayThucTra > HanTra
            if (ngayThucTra.Date > pm.HanTra.Date)
            {
                soNgayTre = (ngayThucTra.Date - pm.HanTra.Date).Days;
            }

            // Ghi nhận trả sách xuống CSDL
            bool result = ctDAO.CapNhatTraSach(maPM, maSach, ngayThucTra, tinhTrang);
            if (result)
            {
                // CẬP NHẬT TỒN KHO: Tăng SLConLai lên 1, giảm SLChoMuon đi 1 ở DauSach
                ctDAO.CapNhatTonKhoKhiTra(maSach);

                // Cập nhật số sách chưa trả ở RAM
                if (pm.SoSachChuaTra > 0)
                {
                    pm.SoSachChuaTra--;
                }

                if (soNgayTre > 0)
                {
                    message = $"Trả sách thành công!\nCẢNH BÁO: Cuốn sách này đã TRỄ HẠN {soNgayTre} ngày (Hạn trả: {pm.HanTra:dd/MM/yyyy}).\nVui lòng kiểm tra và lập Phiếu phạt tương ứng!";
                }
                else
                {
                    message = "Trả sách thành công đúng hạn!";
                }
            }
            else
            {
                message = "Cập nhật trạng thái trả sách thất bại!";
            }

            return result;
        }

        // 9. XÓA PHIẾU MƯỢN (CHỐNG LỦNG CSDL)
        public bool XoaPhieuMuon(string maPM, out string error)
        {
            error = string.Empty;

            PhieuMuonDTO? pm = TimTheoMa(maPM);
            if (pm != null && pm.SoSachChuaTra > 0)
            {
                error = $"Không thể xóa phiếu mượn '{maPM}' vì vẫn còn {pm.SoSachChuaTra} cuốn sách chưa được trả!";
                return false;
            }

            if (pmDAO.KiemTraDangSuDung(maPM))
            {
                error = $"Không thể xóa phiếu mượn '{maPM}' vì đã phát sinh vi phạm và lập Phiếu phạt liên quan!";
                return false;
            }

            bool result = pmDAO.XoaPhieuMuon(maPM);
            if (result)
            {
                if (pm != null)
                {
                    dsPhieuMuon_arr.Remove(pm);
                }
            }
            else
            {
                error = "Xóa phiếu mượn thất bại!";
            }

            return result;
        }

        // 10. TÌM KIẾM PHIẾU MƯỢN TRÊN ARRAYLIST (RAM)
        public ArrayList TimKiem(string tuKhoa)
        {
            ArrayList ketQua = new ArrayList();
            string keyword = tuKhoa.Trim().ToLower();

            foreach (PhieuMuonDTO item in dsPhieuMuon_arr)
            {
                if (item.MaPM.ToLower().Contains(keyword) ||
                    item.MaDocGia.ToLower().Contains(keyword) ||
                    (!string.IsNullOrEmpty(item.TenDocGia) && item.TenDocGia.ToLower().Contains(keyword)) ||
                    item.MaNV.ToLower().Contains(keyword) ||
                    (!string.IsNullOrEmpty(item.TenNV) && item.TenNV.ToLower().Contains(keyword)))
                {
                    ketQua.Add(item);
                }
            }

            return ketQua;
        }
    }
}
