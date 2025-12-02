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

        // 1. Hàm tải dữ liệu lên lưới
        void LoadData()
        {
            try
            {
                // Câu SQL Join nhiều bảng để lấy thông tin chi tiết
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
                    ORDER BY DP.NgayDat DESC"; // Đơn mới nhất lên đầu

                DataTable dt = DatabaseHelper.GetDataTable(sql);
                gcDanhSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // 2. Nút Thêm Mới
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Mở form frmDatPhong ở chế độ thêm mới (mặc định)
            //frmDatPhong frm = new frmDatPhong();
            //frm.ShowDialog();
            LoadData(); // Load lại danh sách sau khi thêm xong
        }

        // 3. Nút Sửa (Xem chi tiết)
        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvDanhSach.FocusedRowHandle < 0) return;

            // Lấy MaDP của dòng đang chọn
            int maDP = Convert.ToInt32(gvDanhSach.GetFocusedRowCellValue("MaDP"));

            // Mở form frmDatPhong và truyền ID vào để nó tự load dữ liệu cũ lên
            //frmDatPhong frm = new frmDatPhong(maDP);
            //frm.ShowDialog();
            LoadData();
        }

        // 4. Nút Hủy Đơn (Thay vì xóa, ta cập nhật trạng thái)
        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvDanhSach.FocusedRowHandle < 0)
            {
                XtraMessageBox.Show("Vui lòng chọn đơn cần hủy!");
                return;
            }

            int maDP = Convert.ToInt32(gvDanhSach.GetFocusedRowCellValue("MaDP"));
            string trangThai = gvDanhSach.GetFocusedRowCellValue("TrangThai").ToString();

            if (trangThai == "Đã hủy")
            {
                XtraMessageBox.Show("Đơn này đã hủy rồi!");
                return;
            }

            if (XtraMessageBox.Show("Bạn có chắc chắn muốn hủy đơn đặt phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string sql = $"UPDATE DATPHONG SET TrangThai = N'Đã hủy' WHERE MaDP = {maDP}";
                DatabaseHelper.ExecuteNonQuery(sql);
                LoadData();
            }
        }

        // 5. Nút Làm Mới
        private void btnLamMoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }
    }
}