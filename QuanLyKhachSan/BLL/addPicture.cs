using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors; 

namespace QuanLyKhachSan.BLL
{
    public class addPicture
    {
       private string _duongDanAnh = @"D:\kali-linux-2025.2-virtualbox-amd64\Hocki1nam3\CSDL\thuchanhthuctap\baitaplon\winform4\QuanLyKhachSan\QuanLyKhachSan\Picture\khachsan.jpg";
        public void ThemHinhNen(Control container)
        {  
            if (!File.Exists(_duongDanAnh))
            {
                XtraMessageBox.Show("Không tìm thấy file ảnh tại đường dẫn đã cấu hình!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PictureBox bg = new PictureBox();
                bg.Image = Image.FromFile(_duongDanAnh);
                bg.Dock = DockStyle.Fill;
                bg.SizeMode = PictureBoxSizeMode.StretchImage;
                container.Controls.Clear(); 
                container.Controls.Add(bg);
                bg.BringToFront();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
            }
        }
    }
}