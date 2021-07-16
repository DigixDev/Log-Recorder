using Export.Model;
using Log_Recorder.DA.Class;
using Log_Recorder.DA.Model;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Export.Classes
{
    //     612 792
    public class ExportBase
    {
        #region private variables

        protected const string _fontName = "Verdana";
        protected const double _pageMargin = 5;
        protected const double _textMargin = 3;
        protected const double _scale = 0.97;
        public readonly XBrush _greenBrush;
        public readonly XBrush _grayBrush;
        public readonly XBrush _markerBrush;

        public double _prevTextY = 0, _prevImageY = 0;
        public System.Drawing.Font _vFont;
        public System.Drawing.Graphics _vGraphics;
        public PdfSharp.Drawing.XRect _contentRect;
        public PdfSharp.Drawing.XFont _smallFont, _normalFont, _boldFont;
        public PdfSharp.Drawing.XPen _blackPen;
        public PdfSharp.Drawing.XPen _redPen;
        public PdfSharp.Drawing.XPen _blackThinPen;
        public PdfSharp.Drawing.XBrush _blackBrush;
        public PdfSharp.Drawing.XStringFormat _stringLeftFormat, _stringCenterFormat, _stringRightFormat;
        private StratumBoxManager _stratumBoxManager;
        private InstallationBoxManager _installationBoxManager;

        protected readonly double _baseDepth = 4;
        protected double _prevSampleDepth, _sampleOffset;
        protected int _maxDepthPerPage = 5;
        //private double _installationPrevDepth= 0;
        #endregion
        #region constructor

        public ExportBase()
        {
            _contentRect = new PdfSharp.Drawing.XRect(_pageMargin, _pageMargin, 610 - _pageMargin * 2-20, 790 - _pageMargin * 2);

            _greenBrush = new XSolidBrush(XColor.FromArgb(208, 246, 204));
            _grayBrush = new XSolidBrush(XColor.FromArgb(210, 210, 210));

            _smallFont = new PdfSharp.Drawing.XFont(_fontName, 5);
            _normalFont = new PdfSharp.Drawing.XFont(_fontName, 6);
            _boldFont = new PdfSharp.Drawing.XFont(_fontName, 6, PdfSharp.Drawing.XFontStyle.Bold);
            _vFont = new System.Drawing.Font(_fontName, 6);
            _blackPen = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Black, 0.3);
            _redPen = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Red, 0.3);
            _blackThinPen = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Gray, 0.1);
            _blackBrush = PdfSharp.Drawing.XBrushes.Black;
            _markerBrush = new XSolidBrush(XColor.FromArgb(150, 228, 252, 91));

            _stringLeftFormat = new PdfSharp.Drawing.XStringFormat();
            _stringCenterFormat = new PdfSharp.Drawing.XStringFormat();
            _stringRightFormat = new PdfSharp.Drawing.XStringFormat();

            _stringLeftFormat.LineAlignment = _stringCenterFormat.LineAlignment = _stringRightFormat.LineAlignment = PdfSharp.Drawing.XLineAlignment.Center;
            _stringLeftFormat.Alignment = PdfSharp.Drawing.XStringAlignment.Near;
            _stringCenterFormat.Alignment = PdfSharp.Drawing.XStringAlignment.Center;
            _stringRightFormat.Alignment = PdfSharp.Drawing.XStringAlignment.Far;

            _vGraphics = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(10, 10));
            _vGraphics.PageUnit = System.Drawing.GraphicsUnit.Point;

            _stratumBoxManager = new StratumBoxManager();
            _installationBoxManager = new InstallationBoxManager();
        }

        #endregion

        protected virtual void MakePage(XGraphics graphics, int currentPage, int totalPage) { }

        protected double GetBestHeight(string text, double width, double height)
        {
            double iHeight = _vGraphics.MeasureString(text, _vFont, (int)width).Height;
            return iHeight > height ? Math.Ceiling(iHeight) : height;
        }

        protected void AddBookmark(string title, PdfSharp.Pdf.PdfPage page)
        {
            if (page.Owner.Outlines.Count == 0)
                page.Owner.Outlines.Add(GlobalData.Project.ProjectNumber, page.Owner.Pages[0]);
            PdfSharp.Pdf.PdfOutline outline= page.Owner.Outlines[0];
            outline.Outlines.Add(title, page);
        }

        protected static PdfSharp.Drawing.XImage GetImage(int width, int height, int id)
        {
            XImage result;
            System.Drawing.Image image = Export.Properties.Resources.ResourceManager.GetObject("_" + id.ToString()) as System.Drawing.Image;
            if (image.Width == width && image.Height==height)
                result = XImage.FromGdiPlusImage(image);
            else
            {
                System.Drawing.Bitmap target = new System.Drawing.Bitmap(width * 2, height * 2);
                using (System.Drawing.TextureBrush brush = new System.Drawing.TextureBrush(image))
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(target))
                {
                    g.FillRectangle(brush, 0, 0, width * 2, height * 2);
                }
                result = XImage.FromGdiPlusImage(target);
            }
            result.Interpolate = false;
            return result;
        }

        protected void DrawFooter(PdfSharp.Drawing.XGraphics graphics, double startY)
        {
            XRect rect = new XRect(_contentRect.Left, startY, 585, 40);
            
            DrawTextField(graphics, _contentRect.Left, startY, _contentRect.Width, 40, _normalFont, null, TextFieldAlign.Center, "Sampling Code: U- Undisturbed   B - Large Disturbed    D - Small Disturbed    W - Water    (U*) Non recovery of Sample",
            "Address: ----------------------------------------------------",
            " Phone: --------------------------------------------------");
            graphics.DrawRectangle(_blackPen, rect);
        }

        protected string ValueToString(object value)
        {
            if (value == null)
                return String.Empty;
            if (value is double)
                return ((double)value).ToString("N2");
            if(value is string)
                return value as string;
            return value.ToString();
        }

        protected void DrawRuller(XGraphics graphics, double startX, double startY, double height, int currentPage)
        {
            int count = 0, offset = currentPage - 1;
            double depth = 0;
            for(int y =4; depth <= _maxDepthPerPage; y += 10)
            {
                if (count == 0 || count % 5 == 0)
                {
                    if (count == 0)
                        graphics.DrawLine(_blackPen, startX + 17, y + startY, 590, y + startY);
                    else
                        graphics.DrawLine(_blackPen, startX + 17, y + startY, startX + 25, y + startY);
                    graphics.DrawString((depth + offset * _maxDepthPerPage).ToString("N2"), _normalFont, XBrushes.Black, startX, y + startY, _stringLeftFormat);
                    depth += 0.5;
                }
                else
                    graphics.DrawLine(_blackPen, startX + 20, y + startY, startX + 25, y + startY);
                count++;
            }
        }

        protected void DrawRemarks(XGraphics graphics, double startY, ObservableCollection<string> Remarks)
        {
            string temp;
            DrawTextField(graphics, 5, startY, 585, 10, _boldFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Remarks");
            startY += 10;
            for (int i = 0; i < 4; i++)
            {
                DrawTextField(graphics, 5, startY, 585, 10, _normalFont, _blackPen, TextFieldAlign.Left, (i + 1).ToString() + ":");
                if (i < Remarks.Count)
                    temp = Remarks[i];
                else
                    temp = "";
                DrawTextField(graphics, 15, startY, 550, 10, _normalFont, null, TextFieldAlign.Left, temp);
                startY += 10;
            }
        }

        protected void DrawSampleOrTest(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage, ObservableCollection<Log_Recorder.DA.Model.SampleOrTest> sampleOrTestList)
        {
            startY += 40;

            foreach (var sampleOrTest in sampleOrTestList.OrderBy(e=>e.Depth))
                DrawSampleOrTestLine(graphics, startY + _baseDepth, sampleOrTest, currentPage);
           
            startY -= 40;
            DrawSampleOrTestHeader(graphics, startY, currentPage);
        }

        protected void DrawSampleOrTestHeader(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage)
        {
            DrawTextField(graphics, 5, startY, 230, 10, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Sample or Tests");

            startY += 10;
            DrawTextField(graphics, 5, startY, 50, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Type");
            DrawTextField(graphics, 55, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Depth", "(mbgl)");
            DrawTextField(graphics, 95, startY, 140, 20, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Result");

            startY += 20;
            DrawTextField(graphics, 95, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 115, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 135, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 155, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 175, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 195, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
            DrawTextField(graphics, 215, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "N");

            startY += 10;
            graphics.DrawRectangles(_blackPen, new XRect[]{
                new XRect(5, startY, 50, 520)
                ,new XRect(95, startY, 20, 520)
                ,new XRect(135, startY, 20, 520)
                ,new XRect(175, startY, 20, 520)
                ,new XRect(215, startY, 20, 520)
            });
        }

        protected void DrawSampleOrTestLine(XGraphics graphics, double startY, Log_Recorder.DA.Model.SampleOrTest sampleOrTest, int currentPage)
        {
            double offset = currentPage - 1;

            if ((sampleOrTest.Depth >= offset * _maxDepthPerPage) && (sampleOrTest.Depth <= ((offset + 1) * _maxDepthPerPage)))
            {
                if (sampleOrTest.Depth != _prevSampleDepth)
                    _sampleOffset = 0;
                double currentY = (sampleOrTest.Depth - offset * _maxDepthPerPage) * 100 + startY + _sampleOffset;
                DrawTextField(graphics, 5, currentY, 50, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Type));
                if (_sampleOffset == 0)
                    DrawTextField(graphics, 55, currentY, 40, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Depth));
                DrawTextField(graphics, 95, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R1));
                DrawTextField(graphics, 115, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R2));
                DrawTextField(graphics, 135, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R3));
                DrawTextField(graphics, 155, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R4));
                DrawTextField(graphics, 175, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R5));
                DrawTextField(graphics, 195, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R6));
                DrawTextField(graphics, 215, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.RN));
                _sampleOffset += 10;
                if (String.IsNullOrEmpty(sampleOrTest.Comment) == false)
                {
                    graphics.DrawRectangle(_markerBrush, 5, currentY + 5, 230, 10);
                    DrawTextField(graphics, 55, currentY + 10, 185, _normalFont, _blackBrush, TextFieldAlign.Left, ValueToString(sampleOrTest.Comment));
                    _sampleOffset += 10;
                }
                _prevSampleDepth = sampleOrTest.Depth;
            }
        }

        protected void InitStrataList(ObservableCollection<Log_Recorder.DA.Model.Strata> StrataList, int width)
        {
            _stratumBoxManager.Clear();
            IEnumerable<Log_Recorder.DA.Model.Strata> strataList= StrataList.OrderBy((e) => e.HoleDepth);
            foreach (var item in strataList)
                _stratumBoxManager.Add(item, GetTextHeight(item.Description, width), _maxDepthPerPage);
        }

        protected void InitInstallationList(ObservableCollection<Log_Recorder.DA.Model.Installation> InstallationList)
        {
            _installationBoxManager.Clear();
            IEnumerable<Log_Recorder.DA.Model.Installation> installationList = InstallationList.OrderBy((e) => e.Depth);
            foreach (var item in installationList)
                _installationBoxManager.Add(item, _maxDepthPerPage);
        }

        protected void DrawStrataList(PdfSharp.Drawing.XGraphics graphics, double startX, double startY, double width, double height, int currentPage, ObservableCollection<Log_Recorder.DA.Model.WaterLevel> waterLevelList)
        {
            int page, curY;
            double waterLevelOnStrike, prevEndTextY = 0;
            
            DrawStrataListHeader(graphics, startX, startY, width, height);
            startY += 40;
            DrawRuller(graphics, startX+5, startY, height, currentPage);

            startY += _baseDepth;
            foreach (var item in waterLevelList.OrderBy((e) => e.CasingDepth))
            {
                if (Double.TryParse(item.WaterLevelOnStrike, out waterLevelOnStrike) == false && item.CasingDepth.HasValue == true)
                    waterLevelOnStrike = item.CasingDepth.Value;
                else
                    continue;
                page = (int)Math.Ceiling(waterLevelOnStrike / _maxDepthPerPage);
                if ((waterLevelOnStrike % _maxDepthPerPage) == 0)
                    page++;
                //page = (int)Math.Ceiling(item.WaterLevelOnStrike / _maxDepthPerPage);
                //if ((item.WaterLevelOnStrike % _maxDepthPerPage) == 0)
                //    page++;
                if (page == currentPage)
                {
                    curY = (int)(startY + (item.CasingDepth % _maxDepthPerPage) * 100);
                    if (curY == startY)
                        curY += 3;
                    DrawTextField(graphics, startX + 110, curY, 40, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(item.CasingDepth));
                }
            }

            foreach (var box in _stratumBoxManager.listOfBoxes)
            {
                if (box.CurrentPage == currentPage)
                {
                    box.UpdateTextHeight(ref prevEndTextY, GetTextHeight(box.BoxStrata.Description, (int)(width - 130)));
                    DrawStrataLine(graphics, startX, startY, width, box, box.BoxStrata.Description, box.BoxStrata.Code, currentPage);
                }
            }
        }

        private void DrawStrataListHeader(XGraphics graphics, double startX, double startY, double width, double height)
        {
            DrawTextField(graphics, startX, startY, 30, 40, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "");
            DrawTextField(graphics, startX+150, startY, width-130, 40, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Strata Description");
            DrawTextField(graphics, startX+ 30, startY, 120, 10, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Strata");
            startY += 10;
            DrawTextField(graphics, startX+30, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Legend");
            DrawTextField(graphics, startX+70, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Depth", "(mbgl)");
            DrawTextField(graphics, startX+110, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Water", "Strikes", "(mbgl)");
            startY += 30;

            graphics.DrawRectangles(_blackPen, new XRect[]{
                new XRect(startX, startY, 30, height),
                new XRect(startX+30, startY, 40, height),
                new XRect(startX+110, startY, 40, height),
                new XRect(startX+150, startY, width-130, height)
            });
        }

        private double GetTextHeight(string text, int width)
        {
            return Math.Max(Math.Ceiling(_vGraphics.MeasureString(text, _vFont, (int)width-20).Height), 10);
        }

        private void DrawStrataLine(XGraphics graphics,double startX, double startY, double width, StratumBox box, string description, int id, int currentPage)
        {
            XRect imageRect = new XRect(startX + 30, box.StartImageY + startY, 40, box.ImageHeight);
            XRect textRect = new XRect(startX + 150, box.StartTextY + startY, width - 130, box.TextHeight);

            if (id < 100)
                return;
            if (String.IsNullOrEmpty(description))
                description = String.Empty;
            graphics.DrawImage(GetImage(40, (int)imageRect.Height, id), imageRect);
            graphics.DrawRectangle(_blackPen, imageRect);
            if (box.IsLastPart)
                DrawTextField(graphics, startX + 70, box.EndTextY + startY - 8, 40, 8, _normalFont, null, TextFieldAlign.Center, ValueToString(box.BoxStrata.HoleDepth));
          //  DrawTextField(graphics, startX + 150, box.StartTextY + startY, width-120, 10, _normalFont, null, null, TextFieldAlign.Left, box.BoxStrata.Description);
            DrawTextParagraph(graphics, textRect, _normalFont, _blackBrush, box.BoxStrata.Description);

           // graphics.DrawRectangle(_redPen, textRect);

            graphics.DrawLine(_blackPen, startX+70, box.EndImageY + startY, startX+80, box.EndTextY + startY);
            graphics.DrawLine(_blackPen, startX + 80, box.EndTextY + startY, startX + width + 20, box.EndTextY + startY);
        }

        protected void Reset()
        {
            _sampleOffset = _prevSampleDepth = _prevTextY = _prevImageY = 0;
        }

        protected void DrawInstallationHeader(XGraphics graphics, double startX, double startY)
        {
            DrawTextField(graphics, startX, startY, 40, 40, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Installation");
            startY += 40;
            graphics.DrawRectangle(_blackPen, new XRect(startX, startY, 40, 520));
        }

        protected void DrawInstallation(XGraphics graphics, double startX, double startY, int currentPage, ObservableCollection<Log_Recorder.DA.Model.Installation> installationList)
        {
            DrawInstallationHeader(graphics, startX, startY);
            startY += 40;

            foreach (InstallationBox box in _installationBoxManager.listOfBoxes)
                if (box.CurrentPage == currentPage)
                    DrawInstallationLine(graphics, startX, startY + box.StartY, startY + box.EndY, box.BoxInstallation.Code);
        }

        private int DepthToInt(double depth, double startY, bool isStart)
        {
            if (depth == 0)
                return (int)startY;
            if (depth % _maxDepthPerPage == 0)
            {
                if (isStart)
                    return (int)(startY);
                else
                    return (int)(_maxDepthPerPage * 100 + startY);
            }

            return (int)((depth % _maxDepthPerPage) * 100 + startY);
        }

        private int GetPage(double depth, bool isStart)
        {
            if (depth == 0)
                return 1;
            int temp = (int)(depth / _maxDepthPerPage);
            if ((depth % _maxDepthPerPage) == 0 && isStart == false)
                return temp;
            else
                return temp + 1;
        }

        private void DrawInstallationLine(XGraphics graphics, double startX, double startY, double endY, int code)
        {
            int height = (int)(endY - startY);
            startY += _baseDepth;
            endY += _baseDepth;

            if (code < 200)
                return;

            XRect imageRect = new XRect(startX, startY, 40, height);
            XImage image = GetImage(40, height, code);
            graphics.DrawImage(image, imageRect);
            graphics.DrawRectangle(_blackPen, imageRect);
        }

        protected void DrawWaterLevels(XGraphics graphics, double startY, ObservableCollection<Log_Recorder.DA.Model.WaterLevel> WaterLevelList)
        {
            double t = 78;
            DrawTextField(graphics, 5, startY, 585, 10, _boldFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Water levels recorded during boring, m");
            DrawTextField(graphics, 5, startY + 10, 115, 10, _normalFont, _blackPen, TextFieldAlign.Left, "Date:");
            DrawTextField(graphics, 5, startY + 20, 115, 10, _normalFont, _blackPen, TextFieldAlign.Left, "Hole depth:");
            DrawTextField(graphics, 5, startY + 30, 115, 10, _normalFont, _blackPen, TextFieldAlign.Left, "Casing depth:");
            DrawTextField(graphics, 5, startY + 40, 115, 10, _normalFont, _blackPen, TextFieldAlign.Left, "Level water on strike:");
            DrawTextField(graphics, 5, startY + 50, 115, 10, _normalFont, _blackPen, TextFieldAlign.Left, "Water Level after 20mins:");

            for (int cols = 0; cols < 6; cols++)
            {
                if (cols == 5)
                    t = 80;
                double top = startY + 10;
                if (cols < WaterLevelList.Count)
                {
                    DrawTextField(graphics, 120 + cols * 78, top, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, WaterLevelList[cols].Date.ToShortDateString());
                    DrawTextField(graphics, 120 + cols * 78, top + 10, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(WaterLevelList[cols].HoleDepth));
                    DrawTextField(graphics, 120 + cols * 78, top + 20, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(WaterLevelList[cols].CasingDepth));
                    DrawTextField(graphics, 120 + cols * 78, top + 30, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(WaterLevelList[cols].WaterLevelOnStrike));
                    DrawTextField(graphics, 120 + cols * 78, top + 40, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(WaterLevelList[cols].WaterLevelAfter20Mins));
                }
                else
                {
                    for (int rows = 0; rows < 5; rows++)
                    {
                        DrawTextField(graphics, 120 + cols * 78, top, t, 10, _normalFont, _blackPen, TextFieldAlign.Left, "");
                        top += 10;
                    }
                }
            }
        }

        protected double GetMax(ObservableCollection<Log_Recorder.DA.Model.Strata> StrataList)
        {
            double max = 0;
            foreach (var item in StrataList)
                max = Math.Max(item.HoleDepth, max);
            return max;
        }

        protected void DrawLogo(PdfSharp.Drawing.XGraphics graphics)
        {
            graphics.DrawImage(XImage.FromGdiPlusImage(Properties.Resources.logo), 130, 10, 120, 30);
            graphics.DrawRectangle(_blackPen, 5, _contentRect.Top, 351, 40);
        }

        protected void DrawHeader(PdfSharp.Drawing.XGraphics graphics, double startY, int current, int total, int sheetType, Log_Recorder.DA.Model.SheetData sheetData)
        {
            DateTime dt=new DateTime(2015, 6, 20);
            DrawTextField(graphics, 356, _contentRect.Top, 234, 10, _boldFont, _blackPen, _greenBrush, TextFieldAlign.Center, Log_Recorder.DA.Class.SheetType.GetSheetTitle(sheetType));

            DrawTextField(graphics, 356, _contentRect.Top+10, 110, 30, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Exploratory Hole No:");
            DrawTextField(graphics, 466, _contentRect.Top+10, 124, 30, _boldFont, _blackPen, TextFieldAlign.Center, sheetData.ExploratoryHoleNo);

            DrawTextField(graphics, 356, startY + 40, 110, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Project No:");
            DrawTextField(graphics, 466, startY + 40, 124, 10, _normalFont, _blackPen, TextFieldAlign.Center, GlobalData.Project.ProjectNumber);

            DrawTextField(graphics, 356, startY + 50, 110, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Ground Level:");
            DrawTextField(graphics, 466, startY + 50, 124, 10, _normalFont, _blackPen, TextFieldAlign.Center, sheetData.GroundLevel);

            DrawTextField(graphics, 356, startY + 60, 110, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Date Commenced:");
            if (sheetData.DateCommenced != null)
            {
                DrawTextField(graphics, 466, startY + 60, 124, 10, _normalFont, _blackPen, TextFieldAlign.Center, sheetData.DateCommenced.Value.ToShortDateString());
                if (sheetData.DateCommenced.Value > dt)
                    throw new Exception();
            }
            else
                graphics.DrawRectangle(_blackPen, 466, startY + 60, 124, 10);

            DrawTextField(graphics, 356, startY + 70, 110, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Date Completed:");
            if (sheetData.DateCompleted != null)
            {
                DrawTextField(graphics, 466, startY + 70, 124, 10, _normalFont, _blackPen, TextFieldAlign.Center, sheetData.DateCompleted.Value.ToShortDateString());
                if (sheetData.DateCompleted.Value > dt)
                    throw new Exception();
            }
            else
                graphics.DrawRectangle(_blackPen, 466, startY + 70, 124, 10);

            DrawTextField(graphics, 356, startY + 80, 110, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Sheet No:");
            DrawTextField(graphics, 466, startY + 80, 124, 10, _normalFont, _blackPen, TextFieldAlign.Center, String.Format("{0} Of {1}", current, total));

            //------------------- left -----------------------------
            DrawTextField(graphics, 5, startY + 40, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Site Address:");
            DrawTextField(graphics, 120, startY + 40, 236, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(GlobalData.Project.SiteAddress));

            DrawTextField(graphics, 5, startY + 50, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Client:");
            DrawTextField(graphics, 120, startY + 50, 236, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(GlobalData.Project.ClientName));

            DrawTextField(graphics, 5, startY + 60, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Logged By:");
            DrawTextField(graphics, 120, startY + 60, 236, 10, _normalFont, _blackPen, TextFieldAlign.Left,ValueToString(sheetData.LoggedBy));

            DrawTextField(graphics, 5, startY + 70, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Checked By:");
            DrawTextField(graphics, 120, startY + 70, 236, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(sheetData.CheckedBy));

            //--------------------------------------
            DrawTextField(graphics, 5, startY + 80, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Type and diameter of equipment:");
            DrawTextField(graphics, 120, startY + 80, 236, 10, _normalFont, _blackPen, TextFieldAlign.Left, ValueToString(sheetData.EquimnetType));
            //---------------------------------------
        }

        protected void DrawTextField(PdfSharp.Drawing.XGraphics graphics, double x, double y, double width, double height, PdfSharp.Drawing.XFont font, PdfSharp.Drawing.XPen pen, XBrush brush, TextFieldAlign align, params string[] texts)
        {
            PdfSharp.Drawing.XRect rect = new PdfSharp.Drawing.XRect(x, y, width, height);
            DrawTextField(graphics, rect, font, pen, brush, align, texts);
        }

        protected void DrawTextField(PdfSharp.Drawing.XGraphics graphics, double x, double y, double width, double height, PdfSharp.Drawing.XFont font, PdfSharp.Drawing.XPen pen, TextFieldAlign align, params string[] texts)
        {
            PdfSharp.Drawing.XRect rect = new PdfSharp.Drawing.XRect(x, y, width, height);
            DrawTextField(graphics, rect, font, pen, null, align, texts);
        }

        protected void DrawTextParagraph(PdfSharp.Drawing.XGraphics graphics, XRect rect, PdfSharp.Drawing.XFont font, XBrush brush, string text)
        {
            XRect textRect = new XRect(rect.Left + _textMargin, rect.Top, rect.Width - 2 * _textMargin, rect.Height);
            XTextFormatter tf = new XTextFormatter(graphics);
            tf.DrawString(text, font, brush, textRect, XStringFormats.TopLeft);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(500, 500));
            g.PageUnit = System.Drawing.GraphicsUnit.Point;
            System.Drawing.SizeF si = g.MeasureString(text, new System.Drawing.Font("Verdana", 6), (int)rect.Width);
            //graphics.DrawRectangle(XPens.Red, textRect);
        }

        protected void DrawTextField(PdfSharp.Drawing.XGraphics graphics, double x, double y, double width, PdfSharp.Drawing.XFont font, XBrush brush, TextFieldAlign align, string text)
        {
            XRect textRect;

            textRect = new XRect(x + _textMargin, y - 5, width - _textMargin * 2, 10);
            if (String.IsNullOrEmpty(text))
                return;
            switch (align)
            {
                case TextFieldAlign.Left:
                    graphics.DrawString(text, font, _blackBrush, textRect, _stringLeftFormat);
                    break;
                case TextFieldAlign.Center:
                    graphics.DrawString(text, font, _blackBrush, textRect, _stringCenterFormat);
                    break;
                case TextFieldAlign.Right:
                    graphics.DrawString(text, font, _blackBrush, textRect, _stringRightFormat);
                    break;
            }
        }

        protected void DrawTextFieldRotate(PdfSharp.Drawing.XGraphics graphics, double x, double y, double width, double rotateAngle, PdfSharp.Drawing.XFont font, XBrush brush, TextFieldAlign align, string text)
        {
            XRect textRect;
            XPoint centerPoint = new XPoint(x, y);
            graphics.Save();
            graphics.RotateAtTransform(rotateAngle, centerPoint);
            textRect = new XRect(x + _textMargin, y - 5, width - _textMargin * 2, 10);
            graphics.DrawString(text, font, _blackBrush, centerPoint, _stringCenterFormat);
            graphics.Restore();
        }

        protected void DrawTextField(PdfSharp.Drawing.XGraphics graphics, XRect rect, PdfSharp.Drawing.XFont font, PdfSharp.Drawing.XPen pen, XBrush brush, TextFieldAlign align, params string[] texts)
        {
            XRect textRect;

            if (pen != null)
                graphics.DrawRectangle(pen, brush, rect);

            if (font == null || texts.Length==0 || String.IsNullOrEmpty(texts[0]))
                return;

            if (texts.Length == 1)
                textRect = new XRect(rect.Left + _textMargin, rect.Top, rect.Width - 2 * _textMargin, rect.Height);
            else
            {
                double temp = (rect.Height - texts.Length * 10) / 2;
                textRect = new XRect(rect.Left + _textMargin, rect.Top + temp, rect.Width - 2 * _textMargin, 10);
            }
            switch (align)
            {
                case TextFieldAlign.Left:
                    graphics.DrawString(texts[0], font, _blackBrush, textRect, _stringLeftFormat);
                    break;
                case TextFieldAlign.Center:
                    if (texts.Length == 1)
                        graphics.DrawString(texts[0], font, _blackBrush, textRect, _stringCenterFormat);
                    else
                    {
                        foreach (string text in texts)
                        {
                            graphics.DrawString(text, font, _blackBrush, textRect, _stringCenterFormat);
                            textRect.Offset(0, 8);
                        }
                    }
                    break;
                case TextFieldAlign.Right:
                    graphics.DrawString(texts[0], font, _blackBrush, textRect, _stringRightFormat);
                    break;
            }
        }
    }
}
