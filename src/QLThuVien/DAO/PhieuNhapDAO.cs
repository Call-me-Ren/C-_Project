using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    public class PhieuNhapDAO
    {
        public static List<PhieuNhapDTO> GetAll()
        {
            List<PhieuNhapDTO> list = new List<PhieuNhapDTO>();
            string query = "SELECT MaPN, MaNCC, MaNV, NgayNhap, TongTien FROM PhieuNhap ORDER BY NgayNhap DESC";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new PhieuNhapDTO
                {
                    MaPN = row["MaPN"].ToString() ?? "",
                    MaNCC = row["MaNCC"].ToString() ?? "",
                    MaNV = row["MaNV"].ToString() ?? "",
                    NgayNhap = Convert.ToDateTime(row["NgayNhap"]),
                    TongTien = Convert.ToDecimal(row["TongTien"])
                });
            }
            return list;
        }

        public static bool Insert(PhieuNhapDTO pn)
        {
            string query = @"INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien)
                             VALUES (@MaPN, @MaNCC, @MaNV, @NgayNhap, @TongTien)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = pn.MaPN },
                new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = pn.MaNCC },
                new SqlParameter("@MaNV", SqlDbType.VarChar) { Value = pn.MaNV },
                new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = pn.NgayNhap },
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = pn.TongTien }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Lưu toàn bộ Phiếu Nhập, Chi tiết, cập nhật tồn kho DauSach và sinh mã Sach mới trong một Transaction
        /// Đảm bảo tính toàn vẹn CSDL (ACID), tuyệt đối không để lủng dữ liệu nếu có lỗi xảy ra.
        /// </summary>
        public static (bool Success, string Message) LuuPhieuNhapTransaction(PhieuNhapDTO pn, List<ChiTietPhieuNhapDTO> chiTietList)
        {
            using (SqlConnection conn = MyConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm PhieuNhap
                        string sqlPN = @"INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien)
                                         VALUES (@MaPN, @MaNCC, @MaNV, @NgayNhap, @TongTien)";
                        using (SqlCommand cmdPN = new SqlCommand(sqlPN, conn, tran))
                        {
                            cmdPN.Parameters.Add(new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = pn.MaPN });
                            cmdPN.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.VarChar) { Value = pn.MaNCC });
                            cmdPN.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.VarChar) { Value = pn.MaNV });
                            cmdPN.Parameters.Add(new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = pn.NgayNhap });
                            cmdPN.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = pn.TongTien });
                            cmdPN.ExecuteNonQuery();
                        }

                        // 2. Thêm từng ChiTietPhieuNhap, tăng tồn kho DauSach và sinh Sach mới
                        foreach (var ct in chiTietList)
                        {
                            // 2.1 Thêm ChiTietPhieuNhap
                            string sqlCT = @"INSERT INTO ChiTietPhieuNhap (MaPN, MaDauSach, DonGia, SoLuong, ThanhTien)
                                             VALUES (@MaPN, @MaDauSach, @DonGia, @SoLuong, @ThanhTien)";
                            using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                            {
                                cmdCT.Parameters.Add(new SqlParameter("@MaPN", SqlDbType.VarChar) { Value = pn.MaPN });
                                cmdCT.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = ct.MaDauSach });
                                cmdCT.Parameters.Add(new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = ct.DonGia });
                                cmdCT.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Int) { Value = ct.SoLuong });
                                cmdCT.Parameters.Add(new SqlParameter("@ThanhTien", SqlDbType.Decimal) { Value = ct.ThanhTien });
                                cmdCT.ExecuteNonQuery();
                            }

                            // 2.2 Tăng tồn kho DauSach (SoLuong + SLConLai)
                            string sqlDauSach = @"UPDATE DauSach 
                                                  SET SoLuong = SoLuong + @SoLuong,
                                                      SLConLai = SLConLai + @SoLuong
                                                  WHERE MaDauSach = @MaDauSach";
                            using (SqlCommand cmdDS = new SqlCommand(sqlDauSach, conn, tran))
                            {
                                cmdDS.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Int) { Value = ct.SoLuong });
                                cmdDS.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = ct.MaDauSach });
                                cmdDS.ExecuteNonQuery();
                            }

                            // 2.3 Lấy số lượng cuốn sách hiện có để sinh tiếp mã sách vật lý
                            string sqlCount = "SELECT COUNT(*) FROM Sach WHERE MaDauSach = @MaDauSach";
                            int currentCount = 0;
                            using (SqlCommand cmdCount = new SqlCommand(sqlCount, conn, tran))
                            {
                                cmdCount.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = ct.MaDauSach });
                                object? countObj = cmdCount.ExecuteScalar();
                                currentCount = countObj != null ? Convert.ToInt32(countObj) : 0;
                            }

                            // 2.4 Sinh mã và chèn từng cuốn sách vật lý vào bảng Sach
                            for (int i = 1; i <= ct.SoLuong; i++)
                            {
                                int nextIndex = currentCount + i;
                                string maSachMoi = $"{ct.MaDauSach}_{nextIndex:D3}";

                                string sqlSach = @"INSERT INTO Sach (MaSach, MaDauSach, TinhTrang, ViTri) 
                                                   VALUES (@MaSach, @MaDauSach, N'Mới', N'Kho chính')";
                                using (SqlCommand cmdSach = new SqlCommand(sqlSach, conn, tran))
                                {
                                    cmdSach.Parameters.Add(new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSachMoi });
                                    cmdSach.Parameters.Add(new SqlParameter("@MaDauSach", SqlDbType.VarChar) { Value = ct.MaDauSach });
                                    cmdSach.ExecuteNonQuery();
                                }
                            }
                        }

                        // Hoàn tất mọi thao tác thành công -> Commit transaction
                        tran.Commit();
                        return (true, "Tạo phiếu nhập, cập nhật tồn kho và sinh mã sách thành công!");
                    }
                    catch (Exception ex)
                    {
                        // Gặp bất kỳ lỗi nào -> Rollback toàn bộ
                        tran.Rollback();
                        return (false, "Lỗi khi lưu dữ liệu (Giao dịch đã được hủy an toàn): " + ex.Message);
                    }
                }
            }
        }
    }
}
