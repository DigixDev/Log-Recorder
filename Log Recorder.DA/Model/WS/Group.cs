using Log_Recorder.DA.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model.WS
{
    public class Group : IDisposable, IClearGroup, IChangeable
    {
        public SheetData GroupSheetData { get; set; }
        public ObservableCollection<Strata> StrataList { get; set; }
        public ObservableCollection<SampleOrTest> SampleOrTestList { get; set; }
        public ObservableCollection<WaterLevel> WaterLevelList { get; set; }
        public ObservableCollection<Installation> InstallationList { get; set; }
        public ObservableCollection<string> Remarks { get; set; }

        public Group()
        {
            StrataList = new ObservableCollection<Strata>();
            SampleOrTestList = new ObservableCollection<SampleOrTest>();
            WaterLevelList = new ObservableCollection<WaterLevel>();
            Remarks = new ObservableCollection<string>();
            InstallationList = new ObservableCollection<Installation>();
            GroupSheetData = new SheetData();
        }

        public void Dispose()
        {
            Clear();

            StrataList = null;
            SampleOrTestList = null;
            WaterLevelList = null;
            Remarks = null;
            InstallationList = null;
        }

        public void Clear()
        {
            StrataList.Clear();
            SampleOrTestList.Clear();
            WaterLevelList.Clear();
            Remarks.Clear();
            InstallationList.Clear();
        }

        public ObservableCollection<Strata> GetStrataList()
        {
            return StrataList;
        }

        public ObservableCollection<SampleOrTest> GetSampleOrTestList()
        {
            return SampleOrTestList;
        }

        public ObservableCollection<WaterLevel> GetWaterLevelList()
        {
            return WaterLevelList;
        }

        public ObservableCollection<string> GetRemarks()
        {
            return Remarks;
        }

        public void CopyAllLists(ObservableCollection<Strata> strataList, ObservableCollection<SampleOrTest> sampleOrTestList, ObservableCollection<WaterLevel> waterLevelList, ObservableCollection<Model.Installation> installationList, ObservableCollection<string> remarkList, SheetData sheetData)
        {
            foreach (var a in strataList)
                StrataList.Add(a);
            if (sampleOrTestList != null)
            {
                foreach (var b in sampleOrTestList)
                    SampleOrTestList.Add(b);
                foreach (var c in waterLevelList)
                    WaterLevelList.Add(c);
                foreach (var d in installationList)
                    InstallationList.Add(d);
            }
            foreach (var e in remarkList)
                Remarks.Add(e);
            this.GroupSheetData.Copy(sheetData);
        }

        public string GetGroupName()
        {
            return GroupSheetData.ExploratoryHoleNo;
        }

        public ObservableCollection<Installation> GetInstallationList()
        {
            return InstallationList;
        }



        public SheetData GetGroupSheetData()
        {
            return GroupSheetData;
        }
    }
}
