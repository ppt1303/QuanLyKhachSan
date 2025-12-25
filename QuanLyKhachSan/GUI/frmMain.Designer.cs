namespace QuanLyKhachSan.GUI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.fluentDesignFormContainer1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.aceTrangChu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceDatPhong = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceKhachHang = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceHoaDon = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceDichVu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceThietBi = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.acePhong = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // fluentDesignFormContainer1
            // 
            this.fluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer1.Location = new System.Drawing.Point(252, 39);
            this.fluentDesignFormContainer1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.fluentDesignFormContainer1.Name = "fluentDesignFormContainer1";
            this.fluentDesignFormContainer1.Size = new System.Drawing.Size(880, 664);
            this.fluentDesignFormContainer1.TabIndex = 0;
            this.fluentDesignFormContainer1.Click += new System.EventHandler(this.fluentDesignFormContainer1_Click);
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.aceTrangChu,
            this.aceDatPhong,
            this.aceKhachHang,
            this.aceHoaDon,
            this.aceDichVu,
            this.aceThietBi,
            this.acePhong});
            this.accordionControl1.Location = new System.Drawing.Point(0, 39);
            this.accordionControl1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(252, 664);
            this.accordionControl1.TabIndex = 1;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // aceTrangChu
            // 
            this.aceTrangChu.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceTrangChu.ImageOptions.SvgImage")));
            this.aceTrangChu.Name = "aceTrangChu";
            this.aceTrangChu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceTrangChu.Text = "Trang chủ";
            // 
            // aceDatPhong
            // 
            this.aceDatPhong.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceDatPhong.ImageOptions.SvgImage")));
            this.aceDatPhong.Name = "aceDatPhong";
            this.aceDatPhong.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceDatPhong.Text = "Quản lý Đặt phòng";
            // 
            // aceKhachHang
            // 
            this.aceKhachHang.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceKhachHang.ImageOptions.SvgImage")));
            this.aceKhachHang.Name = "aceKhachHang";
            this.aceKhachHang.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceKhachHang.Text = "Quản lý Khách hàng";
            // 
            // aceHoaDon
            // 
            this.aceHoaDon.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceHoaDon.ImageOptions.SvgImage")));
            this.aceHoaDon.Name = "aceHoaDon";
            this.aceHoaDon.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceHoaDon.Text = "Hóa đơn";
            // 
            // aceDichVu
            // 
            this.aceDichVu.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceDichVu.ImageOptions.SvgImage")));
            this.aceDichVu.Name = "aceDichVu";
            this.aceDichVu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceDichVu.Text = "Quản lý Dịch vụ";
            // 
            // aceThietBi
            // 
            this.aceThietBi.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("aceThietBi.ImageOptions.SvgImage")));
            this.aceThietBi.Name = "aceThietBi";
            this.aceThietBi.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.aceThietBi.Text = "Quản lý Vật tư";
            // 
            // acePhong
            // 
            this.acePhong.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("acePhong.ImageOptions.SvgImage")));
            this.acePhong.Name = "acePhong";
            this.acePhong.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.acePhong.Text = "Quản lý Phòng";
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1132, 39);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.Form = this;
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(54, 81);
            this.buttonEdit1.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEdit1.MenuManager = this.fluentFormDefaultManager1;
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Size = new System.Drawing.Size(152, 22);
            this.buttonEdit1.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(92, 80);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(115, 35);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "simpleButton1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1132, 703);
            this.ControlContainer = this.fluentDesignFormContainer1;
            this.Controls.Add(this.fluentDesignFormContainer1);
            this.Controls.Add(this.accordionControl1);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("frmMain.IconOptions.SvgImage")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "frmMain";
            this.NavigationControl = this.accordionControl1;
            this.Text = "Quản lý Khách sạn";
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer fluentDesignFormContainer1;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceDatPhong;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceKhachHang;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceDichVu;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceTrangChu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceHoaDon;
        private DevExpress.XtraBars.Navigation.AccordionControlElement acePhong;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceThietBi;
    }
}