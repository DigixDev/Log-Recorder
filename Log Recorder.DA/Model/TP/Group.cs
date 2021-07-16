using Log_Recorder.DA.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model.TP
{
    public class Group : IDisposable, IClearGroup, IChangeable
    {
        public SheetData GroupSheetData { get; set; }
        public double? PitLength { get; set; }
        public double? PitWidth { get; set; }
        public double? PitDepth { get; set; }
        public ObservableCollection<Strata> StrataList { get; set; }
        public ObservableCollection<SampleOrTest> SampleOrTestList { get; set; }
        public ObservableCollection<WaterLevel> WaterLevelList { get; set; }
        public ObservableCollection<string> Remarks { get; set; }

        public Group()
        {
            StrataList = new ObservableCollection<Strata>();
            SampleOrTestList = new ObservableCollection<SampleOrTest>();
            WaterLevelList = new ObservableCollection<WaterLevel>();
            Remarks = new ObservableCollection<string>();
            GroupSheetData = new SheetData();
        }

        public void Dispose()
        {
            Clear();

            StrataList = null;
            SampleOrTestList = null;
            WaterLevelList = null;
            Remarks = null;
        }

        public void Clear()
        {
            StrataList.Clear();
            SampleOrTestList.Clear();
            WaterLevelList.Clear();
            Remarks.Clear();
        }

        public ObservableCollection<Strata> GetStrataList()
        {
            return StrataList;
        }

        public ObservableCollection<Model.SampleOrTest> GetSampleOrTestList()
        {
            return null;
        }

        public ObservableCollection<WaterLevel> GetWaterLevelList()
        {
            return null;
        }

        public ObservableCollection<Installation> GetInstallationList()
        {
            return null;
        }

        public ObservableCollection<string> GetRemarks()
        {
            return Remarks;
        }

        public void CopyAllLists(ObservableCollection<Strata> strataList, ObservableCollection<Model.SampleOrTest> sampleOrTestList, ObservableCollection<WaterLevel> waterLevelList, ObservableCollection<Installation> installationList, ObservableCollection<string> remarkList, SheetData sheetData)
        {
            foreach (var a in strataList)
                StrataList.Add(a);
            foreach (var e in remarkList)
                Remarks.Add(e);
            this.GroupSheetData.Copy(sheetData);
        }

        public string GetGroupName()
        {
            return GroupSheetData.ExploratoryHoleNo;
        }

        public SheetData GetGroupSheetData()
        {
            return GroupSheetData;
        }
    }
}
