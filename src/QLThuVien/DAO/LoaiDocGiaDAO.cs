using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLThuVien.DTO;

namespace QLThuVien.DAO
{
    /// <summary>
    /// Lớp DAO phụ trách thực thi các câu truy vấn SQL liên quan đến bảng LoaiDocGia.
    /// Áp dụng nguyên lý DRY: Tái sử dụng việc tạo SqlParameter cho Thêm và Sửa.
    /// </summary>
    public class LoaiDocGiaDAO
    {
        // HÀM DÙNG CHUNG: Gom việc tạo SqlParameter cho LoaiDocGia vào 1 chỗ duy nhất
        private SqlParameter[] TaoThamSo(LoaiDocGiaDTO ldg)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaLDG", ldg.MaLDG),
                new SqlParameter("@TenLDG", ldg.TenLDG)
            };
        }

        // 1. ĐỌC TOÀN BỘ DANH SÁCH LOẠI ĐỘC GIẢ (SELECT)
        public List<LoaiDocGiaDTO> DocDSLoaiDocGia()
        {
            List<LoaiDocGiaDTO> danhSach = new List<LoaiDocGiaDTO>();
            string query = "SELECT MaLDG, TenLDG FROM LoaiDocGia";

            DataTable dt = MyConnection.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaLDG"].ToString() ?? string.Empty;
                string ten = row["TenLDG"].ToString() ?? string.Empty;

                danhSach.Add(new LoaiDocGiaDTO(ma, ten));
            }
            return danhSach;
        }

        // 2. THÊM MỚI 1 LOẠI ĐỘC GIẢ (INSERT) - Tái sử dụng TaoThamSo
        public bool ThemLoaiDocGia(LoaiDocGiaDTO ldg)
        {
            string query = "INSERT INTO LoaiDocGia (MaLDG, TenLDG) VALUES (@MaLDG, @TenLDG)";
            return MyConnection.ExecuteNonQuery(query, TaoThamSo(ldg)) > 0;
        }

        // 3. CẬP NHẬT THÔNG TIN LOẠI ĐỘC GIẢ (UPDATE) - Tái sử dụng TaoThamSo
        public bool SuaLoaiDocGia(LoaiDocGiaDTO ldg)
        {
            string query = "UPDATE LoaiDocGia SET TenLDG = @TenLDG WHERE MaLDG = @MaLDG";
            return MyConnection.ExecuteNonQuery(query, TaoThamSo(ldg)) > 0;
        }

        // 4. XÓA 1 LOẠI ĐỘC GIẢ (DELETE)
        public bool XoaLoaiDocGia(string maLDG)
        {
            string query = "DELETE FROM LoaiDocGia WHERE MaLDG = @MaLDG";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaLDG", maLDG)
            };

            return MyConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        // 5. KIỂM TRA XEM MÃ NÀY CÓ ĐANG ĐƯỢC BẢNG ĐỘC GIẢ SỬ DỤNG KHÔNG
        // (Đảm bảo ràng buộc toàn vẹn CSDL, tránh làm CSDL bị lủng)
        public bool KiemTraDangSuDung(string maLDG)
        {
            string query = "SELECT COUNT(*) FROM DocGia WHERE MaLDG = @MaLDG";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaLDG", maLDG)
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
