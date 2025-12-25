using System;
using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL dal = new HoaDonDAL();

        // Lấy danh sách tổng quát
        public DataTable GetInvoiceList(DateTime from, DateTime to)
        {
            return dal.LayDanhSachHoaDon(from, to);
        }

        // Lấy chi tiết để in
     
       
        public DataTable GetInvoiceDetails(int maHD)
        {
            // Gọi lại hàm DAL đã có sẵn
            return dal.LayChiTietInHoaDon(maHD);
        }

        public int GetLatestInvoiceID(int maNP)
        {
            return dal.LayMaHDMoiNhat(maNP);
        }
    }
}