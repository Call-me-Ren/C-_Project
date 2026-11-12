using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Lớp DAO xử lý các thao tác CSDL liên quan đến bảng PhieuMuon.
    /// Tuân thủ quy tắc: Chỉ chứa SQL, dùng SqlParameter, gọi qua MyConnection.
    /// </summary>
    public class PhieuMuonDAO
    {
        // 1. ĐỌC DANH SÁCH TẤT CẢ PHIẾU MƯỢN (KÈM TÊN ĐỘC GIẢ, NHÂN VIÊN VÀ SỐ SÁCH)
        public List<PhieuMuonDTO> DocDSPhieuMuon()
        {
            List<PhieuMuonDTO> list = new List<PhieuMuonDTO>();
            string query = @"
                SELECT 
                    pm.MaPM, 
                    pm.MaDocGia, 
                    pm.MaNV, 
                    pm.NgayMuon, 
                    pm.HanTra,
                    LTRIM(RTRIM(ISNULL(dg.HoLot, '') + ' ' + dg.TenDG)) AS TenDocGia,
                    LTRIM(RTRIM(ISNULL(nv.HoLot, '') + ' ' + nv.TenNV)) AS TenNV,
                    ISNULL(ct.TongSoSach, 0) AS TongSoSach,
                    ISNULL(ct.SoSachChuaTra, 0) AS SoSachChuaTra
                FROM PhieuMuon pm
                LEFT JOIN DocGia dg ON pm.MaDocGia = dg.MaDG
                LEFT JOIN NhanVien nv ON pm.MaNV = nv.MaNV
                LEFT JOIN (
                    SELECT 
                        MaPhieuMuon,
                        COUNT(1) AS TongSoSach,
                        SUM(CASE WHEN NgayThucTra IS NULL THEN 1 ELSE 0 END) AS SoSachChuaTra
                    FROM ChiTietPhieuMuon
                    GROUP BY MaPhieuMuon
                ) ct ON pm.MaPM = ct.MaPhieuMuon
                ORDER BY pm.NgayMuon DESC";

            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                PhieuMuonDTO pm = new PhieuMuonDTO(
                    row["MaPM"].ToString() ?? string.Empty,
                    row["MaDocGia"].ToString() ?? string.Empty,
                    row["MaNV"].ToString() ?? string.Empty,
                    Convert.ToDateTime(row["NgayMuon"]),
                    Convert.ToDateTime(row["HanTra"]),
                    row["TenDocGia"] == DBNull.Value ? null : row["TenDocGia"].ToString(),
                    row["TenNV"] == DBNull.Value ? null : row["TenNV"].ToString()
                )
                {
                    TongSoSach = Convert.ToInt32(row["TongSoSach"]),
                    SoSachChuaTra = Convert.ToInt32(row["SoSachChuaTra"])
                };

                list.Add(pm);
            }

            return list;
        }

        // 2. THÊM PHIẾU MƯỢN MỚI
        public bool ThemPhieuMuon(PhieuMuonDTO pm)
        {
            string query = @"
                INSERT INTO PhieuMuon (MaPM, MaDocGia, MaNV, NgayMuon, HanTra)
                VALUES (@MaPM, @MaDocGia, @MaNV, @NgayMuon, @HanTra)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = pm.MaPM },
                new SqlParameter("@MaDocGia", SqlDbType.VarChar, 20) { Value = pm.MaDocGia },
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = pm.MaNV },
                new SqlParameter("@NgayMuon", SqlDbType.DateTime) { Value = pm.NgayMuon },
                new SqlParameter("@HanTra", SqlDbType.Date) { Value = pm.HanTra.Date }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 3. XÓA PHIẾU MƯỢN
        public bool XoaPhieuMuon(string maPM)
        {
            string query = "DELETE FROM PhieuMuon WHERE MaPM = @MaPM";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 4. ĐẾM SỐ SÁCH MÀ ĐỘC GIẢ ĐANG MƯỢN CHƯA TRẢ
        public int DemSoSachDangMuonCuaDocGia(string maDocGia)
        {
            string query = @"
                SELECT COUNT(1)
                FROM ChiTietPhieuMuon ct
                INNER JOIN PhieuMuon pm ON ct.MaPhieuMuon = pm.MaPM
                WHERE pm.MaDocGia = @MaDocGia AND ct.NgayThucTra IS NULL";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDocGia", SqlDbType.VarChar, 20) { Value = maDocGia }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // 5. ĐỌC THAM SỐ HỆ THỐNG (Bảng 17: ThamSo)
        public string LayGiaTriThamSo(string maTS, string macDinh)
        {
            try
            {
                string query = "SELECT GiaTri FROM ThamSo WHERE MaTS = @MaTS";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaTS", SqlDbType.VarChar, 50) { Value = maTS }
                };

                object? result = MyConnection.ExecuteScalar(query, parameters);
                return result != null ? result.ToString() ?? macDinh : macDinh;
            }
            catch
            {
                return macDinh;
            }
        }

        // 6. KIỂM TRA PHIẾU MƯỢN ĐÃ CÓ PHÁT SINH PHIẾU PHẠT (Chống lủng CSDL)
        public bool KiemTraDangSuDung(string maPM)
        {
            string query = "SELECT COUNT(1) FROM PhieuPhat WHERE MaPhieuMuon = @MaPM";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM }
            };

            int count = Convert.ToInt32(MyConnection.ExecuteScalar(query, parameters));
            return count > 0;
        }
    }
}
