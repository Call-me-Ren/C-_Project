#nullable disable
using System.Collections.Generic;
using System.Collections;
using QLThuVien.DAO;
using QLThuVien.DTO;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;

namespace QLThuVien.BUS
{
    public class DauSachBUS
    {
        private readonly ArrayList danhSachDauSach = new ArrayList();
        private bool daNapDanhSach;

        public DauSachBUS()
        {
        }

        // 1. Đọc danh sách Đầu sách (Nạp mới từ CSDL)
        public List<DauSachDTO> DocDSSach()
        {
            DauSachDAO dauSachDAO = new DauSachDAO();
            List<DauSachDTO> danhSach = dauSachDAO.DocDSSach();
            danhSachDauSach.Clear();
            danhSachDauSach.AddRange(danhSach);
            daNapDanhSach = true;
            return danhSach;
        }

        private ArrayList LayDanhSachTrongBoNho()
        {
            if (!daNapDanhSach) DocDSSach();
            return danhSachDauSach;
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

        // Chỉ thêm mã đầu sách mới; mã trùng và dòng không hợp lệ được báo lại, không ghi đè dữ liệu cũ.
        public string NhapExcel(string duongDan, out int soLuongThem, out int soLuongBoQua, out List<string> loi)
        {
            soLuongThem = 0;
            soLuongBoQua = 0;
            loi = new List<string>();
            List<string[]> cacDong = DauSachExcelService.Doc(duongDan);
            if (cacDong.Count == 0) throw new InvalidDataException("File Excel không có dữ liệu.");

            Dictionary<string, int> cot = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < cacDong[0].Length; i++)
            {
                string tenCot = ChuanHoaTieuDeExcel(cacDong[0][i]);
                if (!string.IsNullOrEmpty(tenCot)) cot[tenCot] = i;
            }

            string[] tenThayThe = { "madausach", "tensach", "matacgia", "matheloai", "manxb", "namxuatban", "dongia" };
            // Chấp nhận cả tên thuộc tính trong code để tiện dùng file mẫu kỹ thuật.
            string[][] alias =
            {
                new[] { "madausach" }, new[] { "tensach" }, new[] { "matacgia", "matg" },
                new[] { "matheloai", "matl" }, new[] { "manxb" }, new[] { "namxuatban", "namxb" }, new[] { "dongia" }
            };
            int[] viTri = new int[tenThayThe.Length];
            for (int i = 0; i < alias.Length; i++)
            {
                int found = -1;
                foreach (string ten in alias[i]) if (cot.TryGetValue(ten, out found)) break;
                if (found < 0) throw new InvalidDataException($"Thiếu cột bắt buộc '{tenThayThe[i]}' trong file Excel.");
                viTri[i] = found;
            }

            HashSet<string> maDaCo = new HashSet<string>(DocDSSach().Select(x => x.MaDauSach), StringComparer.OrdinalIgnoreCase);
            HashSet<string> maTacGia = new HashSet<string>(new TacGiaBUS().DocDSSach().Select(x => x.MaTG), StringComparer.OrdinalIgnoreCase);
            HashSet<string> maTheLoai = new HashSet<string>(new TheLoaiBUS().DocDSTL().Select(x => x.MaTL), StringComparer.OrdinalIgnoreCase);
            HashSet<string> maNXB = new HashSet<string>(new NXBBUS().DocDSNXB().Select(x => x.MaNXB), StringComparer.OrdinalIgnoreCase);
            DauSachDAO dao = new DauSachDAO();

            for (int i = 1; i < cacDong.Count; i++)
            {
                string[] dong = cacDong[i];
                if (dong.All(string.IsNullOrWhiteSpace)) continue;
                int soDongExcel = i + 1;
                try
                {
                    string ma = LayGiaTri(dong, viTri[0]);
                    string tenSach = LayGiaTri(dong, viTri[1]);
                    string maTG = LayGiaTri(dong, viTri[2]);
                    string maTL = LayGiaTri(dong, viTri[3]);
                    string maNXBValue = LayGiaTri(dong, viTri[4]);
                    string namText = LayGiaTri(dong, viTri[5]);
                    string giaText = LayGiaTri(dong, viTri[6]);

                    if (string.IsNullOrWhiteSpace(ma) || ma.Length > 20) throw new FormatException("Mã đầu sách bắt buộc và tối đa 20 ký tự.");
                    if (string.IsNullOrWhiteSpace(tenSach) || tenSach.Length > 200) throw new FormatException("Tên sách bắt buộc và tối đa 200 ký tự.");
                    if (!maTacGia.Contains(maTG)) throw new FormatException($"Mã tác giả '{maTG}' không tồn tại.");
                    if (!maTheLoai.Contains(maTL)) throw new FormatException($"Mã thể loại '{maTL}' không tồn tại.");
                    if (!maNXB.Contains(maNXBValue)) throw new FormatException($"Mã nhà xuất bản '{maNXBValue}' không tồn tại.");
                    int? namXB = null;
                    if (!string.IsNullOrWhiteSpace(namText))
                    {
                        if (!int.TryParse(namText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int nam) || nam < 0 || nam > DateTime.Now.Year)
                            throw new FormatException("Năm xuất bản không hợp lệ.");
                        namXB = nam;
                    }
                    decimal donGia = 0;
                    if (!string.IsNullOrWhiteSpace(giaText) &&
                        (!decimal.TryParse(giaText, NumberStyles.Number, CultureInfo.InvariantCulture, out donGia) ||
                         donGia < 0 || donGia > 9999999999999999.99m || decimal.Round(donGia, 2) != donGia))
                        throw new FormatException("Đơn giá phải là số không âm và có tối đa 2 chữ số thập phân.");
                    if (!maDaCo.Add(ma))
                    {
                        soLuongBoQua++;
                        loi.Add($"Dòng {soDongExcel}: mã '{ma}' đã tồn tại hoặc bị lặp trong file, đã bỏ qua.");
                        continue;
                    }

                    dao.Them(new DauSachDTO
                    {
                        MaDauSach = ma,
                        TenSach = tenSach,
                        MaTG = maTG,
                        MaTL = maTL,
                        MaNXB = maNXBValue,
                        NamXB = namXB,
                        DonGia = donGia
                    });
                    soLuongThem++;
                }
                catch (Exception ex)
                {
                    soLuongBoQua++;
                    loi.Add($"Dòng {soDongExcel}: {ex.Message}");
                }
            }

            return $"Đã thêm {soLuongThem} đầu sách mới; bỏ qua {soLuongBoQua} dòng.";
        }

