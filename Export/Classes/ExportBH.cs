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
    public class ExportBH : ExportBase
    {
        private Log_Recorder.DA.Model.BH.Group _bhGroup;
        public ExportBH()
            : base()
        {
         }

        public void MakePDFReport(Log_Recorder.DA.Model.BH.Group group, PdfSharp.Pdf.PdfDocument document)
        {
            double _startY;
            double maxDepth;
            int totalPage;
            _bhGroup = group;
            try
            {
                maxDepth = GetMax(_bhGroup.StrataList);
                totalPage = (int)Math.Ceiling(maxDepth / _maxDepthPerPage);
                InitStrataList(_bhGroup.StrataList, 165);
                InitInstallationList(_bhGroup.InstallationList);
                for (int i = 0; i < totalPage; i++)
                {
                    PdfSharp.Pdf.PdfPage page = document.AddPage();
                    _startY = _contentRect.Top;
                    PdfSharp.Drawing.XGraphics graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    graphics.ScaleTransform(_scale);
                    MakePage(graphics, i + 1, totalPage);
                    graphics.Dispose();
                    if (i == 0)
                        AddBookmark(_bhGroup.GroupSheetData.ExploratoryHoleNo, page);
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
            DrawHeader(graphics, 5, currentPage, totalPage, Log_Recorder.DA.Class.SheetType.BH, _bhGroup.GroupSheetData);
            DrawLogo(graphics);
            DrawWaterLevels(graphics, 95, _bhGroup.WaterLevelList);
            DrawRemarks(graphics, 155, _bhGroup.Remarks);
            DrawTable(graphics, 205, 520, currentPage);
            DrawFooter(graphics, 765);
        }

        private void DrawTable(PdfSharp.Drawing.XGraphics graphics, double startY, double height, int currentPage)
        {
            DrawSampleOrTest(graphics, startY, currentPage, _bhGroup.SampleOrTestList);
            DrawStrataList(graphics, 235, startY, 295, height, currentPage, _bhGroup.WaterLevelList);
            DrawInstallation(graphics, 550, startY, currentPage, _bhGroup.InstallationList);
        }
        
        //private void DrawSampleOrTest(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage)
        //{
        //    DrawSampleOrTestHeader(graphics, startY, currentPage);
        //    startY += 40;

        //    foreach (var sampleOrTest in _bhGroup.SampleOrTestList)
        //        DrawSampleOrTestLine(graphics, startY + _baseDepth, sampleOrTest, currentPage);
        //}

        //private void DrawSampleOrTestHeader(PdfSharp.Drawing.XGraphics graphics, double startY, int currentPage)
        //{
        //    DrawTextField(graphics, 5, startY, 230, 10, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Sample or Tests");

        //    startY += 10;
        //    DrawTextField(graphics, 5, startY, 50, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Type");
        //    DrawTextField(graphics, 55, startY, 40, 30, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Depth", "(mbgl)");
        //    DrawTextField(graphics, 95, startY, 140, 20, _boldFont, _blackPen, _grayBrush, TextFieldAlign.Center, "Result");

        //    startY += 20;
        //    DrawTextField(graphics, 95, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 115, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 135, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 155, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 175, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 195, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "75");
        //    DrawTextField(graphics, 215, startY, 20, 10, _normalFont, _blackPen, _grayBrush, TextFieldAlign.Center, "N");

        //    startY += 10;
        //    graphics.DrawRectangles(_blackPen, new XRect[]{
        //        new XRect(5, startY, 50, 520)
        //        ,new XRect(95, startY, 20, 520)
        //        ,new XRect(135, startY, 20, 520)
        //        ,new XRect(175, startY, 20, 520)
        //        ,new XRect(215, startY, 20, 520)
        //    });
        //}

        //private void DrawSampleOrTestLine(XGraphics graphics, double startY, Log_Recorder.DA.Model.SampleOrTest sampleOrTest, int currentPage)
        //{
        //    double offset = currentPage - 1;

        //    if ((sampleOrTest.Depth >= offset * _maxDepthPerPage) && (sampleOrTest.Depth <= ((offset + 1) * _maxDepthPerPage)))
        //    {
        //        double currentY = (sampleOrTest.Depth - offset * _maxDepthPerPage) * 100 + startY;
        //        DrawTextField(graphics, 5, currentY, 50, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Type));
        //        DrawTextField(graphics, 50, currentY, 40, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Depth));
        //        DrawTextField(graphics, 90, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R1));
        //        DrawTextField(graphics, 110, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R2));
        //        DrawTextField(graphics, 130, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R3));
        //        DrawTextField(graphics, 150, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R4));
        //        DrawTextField(graphics, 170, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R5));
        //        DrawTextField(graphics, 190, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.R6));
        //        DrawTextField(graphics, 210, currentY, 20, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.RN));
        //        if(String.IsNullOrEmpty(sampleOrTest.Comment)!=false)
        //        {
        //            graphics.DrawRectangle(XBrushes.White, 5, currentY + 10, 235, 10);
        //            DrawTextField(graphics, 5, currentY + 10, 235, _normalFont, _blackBrush, TextFieldAlign.Center, ValueToString(sampleOrTest.Type));
        //        }
        //    }
        //}
    }
}
