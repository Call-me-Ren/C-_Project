using System;
using System.IO;
using System.Linq;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    /// <summary>Lớp BUS lấy và kiểm tra dữ liệu phiếu phạt để lớp GUI chuyển cho bộ xuất PDF.</summary>
    public class PhieuPhatBaoCaoBUS
    {
        private readonly PhieuPhatBaoCaoDAO dao = new PhieuPhatBaoCaoDAO();

        /// <summary>Lấy dữ liệu đầu phiếu và vi phạm theo mã để lập báo cáo PDF.</summary>
        /// <param name="maPhieuPhat">Mã phiếu cần xuất.</param>
        /// <returns>Mô hình báo cáo gồm đầu phiếu và các dòng chi tiết.</returns>
        /// <exception cref="BusinessRuleException">Mã rỗng hoặc không tìm thấy phiếu.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Không đọc được đầu phiếu hoặc các vi phạm.</exception>
        public PhieuPhatBaoCaoDTO LayTheoMa(string maPhieuPhat)
        {
            if (string.IsNullOrWhiteSpace(maPhieuPhat))
                throw new BusinessRuleException("Hãy chọn phiếu phạt cần xuất.");

            return dao.LayTheoMa(maPhieuPhat.Trim())
                ?? throw new BusinessRuleException("Không tìm thấy phiếu phạt.");
        }

        /// <summary>Tạo tên PDF gợi ý từ mã phiếu và loại bỏ ký tự không hợp lệ của tên tệp.</summary>
        /// <param name="maPhieuPhat">Mã phiếu dùng trong tên tệp; giá trị <see langword="null"/> được xem như rỗng.</param>
        /// <returns>Tên dạng PhieuPhat_mã.pdf; trả về PhieuPhat.pdf nếu mã sau lọc rỗng.</returns>
        public static string TenFileGoiY(string maPhieuPhat)
        {
            string safe = string.Concat((maPhieuPhat ?? string.Empty)
                .Where(character => !Path.GetInvalidFileNameChars().Contains(character)));
            return string.IsNullOrWhiteSpace(safe) ? "PhieuPhat.pdf" : $"PhieuPhat_{safe}.pdf";
        }
    }
}
