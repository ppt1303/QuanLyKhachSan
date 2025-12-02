namespace QuanLyKhachSan.GUI
{
    partial class ucSoDoPhong
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tileControlRoom = new DevExpress.XtraEditors.TileControl();
            this.SuspendLayout();
            // 
            // tileControlRoom
            // 
            this.tileControlRoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControlRoom.Location = new System.Drawing.Point(0, 0);
            this.tileControlRoom.Name = "tileControlRoom";
            this.tileControlRoom.Size = new System.Drawing.Size(1382, 838);
            this.tileControlRoom.TabIndex = 0;
            this.tileControlRoom.Text = "tileControl1";
        
            // 
            // ucSoDoPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tileControlRoom);
            this.Name = "ucSoDoPhong";
            this.Size = new System.Drawing.Size(1382, 838);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TileControl tileControlRoom;
    }
}
