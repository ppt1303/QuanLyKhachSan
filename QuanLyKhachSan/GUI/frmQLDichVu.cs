using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

// Đảm bảo namespace này khớp với project của bạn
namespace QuanLyKhachSan.GUI
{
    public partial class frmQLDichVu : Form
    {
        // Vui lòng kiểm tra lại: Nếu tên Database là QuanLiDeTai, hãy sửa lại Initial Catalog.
        private const string CONNECTION_STRING = "Data Source=KARMA\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        // Biến lưu MaDV của dịch vụ được chọn để SỬA
        private int selectedMaDV_ForEdit = -1;

        public frmQLDichVu()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            dgvDIchVu.MultiSelect = true;
            dgvDIchVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Loại bỏ CellClick để tránh xung đột với SelectionChanged, nhưng vẫn giữ hàm để gán nếu cần.
            // dgvDIchVu.CellClick += dgvDIchVu_CellClick; // Giữ nguyên dòng này nếu bạn muốn CellClick vẫn có tác dụng.

            // ************ QUAN TRỌNG: SỬ DỤNG SelectionChanged ************
            dgvDIchVu.SelectionChanged += dgvDIchVu_SelectionChanged;
            // **************************************************************

            // Gán sự kiện cho các nút
            this.Load += frmQLDichVu_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;

            // Ban đầu, vô hiệu hóa nút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        // =================================================================
        // A. SỰ KIỆN LOAD FORM
        // =================================================================
        private void frmQLDichVu_Load(object sender, EventArgs e)
        {
            LoadDichVu();
        }

        private void LoadDichVu()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    string query = "SELECT MaDV, TenDV, Gia FROM dbo.DICHVU ORDER BY MaDV";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDIchVu.DataSource = dt;

                    // Cấu hình hiển thị cột
                    dgvDIchVu.Columns["MaDV"].HeaderText = "Mã DV";
                    dgvDIchVu.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                    dgvDIchVu.Columns["Gia"].HeaderText = "Giá";
                    dgvDIchVu.Columns["Gia"].DefaultCellStyle.Format = "N0"; // Định dạng số
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================================================================
        // B. LOGIC CHỌN HÀNG (SelectionChanged - Logic chính)
        // =================================================================
        private void dgvDIchVu_SelectionChanged(object sender, EventArgs e)
        {
            int count = dgvDIchVu.SelectedRows.Count;

            // Trường hợp 1: 0 HÀNG ĐƯỢC CHỌN
            if (count == 0)
            {
                // ResetForm(); // Có thể gây lỗi stack overflow nếu gọi liên tục
                selectedMaDV_ForEdit = -1;
                txtTenDV.Clear();
                txtGia.Clear();
                btnThem.Enabled = true;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                return;
            }

            // Trường hợp 2: DUY NHẤT 1 HÀNG ĐƯỢC CHỌN (Cho phép Sửa)
            if (count == 1)
            {
                DataGridViewRow row = dgvDIchVu.SelectedRows[0];

                if (row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                {
                    selectedMaDV_ForEdit = Convert.ToInt32(row.Cells["MaDV"].Value);
                    txtTenDV.Text = row.Cells["TenDV"].Value.ToString();
                    txtGia.Text = row.Cells["Gia"].Value.ToString();

                    btnThem.Enabled = false;
                    btnSua.Enabled = true; // HIỂN THỊ NÚT SỬA
                    btnXoa.Enabled = true;
                }
            }

            // Trường hợp 3: NHIỀU HÀNG ĐƯỢC CHỌN (> 1)
            else
            {
                selectedMaDV_ForEdit = -1;
                txtTenDV.Clear();
                txtGia.Clear();

                btnThem.Enabled = false;
                btnSua.Enabled = false; // ẨN NÚT SỬA
                btnXoa.Enabled = true;
            }
        }

        // Hàm CellClick cũ: Giữ lại để tránh lỗi nếu bạn có gán nó ở Designer. 
        // Logic chính đã ở SelectionChanged.
        private void dgvDIchVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Logic chính đã chuyển sang SelectionChanged
            // Nếu bạn muốn CellClick kích hoạt SelectionChanged ngay lập tức,
            // bạn có thể gọi dgvDIchVu_SelectionChanged(sender, e); ở đây, 
            // nhưng thông thường nó đã được kích hoạt rồi.
        }


        // =================================================================
        // C. SỰ KIỆN CLICK NÚT THAO TÁC (Đã sửa lỗi Identity)
        // =================================================================

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenDV.Text) || string.IsNullOrEmpty(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên Dịch Vụ và Giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ************ SỬA LỖI IDENTITY ************
            // Loại bỏ GetNextMaDV() và không truyền @MaDV
            // Yêu cầu: sp_ThemDichVu trong SQL Server PHẢI được ALTER để không nhận @MaDV.
            // *******************************************

            try
            {
                ExecuteProcedure("sp_ThemDichVu",
                    new SqlParameter("@TenDV", txtTenDV.Text.Trim()),
                    new SqlParameter("@Gia", gia)
                );
                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
                // Không gọi ResetForm ở đây, LoadDichVu sẽ kích hoạt SelectionChanged và reset form
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaDV_ForEdit == -1)
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ExecuteProcedure("sp_SuaDichVu",
                    new SqlParameter("@MaDV", selectedMaDV_ForEdit),
                    new SqlParameter("@TenDV", txtTenDV.Text.Trim()),
                    new SqlParameter("@Gia", gia)
                );
                MessageBox.Show("Sửa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
                // Không gọi ResetForm ở đây, LoadDichVu sẽ kích hoạt SelectionChanged và reset form
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi sửa dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDIchVu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvDIchVu.SelectedRows.Count} dịch vụ đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Lấy danh sách MaDV của các hàng được chọn
                string maDVList = string.Join(",", dgvDIchVu.SelectedRows.Cast<DataGridViewRow>()
                    .Where(row => row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                    .Select(row => row.Cells["MaDV"].Value.ToString()));

                if (string.IsNullOrWhiteSpace(maDVList)) return;

                try
                {
                    ExecuteProcedure("sp_XoaNhieuDichVu",
                        new SqlParameter("@MaDVCs", maDVList)
                    );

                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi xóa dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =================================================================
        // D. HÀM HỖ TRỢ
        // =================================================================
        private void ResetForm()
        {
            txtTenDV.Clear();
            txtGia.Clear();
            selectedMaDV_ForEdit = -1;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dgvDIchVu.ClearSelection();
        }

        private void ExecuteProcedure(string procName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand command = new SqlCommand(procName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void txtGia_TextChanged(object sender, EventArgs e)
        {

        }

        // **HÀM GetNextMaDV đã bị loại bỏ vì xung đột với Identity Column.**
    }
}