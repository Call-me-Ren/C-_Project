using System;
using System.IO;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class PhieuPhatBaoCaoBUS
    {
        private readonly PhieuPhatBaoCaoDAO dao = new PhieuPhatBaoCaoDAO();

        public PhieuPhatBaoCaoDTO LayTheoMa(string maPhieuPhat)
        {
            if (string.IsNullOrWhiteSpace(maPhieuPhat))
                throw new BusinessRuleException("Hãy chọn phiếu phạt cần xuất.");

            return dao.LayTheoMa(maPhieuPhat.Trim())
                ?? throw new BusinessRuleException("Không tìm thấy phiếu phạt.");
        }

        public static string TenFileGoiY(string maPhieuPhat)
        {
            string safe = string.Concat((maPhieuPhat ?? string.Empty)
                .Where(character => !Path.GetInvalidFileNameChars().Contains(character)));
            return string.IsNullOrWhiteSpace(safe) ? "PhieuPhat.pdf" : $"PhieuPhat_{safe}.pdf";
        }
    }
}
