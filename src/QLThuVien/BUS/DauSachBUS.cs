using System.Collections.Generic;
using QLThuVien.DAO;
using QLThuVien.DTO;

namespace QLThuVien.BUS
{
    public class DauSachBUS
    {
        public static List<DauSachDTO> GetAll()
        {
            return DauSachDAO.GetAll();
        }
    }
}
