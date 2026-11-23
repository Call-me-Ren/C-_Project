using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>Lớp DAO truy vấn phiếu phạt và các lựa chọn liên quan, ánh xạ dữ liệu sang DTO cho lớp BUS.</summary>
    public class PhieuPhatDAO
    {
        /// <summary>Lấy danh sách độc giả để lớp GUI chọn khi lập phiếu phạt.</summary>
        /// <returns>Các lựa chọn chứa mã và nhãn độc giả; rỗng nếu chưa có độc giả.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Lấy các phiếu mượn kèm hạn trả và mã độc giả để ràng buộc lựa chọn phiếu phạt.</summary>
        /// <returns>Các lựa chọn phiếu mượn; rỗng nếu chưa có phiếu mượn.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Đọc các phiếu phạt theo ngày lập mới nhất trước.</summary>
        /// <returns>Danh sách phiếu phạt; rỗng nếu chưa có phiếu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Tìm phiếu phạt theo mã phiếu.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần tra cứu, tối đa 20 ký tự.</param>
        /// <returns>DTO phiếu nếu tìm thấy; nếu không thì <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Thêm một phiếu phạt vào cơ sở dữ liệu.</summary>
        /// <param name="pp">DTO chứa thông tin phiếu đã qua kiểm tra nghiệp vụ.</param>
        /// <returns><see langword="true"/> nếu có dòng được thêm; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh thêm thất bại, chẳng hạn do trùng mã hoặc lỗi kết nối.</exception>
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

        /// <summary>Cập nhật độc giả, phiếu mượn, nhân viên, ngày lập, trạng thái và tổng tiền của phiếu.</summary>
        /// <param name="pp">DTO chứa mã phiếu và các giá trị mới.</param>
        /// <returns><see langword="true"/> nếu có dòng được cập nhật; nếu mã không có thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh cập nhật thất bại.</exception>
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

        /// <summary>Xóa phiếu phạt theo mã; chi tiết liên quan tuân theo ràng buộc đã cấu hình trong cơ sở dữ liệu.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần xóa.</param>
        /// <returns><see langword="true"/> nếu có dòng được xóa; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh xóa thất bại do lỗi truy vấn hoặc ràng buộc dữ liệu.</exception>
        public bool Xoa(string maPhieuPhat)
        {
            string query = "DELETE FROM PhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Cập nhật tổng tiền phạt đã tính cho một phiếu.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần cập nhật.</param>
        /// <param name="tongTien">Tổng các vi phạm, tính bằng VNĐ và không âm theo nghiệp vụ.</param>
        /// <returns><see langword="true"/> nếu phiếu được cập nhật; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh cập nhật thất bại.</exception>
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

        /// <summary>Kiểm tra mã phiếu phạt đã tồn tại trong cơ sở dữ liệu hay chưa.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu tìm thấy phiếu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Kiểm tra mã độc giả có tồn tại hay không.</summary>
        /// <param name="maDG">Mã độc giả cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu có bản ghi độc giả.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Kiểm tra mã phiếu mượn có tồn tại hay không.</summary>
        /// <param name="maPM">Mã phiếu mượn cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu có bản ghi phiếu mượn.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Lấy ngày mượn của phiếu mượn theo mã.</summary>
        /// <param name="maPM">Mã phiếu mượn cần tra cứu.</param>
        /// <returns>Ngày mượn nếu tìm thấy phiếu; nếu không thì <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public DateTime? LayNgayMuon(string maPM)
        {
            string query = "SELECT NgayMuon FROM PhieuMuon WHERE MaPM = @MaPM";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPM", SqlDbType.VarChar, 20) { Value = maPM }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result == null || result == DBNull.Value ? null : Convert.ToDateTime(result);
        }

        /// <summary>Kiểm tra phiếu mượn có thuộc độc giả đã chọn không.</summary>
        /// <param name="maPM">Mã phiếu mượn cần xác minh.</param>
        /// <param name="maDG">Mã độc giả cần đối chiếu với phiếu mượn.</param>
        /// <returns><see langword="true"/> nếu phiếu mượn gắn đúng độc giả.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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

        /// <summary>Kiểm tra mã nhân viên có tồn tại để lập phiếu phạt hay không.</summary>
        /// <param name="maNV">Mã nhân viên cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu có bản ghi nhân viên.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
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
