using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QuanLyKhachSan
{
    public partial class rptHoaDon : DevExpress.XtraReports.UI.XtraReport
    {
        public rptHoaDon()
        {
            // 1. Cấu hình trang giấy
            this.Name = "rptHoaDon";
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);

            // 2. Gọi hàm vẽ giao diện
            CreateReportLayout();
        }

        private void CreateReportLayout()
        {
            // --- TẠO CÁC BĂNG (BANDS) ---
            DetailBand detail = new DetailBand();
            detail.HeightF = 30;

            ReportHeaderBand reportHeader = new ReportHeaderBand();
            reportHeader.HeightF = 150;

            ReportFooterBand reportFooter = new ReportFooterBand();
            reportFooter.HeightF = 100;

            this.Bands.AddRange(new Band[] { reportHeader, detail, reportFooter });

            // --- PHẦN ĐẦU: TIÊU ĐỀ ---
            XRLabel lblTitle = new XRLabel();
            lblTitle.Text = "HÓA ĐƠN THANH TOÁN";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblTitle.BoundsF = new RectangleF(0, 50, 650, 30);
            reportHeader.Controls.Add(lblTitle);

            // Tiêu đề bảng
            XRTable tableHeader = new XRTable();
            tableHeader.BoundsF = new RectangleF(0, 110, 650, 25);
            tableHeader.Borders = DevExpress.XtraPrinting.BorderSide.All;
            tableHeader.BackColor = Color.LightGray;
            tableHeader.Font = new Font("Arial", 10, FontStyle.Bold);

            XRTableRow headerRow = new XRTableRow();
            headerRow.Cells.Add(new XRTableCell { Text = "Nội Dung", WidthF = 450 });
            headerRow.Cells.Add(new XRTableCell { Text = "Thành Tiền", WidthF = 200, TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight });
            tableHeader.Rows.Add(headerRow);
            reportHeader.Controls.Add(tableHeader);

            // --- PHẦN THÂN: DỮ LIỆU (QUAN TRỌNG NHẤT) ---
            XRTable tableDetail = new XRTable();
            tableDetail.BoundsF = new RectangleF(0, 0, 650, 25);
            tableDetail.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;

            XRTableRow detailRow = new XRTableRow();

            // Ô 1: Bind vào cột [NoiDung]
            XRTableCell cellTen = new XRTableCell();
            cellTen.WidthF = 450;
            cellTen.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[NoiDung]")); // Khớp SQL
            cellTen.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);

            // Ô 2: Bind vào cột [ThanhTien]
            XRTableCell cellTien = new XRTableCell();
            cellTien.WidthF = 200;
            cellTien.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            cellTien.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ThanhTien]")); // Khớp SQL
            cellTien.TextFormatString = "{0:N0}";
            cellTien.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);

            detailRow.Cells.Add(cellTen);
            detailRow.Cells.Add(cellTien);
            tableDetail.Rows.Add(detailRow);
            detail.Controls.Add(tableDetail);

            // --- PHẦN ĐUÔI: TỔNG TIỀN ---
            // --- PHẦN 4: FOOTER (TỔNG TIỀN) ---

            // 1. Tạo Label Tổng
            XRLabel lblTong = new XRLabel();
            lblTong.BoundsF = new RectangleF(300, 10, 350, 25); // Chỉnh lại tọa độ xíu cho cân
            lblTong.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTong.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

            // 2. Cấu hình tính tổng (Quan trọng nhất đoạn này)
            XRSummary summary = new XRSummary();

            // Phải chọn là Report (Tính hết cả bài)
            summary.Running = SummaryRunning.Report;

            // QUAN TRỌNG: Phải chọn phép tính là SUM (Cộng). Cậu đang thiếu dòng này nè!
            summary.Func = SummaryFunc.Sum;

            // Format số tiền
            summary.FormatString = "{0:N0} VNĐ";
            summary.IgnoreNullValues = true;

            // 3. Gán dữ liệu vào
            // Lưu ý: "Text" phải bind vào cột [ThanhTien] thì nó mới biết lấy cột đó để cộng
            lblTong.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ThanhTien]"));
            lblTong.Summary = summary;

            // Thêm vào Footer
            reportFooter.Controls.Add(lblTong);
            
        }
    }
}