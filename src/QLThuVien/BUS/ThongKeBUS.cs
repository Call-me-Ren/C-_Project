using System.Collections;
using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// Nghiệp vụ thống kê tiền nhập theo đầu sách và quý.
    /// </summary>
    public static class ThongKeBUS
    {
        public static ArrayList dsThongKeNhap_arr = new ArrayList();

        public static List<ThongKeQuyDTO> ThongKeNhapTheoQuy(int nam)
        {
            dsThongKeNhap_arr.Clear();
            List<ThongKeQuyDTO> ketQua = ThongKeDAO.ThongKeNhapTheoQuy(nam);

            foreach (ThongKeQuyDTO dong in ketQua)
                dsThongKeNhap_arr.Add(dong);

            return ketQua;
        }
    }
}
