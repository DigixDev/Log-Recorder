using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Class
{
    public interface IChangeable
    {
        ObservableCollection<Model.Strata> GetStrataList();
        ObservableCollection<Model.SampleOrTest> GetSampleOrTestList();
        ObservableCollection<Model.WaterLevel> GetWaterLevelList();
        ObservableCollection<Model.Installation> GetInstallationList();
        ObservableCollection<string> GetRemarks();
        SheetData GetGroupSheetData();
        void CopyAllLists(ObservableCollection<Model.Strata> strataList, ObservableCollection<Model.SampleOrTest> sampleOrTestList, ObservableCollection<Model.WaterLevel> waterLevelList, ObservableCollection<Model.Installation> installationList, ObservableCollection<string> remarkList, SheetData sheetData);
        string GetGroupName();
    }
}
