using System;
using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class KhachHangBLL
    {
        KhachHangDAL khachHangDAL = new KhachHangDAL();

        public DataTable LayDanhSachKhachHang()
        {
            return khachHangDAL.LayDanhSachKhachHang();
        }

    
        public bool ThemKhachHang(string hoTen, string cccd, string sdt, string gioiTinh, DateTime ngaySinh, string quocTich)
        {
            return khachHangDAL.ThemKhachHang(hoTen, cccd, sdt, gioiTinh, ngaySinh, quocTich);
        }

        public DataTable TimKiemKhachHang(string keyword)
        {
            return khachHangDAL.TimKiemKhachHang(keyword);
        }

       
        public bool CapNhatKhachHang(int maKH, string hoTen, string sdt, string cccd, string gioiTinh, DateTime ngaySinh, string quocTich)
        {
            return khachHangDAL.CapNhatKhachHang(maKH, hoTen, sdt, cccd, gioiTinh, ngaySinh, quocTich);
        }

        public bool XoaKhachHang(int maKH)
        {
            return khachHangDAL.XoaKhachHang(maKH);
        }
    }
}