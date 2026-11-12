using System;

namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho 1 dòng dữ liệu trong bảng ChiTietPhieuMuon (Detail)
    /// </summary>
    public class ChiTietPhieuMuonDTO
    {
        // 1. CÁC THUỘC TÍNH ÁNH XẠ CSDL
        public string MaPhieuMuon { get; set; } = string.Empty;
        public string MaSach { get; set; } = string.Empty;
        public string? TinhTrang { get; set; }
        public DateTime? NgayThucTra { get; set; }

        // 2. THUỘC TÍNH MỞ RỘNG HIỂN THỊ
        public string? TenSach { get; set; }
        public string? MaDauSach { get; set; }

        public string TrangThaiTra
        {
            get
            {
                if (NgayThucTra.HasValue)
                {
                    return $"Đã trả ({NgayThucTra.Value:dd/MM/yyyy})";
                }
                return "Đang mượn";
            }
        }

        // 3. CONSTRUCTOR
        public ChiTietPhieuMuonDTO()
        {
        }

        public ChiTietPhieuMuonDTO(string maPhieuMuon, string maSach, string? tinhTrang = null, DateTime? ngayThucTra = null, string? tenSach = null, string? maDauSach = null)
        {
            MaPhieuMuon = maPhieuMuon;
            MaSach = maSach;
            TinhTrang = tinhTrang;
            NgayThucTra = ngayThucTra;
            TenSach = tenSach;
            MaDauSach = maDauSach;
        }

        public override string ToString()
        {
            return $"{MaSach} - {TenSach ?? MaSach}";
        }
    }
}
