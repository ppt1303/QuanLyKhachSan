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

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }

        private void accordionControlElement4_Click(object sender, EventArgs e)
        {
            frmQLDichVu f = new frmQLDichVu();
            f.ShowDialog();
        }
    }
}