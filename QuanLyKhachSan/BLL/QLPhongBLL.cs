using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class QLPhongBLL
    {
        private QLPhongDAL dal = new QLPhongDAL();

        public DataTable LayTatCaPhong()
        {
            return dal.LayTatCaPhong();
        }

        public DataTable LayDSLoaiPhong()
        {
            return dal.LayDSLoaiPhong();
        }

        public bool ThemPhong(string tenPhong, int maLP, int tang, string huong)
        {
            return dal.ThemPhong(tenPhong, maLP, tang, huong);
        }

        public bool SuaPhong(int maPhong, string tenPhong, int maLP, int tang, string huong)
        {
            return dal.SuaPhong(maPhong, tenPhong, maLP, tang, huong);
        }

        public bool XoaPhong(int maPhong)
        {
            return dal.XoaPhong(maPhong);
        }
    }
}