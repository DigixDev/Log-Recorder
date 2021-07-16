using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Export.Classes
{
    public static class Exporter
    {
        public static void Export(Log_Recorder.DA.Model.Project project, string fileName, bool showResult)
        {
            try
            {
                GlobalData.Project = project;
                PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                foreach (var group in project.WSGroupList)
                {
                    ExportWS ws = new ExportWS();
                    ws.MakePDFReport(group, doc);
                    ws = null;
                }
                foreach (var group in project.BHGroupList)
                {
                    ExportBH bh = new ExportBH();
                    bh.MakePDFReport(group, doc);
                    bh = null;
                }
                foreach (var group in project.RBHGroupList)
                {
                    ExportRBH rbh = new ExportRBH();
                    rbh.MakePDFReport(group, doc);
                    rbh = null;
                }
                foreach (var group in project.TPGroupList)
                {
                    ExportTP tp = new ExportTP();
                    tp.MakePDFReport(group, doc);
                    tp = null;
                }
                foreach (var group in project.DPGroupList)
                {
                    ExportDP dp = new ExportDP();
                    dp.MakePDFReport(group, doc, 0);
                    dp = null;
                }

                doc.Save(fileName);
                doc.Close();
                doc = null;
                if (showResult == true)
                    Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static void ExportSingle(Log_Recorder.DA.Model.Project project, string fileName, string holeNo, bool showResult)
        {
            bool done = false;
            try
            {
                GlobalData.Project = project;
                PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();

                foreach (var group in project.WSGroupList)
                {
                    if (group.GroupSheetData.ExploratoryHoleNo == holeNo)
                    {
                        ExportWS ws = new ExportWS();
                        ws.MakePDFReport(group, doc);
                        ws = null;
                        done = true;
                        break;
                    }
                }
                foreach (var group in project.BHGroupList)
                {
                    if (group.GroupSheetData.ExploratoryHoleNo == holeNo && done==false)
                    {
                        ExportBH bh = new ExportBH();
                        bh.MakePDFReport(group, doc);
                        bh = null;
                        done = true;
                        break;
                    }
                }
                foreach (var group in project.RBHGroupList)
                {
                    if (group.GroupSheetData.ExploratoryHoleNo == holeNo && done == false)
                    {
                        ExportRBH rbh = new ExportRBH();
                        rbh.MakePDFReport(group, doc);
                        rbh = null;
                        done = true;
                        break;
                    }
                }
                foreach (var group in project.TPGroupList)
                {
                    if (group.GroupSheetData.ExploratoryHoleNo == holeNo && done == false)
                    {
                        ExportTP tp = new ExportTP();
                        tp.MakePDFReport(group, doc);
                        tp = null;
                        done = true;
                        break;
                    }
                }
                foreach (var group in project.DPGroupList)
                {
                    if (group.GroupSheetData.ExploratoryHoleNo == holeNo && done == false)
                    {
                        ExportDP dp = new ExportDP();
                        dp.MakePDFReport(group, doc, 0);
                        dp = null;
                        done = true;
                        break;
                    }
                }

                doc.Save(fileName);
                doc.Close();
                doc = null;
                if (showResult == true)
                    Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
