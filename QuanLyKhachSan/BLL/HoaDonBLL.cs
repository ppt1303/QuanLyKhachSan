using System;
using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL dal = new HoaDonDAL();

        public DataTable GetInvoiceList(DateTime from, DateTime to)
        {
            return dal.LayDanhSachHoaDon(from, to);
        }
        public DataTable GetInvoiceDetails(int maHD)
        {
            // Gọi xuống DAL để lấy dữ liệu
            return dal.LayChiTietHoaDon(maHD);
        }
    }
}