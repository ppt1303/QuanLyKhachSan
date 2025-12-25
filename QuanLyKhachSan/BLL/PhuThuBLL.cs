using System.Data;
using System;
using QuanLyKhachSan.DAL; 

namespace QuanLyKhachSan.BLL
{
    public class PhuThuBLL
    {
        private PhuThuDAL dal = new PhuThuDAL();

        public DataTable LayDanhSachPhuThu()
        {
            return dal.GetListPhuThu();
        }

        public bool ThemPhuThu(int maNP, int maPhuThu, int soLuong, string ghiChu)
        {
            return dal.InsertPhuThu(maNP, maPhuThu, soLuong, ghiChu);
        }

        public DateTime GetExpectedCheckout(int maNP)
        {
            return dal.LayNgayTraDuKien(maNP);
        }
        public DataTable LayPhuThuTheoPhong(int maNP)
        {
            return dal.LayPhuThuTheoPhong(maNP);
        }
        public bool XoaPhuThu(int maSDPT)
        {
            return dal.XoaPhuThu(maSDPT);
        }

        public DataTable GetListSurcharge(int maNP)
        {
            return dal.LayPhuThuTheoPhong(maNP);
        }
    }
}