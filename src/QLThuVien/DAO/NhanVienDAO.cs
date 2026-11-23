using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>Lớp DAO truy vấn và cập nhật bảng nhân viên, ánh xạ dữ liệu sang <see cref="NhanVienDTO"/>.</summary>
    public class NhanVienDAO
    {
        /// <summary>Đọc toàn bộ nhân viên theo mã tăng dần.</summary>
        /// <returns>Danh sách nhân viên; rỗng nếu bảng chưa có dữ liệu.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public List<NhanVienDTO> LayDanhSach()
        {
            List<NhanVienDTO> list = new List<NhanVienDTO>();
            string query = "SELECT MaNV, HoLot, TenNV, ChucVu FROM NhanVien ORDER BY MaNV ASC";
            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                NhanVienDTO nv = new NhanVienDTO(
                    row["MaNV"].ToString() ?? string.Empty,
                    row["HoLot"] == DBNull.Value ? null : row["HoLot"].ToString(),
                    row["TenNV"].ToString() ?? string.Empty,
                    row["ChucVu"] == DBNull.Value ? null : row["ChucVu"].ToString()
                );
                list.Add(nv);
            }

            return list;
        }

        /// <summary>Tìm một nhân viên theo mã định danh.</summary>
        /// <param name="maNV">Mã nhân viên cần tìm, tối đa 20 ký tự.</param>
        /// <returns>DTO của nhân viên nếu tìm thấy; nếu không thì <see langword="null"/>.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public NhanVienDTO? LayTheoMa(string maNV)
        {
            string query = "SELECT MaNV, HoLot, TenNV, ChucVu FROM NhanVien WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new NhanVienDTO(
                    row["MaNV"].ToString() ?? string.Empty,
                    row["HoLot"] == DBNull.Value ? null : row["HoLot"].ToString(),
                    row["TenNV"].ToString() ?? string.Empty,
                    row["ChucVu"] == DBNull.Value ? null : row["ChucVu"].ToString()
                );
            }

            return null;
        }

        /// <summary>Thêm một bản ghi nhân viên vào cơ sở dữ liệu.</summary>
        /// <param name="nv">DTO chứa mã, họ lót, tên và chức vụ đã được lớp BUS kiểm tra.</param>
        /// <returns><see langword="true"/> nếu có dòng được thêm; ngược lại là <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh thêm thất bại, chẳng hạn do trùng khóa hoặc lỗi kết nối.</exception>
        public bool Them(NhanVienDTO nv)
        {
            string query = "INSERT INTO NhanVien (MaNV, HoLot, TenNV, ChucVu) VALUES (@MaNV, @HoLot, @TenNV, @ChucVu)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = nv.MaNV },
                new SqlParameter("@HoLot", SqlDbType.NVarChar, 50) { Value = (object?)nv.HoLot ?? DBNull.Value },
                new SqlParameter("@TenNV", SqlDbType.NVarChar, 50) { Value = nv.TenNV },
                new SqlParameter("@ChucVu", SqlDbType.NVarChar, 50) { Value = (object?)nv.ChucVu ?? DBNull.Value }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Cập nhật họ tên và chức vụ của nhân viên theo mã.</summary>
        /// <param name="nv">DTO chứa mã nhân viên và các giá trị cần ghi.</param>
        /// <returns><see langword="true"/> nếu có dòng được cập nhật; nếu mã không có thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh cập nhật thất bại.</exception>
        public bool Sua(NhanVienDTO nv)
        {
            string query = "UPDATE NhanVien SET HoLot = @HoLot, TenNV = @TenNV, ChucVu = @ChucVu WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = nv.MaNV },
                new SqlParameter("@HoLot", SqlDbType.NVarChar, 50) { Value = (object?)nv.HoLot ?? DBNull.Value },
                new SqlParameter("@TenNV", SqlDbType.NVarChar, 50) { Value = nv.TenNV },
                new SqlParameter("@ChucVu", SqlDbType.NVarChar, 50) { Value = (object?)nv.ChucVu ?? DBNull.Value }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Xóa nhân viên theo mã.</summary>
        /// <param name="maNV">Mã nhân viên cần xóa, tối đa 20 ký tự.</param>
        /// <returns><see langword="true"/> nếu có dòng được xóa; nếu mã không có thì <see langword="false"/>.</returns>
        /// <exception cref="SqlException">Lệnh xóa thất bại, bao gồm trường hợp còn ràng buộc dữ liệu.</exception>
        public bool Xoa(string maNV)
        {
            string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>Kiểm tra mã nhân viên đã tồn tại trong bảng hay chưa.</summary>
        /// <param name="maNV">Mã nhân viên cần tra cứu.</param>
        /// <returns><see langword="true"/> nếu có bản ghi mang mã đó.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraTonTai(string maNV)
        {
            string query = "SELECT COUNT(1) FROM NhanVien WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>Kiểm tra nhân viên có đang được phiếu nhập, phiếu mượn hoặc phiếu phạt tham chiếu không.</summary>
        /// <param name="maNV">Mã nhân viên cần kiểm tra ràng buộc.</param>
        /// <returns><see langword="true"/> nếu tồn tại ít nhất một giao dịch tham chiếu mã này.</returns>
        /// <exception cref="SqlException">Truy vấn cơ sở dữ liệu thất bại.</exception>
        public bool KiemTraKhoaNgoai(string maNV)
        {
            string query = @"SELECT
                (SELECT COUNT(1) FROM PhieuNhap WHERE MaNV = @MaNV) +
                (SELECT COUNT(1) FROM PhieuMuon WHERE MaNV = @MaNV) +
                (SELECT COUNT(1) FROM PhieuPhat WHERE MaNV = @MaNV)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }
    }
}
