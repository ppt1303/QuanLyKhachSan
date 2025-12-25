using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    // TÊN CHUẨN: 1 chữ 'n'
    public partial class ucQuanLyDatPhong : UserControl
    {
        BookingBLL _bll = new BookingBLL();

        // Constructor: Phải khớp tên class (1 chữ 'n')
        public ucQuanLyDatPhong()
        {
            InitializeComponent();
            LoadDanhSach(); 
            FormatLuoi();


        }

        private void ucQuanLyDatPhong_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            FormatLuoi();
        }

        private void LoadDanhSach()
        {
            DataTable dt = _bll.GetDanhSachDonDat();
            // Nếu vẫn báo lỗi dòng dưới, hãy xem mục "Lưu ý quan trọng" bên dưới
            dgvDanhSachDon.DataSource = dt;
        }

        private void FormatLuoi()
        {
            if (dgvDanhSachDon.Columns.Count == 0) return;

            // Đặt tên tiếng Việt cho cột
            dgvDanhSachDon.Columns["MaDP"].HeaderText = "Mã Đơn";
            dgvDanhSachDon.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
            dgvDanhSachDon.Columns["NgayDat"].HeaderText = "Ngày Đặt";
            dgvDanhSachDon.Columns["DanhSachPhong"].HeaderText = "Danh Sách Phòng";
            dgvDanhSachDon.Columns["SoLuongPhong"].HeaderText = "SL Phòng";
            dgvDanhSachDon.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dgvDanhSachDon.Columns["GhiChu"].HeaderText = "Ghi Chú";

            dgvDanhSachDon.Columns["MaDP"].Width = 80;
            dgvDanhSachDon.Columns["DanhSachPhong"].Width = 200;
            dgvDanhSachDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnDatPhongMoi_Click(object sender, EventArgs e)
        {
            frmDatPhong frm = new frmDatPhong();
            frm.ShowDialog();
            LoadDanhSach();
        }

        // Hàm này để tránh lỗi nếu bên giao diện lỡ đăng ký sự kiện click
        private void dgvDanhSachDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDatPhongMoi_Click_1(object sender, EventArgs e)
        {
            frmDatPhong frm = new frmDatPhong();

           
            frm.ShowDialog();

            LoadDanhSach();
        }
    }
}