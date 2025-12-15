using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq; 

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
            //else if (e.Element.Name == "aceDatPhong")
            //{
            //    LoadUserControl(typeof(ucDatPhong));
            //}
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
    }
}