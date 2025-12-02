using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class ucKhachHang : DevExpress.XtraEditors.XtraUserControl
    {
        public ucKhachHang()
        {
            InitializeComponent();
            this.Load += UcKhachHang_Load;
            gvKhachHang.RowUpdated += gvKhachHang_RowUpdated;
            gcKhachHang.DataSourceChanged += gcKhachHang_DataSourceChanged;
        }
        private void UcKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            CauHinhGiaoDienLuoi();
        }

        void LoadData()
        {
            try
            {
                string sql = "SELECT * FROM KHACHHANG ORDER BY MaKH DESC";
                DataTable dt = DatabaseHelper.GetDataTable(sql);
                gcKhachHang.DataSource = dt;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CauHinhGiaoDienLuoi()
        {
            gvKhachHang.OptionsView.ShowAutoFilterRow = true;
            gvKhachHang.OptionsBehavior.Editable = true;
            gvKhachHang.OptionsBehavior.ReadOnly = false;
        }
        private void gcKhachHang_DataSourceChanged(object sender, EventArgs e)
        {
            if (gvKhachHang.Columns["MaKH"] != null)
            {
                gvKhachHang.Columns["MaKH"].OptionsColumn.AllowEdit = false;
                gvKhachHang.Columns["MaKH"].AppearanceCell.BackColor = Color.LightGray; 
                gvKhachHang.Columns["MaKH"].Caption = "Mã KH";
            }
            if (gvKhachHang.Columns["HoTen"] != null) gvKhachHang.Columns["HoTen"].Caption = "Họ và Tên";
            if (gvKhachHang.Columns["SDT"] != null) gvKhachHang.Columns["SDT"].Caption = "Số điện thoại";
            if (gvKhachHang.Columns["CCCD"] != null) gvKhachHang.Columns["CCCD"].Caption = "CCCD/CMND";
        }
        private void gvKhachHang_RowUpdated(object sender, RowObjectEventArgs e)
        {
            DataRowView row = e.Row as DataRowView;
            if (row == null) return;

            try
            {
                int maKH = Convert.ToInt32(row["MaKH"]);
                string hoTen = row["HoTen"].ToString();
                string cccd = row["CCCD"].ToString();
                string sdt = row["SDT"].ToString();
                string quocTich = row["QuocTich"].ToString();
                string gioiTinh = row["GioiTinh"].ToString();
                object ngaySinh = row["NgaySinh"];
                if (ngaySinh == DBNull.Value) ngaySinh = DBNull.Value;
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE KHACHHANG SET 
                                   HoTen = @HoTen, 
                                   CCCD = @CCCD, 
                                   SDT = @SDT, 
                                   QuocTich = @QuocTich, 
                                   GioiTinh = @GioiTinh,
                                   NgaySinh = @NgaySinh
                                   WHERE MaKH = @MaKH";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@CCCD", cccd);
                        cmd.Parameters.AddWithValue("@SDT", sdt);
                        cmd.Parameters.AddWithValue("@QuocTich", quocTich);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@MaKH", maKH);

                        cmd.ExecuteNonQuery();
                    }
                }
                XtraMessageBox.Show("Đã lưu thay đổi!", "Thông báo");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData(); 
            }
        }
    }
}