using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class NhanVienDAO
    {
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

        public bool Xoa(string maNV)
        {
            string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar, 20) { Value = maNV }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

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
