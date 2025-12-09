using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using QuanLyKhachSan.BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private addPicture _bllAddPicture;
        public frmMain()
        {
            InitializeComponent();
            // Đăng ký sự kiện
            this.Load += FrmMain_Load;
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
            _bllAddPicture = new addPicture();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            _bllAddPicture.ThemHinhNen(fluentDesignFormContainer1);
        }
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
        }

    }
}