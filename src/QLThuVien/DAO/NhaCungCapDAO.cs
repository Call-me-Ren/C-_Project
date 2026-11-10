using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Data Access Object cho bảng NhaCungCap
    /// Tuân thủ quy tắc kiến trúc: Chỉ chứa câu lệnh SQL, gọi qua MyConnection, dùng SqlParameter
    /// </summary>
    public class NhaCungCapDAO
    {
        public static List<NhaCungCapDTO> GetAll()
        {
            List<NhaCungCapDTO> list = new List<NhaCungCapDTO>();
            string query = "SELECT MaNCC, TenNCC FROM NhaCungCap ORDER BY MaNCC";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new NhaCungCapDTO(
                    row["MaNCC"].ToString() ?? "",
                    row["TenNCC"].ToString() ?? ""
                ));
            }

            return list;
        }

        public static NhaCungCapDTO? GetByMa(string maNCC)
        {
            string query = "SELECT MaNCC, TenNCC FROM NhaCungCap WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = maNCC }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new NhaCungCapDTO(
                    row["MaNCC"].ToString() ?? "",
                    row["TenNCC"].ToString() ?? ""
                );
            }

            return null;
        }

        public static bool Insert(NhaCungCapDTO ncc)
        {
            string query = "INSERT INTO NhaCungCap (MaNCC, TenNCC) VALUES (@MaNCC, @TenNCC)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = ncc.MaNCC },
                new SqlParameter("@TenNCC", SqlDbType.NVarChar) { Value = ncc.TenNCC }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public static bool Update(NhaCungCapDTO ncc)
        {
            string query = "UPDATE NhaCungCap SET TenNCC = @TenNCC WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenNCC", SqlDbType.NVarChar) { Value = ncc.TenNCC },
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = ncc.MaNCC }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public static bool Delete(string maNCC)
        {
            string query = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = maNCC }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        /// <summary>
        /// Kiểm tra xem Nhà cung cấp này đã có phát sinh Phiếu nhập chưa (tránh vi phạm khóa ngoại làm lủng CSDL)
        /// </summary>
        public static bool HasPhieuNhap(string maNCC)
        {
            string query = "SELECT COUNT(*) FROM PhieuNhap WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = maNCC }
            };

            object? result = MyConnection.ExecuteScalar(query, parameters);
            if (result != null && int.TryParse(result.ToString(), out int count))
            {
                return count > 0;
            }
            return false;
        }
    }
}
