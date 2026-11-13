using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>
    /// BUS quản lý cấu hình các tham số hệ thống từ bảng ThamSo (bảng thứ 17).
    /// </summary>
    public class ThamSoBUS
    {
        private readonly ThamSoDAO tsDAO = new ThamSoDAO();
        public ArrayList dsThamSo_arr = new ArrayList();

        public static string LayGiaTriTheoMa(string maTS, string macDinh = "")
        {
            return ThamSoDAO.LayGiaTriTheoMa(maTS, macDinh);
        }

        public static int LayGiaTriInt(string maTS, int macDinh)
        {
            string raw = LayGiaTriTheoMa(maTS, macDinh.ToString(CultureInfo.InvariantCulture));
            return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val) ? val : macDinh;
        }

        public static decimal LayGiaTriDecimal(string maTS, decimal macDinh)
        {
            string raw = LayGiaTriTheoMa(maTS, macDinh.ToString(CultureInfo.InvariantCulture));
            return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val) ? val : macDinh;
        }

        public ArrayList DocDSThamSo()
        {
            List<ThamSoDTO> list = tsDAO.DocDSThamSo();
            dsThamSo_arr.Clear();
            foreach (ThamSoDTO ts in list)
            {
                dsThamSo_arr.Add(ts);
            }
            return dsThamSo_arr;
        }

        public bool CapNhatGiaTri(string maTS, string giaTri, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(maTS))
            {
                error = "Mã tham số không được rỗng!";
                return false;
            }

            bool ok = tsDAO.CapNhatGiaTri(maTS, giaTri.Trim());
            if (ok)
            {
                DocDSThamSo();
            }
            else
            {
                error = "Cập nhật giá trị tham số thất bại!";
            }
            return ok;
        }
    }
}
