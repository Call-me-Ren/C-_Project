using System;

namespace QLThuVien.DTO
{
    public class PhieuPhatDTO
    {
        public string MaPhieuPhat { get; set; } = string.Empty;
        public string MaDocGia { get; set; } = string.Empty;
        public string? MaPhieuMuon { get; set; }
        public string MaNV { get; set; } = string.Empty;
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public string TrangThai { get; set; } = "Chưa thanh toán";
        public decimal TongTienPhat { get; set; } = 0;

        public PhieuPhatDTO()
        {
        }

        public PhieuPhatDTO(string maPhieuPhat, string maDocGia, string? maPhieuMuon, string maNV, DateTime ngayLap, string trangThai, decimal tongTienPhat)
        {
            MaPhieuPhat = maPhieuPhat;
            MaDocGia = maDocGia;
            MaPhieuMuon = maPhieuMuon;
            MaNV = maNV;
            NgayLap = ngayLap;
            TrangThai = trangThai;
            TongTienPhat = tongTienPhat;
        }
    }
}
