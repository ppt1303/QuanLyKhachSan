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

        // Xử lý Menu bên trái
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            if (e.Element.Style == ElementStyle.Item)
            {
                switch (e.Element.Name)
                {
                    case "aceSoDoPhong": // Nếu bấm menu Sơ đồ
                        if (!(fluentDesignFormContainer1.Controls.Count > 0 && fluentDesignFormContainer1.Controls[0] is ucSoDoPhong))
                        {
                            LoadUserControl(new ucSoDoPhong());
                        }
                        break;

                    case "aceBooking": // Nếu bấm menu Quản lý Đặt phòng
                        LoadUserControl(new ucDanhSachDatPhong());
                        break;

                    case "aceLogout":
                        Application.Exit();
                        break;
                }
            }
        }

        // Hàm thay thế màn hình ở giữa (Container)
        private void LoadUserControl(UserControl uc)
        {
            try
            {
                // Xóa màn hình cũ
                fluentDesignFormContainer1.Controls.Clear();

                // Thêm màn hình mới
                uc.Dock = DockStyle.Fill;
                fluentDesignFormContainer1.Controls.Add(uc);
                uc.BringToFront();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải màn hình: " + ex.Message);
            }
        }

        private void aceRoomMap_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucSoDoPhong());
        }

        private void aceBooking_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucDanhSachDatPhong());
        }

        private void aceCustomer_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucKhachHang());
        }
    }
}