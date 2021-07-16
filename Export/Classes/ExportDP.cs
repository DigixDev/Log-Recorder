using PdfSharp.Charting;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Export.Classes
{
    //     612 792
    public class ExportDP : ExportBase
    {
        private Log_Recorder.DA.Model.DP.Group _dpGroup;
        private XRect _landscapeRect;
        private XPen _chartPen, _litePen;
        private double _dX, _dY;
        public ExportDP()
            : base()
        {
            _landscapeRect = new XRect(20, 10, 780, 580);
            _chartPen = new XPen(XColors.SeaGreen, 1);
            _litePen = new XPen(XColors.LightGray, 0.1);
        }

        public void MakePDFReport(Log_Recorder.DA.Model.DP.Group group, PdfSharp.Pdf.PdfDocument document, int rotate=270)
        {
            double _startY;
            _dpGroup = group;
            try
            {
                PdfSharp.Pdf.PdfPage page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                _startY = _contentRect.Top;
                PdfSharp.Drawing.XGraphics graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                graphics.ScaleTransform(_scale);
                MakePage(graphics);
                page.Rotate = rotate;
                graphics.Dispose();
                AddBookmark(_dpGroup.GroupSheetData.ExploratoryHoleNo, page);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected void MakePage(XGraphics graphics)
        {
            Reset();
            DrawHeaderDP(graphics, 5);
            DrawLogoDP(graphics);
            DrawTable(graphics, _landscapeRect.Left + 15, _landscapeRect.Top + 110);
            DrawChart(graphics, 440, _landscapeRect.Top + 110, 330, 350);

            DrawFooterDP(graphics);
        }

        private void DrawChart(XGraphics graphics, double startX, double startY, double width, double height)
        {
            double maxDepth = 7, maxValue = 40, curY;
            int? value;
            GetMaxValues(out maxDepth, out maxValue);
            if (maxDepth == -1 || maxValue == -1)
                return;

            List<XPoint> points=new List<XPoint>();
            
            _dX = width / maxValue;
            _dY = height / maxDepth;

            DrawTextField(graphics, startX + width / 2, startY - 20, 30, _boldFont, _blackBrush, TextFieldAlign.Right, "Blows per 100mm increment");
            DrawTextFieldRotate(graphics, startX - 35, startY + height / 2, 200, 270, _boldFont, _blackBrush, TextFieldAlign.Right, "Depth (M bgl)");

            for (double y = 0; y <= maxDepth; y++)
            {
                curY= startY + y * _dY;
                DrawTextField(graphics, startX - 35,curY, 30, _normalFont, _blackBrush, TextFieldAlign.Right, y.ToString("N2"));
                graphics.DrawLine(_blackPen, startX - 3, curY, startX, curY);
                graphics.DrawLine(_litePen, startX, curY, startX + width, curY);
            }

            for(double x=0; x<=maxValue; x+=2)
            {
                if(x%10==0)
                {
                    graphics.DrawLine(_blackPen, startX + x * _dX, startY - 4, startX + x * _dX, startY);
                    DrawTextField(graphics, startX + x * _dX - 20, startY - 10, 40, _normalFont, _blackBrush, TextFieldAlign.Center, x.ToString());
                }
                else
                    graphics.DrawLine(_blackPen, startX + x * _dX, startY - 2, startX + x * _dX, startY);
            }

            for (int rowindex = 0; rowindex < 20; rowindex++)
            {
                for (int colIndex = 1; colIndex <= 10; colIndex++)
                {
                    value = _dpGroup.Rows[rowindex].GetAt(colIndex);
                    if (value.HasValue)
                        points.Add(new XPoint(value.Value * _dX + startX, (rowindex + colIndex * 0.1) * _dY + startY));
                }
            }

            if (points.Count > 0)
                graphics.DrawLines(_chartPen, points.ToArray());

            graphics.DrawRectangle(_blackPen, startX, startY, width, height);
        }

        private void GetMaxValues(out double maxDepth, out double maxValue)
        {
            maxDepth = maxValue = -1;
            int? value;
            for(int rowNumber=1; rowNumber<=20; rowNumber++)
            {
                for(int colIndex=1; colIndex<=10; colIndex++)
                {
                    value=_dpGroup.Rows[rowNumber-1].GetAt(colIndex);
                    if(value.HasValue)
                    {
                        maxDepth = Math.Max(maxDepth, rowNumber+colIndex*0.1);
                        maxValue = Math.Max(maxValue, value.Value);
                    }
                }
            }
            if (maxDepth != -1)
                maxDepth = Math.Ceiling(maxDepth);
            if (maxValue != -1)
                maxValue = Math.Ceiling(maxValue/10)*10;
        }

        private void DrawFooterDP(XGraphics graphics)
        {
            XRect rect = new XRect(_landscapeRect.Left, _landscapeRect.Bottom-40, _landscapeRect.Width, 40);

            DrawTextField(graphics, rect.Left, rect.Top, rect.Width, 40, _normalFont, null, TextFieldAlign.Center, "Sampling Code: U- Undisturbed   B - Large Disturbed    D - Small Disturbed    W - Water    (U*) Non recovery of Sample",
            "Address: ----------------------------------------------------",
            " Phone: --------------------------------------------------");
            graphics.DrawRectangle(_blackPen, rect);
        }

        private void DrawLogoDP(XGraphics graphics)
        {
            graphics.DrawImage(XImage.FromGdiPlusImage(Properties.Resources.logo), _landscapeRect.Left+(_landscapeRect.Width-620)/2, _landscapeRect.Top+10, 120, 30);
            graphics.DrawRectangle(_blackPen, _landscapeRect.Left , _landscapeRect.Top , _landscapeRect.Width, 45);
        }


        protected void DrawHeaderDP(XGraphics graphics, double startY)
        {
            DrawTextField(graphics, _landscapeRect.Right - 200, _landscapeRect.Top, 200, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Center, "Probe No:");
            DrawTextField(graphics, _landscapeRect.Right - 200, _landscapeRect.Top + 15, 200, 30, _boldFont, _blackPen, TextFieldAlign.Center, _dpGroup.ProbeNumber);

            DrawTextField(graphics, _landscapeRect.Right - 500, _landscapeRect.Top, 100, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Project No:");
            DrawTextField(graphics, _landscapeRect.Right - 400, _landscapeRect.Top, 200, 15, _normalFont, _blackPen, TextFieldAlign.Left, GlobalData.Project.ProjectNumber);
            
            DrawTextField(graphics, _landscapeRect.Right - 500, _landscapeRect.Top+15, 100, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Site Address:");
            DrawTextField(graphics, _landscapeRect.Right - 400, _landscapeRect.Top+15, 200, 15, _normalFont, _blackPen, TextFieldAlign.Left, GlobalData.Project.SiteAddress);

            DrawTextField(graphics, _landscapeRect.Right - 500, _landscapeRect.Top+30, 100, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Client:");
            DrawTextField(graphics, _landscapeRect.Right - 400, _landscapeRect.Top+30, 200, 15, _normalFont, _blackPen, TextFieldAlign.Left, GlobalData.Project.ClientName);

            DrawTextField(graphics, _landscapeRect.Left + 15, _landscapeRect.Top + 60, 50, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Probe Type:");
            DrawTextField(graphics, _landscapeRect.Left + 65, _landscapeRect.Top + 60, 200, 15, _boldFont, _blackPen, TextFieldAlign.Left, _dpGroup.ProbeType);

            DrawTextField(graphics, _landscapeRect.Left + 15, _landscapeRect.Top + 75, 50, 15, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Operator:");
            DrawTextField(graphics, _landscapeRect.Left + 65, _landscapeRect.Top + 75, 200, 15, _boldFont, _blackPen, TextFieldAlign.Left, _dpGroup.Operator);

            graphics.DrawRectangle(_blackPen, _landscapeRect);
        }

        private void DrawTable(PdfSharp.Drawing.XGraphics graphics, double startX, double startY)
        {
            DrawTableHeader(graphics, startX, startY);
            startY += 15;
            foreach (var row in _dpGroup.Rows)
            {
                DrawTableRow(graphics, row, startX, startY);
                startY += 15;
            }
        }

        private void DrawTableRow(XGraphics graphics, Log_Recorder.DA.Model.DP.Row row, double startX, double startY)
        {
            DrawTextField(graphics, startX, startY, 50, 15, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Left, row.RowTitle);
            startX += 50;

            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V01));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V02));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V03));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V04));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V05));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V06));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V07));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V08));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V09));
            startX += 25;
            DrawTextField(graphics, startX, startY, 25, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.V10));
            startX += 25;

            DrawTextField(graphics, startX, startY, 40, 15, _normalFont, _blackPen, TextFieldAlign.Center, ValueToString(row.Torque));

        }

        private void DrawTableHeader(XGraphics graphics, double startX, double startY)
        {
            DrawTextField(graphics, startX, startY, 50, 15, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Left, "");
            startX += 50;
            for(int i=1; i<=10; i++)
            {
                DrawTextField(graphics, startX, startY, 25, 15, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, String.Format("+0.{0:00}", i));
                startX += 25;
            }

            DrawTextField(graphics, startX, startY, 40, 15, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Torque");
        }
    }
}
