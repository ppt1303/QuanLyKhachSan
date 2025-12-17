using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using QuanLyKhachSan.GUI; // Đảm bảo dòng này có

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private addPicture _bllAddPicture;

        public frmMain()
        {
            InitializeComponent();
            _bllAddPicture = new addPicture();
            this.Load += FrmMain_Load;
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            _bllAddPicture.ThemHinhNen(fluentDesignFormContainer1);
        }

        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            if (e.Element.Name == "aceTrangChu")
            {
                LoadUserControl(typeof(ucTrangChu));
            }
            else if (e.Element.Name == "aceDichVu")
            {
                LoadUserControl(typeof(ucDichVu));
            }
            else if (e.Element.Name == "aceThietBi")
            {
                LoadUserControl(typeof(ucThietBi));
            }
            else if (e.Element.Name == "acePhong")
            {
                LoadUserControl(typeof(ucQLPhong));
            }
            // ************ TÙY CHỌN 1: Nếu bạn dùng elementClick cho Khách hàng ************
            // else if (e.Element.Name == "aceKhachHang") 
            // {
            //     LoadUserControl(typeof(ucKhachHang));
            // }
            // ******************************************************************************
        }

        private void LoadUserControl(Type controlType)
        {
            foreach (Control ctrl in fluentDesignFormContainer1.Controls)
            {
                if (ctrl.GetType() == controlType)
                {
                    ctrl.BringToFront();
                    return;
                }
            }
            Control control = (Control)Activator.CreateInstance(controlType);
            control.Dock = DockStyle.Fill;
            fluentDesignFormContainer1.Controls.Add(control);
            control.BringToFront();
        }

        private void aceDatPhong_Click(object sender, EventArgs e)
        {
            fluentDesignFormContainer1.Controls.Clear();

            // 2. Khởi tạo UserControl Quản Lý Đặt Phòng
            // Chú ý: ucQuanLyDatPhong có thể nằm ngoài namespace QuanLyKhachSan.GUI
            // Nếu bạn không có file ucQuanLyDatPhong.cs trong thư mục GUI, hãy kiểm tra lại
            // Tôi giả định nó tồn tại và có thể được truy cập.
            ucQuanLyDatPhong uc = new ucQuanLyDatPhong();

            // 3. Cài đặt để nó bung full kích thước khung chứa
            uc.Dock = DockStyle.Fill;

            // 4. Thêm nó vào khung chứa chính (fluentDesignFormContainer1)
            fluentDesignFormContainer1.Controls.Add(uc);

            // 5. Đưa nó lên lớp trên cùng để hiển thị
            uc.BringToFront();
        }

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }

        private void fluentDesignFormContainer1_Click(object sender, EventArgs e)
        {

        }

        private void aceHoaDon_Click(object sender, EventArgs e)
        {
            // 1. Clear old screens
            fluentDesignFormContainer1.Controls.Clear();

            // 2. Initialize the Invoice UserControl
            // Make sure you have created this UserControl first!
            ucHoaDon uc = new ucHoaDon();

            // 3. Set to fill the container
            uc.Dock = DockStyle.Fill;

            // 4. Add to container
            fluentDesignFormContainer1.Controls.Add(uc);

            // 5. Bring to front
            uc.BringToFront();
        }

        private void aceTrangChu_Click(object sender, EventArgs e)
        {

        }

        private void aceKhachHang_Click(object sender, EventArgs e)
        {
            // ************ LOGIC HIỂN THỊ ucKhachHang TẠI ĐÂY ************

            // 1. Clear old screens
            fluentDesignFormContainer1.Controls.Clear();

            // 2. Khởi tạo UserControl Khách hàng
            ucKhachHang uc = new ucKhachHang();

            // 3. Cài đặt để nó bung full kích thước khung chứa
            uc.Dock = DockStyle.Fill;

            // 4. Thêm nó vào khung chứa chính (fluentDesignFormContainer1)
            fluentDesignFormContainer1.Controls.Add(uc);

            // 5. Đưa nó lên lớp trên cùng để hiển thị
            uc.BringToFront();

            // *************************************************************
        }
    }
}