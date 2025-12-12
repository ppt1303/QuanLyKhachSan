using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class ThietBiBLL
    {
        private ThietBiDAL _dal = new ThietBiDAL();

        public DataTable LayDSThietBi() => _dal.LayDSThietBi();

        public bool ThemThietBi(string tenTB, string moTa, int soLuong)
        {
            return _dal.ThemThietBi(tenTB, moTa, soLuong);
        }

        public bool SuaThietBi(int maTB, string tenTB, string moTa, int soLuong)
        {
            return _dal.SuaThietBi(maTB, tenTB, moTa, soLuong);
        }

        public bool XoaThietBi(string maTBList) => _dal.XoaThietBi(maTBList);
    }
}