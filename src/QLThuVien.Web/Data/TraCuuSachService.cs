using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QLThuVien.Web.Data
{
    public class DauSachTraCuu
    {
        public string MaDauSach { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string TenTacGia { get; set; } = string.Empty;
        public string TenTheLoai { get; set; } = string.Empty;
        public string TenNXB { get; set; } = string.Empty;
        public int NamXB { get; set; }
        public int SoLuong { get; set; }
        public int SLChoMuon { get; set; }
        public int SLConLai { get; set; }
        public decimal DonGia { get; set; }

        public bool ConSach => SLConLai > 0;
    }

    public class TraCuuSachService
    {
        private const string DefaultFallbackConnectionString = @"Server=.;Database=QLThuVien;Integrated Security=True;TrustServerCertificate=True;";
        private readonly string connectionString;

        public TraCuuSachService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"]
                ?? DefaultFallbackConnectionString;
        }

        public List<DauSachTraCuu> LayDanhSachDauSach(string? tuKhoa = null)
        {
            List<DauSachTraCuu> list = new List<DauSachTraCuu>();

            string query = @"
                SELECT 
                    ds.MaDauSach,
                    ds.TenSach,
                    ISNULL(tg.TenTG, N'Chưa rõ') AS TenTacGia,
                    ISNULL(tl.TenTL, N'Chưa rõ') AS TenTheLoai,
                    ISNULL(nxb.TenNXB, N'Chưa rõ') AS TenNXB,
                    ISNULL(ds.NamXB, 0) AS NamXB,
                    ds.SoLuong,
                    ds.SLChoMuon,
                    ds.SLConLai,
                    ds.DonGia
                FROM DauSach ds
                LEFT JOIN TacGia tg ON ds.MaTG = tg.MaTG
                LEFT JOIN TheLoai tl ON ds.MaTL = tl.MaTL
                LEFT JOIN NhaXuatBan nxb ON ds.MaNXB = nxb.MaNXB
                WHERE (@TuKhoa IS NULL OR @TuKhoa = '' 
                       OR ds.TenSach LIKE '%' + @TuKhoa + '%'
                       OR tg.TenTG LIKE '%' + @TuKhoa + '%'
                       OR tl.TenTL LIKE '%' + @TuKhoa + '%'
                       OR ds.MaDauSach LIKE '%' + @TuKhoa + '%')
                ORDER BY ds.TenSach ASC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", (object?)tuKhoa?.Trim() ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new DauSachTraCuu
                                {
                                    MaDauSach = reader["MaDauSach"].ToString() ?? string.Empty,
                                    TenSach = reader["TenSach"].ToString() ?? string.Empty,
                                    TenTacGia = reader["TenTacGia"].ToString() ?? string.Empty,
                                    TenTheLoai = reader["TenTheLoai"].ToString() ?? string.Empty,
                                    TenNXB = reader["TenNXB"].ToString() ?? string.Empty,
                                    NamXB = reader["NamXB"] != DBNull.Value ? Convert.ToInt32(reader["NamXB"]) : 0,
                                    SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                                    SLChoMuon = reader["SLChoMuon"] != DBNull.Value ? Convert.ToInt32(reader["SLChoMuon"]) : 0,
                                    SLConLai = reader["SLConLai"] != DBNull.Value ? Convert.ToInt32(reader["SLConLai"]) : 0,
                                    DonGia = reader["DonGia"] != DBNull.Value ? Convert.ToDecimal(reader["DonGia"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TraCuuSachService] Error: {ex.Message}");
            }

            return list;
        }
    }
}