        private static string LayGiaTri(string[] dong, int viTri)
        {
            return viTri < dong.Length ? (dong[viTri] ?? string.Empty).Trim() : string.Empty;
        }

        private static string ChuanHoaTieuDeExcel(string giaTri)
        {
            string khongDau = (giaTri ?? string.Empty).Trim().Normalize(NormalizationForm.FormD);
            StringBuilder ketQua = new StringBuilder();
            foreach (char kyTu in khongDau)
                if (CharUnicodeInfo.GetUnicodeCategory(kyTu) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(kyTu))
                    ketQua.Append(char.ToLowerInvariant(kyTu));
            return ketQua.ToString().Replace("đ", "d");
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
            List<DauSachDTO> ds = new List<DauSachDTO>();
            foreach (DauSachDTO item in LayDanhSachTrongBoNho()) ds.Add(item);
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
            List<DauSachDTO> ds = new List<DauSachDTO>();
            foreach (DauSachDTO item in LayDanhSachTrongBoNho()) ds.Add(item);
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

        public List<DauSachDTO> TimKiemNangCao(string maDauSach, string tenSach, int? soLuong, int? namXB,
            string maTG, string maTL, decimal? tuGia, decimal? denGia)
        {
            IEnumerable<DauSachDTO> ketQua = TimKiemNangCao(maDauSach, tenSach, soLuong, namXB, maTG, maTL);
            if (tuGia.HasValue) ketQua = ketQua.Where(s => s.DonGia >= tuGia.Value);
            if (denGia.HasValue) ketQua = ketQua.Where(s => s.DonGia <= denGia.Value);
            return ketQua.ToList();
        }

        public List<DauSachDTO> TimKiemNangCao(string maDauSach, string tenSach, int? soLuong,
            string toanTuSoLuong, string maTG, string maTL, int? namXB, int? tuNam, int? denNam,
            string toanTuNam, string toanTuNamDen, decimal? tuGia, string toanTuGia,
            decimal? denGia, string toanTuGiaDen, string cachKetHop = "AND")
        {
            List<Func<DauSachDTO, bool>> dieuKien = new List<Func<DauSachDTO, bool>>();
            if (!string.IsNullOrWhiteSpace(maDauSach)) dieuKien.Add(s => (s.MaDauSach ?? "").IndexOf(maDauSach.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(tenSach)) dieuKien.Add(s => (s.TenSach ?? "").IndexOf(tenSach.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(maTG)) dieuKien.Add(s => string.Equals(s.MaTG, maTG.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(maTL)) dieuKien.Add(s => string.Equals(s.MaTL, maTL.Trim(), StringComparison.OrdinalIgnoreCase));
            if (soLuong.HasValue) dieuKien.Add(s => SoSanh(s.SoLuong, soLuong.Value, toanTuSoLuong));
            if (namXB.HasValue) dieuKien.Add(s => s.NamXB.HasValue && s.NamXB.Value == namXB.Value);
            if (tuNam.HasValue) dieuKien.Add(s => s.NamXB.HasValue && SoSanh(s.NamXB.Value, tuNam.Value, toanTuNam));
            if (denNam.HasValue) dieuKien.Add(s => s.NamXB.HasValue && SoSanh(s.NamXB.Value, denNam.Value, toanTuNamDen));
            if (tuGia.HasValue) dieuKien.Add(s => SoSanh(s.DonGia, tuGia.Value, toanTuGia));
            if (denGia.HasValue) dieuKien.Add(s => SoSanh(s.DonGia, denGia.Value, toanTuGiaDen));
            IEnumerable<DauSachDTO> danhSach = LayDanhSachTrongBoNho().Cast<DauSachDTO>();
            bool dungOr = string.Equals(cachKetHop, "OR", StringComparison.OrdinalIgnoreCase);
            return danhSach.Where(s => dieuKien.Count == 0 || (dungOr ? dieuKien.Any(d => d(s)) : dieuKien.All(d => d(s)))).ToList();
        }

        private static bool SoSanh<T>(T giaTri, T moc, string toanTu) where T : IComparable<T>
        {
            int ketQua = giaTri.CompareTo(moc);
            switch (toanTu)
            {
                case ">": return ketQua > 0;
                case "<": return ketQua < 0;
                case ">=": return ketQua >= 0;
                case "<=": return ketQua <= 0;
                case "<>": return ketQua != 0;
                default: return ketQua == 0;
            }
        }
    }
}
