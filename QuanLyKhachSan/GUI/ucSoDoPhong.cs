using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class ucSoDoPhong : DevExpress.XtraEditors.XtraUserControl
    {
        private Panel pnlLeft;
        private Panel pnlHeader;
        private FlowLayoutPanel flpRooms;
        private DateEdit dtpNgay;
        private TimeEdit dtpGio;
        private TextEdit txtTimKiem;
        private SimpleButton btnTimKiem;
        private RadioGroup rgTrangThai;
        private RadioGroup rgLoaiPhong;
        private RadioGroup rgDonDep;

        public ucSoDoPhong()
        {
            InitializeComponent();
            this.Load += UcSoDoPhong_Load;
        }

        private void UcSoDoPhong_Load(object sender, EventArgs e)
        {
            InitializeControls();
            LoadRoomData();
        }

        private void InitializeControls()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            // === HEADER PANEL ===
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White
            };

            // Menu Icon
            LabelControl lblMenu = new LabelControl
            {
                Text = "☰",
                Location = new Point(20, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Cursor = Cursors.Hand,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(30, 40)
            };
            pnlHeader.Controls.Add(lblMenu);

            // Title
            LabelControl lblTitle = new LabelControl
            {
                Text = "Phòng",
                Location = new Point(70, 35),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSizeMode = LabelAutoSizeMode.Vertical
            };
            pnlHeader.Controls.Add(lblTitle);

            // Chọn ngày
            LabelControl lblChonNgay = new LabelControl
            {
                Text = "Chọn ngày",
                Location = new Point(20, 10),
                ForeColor = Color.Gray
            };
            pnlHeader.Controls.Add(lblChonNgay);

            dtpNgay = new DateEdit
            {
                Location = new Point(20, 30),
                Size = new Size(150, 24),
                EditValue = new DateTime(2021, 5, 11)
            };
            dtpNgay.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            dtpNgay.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtpNgay.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            dtpNgay.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            pnlHeader.Controls.Add(dtpNgay);

            // Chọn giờ
            LabelControl lblChonGio = new LabelControl
            {
                Text = "Chọn giờ",
                Location = new Point(190, 10),
                ForeColor = Color.Gray
            };
            pnlHeader.Controls.Add(lblChonGio);

            dtpGio = new TimeEdit
            {
                Location = new Point(190, 30),
                Size = new Size(130, 24),
                EditValue = DateTime.Now
            };
            dtpGio.Properties.DisplayFormat.FormatString = "hh:mm tt";
            dtpGio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtpGio.Properties.EditFormat.FormatString = "hh:mm tt";
            dtpGio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            pnlHeader.Controls.Add(dtpGio);

            // Search Box
            txtTimKiem = new TextEdit
            {
                Location = new Point(850, 30),
                Size = new Size(300, 24)
            };
            txtTimKiem.Properties.NullValuePrompt = "Tìm phòng";
            txtTimKiem.Properties.NullValuePromptShowForEmptyValue = true;
            txtTimKiem.Properties.Appearance.Font = new Font("Segoe UI", 10);
            pnlHeader.Controls.Add(txtTimKiem);

            // Search Button
            btnTimKiem = new SimpleButton
            {
                Text = "▶",
                Location = new Point(1160, 28),
                Size = new Size(50, 28),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnTimKiem.Click += BtnTimKiem_Click;
            pnlHeader.Controls.Add(btnTimKiem);

            this.Controls.Add(pnlHeader);

            // === LEFT PANEL ===
            pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = Color.White,
                Padding = new Padding(15, 20, 15, 15)
            };

            int yPos = 10;

            // TRẠNG THÁI
            LabelControl lblTrangThai = new LabelControl
            {
                Text = "Trạng thái",
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 174, 239)
            };
            pnlLeft.Controls.Add(lblTrangThai);
            yPos += 30;

            rgTrangThai = new RadioGroup
            {
                Location = new Point(15, yPos),
                Size = new Size(200, 120),
                BorderStyle = BorderStyles.NoBorder
            };
            rgTrangThai.Properties.Items.Add(new RadioGroupItem(0, "Phòng trống"));
            rgTrangThai.Properties.Items.Add(new RadioGroupItem(1, "Phòng đã đặt"));
            rgTrangThai.Properties.Items.Add(new RadioGroupItem(2, "Phòng đang thuê"));
            rgTrangThai.Properties.Items.Add(new RadioGroupItem(3, "Tất cả phòng"));
            rgTrangThai.SelectedIndex = 3;
            pnlLeft.Controls.Add(rgTrangThai);
            yPos += 130;

            // LOẠI PHÒNG
            LabelControl lblLoaiPhong = new LabelControl
            {
                Text = "Loại phòng",
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 174, 239)
            };
            pnlLeft.Controls.Add(lblLoaiPhong);
            yPos += 30;

            rgLoaiPhong = new RadioGroup
            {
                Location = new Point(15, yPos),
                Size = new Size(200, 120),
                BorderStyle = BorderStyles.NoBorder
            };
            rgLoaiPhong.Properties.Items.Add(new RadioGroupItem(0, "Phòng đơn"));
            rgLoaiPhong.Properties.Items.Add(new RadioGroupItem(1, "Phòng đôi"));
            rgLoaiPhong.Properties.Items.Add(new RadioGroupItem(2, "Phòng gia đình"));
            rgLoaiPhong.Properties.Items.Add(new RadioGroupItem(3, "Tất cả loại phòng"));
            rgLoaiPhong.SelectedIndex = 3;
            pnlLeft.Controls.Add(rgLoaiPhong);
            yPos += 130;

            // DỌN DẸP
            LabelControl lblDonDep = new LabelControl
            {
                Text = "Dọn dẹp",
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 174, 239)
            };
            pnlLeft.Controls.Add(lblDonDep);
            yPos += 30;

            rgDonDep = new RadioGroup
            {
                Location = new Point(15, yPos),
                Size = new Size(200, 120),
                BorderStyle = BorderStyles.NoBorder
            };
            rgDonDep.Properties.Items.Add(new RadioGroupItem(0, "Đã dọn dẹp"));
            rgDonDep.Properties.Items.Add(new RadioGroupItem(1, "Chưa dọn dẹp"));
            rgDonDep.Properties.Items.Add(new RadioGroupItem(2, "Sửa chữa"));
            rgDonDep.Properties.Items.Add(new RadioGroupItem(3, "Tất cả"));
            rgDonDep.SelectedIndex = 3;
            pnlLeft.Controls.Add(rgDonDep);

            this.Controls.Add(pnlLeft);

            // === MAIN PANEL WITH ROOMS ===
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                AutoScroll = true,
                Padding = new Padding(20)
            };

            flpRooms = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(10)
            };

            pnlMain.Controls.Add(flpRooms);
            this.Controls.Add(pnlMain);
        }

        private void LoadRoomData()
        {
            flpRooms.Controls.Clear();

            // PHÒNG ĐƠN
            AddSectionHeader("Phòng đơn");
            AddRoomCard("P101", true, true, 0, "");
            AddRoomCard("P102", true, true, 0, "");
            AddRoomCard("P103", true, false, 0, "");
            AddRoomCard("P104", true, true, 0, "");
            AddRoomCard("P105", true, true, 0, "");
            AddRoomCard("P106", true, true, 0, "");

            // PHÒNG ĐÔI
            AddSectionHeader("Phòng đôi");
            AddRoomCard("P201", true, true, 0, "");
            AddRoomCard("P202", false, true, 5, "Phạm Hoàng Ta");
            AddRoomCard("P203", true, true, 0, "");
            AddRoomCard("P204", true, true, 0, "");
            AddRoomCard("P205", true, true, 0, "");
            AddRoomCard("P206", true, true, 0, "");
            AddRoomCard("P207", true, true, 0, "");
            AddRoomCard("P208", true, true, 0, "");
        }

        private void AddSectionHeader(string title)
        {
            LabelControl lblSection = new LabelControl
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 174, 239),
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(flpRooms.Width - 40, 30),
                Margin = new Padding(10, 20, 10, 10)
            };
            flpRooms.Controls.Add(lblSection);
            flpRooms.SetFlowBreak(lblSection, true);
        }

        private void AddRoomCard(string roomNumber, bool isAvailable, bool isClean, int days, string guestName)
        {
            Panel cardPanel = new Panel
            {
                Size = new Size(260, 140),
                Margin = new Padding(10),
                BackColor = isAvailable ? Color.FromArgb(100, 200, 150) : Color.FromArgb(120, 120, 120),
                Cursor = Cursors.Hand
            };

            // Room Number
            LabelControl lblRoomNumber = new LabelControl
            {
                Text = roomNumber,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(100, 20)
            };
            cardPanel.Controls.Add(lblRoomNumber);

            // Status Label (top right)
            LabelControl lblStatus = new LabelControl
            {
                Text = isAvailable ? "Phòng trống" : "Phòng đã đặt",
                Location = new Point(160, 10),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(90, 20)
            };
            cardPanel.Controls.Add(lblStatus);

            // Check Icon
            LabelControl lblCheck = new LabelControl
            {
                Text = "✓",
                Location = new Point(20, 35),
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(50, 50)
            };
            cardPanel.Controls.Add(lblCheck);

            // Main Status (Guest name or Phòng trống)
            LabelControl lblMainStatus = new LabelControl
            {
                Text = isAvailable ? "Phòng trống" : guestName,
                Location = new Point(80, 50),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(170, 25)
            };
            cardPanel.Controls.Add(lblMainStatus);

            // Bottom Info Panel
            Panel bottomPanel = new Panel
            {
                Location = new Point(0, 100),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(30, 0, 0, 0)
            };

            // Time Info
            LabelControl lblTime = new LabelControl
            {
                Text = isAvailable ? "🕐 0 giờ" : $"🕐 {days} ngày",
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(100, 20)
            };
            bottomPanel.Controls.Add(lblTime);

            // Clean Status
            LabelControl lblClean = new LabelControl
            {
                Text = isClean ? "✓ Đã dọn dẹp" : "✗ Chưa dọn dẹp",
                Location = new Point(130, 10),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSizeMode = LabelAutoSizeMode.None,
                Size = new Size(120, 20)
            };
            bottomPanel.Controls.Add(lblClean);

            cardPanel.Controls.Add(bottomPanel);

            // Click event
            cardPanel.Click += (s, e) => RoomCard_Click(roomNumber, isAvailable, guestName);
            cardPanel.Tag = roomNumber;

            flpRooms.Controls.Add(cardPanel);
        }

        private void RoomCard_Click(string roomNumber, bool isAvailable, string guestName)
        {
            string message = isAvailable
                ? $"Phòng {roomNumber} đang trống.\nBạn có muốn đặt phòng này?"
                : $"Phòng {roomNumber}\nKhách: {guestName}\nBạn có muốn xem chi tiết?";

            XtraMessageBox.Show(message, "Thông tin phòng", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtTimKiem.Text;
            if (!string.IsNullOrEmpty(searchText) && searchText != "Tìm phòng")
            {
                // Implement search logic here
                XtraMessageBox.Show($"Tìm kiếm: {searchText}", "Tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Item_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            // Handle tile item click if using TileControl
        }

        private void tileControlRoom_Click(object sender, EventArgs e)
        {

        }
    }
}