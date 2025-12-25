using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class DichVuBLL
    {
        private DichVuDAL _dal;

        public DichVuBLL()
        {
            _dal = new DichVuDAL();
        }

        public DataTable LayDSDichVu()
        {
            return _dal.LayDSDichVu();
        }

        public bool ThemDichVu(string tenDV, decimal gia)
        {
          
            return _dal.ThemDichVu(tenDV, gia);
        }

        public bool SuaDichVu(int maDV, string tenDV, decimal gia)
        {
           
            return _dal.SuaDichVu(maDV, tenDV, gia);
        }

        public bool XoaDichVu(string maDVList)
        {
            return _dal.XoaDichVu(maDVList);
        }
    }
}