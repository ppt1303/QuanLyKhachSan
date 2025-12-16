using System;
using System.Windows.Forms;
using QuanLyKhachSan.BLL; // Nhớ using BLL

namespace QuanLyKhachSan.GUI
{
    public partial class frmChiTietHoaDon : Form
    {
        private int _maHD;
        private HoaDonBLL _bll = new HoaDonBLL();

        // Constructor nhận Mã Hóa Đơn
        public frmChiTietHoaDon(int maHD)
        {
            InitializeComponent();
            _maHD = maHD;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            this.Text = "Chi tiết hóa đơn #" + _maHD;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Gọi BLL lấy dữ liệu
                dgvChiTiet.DataSource = _bll.GetInvoiceDetails(_maHD);

                // Làm đẹp Grid
                dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvChiTiet.RowHeadersVisible = false;

                if (dgvChiTiet.Columns["SoTien"] != null)
                {
                    dgvChiTiet.Columns["SoTien"].DefaultCellStyle.Format = "N0";
                    dgvChiTiet.Columns["SoTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}