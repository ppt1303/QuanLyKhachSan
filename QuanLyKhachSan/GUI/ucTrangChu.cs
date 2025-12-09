using DevExpress.XtraEditors;

using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;
using QuanLyKhachSan.DTO;

namespace QuanLyKhachSan.GUI
{
    public partial class ucTrangChu : XtraUserControl
    {
        private PhongBLL _bll;
        private DataTable _dtPhong;
        private int _filterTang = 0;

        public ucTrangChu()
        {
            InitializeComponent();
            _bll = new PhongBLL(); 
            tileControlPhong.AllowDrag = false; 
            tileControlPhong.Orientation = Orientation.Vertical; 
            tileControlPhong.ScrollMode = TileControlScrollMode.ScrollBar; 
            tileControlPhong.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top; 
        }

 
        public void LoadData()
        {
            try
            {
                _dtPhong = _bll.LayTatCaPhong();
                if (_dtPhong == null || _dtPhong.Rows.Count == 0) return;
                HienThiThongKe();   
                TaoBoLocTang();     
                VeSoDoPhong();      
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải trang chủ: " + ex.Message);
            }
        }
        private void HienThiThongKe()
        {
            pnlThongKe.Controls.Clear();
            RoomStatistics stats = _bll.TinhThongKe(_dtPhong);
            AddStatLabel("BẢO TRÌ: " + stats.BaoTri, Color.DimGray);
            AddStatLabel("BẨN: " + stats.Ban, Color.Gray);
            AddStatLabel("ĐẶT TRƯỚC: " + stats.DatTruoc, Color.Goldenrod);
            AddStatLabel("ĐANG Ở: " + stats.DangO, Color.Firebrick);
            AddStatLabel("TRỐNG: " + stats.Trong, Color.SeaGreen);
            AddStatLabel("TẤT CẢ: " + stats.TongSo, Color.Black);
        }

        private void AddStatLabel(string text, Color color)
        {
            LabelControl lbl = new LabelControl();
            lbl.Text = text;
            lbl.Appearance.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.Appearance.ForeColor = color;
            lbl.Padding = new Padding(15, 10, 15, 10);
            lbl.Dock = DockStyle.Left; 
            pnlThongKe.Controls.Add(lbl);
            lbl.BringToFront(); 
        }
        private void TaoBoLocTang()
        {
            pnlBoLoc.Controls.Clear();
            AddFilterButton("Tất cả", 0);
            DataTable dtTang = _bll.LayDanhSachTang(_dtPhong);
            if (dtTang != null)
            {
                foreach (DataRow row in dtTang.Rows)
                {
                    if (row["Tang"] != DBNull.Value)
                    {
                        int tang = Convert.ToInt32(row["Tang"]);
                        AddFilterButton("Tầng " + tang, tang);
                    }
                }
            }
        }

        private void AddFilterButton(string text, int tagValue)
        {
            SimpleButton btn = new SimpleButton();
            btn.Text = text;
            btn.Tag = tagValue; 
            btn.Click += BtnFilter_Click; 
            btn.Dock = DockStyle.Left;
            btn.Width = 80;
            btn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            pnlBoLoc.Controls.Add(btn);
            btn.BringToFront();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            _filterTang = Convert.ToInt32(btn.Tag);
            VeSoDoPhong();
        }
        private void VeSoDoPhong()
        {
            tileControlPhong.Groups.Clear();
            DataRow[] rows = _bll.LocPhongTheoTang(_dtPhong, _filterTang);

            if (rows.Length == 0) return;
            var distinctTang = rows.Select(r => r["Tang"]).Distinct().OrderBy(t => t);
            foreach (var tang in distinctTang)
            {
                TileGroup group = new TileGroup();
                group.Text = "TẦNG " + tang;
                tileControlPhong.Groups.Add(group);
                var phongInGroup = rows.Where(r => r["Tang"].ToString() == tang.ToString());

                foreach (DataRow row in phongInGroup)
                {
                    TileItem item = new TileItem();
                    FormatTileItem(
                        item,
                        row["TenPhong"].ToString(),
                        row["TenLP"].ToString(),
                        row["TrangThaiO"].ToString(),
                        Convert.ToInt32(row["TrangThaiPhong"])
                    );
                    item.Tag = row["MaPhong"];
                    item.ItemClick += Item_ItemClick;

                    group.Items.Add(item);
                }
            }
        }
        private void FormatTileItem(TileItem item, string tenPhong, string loaiPhong, string trangThaiO, int trangThaiCode)
        {
            item.ItemSize = TileItemSize.Wide; 
            item.Elements.Clear();
            TileItemElement tenEl = new TileItemElement();
            tenEl.Text = tenPhong;
            tenEl.TextAlignment = TileItemContentAlignment.TopLeft;
            tenEl.Appearance.Normal.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            TileItemElement loaiEl = new TileItemElement();
            loaiEl.Text = loaiPhong;
            loaiEl.TextAlignment = TileItemContentAlignment.TopRight;
            loaiEl.Appearance.Normal.FontSizeDelta = -1;
            TileItemElement statusEl = new TileItemElement();
            statusEl.TextAlignment = TileItemContentAlignment.BottomLeft;
            Color backColor;
            string statusText;

            if (trangThaiCode == 0) 
            {
                backColor = Color.DimGray;
                statusText = "(Bảo trì)";
            }
            else if (trangThaiCode == 2) 
            {
                backColor = Color.Gray;
                statusText = "(Clean)";
            }
            else if (trangThaiO == "Đang ở") 
            {
                backColor = Color.Firebrick; 
                statusText = "(Đang ở)";
            }
            else if (trangThaiO == "Đặt trước") 
            {
                backColor = Color.Goldenrod; 
                statusText = "(Đã cọc)";
            }
            else 
            {
                backColor = Color.SeaGreen; 
                statusText = "(Trống)";
            }
            item.AppearanceItem.Normal.BackColor = backColor;
            statusEl.Text = statusText;
            item.Elements.Add(tenEl);
            item.Elements.Add(loaiEl);
            item.Elements.Add(statusEl);
        }
        private void Item_ItemClick(object sender, TileItemEventArgs e)
        {
            int maPhong = Convert.ToInt32(e.Item.Tag);
            XtraMessageBox.Show("Bạn đã chọn phòng: " + maPhong);
        }
    }
}