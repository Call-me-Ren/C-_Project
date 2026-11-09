using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class PhieuPhatBUS
    {
        private readonly PhieuPhatDAO daoPhieu = new PhieuPhatDAO();
        private readonly ChiTietPhieuPhatDAO daoChiTiet = new ChiTietPhieuPhatDAO();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly QuyDinhPhatBUS busQuyDinh = new QuyDinhPhatBUS();
        private bool danhSachDaDoc;

        public ArrayList dsPhieuPhat_arr = new ArrayList();
        public ArrayList dsChiTiet_arr = new ArrayList();

        public List<LookupOptionDTO> LayDanhSachDocGiaLookup() => daoPhieu.LayDanhSachDocGiaLookup();

        public List<LookupOptionDTO> LayDanhSachPhieuMuonLookup() => daoPhieu.LayDanhSachPhieuMuonLookup();

        public List<SachMuonDTO> LaySachTheoPhieuMuon(string maPhieuMuon)
        {
            if (string.IsNullOrWhiteSpace(maPhieuMuon)) return new List<SachMuonDTO>();
            return daoChiTiet.LaySachTheoPhieuMuon(maPhieuMuon.Trim());
        }

        public List<QuyDinhPhatDTO> LayDanhSachQuyDinh(bool reload = true)
        {
            return busQuyDinh.LayDanhSach(reload);
        }

        public string TaoMaPhieuTiepTheo()
        {
            return SinhMaPhieuTiepTheo(daoPhieu.LayDanhSach().Select(pp => pp.MaPhieuPhat));
        }

        public static string SinhMaPhieuTiepTheo(IEnumerable<string> maHienCo)
        {
            HashSet<string> daSuDung = new HashSet<string>(maHienCo, StringComparer.OrdinalIgnoreCase);
            for (int so = 1; ; so++)
            {
                string ma = "PP" + so.ToString("D2", CultureInfo.InvariantCulture);
                if (!daSuDung.Contains(ma)) return ma;
            }
        }

        public List<PhieuPhatDTO> DocDS()
        {
            List<PhieuPhatDTO> list = daoPhieu.LayDanhSach();
            dsPhieuPhat_arr.Clear();
            foreach (PhieuPhatDTO pp in list)
            {
                dsPhieuPhat_arr.Add(pp);
            }

            DocTatCaChiTiet();
            danhSachDaDoc = true;
            return list;
        }

        public List<ChiTietPhieuPhatDTO> DocTatCaChiTiet()
        {
            List<ChiTietPhieuPhatDTO> list = daoChiTiet.LayTatCa();
            dsChiTiet_arr.Clear();
            foreach (ChiTietPhieuPhatDTO ct in list)
            {
                dsChiTiet_arr.Add(ct);
            }
            return list;
        }

        public List<PhieuPhatDTO> LayDanhSach(bool reload = false)
        {
            if (reload || !danhSachDaDoc)
            {
                return DocDS();
            }

            List<PhieuPhatDTO> result = new List<PhieuPhatDTO>();
            foreach (object item in dsPhieuPhat_arr)
            {
                if (item is PhieuPhatDTO pp) result.Add(pp);
            }
            return result;
        }

        public List<ChiTietPhieuPhatDTO> LayChiTietTheoPhieu(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) return new List<ChiTietPhieuPhatDTO>();

            string maTrim = maPhieuPhat.Trim();
            List<ChiTietPhieuPhatDTO> result = new List<ChiTietPhieuPhatDTO>();
            foreach (object item in dsChiTiet_arr)
            {
                if (item is ChiTietPhieuPhatDTO ct && Same(ct.MaPhieuPhat, maTrim)) result.Add(ct);
            }
            return result;
        }

        public bool KiemTraMaPhieuDuyNhat(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) return false;

            foreach (object item in dsPhieuPhat_arr)
            {
                if (item is PhieuPhatDTO pp && Same(pp.MaPhieuPhat, maPhieuPhat)) return false;
            }
            return true;
        }

        public bool KiemTraChiTietDuyNhat(string maPhieuPhat, string maSach, string maQD, int? boQuaMaCTPP = null)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat) || string.IsNullOrWhiteSpace(maSach) || string.IsNullOrWhiteSpace(maQD)) return false;

            foreach (object item in dsChiTiet_arr)
            {
                if (item is ChiTietPhieuPhatDTO ct &&
                    ct.MaCTPP != boQuaMaCTPP &&
                    Same(ct.MaPhieuPhat, maPhieuPhat) &&
                    Same(ct.MaSach, maSach) &&
                    Same(ct.MaQD, maQD))
                {
                    return false;
                }
            }
            return true;
        }

        public void KiemTraHopLePhieuPhat(PhieuPhatDTO pp, bool isThemMoi)
        {
            if (pp == null) throw new ArgumentNullException(nameof(pp), "Dữ liệu phiếu phạt không được null.");

            if (string.IsNullOrWhiteSpace(pp.MaPhieuPhat)) throw new ArgumentException("Mã phiếu phạt không được để trống.");
            if (pp.MaPhieuPhat.Trim().Length > 20) throw new ArgumentException("Mã phiếu phạt không được vượt quá 20 ký tự.");

            if (isThemMoi)
            {
                if (!KiemTraMaPhieuDuyNhat(pp.MaPhieuPhat))
                {
                    throw new InvalidOperationException($"Mã phiếu phạt '{pp.MaPhieuPhat.Trim()}' đã tồn tại.");
                }
                if (daoPhieu.KiemTraTonTai(pp.MaPhieuPhat.Trim()))
                {
                    throw new InvalidOperationException($"Mã phiếu phạt '{pp.MaPhieuPhat.Trim()}' đã tồn tại trong cơ sở dữ liệu.");
                }
            }

            if (string.IsNullOrWhiteSpace(pp.MaDocGia)) throw new ArgumentException("Mã độc giả không được để trống.");
            if (pp.MaDocGia.Trim().Length > 20) throw new ArgumentException("Mã độc giả không được vượt quá 20 ký tự.");
            if (!daoPhieu.KiemTraDocGiaTonTai(pp.MaDocGia.Trim()))
            {
                throw new InvalidOperationException($"Mã độc giả '{pp.MaDocGia.Trim()}' không tồn tại trong hệ thống.");
            }

            if (string.IsNullOrWhiteSpace(pp.MaNV)) throw new ArgumentException("Mã nhân viên lập phiếu không được để trống.");
            if (pp.MaNV.Trim().Length > 20) throw new ArgumentException("Mã nhân viên không được vượt quá 20 ký tự.");
            if (!busNhanVien.LayDanhSach(reload: true).Any(nv => Same(nv.MaNV, pp.MaNV)))
            {
                throw new InvalidOperationException($"Mã nhân viên '{pp.MaNV.Trim()}' không tồn tại trong hệ thống.");
            }

            if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon))
            {
                throw new ArgumentException("Mã phiếu mượn không được để trống.");
            }
            if (pp.MaPhieuMuon.Trim().Length > 20) throw new ArgumentException("Mã phiếu mượn không được vượt quá 20 ký tự.");
            if (!daoPhieu.KiemTraPhieuMuonTonTai(pp.MaPhieuMuon.Trim()))
            {
                throw new InvalidOperationException($"Mã phiếu mượn '{pp.MaPhieuMuon.Trim()}' không tồn tại trong hệ thống.");
            }
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(pp.MaPhieuMuon.Trim(), pp.MaDocGia.Trim()))
            {
                throw new InvalidOperationException($"Phiếu mượn '{pp.MaPhieuMuon.Trim()}' không thuộc độc giả '{pp.MaDocGia.Trim()}'.");
            }

            string trangThai = string.IsNullOrWhiteSpace(pp.TrangThai) ? "Chưa thanh toán" : pp.TrangThai.Trim();
            if (!Same(trangThai, "Chưa thanh toán") && !Same(trangThai, "Đã thanh toán"))
            {
                throw new ArgumentException("Trạng thái chỉ nhận 'Chưa thanh toán' hoặc 'Đã thanh toán'.");
            }
        }

        public void KiemTraHopLeChiTiet(ChiTietPhieuPhatDTO ct, bool isThemMoi)
        {
            EnsureLoaded();
            if (ct == null) throw new ArgumentNullException(nameof(ct), "Dữ liệu chi tiết phiếu phạt không được null.");
            if (string.IsNullOrWhiteSpace(ct.MaPhieuPhat)) throw new ArgumentException("Mã phiếu phạt không được để trống.");
            if (ct.MaPhieuPhat.Trim().Length > 20) throw new ArgumentException("Mã phiếu phạt không được vượt quá 20 ký tự.");
            if (string.IsNullOrWhiteSpace(ct.MaQD)) throw new ArgumentException("Mã quy định phạt không được để trống.");
            if (ct.MaQD.Trim().Length > 20) throw new ArgumentException("Mã quy định phạt không được vượt quá 20 ký tự.");
            if (string.IsNullOrWhiteSpace(ct.MaSach)) throw new ArgumentException("Mỗi vi phạm phải gắn với một cuốn sách.");
            if (ct.MaSach.Trim().Length > 20) throw new ArgumentException("Mã sách không được vượt quá 20 ký tự.");
            if (!string.IsNullOrWhiteSpace(ct.GhiChu) && ct.GhiChu.Trim().Length > 200)
            {
                throw new ArgumentException("Ghi chú không được vượt quá 200 ký tự.");
            }
            if (ct.DonGia < 0) throw new ArgumentException("Đơn giá không được âm.");
            if (ct.SoNgayTre.HasValue && ct.SoNgayTre.Value < 0) throw new ArgumentException("Số ngày trễ không được âm.");
            if (ct.TienPhat < 0) throw new ArgumentException("Tiền phạt không được âm.");

            if (!daoPhieu.KiemTraTonTai(ct.MaPhieuPhat.Trim()))
            {
                throw new InvalidOperationException($"Phiếu phạt '{ct.MaPhieuPhat.Trim()}' không tồn tại.");
            }
            PhieuPhatDTO? phieu = dsPhieuPhat_arr.Cast<object>().OfType<PhieuPhatDTO>()
                .FirstOrDefault(item => Same(item.MaPhieuPhat, ct.MaPhieuPhat));
            phieu ??= daoPhieu.LayTheoMa(ct.MaPhieuPhat.Trim());
            if (phieu == null) throw new InvalidOperationException($"Phiếu phạt '{ct.MaPhieuPhat.Trim()}' không tồn tại.");
            if (string.IsNullOrWhiteSpace(phieu.MaPhieuMuon))
                throw new InvalidOperationException($"Phiếu phạt '{phieu.MaPhieuPhat}' chưa gắn phiếu mượn.");
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(phieu.MaPhieuMuon, phieu.MaDocGia))
                throw new InvalidOperationException($"Phiếu mượn '{phieu.MaPhieuMuon}' không thuộc độc giả '{phieu.MaDocGia}'.");
            if (TimQuyDinh(ct.MaQD.Trim()) == null)
            {
                throw new InvalidOperationException($"Quy định phạt '{ct.MaQD.Trim()}' không tồn tại.");
            }
            if (!daoChiTiet.KiemTraSachTonTai(ct.MaSach.Trim()))
            {
                throw new InvalidOperationException($"Cuốn sách có mã '{ct.MaSach.Trim()}' không tồn tại.");
            }
            if (!daoChiTiet.LaySachTheoPhieuMuon(phieu.MaPhieuMuon).Any(s => Same(s.MaSach, ct.MaSach)))
            {
                throw new InvalidOperationException($"Cuốn sách '{ct.MaSach.Trim()}' không thuộc phiếu mượn '{phieu.MaPhieuMuon}'.");
            }

            if (isThemMoi)
            {
                if (!KiemTraChiTietDuyNhat(ct.MaPhieuPhat, ct.MaSach, ct.MaQD))
                {
                    throw new InvalidOperationException($"Quy định '{ct.MaQD}' đã được áp dụng cho sách '{ct.MaSach}' trên phiếu '{ct.MaPhieuPhat}'.");
                }
                if (daoChiTiet.KiemTraTrung(ct.MaPhieuPhat.Trim(), ct.MaSach.Trim(), ct.MaQD.Trim()))
                {
                    throw new InvalidOperationException($"Quy định '{ct.MaQD}' đã được áp dụng cho sách '{ct.MaSach}' trên phiếu '{ct.MaPhieuPhat}'.");
                }
            }
        }

        public static int TinhSoNgayTre(DateTime hanTra, DateTime ngayThucTra)
        {
            return Math.Max(0, (ngayThucTra.Date - hanTra.Date).Days);
        }

        public static decimal TinhTienPhat(QuyDinhPhatDTO quyDinh, int soNgayTre, decimal donGiaSach)
        {
            if (quyDinh == null) throw new ArgumentNullException(nameof(quyDinh));
            if (quyDinh.MucPhat < 0) throw new ArgumentException("Mức phạt không được âm.", nameof(quyDinh));
            if (soNgayTre < 0) throw new ArgumentException("Số ngày trễ không được âm.", nameof(soNgayTre));
            if (donGiaSach < 0) throw new ArgumentException("Đơn giá sách không được âm.", nameof(donGiaSach));

            decimal tienPhat = LoaiQuyDinh(quyDinh) switch
            {
                LoaiPhat.TreHan => soNgayTre * quyDinh.MucPhat,
                LoaiPhat.MatSach => donGiaSach * quyDinh.MucPhat,
                _ => quyDinh.MucPhat
            };

            return decimal.Round(tienPhat, 2, MidpointRounding.AwayFromZero);
        }

        public decimal TinhTienPhat(string maQD, int soNgayTre, decimal donGiaSach)
        {
            if (string.IsNullOrWhiteSpace(maQD)) return 0;
            QuyDinhPhatDTO? qd = TimQuyDinh(maQD.Trim());
            return qd == null ? 0 : TinhTienPhat(qd, soNgayTre, donGiaSach);
        }

        public ChiTietPhieuPhatDTO TinhChiTietPhat(string maPhieuPhat, string maQD, string? maSach, string? ghiChu = null)
        {
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new ArgumentException("Vui lòng chọn phiếu phạt.");
            EnsureLoaded();
            PhieuPhatDTO? pp = dsPhieuPhat_arr.Cast<object>().OfType<PhieuPhatDTO>()
                .FirstOrDefault(item => Same(item.MaPhieuPhat, maPhieuPhat));
            if (pp == null) throw new KeyNotFoundException($"Không tìm thấy phiếu phạt '{maPhieuPhat.Trim()}'.");

            if (string.IsNullOrWhiteSpace(maQD)) throw new ArgumentException("Vui lòng chọn quy định phạt.");
            QuyDinhPhatDTO? qd = TimQuyDinh(maQD.Trim());
            if (qd == null) throw new InvalidOperationException($"Quy định phạt '{maQD.Trim()}' không tồn tại.");

            if (string.IsNullOrWhiteSpace(maSach)) throw new ArgumentException("Vui lòng nhập mã cuốn sách vi phạm.");
            string sachTrim = maSach.Trim();
            if (!daoChiTiet.KiemTraSachTonTai(sachTrim)) throw new InvalidOperationException($"Cuốn sách '{sachTrim}' không tồn tại.");

            if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon))
                throw new InvalidOperationException($"Phiếu phạt '{pp.MaPhieuPhat}' chưa gắn phiếu mượn.");
            string maPhieuMuon = pp.MaPhieuMuon.Trim();
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(maPhieuMuon, pp.MaDocGia))
                throw new InvalidOperationException($"Phiếu mượn '{maPhieuMuon}' không thuộc độc giả '{pp.MaDocGia}'.");
            if (!daoChiTiet.KiemTraSachTonTai(sachTrim))
                throw new InvalidOperationException($"Cuốn sách '{sachTrim}' không tồn tại trong hệ thống.");
            SachMuonDTO? sachMuon = daoChiTiet.LaySachTheoPhieuMuon(maPhieuMuon)
                .FirstOrDefault(item => Same(item.MaSach, sachTrim));
            if (sachMuon == null)
                throw new InvalidOperationException($"Cuốn sách '{sachTrim}' không thuộc phiếu mượn '{maPhieuMuon}'.");

            LoaiPhat loaiPhat = LoaiQuyDinh(qd);
            decimal giaSach = daoChiTiet.LayDonGiaSach(sachTrim);
            int? soNgayTre = null;
            decimal donGia = giaSach;

            if (loaiPhat == LoaiPhat.TreHan)
            {
                DateTime ngayTinhPhat = sachMuon.NgayThucTra?.Date ?? DateTime.Today;
                soNgayTre = TinhSoNgayTre(sachMuon.HanTra, ngayTinhPhat);
                donGia = qd.MucPhat;
            }
            else if (loaiPhat == LoaiPhat.CoDinh)
            {
                donGia = qd.MucPhat;
            }

            decimal tienPhat = TinhTienPhat(qd, soNgayTre ?? 0, giaSach);
            string ghiChuMacDinh = loaiPhat switch
            {
                LoaiPhat.TreHan => soNgayTre > 0 ? $"Trả trễ {soNgayTre} ngày" : "Trả đúng hạn",
                LoaiPhat.MatSach => "Bồi thường sách bị mất",
                _ => qd.TenQD
            };

            return new ChiTietPhieuPhatDTO(
                pp.MaPhieuPhat,
                qd.MaQD,
                sachTrim,
                donGia,
                soNgayTre,
                string.IsNullOrWhiteSpace(ghiChu) ? ghiChuMacDinh : ghiChu.Trim(),
                tienPhat
            );
        }

        public decimal LayDonGiaSach(string? maSach)
        {
            if (string.IsNullOrWhiteSpace(maSach)) return 0;
            return daoChiTiet.LayDonGiaSach(maSach.Trim());
        }

        public (DateTime? HanTra, DateTime? NgayThucTra) LayThongTinMuonTra(string? maPhieuMuon, string? maSach)
        {
            if (string.IsNullOrWhiteSpace(maPhieuMuon) || string.IsNullOrWhiteSpace(maSach)) return (null, null);
            SachMuonDTO? sach = daoChiTiet.LaySachTheoPhieuMuon(maPhieuMuon.Trim())
                .FirstOrDefault(item => Same(item.MaSach, maSach));
            return sach == null ? (null, null) : (sach.HanTra, sach.NgayThucTra);
        }

        public bool ThemPhieu(PhieuPhatDTO pp)
        {
            EnsureLoaded();
            if (pp == null) throw new ArgumentNullException(nameof(pp), "Dữ liệu phiếu phạt không được null.");
            pp.MaPhieuPhat = TaoMaPhieuTiepTheo();
            KiemTraHopLePhieuPhat(pp, isThemMoi: true);

            PhieuPhatDTO normalized = new PhieuPhatDTO(
                pp.MaPhieuPhat.Trim(),
                pp.MaDocGia.Trim(),
                pp.MaPhieuMuon!.Trim(),
                pp.MaNV.Trim(),
                pp.NgayLap,
                NormalizeTrangThai(pp.TrangThai),
                0
            );

            bool success = daoPhieu.Them(normalized);
            if (success) dsPhieuPhat_arr.Insert(0, normalized);
            return success;
        }

        public bool SuaPhieu(PhieuPhatDTO pp)
        {
            EnsureLoaded();
            KiemTraHopLePhieuPhat(pp, isThemMoi: false);

            string maTrim = pp.MaPhieuPhat.Trim();
            if (!daoPhieu.KiemTraTonTai(maTrim)) throw new KeyNotFoundException($"Không tìm thấy phiếu phạt '{maTrim}' để cập nhật.");

            PhieuPhatDTO normalized = new PhieuPhatDTO(
                maTrim,
                pp.MaDocGia.Trim(),
                pp.MaPhieuMuon!.Trim(),
                pp.MaNV.Trim(),
                pp.NgayLap,
                NormalizeTrangThai(pp.TrangThai),
                0
            );

            List<ChiTietPhieuPhatDTO> chiTiet = daoChiTiet.LayDanhSachTheoPhieu(maTrim);
            ThayChiTietTrongCache(maTrim, chiTiet);
            PhieuPhatDTO? phieuHienTai = dsPhieuPhat_arr.Cast<object>().OfType<PhieuPhatDTO>()
                .FirstOrDefault(existing => Same(existing.MaPhieuPhat, maTrim));
            if (phieuHienTai != null && chiTiet.Count > 0 &&
                (!Same(phieuHienTai.MaDocGia, normalized.MaDocGia) || !Same(phieuHienTai.MaPhieuMuon, normalized.MaPhieuMuon)))
            {
                throw new InvalidOperationException("Hãy xóa các vi phạm trước khi đổi độc giả hoặc phiếu mượn của phiếu phạt.");
            }
            KiemTraLienKetChiTiet(normalized, chiTiet);
            normalized.TongTienPhat = TongTien(chiTiet);

            bool success = daoPhieu.Sua(normalized);
            if (success)
            {
                for (int i = 0; i < dsPhieuPhat_arr.Count; i++)
                {
                    if (dsPhieuPhat_arr[i] is PhieuPhatDTO current && Same(current.MaPhieuPhat, maTrim))
                    {
                        dsPhieuPhat_arr[i] = normalized;
                        break;
                    }
                }
            }
            return success;
        }

        public bool XoaPhieu(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new ArgumentException("Mã phiếu phạt cần xóa không được để trống.");

            string maTrim = maPhieuPhat.Trim();
            if (!daoPhieu.KiemTraTonTai(maTrim)) throw new KeyNotFoundException($"Không tìm thấy phiếu phạt '{maTrim}' để xóa.");

            bool success = daoPhieu.Xoa(maTrim);
            if (success)
            {
                RemoveWhere(dsPhieuPhat_arr, item => item is PhieuPhatDTO current && Same(current.MaPhieuPhat, maTrim));
                RemoveWhere(dsChiTiet_arr, item => item is ChiTietPhieuPhatDTO ct && Same(ct.MaPhieuPhat, maTrim));
            }
            return success;
        }

        public bool ThemChiTiet(ChiTietPhieuPhatDTO ct)
        {
            EnsureLoaded();
            KiemTraHopLeChiTiet(ct, isThemMoi: true);

            ChiTietPhieuPhatDTO normalized = TinhChiTietPhat(ct.MaPhieuPhat.Trim(), ct.MaQD.Trim(), ct.MaSach, ct.GhiChu);
            KiemTraHopLeChiTiet(normalized, isThemMoi: true);

            bool success = daoChiTiet.Them(normalized);
            if (success)
            {
                dsChiTiet_arr.Add(normalized);
                CapNhatTongTienPhieu(normalized.MaPhieuPhat);
            }
            return success;
        }

        public bool SuaChiTiet(int maCTPP, ChiTietPhieuPhatDTO ct)
        {
            EnsureLoaded();
            if (ct == null) throw new ArgumentNullException(nameof(ct), "Dữ liệu chi tiết phiếu phạt không được null.");
            if (maCTPP <= 0) throw new ArgumentException("Vui lòng chọn vi phạm cần sửa.");

            ChiTietPhieuPhatDTO? existing = TimChiTietTheoId(maCTPP) ?? daoChiTiet.LayTheoId(maCTPP);
            if (existing == null) throw new KeyNotFoundException("Không tìm thấy vi phạm cần sửa.");

            ChiTietPhieuPhatDTO input = new ChiTietPhieuPhatDTO(
                existing.MaPhieuPhat,
                ct.MaQD,
                ct.MaSach,
                ct.DonGia,
                ct.SoNgayTre,
                ct.GhiChu,
                ct.TienPhat,
                maCTPP
            );
            KiemTraHopLeChiTiet(input, isThemMoi: false);

            ChiTietPhieuPhatDTO normalized = TinhChiTietPhat(existing.MaPhieuPhat, input.MaQD.Trim(), input.MaSach, input.GhiChu);
            normalized.MaCTPP = maCTPP;
            if (!KiemTraChiTietDuyNhat(normalized.MaPhieuPhat, normalized.MaSach!, normalized.MaQD, maCTPP) ||
                daoChiTiet.KiemTraTrung(normalized.MaPhieuPhat, normalized.MaSach!, normalized.MaQD, maCTPP))
            {
                throw new InvalidOperationException($"Quy định '{normalized.MaQD}' đã được áp dụng cho sách '{normalized.MaSach}' trên phiếu '{normalized.MaPhieuPhat}'.");
            }

            bool success = daoChiTiet.Sua(normalized);
            if (success)
            {
                for (int i = 0; i < dsChiTiet_arr.Count; i++)
                {
                    if (dsChiTiet_arr[i] is ChiTietPhieuPhatDTO current && current.MaCTPP == maCTPP)
                    {
                        dsChiTiet_arr[i] = normalized;
                        break;
                    }
                }
                CapNhatTongTienPhieu(normalized.MaPhieuPhat);
            }
            return success;
        }

        public bool XoaChiTiet(int maCTPP)
        {
            EnsureLoaded();
            if (maCTPP <= 0) throw new ArgumentException("Mã vi phạm không hợp lệ.");
            ChiTietPhieuPhatDTO? existing = TimChiTietTheoId(maCTPP) ?? daoChiTiet.LayTheoId(maCTPP);
            if (existing == null) throw new KeyNotFoundException("Không tìm thấy vi phạm cần xóa.");

            bool success = daoChiTiet.Xoa(maCTPP);
            if (success)
            {
                RemoveWhere(dsChiTiet_arr, item => item is ChiTietPhieuPhatDTO current &&
                    current.MaCTPP == maCTPP);
                CapNhatTongTienPhieu(existing.MaPhieuPhat);
            }
            return success;
        }

        public decimal CapNhatTongTienPhieu(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new ArgumentException("Mã phiếu phạt không được để trống.");

            string phieuTrim = maPhieuPhat.Trim();
            List<ChiTietPhieuPhatDTO> chiTiet = daoChiTiet.LayDanhSachTheoPhieu(phieuTrim);
            ThayChiTietTrongCache(phieuTrim, chiTiet);
            decimal tongTien = TongTien(chiTiet);
            if (!daoPhieu.CapNhatTongTien(phieuTrim, tongTien))
            {
                throw new KeyNotFoundException($"Không tìm thấy phiếu phạt '{phieuTrim}' để cập nhật tổng tiền.");
            }

            foreach (object item in dsPhieuPhat_arr)
            {
                if (item is PhieuPhatDTO pp && Same(pp.MaPhieuPhat, phieuTrim))
                {
                    pp.TongTienPhat = tongTien;
                    break;
                }
            }
            return tongTien;
        }

        public List<PhieuPhatDTO> TimKiemPhieu(string tuKhoa)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(tuKhoa)) return LayDanhSach();

            string key = tuKhoa.Trim();
            List<PhieuPhatDTO> result = new List<PhieuPhatDTO>();
            foreach (object item in dsPhieuPhat_arr)
            {
                if (item is not PhieuPhatDTO pp) continue;

                bool match = pp.MaPhieuPhat.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                             pp.MaDocGia.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                             (pp.MaPhieuMuon != null && pp.MaPhieuMuon.Contains(key, StringComparison.OrdinalIgnoreCase)) ||
                             pp.MaNV.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                             pp.TrangThai.Contains(key, StringComparison.OrdinalIgnoreCase);
                if (match) result.Add(pp);
            }
            return result;
        }

        private void KiemTraLienKetChiTiet(PhieuPhatDTO pp, List<ChiTietPhieuPhatDTO> chiTiet)
        {
            foreach (ChiTietPhieuPhatDTO ct in chiTiet)
            {
                QuyDinhPhatDTO? qd = TimQuyDinh(ct.MaQD);
                if (qd == null) throw new InvalidOperationException($"Quy định phạt '{ct.MaQD}' không còn tồn tại.");

                if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon) || string.IsNullOrWhiteSpace(ct.MaSach) ||
                    !daoChiTiet.LaySachTheoPhieuMuon(pp.MaPhieuMuon).Any(s => Same(s.MaSach, ct.MaSach)))
                {
                    throw new InvalidOperationException($"Không thể lưu vì sách '{ct.MaSach}' trong chi tiết không thuộc phiếu mượn '{pp.MaPhieuMuon}'.");
                }
            }
        }

        private QuyDinhPhatDTO? TimQuyDinh(string maQD)
        {
            return busQuyDinh.LayDanhSach().FirstOrDefault(qd => Same(qd.MaQD, maQD));
        }

        private static LoaiPhat LoaiQuyDinh(QuyDinhPhatDTO qd)
        {
            string noiDung = BoDau(string.Join(" ", qd.TenQD, qd.LoaiPhat, qd.CachTinh, qd.DonViTinh));
            if (noiDung.Contains("ngay") || noiDung.Contains("tre han") || noiDung.Contains("qua han")) return LoaiPhat.TreHan;
            if (noiDung.Contains("gia bia") || noiDung.Contains("he so") || noiDung.Contains("mat sach") || noiDung.Contains("lam mat")) return LoaiPhat.MatSach;
            return LoaiPhat.CoDinh;
        }

        private static string BoDau(string value)
        {
            string normalized = value.ToLowerInvariant().Replace('đ', 'd').Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) builder.Append(c);
            }
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string NormalizeTrangThai(string? trangThai)
        {
            return string.IsNullOrWhiteSpace(trangThai) ? "Chưa thanh toán" : trangThai.Trim();
        }

        private static decimal TongTien(IEnumerable<ChiTietPhieuPhatDTO> chiTiet)
        {
            return decimal.Round(chiTiet.Sum(ct => ct.TienPhat), 2, MidpointRounding.AwayFromZero);
        }

        private ChiTietPhieuPhatDTO? TimChiTietTheoId(int maCTPP)
        {
            foreach (object item in dsChiTiet_arr)
            {
                if (item is ChiTietPhieuPhatDTO ct && ct.MaCTPP == maCTPP) return ct;
            }
            return null;
        }

        private void ThayChiTietTrongCache(string maPhieuPhat, List<ChiTietPhieuPhatDTO> chiTiet)
        {
            RemoveWhere(dsChiTiet_arr, item => item is ChiTietPhieuPhatDTO ct && Same(ct.MaPhieuPhat, maPhieuPhat));
            foreach (ChiTietPhieuPhatDTO ct in chiTiet) dsChiTiet_arr.Add(ct);
        }

        private void EnsureLoaded()
        {
            if (!danhSachDaDoc) DocDS();
        }

        private static bool Same(string? left, string? right)
        {
            return string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static void RemoveWhere(ArrayList list, Predicate<object?> predicate)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i])) list.RemoveAt(i);
            }
        }

        private enum LoaiPhat
        {
            TreHan,
            CoDinh,
            MatSach
        }
    }
}
