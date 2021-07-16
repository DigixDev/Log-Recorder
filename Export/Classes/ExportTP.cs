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
    public class ExportTP : ExportBase
    {
        private Log_Recorder.DA.Model.TP.Group _tpGroup;
        public ExportTP()
            : base()
        {
         }

        public void MakePDFReport(Log_Recorder.DA.Model.TP.Group group, PdfSharp.Pdf.PdfDocument document)
        {
            double _startY;
            double maxDepth;
            int totalPage;
            _tpGroup = group;
            _maxDepthPerPage = 5;
            try
            {
                maxDepth = GetMax(_tpGroup.StrataList);
                totalPage = (int)Math.Ceiling(maxDepth / _maxDepthPerPage);
                InitStrataList(_tpGroup.StrataList, 185);
                for (int i = 0; i < totalPage; i++)
                {
                    PdfSharp.Pdf.PdfPage page = document.AddPage();
                    _startY = _contentRect.Top;
                    PdfSharp.Drawing.XGraphics graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    graphics.ScaleTransform(_scale);
                    MakePage(graphics, i + 1, totalPage);
                    graphics.Dispose();
                    if (i == 0)
                        AddBookmark(_tpGroup.GroupSheetData.ExploratoryHoleNo, page);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void MakePage(XGraphics graphics, int currentPage, int totalPage)
        {
            Reset();
            DrawHeader(graphics, 5, currentPage, totalPage, Log_Recorder.DA.Class.SheetType.TP, _tpGroup.GroupSheetData);
            DrawLogo(graphics);
            DrawPitDimension(graphics, 95);
            //DrawWaterLevels(graphics, 105, _tpGroup.WaterLevelList);
            DrawRemarks(graphics, 105, _tpGroup.Remarks);
            DrawTable(graphics, 155, 570, currentPage);
            DrawFooter(graphics, 765);
        }

        private void DrawPitDimension(XGraphics graphics, double startY)
        {
            graphics.DrawRectangle(_blackPen, 5, startY, 585, 10);
            DrawTextField(graphics, 5, startY, 115, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Pit Dimension:");

            DrawTextField(graphics, 120, startY, 60, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Length:");
            if (_tpGroup.PitLength.HasValue)
                DrawTextField(graphics, 180, startY, 60, 10, _normalFont, null, TextFieldAlign.Left, _tpGroup.PitLength.Value.ToString("N2"));

            DrawTextField(graphics, 270, startY, 60, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Width:");
            if (_tpGroup.PitWidth.HasValue)
                DrawTextField(graphics, 330, startY, 60, 10, _normalFont, null, TextFieldAlign.Left, _tpGroup.PitWidth.Value.ToString("N2"));

            DrawTextField(graphics, 420, startY, 60, 10, _normalFont, _blackPen, _greenBrush, TextFieldAlign.Left, "Depth:");
            if (_tpGroup.PitDepth.HasValue)
                DrawTextField(graphics, 480, startY, 60, 10, _normalFont, null, TextFieldAlign.Left, _tpGroup.PitDepth.Value.ToString("N2"));
        }

        private void DrawTable(PdfSharp.Drawing.XGraphics graphics, double startY, double height, int currentPage)
        {
            DrawSampleOrTest(graphics, startY, currentPage);
            DrawStrataList(graphics, 255, startY, 315, height, currentPage, _tpGroup.WaterLevelList);
        }
        
        private void DrawSampleOrTest(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage)
        {
            DrawSampleOrTestHeader(graphics, startY, currentPage);
            startY += 40;

            foreach (var sampleOrTest in _tpGroup.SampleOrTestList)
                DrawSampleOrTestLine(graphics, startY + _baseDepth, sampleOrTest, currentPage);
        }

        private new void DrawSampleOrTestHeader(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage)
        {
            DrawTextField(graphics, 5, startY, 250, 10, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Sample or Tests");

            startY += 10;
            DrawTextField(graphics, 5, startY, 50, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Type");
            DrawTextField(graphics, 55, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Depth", "(mbgl)");
            DrawTextField(graphics, 95, startY, 160, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Result");

            startY += 30;
            graphics.DrawRectangles(_blackPen, new XRect[]{
                new XRect(5, startY, 50, 570),
                new XRect(55, startY, 40, 570)
            });
        }

        private new void DrawSampleOrTestLine(XGraphics graphics, double startY, Log_Recorder.DA.Model.TP.SampleOrTest sampleOrTest, int currentPage)
        {
            double offset = currentPage - 1;

            if ((sampleOrTest.Depth >= offset * _maxDepthPerPage) && (sampleOrTest.Depth < ((offset + 1) * _maxDepthPerPage)))
            {
                if (sampleOrTest.Depth != _prevSampleDepth)
                    _sampleOffset = 0;
                double currentDepth = sampleOrTest.Depth - offset * _maxDepthPerPage;
                double currentY = startY + sampleOrTest.Depth * 100 + _sampleOffset;
                DrawTextField(graphics, 5, currentY, 50, _normalFont, _blackBrush, TextFieldAlign.Center, sampleOrTest.Type);
                if (_sampleOffset == 0)
                    DrawTextField(graphics, 55, currentY, 40, _normalFont, _blackBrush, TextFieldAlign.Center, sampleOrTest.Depth.ToString("N2"));
                DrawTextField(graphics, 95, currentY, 160, _normalFont, _blackBrush, TextFieldAlign.Center, sampleOrTest.Result);
                _sampleOffset += 10;
                if (String.IsNullOrEmpty(sampleOrTest.Comment) == false)
                {
                    graphics.DrawRectangle(_markerBrush, 5, currentY + 5, 250, 10);
                    DrawTextField(graphics, 5, currentY + 10, 250, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Comment));
                    _sampleOffset += 10;
                }
                _prevSampleDepth = sampleOrTest.Depth;
            }
        }
    }
}
