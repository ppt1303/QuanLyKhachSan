using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
        
        private PanelControl pnlHeader;      
        private FlowLayoutPanel flowStats;    
        private FlowLayoutPanel flowFloors;  
        private TileControl tileControlPhong; 
        private DateEdit dtpNgay;
        private TimeEdit dtpGio;
        private SearchControl txtTimKiem;

        private PhongBLL _bll;
        private BookingBLL _bookingBLL;
        private DataTable _dtPhong;
        private int _filterTang = 0;
        private int _filterTrangThai = -1;
        public ucTrangChu()
        {
            InitializeComponent();

            _bll = new PhongBLL();
            _bookingBLL = new BookingBLL();

            KhoiTaoGiaoDien();

            tileControlPhong.ItemClick += TileControlPhong_ItemClick;
            this.Load += UcTrangChu_Load;

            BookingBLL.OnDataChanged += () =>
            {
                if (this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => LoadData()));
                    }
                    else
                    {
                        LoadData();
                    }
                }
            };
        }
        private void UcTrangChu_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadData();
            }
        }
        private void KhoiTaoGiaoDien()
        {
 
            pnlHeader = new PanelControl();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.Padding = new Padding(10);
            pnlHeader.BorderStyle = BorderStyles.NoBorder;

            dtpNgay = new DateEdit();
            dtpNgay.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Fluent;
            dtpNgay.DateTime = DateTime.Now;
            dtpNgay.Parent = pnlHeader;
            dtpNgay.Location = new Point(10, 15);
            dtpNgay.Width = 120;
            dtpNgay.Properties.NullText = "Chọn ngày";
            dtpNgay.EditValueChanged += (s, e) => LoadData(); 

            dtpGio = new TimeEdit();
            dtpGio.Time = DateTime.Now;
            dtpGio.Parent = pnlHeader;
            dtpGio.Location = new Point(140, 15);
            dtpGio.Width = 100;
            dtpGio.EditValueChanged += (s, e) => LoadData(); 

            txtTimKiem = new SearchControl();
            txtTimKiem.Parent = pnlHeader;
            txtTimKiem.Location = new Point(260, 15);
            txtTimKiem.Width = 300;
            txtTimKiem.Properties.NullValuePrompt = "Tìm tên phòng, loại phòng...";
            txtTimKiem.TextChanged += (s, e) => VeSoDoPhong(); 

            flowStats = new FlowLayoutPanel();
            flowStats.Dock = DockStyle.Top;
            flowStats.Height = 45;
            flowStats.Padding = new Padding(5);
            flowStats.BackColor = Color.WhiteSmoke;

            flowFloors = new FlowLayoutPanel();
            flowFloors.Dock = DockStyle.Top;
            flowFloors.Height = 50;
            flowFloors.Padding = new Padding(5);
            flowFloors.AutoScroll = true;    
            flowFloors.WrapContents = false; 

            tileControlPhong = new TileControl();
            tileControlPhong.Dock = DockStyle.Fill; 
            tileControlPhong.AllowDrag = false;
            tileControlPhong.Orientation = Orientation.Vertical;
            tileControlPhong.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
            tileControlPhong.ScrollMode = TileControlScrollMode.ScrollBar;
            tileControlPhong.Padding = new Padding(10);

            this.Controls.Add(tileControlPhong);
            this.Controls.Add(flowFloors);
            this.Controls.Add(flowStats);
            this.Controls.Add(pnlHeader);
        }
        public void LoadData()
        {
            try
            {
                DateTime ngay = dtpNgay.DateTime.Date;
                TimeSpan gio = dtpGio.Time.TimeOfDay;
                DateTime thoiDiemXem = ngay + gio;

                _dtPhong = _bll.GetDanhSachPhongTheoNgay(thoiDiemXem);

                if (_dtPhong == null || _dtPhong.Rows.Count == 0) return;

                TaoThanhBoLocVaThongKe(); 
                VeSoDoPhong();            
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }    
        private void TaoThanhBoLocVaThongKe()
        {
            flowStats.Controls.Clear();
            if (_dtPhong == null) return;

            string colName = "TrangThaiHienThi";
  
            int countAll = _dtPhong.Rows.Count;
            int countTrong = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 1);
            int countDangO = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 2);
            int countDatTruoc = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 3);
            int countChuaDon = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 4);
            int countBaoTri = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 0);

            AddStatButton(flowStats, $"TẤT CẢ ({countAll})", Color.Black, -1);
            AddStatButton(flowStats, $"TRỐNG ({countTrong})", Color.SeaGreen, 1);
            AddStatButton(flowStats, $"ĐANG Ở ({countDangO})", Color.Firebrick, 2);
            AddStatButton(flowStats, $"ĐẶT TRƯỚC ({countDatTruoc})", Color.Goldenrod, 3);
            AddStatButton(flowStats, $"CHƯA DỌN ({countChuaDon})", Color.Gray, 4);
            AddStatButton(flowStats, $"BẢO TRÌ ({countBaoTri})", Color.Purple, 0);

            flowFloors.Controls.Clear();

            SimpleButton btnAll = new SimpleButton { Text = "Mọi tầng", Size = new Size(100, 35), Tag = 0 };
            HighlightButton(btnAll, _filterTang == 0);
            btnAll.Click += FloorButton_Click;
            flowFloors.Controls.Add(btnAll);

            var listTang = _dtPhong.AsEnumerable()
                                   .Select(r => r.Field<byte>("Tang"))
                                   .Distinct()
                                   .OrderBy(t => t);

            foreach (byte tang in listTang)
            {
                SimpleButton btn = new SimpleButton { Text = "Tầng " + tang, Size = new Size(80, 35), Tag = (int)tang };
                HighlightButton(btn, _filterTang == (int)tang);
                btn.Click += FloorButton_Click;
                flowFloors.Controls.Add(btn);
            }
        }

        private void FloorButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            _filterTang = Convert.ToInt32(btn.Tag);

            VeSoDoPhong();            
            TaoThanhBoLocVaThongKe();
        }
        private void AddStatButton(FlowLayoutPanel panel, string text, Color color, int statusFilter)
        {
            SimpleButton btn = new SimpleButton { Text = text, Tag = statusFilter, AutoSize = true };
            btn.Padding = new Padding(5);
            btn.Margin = new Padding(3, 3, 10, 3);
            btn.Cursor = Cursors.Hand;

            if (_filterTrangThai == statusFilter)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = color;
            }

            btn.Click += (s, e) =>
            {
                _filterTrangThai = (int)((SimpleButton)s).Tag;
                VeSoDoPhong();
                TaoThanhBoLocVaThongKe();
            };

            panel.Controls.Add(btn);
        }

        private void HighlightButton(SimpleButton btn, bool isActive)
        {
            if (isActive)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btn.Appearance.ForeColor = Color.Black;
            }
        }

        private void VeSoDoPhong()
        {
            tileControlPhong.Groups.Clear();

            string filterExpr = "1=1";
            if (_filterTang > 0) filterExpr += $" AND Tang = {_filterTang}";

            string searchText = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filterExpr += $" AND (TenPhong LIKE '%{searchText}%' OR TenLP LIKE '%{searchText}%')";
            }

            DataRow[] rows = _dtPhong.Select(filterExpr, "Tang ASC, TenPhong ASC");

            if (_filterTrangThai != -1)
            {
                rows = rows.Where(r =>
                {
                    int code = Convert.ToInt32(r["TrangThaiHienThi"]);
                    return code == _filterTrangThai;
                }).ToArray();
            }

            if (rows.Length == 0) return;

            var distinctTang = rows.Select(r => r["Tang"]).Distinct().OrderBy(t => t);
            foreach (var tang in distinctTang)
            {
                TileGroup group = new TileGroup { Text = "TẦNG " + tang };
                tileControlPhong.Groups.Add(group);

                var phongInGroup = rows.Where(r => r["Tang"].ToString() == tang.ToString());
                foreach (DataRow row in phongInGroup)
                {
                    TileItem item = new TileItem();
                    FormatTileItem_Final(item, row);
                    item.Tag = row["MaPhong"]; 
                    group.Items.Add(item);
                }
            }
        }

        private void FormatTileItem_Final(TileItem item, DataRow row)
        {
            item.ItemSize = TileItemSize.Wide;
            item.Elements.Clear();

            string tenPhong = row["TenPhong"].ToString();
            string tenLP = row["TenLP"].ToString();
            int ttHienThi = Convert.ToInt32(row["TrangThaiHienThi"]);

            Color backColor;
            string statusText;
            string iconText;

            switch (ttHienThi)
            {
                case 0: 
                    backColor = Color.Purple;
                    statusText = "Đang bảo trì";
                    iconText = "🛠";
                    break;
                case 4: 
                    backColor = Color.Gray;
                    statusText = "Chưa dọn dẹp";
                    iconText = "🧹";
                    break;
                case 2: 
                    backColor = Color.Firebrick;
                    statusText = "Đang thuê";
                    iconText = "👤";
                    break;
                case 3: 
                    backColor = Color.Goldenrod;
                    statusText = "Đã đặt trước";
                    iconText = "📅";
                    break;
                case 1: 
                default:
                    backColor = Color.SeaGreen;
                    statusText = "Phòng trống";
                    iconText = "✔";
                    break;
            }

            item.AppearanceItem.Normal.BackColor = backColor;
            item.AppearanceItem.Normal.BorderColor = Color.Transparent;

            TileItemElement eTen = new TileItemElement { Text = tenPhong, TextAlignment = TileItemContentAlignment.TopLeft };
            eTen.Appearance.Normal.Font = new Font("Segoe UI", 16, FontStyle.Bold);

            TileItemElement eStatus = new TileItemElement { Text = statusText, TextAlignment = TileItemContentAlignment.TopRight };
            eStatus.Appearance.Normal.FontSizeDelta = -2;

            TileItemElement eIcon = new TileItemElement { Text = iconText, TextAlignment = TileItemContentAlignment.MiddleLeft };
            eIcon.Appearance.Normal.Font = new Font("Segoe UI", 24, FontStyle.Bold);

            TileItemElement eFooter = new TileItemElement { Text = tenLP, TextAlignment = TileItemContentAlignment.BottomLeft };
            eFooter.Appearance.Normal.FontSizeDelta = -2;

            item.Elements.Add(eTen);
            item.Elements.Add(eStatus);
            item.Elements.Add(eIcon);
            item.Elements.Add(eFooter);
        }

        private void TileControlPhong_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (e.Item.Tag == null) return;
                int maPhong = Convert.ToInt32(e.Item.Tag);

                DataRow[] rows = _dtPhong.Select($"MaPhong = {maPhong}");
                if (rows.Length == 0) return;

                DataRow row = rows[0];
                string tenPhong = row["TenPhong"].ToString();
                int trangThai = Convert.ToInt32(row["TrangThaiHienThi"]);

                switch (trangThai)
                {
                    case 1: 
                    case 4: 
                      
                        frmChiTietPhong frmTrong = new frmChiTietPhong(0, maPhong);
                        frmTrong.Text = $"Quản lý phòng {tenPhong} - TRỐNG";
                        frmTrong.ShowDialog();
                        break;

                    case 2: 

                        int maNP = _bookingBLL.GetCurrentStayID(maPhong);
                        if (maNP > 0)
                        {
                            frmChiTietPhong frmDangO = new frmChiTietPhong(maNP, maPhong);
                            frmDangO.Text = $"Chi tiết phòng {tenPhong} - ĐANG CÓ KHÁCH";
                            frmDangO.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show($"Lỗi: Không tìm thấy thông tin nhận phòng cho {tenPhong}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case 3: 
                        XtraMessageBox.Show($"Phòng {tenPhong} đã đặt trước. Vui lòng Check-in trong mục Đặt phòng.", "Thông báo");
                        break;

                    case 0: 
                        if (XtraMessageBox.Show($"Phòng {tenPhong} đang bảo trì. Mở khóa về Trống?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                           
                            bool kq = _bll.CapNhatTrangThai(maPhong, 1); 

                            if (kq)
                            {
                                XtraMessageBox.Show("Đã mở khóa phòng thành công!", "Thông báo");
                                
                                BookingBLL.NotifyDataChanged();
                              
                                LoadData();
                            }
                            else
                            {
                                XtraMessageBox.Show("Lỗi: Không thể cập nhật trạng thái.", "Lỗi");
                            }
                        }
                        break;
                }

                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ucTrangChu_Load_1(object sender, EventArgs e)
        {

        }
    }
}