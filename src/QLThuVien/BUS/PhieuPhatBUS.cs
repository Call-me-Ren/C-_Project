using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>Lớp BUS kiểm tra và xử lý phiếu phạt, tính vi phạm, đồng bộ tổng tiền và phối hợp với các lớp DAO.</summary>
    public class PhieuPhatBUS
    {
        private readonly PhieuPhatDAO daoPhieu = new PhieuPhatDAO();
        private readonly ChiTietPhieuPhatDAO daoChiTiet = new ChiTietPhieuPhatDAO();
        private readonly NhanVienBUS busNhanVien = new NhanVienBUS();
        private readonly QuyDinhPhatBUS busQuyDinh = new QuyDinhPhatBUS();
        private bool danhSachDaDoc;

        /// <summary>Bộ nhớ đệm các phiếu phạt đã đọc để lớp GUI hiển thị và tìm kiếm.</summary>
        public ArrayList dsPhieuPhat_arr = new ArrayList();
        /// <summary>Bộ nhớ đệm các vi phạm đã đọc để lớp GUI lấy chi tiết theo phiếu.</summary>
        public ArrayList dsChiTiet_arr = new ArrayList();

        /// <summary>Lấy lựa chọn độc giả từ DAO để lớp GUI lập phiếu.</summary>
        /// <returns>Các lựa chọn gồm mã và nhãn độc giả; rỗng nếu chưa có dữ liệu.</returns>
        /// <exception cref="SqlException">Truy vấn danh sách độc giả thất bại.</exception>
        public List<LookupOptionDTO> LayDanhSachDocGiaLookup() => daoPhieu.LayDanhSachDocGiaLookup();

        /// <summary>Lấy lựa chọn phiếu mượn kèm mã độc giả và hạn trả.</summary>
        /// <returns>Các lựa chọn phiếu mượn; rỗng nếu chưa có phiếu.</returns>
        /// <exception cref="SqlException">Truy vấn danh sách phiếu mượn thất bại.</exception>
        public List<LookupOptionDTO> LayDanhSachPhieuMuonLookup() => daoPhieu.LayDanhSachPhieuMuonLookup();

        /// <summary>Lấy các cuốn sách thuộc phiếu mượn để lập vi phạm.</summary>
        /// <param name="maPhieuMuon">Mã phiếu mượn cần tra cứu; rỗng sẽ không truy vấn.</param>
        /// <returns>Các cuốn sách kèm hạn trả và ngày thực trả; rỗng nếu mã trống hoặc phiếu không có sách.</returns>
        /// <exception cref="SqlException">Truy vấn sách thuộc phiếu mượn thất bại.</exception>
        public List<SachMuonDTO> LaySachTheoPhieuMuon(string maPhieuMuon)
        {
            if (string.IsNullOrWhiteSpace(maPhieuMuon)) return new List<SachMuonDTO>();
            return daoChiTiet.LaySachTheoPhieuMuon(maPhieuMuon.Trim());
        }

        /// <summary>Lấy danh sách quy định phạt thông qua BUS quy định.</summary>
        /// <param name="reload"><see langword="true"/> để yêu cầu đọc lại từ DAO; mặc định là <see langword="true"/>.</param>
        /// <returns>Các quy định phạt; rỗng nếu chưa có quy định.</returns>
        /// <exception cref="SqlException">Đọc danh sách quy định thất bại.</exception>
        public List<QuyDinhPhatDTO> LayDanhSachQuyDinh(bool reload = true)
        {
            return busQuyDinh.LayDanhSach(reload);
        }

        /// <summary>Sinh mã phiếu phạt kế tiếp từ các phiếu hiện có.</summary>
        /// <returns>Mã dạng PPnn đầu tiên chưa được sử dụng.</returns>
        /// <exception cref="SqlException">Không đọc được danh sách mã phiếu từ DAO.</exception>
        public string TaoMaPhieuTiepTheo()
        {
            return SinhMaPhieuTiepTheo(daoPhieu.LayDanhSach().Select(pp => pp.MaPhieuPhat));
        }

        /// <summary>Tìm mã PPnn đầu tiên chưa có trong tập mã đã cấp.</summary>
        /// <param name="maHienCo">Tập mã đang sử dụng; so sánh không phân biệt hoa thường.</param>
        /// <returns>Mã nhỏ nhất còn trống, đệm phần số tối thiểu hai chữ số.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="maHienCo"/> là <see langword="null"/>.</exception>
        /// <remarks>Ví dụ với PP01 và PP03, kết quả là PP02; sau PP99 mã được mở rộng thành PP100. Mã không theo mẫu không ngăn sinh PPnn.</remarks>
        public static string SinhMaPhieuTiepTheo(IEnumerable<string> maHienCo)
        {
            HashSet<string> daSuDung = new HashSet<string>(maHienCo, StringComparer.OrdinalIgnoreCase);
            for (int so = 1; ; so++)
            {
                string ma = "PP" + so.ToString("D2", CultureInfo.InvariantCulture);
                if (!daSuDung.Contains(ma)) return ma;
            }
        }

        /// <summary>Đọc lại phiếu và vi phạm từ DAO, đồng thời đánh dấu bộ nhớ đệm đã được nạp.</summary>
        /// <returns>Danh sách phiếu phạt theo ngày mới nhất trước; rỗng nếu chưa có phiếu.</returns>
        /// <exception cref="SqlException">Đọc phiếu hoặc chi tiết thất bại.</exception>
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

        /// <summary>Đọc lại toàn bộ vi phạm từ DAO và thay thế bộ nhớ đệm chi tiết.</summary>
        /// <returns>Danh sách vi phạm theo phiếu và mã dòng; rỗng nếu chưa có vi phạm.</returns>
        /// <exception cref="SqlException">Truy vấn chi tiết phiếu phạt thất bại.</exception>
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

        /// <summary>Lấy danh sách phiếu từ bộ nhớ đệm hoặc nạp lại từ DAO.</summary>
        /// <param name="reload"><see langword="true"/> để đọc mới; mặc định là <see langword="false"/>.</param>
        /// <returns>Bản sao danh sách phiếu đang có; có thể rỗng.</returns>
        /// <exception cref="SqlException">Lỗi đọc dữ liệu khi bộ nhớ đệm cần nạp lại.</exception>
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

        /// <summary>Lấy các vi phạm của một phiếu từ bộ nhớ đệm, nạp dữ liệu trước nếu cần.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần lấy; rỗng sẽ trả danh sách rỗng.</param>
        /// <returns>Chi tiết thuộc phiếu đã chọn; rỗng nếu không có mã hoặc chưa có vi phạm.</returns>
        /// <exception cref="SqlException">Lỗi đọc dữ liệu khi bộ nhớ đệm chưa được nạp.</exception>
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

        /// <summary>Kiểm tra mã phiếu chưa xuất hiện trong bộ nhớ đệm.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần kiểm tra; rỗng luôn trả về <see langword="false"/>.</param>
        /// <returns><see langword="true"/> nếu chưa có phiếu cùng mã trong bộ nhớ đệm.</returns>
        /// <exception cref="SqlException">Lỗi nạp bộ nhớ đệm lần đầu.</exception>
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

        /// <summary>Kiểm tra phiếu chưa có vi phạm trùng cùng cuốn sách và quy định.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần kiểm tra.</param>
        /// <param name="maSach">Mã cuốn sách cần đối chiếu.</param>
        /// <param name="maQD">Mã quy định cần đối chiếu.</param>
        /// <param name="boQuaMaCTPP">Mã dòng đang sửa cần bỏ qua; có thể là <see langword="null"/>.</param>
        /// <returns><see langword="true"/> nếu chưa có dòng trùng khác trong bộ nhớ đệm.</returns>
        /// <exception cref="SqlException">Lỗi nạp bộ nhớ đệm lần đầu.</exception>
        public bool KiemTraChiTietDuyNhat(string maPhieuPhat, string maSach, string maQD, int? boQuaMaCTPP = null)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat) || string.IsNullOrWhiteSpace(maSach) || string.IsNullOrWhiteSpace(maQD)) return false;

            return !dsChiTiet_arr.Cast<object>().OfType<ChiTietPhieuPhatDTO>().Any(ct =>
                Same(ct.MaPhieuPhat, maPhieuPhat) &&
                Same(ct.MaSach, maSach) &&
                Same(ct.MaQD, maQD) &&
                (!boQuaMaCTPP.HasValue || ct.MaCTPP != boQuaMaCTPP.Value));
        }

        /// <summary>Kiểm tra thông tin phiếu, các liên kết độc giả/phiếu mượn/nhân viên và trạng thái.</summary>
        /// <param name="pp">Phiếu cần kiểm tra.</param>
        /// <param name="isThemMoi"><see langword="true"/> để kiểm tra mã chưa tồn tại khi thêm mới.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pp"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Thiếu hoặc sai mã liên kết, phiếu mượn không thuộc độc giả, ngày lập không hợp lệ hoặc trạng thái không được hỗ trợ.</exception>
        /// <exception cref="SqlException">Không xác minh được độc giả, nhân viên, phiếu mượn hoặc ngày mượn.</exception>
        /// <remarks>Ngày lập không được ở tương lai hay trước ngày mượn; trạng thái chỉ nhận Chưa thanh toán hoặc Đã thanh toán.</remarks>
        public void KiemTraHopLePhieuPhat(PhieuPhatDTO pp, bool isThemMoi)
        {
            if (pp == null) throw new ArgumentNullException(nameof(pp), "Dữ liệu phiếu phạt không được null.");

            if (string.IsNullOrWhiteSpace(pp.MaPhieuPhat)) throw new BusinessRuleException("Mã phiếu phạt không được để trống.");
            if (pp.MaPhieuPhat.Trim().Length > 20) throw new BusinessRuleException("Mã phiếu phạt không được vượt quá 20 ký tự.");

            if (isThemMoi)
            {
                if (!KiemTraMaPhieuDuyNhat(pp.MaPhieuPhat))
                {
                    throw new BusinessRuleException($"Mã phiếu phạt '{pp.MaPhieuPhat.Trim()}' đã tồn tại.");
                }
                if (daoPhieu.KiemTraTonTai(pp.MaPhieuPhat.Trim()))
                {
                    throw new BusinessRuleException($"Mã phiếu phạt '{pp.MaPhieuPhat.Trim()}' đã tồn tại.");
                }
            }

            if (string.IsNullOrWhiteSpace(pp.MaDocGia)) throw new BusinessRuleException("Mã độc giả không được để trống.");
            if (pp.MaDocGia.Trim().Length > 20) throw new BusinessRuleException("Mã độc giả không được vượt quá 20 ký tự.");
            if (!daoPhieu.KiemTraDocGiaTonTai(pp.MaDocGia.Trim()))
            {
                throw new BusinessRuleException($"Độc giả '{pp.MaDocGia.Trim()}' không tồn tại.");
            }

            if (string.IsNullOrWhiteSpace(pp.MaNV)) throw new BusinessRuleException("Mã nhân viên lập phiếu không được để trống.");
            if (pp.MaNV.Trim().Length > 20) throw new BusinessRuleException("Mã nhân viên không được vượt quá 20 ký tự.");
            if (!busNhanVien.LayDanhSach(reload: true).Any(nv => Same(nv.MaNV, pp.MaNV)))
            {
                throw new BusinessRuleException($"Nhân viên '{pp.MaNV.Trim()}' không tồn tại.");
            }

            if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon))
            {
                throw new BusinessRuleException("Mã phiếu mượn không được để trống.");
            }
            if (pp.MaPhieuMuon.Trim().Length > 20) throw new BusinessRuleException("Mã phiếu mượn không được vượt quá 20 ký tự.");
            if (!daoPhieu.KiemTraPhieuMuonTonTai(pp.MaPhieuMuon.Trim()))
            {
                throw new BusinessRuleException($"Phiếu mượn '{pp.MaPhieuMuon.Trim()}' không tồn tại.");
            }
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(pp.MaPhieuMuon.Trim(), pp.MaDocGia.Trim()))
            {
                throw new BusinessRuleException($"Phiếu mượn '{pp.MaPhieuMuon.Trim()}' không thuộc độc giả '{pp.MaDocGia.Trim()}'.");
            }

            if (pp.NgayLap.Date > DateTime.Today)
                throw new BusinessRuleException("Ngày lập không được ở tương lai.");
            DateTime? ngayMuon = daoPhieu.LayNgayMuon(pp.MaPhieuMuon.Trim());
            if (!ngayMuon.HasValue)
                throw new BusinessRuleException($"Phiếu mượn '{pp.MaPhieuMuon.Trim()}' không tồn tại.");
            if (pp.NgayLap.Date < ngayMuon.Value.Date)
                throw new BusinessRuleException("Ngày lập không được trước ngày mượn.");

            string trangThai = string.IsNullOrWhiteSpace(pp.TrangThai) ? "Chưa thanh toán" : pp.TrangThai.Trim();
            if (!Same(trangThai, "Chưa thanh toán") && !Same(trangThai, "Đã thanh toán"))
            {
                throw new BusinessRuleException("Trạng thái chỉ nhận 'Chưa thanh toán' hoặc 'Đã thanh toán'.");
            }
        }

        /// <summary>Kiểm tra vi phạm thuộc phiếu hợp lệ, sách trong phiếu mượn và quy định tồn tại.</summary>
        /// <param name="ct">Chi tiết vi phạm cần kiểm tra.</param>
        /// <param name="isThemMoi"><see langword="true"/> để từ chối trùng cùng phiếu, cuốn sách và quy định.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ct"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Mã hoặc số tiền không hợp lệ, sách không thuộc phiếu mượn, hoặc có vi phạm trùng.</exception>
        /// <exception cref="SqlException">Không xác minh được phiếu, sách hoặc quy định trong cơ sở dữ liệu.</exception>
        /// <remarks>Số ngày trễ, đơn giá và tiền phạt phải không âm; ghi chú tối đa 200 ký tự.</remarks>
        public void KiemTraHopLeChiTiet(ChiTietPhieuPhatDTO ct, bool isThemMoi)
        {
            EnsureLoaded();
            if (ct == null) throw new ArgumentNullException(nameof(ct), "Dữ liệu chi tiết phiếu phạt không được null.");
            if (string.IsNullOrWhiteSpace(ct.MaPhieuPhat)) throw new BusinessRuleException("Mã phiếu phạt không được để trống.");
            if (ct.MaPhieuPhat.Trim().Length > 20) throw new BusinessRuleException("Mã phiếu phạt không được vượt quá 20 ký tự.");
            if (string.IsNullOrWhiteSpace(ct.MaQD)) throw new BusinessRuleException("Mã quy định phạt không được để trống.");
            if (ct.MaQD.Trim().Length > 20) throw new BusinessRuleException("Mã quy định phạt không được vượt quá 20 ký tự.");
            if (string.IsNullOrWhiteSpace(ct.MaSach)) throw new BusinessRuleException("Vi phạm phải có cuốn sách.");
            if (ct.MaSach.Trim().Length > 20) throw new BusinessRuleException("Mã sách không được vượt quá 20 ký tự.");
            if (!string.IsNullOrWhiteSpace(ct.GhiChu) && ct.GhiChu.Trim().Length > 200)
            {
                throw new BusinessRuleException("Ghi chú không được vượt quá 200 ký tự.");
            }
            if (ct.DonGia < 0) throw new BusinessRuleException("Đơn giá không được âm.");
            if (ct.SoNgayTre.HasValue && ct.SoNgayTre.Value < 0) throw new BusinessRuleException("Số ngày trễ không được âm.");
            if (ct.TienPhat < 0) throw new BusinessRuleException("Tiền phạt không được âm.");

            if (!daoPhieu.KiemTraTonTai(ct.MaPhieuPhat.Trim()))
            {
                throw new BusinessRuleException($"Phiếu phạt '{ct.MaPhieuPhat.Trim()}' không tồn tại.");
            }
            PhieuPhatDTO? phieu = dsPhieuPhat_arr.Cast<object>().OfType<PhieuPhatDTO>()
                .FirstOrDefault(item => Same(item.MaPhieuPhat, ct.MaPhieuPhat));
            phieu ??= daoPhieu.LayTheoMa(ct.MaPhieuPhat.Trim());
            if (phieu == null) throw new BusinessRuleException($"Phiếu phạt '{ct.MaPhieuPhat.Trim()}' không tồn tại.");
            if (string.IsNullOrWhiteSpace(phieu.MaPhieuMuon))
                throw new BusinessRuleException("Phiếu phạt chưa gắn phiếu mượn.");
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(phieu.MaPhieuMuon, phieu.MaDocGia))
                throw new BusinessRuleException($"Phiếu mượn '{phieu.MaPhieuMuon}' không thuộc độc giả '{phieu.MaDocGia}'.");
            if (TimQuyDinh(ct.MaQD.Trim()) == null)
            {
                throw new BusinessRuleException($"Quy định '{ct.MaQD.Trim()}' không tồn tại.");
            }
            if (!daoChiTiet.KiemTraSachTonTai(ct.MaSach.Trim()))
            {
                throw new BusinessRuleException($"Cuốn '{ct.MaSach.Trim()}' không tồn tại.");
            }
            if (!daoChiTiet.LaySachTheoPhieuMuon(phieu.MaPhieuMuon).Any(s => Same(s.MaSach, ct.MaSach)))
            {
                throw new BusinessRuleException($"Cuốn '{ct.MaSach.Trim()}' không thuộc phiếu mượn này.");
            }

            if (isThemMoi)
            {
                if (!KiemTraChiTietDuyNhat(ct.MaPhieuPhat, ct.MaSach, ct.MaQD) ||
                    daoChiTiet.KiemTraTrung(ct.MaPhieuPhat.Trim(), ct.MaSach.Trim(), ct.MaQD.Trim()))
                {
                    throw new BusinessRuleException(ThongBaoViPhamTrung(ct));
                }
            }
        }

        /// <summary>Tính số ngày trễ giữa hạn trả và ngày trả hoặc ngày lập vi phạm, theo ngày lịch.</summary>
        /// <param name="hanTra">Ngày đến hạn trả.</param>
        /// <param name="ngayThucTra">Ngày dùng để chốt số ngày trễ.</param>
        /// <returns>Số ngày chênh lệch nếu trễ; bằng 0 khi trả đúng hạn hoặc trước hạn.</returns>
        /// <remarks>Phần giờ bị bỏ qua và kết quả được chặn ở 0, nên ngày trả trước hạn không tạo số ngày âm.</remarks>
        public static int TinhSoNgayTre(DateTime hanTra, DateTime ngayThucTra)
        {
            return Math.Max(0, (ngayThucTra.Date - hanTra.Date).Days);
        }

        /// <summary>Tính tiền theo loại quy định, làm tròn đến hai chữ số thập phân.</summary>
        /// <param name="quyDinh">Quy định xác định cách tính và mức phạt.</param>
        /// <param name="soNgayTre">Số ngày trễ không âm; quy định cố định và mất sách không dùng giá trị này.</param>
        /// <param name="donGiaSach">Giá sách tính bằng VNĐ, không âm.</param>
        /// <returns>Tiền phạt đã làm tròn, tính bằng VNĐ.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="quyDinh"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Mức phạt, số ngày trễ hoặc giá sách là số âm.</exception>
        /// <exception cref="OverflowException">Tích tiền vượt phạm vi biểu diễn của kiểu <see cref="Decimal"/>.</exception>
        /// <remarks>QD01: số ngày trễ × mức phạt/ngày; QD02: mức cố định; QD03: giá sách × hệ số. Ví dụ 6 × 5.000 = 30.000 VNĐ; QD03 với giá 110.000 và hệ số 1,00 là 110.000 VNĐ. QD01 với 0 ngày có tiền phạt 0; không đặt ngưỡng ngày tối đa ngoài giới hạn kiểu số. Kết quả làm tròn 2 chữ số theo AwayFromZero.</remarks>
        public static decimal TinhTienPhat(QuyDinhPhatDTO quyDinh, int soNgayTre, decimal donGiaSach)
        {
            if (quyDinh == null) throw new ArgumentNullException(nameof(quyDinh));
            if (quyDinh.MucPhat < 0) throw new BusinessRuleException("Mức phạt phải từ 0 trở lên.");
            if (soNgayTre < 0) throw new BusinessRuleException("Số ngày trễ không được âm.");
            if (donGiaSach < 0) throw new BusinessRuleException("Đơn giá sách không được âm.");

            decimal tienPhat = LoaiQuyDinh(quyDinh) switch
            {
                LoaiPhat.TreHan => soNgayTre * quyDinh.MucPhat,
                LoaiPhat.MatSach => donGiaSach * quyDinh.MucPhat,
                _ => quyDinh.MucPhat
            };

            return decimal.Round(tienPhat, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>Tạo chi tiết vi phạm đã tính tiền cho mã phiếu, quy định và cuốn sách được chọn.</summary>
        /// <param name="maPhieuPhat">Mã phiếu phạt đã có trong bộ nhớ đệm.</param>
        /// <param name="maQD">Mã quy định phạt cần áp dụng.</param>
        /// <param name="maSach">Mã cuốn sách thuộc phiếu mượn liên quan.</param>
        /// <param name="ghiChu">Ghi chú tùy chọn; giá trị rỗng sẽ dùng nội dung mặc định theo quy định.</param>
        /// <returns>DTO chi tiết có số ngày trễ, đơn giá, ghi chú và tiền phạt đã tính.</returns>
        /// <exception cref="BusinessRuleException">Không chọn được phiếu, quy định hoặc sách hợp lệ; hoặc ngày lập không phù hợp với hạn/trả thực tế.</exception>
        /// <exception cref="SqlException">Không đọc được thông tin phiếu mượn, sách hoặc giá sách.</exception>
        /// <exception cref="OverflowException">Phép tính tiền vượt phạm vi biểu diễn của kiểu <see cref="Decimal"/>.</exception>
        /// <remarks>QD01 dùng ngày trả thực tế nếu đã trả, nếu chưa thì dùng ngày lập phiếu; QD02 lấy mức phạt cố định; QD03 dùng giá sách và hệ số. Vi phạm trễ hạn phải được lập sau hạn trả và không trước ngày thực trả.</remarks>
        public ChiTietPhieuPhatDTO TinhChiTietPhat(string maPhieuPhat, string maQD, string? maSach, string? ghiChu = null)
        {
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new BusinessRuleException("Vui lòng chọn phiếu phạt.");
            EnsureLoaded();
            PhieuPhatDTO? pp = dsPhieuPhat_arr.Cast<object>().OfType<PhieuPhatDTO>()
                .FirstOrDefault(item => Same(item.MaPhieuPhat, maPhieuPhat));
            if (pp == null) throw new BusinessRuleException($"Không tìm thấy phiếu phạt '{maPhieuPhat.Trim()}'.");

            return TinhChiTietPhat(pp, maQD, maSach, ghiChu);
        }

        private ChiTietPhieuPhatDTO TinhChiTietPhat(PhieuPhatDTO pp, string maQD, string? maSach, string? ghiChu = null)
        {
            if (string.IsNullOrWhiteSpace(maQD)) throw new BusinessRuleException("Vui lòng chọn quy định phạt.");
            QuyDinhPhatDTO? qd = TimQuyDinh(maQD.Trim());
            if (qd == null) throw new BusinessRuleException($"Quy định '{maQD.Trim()}' không tồn tại.");

            if (string.IsNullOrWhiteSpace(maSach)) throw new BusinessRuleException("Vui lòng chọn cuốn sách vi phạm.");
            string sachTrim = maSach.Trim();
            if (!daoChiTiet.KiemTraSachTonTai(sachTrim)) throw new BusinessRuleException($"Cuốn '{sachTrim}' không tồn tại.");

            if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon))
                throw new BusinessRuleException("Phiếu phạt chưa gắn phiếu mượn.");
            string maPhieuMuon = pp.MaPhieuMuon.Trim();
            if (!daoPhieu.KiemTraPhieuMuonCuaDocGia(maPhieuMuon, pp.MaDocGia))
                throw new BusinessRuleException($"Phiếu mượn '{maPhieuMuon}' không thuộc độc giả '{pp.MaDocGia}'.");
            SachMuonDTO? sachMuon = daoChiTiet.LaySachTheoPhieuMuon(maPhieuMuon)
                .FirstOrDefault(item => Same(item.MaSach, sachTrim));
            if (sachMuon == null)
                throw new BusinessRuleException($"Cuốn '{sachTrim}' không thuộc phiếu mượn này.");

            LoaiPhat loaiPhat = LoaiQuyDinh(qd);
            decimal giaSach = daoChiTiet.LayDonGiaSach(sachTrim);
            int? soNgayTre = null;
            decimal donGia = giaSach;

            if (loaiPhat == LoaiPhat.TreHan)
            {
                if (pp.NgayLap.Date <= sachMuon.HanTra.Date)
                    throw new BusinessRuleException("Ngày lập phải sau hạn trả để ghi vi phạm trễ hạn.");
                if (sachMuon.NgayThucTra.HasValue && pp.NgayLap.Date < sachMuon.NgayThucTra.Value.Date)
                    throw new BusinessRuleException("Ngày lập không được trước ngày thực trả.");
                DateTime ngayTinhPhat = sachMuon.NgayThucTra?.Date ?? pp.NgayLap.Date;
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

        /// <summary>Sinh mã, kiểm tra và lưu phiếu phạt mới, sau đó thêm phiếu vào bộ nhớ đệm.</summary>
        /// <param name="pp">Thông tin phiếu cần lưu; mã đầu vào được thay bằng mã tự sinh.</param>
        /// <returns><see langword="true"/> nếu DAO lưu thành công; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pp"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Thông tin phiếu không đạt quy tắc nghiệp vụ.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra hoặc lưu phiếu được truyền từ DAO.</exception>
        /// <remarks>Phiếu đầu được lưu với tổng tiền 0 VNĐ; các dòng vi phạm chỉ có thể ghi sau khi mã phiếu đã tồn tại.</remarks>
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

            // Lưu đầu phiếu trước để các vi phạm sau đó có mã phiếu hợp lệ làm khóa liên kết.
            bool success = daoPhieu.Them(normalized);
            if (success) dsPhieuPhat_arr.Insert(0, normalized);
            return success;
        }

        /// <summary>Kiểm tra và cập nhật thông tin phiếu, giữ nguyên tổng từ các chi tiết hiện có.</summary>
        /// <param name="pp">Thông tin phiếu cần cập nhật, gồm mã phiếu hiện có.</param>
        /// <returns><see langword="true"/> nếu DAO cập nhật thành công; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pp"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Phiếu không hợp lệ, không tìm thấy, thay đổi liên kết khi còn vi phạm hoặc làm sai số tiền chi tiết.</exception>
        /// <exception cref="SqlException">Lỗi đọc chi tiết hoặc cập nhật phiếu được truyền từ DAO.</exception>
        /// <remarks>Nếu đã có vi phạm thì không cho đổi độc giả hoặc phiếu mượn; các dòng được tính lại trước khi lưu đầu phiếu.</remarks>
        public bool SuaPhieu(PhieuPhatDTO pp)
        {
            EnsureLoaded();
            KiemTraHopLePhieuPhat(pp, isThemMoi: false);

            string maTrim = pp.MaPhieuPhat.Trim();
            if (!daoPhieu.KiemTraTonTai(maTrim)) throw new BusinessRuleException($"Không tìm thấy phiếu phạt '{maTrim}'.");

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
                throw new BusinessRuleException("Xóa các vi phạm trước khi đổi độc giả hoặc phiếu mượn.");
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

        /// <summary>Xóa phiếu phạt và loại phiếu cùng chi tiết khỏi bộ nhớ đệm khi DAO xóa thành công.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần xóa.</param>
        /// <returns><see langword="true"/> nếu DAO xóa được phiếu; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="BusinessRuleException">Mã rỗng hoặc phiếu không tồn tại.</exception>
        /// <exception cref="SqlException">Lỗi kiểm tra hoặc xóa phiếu được truyền từ DAO.</exception>
        public bool XoaPhieu(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new BusinessRuleException("Mã phiếu phạt cần xóa không được để trống.");

            string maTrim = maPhieuPhat.Trim();
            if (!daoPhieu.KiemTraTonTai(maTrim)) throw new BusinessRuleException($"Không tìm thấy phiếu phạt '{maTrim}'.");

            bool success = daoPhieu.Xoa(maTrim);
            if (success)
            {
                RemoveWhere(dsPhieuPhat_arr, item => item is PhieuPhatDTO current && Same(current.MaPhieuPhat, maTrim));
                RemoveWhere(dsChiTiet_arr, item => item is ChiTietPhieuPhatDTO ct && Same(ct.MaPhieuPhat, maTrim));
            }
            return success;
        }

        /// <summary>Tính lại, kiểm tra trùng rồi lưu vi phạm mới và cập nhật tổng tiền phiếu.</summary>
        /// <param name="ct">Thông tin mã phiếu, quy định, cuốn sách và ghi chú cần thêm.</param>
        /// <returns><see langword="true"/> nếu vi phạm được lưu; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ct"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Dữ liệu không hợp lệ hoặc đã có vi phạm cùng cuốn và quy định trong phiếu.</exception>
        /// <exception cref="SqlException">Lỗi cơ sở dữ liệu không thuộc trường hợp trùng khóa.</exception>
        /// <remarks>Tiền và số ngày đầu vào không được tin cậy: chúng được tính lại theo dữ liệu sách, ngày phiếu và quy định trước khi lưu.</remarks>
        public bool ThemChiTiet(ChiTietPhieuPhatDTO ct)
        {
            EnsureLoaded();
            if (ct == null) throw new ArgumentNullException(nameof(ct));
            ChiTietPhieuPhatDTO normalized = TinhChiTietPhat(ct.MaPhieuPhat.Trim(), ct.MaQD.Trim(), ct.MaSach, ct.GhiChu);
            KiemTraHopLeChiTiet(normalized, isThemMoi: true);

            try
            {
                List<ChiTietPhieuPhatDTO> chiTiet = LayChiTietTheoPhieu(normalized.MaPhieuPhat);
                chiTiet.Add(normalized);
                decimal tongTien = TongTien(chiTiet);
                int maCTPP = daoChiTiet.ThemVaCapNhatTongTien(normalized, tongTien);
                if (maCTPP <= 0) return false;
                normalized.MaCTPP = maCTPP;
                dsChiTiet_arr.Add(normalized);
                CapNhatTongTienTrongCache(normalized.MaPhieuPhat, tongTien);
                return true;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new BusinessRuleException(ThongBaoViPhamTrung(normalized), ex);
            }
        }

        /// <summary>Tính lại và lưu thay đổi của vi phạm, đồng bộ tổng tiền và bộ nhớ đệm.</summary>
        /// <param name="maCTPP">Mã dòng vi phạm cần sửa, phải lớn hơn 0.</param>
        /// <param name="ct">Quy định, sách và ghi chú mới; phiếu được giữ theo dòng đang sửa.</param>
        /// <returns><see langword="true"/> nếu dòng được cập nhật; <see langword="false"/> nếu dòng không còn tồn tại.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ct"/> là <see langword="null"/>.</exception>
        /// <exception cref="BusinessRuleException">Không chọn được dòng, dữ liệu không hợp lệ hoặc vi phạm bị trùng với dòng khác.</exception>
        /// <exception cref="SqlException">Lỗi cơ sở dữ liệu không thuộc trường hợp trùng khóa.</exception>
        /// <remarks>Khi kiểm tra trùng, mã dòng đang sửa được bỏ qua; sau đó tiền được tính lại trước khi DAO cập nhật dòng và tổng.</remarks>
        public bool SuaChiTiet(int maCTPP, ChiTietPhieuPhatDTO ct)
        {
            EnsureLoaded();
            if (ct == null) throw new ArgumentNullException(nameof(ct), "Dữ liệu chi tiết phiếu phạt không được null.");
            if (maCTPP <= 0)
                throw new BusinessRuleException("Vui lòng chọn vi phạm cần sửa.");

            ChiTietPhieuPhatDTO? existing = TimChiTietTheoMaCTPP(maCTPP) ?? daoChiTiet.LayTheoMaCTPP(maCTPP);
            if (existing == null) throw new BusinessRuleException("Không tìm thấy vi phạm cần sửa.");

            ChiTietPhieuPhatDTO input = new ChiTietPhieuPhatDTO(
                existing.MaPhieuPhat,
                ct.MaQD,
                ct.MaSach,
                ct.DonGia,
                ct.SoNgayTre,
                ct.GhiChu,
                ct.TienPhat
            );
            KiemTraHopLeChiTiet(input, isThemMoi: false);

            ChiTietPhieuPhatDTO normalized = TinhChiTietPhat(existing.MaPhieuPhat, input.MaQD.Trim(), input.MaSach, input.GhiChu);
            normalized.MaCTPP = existing.MaCTPP;
            if (!KiemTraChiTietDuyNhat(normalized.MaPhieuPhat, normalized.MaSach, normalized.MaQD, maCTPP) ||
                daoChiTiet.KiemTraTrung(normalized.MaPhieuPhat, normalized.MaSach, normalized.MaQD, maCTPP))
            {
                throw new BusinessRuleException(ThongBaoViPhamTrung(normalized));
            }

            try
            {
                List<ChiTietPhieuPhatDTO> chiTiet = LayChiTietTheoPhieu(normalized.MaPhieuPhat);
                int chiTietIndex = chiTiet.FindIndex(current => current.MaCTPP == maCTPP);
                if (chiTietIndex < 0) return false;
                chiTiet[chiTietIndex] = normalized;
                decimal tongTien = TongTien(chiTiet);
                bool success = daoChiTiet.SuaVaCapNhatTongTien(normalized, tongTien);
                if (!success) return false;
                for (int i = 0; i < dsChiTiet_arr.Count; i++)
                {
                    if (dsChiTiet_arr[i] is ChiTietPhieuPhatDTO current && current.MaCTPP == maCTPP)
                    {
                        dsChiTiet_arr[i] = normalized;
                        break;
                    }
                }
                CapNhatTongTienTrongCache(normalized.MaPhieuPhat, tongTien);
                return true;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new BusinessRuleException(ThongBaoViPhamTrung(normalized), ex);
            }
        }

        /// <summary>Xóa một vi phạm và tính lại tổng tiền còn lại của phiếu.</summary>
        /// <param name="maCTPP">Mã dòng vi phạm cần xóa, phải lớn hơn 0.</param>
        /// <returns><see langword="true"/> nếu dòng được xóa; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="BusinessRuleException">Không chọn được dòng hoặc không tìm thấy vi phạm.</exception>
        /// <exception cref="SqlException">Lệnh xóa hoặc cập nhật tổng tiền thất bại.</exception>
        public bool XoaChiTiet(int maCTPP)
        {
            EnsureLoaded();
            if (maCTPP <= 0)
                throw new BusinessRuleException("Vui lòng chọn vi phạm cần xóa.");
            ChiTietPhieuPhatDTO? existing = TimChiTietTheoMaCTPP(maCTPP) ?? daoChiTiet.LayTheoMaCTPP(maCTPP);
            if (existing == null) throw new BusinessRuleException("Không tìm thấy vi phạm cần xóa.");

            List<ChiTietPhieuPhatDTO> chiTiet = LayChiTietTheoPhieu(existing.MaPhieuPhat);
            decimal tongTien = TongTien(chiTiet.Where(current => current.MaCTPP != maCTPP));
            bool success = daoChiTiet.XoaVaCapNhatTongTien(maCTPP, existing.MaPhieuPhat, tongTien);
            if (success)
            {
                RemoveWhere(dsChiTiet_arr, item => item is ChiTietPhieuPhatDTO current && current.MaCTPP == maCTPP);
                CapNhatTongTienTrongCache(existing.MaPhieuPhat, tongTien);
            }
            return success;
        }

        private void CapNhatTongTienTrongCache(string maPhieuPhat, decimal tongTien)
        {
            foreach (object item in dsPhieuPhat_arr)
            {
                if (item is PhieuPhatDTO pp && Same(pp.MaPhieuPhat, maPhieuPhat))
                {
                    pp.TongTienPhat = tongTien;
                    break;
                }
            }
        }

        /// <summary>Tính lại tổng tiền từ các dòng đang lưu và ghi kết quả vào phiếu.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần tính lại.</param>
        /// <returns>Tổng tiền các vi phạm, làm tròn hai chữ số và tính bằng VNĐ.</returns>
        /// <exception cref="BusinessRuleException">Mã phiếu rỗng hoặc phiếu không tồn tại khi cập nhật.</exception>
        /// <exception cref="SqlException">Không đọc được chi tiết hoặc không cập nhật được phiếu.</exception>
        /// <remarks>Tổng là tổng TienPhat của các dòng trên phiếu, làm tròn theo AwayFromZero; phiếu không có dòng có tổng bằng 0.</remarks>
        public decimal CapNhatTongTienPhieu(string maPhieuPhat)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(maPhieuPhat)) throw new BusinessRuleException("Mã phiếu phạt không được để trống.");

            string phieuTrim = maPhieuPhat.Trim();
            List<ChiTietPhieuPhatDTO> chiTiet = daoChiTiet.LayDanhSachTheoPhieu(phieuTrim);
            ThayChiTietTrongCache(phieuTrim, chiTiet);
            decimal tongTien = TongTien(chiTiet);
            if (!daoPhieu.CapNhatTongTien(phieuTrim, tongTien))
            {
                throw new BusinessRuleException($"Không tìm thấy phiếu phạt '{phieuTrim}'.");
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

        /// <summary>Tìm phiếu theo mã phiếu, mã độc giả, mã phiếu mượn, mã nhân viên hoặc trạng thái.</summary>
        /// <param name="tuKhoa">Từ khóa không phân biệt hoa thường; rỗng trả toàn bộ danh sách đã nạp.</param>
        /// <returns>Các phiếu có trường tìm kiếm chứa từ khóa; rỗng nếu không có kết quả.</returns>
        /// <exception cref="SqlException">Lỗi nạp danh sách lần đầu.</exception>
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
                if (string.IsNullOrWhiteSpace(pp.MaPhieuMuon) || string.IsNullOrWhiteSpace(ct.MaSach) ||
                    !daoChiTiet.LaySachTheoPhieuMuon(pp.MaPhieuMuon).Any(s => Same(s.MaSach, ct.MaSach)))
                {
                    throw new BusinessRuleException($"Cuốn '{ct.MaSach}' không thuộc phiếu mượn này.");
                }

                ChiTietPhieuPhatDTO tinhLai = TinhChiTietPhat(pp, ct.MaQD, ct.MaSach, ct.GhiChu);
                if (tinhLai.SoNgayTre != ct.SoNgayTre || tinhLai.DonGia != ct.DonGia || tinhLai.TienPhat != ct.TienPhat)
                    throw new BusinessRuleException("Ngày lập làm thay đổi tiền phạt, hãy sửa vi phạm trước.");
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

        private ChiTietPhieuPhatDTO? TimChiTietTheoMaCTPP(int maCTPP)
        {
            return dsChiTiet_arr.Cast<object>().OfType<ChiTietPhieuPhatDTO>()
                .FirstOrDefault(ct => ct.MaCTPP == maCTPP);
        }

        private string ThongBaoViPhamTrung(ChiTietPhieuPhatDTO ct)
        {
            string tenQuyDinh = TimQuyDinh(ct.MaQD)?.TenQD ?? ct.MaQD;
            return $"Cuốn '{ct.MaSach}' đã bị ghi vi phạm '{tenQuyDinh}' trong phiếu này.";
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
