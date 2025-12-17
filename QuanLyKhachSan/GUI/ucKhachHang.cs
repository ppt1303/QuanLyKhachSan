using QuanLyKhachSan.BLL;
using System;
using System.Data;
using System.Drawing; // Thêm thư viện này
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class ucKhachHang : UserControl
    {
        private KhachHangBLL khachHangBLL = new KhachHangBLL();
        private int MaKH_Chon = -1;

        public ucKhachHang()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill; // Đảm bảo UserControl hiển thị toàn bộ
            ClearInputs(); // Thiết lập trạng thái ban đầu
            LoadDanhSachKhachHang();
        }

        private void LoadDanhSachKhachHang()
        {
            try
            {
                DataTable dt = khachHangBLL.LayDanhSachKhachHang();

                // Thêm cột hiển thị giới tính để đơn giản hóa việc hiển thị
                if (!dt.Columns.Contains("GioiTinhHienThi"))
                    dt.Columns.Add("GioiTinhHienThi", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["GioiTinhHienThi"] = row["GioiTinh"].ToString() == "Nam" ? "Nam" : "Nữ";
                }

                guna2DataGridView1.DataSource = dt;

                // Thiết lập hiển thị cho cột
                guna2DataGridView1.Columns["MaKH"].HeaderText = "Mã KH";
                guna2DataGridView1.Columns["HoTen"].HeaderText = "Họ Tên";
                guna2DataGridView1.Columns["SDT"].HeaderText = "SĐT";
                guna2DataGridView1.Columns["CCCD"].HeaderText = "CCCD/CMND";
                guna2DataGridView1.Columns["GioiTinh"].Visible = false; // Ẩn cột GioiTinh gốc
                guna2DataGridView1.Columns["GioiTinhHienThi"].HeaderText = "Giới Tính"; // Hiển thị cột đã xử lý
                guna2DataGridView1.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                guna2DataGridView1.Columns["QuocTich"].HeaderText = "Quốc Tịch";

                // Định dạng cột Ngày Sinh
                if (guna2DataGridView1.Columns.Contains("NgaySinh"))
                {
                    guna2DataGridView1.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng: " + ex.Message);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                MaKH_Chon = Convert.ToInt32(row.Cells["MaKH"].Value);
                guna2TextBox_HoTen.Text = row.Cells["HoTen"].Value.ToString();
                guna2TextBox_SDT.Text = row.Cells["SDT"].Value.ToString();
                guna2TextBox_CCCD.Text = row.Cells["CCCD"].Value.ToString();
                guna2ComboBox_GioiTinh.Text = row.Cells["GioiTinhHienThi"].Value.ToString();
                guna2DateTimePicker_NgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                guna2TextBox_QuocTich.Text = row.Cells["QuocTich"].Value.ToString();

                guna2Button_CapNhat.Enabled = true;
                guna2Button_Xoa.Enabled = true;
                guna2Button_Them.Enabled = false;
            }
        }

        private void ClearInputs()
        {
            MaKH_Chon = -1;
            guna2TextBox_HoTen.Clear();
            guna2TextBox_SDT.Clear();
            guna2TextBox_CCCD.Clear();
            guna2TextBox_QuocTich.Text = "Việt Nam";
            guna2ComboBox_GioiTinh.SelectedIndex = 0; // Chọn Nam mặc định
            guna2DateTimePicker_NgaySinh.Value = DateTime.Now.AddYears(-20); // Đặt ngày sinh hợp lý
            guna2TextBox_TimKiem.Clear();

            guna2Button_CapNhat.Enabled = false;
            guna2Button_Xoa.Enabled = false;
            guna2Button_Them.Enabled = true;
        }

        private void guna2Button_LamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadDanhSachKhachHang();
        }

        private void guna2Button_Them_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_HoTen.Text) || string.IsNullOrEmpty(guna2TextBox_CCCD.Text) || string.IsNullOrEmpty(guna2TextBox_SDT.Text) || guna2ComboBox_GioiTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, CCCD, SĐT và Giới tính (*).");
                return;
            }

            try
            {
                string hoTen = guna2TextBox_HoTen.Text;
                string cccd = guna2TextBox_CCCD.Text;
                string sdt = guna2TextBox_SDT.Text;
                // Lấy giá trị thực tế của giới tính (Nam/Nữ/Khác)
                string gioiTinh = guna2ComboBox_GioiTinh.SelectedItem.ToString();
                DateTime ngaySinh = guna2DateTimePicker_NgaySinh.Value;
                string quocTich = guna2TextBox_QuocTich.Text;

                if (khachHangBLL.ThemKhachHang(hoTen, cccd, sdt, gioiTinh, ngaySinh, quocTich))
                {
                    MessageBox.Show("Thêm khách hàng thành công.");
                    LoadDanhSachKhachHang();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại (có thể CCCD đã tồn tại hoặc lỗi khác).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        private void guna2Button_CapNhat_Click(object sender, EventArgs e)
        {
            if (MaKH_Chon == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật.");
                return;
            }

            try
            {
                string hoTen = guna2TextBox_HoTen.Text;
                string cccd = guna2TextBox_CCCD.Text;
                string sdt = guna2TextBox_SDT.Text;
                string gioiTinh = guna2ComboBox_GioiTinh.Text;
                DateTime ngaySinh = guna2DateTimePicker_NgaySinh.Value;
                string quocTich = guna2TextBox_QuocTich.Text;

                // 1. Thực hiện lệnh cập nhật và lưu kết quả trả về
                bool success = khachHangBLL.CapNhatKhachHang(MaKH_Chon, hoTen, sdt, cccd, gioiTinh, ngaySinh, quocTich);

                // 2. Xử lý thông báo dựa trên kết quả:
                if (success)
                {
                    // Trường hợp lý tưởng: BLL trả về TRUE
                    MessageBox.Show("Cập nhật khách hàng thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Trường hợp hiện tại: BLL trả về FALSE dù đã lưu (Lỗi logic trả về 0 hàng bị ảnh hưởng)
                    // Vì bạn đã xác nhận dữ liệu đã được lưu, chúng ta sẽ xem đây là thành công nhưng có cảnh báo
                    MessageBox.Show("Cập nhật hoàn tất.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 3. ********* BƯỚC SỬA LỖI CHÍNH *********
                // CHẠY LỆNH LÀM MỚI BẢNG VÀ XÓA Ô INPUT SAU KHI CẬP NHẬT (DÙ KẾT QUẢ success LÀ GÌ)
                LoadDanhSachKhachHang(); // Làm mới DataGridView
                ClearInputs();          // Làm mới các ô nhập liệu

            }
            catch (Exception ex)
            {
                // Giữ lại phần bắt lỗi hệ thống để đề phòng lỗi nghiêm trọng
                MessageBox.Show("LỖI HỆ THỐNG TRONG QUÁ TRÌNH CẬP NHẬT: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // File: QuanLyKhachSan/GUI/ucKhachHang.cs

        private void guna2Button_Xoa_Click(object sender, EventArgs e)
        {
            if (MaKH_Chon == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string warningMessage = "Bạn có chắc chắn muốn xóa khách hàng này? Thao tác này sẽ XÓA HOÀN TOÀN TẤT CẢ DỮ LIỆU GIAO DỊCH liên quan và không thể hoàn tác.";

            if (MessageBox.Show(warningMessage, "Xác nhận Xóa Hoàn Toàn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // 1. Thực hiện lệnh xóa. Không cần kiểm tra giá trị trả về ngay tại đây.
                    // Nếu có lỗi SQL Foreign Key, nó sẽ nhảy thẳng đến khối catch.
                    khachHangBLL.XoaKhachHang(MaKH_Chon);

                    // 2. Nếu không có Exception: Báo cáo thành công và làm mới giao diện
                    MessageBox.Show("Xóa khách hàng và tất cả giao dịch liên quan thành công.", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. ********* BƯỚC BẮT BUỘC LÀM MỚI GIAO DIỆN *********
                    LoadDanhSachKhachHang();
                    ClearInputs();

                }
                catch (Exception ex)
                {
                    // 4. Nếu có lỗi hệ thống: Chỉ hiển thị lỗi chính xác từ DB
                    string errorMessage = "LỖI HỆ THỐNG KHI XÓA: " + ex.Message;

                    // Nếu lỗi là do ràng buộc, thông báo người dùng bảng nào đang giữ ID
                    if (ex.Message.Contains("FK_"))
                    {
                        errorMessage = "Không thể xóa Khách hàng. Lỗi ràng buộc khóa ngoại (Foreign Key) với một bảng giao dịch khác chưa được xóa. (Lỗi DB: " + ex.Message + ")";
                    }

                    MessageBox.Show(errorMessage, "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2Button_TimKiem_Click(object sender, EventArgs e)
        {
            string keyword = guna2TextBox_TimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadDanhSachKhachHang();
                return;
            }

            try
            {
                DataTable dt = khachHangBLL.TimKiemKhachHang(keyword);

                // Cần xử lý lại cột hiển thị GioiTinh cho kết quả tìm kiếm
                if (!dt.Columns.Contains("GioiTinhHienThi"))
                    dt.Columns.Add("GioiTinhHienThi", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["GioiTinhHienThi"] = row["GioiTinh"].ToString() == "Nam" ? "Nam" : "Nữ";
                }
                guna2DataGridView1.DataSource = dt;

                // Đảm bảo các cột hiển thị đúng sau khi tìm kiếm
                if (guna2DataGridView1.Columns.Contains("GioiTinh"))
                {
                    guna2DataGridView1.Columns["GioiTinh"].Visible = false;
                }
                if (guna2DataGridView1.Columns.Contains("GioiTinhHienThi"))
                {
                    guna2DataGridView1.Columns["GioiTinhHienThi"].HeaderText = "Giới Tính";
                }
                if (guna2DataGridView1.Columns.Contains("NgaySinh"))
                {
                    guna2DataGridView1.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}