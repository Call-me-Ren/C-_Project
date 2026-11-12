using System;

namespace QLThuVien.DTO
{
    /// <summary>
    /// DTO đại diện cho 1 dòng dữ liệu trong bảng PhieuMuon (Master)
    /// </summary>
    public class PhieuMuonDTO
    {
        // 1. CÁC THUỘC TÍNH ÁNH XẠ CSDL
        public string MaPM { get; set; } = string.Empty;
        public string MaDocGia { get; set; } = string.Empty;
        public string MaNV { get; set; } = string.Empty;
        public DateTime NgayMuon { get; set; } = DateTime.Now;
        public DateTime HanTra { get; set; } = DateTime.Now.AddDays(7);

        // 2. THUỘC TÍNH MỞ RỘNG HIỂN THỊ TRÊN GIAO DIỆN
        public string? TenDocGia { get; set; }
        public string? TenNV { get; set; }
        public int TongSoSach { get; set; } = 0;
        public int SoSachChuaTra { get; set; } = 0;

        public string TrangThai
        {
            get
            {
                if (TongSoSach == 0) return "Chưa có sách";
                if (SoSachChuaTra == 0) return "Đã trả đủ";
                return $"Chưa trả ({SoSachChuaTra}/{TongSoSach})";
            }
        }

        // 3. CONSTRUCTOR
        public PhieuMuonDTO()
        {
        }

        public PhieuMuonDTO(string maPM, string maDocGia, string maNV, DateTime ngayMuon, DateTime hanTra, string? tenDocGia = null, string? tenNV = null)
        {
            MaPM = maPM;
            MaDocGia = maDocGia;
            MaNV = maNV;
            NgayMuon = ngayMuon;
            HanTra = hanTra;
            TenDocGia = tenDocGia;
            TenNV = tenNV;
        }

        public override string ToString()
        {
            return $"{MaPM} - {TenDocGia ?? MaDocGia}";
        }
    }
}
