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
    public class ExportRBH : ExportBase
    {
        private Log_Recorder.DA.Model.RBH.Group _rbhGroup;
        public ExportRBH()
            : base()
        {
         }

        public void MakePDFReport(Log_Recorder.DA.Model.RBH.Group group, PdfSharp.Pdf.PdfDocument document)
        {
            double _startY;
            double maxDepth;
            int totalPage;
            _rbhGroup = group;
            try
            {
                maxDepth = GetMax(_rbhGroup.StrataList);
                totalPage = (int)Math.Ceiling(maxDepth / _maxDepthPerPage);
                InitStrataList(_rbhGroup.StrataList, 165);
                InitInstallationList(_rbhGroup.InstallationList);
               
                for (int i = 0; i < totalPage; i++)
                {
                    PdfSharp.Pdf.PdfPage page = document.AddPage();
                    _startY = _contentRect.Top;
                    PdfSharp.Drawing.XGraphics graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    graphics.ScaleTransform(_scale);
                    MakePage(graphics, i + 1, totalPage);
                    graphics.Dispose();
                    if (i == 0)
                        AddBookmark(_rbhGroup.GroupSheetData.ExploratoryHoleNo, page);
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
            DrawHeader(graphics, 5, currentPage, totalPage, Log_Recorder.DA.Class.SheetType.RBH, _rbhGroup.GroupSheetData);
            DrawLogo(graphics);
            DrawWaterLevels(graphics, 95, _rbhGroup.WaterLevelList);
            DrawRemarks(graphics, 155, _rbhGroup.Remarks);
            DrawTable(graphics, 205, 520, currentPage);
            DrawFooter(graphics, 765);
        }

        private void DrawTable(PdfSharp.Drawing.XGraphics graphics, double startY, double height, int currentPage)
        {
            DrawSampleOrTest(graphics, startY, currentPage, _rbhGroup.SampleOrTestList);
            DrawStrataList(graphics, 235, startY, 295, height, currentPage, _rbhGroup.WaterLevelList);
            DrawInstallation(graphics, 550, startY, currentPage, _rbhGroup.InstallationList);
        }
        
    }
}
