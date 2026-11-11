using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Lớp DAO phụ trách thực thi các câu lệnh SQL liên quan đến bảng DocGia.
    /// Tuân thủ quy tắc: Chỉ DAO chứa SQL, dùng SqlParameter, gọi qua MyConnection.
    /// </summary>
    public class DocGiaDAO
    {
        // Hàm tái sử dụng: Tạo SqlParameter cho Thêm và Sửa
        private SqlParameter[] TaoThamSo(DocGiaDTO dg)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = dg.MaDG },
                new SqlParameter("@HoLot", SqlDbType.NVarChar, 50) { Value = (object?)dg.HoLot ?? DBNull.Value },
                new SqlParameter("@TenDG", SqlDbType.NVarChar, 50) { Value = dg.TenDG },
                new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = (object?)dg.NgaySinh ?? DBNull.Value },
                new SqlParameter("@SDT", SqlDbType.VarChar, 15) { Value = (object?)dg.SDT ?? DBNull.Value },
                new SqlParameter("@MaLDG", SqlDbType.VarChar, 20) { Value = dg.MaLDG }
            };
        }

        // 1. ĐỌC DANH SÁCH ĐỘC GIẢ (KÈM TÊN LOẠI ĐỘC GIẢ)
        public List<DocGiaDTO> DocDSDocGia()
        {
            List<DocGiaDTO> list = new List<DocGiaDTO>();
            string query = @"
                SELECT dg.MaDG, dg.HoLot, dg.TenDG, dg.NgaySinh, dg.SDT, dg.MaLDG, ldg.TenLDG
                FROM DocGia dg
                LEFT JOIN LoaiDocGia ldg ON dg.MaLDG = ldg.MaLDG
                ORDER BY dg.MaDG ASC";

            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                DateTime? ngaySinh = null;
                if (row["NgaySinh"] != DBNull.Value && DateTime.TryParse(row["NgaySinh"].ToString(), out DateTime ns))
                {
                    ngaySinh = ns;
                }

                DocGiaDTO dg = new DocGiaDTO(
                    row["MaDG"].ToString() ?? string.Empty,
                    row["HoLot"] == DBNull.Value ? null : row["HoLot"].ToString(),
                    row["TenDG"].ToString() ?? string.Empty,
                    ngaySinh,
                    row["SDT"] == DBNull.Value ? null : row["SDT"].ToString(),
                    row["MaLDG"].ToString() ?? string.Empty,
                    row["TenLDG"] == DBNull.Value ? null : row["TenLDG"].ToString()
                );

                list.Add(dg);
            }

            return list;
        }

        // 2. THÊM ĐỘC GIẢ MỚI (INSERT)
        public bool ThemDocGia(DocGiaDTO dg)
        {
            string query = @"
                INSERT INTO DocGia (MaDG, HoLot, TenDG, NgaySinh, SDT, MaLDG)
                VALUES (@MaDG, @HoLot, @TenDG, @NgaySinh, @SDT, @MaLDG)";

            return MyConnection.ExecuteNonQuery(query, TaoThamSo(dg)) > 0;
        }

        // 3. CẬP NHẬT ĐỘC GIẢ (UPDATE)
        public bool SuaDocGia(DocGiaDTO dg)
        {
            string query = @"
                UPDATE DocGia
                SET HoLot = @HoLot,
                    TenDG = @TenDG,
                    NgaySinh = @NgaySinh,
                    SDT = @SDT,
                    MaLDG = @MaLDG
                WHERE MaDG = @MaDG";

            return MyConnection.ExecuteNonQuery(query, TaoThamSo(dg)) > 0;
        }

        // 4. XÓA ĐỘC GIẢ (DELETE)
        public bool XoaDocGia(string maDG)
        {
            string query = "DELETE FROM DocGia WHERE MaDG = @MaDG";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG }
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 5. KIỂM TRA ĐỘC GIẢ ĐANG ĐƯỢC SỬ DỤNG Ở BẢNG KHÁC (Tránh lủng CSDL)
        public bool KiemTraDangSuDung(string maDG)
        {
            // Kiểm tra trong PhieuMuon
            string queryPM = "SELECT COUNT(1) FROM PhieuMuon WHERE MaDocGia = @MaDG";
            SqlParameter[] paramPM = new SqlParameter[]
            {
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG }
            };
            int countPM = Convert.ToInt32(MyConnection.ExecuteScalar(queryPM, paramPM));
            if (countPM > 0) return true;

            // Kiểm tra trong PhieuPhat
            string queryPP = "SELECT COUNT(1) FROM PhieuPhat WHERE MaDocGia = @MaDG";
            SqlParameter[] paramPP = new SqlParameter[]
            {
                new SqlParameter("@MaDG", SqlDbType.VarChar, 20) { Value = maDG }
            };
            int countPP = Convert.ToInt32(MyConnection.ExecuteScalar(queryPP, paramPP));
            return countPP > 0;
        }
    }
}
