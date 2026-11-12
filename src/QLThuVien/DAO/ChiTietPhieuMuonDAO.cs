using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Lớp DAO xử lý các thao tác CSDL liên quan đến bảng ChiTietPhieuMuon và cập nhật tồn kho DauSach.
    /// Tuân thủ quy tắc: Chỉ chứa SQL, dùng SqlParameter, gọi qua MyConnection.
    /// </summary>
    public class ChiTietPhieuMuonDAO
    {
        // 1. LẤY DANH SÁCH CUỐN SÁCH MƯỢN THEO MÃ PHIẾU
        public List<ChiTietPhieuMuonDTO> LayChiTietTheoPhieu(string maPM)
        {
            List<ChiTietPhieuMuonDTO> list = new List<ChiTietPhieuMuonDTO>();
            string query = @"
                SELECT 
                    ct.MaPhieuMuon,
                    ct.MaSach,
                    ct.TinhTrang,
                    ct.NgayThucTra,
                    ds.TenSach,
                    ds.MaDauSach
                FROM ChiTietPhieuMuon ct
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                INNER JOIN DauSach ds ON s.MaDauSach = ds.MaDauSach
                WHERE ct.MaPhieuMuon = @MaPM
                ORDER BY ct.MaSach ASC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                DateTime? ngayThucTra = null;
                if (row["NgayThucTra"] != DBNull.Value && DateTime.TryParse(row["NgayThucTra"].ToString(), out DateTime nt))
                {
                    ngayThucTra = nt;
                }

                ChiTietPhieuMuonDTO ct = new ChiTietPhieuMuonDTO(
                    row["MaPhieuMuon"].ToString() ?? string.Empty,
                    row["MaSach"].ToString() ?? string.Empty,
                    row["TinhTrang"] == DBNull.Value ? null : row["TinhTrang"].ToString(),
                    ngayThucTra,
                    row["TenSach"] == DBNull.Value ? null : row["TenSach"].ToString(),
                    row["MaDauSach"] == DBNull.Value ? null : row["MaDauSach"].ToString()
                );

                list.Add(ct);
            }

            return list;
        }

        // 2. THÊM CHI TIẾT PHIẾU MƯỢN
        public bool ThemChiTiet(ChiTietPhieuMuonDTO ct)
        {
            string query = @"
                INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, TinhTrang, NgayThucTra)
                VALUES (@MaPhieuMuon, @MaSach, @TinhTrang, @NgayThucTra)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuMuon", SqlDbType.VarChar, 20) { Value = ct.MaPhieuMuon },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = ct.MaSach },
                new SqlParameter("@TinhTrang", SqlDbType.NVarChar, 50) { Value = (object?)ct.TinhTrang ?? DBNull.Value },
                new SqlParameter("@NgayThucTra", SqlDbType.Date) { Value = (object?)ct.NgayThucTra ?? DBNull.Value }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 3. CẬP NHẬT TRẢ SÁCH (GHI NHẬN NGÀY THỰC TRẢ VÀ TÌNH TRẠNG KHI TRẢ)
        public bool CapNhatTraSach(string maPM, string maSach, DateTime ngayThucTra, string? tinhTrang)
        {
            string query = @"
                UPDATE ChiTietPhieuMuon
                SET NgayThucTra = @NgayThucTra,
                    TinhTrang = @TinhTrang
                WHERE MaPhieuMuon = @MaPM AND MaSach = @MaSach";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach },
                new SqlParameter("@NgayThucTra", SqlDbType.Date) { Value = ngayThucTra.Date },
                new SqlParameter("@TinhTrang", SqlDbType.NVarChar, 50) { Value = (object?)tinhTrang ?? DBNull.Value }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 4. XÓA CHI TIẾT PHIẾU MƯỢN
        public bool XoaChiTiet(string maPM, string maSach)
        {
            string query = "DELETE FROM ChiTietPhieuMuon WHERE MaPhieuMuon = @MaPM AND MaSach = @MaSach";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 5. KIỂM TRA CUỐN SÁCH CÓ ĐANG BỊ MƯỢN CHƯA TRẢ HAY KHÔNG
        public bool KiemTraSachDangMuon(string maSach)
        {
            string query = "SELECT COUNT(1) FROM ChiTietPhieuMuon WHERE MaSach = @MaSach AND NgayThucTra IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };

            int count = Convert.ToInt32(MyConnection.ExecuteScalar(query, parameters));
            return count > 0;
        }

        // 6. CẬP NHẬT TỒN KHO KHI MƯỢN SÁCH: Giảm SLConLai đi 1, tăng SLChoMuon lên 1
        public bool CapNhatTonKhoKhiMuon(string maSach)
        {
            string query = @"
                UPDATE DauSach
                SET SLConLai = SLConLai - 1,
                    SLChoMuon = SLChoMuon + 1
                WHERE MaDauSach = (SELECT MaDauSach FROM Sach WHERE MaSach = @MaSach)
                  AND SLConLai > 0";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 7. CẬP NHẬT TỒN KHO KHI TRẢ SÁCH: Tăng SLConLai lên 1, giảm SLChoMuon đi 1
        public bool CapNhatTonKhoKhiTra(string maSach)
        {
            string query = @"
                UPDATE DauSach
                SET SLConLai = SLConLai + 1,
                    SLChoMuon = CASE WHEN SLChoMuon > 0 THEN SLChoMuon - 1 ELSE 0 END
                WHERE MaDauSach = (SELECT MaDauSach FROM Sach WHERE MaSach = @MaSach)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 8. LẤY DANH SÁCH CUỐN SÁCH CÓ SẴN (SẴN SÀNG CHO MƯỢN)
        public DataTable LayDanhSachSachCoSan()
        {
            string query = @"
                SELECT 
                    s.MaSach,
                    ds.TenSach,
                    s.MaDauSach,
                    ds.SLConLai,
                    (s.MaSach + ' - ' + ds.TenSach) AS HienThi
                FROM Sach s
                INNER JOIN DauSach ds ON s.MaDauSach = ds.MaDauSach
                WHERE ds.SLConLai > 0
                  AND s.MaSach NOT IN (
                      SELECT MaSach FROM ChiTietPhieuMuon WHERE NgayThucTra IS NULL
                  )
                ORDER BY s.MaSach ASC";

            return MyConnection.ExecuteQuery(query);
        }
    }
}
