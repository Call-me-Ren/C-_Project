#nullable disable
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

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
            try
            {
                string query = @"INSERT INTO DauSach (MaDauSach, MaNXB, MaTG, MaTL, TenSach, NamXB, SoLuong, SLChoMuon, SLConLai, DonGia) 
                                 VALUES (@MaDauSach, @MaNXB, @MaTG, @MaTL, @TenSach, @NamXB, 0, 0, 0, @DonGia)";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDauSach", ds.MaDauSach);
                    cmd.Parameters.AddWithValue("@MaNXB", (object)ds.MaNXB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTG", (object)ds.MaTG ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTL", (object)ds.MaTL ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenSach", ds.TenSach);
                    cmd.Parameters.AddWithValue("@NamXB", (object)ds.NamXB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DonGia", ds.DonGia);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. Sửa Đầu sách (Cập nhật theo Mã Cũ)
        public void Sua(DauSachDTO ds, string maCu)
        {
            try
            {
                string query = @"UPDATE DauSach 
                                 SET MaDauSach = @MaDauSachMoi, MaNXB = @MaNXB, MaTG = @MaTG, MaTL = @MaTL, 
                                     TenSach = @TenSach, NamXB = @NamXB, DonGia = @DonGia 
                                 WHERE MaDauSach = @MaDauSachCu";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDauSachMoi", ds.MaDauSach);
                    cmd.Parameters.AddWithValue("@MaDauSachCu", maCu);
                    cmd.Parameters.AddWithValue("@MaNXB", (object)ds.MaNXB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTG", (object)ds.MaTG ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTL", (object)ds.MaTL ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenSach", ds.TenSach);
                    cmd.Parameters.AddWithValue("@NamXB", (object)ds.NamXB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DonGia", ds.DonGia);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show($"Không tìm thấy đầu sách mã '{maCu}' trong CSDL để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật Đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. Xóa Đầu sách
        public void Xoa(string maDauSach)
        {
            try
            {
                string query = "DELETE FROM DauSach WHERE MaDauSach = @MaDauSach";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Không thể xóa đầu sách vì vẫn còn cuốn sách hoặc dữ liệu mượn liên quan.", ex);
            }
        }

        // 5. Đọc 1 Đầu sách theo Mã
        public DauSachDTO DocTheoMa(string maDauSach)
        {
            string query = "SELECT * FROM DauSach WHERE MaDauSach = @MaDauSach";
            using (SqlConnection conn = MyConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DauSachDTO
                        {
                            MaDauSach = reader["MaDauSach"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            NamXB = reader["NamXB"] != DBNull.Value ? Convert.ToInt32(reader["NamXB"]) : null,
                            SoLuong = Convert.ToInt32(reader["SoLuong"]),
                            SLChoMuon = Convert.ToInt32(reader["SLChoMuon"]),
                            SLConLai = Convert.ToInt32(reader["SLConLai"]),
                            DonGia = Convert.ToDecimal(reader["DonGia"]),
                            MaTG = reader["MaTG"].ToString(),
                            MaTL = reader["MaTL"].ToString(),
                            MaNXB = reader["MaNXB"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // 6. Tính toán & Đồng bộ lại số lượng từ bảng Sach
        public void DongBoSoLuong(string maDauSach)
        {
            try
            {
                string query = @"UPDATE ds
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
                                     GROUP BY MaDauSach
                                 ) x ON ds.MaDauSach = x.MaDauSach
                                 WHERE ds.MaDauSach = @MaDauSach";

                using (SqlConnection conn = MyConnection.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đồng bộ số lượng Đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
