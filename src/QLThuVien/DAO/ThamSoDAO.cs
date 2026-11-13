using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Data Access Object cho bảng ThamSo
    /// </summary>
    public class ThamSoDAO
    {
        public List<ThamSoDTO> DocDS()
        {
            KiemTraVaKhoiTaoBang();
            List<ThamSoDTO> list = new List<ThamSoDTO>();
            string query = "SELECT MaTS, TenTS, GiaTri FROM ThamSo ORDER BY MaTS";

            DataTable dt = MyConnection.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ThamSoDTO(
                    row["MaTS"].ToString() ?? "",
                    row["TenTS"].ToString() ?? "",
                    row["GiaTri"].ToString() ?? ""
                ));
            }

            return list;
        }

        /// <summary>
        /// Tự động kiểm tra và khởi tạo bảng ThamSo kèm dữ liệu mẫu nếu CSDL chưa có
        /// (Giúp phần mềm tự phục hồi ngay cả khi người dùng chưa chạy script SQL trên SSMS)
        /// </summary>
        private void KiemTraVaKhoiTaoBang()
        {
            try
            {
                string sqlCheck = @"
                    IF OBJECT_ID('ThamSo', 'U') IS NULL
                    BEGIN
                        CREATE TABLE ThamSo (
                            MaTS VARCHAR(50) NOT NULL PRIMARY KEY,
                            TenTS NVARCHAR(100) NOT NULL,
                            GiaTri NVARCHAR(100) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT 1 FROM ThamSo)
                    BEGIN
                        INSERT INTO ThamSo (MaTS, TenTS, GiaTri) VALUES
                        ('SO_SACH_MUON_TOI_DA', N'Số cuốn sách tối đa mỗi lần mượn', '5'),
                        ('SO_NGAY_MUON_TOI_DA', N'Số ngày mượn tối đa mặc định', '7'),
                        ('TIEN_PHAT_MOI_NGAY', N'Tiền phạt trễ hạn mỗi ngày (VNĐ)', '5000'),
                        ('NAM_XB_TOI_THIEU', N'Năm xuất bản tối thiểu hợp lệ', '1900');
                    END
                ";
                MyConnection.ExecuteNonQuery(sqlCheck);
            }
            catch
            {
                // Bỏ qua nếu người dùng không đủ quyền DDL
            }
        }

        public ThamSoDTO? LayTheoMa(string maTS)
        {
            string query = "SELECT MaTS, TenTS, GiaTri FROM ThamSo WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaTS", SqlDbType.VarChar) { Value = maTS }
            };

            DataTable dt = MyConnection.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new ThamSoDTO(
                    row["MaTS"].ToString() ?? "",
                    row["TenTS"].ToString() ?? "",
                    row["GiaTri"].ToString() ?? ""
                );
            }

            return null;
        }

        public bool CapNhatGiaTri(string maTS, string giaTriMoi)
        {
            string query = "UPDATE ThamSo SET GiaTri = @GiaTri WHERE MaTS = @MaTS";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@GiaTri", SqlDbType.NVarChar) { Value = giaTriMoi },
                new SqlParameter("@MaTS", SqlDbType.VarChar) { Value = maTS }
            };

            int rows = MyConnection.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }
    }
}
