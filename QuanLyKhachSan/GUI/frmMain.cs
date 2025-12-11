using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq; // Cần thêm thư viện này để dùng FirstOrDefault

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private addPicture _bllAddPicture;

        public frmMain()
        {
            InitializeComponent();
            _bllAddPicture = new addPicture();

            // Đăng ký các sự kiện
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
            // Sau này bạn có thể thêm các else if cho các nút khác
            // else if (e.Element.Name == "aceQuanLyPhong") 
            // {
            //     LoadUserControl(typeof(ucQuanLyPhong));
            // }
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
        private void frmMain_Load_1(object sender, EventArgs e) { }
        private void accordionControlElement1_Click(object sender, EventArgs e) { }

        private void aceDatPhong_Click(object sender, EventArgs e)
        {

        }
    }
}