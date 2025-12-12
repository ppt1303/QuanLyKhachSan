using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    public partial class frmQLThietBi : Form
    {
        private ThietBiBLL thietBiBLL = new ThietBiBLL();
        private PhongBLL phongBLL = new PhongBLL();
        private PhongThietBiBLL ptbBLL = new PhongThietBiBLL();

        // Cờ chặn sự kiện (QUAN TRỌNG NHẤT)
        private bool isBindingData = false;

        private int selectedMaTB_Tab1 = -1;
        private int currentMaPhong_Tab2 = -1;
        private int selectedMaTB_Tab2 = -1;

        public frmQLThietBi()
        {
            InitializeComponent();
            SetupTab1Events();
            SetupTab2Events();
            LoadKhoThietBi();
        }

        // =========================================================================
        // TAB 1: QUẢN LÝ KHO THIẾT BỊ
        // =========================================================================
        private void SetupTab1Events()
        {
            dgvThietBi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThietBi.MultiSelect = false;
            dgvThietBi.ReadOnly = true;
            dgvThietBi.SelectionChanged += dgvThietBi_SelectionChanged;

            btnThemTB.Click -= btnThemTB_Click; btnThemTB.Click += btnThemTB_Click;
            btnSuaTB.Click -= btnSuaTB_Click; btnSuaTB.Click += btnSuaTB_Click;
            btnXoaTB.Click -= btnXoaTB_Click; btnXoaTB.Click += btnXoaTB_Click;

            panel1.Click += (s, e) => ResetTab1(true);
            panel2.Click += (s, e) => ResetTab1(true);

            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void LoadKhoThietBi()
        {
            try
            {
                isBindingData = true; // Chặn sự kiện
                dgvThietBi.DataSource = thietBiBLL.LayDSThietBi();

                if (dgvThietBi.Columns.Contains("MaTB")) dgvThietBi.Columns["MaTB"].HeaderText = "Mã TB";
                if (dgvThietBi.Columns.Contains("TenTB")) dgvThietBi.Columns["TenTB"].HeaderText = "Tên Thiết Bị";
                if (dgvThietBi.Columns.Contains("SoLuong"))
                {
                    dgvThietBi.Columns["SoLuong"].HeaderText = "Tổng Kho";
                    dgvThietBi.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThietBi.Columns.Contains("MoTa")) dgvThietBi.Columns["MoTa"].HeaderText = "Mô Tả";

                dgvThietBi.ClearSelection(); // Bỏ chọn dòng đầu
                ResetTab1(false);
            }
            catch { }
            finally { isBindingData = false; }
        }

        private void dgvThietBi_SelectionChanged(object sender, EventArgs e)
        {
            if (isBindingData) return;

            if (dgvThietBi.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dgvThietBi.SelectedRows[0];
                if (row.Cells["MaTB"].Value != null)
                {
                    selectedMaTB_Tab1 = Convert.ToInt32(row.Cells["MaTB"].Value);
                    txtTenTB.Text = row.Cells["TenTB"].Value.ToString();
                    txtMoTaTB.Text = row.Cells["MoTa"].Value.ToString();

                    if (dgvThietBi.Columns.Contains("SoLuong") && row.Cells["SoLuong"].Value != null && Controls.ContainsKey("txtSoLuongKho"))
                        Controls["txtSoLuongKho"].Text = row.Cells["SoLuong"].Value.ToString();

                    btnThemTB.Enabled = false;
                    btnSuaTB.Enabled = true;
                    btnXoaTB.Enabled = true;
                    return;
                }
            }
            ResetTab1(false);
        }

        private void ResetTab1(bool clearGrid)
        {
            selectedMaTB_Tab1 = -1;
            txtTenTB.Clear();
            txtMoTaTB.Clear();
            if (Controls.ContainsKey("txtSoLuongKho")) Controls["txtSoLuongKho"].Text = "";

            btnThemTB.Enabled = true;
            btnSuaTB.Enabled = false;
            btnXoaTB.Enabled = false;

            if (clearGrid)
            {
                isBindingData = true;
                dgvThietBi.ClearSelection();
                isBindingData = false;
            }
        }

        private void btnThemTB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTB.Text)) return;
            int slKho = 0;
            if (Controls.ContainsKey("txtSoLuongKho")) int.TryParse(Controls["txtSoLuongKho"].Text, out slKho);

            if (thietBiBLL.ThemThietBi(txtTenTB.Text, txtMoTaTB.Text, slKho))
            {
                MessageBox.Show("Đã nhập kho thành công!");
                LoadKhoThietBi();
            }
        }

        private void btnSuaTB_Click(object sender, EventArgs e)
        {
            if (selectedMaTB_Tab1 > 0)
            {
                int slKho = 0;
                if (Controls.ContainsKey("txtSoLuongKho")) int.TryParse(Controls["txtSoLuongKho"].Text, out slKho);

                if (thietBiBLL.SuaThietBi(selectedMaTB_Tab1, txtTenTB.Text, txtMoTaTB.Text, slKho))
                {
                    MessageBox.Show("Cập nhật kho thành công!");
                    LoadKhoThietBi();
                }
            }
        }

        private void btnXoaTB_Click(object sender, EventArgs e)
        {
            if (selectedMaTB_Tab1 > 0 && MessageBox.Show("Xóa thiết bị này khỏi kho?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (thietBiBLL.XoaThietBi(selectedMaTB_Tab1.ToString()))
                {
                    MessageBox.Show("Đã xóa!");
                    LoadKhoThietBi();
                }
                else MessageBox.Show("Không thể xóa (đang được sử dụng).");
            }
        }


        // =========================================================================
        // TAB 2: QUẢN LÝ THIẾT BỊ PHÒNG
        // =========================================================================

        private void SetupTab2Events()
        {
            dgvThietBiPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThietBiPhong.MultiSelect = true;
            dgvThietBiPhong.ReadOnly = true;

            //btnSuaTBP.Visible = false;
            btnThemTBP.Text = "Cập nhật";

            // Gán sự kiện lọc
            cboTenPhong.SelectedIndexChanged += (s, e) => { if (!isBindingData) FilterData(); };
            cboTenTBP.SelectedIndexChanged += (s, e) => { if (!isBindingData) FilterData(); };

            // Gán sự kiện Grid
            dgvThietBiPhong.SelectionChanged += dgvThietBiPhong_SelectionChanged;

            // Gán sự kiện Nút
            btnThemTBP.Click -= BtnCapNhatTBP_Click; btnThemTBP.Click += BtnCapNhatTBP_Click;
            btnXoaTBP.Click -= BtnXoaTBP_Click; btnXoaTBP.Click += BtnXoaTBP_Click;

            // Gán sự kiện Nút Reset
            Control[] btns = this.Controls.Find("btnReset", true);
            if (btns.Length > 0)
            {
                btns[0].Click -= btnReset_Click;
                btns[0].Click += btnReset_Click;
            }

            panel3.Click += (s, e) => ResetTab2(true);
            panel4.Click += (s, e) => ResetTab2(true);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                // BẮT ĐẦU CHẶN SỰ KIỆN TOÀN CỤC
                isBindingData = true;

                LoadComboboxTab2_Empty();
                FilterData_Internal(-1, -1);
                ResetTab2(true);

                // MỞ KHÓA SỰ KIỆN
                isBindingData = false;
            }
            else if (tabControl1.SelectedTab.Name == "tabPage1")
            {
                LoadKhoThietBi();
            }
        }

        // --- HÀM NÚT RESET ---
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetTab2(true);
        }

        private void LoadComboboxTab2_Empty()
        {
            try
            {
                // Không cần gán isBindingData=true ở đây nữa vì đã gán ở hàm gọi
                DataTable dtPhong = phongBLL.LayTatCaPhong(DateTime.Now);
                if (dtPhong != null)
                {
                    cboTenPhong.DataSource = dtPhong;
                    cboTenPhong.DisplayMember = "TenPhong";
                    cboTenPhong.ValueMember = "MaPhong";
                    cboTenPhong.SelectedIndex = -1;
                    cboTenPhong.Text = ""; // Xóa text hiển thị         
                }

                DataTable dtTB = ptbBLL.LayTatCaThietBi();
                if (dtTB != null)
                {
                    cboTenTBP.DataSource = dtTB;
                    cboTenTBP.DisplayMember = "TenTB";
                    cboTenTBP.ValueMember = "MaTB";
                    cboTenTBP.SelectedIndex = -1;
                    cboTenTBP.Text = "";
                }
            }
            catch { }
        }

        private void FilterData()
        {
            if (isBindingData) return;

            int maPhong = -1;
            int maTB = -1;

            if (cboTenPhong.SelectedIndex != -1 && cboTenPhong.SelectedValue != null)
                maPhong = Convert.ToInt32(cboTenPhong.SelectedValue);

            if (cboTenTBP.SelectedIndex != -1 && cboTenTBP.SelectedValue != null)
                maTB = Convert.ToInt32(cboTenTBP.SelectedValue);

            // Bật cờ khi lọc để tránh SelectionChanged nhảy lung tung
            bool oldState = isBindingData;
            isBindingData = true;
            FilterData_Internal(maPhong, maTB);
            isBindingData = oldState;
        }

        private void FilterData_Internal(int maPhong, int maTB)
        {
            try
            {
                DataTable dt = ptbBLL.TimKiem(maPhong, maTB);
                dgvThietBiPhong.DataSource = dt;

                if (dgvThietBiPhong.Columns.Contains("MaPhong")) dgvThietBiPhong.Columns["MaPhong"].Visible = false;
                if (dgvThietBiPhong.Columns.Contains("MaTB")) dgvThietBiPhong.Columns["MaTB"].Visible = false;

                // QUAN TRỌNG: Bỏ chọn ngay lập tức
                dgvThietBiPhong.ClearSelection();
            }
            catch { }
        }

        private void dgvThietBiPhong_SelectionChanged(object sender, EventArgs e)
        {
            // Nếu đang binding (đang load lại trang/reset) -> Không được điền dữ liệu
            if (isBindingData) return;

            if (dgvThietBiPhong.SelectedRows.Count == 1)
            {
                // Bật cờ tạm thời để khi gán giá trị cho ComboBox, nó không gọi lại Filter
                isBindingData = true;

                DataGridViewRow row = dgvThietBiPhong.SelectedRows[0];

                if (dgvThietBiPhong.Columns.Contains("MaPhong") && row.Cells["MaPhong"].Value != null)
                    cboTenPhong.SelectedValue = Convert.ToInt32(row.Cells["MaPhong"].Value);

                if (dgvThietBiPhong.Columns.Contains("MaTB") && row.Cells["MaTB"].Value != null)
                {
                    int val = Convert.ToInt32(row.Cells["MaTB"].Value);
                    cboTenTBP.SelectedValue = val;
                    selectedMaTB_Tab2 = val;
                }

                if (dgvThietBiPhong.Columns.Contains("Số Lượng") && row.Cells["Số Lượng"].Value != null)
                    txtSoLuong.Text = row.Cells["Số Lượng"].Value.ToString();

                btnThemTBP.Enabled = true;
                btnXoaTBP.Enabled = true;

                cboTenTBP.Enabled = false; // Khóa khi sửa
                cboTenPhong.Enabled = false;

                isBindingData = false; // Mở lại cờ
            }
            else
            {
                // Nếu không chọn dòng nào (do ClearSelection)
                // Không làm gì để tránh reset form khi người dùng đang nhập dở
            }
        }

        private void ResetTab2(bool clearGrid)
        {
            // Bật cờ để chặn sự kiện
            bool oldState = isBindingData;
            isBindingData = true;

            selectedMaTB_Tab2 = -1;

            cboTenPhong.SelectedIndex = -1; cboTenPhong.Text = "";
            cboTenTBP.SelectedIndex = -1; cboTenTBP.Text = "";
            txtSoLuong.Clear();

            cboTenTBP.Enabled = true;
            cboTenPhong.Enabled = true;

            btnThemTBP.Enabled = true;
            btnXoaTBP.Enabled = false;

            if (clearGrid)
            {
                dgvThietBiPhong.ClearSelection();
                // Khi reset, load lại toàn bộ lưới để người dùng thấy tất cả
                FilterData_Internal(-1, -1);
            }

            isBindingData = oldState;
        }

        private void BtnCapNhatTBP_Click(object sender, EventArgs e)
        {
            if (cboTenPhong.SelectedIndex == -1) { MessageBox.Show("Chưa chọn phòng!"); return; }
            if (cboTenTBP.SelectedIndex == -1) { MessageBox.Show("Chưa chọn thiết bị!"); return; }
            if (!int.TryParse(txtSoLuong.Text, out int sl) || sl <= 0) { MessageBox.Show("Số lượng sai!"); return; }

            int maPhong = Convert.ToInt32(cboTenPhong.SelectedValue);
            int maTB = Convert.ToInt32(cboTenTBP.SelectedValue);

            string msgCheck = ptbBLL.KiemTraKhaDung(maPhong, maTB, sl);
            if (msgCheck != "OK")
            {
                MessageBox.Show(msgCheck, "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ptbBLL.CapNhatThietBiPhong(maPhong, maTB, sl))
            {
                MessageBox.Show("Cập nhật thành công!");
                cboTenTBP.Enabled = true;
                cboTenPhong.Enabled = true;
                FilterData();
            }
            else MessageBox.Show("Lỗi cập nhật CSDL.");
        }

        private void BtnXoaTBP_Click(object sender, EventArgs e)
        {
            if (dgvThietBiPhong.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Gỡ bỏ thiết bị này khỏi phòng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvThietBiPhong.SelectedRows)
                {
                    int mp = Convert.ToInt32(row.Cells["MaPhong"].Value);
                    int mt = Convert.ToInt32(row.Cells["MaTB"].Value);
                    ptbBLL.XoaThietBiKhoiPhong(mp, mt);
                }
                MessageBox.Show("Đã gỡ bỏ!");
                FilterData();
                ResetTab2(true);
            }
        }

        // --- HÀM DESIGNER ---
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void btnXoaTB_Click_1(object sender, EventArgs e) { }
        private void dgvThietBi_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void frmQLThietBi_Load(object sender, EventArgs e) { LoadKhoThietBi(); }
        private void frmQLThietBi_Load_1(object sender, EventArgs e) { LoadKhoThietBi(); }
    }
}