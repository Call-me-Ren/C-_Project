using System;

namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho bảng PhieuNhap trong CSDL
    /// </summary>
    public class PhieuNhapDTO
    {
        public string MaPN { get; set; } = string.Empty;
        public string MaNCC { get; set; } = string.Empty;
        public string MaNV { get; set; } = string.Empty;
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; } = 0;

        public PhieuNhapDTO() { }

        public PhieuNhapDTO(string maPN, string maNCC, string maNV, DateTime ngayNhap, decimal tongTien)
        {
            MaPN = maPN;
            MaNCC = maNCC;
            MaNV = maNV;
            NgayNhap = ngayNhap;
            TongTien = tongTien;
        }
    }
}
