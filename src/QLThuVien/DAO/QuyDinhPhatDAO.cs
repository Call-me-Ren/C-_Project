using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>Lớp DAO đọc và ghi quy định phạt, ánh xạ dữ liệu cơ sở dữ liệu sang <see cref="QuyDinhPhatDTO"/>.</summary>
    public class QuyDinhPhatDAO
    {
        /// <summary>Đọc toàn bộ quy định phạt theo mã tăng dần.</summary>
        /// <returns>Danh sách quy định; rỗng nếu chưa có dữ liệu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<QuyDinhPhatDTO> LayDanhSach()
        {
            List<QuyDinhPhatDTO> list = new List<QuyDinhPhatDTO>();
            string query = "SELECT MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat FROM QuyDinhPhat ORDER BY MaQD ASC";
            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                QuyDinhPhatDTO qd = new QuyDinhPhatDTO(
                    row["MaQD"].ToString() ?? string.Empty,
                    row["TenQD"].ToString() ?? string.Empty,
                    row["LoaiPhat"] == DBNull.Value ? null : row["LoaiPhat"].ToString(),
                    row["CachTinh"] == DBNull.Value ? null : row["CachTinh"].ToString(),
                    row["DonViTinh"] == DBNull.Value ? null : row["DonViTinh"].ToString(),
                    row["MucPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["MucPhat"])
                );
                list.Add(qd);
            }

            return list;
        }

        /// <summary>Tìm một quy định phạt theo mã.</summary>
        /// <param name="maQD">Mã quy định cần tìm, tối đa 20 ký tự.</param>
        /// <returns>DTO quy định nếu tìm thấy; nếu không thì <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public QuyDinhPhatDTO? LayTheoMa(string maQD)
        {
            string query = "SELECT MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new QuyDinhPhatDTO(
                    row["MaQD"].ToString() ?? string.Empty,
                    row["TenQD"].ToString() ?? string.Empty,
                    row["LoaiPhat"] == DBNull.Value ? null : row["LoaiPhat"].ToString(),
                    row["CachTinh"] == DBNull.Value ? null : row["CachTinh"].ToString(),
                    row["DonViTinh"] == DBNull.Value ? null : row["DonViTinh"].ToString(),
                    row["MucPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(row["MucPhat"])
                );
            }

            return null;
        }

        /// <summary>Thêm một quy định phạt đã được lớp BUS kiểm tra.</summary>
        /// <param name="qd">DTO chứa mã, tên, loại, cách tính, đơn vị và mức phạt.</param>
        /// <returns><see langword="true"/> nếu có dòng được thêm; nếu không thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh thêm thất bại, chẳng hạn do trùng khóa hoặc lỗi kết nối.</exception>
        public bool Them(QuyDinhPhatDTO qd)
        {
            string query = @"INSERT INTO QuyDinhPhat (MaQD, TenQD, LoaiPhat, CachTinh, DonViTinh, MucPhat)
                             VALUES (@MaQD, @TenQD, @LoaiPhat, @CachTinh, @DonViTinh, @MucPhat)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = qd.MaQD },
                new SqlParameter("@TenQD", SqlDbType.NVarChar, 100) { Value = qd.TenQD },
                new SqlParameter("@LoaiPhat", SqlDbType.NVarChar, 50) { Value = (object?)qd.LoaiPhat ?? DBNull.Value },
                new SqlParameter("@CachTinh", SqlDbType.NVarChar, 100) { Value = (object?)qd.CachTinh ?? DBNull.Value },
                new SqlParameter("@DonViTinh", SqlDbType.NVarChar, 20) { Value = (object?)qd.DonViTinh ?? DBNull.Value },
                new SqlParameter("@MucPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = qd.MucPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Cập nhật cấu hình của quy định theo mã.</summary>
        /// <param name="qd">DTO chứa mã và các giá trị cấu hình cần ghi.</param>
        /// <returns><see langword="true"/> nếu có dòng được cập nhật; nếu mã không có thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh cập nhật thất bại.</exception>
        public bool Sua(QuyDinhPhatDTO qd)
        {
            string query = @"UPDATE QuyDinhPhat
                             SET TenQD = @TenQD, LoaiPhat = @LoaiPhat, CachTinh = @CachTinh, DonViTinh = @DonViTinh, MucPhat = @MucPhat
                             WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = qd.MaQD },
                new SqlParameter("@TenQD", SqlDbType.NVarChar, 100) { Value = qd.TenQD },
                new SqlParameter("@LoaiPhat", SqlDbType.NVarChar, 50) { Value = (object?)qd.LoaiPhat ?? DBNull.Value },
                new SqlParameter("@CachTinh", SqlDbType.NVarChar, 100) { Value = (object?)qd.CachTinh ?? DBNull.Value },
                new SqlParameter("@DonViTinh", SqlDbType.NVarChar, 20) { Value = (object?)qd.DonViTinh ?? DBNull.Value },
                new SqlParameter("@MucPhat", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = qd.MucPhat }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Xóa quy định phạt theo mã.</summary>
        /// <param name="maQD">Mã quy định cần xóa.</param>
        /// <returns><see langword="true"/> nếu có dòng được xóa; nếu mã không có thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh xóa thất bại, bao gồm trường hợp vi phạm còn tham chiếu quy định.</exception>
        public bool Xoa(string maQD)
        {
            string query = "DELETE FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Kiểm tra quy định có tồn tại trong bảng hay không.</summary>
        /// <param name="maQD">Mã quy định cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu tìm thấy bản ghi.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraTonTai(string maQD)
        {
            string query = "SELECT COUNT(1) FROM QuyDinhPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>Kiểm tra có chi tiết phiếu phạt nào đang dùng quy định này không.</summary>
        /// <param name="maQD">Mã quy định cần kiểm tra ràng buộc.</param>
        /// <returns><see langword="true"/> nếu ít nhất một chi tiết tham chiếu mã quy định.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraKhoaNgoai(string maQD)
        {
            string query = "SELECT COUNT(1) FROM ChiTietPhieuPhat WHERE MaQD = @MaQD";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaQD", SqlDbType.VarChar, 20) { Value = maQD }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }
    }
}
