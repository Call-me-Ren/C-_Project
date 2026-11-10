#nullable disable
using System;

namespace DTO
{
    public class TacGiaDTO
    {
        public string MaTG { get; set; }
        public string HoLot { get; set; }
        public string TenTG { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string SDT { get; set; }

        public TacGiaDTO() { }

        public TacGiaDTO(string maTG, string hoLot, string tenTG, DateTime? ngaySinh, string sdt)
        {
            MaTG = maTG;
            HoLot = hoLot;
            TenTG = tenTG;
            NgaySinh = ngaySinh;
            SDT = sdt;
        }
    }
}