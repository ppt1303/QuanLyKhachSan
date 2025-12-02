using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class ucSoDoPhong : DevExpress.XtraEditors.XtraUserControl
    {
        public ucSoDoPhong()
        {
            InitializeComponent();
            this.Load += UcSoDoPhong_Load;
        }

        private void UcSoDoPhong_Load(object sender, EventArgs e)
        {
            LoadRoomList();
        }
        public void LoadRoomList()
        {
            try
            {
                tileControlRoom.Groups.Clear();
                TileGroup group1 = new TileGroup();
                group1.Text = "DANH SÁCH PHÒNG";
                tileControlRoom.Groups.Add(group1);

                string sql = "SELECT MaPhong, TenPhong, TrangThaiPhong, TenLP FROM PHONG P JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP";
                DataTable dt = DatabaseHelper.GetDataTable(sql);

                if (dt == null || dt.Rows.Count == 0) return;

                foreach (DataRow row in dt.Rows)
                {
                    TileItem item = new TileItem();
                    item.ItemSize = TileItemSize.Medium;
                    item.Text = row["TenPhong"].ToString();
                    item.Tag = row["MaPhong"];

                    TileItemElement subText = new TileItemElement();
                    subText.Text = row["TenLP"].ToString();
                    subText.TextAlignment = TileItemContentAlignment.BottomRight;
                    subText.Appearance.Normal.FontSizeDelta = -1;
                    item.Elements.Add(subText);

                    int trangThai = Convert.ToInt32(row["TrangThaiPhong"]);
                    switch (trangThai)
                    {
                        case 1: // Trống
                            item.AppearanceItem.Normal.BackColor = Color.ForestGreen;
                            item.Elements.Add(new TileItemElement { Text = "Trống", TextAlignment = TileItemContentAlignment.TopRight });
                            break;
                        case 2: // Đã đặt
                            item.AppearanceItem.Normal.BackColor = Color.Purple;
                            item.Elements.Add(new TileItemElement { Text = "Đã đặt", TextAlignment = TileItemContentAlignment.TopRight });
                            break;
                        case 3: // Có khách
                            item.AppearanceItem.Normal.BackColor = Color.Firebrick;
                            item.Elements.Add(new TileItemElement { Text = "Có khách", TextAlignment = TileItemContentAlignment.TopRight });
                            break;
                        default:
                            item.AppearanceItem.Normal.BackColor = Color.Gray;
                            item.Elements.Add(new TileItemElement { Text = "Bảo trì", TextAlignment = TileItemContentAlignment.TopRight });
                            break;
                    }

                    item.ItemClick += Item_ItemClick;
                    group1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sơ đồ: " + ex.Message);
            }
        }

        private void Item_ItemClick(object sender, TileItemEventArgs e)
        {
            string maPhong = e.Item.Tag.ToString();
            Color backColor = e.Item.AppearanceItem.Normal.BackColor;

            if (backColor == Color.ForestGreen)
            {
                frmDatPhong frm = new frmDatPhong(int.Parse(maPhong));
                frm.ShowDialog();
                LoadRoomList(); 
            }
            else if (backColor == Color.Purple)
            {
                frmDatPhong frm = new frmDatPhong(int.Parse(maPhong));
                frm.ShowDialog();
                LoadRoomList();

            }
            else if (backColor == Color.Firebrick)
            {
                MessageBox.Show($"Đang mở menu dịch vụ cho phòng: {e.Item.Text}");
                // Sau này sẽ code mở frmDichVu ở đây
            }
        }
    }
}