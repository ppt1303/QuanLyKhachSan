using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace QuanLyKhachSan.GUI
{
    public partial class ucDanhSachDatPhong : XtraUserControl
    {
        public ucDanhSachDatPhong()
        {
            InitializeComponent();
            this.Load += UcDanhSachDatPhong_Load;
        }

        private void UcDanhSachDatPhong_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        void LoadData()
        {
            try
            {
                string sql = @"
                    SELECT 
                        DP.MaDP,
                        DP.NgayDat,
                        DP.TrangThai,
                        DP.TienCoc,
                        KH.HoTen AS [KhachHang],
                        KH.SDT,
                        P.TenPhong AS [Phong],
                        CD.NgayNhanPhong,
                        CD.NgayTraPhong
                    FROM DATPHONG DP
                    JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
                    JOIN CHITIET_DATPHONG CD ON DP.MaDP = CD.MaDP
                    JOIN PHONG P ON CD.MaPhong = P.MaPhong
                    ORDER BY DP.NgayDat DESC"; 

                DataTable dt = DatabaseHelper.GetDataTable(sql);
                gcDanhSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
    }
}