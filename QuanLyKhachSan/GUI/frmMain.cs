using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Windows.Forms;
// Đừng quên using namespace chứa các UserControl của bạn
// using QuanLyKhachSan.GUI.UserControls; 

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private addPicture _bllAddPicture;

        public frmMain()
        {
            InitializeComponent();
            _bllAddPicture = new addPicture();

            // Đăng ký sự kiện (thường Designer đã tự làm, nhưng viết ở đây cho chắc chắn)
            this.Load += FrmMain_Load;
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Load hình nền hoặc thiết lập ban đầu
            _bllAddPicture.ThemHinhNen(fluentDesignFormContainer1);

            // Mở mặc định Trang chủ khi vừa chạy phần mềm (Tùy chọn)
            // LoadUserControl(new ucTrangChu());
        }

        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            // e.Element là mục vừa được click vào
            if (e.Element == aceTrangChu) // So sánh với tên biến (Name) của mục Trang chủ
            {
                LoadUserControl(new ucTrangChu());
            }
            else if (e.Element.Text == "Quản lý Đặt phòng") // Ví dụ xử lý mục tiếp theo
            {
                // LoadUserControl(new ucDatPhong());
            }
            else if (e.Element.Text == "Quản lý Khách hàng")
            {
                // LoadUserControl(new ucKhachHang());
            }
            // ... Tương tự cho các mục khác trong hình
        }

        // Hàm hỗ trợ hiển thị UserControl lên container
        private void LoadUserControl(Control control)
        {
            // Kiểm tra nếu control này đang hiển thị rồi thì không load lại
            foreach (Control ctrl in fluentDesignFormContainer1.Controls)
            {
                if (ctrl.GetType() == control.GetType())
                {
                    ctrl.BringToFront();
                    return;
                }
            }

            // Xóa các control cũ (nếu muốn tiết kiệm ram) hoặc chỉ cần BringToFront
            fluentDesignFormContainer1.Controls.Clear();

            // Thiết lập để control mới tràn màn hình
            control.Dock = DockStyle.Fill;
            fluentDesignFormContainer1.Controls.Add(control);
            control.BringToFront();
        }
    }
}