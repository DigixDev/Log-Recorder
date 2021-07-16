using Log_Recorder.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.IO;

namespace Log_Recorder.ModelView
{
    public class ProjectModelView : ModelViewBase
    {
        #region properties
            public DA.Model.Project ActiveProject { get; set; }
            public List<DA.Model.Pattern> StrataPatterns { get; set; }
            public int SelectedCommentIndex  { get; set; }
        #endregion

        #region constructor
            public ProjectModelView()
            {
                ActiveProject = new DA.Model.Project();
                StrataPatterns = new List<DA.Model.Pattern>(DA.Class.PatternRepository.GetStrataList());
            }
        #endregion

        internal bool OpenProject(string fileName)
            {
                try
                {
                    DA.Class.Global.FileName = fileName;
                    System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
                    string[] content=reader.ReadToEnd().Split(new string[]{"#@"}, StringSplitOptions.None);
                    
                    XmlSerializer serializerWS = new XmlSerializer(typeof(DA.Model.Project));
                    XmlSerializer serializerBH = new XmlSerializer(typeof(ObservableCollection<DA.Model.BH.Group>));
                    XmlSerializer serializerRBH = new XmlSerializer(typeof(ObservableCollection<DA.Model.RBH.Group>));
                    XmlSerializer serializerTP = new XmlSerializer(typeof(ObservableCollection<DA.Model.TP.Group>));
                    XmlSerializer serializerDP = new XmlSerializer(typeof(ObservableCollection<DA.Model.DP.Group>));

                    ActiveProject = serializerWS.Deserialize(new StringReader(content[0])) as DA.Model.Project;
                    ActiveProject.BHGroupList = serializerBH.Deserialize(new StringReader(content[1])) as ObservableCollection<DA.Model.BH.Group>;
                    ActiveProject.RBHGroupList = serializerRBH.Deserialize(new StringReader(content[2])) as ObservableCollection<DA.Model.RBH.Group>;
                    ActiveProject.TPGroupList = serializerTP.Deserialize(new StringReader(content[3])) as ObservableCollection<DA.Model.TP.Group>;
                    ActiveProject.DPGroupList = serializerDP.Deserialize(new StringReader(content[4])) as ObservableCollection<DA.Model.DP.Group>;

                    reader.Close();
                    OnPropertyChanged("ActiveProject");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }

        internal bool SaveProject(string fileName)
        {
            try
            {
                DA.Class.Global.FileName = fileName;
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName);
                XmlSerializer serializerWS = new XmlSerializer(typeof(Log_Recorder.DA.Model.Project));
                XmlSerializer serializerBH = new XmlSerializer(typeof(ObservableCollection<DA.Model.BH.Group>));
                XmlSerializer serializerRBH = new XmlSerializer(typeof(ObservableCollection<DA.Model.RBH.Group>));
                XmlSerializer serializerTP = new XmlSerializer(typeof(ObservableCollection<DA.Model.TP.Group>));
                XmlSerializer serializerDP = new XmlSerializer(typeof(ObservableCollection<DA.Model.DP.Group>));

                serializerWS.Serialize(writer, ActiveProject);
                writer.Write("#@");
                serializerBH.Serialize(writer, ActiveProject.BHGroupList);
                writer.Write("#@");
                serializerRBH.Serialize(writer, ActiveProject.RBHGroupList);
                writer.Write("#@");
                serializerTP.Serialize(writer, ActiveProject.TPGroupList);
                writer.Write("#@");
                serializerDP.Serialize(writer, ActiveProject.DPGroupList);

                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        internal void SaveAsPDF(string fileName, string holeNo="")
        {
            if(holeNo==String.Empty)
                Export.Classes.Exporter.Export(ActiveProject, fileName, true);
            else
                Export.Classes.Exporter.ExportSingle(ActiveProject, fileName, holeNo, true);
        }

        internal bool HasProject()
        {
            if (ActiveProject.ProjectNumber == String.Empty)
                return false;
            return true;
        }

        internal void NewProject()
        {
            ActiveProject.MakeEmpty();
            DA.Class.Global.FileName = String.Empty;
        }

        internal DA.Model.WS.Group AddNewSheetWS(string name)
        {
            DA.Model.WS.Group ws = new DA.Model.WS.Group();
            ws.GroupSheetData.ExploratoryHoleNo = name;
            ActiveProject.WSGroupList.Add(ws);
            return ws;
        }

        internal DA.Model.BH.Group AddNewSheetBH(string name)
        {
            DA.Model.BH.Group bh = new DA.Model.BH.Group();
            bh.GroupSheetData.ExploratoryHoleNo = name;
            ActiveProject.BHGroupList.Add(bh);
            return bh;
        }

        internal DA.Model.RBH.Group AddNewSheetRBH(string name)
        {
            DA.Model.RBH.Group rbh = new DA.Model.RBH.Group();
            rbh.GroupSheetData.ExploratoryHoleNo = name;
            ActiveProject.RBHGroupList.Add(rbh);
            return rbh;
        }

        internal DA.Model.TP.Group AddNewSheetTP(string name)
        {
            DA.Model.TP.Group tp = new DA.Model.TP.Group();
            tp.GroupSheetData.ExploratoryHoleNo = name;
            ActiveProject.TPGroupList.Add(tp);
            return tp;
        }

        internal DA.Model.DP.Group AddNewSheetDP(string name)
        {
            DA.Model.DP.Group dp = new DA.Model.DP.Group();
            dp.GroupSheetData.ExploratoryHoleNo = name;
            ActiveProject.DPGroupList.Add(dp);
            return dp;
        }
    }
}
