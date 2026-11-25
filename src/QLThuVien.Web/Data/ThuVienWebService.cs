using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QLThuVien.Web.Data
{
    public class TheLoaiWebModel
    {
        public string MaTL { get; set; } = string.Empty;
        public string TenTL { get; set; } = string.Empty;
    }

    public class LookupItemWeb
    {
        public string Ma { get; set; } = string.Empty;
        public string Ten { get; set; } = string.Empty;
    }

    public class DauSachEditModel
    {
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string MaTG { get; set; } = string.Empty;
        public string MaTL { get; set; } = string.Empty;
        public string MaNXB { get; set; } = string.Empty;
        public int? NamXB { get; set; } = DateTime.Now.Year;
        public decimal DonGia { get; set; }
    }

    public class YeuCauMuonWeb
    {
        public string MaYeuCau { get; set; } = string.Empty;
        public string MaDocGia { get; set; } = string.Empty;
        public string TenDocGia { get; set; } = string.Empty;
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public DateTime NgayYeuCau { get; set; } = DateTime.Now;
        public string GhiChu { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Chờ duyệt";
        public string? MaPMDaTao { get; set; }
    }

    public class YeuCauMuonStore
    {
        private readonly ConcurrentDictionary<string, YeuCauMuonWeb> items = new(StringComparer.OrdinalIgnoreCase);
        private int counter = 0;

        public List<YeuCauMuonWeb> LayTatCa()
        {
            return items.Values
                .OrderByDescending(x => x.NgayYeuCau)
                .ToList();
        }

        public YeuCauMuonWeb Them(string maDG, string tenDG, string maDauSach, string tenSach, string ghiChu)
        {
            int stt = System.Threading.Interlocked.Increment(ref counter);
            string maYC = $"YC{stt:D3}";
            YeuCauMuonWeb item = new()
            {
                MaYeuCau = maYC,
                MaDocGia = maDG,
                TenDocGia = tenDG,
                MaDauSach = maDauSach,
                TenSach = tenSach,
                NgayYeuCau = DateTime.Now,
                GhiChu = ghiChu ?? string.Empty,
                TrangThai = "Chờ duyệt"
            };
            items[maYC] = item;
            return item;
        }

        public YeuCauMuonWeb? LayTheoMa(string maYeuCau)
        {
            items.TryGetValue(maYeuCau, out var item);
            return item;
        }
    }

    public class ThuVienWebService
    {
        private const string DefaultFallbackConnectionString = @"Server=.;Database=QLThuVien;Integrated Security=True;TrustServerCertificate=True;";
        private readonly string connectionString;
        private readonly YeuCauMuonStore yeuCauStore;

        public ThuVienWebService(IConfiguration configuration, YeuCauMuonStore yeuCauStore)
        {
            this.yeuCauStore = yeuCauStore;
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"]
                ?? DefaultFallbackConnectionString;
        }

        // ============================================================
        // 1. QUẢN LÝ THỂ LOẠI (CRUD + TÌM KIẾM)
        // ============================================================
        public List<TheLoaiWebModel> LayDanhSachTheLoai(string? tuKhoa = null)
        {
            List<TheLoaiWebModel> list = new();
            string sql = @"SELECT MaTL, TenTL FROM TheLoai
                           WHERE (@TuKhoa IS NULL OR @TuKhoa = ''
                                  OR MaTL LIKE '%' + @TuKhoa + '%'
                                  OR TenTL LIKE '%' + @TuKhoa + '%')
                           ORDER BY MaTL ASC";

            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100) { Value = (object?)tuKhoa?.Trim() ?? DBNull.Value });
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TheLoaiWebModel
                {
                    MaTL = reader["MaTL"].ToString() ?? string.Empty,
                    TenTL = reader["TenTL"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        public (bool ok, string message) ThemTheLoai(TheLoaiWebModel tl)
        {
            if (string.IsNullOrWhiteSpace(tl.MaTL) || string.IsNullOrWhiteSpace(tl.TenTL))
                return (false, "Vui lòng nhập đầy đủ Mã thể loại và Tên thể loại.");

            string ma = tl.MaTL.Trim();
            string ten = tl.TenTL.Trim();

            using SqlConnection conn = new(connectionString);
            conn.Open();
            using (SqlCommand check = new("SELECT COUNT(1) FROM TheLoai WHERE MaTL = @MaTL", conn))
            {
                check.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = ma });
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    return (false, $"Mã thể loại '{ma}' đã tồn tại!");
            }

            using SqlCommand cmd = new("INSERT INTO TheLoai (MaTL, TenTL) VALUES (@MaTL, @TenTL)", conn);
            cmd.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = ma });
            cmd.Parameters.Add(new SqlParameter("@TenTL", SqlDbType.NVarChar, 100) { Value = ten });
            cmd.ExecuteNonQuery();
            return (true, $"Đã thêm thể loại '{ma} - {ten}' thành công.");
        }

        public (bool ok, string message) SuaTheLoai(TheLoaiWebModel tl)
        {
            if (string.IsNullOrWhiteSpace(tl.MaTL) || string.IsNullOrWhiteSpace(tl.TenTL))
                return (false, "Mã thể loại và Tên thể loại không được để trống.");

            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlCommand cmd = new("UPDATE TheLoai SET TenTL = @TenTL WHERE MaTL = @MaTL", conn);
            cmd.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = tl.MaTL.Trim() });
            cmd.Parameters.Add(new SqlParameter("@TenTL", SqlDbType.NVarChar, 100) { Value = tl.TenTL.Trim() });
            int rows = cmd.ExecuteNonQuery();
            return rows > 0 ? (true, "Cập nhật thể loại thành công.") : (false, "Không tìm thấy thể loại cần sửa.");
        }

        public (bool ok, string message) XoaTheLoai(string maTL)
        {
            if (string.IsNullOrWhiteSpace(maTL))
                return (false, "Chưa chọn thể loại cần xóa.");

            using SqlConnection conn = new(connectionString);
            conn.Open();
            using (SqlCommand check = new("SELECT COUNT(1) FROM DauSach WHERE MaTL = @MaTL", conn))
            {
                check.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = maTL.Trim() });
                int count = Convert.ToInt32(check.ExecuteScalar());
                if (count > 0)
                    return (false, $"Không thể xóa vì đang có {count} đầu sách thuộc thể loại '{maTL}'.");
            }

            using SqlCommand cmd = new("DELETE FROM TheLoai WHERE MaTL = @MaTL", conn);
            cmd.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = maTL.Trim() });
            return cmd.ExecuteNonQuery() > 0
                ? (true, $"Đã xóa thể loại '{maTL}'.")
                : (false, "Không tìm thấy thể loại cần xóa.");
        }

        // ============================================================
        // 2. QUẢN LÝ ĐẦU SÁCH (CRUD + LOOKUP DANH MỤC)
        // ============================================================
        public List<LookupItemWeb> LayDanhMuc(string loai)
        {
            string sql = loai switch
            {
                "TacGia" => "SELECT MaTG AS Ma, CONCAT(MaTG, N' - ', ISNULL(HoLot + N' ', N''), TenTG) AS Ten FROM TacGia ORDER BY MaTG",
                "TheLoai" => "SELECT MaTL AS Ma, CONCAT(MaTL, N' - ', TenTL) AS Ten FROM TheLoai ORDER BY MaTL",
                "NXB" => "SELECT MaNXB AS Ma, CONCAT(MaNXB, N' - ', TenNXB) AS Ten FROM NhaXuatBan ORDER BY MaNXB",
                "DocGia" => "SELECT MaDG AS Ma, CONCAT(MaDG, N' - ', ISNULL(HoLot + N' ', N''), TenDG) AS Ten FROM DocGia ORDER BY MaDG",
                "NhanVien" => "SELECT MaNV AS Ma, CONCAT(MaNV, N' - ', ISNULL(HoLot + N' ', N''), TenNV) AS Ten FROM NhanVien ORDER BY MaNV",
                _ => throw new ArgumentException("Loại danh mục không hợp lệ.")
            };

            List<LookupItemWeb> list = new();
            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlCommand cmd = new(sql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new LookupItemWeb
                {
                    Ma = reader["Ma"].ToString() ?? string.Empty,
                    Ten = reader["Ten"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        public DauSachEditModel? LayDauSachTheoMa(string maDauSach)
        {
            string sql = "SELECT MaDauSach, TenSach, MaTG, MaTL, MaNXB, NamXB, DonGia FROM DauSach WHERE MaDauSach = @Ma";
            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maDauSach });
            using SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;
            return new DauSachEditModel
            {
                MaDauSach = reader["MaDauSach"].ToString() ?? string.Empty,
                TenSach = reader["TenSach"].ToString() ?? string.Empty,
                MaTG = reader["MaTG"].ToString() ?? string.Empty,
                MaTL = reader["MaTL"].ToString() ?? string.Empty,
                MaNXB = reader["MaNXB"].ToString() ?? string.Empty,
                NamXB = reader["NamXB"] == DBNull.Value ? null : Convert.ToInt32(reader["NamXB"]),
                DonGia = reader["DonGia"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DonGia"])
            };
        }

        public (bool ok, string message) ThemDauSach(DauSachEditModel ds)
        {
            if (string.IsNullOrWhiteSpace(ds.MaDauSach) || string.IsNullOrWhiteSpace(ds.TenSach))
                return (false, "Mã đầu sách và Tên sách không được để trống.");
            if (ds.DonGia < 0)
                return (false, "Đơn giá không được âm.");
            if (ds.NamXB.HasValue && (ds.NamXB.Value < 1900 || ds.NamXB.Value > DateTime.Now.Year))
                return (false, $"Năm xuất bản phải từ 1900 đến {DateTime.Now.Year}.");

            string ma = ds.MaDauSach.Trim();
            using SqlConnection conn = new(connectionString);
            conn.Open();

            using (SqlCommand check = new("SELECT COUNT(1) FROM DauSach WHERE MaDauSach = @Ma", conn))
            {
                check.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = ma });
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    return (false, $"Mã đầu sách '{ma}' đã tồn tại!");
            }

            string sql = @"INSERT INTO DauSach (MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, SoLuong, SLChoMuon, SLConLai, DonGia)
                           VALUES (@MaDauSach, @MaNXB, @MaTG, @MaTL, @TenSach, @NamXB, 0, 0, 0, @DonGia)";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = ma });
            cmd.Parameters.Add(new SqlParameter("@MaNXB", SqlDbType.VarChar, 20) { Value = ds.MaNXB });
            cmd.Parameters.Add(new SqlParameter("@MaTG", SqlDbType.VarChar, 20) { Value = ds.MaTG });
            cmd.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = ds.MaTL });
            cmd.Parameters.Add(new SqlParameter("@TenSach", SqlDbType.NVarChar, 200) { Value = ds.TenSach.Trim() });
            cmd.Parameters.Add(new SqlParameter("@NamXB", SqlDbType.Int) { Value = (object?)ds.NamXB ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = ds.DonGia });
            cmd.ExecuteNonQuery();
            return (true, $"Đã thêm đầu sách '{ma}' thành công.");
        }

        public (bool ok, string message) SuaDauSach(DauSachEditModel ds)
        {
            if (string.IsNullOrWhiteSpace(ds.MaDauSach) || string.IsNullOrWhiteSpace(ds.TenSach))
                return (false, "Tên sách không được để trống.");
            if (ds.DonGia < 0)
                return (false, "Đơn giá không được âm.");

            string sql = @"UPDATE DauSach
                           SET TenSach = @TenSach, MaTG = @MaTG, MaTL = @MaTL, MaNXB = @MaNXB, NamXB = @NamXB, DonGia = @DonGia
                           WHERE MaDauSach = @MaDauSach";
            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = ds.MaDauSach.Trim() });
            cmd.Parameters.Add(new SqlParameter("@MaNXB", SqlDbType.VarChar, 20) { Value = ds.MaNXB });
            cmd.Parameters.Add(new SqlParameter("@MaTG", SqlDbType.VarChar, 20) { Value = ds.MaTG });
            cmd.Parameters.Add(new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = ds.MaTL });
            cmd.Parameters.Add(new SqlParameter("@TenSach", SqlDbType.NVarChar, 200) { Value = ds.TenSach.Trim() });
            cmd.Parameters.Add(new SqlParameter("@NamXB", SqlDbType.Int) { Value = (object?)ds.NamXB ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = ds.DonGia });
            return cmd.ExecuteNonQuery() > 0
                ? (true, $"Đã cập nhật đầu sách '{ds.MaDauSach}'.")
                : (false, "Không tìm thấy đầu sách cần sửa.");
        }

        public (bool ok, string message) XoaDauSach(string maDauSach)
        {
            using SqlConnection conn = new(connectionString);
            conn.Open();
            using (SqlCommand check = new("SELECT SLChoMuon FROM DauSach WHERE MaDauSach = @Ma", conn))
            {
                check.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maDauSach });
                object? val = check.ExecuteScalar();
                if (val == null) return (false, "Không tìm thấy đầu sách.");
                if (Convert.ToInt32(val) > 0)
                    return (false, "Không thể xóa đầu sách đang có sách được cho mượn.");
            }

            using SqlTransaction trans = conn.BeginTransaction();
            try
            {
                using (SqlCommand checkRef = new(@"SELECT COUNT(1) FROM Sach s
                                                   WHERE s.MaDauSach = @Ma AND (
                                                       EXISTS (SELECT 1 FROM ChiTietPhieuMuon ct WHERE ct.MaSach = s.MaSach)
                                                       OR EXISTS (SELECT 1 FROM ChiTietPhieuPhat cp WHERE cp.MaSach = s.MaSach))", conn, trans))
                {
                    checkRef.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maDauSach });
                    if (Convert.ToInt32(checkRef.ExecuteScalar()) > 0)
                        return (false, "Đầu sách này đã phát sinh lịch sử mượn/phạt, không thể xóa.");
                }

                using (SqlCommand delSach = new("DELETE FROM Sach WHERE MaDauSach = @Ma", conn, trans))
                {
                    delSach.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maDauSach });
                    delSach.ExecuteNonQuery();
                }

                using (SqlCommand delDS = new("DELETE FROM DauSach WHERE MaDauSach = @Ma", conn, trans))
                {
                    delDS.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar, 20) { Value = maDauSach });
                    delDS.ExecuteNonQuery();
                }

                trans.Commit();
                return (true, $"Đã xóa đầu sách '{maDauSach}'.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return (false, "Không thể xóa đầu sách: " + ex.Message);
            }
        }

        // ============================================================
        // 3. ĐẶT MƯỢN SÁCH & ADMIN DUYỆT YÊU CẦU (CỘNG ĐIỂM)
        // ============================================================
        public List<YeuCauMuonWeb> LayDanhSachYeuCauMuon() => yeuCauStore.LayTatCa();

        public (bool ok, string message) GuiYeuCauMuon(string maDG, string maDauSach, string ghiChu)
        {
            if (string.IsNullOrWhiteSpace(maDG) || string.IsNullOrWhiteSpace(maDauSach))
                return (false, "Vui lòng chọn Độc giả và Đầu sách muốn mượn.");

            using SqlConnection conn = new(connectionString);
            conn.Open();

            string tenDG = maDG;
            using (SqlCommand cmdDG = new("SELECT CONCAT(ISNULL(HoLot + N' ', N''), TenDG) FROM DocGia WHERE MaDG = @MaDG", conn))
            {
                cmdDG.Parameters.Add(new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG });
                object? val = cmdDG.ExecuteScalar();
                if (val == null) return (false, "Độc giả không tồn tại.");
                tenDG = val.ToString() ?? maDG;
            }

            string tenSach = maDauSach;
            int slConLai = 0;
            using (SqlCommand cmdDS = new("SELECT TenSach, SLConLai FROM DauSach WHERE MaDauSach = @MaDS", conn))
            {
                cmdDS.Parameters.Add(new SqlParameter("@MaDS", SqlDbType.VarChar, 20) { Value = maDauSach });
                using SqlDataReader r = cmdDS.ExecuteReader();
                if (!r.Read()) return (false, "Đầu sách không tồn tại.");
                tenSach = r["TenSach"].ToString() ?? maDauSach;
                slConLai = Convert.ToInt32(r["SLConLai"]);
            }

            if (slConLai <= 0)
                return (false, $"Đầu sách '{tenSach}' hiện đã hết sách trên kệ.");

            YeuCauMuonWeb yc = yeuCauStore.Them(maDG, tenDG, maDauSach, tenSach, ghiChu);
            return (true, $"Đã gửi yêu cầu đặt mượn {yc.MaYeuCau} thành công! Vui lòng chờ Thủ thư (Admin) duyệt.");
        }

        public (bool ok, string message) DuyetYeuCauMuon(string maYeuCau, string maNV)
        {
            YeuCauMuonWeb? yc = yeuCauStore.LayTheoMa(maYeuCau);
            if (yc == null) return (false, "Không tìm thấy yêu cầu mượn.");
            if (yc.TrangThai != "Chờ duyệt") return (false, $"Yêu cầu này đang ở trạng thái '{yc.TrangThai}'.");
            if (string.IsNullOrWhiteSpace(maNV)) return (false, "Vui lòng chọn Nhân viên duyệt.");

            using SqlConnection conn = new(connectionString);
            conn.Open();
            using SqlTransaction trans = conn.BeginTransaction();
            try
            {
                // Lấy số ngày mượn tối đa từ bảng ThamSo
                int soNgayMuon = 7;
                using (SqlCommand cmdTS = new("SELECT GiaTri FROM ThamSo WHERE MaTS = 'SO_NGAY_MUON_TOI_DA'", conn, trans))
                {
                    object? val = cmdTS.ExecuteScalar();
                    if (val != null && int.TryParse(val.ToString(), out int parsed) && parsed > 0)
                        soNgayMuon = parsed;
                }

                // Tìm 1 cuốn sách vật lý có sẵn thuộc đầu sách
                string? maSachSanSang = null;
                using (SqlCommand cmdSach = new(@"SELECT TOP 1 MaSach FROM Sach
                                                   WHERE MaDauSach = @MaDS AND TrangThai = 0 AND ISNULL(TinhTrang, N'') <> N'Mất'
                                                   ORDER BY MaSach", conn, trans))
                {
                    cmdSach.Parameters.Add(new SqlParameter("@MaDS", SqlDbType.VarChar, 20) { Value = yc.MaDauSach });
                    maSachSanSang = cmdSach.ExecuteScalar()?.ToString();
                }

                if (string.IsNullOrEmpty(maSachSanSang))
                {
                    trans.Rollback();
                    return (false, $"Hiện không còn cuốn sách vật lý nào sẵn sàng cho đầu sách '{yc.TenSach}'.");
                }

                // Sinh mã phiếu mượn mới PMxx
                string maPM = "PM01";
                using (SqlCommand cmdMax = new("SELECT MaPM FROM PhieuMuon", conn, trans))
                using (SqlDataReader r = cmdMax.ExecuteReader())
                {
                    int max = 0;
                    while (r.Read())
                    {
                        string raw = r["MaPM"].ToString() ?? string.Empty;
                        if (raw.StartsWith("PM", StringComparison.OrdinalIgnoreCase) && int.TryParse(raw.Substring(2), out int n) && n > max)
                            max = n;
                    }
                    maPM = $"PM{(max + 1):D2}";
                }

                DateTime ngayMuon = DateTime.Today;
                DateTime hanTra = ngayMuon.AddDays(soNgayMuon);

                using (SqlCommand insPM = new(@"INSERT INTO PhieuMuon (MaPM, MaDocGia, MaNV, NgayMuon, HanTra)
                                                VALUES (@MaPM, @MaDG, @MaNV, @NgayMuon, @HanTra)", conn, trans))
                {
                    insPM.Parameters.Add(new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM });
                    insPM.Parameters.Add(new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = yc.MaDocGia });
                    insPM.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV });
                    insPM.Parameters.Add(new SqlParameter("@NgayMuon", SqlDbType.Date) { Value = ngayMuon });
                    insPM.Parameters.Add(new SqlParameter("@HanTra", SqlDbType.Date) { Value = hanTra });
                    insPM.ExecuteNonQuery();
                }

                using (SqlCommand insCT = new(@"INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, TinhTrang, NgayThucTra)
                                                VALUES (@MaPM, @MaSach, N'Đang mượn', NULL)", conn, trans))
                {
                    insCT.Parameters.Add(new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM });
                    insCT.Parameters.Add(new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSachSanSang });
                    insCT.ExecuteNonQuery();
                }

                using (SqlCommand updSach = new("UPDATE Sach SET TrangThai = 1 WHERE MaSach = @MaSach", conn, trans))
                {
                    updSach.Parameters.Add(new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSachSanSang });
                    updSach.ExecuteNonQuery();
                }

                using (SqlCommand updDS = new(@"UPDATE DauSach
                                                SET SLChoMuon = SLChoMuon + 1,
                                                    SLConLai = CASE WHEN SLConLai > 0 THEN SLConLai - 1 ELSE 0 END
                                                WHERE MaDauSach = @MaDS", conn, trans))
                {
                    updDS.Parameters.Add(new SqlParameter("@MaDS", SqlDbType.VarChar, 20) { Value = yc.MaDauSach });
                    updDS.ExecuteNonQuery();
                }

                trans.Commit();
                yc.TrangThai = "Đã duyệt";
                yc.MaPMDaTao = maPM;
                return (true, $"Đã duyệt yêu cầu {yc.MaYeuCau}! Hệ thống đã tạo phiếu mượn '{maPM}' cho cuốn '{maSachSanSang}' và trừ tồn kho.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return (false, "Lỗi khi duyệt yêu cầu: " + ex.Message);
            }
        }

        public (bool ok, string message) TuChoiYeuCauMuon(string maYeuCau)
        {
            YeuCauMuonWeb? yc = yeuCauStore.LayTheoMa(maYeuCau);
            if (yc == null) return (false, "Không tìm thấy yêu cầu.");
            if (yc.TrangThai != "Chờ duyệt") return (false, "Chỉ có thể từ chối yêu cầu đang chờ duyệt.");
            yc.TrangThai = "Đã từ chối";
            return (true, $"Đã từ chối yêu cầu {maYeuCau}.");
        }
    }
}
