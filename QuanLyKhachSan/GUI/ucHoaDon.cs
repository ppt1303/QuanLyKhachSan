using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using QuanLyKhachSan;

namespace QuanLyKhachSan.GUI
{
    public partial class ucHoaDon : UserControl
    {
        private HoaDonBLL _bll = new HoaDonBLL();

        public ucHoaDon()
        {
            InitializeComponent();
        }

        // Sự kiện khi UserControl vừa hiện lên
        private void ucHoaDon_Load(object sender, EventArgs e)
        {
            CaiDatMacDinh();
            LoadData();
        }

        private void CaiDatMacDinh()
        {
            // Mặc định chọn từ đầu tháng đến hiện tại
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;

            // Format hiển thị ngày tháng cho DateTimePicker (dd/MM/yyyy)
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
        }

        private void LoadData()
        {
            try
            {
                // Gọi BLL lấy dữ liệu
                DataTable dt = _bll.GetInvoiceList(dtpTuNgay.Value, dtpDenNgay.Value);

                // Đổ vào GridView
                dgvHoaDon.DataSource = dt;

                // Làm đẹp GridView
                FormatGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }

        private void FormatGrid()
        {
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.RowHeadersVisible = false; // Ẩn cột đầu xấu xí

            // Căn phải cột tiền (nếu tên cột đúng là 'Tổng Tiền')
            if (dgvHoaDon.Columns["Tổng Tiền"] != null)
            {
                dgvHoaDon.Columns["Tổng Tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        // Sự kiện nút Tìm Kiếm

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            
            // 1. Kiểm tra xem người dùng có chọn dòng nào chưa
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells["Mã HĐ"].Value); 

            // 3. Mở Form chi tiết lên
            frmChiTietHoaDon frm = new frmChiTietHoaDon(maHD);
            frm.ShowDialog(); 
        
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {

        }

        private void btnInHoaDon_Click_1(object sender, EventArgs e)
        {

            if (dgvHoaDon.SelectedRows.Count == 0) return;

            try
            {
                int maHD = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells["Mã HĐ"].Value);

        
                DataTable dt = _bll.GetInvoiceDetails(maHD);

         
                string tenCot = "";
                foreach (DataColumn col in dt.Columns)
                {
                    tenCot += col.ColumnName + " | ";
                }
                
         

                if (dt.Rows.Count > 0)
                {
                    QuanLyKhachSan.rptHoaDon rpt = new QuanLyKhachSan.rptHoaDon();
                    rpt.DataSource = dt;

              
                    rpt.DataMember = "";

                    rpt.ShowPreviewDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
