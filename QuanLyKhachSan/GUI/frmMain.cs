using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using System;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private ucTrangChu _ucTrangChu;
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
            string text = e.Element.Name;

            switch (text)
            {
                case "aceTrangChu":
                    LoadTrangChu();
                    break;
            }
        }
        private void LoadTrangChu()
        {
            if (_ucTrangChu == null)
            {
                _ucTrangChu = new ucTrangChu();
                _ucTrangChu.Dock = DockStyle.Fill; 
                fluentDesignFormContainer1.Controls.Add(_ucTrangChu);
            }
            _ucTrangChu.LoadData();
            _ucTrangChu.BringToFront();
        }
    }
}