using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>Lớp DAO truy vấn vi phạm, xác minh dữ liệu sách và lưu chi tiết cùng tổng tiền phiếu phạt.</summary>
    public class ChiTietPhieuPhatDAO
    {
        private const string CacCot = "MaCTPP, MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat";

        /// <summary>Đọc toàn bộ vi phạm theo mã phiếu và mã dòng.</summary>
        /// <returns>Danh sách chi tiết; rỗng nếu chưa có vi phạm.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<ChiTietPhieuPhatDTO> LayTatCa()
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat ORDER BY MaPhieuPhat, MaCTPP";
            return DocDanhSach(query);
        }

        /// <summary>Đọc các vi phạm thuộc một phiếu phạt theo thứ tự mã dòng.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần lấy chi tiết, tối đa 20 ký tự.</param>
        /// <returns>Danh sách vi phạm của phiếu; rỗng nếu phiếu chưa có chi tiết.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<ChiTietPhieuPhatDTO> LayDanhSachTheoPhieu(string maPhieuPhat)
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat ORDER BY MaCTPP";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat }
            };
            return DocDanhSach(query, parameters);
        }

        /// <summary>Tìm một vi phạm theo mã dòng tự tăng.</summary>
        /// <param name="maCTPP">Mã dòng vi phạm cần tra cứu.</param>
        /// <returns>DTO chi tiết nếu tìm thấy; nếu không thì <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public ChiTietPhieuPhatDTO? LayTheoMaCTPP(int maCTPP)
        {
            string query = $"SELECT {CacCot} FROM ChiTietPhieuPhat WHERE MaCTPP = @MaCTPP";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = maCTPP }
            };
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            return dt.Rows.Count == 0 ? null : DocDongDuLieu(dt.Rows[0]);
        }

        /// <summary>Thêm vi phạm và cập nhật tổng tiền phiếu trong cùng một giao dịch.</summary>
        /// <param name="ct">Chi tiết vi phạm đã được lớp BUS tính và kiểm tra.</param>
        /// <param name="tongTien">Tổng tiền mới của phiếu, tính bằng VNĐ.</param>
        /// <returns>Mã dòng do cơ sở dữ liệu cấp; giá trị dương nếu thành công.</returns>
        /// <exception cref="SqlException">Lưu chi tiết hoặc cập nhật tổng tiền thất bại.</exception>
        /// <remarks>Nếu một trong hai thao tác không thành công, giao dịch được hoàn tác để chi tiết và tổng tiền không lệch nhau.</remarks>
        public int ThemVaCapNhatTongTien(ChiTietPhieuPhatDTO ct, decimal tongTien)
        {
            using SqlConnection conn = MyConnection.GetConnection();
            conn.Open();
            // Chi tiết và tổng tiền phải cùng được ghi để phiếu không lưu trạng thái dở dang.
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string query = @"INSERT INTO ChiTietPhieuPhat (MaPhieuPhat, MaQD, MaSach, DonGia, SoNgayTre, GhiChu, TienPhat)
                                 OUTPUT INSERTED.MaCTPP
                                 VALUES (@MaPhieuPhat, @MaQD, @MaSach, @DonGia, @SoNgayTre, @GhiChu, @TienPhat)";
                int maCTPP;
                using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                {
                    cmd.Parameters.AddRange(TaoThamSo(ct));
                    maCTPP = Convert.ToInt32(cmd.ExecuteScalar());
                }

                CapNhatTongTienTrongGiaoDich(conn, tran, ct.MaPhieuPhat, tongTien);
                tran.Commit();
                return maCTPP;
            }
            catch
            {
                RollbackAnToan(tran);
                throw;
            }
        }

        /// <summary>Sửa vi phạm và cập nhật tổng tiền phiếu trong cùng một giao dịch.</summary>
        /// <param name="ct">Chi tiết mới, gồm mã dòng hiện có và dữ liệu đã qua lớp BUS kiểm tra.</param>
        /// <param name="tongTien">Tổng tiền mới của phiếu, tính bằng VNĐ.</param>
        /// <returns><see langword="true"/> nếu cập nhật dòng và tổng tiền; <see langword="false"/> nếu không tìm thấy dòng.</returns>
        /// <exception cref="SqlException">Lưu chi tiết hoặc cập nhật tổng tiền thất bại.</exception>
        /// <remarks>Giao dịch được hoàn tác nếu lưu dòng hoặc ghi tổng tiền gặp lỗi.</remarks>
        public bool SuaVaCapNhatTongTien(ChiTietPhieuPhatDTO ct, decimal tongTien)
        {
            using SqlConnection conn = MyConnection.GetConnection();
            conn.Open();
            // Giữ tổng tiền đồng bộ với dòng vi phạm vừa cập nhật trong cùng giao dịch.
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string query = @"UPDATE ChiTietPhieuPhat
                                 SET MaQD = @MaQD, MaSach = @MaSach, DonGia = @DonGia,
                                     SoNgayTre = @SoNgayTre, GhiChu = @GhiChu, TienPhat = @TienPhat
                                 WHERE MaCTPP = @MaCTPP";
                int affected;
                using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                {
                    List<SqlParameter> parameters = new List<SqlParameter>(TaoThamSo(ct))
                    {
                        new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = ct.MaCTPP }
                    };
                    cmd.Parameters.AddRange(parameters.ToArray());
                    affected = cmd.ExecuteNonQuery();
                }
                if (affected == 0)
                {
                    tran.Rollback();
                    return false;
                }

                CapNhatTongTienTrongGiaoDich(conn, tran, ct.MaPhieuPhat, tongTien);
                tran.Commit();
                return true;
            }
            catch
            {
                RollbackAnToan(tran);
                throw;
            }
        }

        /// <summary>Xóa vi phạm và ghi lại tổng tiền còn lại trong cùng một giao dịch.</summary>
        /// <param name="maCTPP">Mã dòng vi phạm cần xóa.</param>
        /// <param name="maPhieuPhat">Mã phiếu chứa dòng, dùng để cập nhật tổng tiền.</param>
        /// <param name="tongTien">Tổng tiền sau khi loại dòng bị xóa, tính bằng VNĐ.</param>
        /// <returns><see langword="true"/> nếu dòng được xóa và tổng tiền được cập nhật; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh xóa hoặc cập nhật tổng tiền thất bại.</exception>
        /// <remarks>Nếu không xóa được dòng, giao dịch được hoàn tác và không ghi tổng tiền mới.</remarks>
        public bool XoaVaCapNhatTongTien(int maCTPP, string maPhieuPhat, decimal tongTien)
        {
            using SqlConnection conn = MyConnection.GetConnection();
            conn.Open();
            // Xóa dòng và cập nhật tổng phải cùng thành công để không giữ tổng tiền cũ.
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string query = "DELETE FROM ChiTietPhieuPhat WHERE MaCTPP = @MaCTPP";
                using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaCTPP", SqlDbType.Int) { Value = maCTPP });
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        tran.Rollback();
                        return false;
                    }
                }

                CapNhatTongTienTrongGiaoDich(conn, tran, maPhieuPhat, tongTien);
                tran.Commit();
                return true;
            }
            catch
            {
                RollbackAnToan(tran);
                throw;
            }
        }

        private static void CapNhatTongTienTrongGiaoDich(SqlConnection conn, SqlTransaction tran, string maPhieuPhat, decimal tongTien)
        {
            const string query = "UPDATE PhieuPhat SET TongTienPhat = @TongTienPhat WHERE MaPhieuPhat = @MaPhieuPhat";
            using SqlCommand cmd = new SqlCommand(query, conn, tran);
            cmd.Parameters.Add(new SqlParameter("@TongTienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = tongTien });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat });
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("Không tìm thấy phiếu phạt cần cập nhật tổng tiền.");
        }

        private static void RollbackAnToan(SqlTransaction tran)
        {
            if (tran.Connection == null) return;
            try
            {
                tran.Rollback();
            }
            catch (InvalidOperationException)
            {
            }
            catch (SqlException)
            {
            }
        }

        /// <summary>Kiểm tra một cuốn và một quy định đã được ghi trên cùng phiếu hay chưa.</summary>
        /// <param name="maPhieuPhat">Mã phiếu phạt cần giới hạn trùng.</param>
        /// <param name="maSach">Mã cuốn sách cần đối chiếu.</param>
        /// <param name="maQD">Mã quy định cần đối chiếu.</param>
        /// <param name="boQuaMaCTPP">Mã dòng được bỏ qua khi đang sửa chính dòng đó; có thể là <see langword="null"/>.</param>
        /// <returns><see langword="true"/> nếu đã có dòng khác cùng bộ ba mã.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraTrung(string maPhieuPhat, string maSach, string maQD, int? boQuaMaCTPP = null)
        {
            string query = @"SELECT COUNT(1)
                             FROM ChiTietPhieuPhat
                             WHERE MaPhieuPhat = @MaPhieuPhat
                               AND MaQD = @MaQD
                               AND MaSach = @MaSach
                               AND (@MaCTPPBoQua IS NULL OR MaCTPP <> @MaCTPPBoQua)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = maPhieuPhat },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach },
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD },
                new SqlParameter("@MaCTPPBoQua", SqlDbType.Int) { Value = (object?)boQuaMaCTPP ?? DBNull.Value }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>Lấy các cuốn sách thuộc phiếu mượn cùng hạn trả và ngày thực trả.</summary>
        /// <param name="maPhieuMuon">Mã phiếu mượn dùng để giới hạn danh sách sách.</param>
        /// <returns>Các sách được ghi trong phiếu mượn; rỗng nếu phiếu không có chi tiết.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<SachMuonDTO> LaySachTheoPhieuMuon(string maPhieuMuon)
        {
            string query = @"SELECT cpm.MaSach, ds.TenSach, pm.HanTra, cpm.NgayThucTra
                             FROM PhieuMuon pm
                             INNER JOIN ChiTietPhieuMuon cpm ON cpm.MaPhieuMuon = pm.MaPM
                             INNER JOIN Sach s ON s.MaSach = cpm.MaSach
                             INNER JOIN DauSach ds ON ds.MaDauSach = s.MaDauSach
                             WHERE pm.MaPM = @MaPhieuMuon
                             ORDER BY cpm.MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaPhieuMuon", SqlDbType.VarChar, 20) { Value = maPhieuMuon }
            };
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            List<SachMuonDTO> list = new List<SachMuonDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SachMuonDTO(
                    row["MaSach"].ToString() ?? string.Empty,
                    row["TenSach"].ToString() ?? string.Empty,
                    Convert.ToDateTime(row["HanTra"]),
                    row["NgayThucTra"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayThucTra"])));
            }
            return list;
        }

        /// <summary>Lấy đơn giá của đầu sách chứa cuốn sách.</summary>
        /// <param name="maSach">Mã cuốn sách cần tra giá.</param>
        /// <returns>Đơn giá VNĐ; trả về 0 nếu giá trong dữ liệu là <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public decimal LayDonGiaSach(string maSach)
        {
            string query = "SELECT ISNULL(ds.DonGia, 0) FROM Sach s JOIN DauSach ds ON s.MaDauSach = ds.MaDauSach WHERE s.MaSach = @MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        /// <summary>Kiểm tra cuốn sách có tồn tại trong danh mục hay không.</summary>
        /// <param name="maSach">Mã cuốn sách cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu tìm thấy cuốn sách.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraSachTonTai(string maSach)
        {
            string query = "SELECT COUNT(1) FROM Sach WHERE MaSach = @MaSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = maSach }
            };
            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        private static SqlParameter[] TaoThamSo(ChiTietPhieuPhatDTO ct)
        {
            return new[]
            {
                new SqlParameter("@MaPhieuPhat", SqlDbType.VarChar, 20) { Value = ct.MaPhieuPhat },
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = ct.MaQD },
                new SqlParameter("@MaSach", SqlDbType.VarChar, 20) { Value = ct.MaSach },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ct.DonGia },
                new SqlParameter("@SoNgayTre", SqlDbType.Int) { Value = (object?)ct.SoNgayTre ?? DBNull.Value },
                new SqlParameter("@GhiChu", SqlDbType.NVarChar, 200) { Value = (object?)ct.GhiChu ?? DBNull.Value },
                new SqlParameter("@TienPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ct.TienPhat }
            };
        }

        private static List<ChiTietPhieuPhatDTO> DocDanhSach(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            List<ChiTietPhieuPhatDTO> list = new List<ChiTietPhieuPhatDTO>();
            foreach (DataRow row in dt.Rows) list.Add(DocDongDuLieu(row));
            return list;
        }

        private static ChiTietPhieuPhatDTO DocDongDuLieu(DataRow row)
        {
            return new ChiTietPhieuPhatDTO(
                row["MaPhieuPhat"].ToString() ?? string.Empty,
                row["MaQD"].ToString() ?? string.Empty,
                row["MaSach"].ToString() ?? string.Empty,
                row["DonGia"] == DBNull.Value ? 0 : Convert.ToDecimal(row["DonGia"]),
                row["SoNgayTre"] == DBNull.Value ? null : Convert.ToInt32(row["SoNgayTre"]),
                row["GhiChu"] == DBNull.Value ? null : row["GhiChu"].ToString(),
                row["TienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TienPhat"]),
                Convert.ToInt32(row["MaCTPP"]));
        }
    }
}
