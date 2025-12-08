using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.GUI
{
    public partial class frmMain : FluentDesignForm
    {
        private PictureBox bg;
        public frmMain()
        {
            InitializeComponent();
            // Đăng ký sự kiện
            this.Load += FrmMain_Load;
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            bg = new PictureBox();
            bg.Image = Image.FromFile(@"C:\Users\admin\OneDrive\Documents\codecsharp\QuanLyKhachSan\QuanLyKhachSan\Picture\khachsan.jpg"); 
            bg.Dock = DockStyle.Fill;
            bg.SizeMode = PictureBoxSizeMode.StretchImage;
            fluentDesignFormContainer1.Controls.Add(bg);
            bg.BringToFront();
        }
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
        }

    }
}