using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class PhieuPhatDAO
    {
        public List<LookupOptionDTO> LayDanhSachDocGiaLookup()
        {
            List<LookupOptionDTO> list = new List<LookupOptionDTO>();
            string query = @"SELECT MaDG, CONCAT(MaDG, N' - ', COALESCE(HoLot + N' ', N''), TenDG) AS HienThi
                             FROM DocGia
                             ORDER BY MaDG";
            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LookupOptionDTO(
                    row["MaDG"].ToString() ?? string.Empty,
                    row["HienThi"].ToString() ?? string.Empty));
            }
            return list;
        }

        public List<LookupOptionDTO> LayDanhSachPhieuMuonLookup()
        {
            List<LookupOptionDTO> list = new List<LookupOptionDTO>();
            string query = "SELECT MaPM, MaDocGia, HanTra FROM PhieuMuon ORDER BY MaPM";
            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                string maPM = row["MaPM"].ToString() ?? string.Empty;
                string maDG = row["MaDocGia"].ToString() ?? string.Empty;
                DateTime hanTra = Convert.ToDateTime(row["HanTra"]);
                list.Add(new LookupOptionDTO(maPM, $"{maPM} - hạn {hanTra:dd/MM/yyyy}", maDG));
            }
            return list;
        }

        public List<PhieuPhatDTO> LayDanhSach()
        {
            List<PhieuPhatDTO> list = new List<PhieuPhatDTO>();
            string query = "SELECT MaPhieuPhat, MaDocGia, MaPhieuMuon, MaNV, NgayLap, TrangThai, TongTienPhat FROM PhieuPhat ORDER BY NgayLap DESC, MaPhieuPhat DESC";
            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                PhieuPhatDTO pp = new PhieuPhatDTO(
                    row["MaPhieuPhat"].ToString() ?? string.Empty,
                    row["MaDocGia"].ToString() ?? string.Empty,
                    row["MaPhieuMuon"] == DBNull.Value ? null : row["MaPhieuMuon"].ToString(),
                    row["MaNV"].ToString() ?? string.Empty,
                    Convert.ToDateTime(row["NgayLap"]),
                    row["TrangThai"] == DBNull.Value ? "Chưa thanh toán" : row["TrangThai"].ToString() ?? "Chưa thanh toán",
                    row["TongTienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TongTienPhat"])
                );
                list.Add(pp);
            }

            return list;
        }

        public PhieuPhatDTO? LayTheoMa(string maPhieuPhat)
        {
            string query = "SELECT MaPhieuPhat, MaDocGia, MaPhieuMuon, MaNV, NgayLap, TrangThai, TongTienPhat FROM PhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new PhieuPhatDTO(
                    row["MaPhieuPhat"].ToString() ?? string.Empty,
                    row["MaDocGia"].ToString() ?? string.Empty,
                    row["MaPhieuMuon"] == DBNull.Value ? null : row["MaPhieuMuon"].ToString(),
                    row["MaNV"].ToString() ?? string.Empty,
                    Convert.ToDateTime(row["NgayLap"]),
                    row["TrangThai"] == DBNull.Value ? "Chưa thanh toán" : row["TrangThai"].ToString() ?? "Chưa thanh toán",
                    row["TongTienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TongTienPhat"])
                );
            }

            return null;
        }

        public bool Them(PhieuPhatDTO pp)
        {
            string query = @"INSERT INTO PhieuPhat (MaPhieuPhat, MaDocGia, MaPhieuMuon, MaNV, NgayLap, TrangThai, TongTienPhat)
                             VALUES (@MaPhieuPhat, @MaDocGia, @MaPhieuMuon, @MaNV, @NgayLap, @TrangThai, @TongTienPhat)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = pp.MaPhieuPhat },
                new SqlParameter("@MaDocGia", SqlDbType.VarChar, 20) { Value = pp.MaDocGia },
                new SqlParameter("@MaPhieuMuon", SqlDbType.VarChar, 20) { Value = (object?)pp.MaPhieuMuon ?? DBNull.Value },
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = pp.MaNV },
                new SqlParameter("@NgayLap", SqlDbType.DateTime) { Value = pp.NgayLap },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pp.TrangThai },
                new SqlParameter("@TongTienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = pp.TongTienPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Sua(PhieuPhatDTO pp)
        {
            string query = @"UPDATE PhieuPhat
                             SET MaDocGia = @MaDocGia, MaPhieuMuon = @MaPhieuMuon, MaNV = @MaNV, NgayLap = @NgayLap, TrangThai = @TrangThai, TongTienPhat = @TongTienPhat
                             WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = pp.MaPhieuPhat },
                new SqlParameter("@MaDocGia", SqlDbType.VarChar, 20) { Value = pp.MaDocGia },
                new SqlParameter("@MaPhieuMuon", SqlDbType.VarChar, 20) { Value = (object?)pp.MaPhieuMuon ?? DBNull.Value },
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = pp.MaNV },
                new SqlParameter("@NgayLap", SqlDbType.DateTime) { Value = pp.NgayLap },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pp.TrangThai },
                new SqlParameter("@TongTienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = pp.TongTienPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Xoa(string maPhieuPhat)
        {
            string query = "DELETE FROM PhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatTongTien(string maPhieuPhat, decimal tongTien)
        {
            string query = "UPDATE PhieuPhat SET TongTienPhat = @TongTienPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat },
                new SqlParameter("@TongTienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = tongTien }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool KiemTraTonTai(string maPhieuPhat)
        {
            string query = "SELECT COUNT(1) FROM PhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool KiemTraDocGiaTonTai(string maDG)
        {
            string query = "SELECT COUNT(1) FROM DocGia WHERE MaDG = @MaDG";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool KiemTraPhieuMuonTonTai(string maPM)
        {
            string query = "SELECT COUNT(1) FROM PhieuMuon WHERE MaPM = @MaPM";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool KiemTraPhieuMuonCuaDocGia(string maPM, string maDG)
        {
            string query = "SELECT COUNT(1) FROM PhieuMuon WHERE MaPM = @MaPM AND MaDocGia = @MaDG";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM },
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool KiemTraNhanVienTonTai(string maNV)
        {
            string query = "SELECT COUNT(1) FROM NhanVien WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }
    }
}
