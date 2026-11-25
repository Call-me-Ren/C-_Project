#nullable disable
using QLThuVien.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace QLThuVien.DAO
{
    public class DauSachDAO
    {
        // 1. Đọc danh sách Đầu sách từ CSDL
        public List<DauSachDTO> DocDSSach()
        {
            List<DauSachDTO> dsSach = new List<DauSachDTO>();
            string query = @"SELECT ds.MaDauSach, ds.TenSach, tg.TenTG, tl.TenTL, nxb.TenNXB, 
                                    ds.NamXB, ds.SoLuong, ds.SLChoMuon, ds.SLConLai, ds.DonGia,
                                    ds.MaTG, ds.MaTL, ds.MaNXB
                             FROM DauSach ds
                             LEFT JOIN TacGia tg ON ds.MaTG = tg.MaTG
                             LEFT JOIN TheLoai tl ON ds.MaTL = tl.MaTL
                             LEFT JOIN NhaXuatBan nxb ON ds.MaNXB = nxb.MaNXB";

            using (SqlDataReader reader = MyConnection.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    DauSachDTO ds = new DauSachDTO
                    {
                        MaDauSach = reader["MaDauSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        NamXB = reader["NamXB"] != DBNull.Value ? Convert.ToInt32(reader["NamXB"]) : null,
                        SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                        SLChoMuon = reader["SLChoMuon"] != DBNull.Value ? Convert.ToInt32(reader["SLChoMuon"]) : 0,
                        SLConLai = reader["SLConLai"] != DBNull.Value ? Convert.ToInt32(reader["SLConLai"]) : 0,
                        DonGia = reader["DonGia"] != DBNull.Value ? Convert.ToDecimal(reader["DonGia"]) : 0,
                        MaTG = reader["MaTG"].ToString(),
                        MaTL = reader["MaTL"].ToString(),
                        MaNXB = reader["MaNXB"].ToString()
                    };
                    dsSach.Add(ds);
                }
            }
            return dsSach;
        }

        // 2. Thêm Đầu sách mới
        public void Them(DauSachDTO ds)
        {
            const string query = @"INSERT INTO DauSach (MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, SoLuong, SLChoMuon, SLConLai, DonGia) 
                                   VALUES (@MaDauSach, @MaNXB, @MaTG, @MaTL, @TenSach, @NamXB, 0, 0, 0, @DonGia)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = ds.MaDauSach },
                new SqlParameter("@MaNXB", SqlDbType.VarChar, 20) { Value = (object)ds.MaNXB ?? DBNull.Value },
                new SqlParameter("@MaTG", SqlDbType.VarChar, 20) { Value = (object)ds.MaTG ?? DBNull.Value },
                new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = (object)ds.MaTL ?? DBNull.Value },
                new SqlParameter("@TenSach", SqlDbType.NVarChar, 200) { Value = ds.TenSach },
                new SqlParameter("@NamXB", SqlDbType.Int) { Value = (object)ds.NamXB ?? DBNull.Value },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ds.DonGia }
            };
            MyConnection.ExecuteNonQuery(query, parameters);
        }

        // 3. Sửa Đầu sách (Cập nhật theo Mã Cũ)
        public void Sua(DauSachDTO ds, string maCu)
        {
            const string query = @"UPDATE DauSach 
                                   SET MaDauSach = @MaDauSachMoi, MaNXB = @MaNXB, MaTG = @MaTG, MaTL = @MaTL, 
                                       TenSach = @TenSach, NamXB = @NamXB, DonGia = @DonGia 
                                   WHERE MaDauSach = @MaDauSachCu";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDauSachMoi", SqlDbType.VarChar, 20) { Value = ds.MaDauSach },
                new SqlParameter("@MaDauSachCu", SqlDbType.VarChar, 20) { Value = maCu },
                new SqlParameter("@MaNXB", SqlDbType.VarChar, 20) { Value = (object)ds.MaNXB ?? DBNull.Value },
                new SqlParameter("@MaTG", SqlDbType.VarChar, 20) { Value = (object)ds.MaTG ?? DBNull.Value },
                new SqlParameter("@MaTL", SqlDbType.VarChar, 20) { Value = (object)ds.MaTL ?? DBNull.Value },
                new SqlParameter("@TenSach", SqlDbType.NVarChar, 200) { Value = ds.TenSach },
                new SqlParameter("@NamXB", SqlDbType.Int) { Value = (object)ds.NamXB ?? DBNull.Value },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ds.DonGia }
            };
            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            if (rows == 0)
            {
                throw new InvalidOperationException($"Không tìm thấy đầu sách mã '{maCu}' trong CSDL để cập nhật.");
            }
        }

        // 4. Xóa Đầu sách
        public void Xoa(string maDauSach)
        {
            try
            {
                const string query = "DELETE FROM DauSach WHERE MaDauSach = @MaDauSach";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = maDauSach }
                };
                MyConnection.ExecuteNonQuery(query, parameters);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa đầu sách vì vẫn còn cuốn sách hoặc dữ liệu mượn liên quan.", ex);
            }
        }

        // 5. Đọc 1 Đầu sách theo Mã
        public DauSachDTO DocTheoMa(string maDauSach)
        {
            const string query = "SELECT * FROM DauSach WHERE MaDauSach = @MaDauSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = maDauSach }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new DauSachDTO
                {
                    MaDauSach = row["MaDauSach"].ToString(),
                    TenSach = row["TenSach"].ToString(),
                    NamXB = row["NamXB"] != DBNull.Value ? Convert.ToInt32(row["NamXB"]) : null,
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    SLChoMuon = Convert.ToInt32(row["SLChoMuon"]),
                    SLConLai = Convert.ToInt32(row["SLConLai"]),
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    MaTG = row["MaTG"].ToString(),
                    MaTL = row["MaTL"].ToString(),
                    MaNXB = row["MaNXB"].ToString()
                };
            }
            return null;
        }

        // 6. Tính toán & Đồng bộ lại số lượng từ bảng Sach (loại trừ sách đã bị đánh dấu 'Mất')
        public void DongBoSoLuong(string maDauSach)
        {
            const string query = @"UPDATE ds
                                   SET ds.SoLuong = ISNULL(x.TongSo, 0),
                                       ds.SLChoMuon = ISNULL(x.SoMuon, 0),
                                       ds.SLConLai = ISNULL(x.TongSo, 0) - ISNULL(x.SoMuon, 0)
                                   FROM DauSach ds
                                   LEFT JOIN (
                                       SELECT 
                                           MaDauSach,
                                           COUNT(*) AS TongSo,
                                           SUM(CASE WHEN CAST(TrangThai AS VARCHAR) = '1' OR CAST(TrangThai AS VARCHAR) = 'True' THEN 1 ELSE 0 END) AS SoMuon
                                       FROM Sach
                                       WHERE MaDauSach = @MaDauSach
                                         AND ISNULL(TinhTrang, N'') <> N'Mất'
                                       GROUP BY MaDauSach
                                   ) x ON ds.MaDauSach = x.MaDauSach
                                   WHERE ds.MaDauSach = @MaDauSach";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDauSach", SqlDbType.VarChar, 20) { Value = maDauSach }
            };
            MyConnection.ExecuteNonQuery(query, parameters);
        }
    }
}
